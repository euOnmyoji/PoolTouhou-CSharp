using System;
using System.IO;

namespace PoolTouhou.Utils {
    public static class Logger {
        static Logger() {
            File.Delete("log.log");
        }

        public static async void Info(string msg) {
            using var stream = new FileStream("log.log", FileMode.Append, FileAccess.Write, FileShare.Read);
            using var writer = new StreamWriter(stream);
            await writer.WriteLineAsync($"{DateTime.Now} {msg}");
        }

        public static async void MemoryLack(string msg) {
            using var stream = new FileStream("log.log", FileMode.Append, FileAccess.Write, FileShare.Read);
            using var writer = new StreamWriter(stream);
            await writer.WriteLineAsync($"{DateTime.Now} {msg}");
        }

        public static async void LogException(Exception e) {
            using var stream = new FileStream("log.log", FileMode.Append, FileAccess.Write, FileShare.Read);
            using var writer = new StreamWriter(stream);
            await writer.WriteLineAsync($"{DateTime.Now} {e.Message + Environment.NewLine + e.StackTrace}");
        }
    }
}