namespace ColorDict.Core.Converters
{
    public static class ColorSpacecConverter
    {
        public static Tuple<double, double, double> RgbToHsv(int red, int green, int blue)
        {
            double r = red / 255.0;
            double g = green / 255.0;
            double b = blue / 255.0;

            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));
            double h, s, v;

            v = max;

            double delta = max - min;

            if (IsAchromatic(max, delta))
            {
                h = s = 0;
            }
            else
            {
                s = GetSaturation(delta, max);
                h = GetHue(r, g, b, max, delta);
            }

            h = h * 60;
            s *= 100;
            v *= 100;

            return new Tuple<double, double, double>(h, s, v);
        }

        private static bool IsAchromatic(double max, double delta)
        {
            return max == 0 || Math.Abs(delta) < double.Epsilon;
        }

        private static double GetSaturation(double delta, double max)
        {
            return delta / max;
        }

        private static double GetHue(double r, double g, double b, double max, double delta)
        {
            double hue;

            if (max == r)
            {
                hue = (g - b) / delta + ((g < b) ? 6 : 0);
            }
            else if (max == g)
            {
                hue = (b - r) / delta + 2;
            }
            else
            {
                hue = (r - g) / delta + 4;
            }

            return hue;
        }

        public static Tuple<double, double, double> HsvToRgb(double hue, double satuation, double value)
        {
            const double EPSILON = 1e-10;

            if (Math.Abs(satuation) < EPSILON)
                return new Tuple<double, double, double>(value, value, value);

            hue = hue >= 360 ? 0 : hue / 60;
            var i = (int)hue;
            var f = hue - i;
            var p = value * (1 - satuation);
            var q = value * (1 - satuation * f);
            var t = value * (1 - satuation * (1 - f));

            switch (i)
            {
                case 0: return new Tuple<double, double, double>(value, t, p);
                case 1: return new Tuple<double, double, double>(q, value, p);
                case 2: return new Tuple<double, double, double>(p, value, t);
                case 3: return new Tuple<double, double, double>(p, q, value);
                case 4: return new Tuple<double, double, double>(t, p, value);
                default: return new Tuple<double, double, double>(value, p, q);
            };
        }
    }
}
