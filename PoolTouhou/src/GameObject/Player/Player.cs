using System;
using System.Collections.Generic;
using System.Drawing;
using PoolTouhou.UI;
using PoolTouhou.Utils;
using SharpDX.Direct2D1;

namespace PoolTouhou.GameObject.Player {
    public abstract class Player : ICollidable, IUpdateable, IDrawable, ILocateable, IDisposable {
        public byte life = 2;
        public byte spell = 3;
        public short dying = -1;
        public short spelling = -1;
        public abstract void Update(ref InputData input);
        public abstract void Draw(RenderTarget renderTarget);
        public abstract bool IsCollided(ICollidable that);
        public abstract ICollection<IBoundingBox> BoundingBoxes { get; }
        public abstract PointF Point { get; set; }
        public abstract void Dispose();
    }
}