using System;
using System.IO;

namespace PoolTouhou.Sound {
//    public sealed class SoundManager : IDisposable {
//        internal readonly XAudio2 xAudio2;
//        private readonly MasteringVoice masteringVoice;
//        private Dictionary<string, SoundData> voices = new Dictionary<string, SoundData>();
//
//        public delegate SoundData GetSoundData(FileStream fs);
//
//        internal SoundManager() {
//            xAudio2 = new XAudio2();
//            masteringVoice = new MasteringVoice(xAudio2);
//        }
//
//        public void Dispose() {
//            var voices = this.voices;
//            this.voices = null;
//            foreach (var data in voices.Values) {
//                data.Dispose();
//            }
//            masteringVoice?.Dispose();
//            xAudio2?.Dispose();
//        }
//
//        public void Load(string name, string path, GetSoundData toSoundData = null) {
//            if (toSoundData == null) {
//                toSoundData = GetSoundStreamMethods.GetWavSoundStream;
//            }
//            if (!File.Exists(path)) {
//                PoolTouhou.Logger.Info($"can't load not exist sound file: {path}");
//            }
//            using var fs = File.OpenRead(path);
//            var data = toSoundData(fs);
//
//            var voice = data.voice;
//            var submixVoice = new SubmixVoice(xAudio2);
//            var descriptor = new VoiceSendDescriptor(submixVoice);
//            voice.SetOutputVoices(descriptor);
//            voice.SubmitSourceBuffer(data.buffer, data.decodedPacketsInfo);
//            voice.SetVolume(0.125f);
//            voices.Add(name, data);
//        }
//
//        public bool TryLoad(string name, string path, GetSoundData toSoundData = null) {
//            if (voices.ContainsKey(name)) {
//                return false;
//            }
//            Load(name, path, toSoundData);
//            return true;
//        }
//
//        public void Unload(string name) {
//            var voice = voices?[name];
//            if (voice != null) {
//                voices.Remove(name);
//                voice.Value.Dispose();
//            }
//        }
//
//        public void PlayOnce(string name, int set = 0) {
//            var data = voices[name];
//            data.voice.FlushSourceBuffers();
//            data.voice.SubmitSourceBuffer(data.buffer, data.decodedPacketsInfo);
//            data.voice.Start(set);
//        }
//
//        public void Overplay(string name, int set = 0) {
//            var data = voices[name];
//            var overVoice = new SourceVoice(xAudio2, data.format, false);
//            overVoice.SubmitSourceBuffer(data.buffer, data.decodedPacketsInfo);
//            overVoice.SetVolume(0.125f);
//            overVoice.BufferEnd += ptr => overVoice.DestroyVoice();
//            overVoice.Start(set);
//        }
//
//        public void Loop(string name, int count = XAudio2.MaximumLoopCount) {
//            try {
//                var data = voices[name];
//                if (!data.looping) {
//                    data.looping = true;
//                    data.buffer.LoopCount = count;
//                    data.voice.FlushSourceBuffers();
//                    data.voice.SubmitSourceBuffer(data.buffer, data.decodedPacketsInfo);
//                    data.voice.Start();
//                }
//            } catch (Exception e) {
//                PoolTouhou.Logger.LogException(e);
//            }
//        }
//    }
//
    public struct SoundData : IDisposable {
        public void Dispose() {

        }
    }

    public static class GetSoundStreamMethods {
        public static SoundData GetWavSoundStream(FileStream fs) {
//            var ss = new SoundStream(fs);
//            ss.Seek(0,SeekOrigin.Begin);
            throw new NotImplementedException();
        }

        public static SoundData GetMp3SoundStream(FileStream fs) {
            throw new NotImplementedException();

        }
    }
}