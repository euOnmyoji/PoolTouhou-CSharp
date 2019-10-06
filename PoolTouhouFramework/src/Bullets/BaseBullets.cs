using System.Collections.Generic;
using System.Drawing;
using PoolTouhouFramework.GameObject;
using PoolTouhouFramework.Utils;

namespace PoolTouhouFramework.Bullets {
    public static class BaseBullets {
        public class LineBulletBase : BulletBase {
            public LineBulletBase() {
            }

            public override bool IsCollided(ICollidable that) {
                throw new System.NotImplementedException();
            }

            public override ICollection<IBoundingBox> BoundingBoxes { get; }

            public override void Draw(double deltaTime) {
                throw new System.NotImplementedException();
            }

            public override void Update(ref InputData input) {
                throw new System.NotImplementedException();
            }

            public override float X { get; set; }
            public override float Y { get; set; }
            public override float Ax { get; set; }
            public override float Ay { get; set; }
            public override float Vx { get; set; }
            public override float Vy { get; set; }

            public PointF Point { get; set; }
            public bool IsDead { get; }
        }
    }
}