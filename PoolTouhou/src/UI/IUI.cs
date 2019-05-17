using PoolTouhou.Utils;

namespace PoolTouhou.UI {
    public interface IUi : IDrawable {
        IUi Update(ref InputData input);
    }
}