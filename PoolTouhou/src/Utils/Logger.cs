using System;
using System.IO;
using System.Text;

namespace PoolTouhou.Utils {
    public sealed class Logger : IDisposable {
        private readonly FileStream logStream;
        private readonly StreamWriter writer;

        public Logger() {
            File.Delete("log.log");
            logStream = new FileStream("log.log", FileMode.Append, FileAccess.Write, FileShare.Read);
            writer = new StreamWriter(logStream, Encoding.UTF8, 32 * 1024);
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

        public void Dispose() {
            writer.Flush();
            writer?.Dispose();
            logStream?.Dispose();
        }
    }
}