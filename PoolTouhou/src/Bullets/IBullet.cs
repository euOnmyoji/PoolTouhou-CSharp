using System.Collections.Generic;
using PoolTouhou.GameObject;
using PoolTouhou.UI;
using PoolTouhou.Utils;
using SharpDX.Direct2D1;

namespace PoolTouhou.Bullets {
    public abstract class BulletBase : ICollidable, IDrawable, IUpdateable, IMovable {
        public bool dead = false;
        public abstract bool IsCollided(ICollidable that);
        public abstract ICollection<IBoundingBox> BoundingBoxes { get; }
        public abstract void Draw(DeviceContext renderTarget);
        public abstract void Update(ref InputData input);
        public abstract float X { get; set; }
        public abstract float Y { get; set; }
        public abstract float Ax { get; set; }
        public abstract float Ay { get; set; }
        public abstract float Vx { get; set; }
        public abstract float Vy { get; set; }
    }

    public enum AccelerationMode : byte {
       DEFAULT,
       LINEAR,
       QUAD
    }
}