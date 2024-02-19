using ColorDict.Core.Config;
using ColorDict.Core.Converters;
using ColorDict.Core.Helpers;
using ColorDict.Core.Models;
using ColorDict.Picker.Local.ViewModels;
using ColorDict.Picker.UI.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Jamesnet.Wpf.Controls;
using Prism.Ioc;
using Prism.Regions;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ColorDict.Editor.Local.ViewModels
{
    public partial class ColorEditorViewModel : ObservableObject, IViewLoadable
    {
        private readonly IRegionManager _regionManager;
        private readonly IContainerProvider _containerProvider;
        private readonly PickerManager _pickerManger;

        [ObservableProperty]
        private BitmapSource _captureImage;

        [ObservableProperty]
        private bool _isColorCapturing;

        [ObservableProperty]
        private string _currentColor;

        [ObservableProperty]
        private int _red;

        [ObservableProperty]
        private int _green;

        [ObservableProperty]
        private int _blue;

        [ObservableProperty]
        private int _hue;

        [ObservableProperty]
        private int _saturation;

        [ObservableProperty]
        private int _value;

        [ObservableProperty]
        public int _alpha = 255;

        public ColorEditorViewModel(IRegionManager regionManager, IContainerProvider containerProvider, PickerManager pickerManager)
        {
            _regionManager = regionManager;
            _containerProvider = containerProvider;
            _pickerManger = pickerManager;
            ExtractedColorSet = new ExtractedColorCollection();

            ColorStruct color = ConvertColor.Parse("#ffffffff");

            if (color.Blue < 128)
            {
                _ = color.SetAdd(128);
                for (int i = 0; i < 64; i++)
                {
                    ExtractColor(color.SetAdd(-2));
                }
            }
            else
            {
                _ = color.SetAdd(-128);
                for (int i = 0; i < 64; i++)
                {
                    ExtractColor(color.SetAdd(2));
                }
            }
        }

        public void OnLoaded(IViewable view)
        {
            FrameworkElement fe = (FrameworkElement)view;
            Window.GetWindow(fe).Closed += (s, e) =>
            {
                ColorConfig.SaveSpoidColor(CurrentColor);
                Window.GetWindow(fe).Close();
            };
        }

        public ExtractedColorCollection ExtractedColorSet
        {
            get;
            set;
        }

        [RelayCommand]
        private void ColorClick(ColorStampModel color)
        {
            if (color != null)
            {
                ExtractColor(new ColorStruct(color.Red, color.Green, color.Blue, (byte)255));
            }
        }

        [RelayCommand]
        private void Capture(object obj)
        {
            _ = _pickerManger.ResolveWindow<PickerWindow>();
            _pickerManger.ShowPicker();
            _pickerManger.windowHidden += _pickerManger_windowHidden;
        }

        private void _pickerManger_windowHidden(object? sender, EventArgs e)
        {
            PickerWindow window = _pickerManger.ResolveWindow<PickerWindow>();
            PickerViewModel pickerViewModel = (PickerViewModel)window.DataContext;
            if (pickerViewModel.SaveColor)
            {
                Color color = pickerViewModel.CurrentColor;
                _red = color.R;
                _green = color.G;
                _blue = color.B;
                ExtractColor(new ColorStruct(color));
                OnPropertyChanged("Red"); OnPropertyChanged("Green"); OnPropertyChanged("Blue");
            }
            _pickerManger.windowHidden -= _pickerManger_windowHidden;
        }

        [RelayCommand]
        private void Paste(object obj)
        {
            if (obj is "COPY")
            {
                Clipboard.SetText(CurrentColor);
            }
        }

        private void ExtractColor(ColorStruct rgba)
        {
            CurrentColor = ConvertColor.Hex(rgba);

            Red = rgba.Red;
            Green = rgba.Green;
            Blue = rgba.Blue;
            ExtractedColorSet.Insert(rgba);

            var hsvTuple = ColorSpacecConverter.RgbToHsv(Red, Green, Blue);
            Hue = (int)hsvTuple.Item1;
            Saturation = (int)hsvTuple.Item2;
            Value = (int)hsvTuple.Item3;
        }

        partial void OnHueChanged(int value)
        {
            ApplyRgb();
        }

        partial void OnSaturationChanged(int value)
        {
            ApplyRgb();
        }

        partial void OnValueChanged(int value)
        {
            ApplyRgb();
        }

        partial void OnRedChanged(int value)
        {
            ApplyHsv();
        }

        partial void OnGreenChanged(int value)
        {
            ApplyHsv();
        }

        partial void OnBlueChanged(int value)
        {
            ApplyHsv();
        }

        private void ApplyRgb()
        {
            var rgbTuple = ColorSpacecConverter.HsvToRgb(Hue, (double)Saturation / 100, (double)Value / 100);
            _red = (int)(rgbTuple.Item1 * 255);
            _green = (int)(rgbTuple.Item2 * 255);
            _blue = (int)(rgbTuple.Item3 * 255);
            OnPropertyChanged("Red"); OnPropertyChanged("Green"); OnPropertyChanged("Blue");
            ApplyCurrentColor();
        }

        private void ApplyHsv()
        {
            var hsvTuple = ColorSpacecConverter.RgbToHsv(Red, Green, Blue);
            _hue = (int)(hsvTuple.Item1);
            _saturation = (int)hsvTuple.Item2;
            _value = (int)hsvTuple.Item3;
            OnPropertyChanged("Hue"); OnPropertyChanged("Saturation"); OnPropertyChanged("Value");
            ApplyCurrentColor();
        }

        private void ApplyCurrentColor()
        {
            CurrentColor = ConvertColor.Hex(new ColorStruct(Red, Green, Blue, Alpha));
        }
    }
}
