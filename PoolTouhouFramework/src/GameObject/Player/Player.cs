using System;
using System.Collections.Generic;
using System.Drawing;
using PoolTouhouFramework.UI;
using PoolTouhouFramework.Utils;

namespace PoolTouhouFramework.GameObject.Player {
    public abstract class Player : ICollidable, IUpdateable, IDrawable, IDisposable {
        public byte life = 2;
        public byte spell = 3;
        public short dying = -1;
        public short spelling = -1;
        public bool slow = false;
        public abstract void Update(ref InputData input);
        public abstract void Draw(double deltaTime);
        public abstract bool IsCollided(ICollidable that);
        public abstract ICollection<IBoundingBox> BoundingBoxes { get; }
        public abstract PointF Point { get; set; }
        public abstract void Dispose();
    }
}