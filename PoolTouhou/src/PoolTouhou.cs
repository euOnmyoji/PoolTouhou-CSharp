﻿using System;
using System.Diagnostics;
using System.Windows.Forms;
using PoolTouhou.Device;
using PoolTouhou.GameStates;
using PoolTouhou.Sound;
using PoolTouhou.UI.Buttons;
using PoolTouhou.Utils;

namespace PoolTouhou {
    public static class PoolTouhou {
        public static MainForm MainForm { get; private set; }
        public static Logger Logger { get; private set; }

        public static GameState GameState {
            get => gameState;
            set {
                var old = gameState;
                gameState = value;
                old?.Dispose();
            }
        }

        public static DirectXResource DxResource { get; private set; }

        public static ushort Tps {
            get => tps;
            set {
                tps = value;
                OneTickCount = (double) Stopwatch.Frequency / value;
            }
        }


        public static Stopwatch Watch { get; } = new Stopwatch();
        public static volatile bool running = true;
        public static SoundManager SoundManager { get; private set; }


        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        public static int Main(string[] args) {
            Watch.Start();
            try {
                Logger = new Logger();
                Init();
            } catch (Exception e) {
                Dispose();
                Logger.Info(e.Message + Environment.NewLine + e.StackTrace);
                MessageBox.Show(e.Message + Environment.NewLine + e.StackTrace, @"很抱歉出错了！");
                Application.Exit();
                return -1;
            }
            GC.Collect();
            Application.Run(MainForm);
            Dispose();
            Logger.Info("回收资源 退出主线程");
            return 0;
        }

        private static ushort tps = 100;
        public static double OneTickCount { get; private set; } = (double) Stopwatch.Frequency / tps;

        private static void Init() {
            Logger.Info("开始实例化游戏窗口类");
            MainForm = new MainForm();
            Logger.Info("初始化图形资源");
            DxResource = new DirectXResource();
            Logger.Info("初始化音频资源");
            SoundManager = new SoundManager();
            Logger.Info("初始化窗口类 & 游戏");
            MainForm.Init();
        }

        private static void Dispose() {
            MainForm?.Dispose();
            SoundManager?.Dispose();
            DxResource?.Dispose();
            gameState?.Dispose();
            ButtonsResources.INSTANCE?.Dispose();
        }

        private static GameState gameState;
    }
}