using System.Drawing;
using static ColorDict.Core.Mouse.MosueHook;

namespace ColorDict.Core.Interfaces
{
    public interface IMouseInfoProvider
    {
        event EventHandler<Color> MouseColorChanged;
        event EventHandler<System.Windows.Point> MousePositionChanged;
        event EventHandler<Tuple<System.Windows.Point, bool>> OnMouseWheel;
        event MouseUpEventHandler OnLeftMouseDown;
        event MouseUpEventHandler OnLeftMouseUp;
        event MouseUpEventHandler OnRightMouseDown;

        System.Windows.Point CurrentPosition { get; }

        void StopMonitoring();
        void SetOriginalCursor();
    }
}
