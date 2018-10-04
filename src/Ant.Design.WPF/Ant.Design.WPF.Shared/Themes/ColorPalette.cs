using System;
using System.Windows.Media;

namespace Antd
{
    public static class ColorPalette
    {
        #region Fields

        private const int hueStep = 2;

        private const int saturationStep = 16;

        private const int saturationStep2 = 5;

        private const int brightnessStep1 = 5;

        private const int brightnessStep2 = 15;

        private const int lightColorCount = 5;

        private const int darkColorCount = 4;

        #endregion

        #region Public Methods

        public static Color Toning(Color color, int index)
        {
            bool isLight = index <= 6;

            HSV hsv = Color2Hsv(color);
            int i = isLight ? lightColorCount + 1 - index : index - lightColorCount - 1;

            return Hsv2Color(GetHue(hsv, i, isLight), GetSaturation(hsv, i, isLight), GetValue(hsv, i, isLight), color.A);
        }

        #endregion

        #region Private Methods

        private static double GetHue(HSV hsv, int i, bool isLight)
        {
            double hue;

            if (hsv.H >= 60 && hsv.H <= 240)
            {
                hue = isLight ? hsv.H - hueStep * i : hsv.H + hueStep * i;
            }
            else
            {
                hue = isLight ? hsv.H + hueStep * i : hsv.H - hueStep * i;
            }

            if (hue < 0)
            {
                hue += 360;
            }
            else if (hue >= 360)
            {
                hue -= 360;
            }

            return Math.Round(hue);
        }
        private static double GetSaturation(HSV hsv, int i, bool isLight)
        {
            double saturation;

            if (isLight)
            {
                saturation = Math.Round(hsv.S * 100) - saturationStep * i;
            }
            else if (i == darkColorCount)
            {
                saturation = Math.Round(hsv.S * 100) + saturationStep;
            }
            else
            {
                saturation = Math.Round(hsv.S * 100) + saturationStep2 * i;
            }

            if (saturation > 100)
            {
                saturation = 100;
            }

            if (isLight && i == lightColorCount && saturation > 10)
            {
                saturation = 10;
            }

            if (saturation < 6)
            {
                saturation = 6;
            }

            return Math.Round(saturation);
        }

        private static double GetValue(HSV hsv, int i, bool isLight)
        {
            if (isLight)
            {
                return Math.Round(hsv.V * 100) + brightnessStep1 * i;
            }
            return Math.Round(hsv.V * 100) - brightnessStep2 * i;
        }

        private static Color Hsv2Color(double h, double s, double v, byte a)
        {
            h = Bound(h, 360) * 6;
            s = Bound(s, 100);
            v = Bound(v, 100);

            double i = Math.Floor(h);
            double f = h - i;

            double p = v * (1 - s),
                   q = v * (1 - f * s),
                   t = v * (1 - (1 - f) * s);

            int mod = (int)(i % 6);
            double r = new double[6] { v, q, p, p, t, v }[mod],
                   g = new double[6] { t, v, v, q, p, p }[mod],
                   b = new double[6] { p, p, t, v, v, q }[mod];

            r = Math.Min(255, Math.Max(0, r * 255));
            g = Math.Min(255, Math.Max(0, g * 255));
            b = Math.Min(255, Math.Max(0, b * 255));

            return new Color() { R = (byte)Math.Round(r), G = (byte)Math.Round(g), B = (byte)Math.Round(b), A = a };
        }

        private static HSV Color2Hsv(Color color)
        {
            double r = Bound(color.R, 255),
                   g = Bound(color.G, 255),
                   b = Bound(color.B, 255);

            double max = Math.Max(r, Math.Max(g, b)),
                   min = Math.Min(r, Math.Min(g, b));

            double h = 0,
                   v = max,
                   d = max - min;
            double s = max == 0 ? 0 : d / max;

            if (max != min)
            {
                if (max == r)
                {
                    h = (g - b) / d + (g < b ? 6 : 0);
                }
                else if (max == g)
                {
                    h = (b - r) / d + 2;
                }
                else if (max == b)
                {
                    h = (r - g) / d + 4;
                }

                h /= 6;
            }

            return new HSV() { H = h * 360, S = s, V = v };
        }

        /// <summary>
        /// Take input from [0, n] and return it as [0, 1]
        /// </summary>
        /// <param name="n"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private static double Bound(double n, double max)
        {
            n = Math.Min(max, Math.Max(0, n));

            // Handle floating point rounding errors
            if ((Math.Abs(n - max) < 0.000001))
            {
                return 1;
            }

            // Convert into [0, 1] range if it isn't already
            return (n % max) / max;
        }

        #endregion
    }

    internal struct HSV
    {
        public double H;

        public double S;

        public double V;
    }
}
