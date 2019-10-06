using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;

namespace PoolTouhouFramework.Utils {
    public sealed class Logger {
        private readonly ConcurrentQueue<string> queue = new ConcurrentQueue<string>();

        private volatile bool running = true;

        public Logger() {
            var logStream = new FileStream("log.log", FileMode.Create, FileAccess.Write, FileShare.Read);
            var writer = new StreamWriter(logStream, Encoding.UTF8, 512) {AutoFlush = true};
            var thread = new Thread(
                () => {
                    try {
                        while (running) {
                            while (queue.TryDequeue(out string s)) {
                                Console.WriteLine(s);
                                writer.WriteLine(s);
                            }
                            lock (this) {
                                if (queue.IsEmpty) {
                                    Monitor.Wait(this, 5 * 1000);
                                }
                            }
                        }
                    } catch (Exception e) {
//                        MessageBox.Show(e.Message + Environment.NewLine + e.StackTrace, @"很抱歉出错了！");
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

        public void StopLog() {
            running = false;
            lock (this) {
                Monitor.Pulse(this);
            }
        }
    }
}