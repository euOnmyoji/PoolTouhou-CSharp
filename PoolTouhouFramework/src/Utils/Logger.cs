using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;

namespace PoolTouhouFramework.Utils {
    public enum LogLevel : byte {
        INFO = 0,
        ERROR = 1
    }

    public sealed class Logger {
        private readonly ConcurrentQueue<(LogLevel, string)> queue = new ConcurrentQueue<(LogLevel, string)>();

        private volatile bool running = true;

        public Logger(string fileName = "log.log") {
            var logStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read);
            var writer = new StreamWriter(logStream, Encoding.UTF8, 512) {AutoFlush = true};
            var thread = new Thread(
                () => {
                    int waitTimes = 0;
                    try {
                        while (running || !queue.IsEmpty || waitTimes < 3) {
                            while (queue.TryDequeue(out var s)) {
                                if (s.Item1 == LogLevel.ERROR) {
                                    Console.Error.WriteLine($"[Error]{s.Item2}");
                                } else {
                                    Console.WriteLine($"[{s.Item1}]{s.Item2}");
                                }
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

        public void Log(string msg, LogLevel level = LogLevel.INFO) {
            queue.Enqueue((level, $"{DateTime.Now} {msg}"));
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