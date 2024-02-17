using ColorDict.Core.Config;
using ColorDict.Core.Converters;
using ColorDict.Core.Models;
using ColorPicker.Local.Worker;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Jamesnet.Wpf.Controls;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ColorDict.Editor.Local.ViewModels
{
    public partial class ColorEditorViewModel : ObservableObject, IViewLoadable
    {

        private bool _isCaptureActivated;
        private readonly PixelExtractWorker _captureWorker;

        [ObservableProperty]
        private BitmapSource _captureImage;

        [ObservableProperty]
        private bool _isColorCapturing;

        [ObservableProperty]
        private string _currentColor;

        [ObservableProperty]
        private string _reverseColor;

        [ObservableProperty]
        private string _contrastColor;

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

        public ColorEditorViewModel()
        {
            ExtractedColorSet = new ExtractedColorCollection();

            _captureWorker = new PixelExtractWorker
            {
                StartExtract = ExtractColor,
                FinishExtract = () => IsColorCapturing = false
            };

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
            IsColorCapturing = true;
            _captureWorker.Begin();
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
            _isCaptureActivated = true;
            CurrentColor = ConvertColor.Hex(rgba);
            ReverseColor = ConvertColor.ReverseHex(rgba);
            ContrastColor = ConvertColor.Contrast(rgba);

            Red = rgba.Red;
            Green = rgba.Green;
            Blue = rgba.Blue;
            ExtractedColorSet.Insert(rgba);
            _isCaptureActivated = false;

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
        }

        private void ApplyHsv()
        {
            var hsvTuple = ColorSpacecConverter.RgbToHsv(Red, Green, Blue);
            _hue = (int)(hsvTuple.Item1);
            _saturation = (int)hsvTuple.Item2;
            _value = (int)hsvTuple.Item3;
            OnPropertyChanged("Hue"); OnPropertyChanged("Saturation"); OnPropertyChanged("Value");
        }
    }
}
