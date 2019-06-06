using System;
using System.IO;

namespace PoolTouhou.Utils {
    public static class Logger {

        static Logger() {
            File.Delete("log.log");
        }

        public static async void Info(string msg) {
            using var writer = new FileStream("log.log", FileMode.Append);
            using var stream = new StreamWriter(writer);
            await stream.WriteLineAsync($"{DateTime.Now} {msg}");
            stream.Flush();
        }

        public static async void MemoryLack(string msg) {
            using var writer = new FileStream("log.log", FileMode.Append);
            using var stream = new StreamWriter(writer);
            await stream.WriteLineAsync($"{DateTime.Now} {msg}");
            stream.Flush();
        }

        public static async void LogException(Exception e) {
            using var writer = new FileStream("log.log", FileMode.Append);
            using var stream = new StreamWriter(writer);
            await stream.WriteLineAsync($"{DateTime.Now} {e.Message + Environment.NewLine + e.StackTrace}");
            stream.Flush();
        }
    }
}