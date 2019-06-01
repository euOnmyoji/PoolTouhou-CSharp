using System;
using System.Collections.Generic;
using System.IO;

namespace PoolTouhou.Utils {
    public static class Logger {
        private static readonly Queue<string> msgQueue = new Queue<string>();

        static Logger() {
            File.Delete("log.log");
        }

        public static async void Info(string msg) {
            using var writer = new FileStream("log.log", FileMode.Append);
            using var stream = new StreamWriter(writer);
            await stream.WriteLineAsync($"{DateTime.Now} {msg}");
            stream.Flush();
        }
    }
}