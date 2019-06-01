using System;
using System.Collections.Generic;
using System.IO;
using SharpDX.Multimedia;
using SharpDX.XAudio2;

namespace PoolTouhou.Sound {
    public sealed class SoundManager : IDisposable {
        private readonly XAudio2 xAudio2;
        private readonly MasteringVoice masteringVoice;
        private readonly Dictionary<string, SoundData> voices = new Dictionary<string, SoundData>();

        internal SoundManager() {
            xAudio2 = new XAudio2();
            masteringVoice = new MasteringVoice(xAudio2);
        }

        public void Dispose() {
            xAudio2?.Dispose();
            masteringVoice?.Dispose();
        }

        public void Load(string name, string path) {
            using var fs = new FileStream(path, FileMode.Open);
            var soundStream = new SoundStream(fs);
            var voice = new SourceVoice(xAudio2, new WaveFormat(), VoiceFlags.None, false);
            var buffer = new AudioBuffer(soundStream);
            var submixVoice = new SubmixVoice(xAudio2);
            var descriptor = new VoiceSendDescriptor(submixVoice);
            voice.SetOutputVoices(descriptor);
            voice.SubmitSourceBuffer(buffer, soundStream.DecodedPacketsInfo);
            voice.SetVolume(0.125f);
            if (voices.ContainsKey(name)) {
                throw new Exception("重复加载一个音频！ name:" + name);
            }
            voices.Add(
                name,
                new SoundData {
                    buffer = buffer, voice = voice, decodedPacketsInfo = soundStream.DecodedPacketsInfo,
                    descriptor = descriptor
                }
            );
        }

        public bool TryLoad(string name, string path) {
            if (voices.ContainsKey(name)) {
                return false;
            }
            using var fs = new FileStream(path, FileMode.Open);
            var soundStream = new SoundStream(fs);
            var voice = new SourceVoice(xAudio2, new WaveFormat(), VoiceFlags.None, false);
            var buffer = new AudioBuffer(soundStream);
            var submixVoice = new SubmixVoice(xAudio2);
            var descriptor = new VoiceSendDescriptor(submixVoice);
            voice.SetOutputVoices(descriptor);
            voice.SubmitSourceBuffer(buffer, soundStream.DecodedPacketsInfo);
            voice.SetVolume(0.125f);
            voices.Add(
                name,
                new SoundData {
                    buffer = buffer, voice = voice, decodedPacketsInfo = soundStream.DecodedPacketsInfo,
                    descriptor = descriptor
                }
            );
            return true;
        }

        public void Unload(string name, int set = 0) {
            var voice = voices[name];
            voices.Remove(name);
            voice.voice.Stop(set);
        }

        public void PlayOnce(string name) {
            voices[name].voice.Start();
        }

        public void Loop(string name, int count = XAudio2.MaximumLoopCount) {
            var data = voices[name];
            data.buffer.LoopCount = count;
            data.voice.FlushSourceBuffers();
            data.voice.SubmitSourceBuffer(data.buffer, data.decodedPacketsInfo);
            data.voice.Start();
            data.looping = true;
        }

        public void TryLoop(string name) {
            var data = voices[name];
            if (!data.looping) {
                data.buffer.LoopCount = XAudio2.MaximumLoopCount;
                data.voice.FlushSourceBuffers();
                data.voice.SubmitSourceBuffer(data.buffer, data.decodedPacketsInfo);
                data.voice.Start();
                data.looping = true;
            }
        }

        private struct SoundData : IDisposable {
            public AudioBuffer buffer;
            public SourceVoice voice;
            public VoiceSendDescriptor descriptor;
            public uint[] decodedPacketsInfo;
            public bool looping;

            public void Dispose() {
                voice?.Dispose();
            }
        }
    }
}