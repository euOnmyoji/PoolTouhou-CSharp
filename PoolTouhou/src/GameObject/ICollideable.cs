using System.Drawing;

namespace PoolTouhou.GameObject {
    public enum CollideableBoxType {
        CIRCLE, TRIANGLE, SQUARE
    }

    public interface ICollideable {
        /// <summary>
        /// 返回具体碰撞结果 如果是负数则碰撞 越小越进去(x)
        /// </summary>
        /// <param name="that"></param>
        /// <returns></returns>
        int IsCollided(ICollideable that);

        PointF GetCenter();

        int GetA();
        int GetB();
        double GetR();
        double GetP();
    }
}