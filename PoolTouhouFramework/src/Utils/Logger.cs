using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;

namespace PoolTouhouFramework.Utils {
    public sealed class Logger {
        private readonly ConcurrentQueue<string> queue = new ConcurrentQueue<string>();

        private volatile bool running = true;

        public Logger(string fileName = "log.log") {
            var logStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read);
            var writer = new StreamWriter(logStream, Encoding.UTF8, 512) {AutoFlush = true};
            var thread = new Thread(
                () => {
                    int waitTimes = 0;
                    try {
                        while (running || !queue.IsEmpty || waitTimes < 3) {
                            while (queue.TryDequeue(out string s)) {
                                Console.WriteLine($"[PTH Logger]{s}");
                                writer.WriteLine(s);
                                waitTimes = 0;
                            }
                            lock (this) {
                                if (queue.IsEmpty) {
                                    Monitor.Wait(this, 1000);
                                    if (!running) { ++waitTimes; }
                                }
                            }
                        }
                    } catch (Exception e) {
                        Console.WriteLine(e.Message + Environment.NewLine + e.StackTrace);
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