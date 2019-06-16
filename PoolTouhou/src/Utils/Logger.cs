using System;
using System.IO;
using System.Text;

namespace PoolTouhou.Utils {
    public sealed class Logger {
        private readonly StreamWriter writer;


        public Logger() {
            var logStream = new FileStream("log.log", FileMode.Create, FileAccess.Write, FileShare.Read);
            writer = new StreamWriter(logStream, Encoding.UTF8, 512) {AutoFlush = true};
        }

        public async void Info(string msg) {
            await writer.WriteLineAsync($"{DateTime.Now} {msg}");
        }

        public async void MemoryLack(string msg) {
            await writer.WriteLineAsync($"{DateTime.Now} {msg}");
        }

        public async void LogException(Exception e) {
            await writer.WriteLineAsync($"{DateTime.Now} {e.Message + Environment.NewLine + e.StackTrace}");
        }
    }
}