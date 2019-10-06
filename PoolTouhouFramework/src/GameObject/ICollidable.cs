using System.Collections.Generic;

namespace PoolTouhouFramework.GameObject {
    public enum CollideableBoxType {
        CIRCLE, TRIANGLE, SQUARE
    }

    public interface ICollidable {
        bool IsCollided(ICollidable that);

        ICollection<IBoundingBox> BoundingBoxes { get; }
    }

    public interface IBoundingBox {
        /// <summary>
        /// 返回具体碰撞结果 如果是负数则碰撞 越小越进去(x)
        /// </summary>
        /// <param name="that">另外一个碰撞箱</param>
        /// <returns>the distance (pixel) of the result</returns>
        int IsCollided(IBoundingBox that);
    }
}