using ColorDict.Core.Converters;
using ColorDict.Core.Models;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ColorDict.Editor.UI.Units
{
    public class ColorMatrix : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty HueProperty
            = DependencyProperty.Register(nameof(Hue), typeof(double), typeof(ColorMatrix),
                new PropertyMetadata(0.0, OnHueChanged));

        public static readonly DependencyProperty HeadXProperty
            = DependencyProperty.Register(nameof(HeadX), typeof(double), typeof(ColorMatrix),
                new PropertyMetadata(0.0));

        public static readonly DependencyProperty HeadYProperty
            = DependencyProperty.Register(nameof(HeadY), typeof(double), typeof(ColorMatrix),
                new PropertyMetadata(0.0));

        static ColorMatrix()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorMatrix), new FrameworkPropertyMetadata(typeof(ColorMatrix)));
        }

        public ColorMatrix()
        {
            GradientBitmap = new WriteableBitmap(32, 32, 96, 96, PixelFormats.Rgb24, null);
            Loaded += ColorMatrix_Loaded;
            RecalculateGradient();
        }

        private void ColorMatrix_Loaded(object sender, RoutedEventArgs e)
        {
            Grid grid = this.Template.FindName("PART_Grid", this) as Grid;
            grid.MouseDown += new MouseButtonEventHandler(OnMouseDown);
            grid.MouseMove += new MouseEventHandler(OnMouseMove);
            grid.MouseUp += new MouseButtonEventHandler(OnMouseUp);
        }

        private WriteableBitmap _gradientBitmap;
        private double _rangeX;
        private double _rangeY;
        private Func<double, double, double, Tuple<double, double, double>> convertHsvToRgb
            = ColorSpacecConverter.HsvToRgb;

        public double Hue
        {
            get => (double)GetValue(HueProperty);
            set => SetValue(HueProperty, value);
        }

        public double HeadX
        {
            get => (double)GetValue(HeadXProperty);
            set => SetValue(HeadXProperty, value);
        }

        public double HeadY
        {
            get => (double)GetValue(HeadYProperty);
            set => SetValue(HeadYProperty, value);
        }

        public WriteableBitmap GradientBitmap
        {
            get => _gradientBitmap;
            set
            {
                _gradientBitmap = value;
                RaisePropertyChanged(nameof(GradientBitmap));
            }
        }

        public double RangeX
        {
            get => _rangeX;
            set
            {
                _rangeX = value;
                RaisePropertyChanged(nameof(RangeX));
            }
        }

        public double RangeY
        {
            get => _rangeY;
            set
            {
                _rangeY = value;
                RaisePropertyChanged(nameof(RangeY));
            }
        }


        private void RecalculateGradient()
        {
            int width = GradientBitmap.PixelWidth;
            int height = GradientBitmap.PixelHeight;
            double hue = Hue;
            var pixels = new byte[width * height * 3];

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    Tuple<double, double, double> rgbTuple =
                        convertHsvToRgb(hue, i / (double)(width - 1), (height - 1 - j) / (double)(height - 1));
                    double red = rgbTuple.Item1, green = rgbTuple.Item2, blue = rgbTuple.Item3;
                    int position = (j * height + i) * 3;
                    pixels[position] = (byte)(red * 255);
                    pixels[position + 1] = (byte)(green * 255);
                    pixels[position + 2] = (byte)(blue * 255);
                }
            }
            GradientBitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, width * 3, 0);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private static void OnHueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ColorMatrix)d).RecalculateGradient();
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            ((UIElement)sender).CaptureMouse();
            UpdatePosition(e.GetPosition(this));
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var grid = (Grid)sender;
            if (grid.IsMouseCaptured)
            {
                UpdatePosition(e.GetPosition(this));
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            ((UIElement)sender).ReleaseMouseCapture();
        }

        private void UpdatePosition(Point position)
        {
            HeadX = Math.Clamp(position.X / ActualWidth, 0, 1) * RangeX;
            HeadY = (1 - Math.Clamp(position.Y / ActualHeight, 0, 1)) * RangeY;
        }

        private void RaisePropertyChanged(string propertyName)
        {
            if (propertyName != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
