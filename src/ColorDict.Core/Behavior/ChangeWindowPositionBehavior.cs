using ColorDict.Core.Helpers;
using ColorDict.Core.Interfaces;
using Microsoft.Xaml.Behaviors;
using System.Windows;

namespace ColorDict.Core.Behavior
{
    public class ChangeWindowPositionBehavior : Behavior<Window>
    {
        // color window should not get into these zones, only mouse to avoid window getting outsize of monitor
        private const int MonitorRightSideDeadZone = 200;
        private const int MonitorBottomSideDeadZone = 80;

        private const int YOffset = 10;
        private const int XOffset = 5;

        private Point _lastMousePosition;
        private IMouseInfoProvider _mouseInfoProvider;


        public ChangeWindowPositionBehavior(IMouseInfoProvider mouseInfoProvider)
        {
            _mouseInfoProvider = mouseInfoProvider;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += AssociatedObject_Loaded;
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            SetWindowPosition(_mouseInfoProvider.CurrentPosition);
            _mouseInfoProvider.MousePositionChanged += (s, mousePosition) =>
            {
                SetWindowPosition(mousePosition);
            };

            AssociatedObject.IsVisibleChanged += AssociatedObject_IsVisibleChanged;
        }

        private void AssociatedObject_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                SetWindowPosition(_lastMousePosition);
            }
        }

        private void SetWindowPosition(Point mousePosition)
        {
            _lastMousePosition = mousePosition;
            var dpi = MonitorResolutionHelper.GetCurrentMonitorDpi();
            var mousePositionScaled = new Point(mousePosition.X / dpi.DpiScaleX, mousePosition.Y / dpi.DpiScaleX);

            var monitorBounds = GetBoundsOfMonitorWithMouseIn(mousePosition);

            var windowLeft = mousePositionScaled.X + XOffset;
            var windowTop = mousePositionScaled.Y + YOffset;

            if ((windowLeft + MonitorRightSideDeadZone) > monitorBounds.Right / dpi.DpiScaleX)
            {
                windowLeft -= MonitorRightSideDeadZone - (((int)monitorBounds.Right / dpi.DpiScaleX - windowLeft));
            }

            if ((windowTop + MonitorBottomSideDeadZone) > monitorBounds.Bottom / dpi.DpiScaleX)
            {
                windowTop -= MonitorBottomSideDeadZone - (((int)monitorBounds.Bottom / dpi.DpiScaleX - windowTop));
            }

            AssociatedObject.Left = windowLeft;
            AssociatedObject.Top = windowTop;
        }

        private static Rect GetBoundsOfMonitorWithMouseIn(Point mousePosition)
        {
            foreach (var monitor in MonitorResolutionHelper.AllMonitors)
            {
                if (monitor.Bounds.Contains(new Point(mousePosition.X, mousePosition.Y)))
                {
                    return monitor.Bounds;
                }
            }

            return new Rect(0, 0, 0, 0);
        }
    }
}
