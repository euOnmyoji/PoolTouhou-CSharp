using System;
using System.Collections.Generic;
using PoolTouhou.Manager;
using PoolTouhou.Utils;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace PoolTouhou.UI {
    public class GameChooseUi : IUi {
        private readonly ICollection<IGame> games;
        private int cur = 0;

        public GameChooseUi() {
            games = GameManager.RegisteredGames.Values;
        }

        public void Draw(RenderTarget renderTarget) {
            renderTarget.Clear(null);
            float x = 0;
            float y = 0;
            int i = 0;
            foreach (var game in games) {
                if (cur == i) {
                }
                renderTarget.DrawText(
                    game.Name,
                    PoolTouhou.MainForm.textFormat,
                    new RawRectangleF(x, y, MainForm.FONT_SIZE * game.Name.Length, y + MainForm.FONT_SIZE),
                    PoolTouhou.MainForm.brush
                );
                y += MainForm.FONT_SIZE;
                ++i;
            }
        }

        public int Update(ref InputData input) {
            if (input.Spell > 0) {
                return UiEvents.EXIT;
            }
            return UiEvents.FINE;
        }
    }
}