using System.Collections.Generic;
using PoolTouhou.Games;
using PoolTouhou.GameStates;
using PoolTouhou.Utils;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace PoolTouhou.UI {
    public class GameChooseUi : IUi {
        private readonly List<IGame> games;
        private sbyte cur = 0;

        public GameChooseUi() {
            games = new List<IGame>(GameManager.RegisteredGames.Values);
        }

        public void Draw(RenderTarget renderTarget) {
            if (games.Count > 1) {
                renderTarget.Clear(null);
                const float x = 0;
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
        }

        public int Update(ref InputData input) {
            if (games.Count == 1) {
                PoolTouhou.GameState = new GamingState(games[0]);
                PoolTouhou.SoundManager.Unload("title");
            }
            if (input.spell > 0) {
                return UiEvents.EXIT;
            }
            if (input.shoot == 1 || input.enter == 1) {
                PoolTouhou.GameState = new GamingState(games[cur]);
                PoolTouhou.SoundManager.Unload("title");
                return UiEvents.SELECTED_GAME;
            }
            return UiEvents.FINE;
        }
    }
}