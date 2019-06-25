using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PoolTouhou.Utils {
    public sealed class Logger {
        private readonly ConcurrentQueue<string> queue = new ConcurrentQueue<string>();

        private volatile bool running = true;

        private volatile uint lackCount;

        public Logger() {
            var logStream = new FileStream("log.log", FileMode.Create, FileAccess.Write, FileShare.Read);
            var writer = new StreamWriter(logStream, Encoding.UTF8, 512) {AutoFlush = true};
            var thread = new Thread(
                () => {
                    try {
                        while (running) {
                            while (queue.TryDequeue(out string s)) {
                                writer.WriteLine(s);
                            }
                            lock (this) {
                                if (queue.IsEmpty) {
                                    Monitor.Wait(this, 60 * 1000);
                                }
                            }
                        }
                    } catch (Exception e) {
                        MessageBox.Show(e.Message + Environment.NewLine + e.StackTrace, @"很抱歉出错了！");
                    }
                }
            ) {Name = "Logger IO", Priority = ThreadPriority.BelowNormal};
            thread.Start();
        }

        public void Info(string msg) {
            queue.Enqueue($"{DateTime.Now} {msg}");
            lock (this) {
                Monitor.Pulse(this);
            }
        }

        public void MemoryLack(string msg) {
            uint excepted = lackCount;
            while (excepted != lackCount++) {
                excepted = lackCount;
            }
            byte oneS = 0;
            excepted >>= 1;
            while (excepted > 0) {
                if ((excepted & 1) != 0) {
                    if (++oneS > 1) {
                        break;
                    }
                }
                excepted >>= 1;
            }
            if (oneS == 1) {
                queue.Enqueue($"{DateTime.Now} {msg} and {lackCount} objects has been lacked!");
                lock (this) {
                    Monitor.Pulse(this);
                }
            }
        }

        public void LogException(Exception e) {
            queue.Enqueue($"{DateTime.Now} {e.Message + Environment.NewLine + e.StackTrace}");
            lock (this) {
                Monitor.Pulse(this);
            }
        }

        public void StopLog() {
            running = false;
            lock (this) {
                Monitor.Pulse(this);
            }
        }
    }
}