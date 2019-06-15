using System;
using System.Collections.Generic;
using System.IO;
using NAudio.Wave;
using SharpDX;
using SharpDX.Multimedia;
using SharpDX.XAudio2;
using WaveFormat = SharpDX.Multimedia.WaveFormat;
using WaveFormatEncoding = SharpDX.Multimedia.WaveFormatEncoding;

namespace PoolTouhou.Sound {
    public sealed class SoundManager : IDisposable {
        internal readonly XAudio2 xAudio2;
        private readonly MasteringVoice masteringVoice;
        private Dictionary<string, SoundData> voices = new Dictionary<string, SoundData>();

        public delegate SoundData GetSoundData(FileStream fs);

        internal SoundManager() {
            xAudio2 = new XAudio2();
            masteringVoice = new MasteringVoice(xAudio2);
        }

        public void Dispose() {
            var voices = this.voices;
            this.voices = null;
            foreach (var data in voices.Values) {
                data.Dispose();
            }
            masteringVoice?.Dispose();
            xAudio2?.Dispose();
        }

        public void Load(string name, string path, GetSoundData toSoundData = null) {
            if (toSoundData == null) {
                toSoundData = GetSoundStreamMethods.GetWavSoundStream;
            }
            if (!File.Exists(path)) {
                PoolTouhou.Logger.Info($"can't load not exist sound file: {path}");
            }
            using var fs = File.OpenRead(path);
            var data = toSoundData(fs);

            var voice = data.voice;
            var submixVoice = new SubmixVoice(xAudio2);
            var descriptor = new VoiceSendDescriptor(submixVoice);
            voice.SetOutputVoices(descriptor);
            voice.SubmitSourceBuffer(data.buffer, data.decodedPacketsInfo);
            voice.SetVolume(0.125f);
            voices.Add(name, data);
        }

        public bool TryLoad(string name, string path, GetSoundData toSoundData = null) {
            if (voices.ContainsKey(name)) {
                return false;
            }
            Load(name, path, toSoundData);
            return true;
        }

        public void Unload(string name) {
            var voice = voices?[name];
            if (voice != null) {
                voices.Remove(name);
                voice.Value.Dispose();
            }
        }

        public void PlayOnce(string name, int set = 0) {
            var data = voices[name];
            data.voice.FlushSourceBuffers();
            data.voice.SubmitSourceBuffer(data.buffer, data.decodedPacketsInfo);
            data.voice.Start(set);
        }

        public void Overplay(string name, int set = 0) {
            var data = voices[name];
            var overVoice = new SourceVoice(xAudio2, data.format, false);
            overVoice.SubmitSourceBuffer(data.buffer, data.decodedPacketsInfo);
            overVoice.SetVolume(0.125f);
            overVoice.BufferEnd += ptr => overVoice.DestroyVoice();
            overVoice.Start(set);
        }

        public void Loop(string name, int count = XAudio2.MaximumLoopCount) {
            try {
                var data = voices[name];
                data.buffer.LoopCount = count;
                data.voice.FlushSourceBuffers();
                data.voice.SubmitSourceBuffer(data.buffer, data.decodedPacketsInfo);
                data.voice.Start();
            } catch (Exception e) {
                PoolTouhou.Logger.LogException(e);
            }
        }
    }

    public struct SoundData : IDisposable {
        public AudioBuffer buffer;
        public SourceVoice voice;
        public WaveFormat format;
        public uint[] decodedPacketsInfo;

        public void Dispose() {
            voice?.DestroyVoice();
            voice?.Dispose();
            buffer?.Stream?.Dispose();
            voice = null;
            buffer = null;
        }
    }

    public static class GetSoundStreamMethods {
        public static SoundData GetWavSoundStream(FileStream fs) {
            var ss = new SoundStream(fs);
            ss.Seek(0,SeekOrigin.Begin);
            return new SoundData {
                voice = new SourceVoice(PoolTouhou.SoundManager.xAudio2, ss.Format, false),
                buffer = new AudioBuffer(ss),
                decodedPacketsInfo = ss.DecodedPacketsInfo,
            };
        }

        public static SoundData GetMp3SoundStream(FileStream fs) {
            var mp3File = new Mp3FileReader(fs);
            var nFormat = mp3File.WaveFormat;
            var format = WaveFormat.CreateCustomFormat(
                (WaveFormatEncoding) nFormat.Encoding,
                nFormat.SampleRate,
                nFormat.Channels,
                nFormat.AverageBytesPerSecond,
                nFormat.BlockAlign,
                nFormat.BitsPerSample
            );
            var data = new DataStream((int) mp3File.Length, true, true);
            var temp = new byte[512 * 1024];
            int len;
            while ((len = mp3File.Read(temp, 0, temp.Length)) != 0) {
                data.Write(temp, 0, len);
            }
            data.Seek(0, SeekOrigin.Begin);
            return new SoundData {
                voice = new SourceVoice(PoolTouhou.SoundManager.xAudio2, format, false),
                format = format,
                buffer = new AudioBuffer(data),
            };
        }
    }
}