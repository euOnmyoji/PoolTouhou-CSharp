using System;
using System.IO;

namespace PoolTouhou.Utils {
    public static class Logger {
        static Logger() {
            File.Delete("log.log");
        }
        public static void Info(string msg) {
            using var writer = new FileStream("log.log", FileMode.Append);
            using var stream = new StreamWriter(writer);
            stream.WriteLine($"{DateTime.Now} {msg}");
        }
    }
}