using ColorDict.Core.Helpers;
using ColorDict.Core.Interfaces;
using ColorDict.Core.Keyboard;
using CommunityToolkit.Mvvm.ComponentModel;
using Prism.Ioc;
using Prism.Regions;
using System.Windows.Media;
using System.Windows;

namespace ColorDict.Picker.Local.ViewModels
{
    public class PickerViewModel : ObservableObject
    {
        private string _colorString;
        private Brush _displayedColorBrush;
        private readonly IMouseInfoProvider _mouseInfoProvider;
        private readonly PickerManager _pickerManager;
        private readonly IColorProvider _colorProvider;
        private System.Drawing.Color _currentColor;
        private bool _mouseDown;

        public PickerViewModel(IContainerProvider containerProvider, IRegionManager regionManager, PickerManager pickerManager,
            IMouseInfoProvider mouseInfoProvider, IColorProvider colorProvider, KeyboardMonitor keyboardMonitor)
        {
            _pickerManager = pickerManager;
            _mouseInfoProvider = mouseInfoProvider;
            _colorProvider = colorProvider;
            mouseInfoProvider.MouseColorChanged += Mouse_ColorChanged;
            mouseInfoProvider.OnLeftMouseDown += MouseInfoProvider_OnLeftMouseDown;
            mouseInfoProvider.OnLeftMouseUp += MouseInfoProvider_OnLeftMouseUp;
            mouseInfoProvider.OnRightMouseDown += MouseInfoProvider_OnRightMouseDown;
            mouseInfoProvider.MousePositionChanged += MouseInfoProvider_MousePositionChanged;
            mouseInfoProvider.OnMouseWheel += MouseInfoProvider_OnMouseWheel;

            keyboardMonitor.Start();

#if !DEBUG
            CheckForUpdates(appUpdateManager, userSettings);
#endif
        }

        public IMouseInfoProvider getMouseInfoProvider()
        {
            return _mouseInfoProvider;
        }

        public string ColorString
        {
            get
            {
                return _colorString;
            }
            set
            {
                _colorString = value;
                OnPropertyChanged();
            }
        }

        public Brush DisplayedColorBrush
        {
            get
            {
                return _displayedColorBrush;
            }
            private set
            {
                _displayedColorBrush = value;
                OnPropertyChanged();
            }
        }

        public System.Drawing.Color CurrentColor
        {
            get 
            { 
                return _currentColor; 
            }
            set => _currentColor = value;
        }

        public bool SaveColor
        {
            get
            {
                return _mouseDown;
            }
            set => _mouseDown = value;
        }

        private void Mouse_ColorChanged(object sender, System.Drawing.Color color)
        {
            _currentColor = color;
            DisplayedColorBrush = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
        }

        private void MouseInfoProvider_MousePositionChanged(object sender, Point e)
        {
            if (_mouseDown)
            {
                _mouseInfoProvider.SetOriginalCursor();
            }
            if (!_mouseDown)
            {
                _currentColor = _colorProvider.GetPixelColor(e);
                //ColorString = ColorFormatHelper.ColorToString(_currentColor, _userSettings.SelectedColorFormat.Value);
                DisplayedColorBrush = new SolidColorBrush(Color.FromArgb(_currentColor.A, _currentColor.R, _currentColor.G, _currentColor.B));
            }
        }

        private void MouseInfoProvider_OnLeftMouseDown(object sender, System.Drawing.Point p)
        {
            _mouseInfoProvider.SetOriginalCursor();
            _mouseDown = true;
            _pickerManager.HidePicker();
        }

        private void MouseInfoProvider_OnLeftMouseUp(object sender, System.Drawing.Point p)
        {
            _mouseDown = false;
            _mouseInfoProvider.StopMonitoring();
        }

        private void MouseInfoProvider_OnRightMouseDown(object sender, System.Drawing.Point p)
        {
            _mouseDown = false;
            _pickerManager.HidePicker();
            _mouseInfoProvider.StopMonitoring();
        }

        private void MouseInfoProvider_OnMouseWheel(object sender, Tuple<Point, bool> e)
        {
            _mouseDown = false;
            _mouseInfoProvider.StopMonitoring();
            _pickerManager.HidePicker();
        }
    }
}
