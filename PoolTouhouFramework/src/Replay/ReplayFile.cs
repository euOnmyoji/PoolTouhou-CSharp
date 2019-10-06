using System.IO;

namespace PoolTouhouFramework.Replay {
    public class ReplayFile {
        public static readonly byte[] REPLAY_HEADER = {0x49, 0x6c, 0x6f, 0x76, 0x65, 0x78, 0x73, 0x66};
        public readonly string replyName;
        public readonly int mainVersion;
        public readonly int smallVersion;
        public readonly int patchVersion;

        public static bool isReplayFile(FileStream fileStream) {
            var temp = new byte[8];
            int len = fileStream.Read(temp, 0, 0);
            return len == 8 && temp == REPLAY_HEADER;
        }
    }
}