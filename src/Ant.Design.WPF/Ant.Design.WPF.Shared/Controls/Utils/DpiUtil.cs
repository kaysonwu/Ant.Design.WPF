namespace Antd.Controls
{
    using System.Windows;
    using System.Windows.Media;

    public  static class DpiUtil
    {
        public static DpiScale GetDpi(Visual visual)
        {
#if NET40 || NET45
            var source = PresentationSource.FromVisual(visual);

            if (source?.CompositionTarget == null)
            {
                return new DpiScale(1.0, 1.0);
            }

            var device = source.CompositionTarget.TransformToDevice;
            return new DpiScale(device.M11, device.M22); 
#else
            return VisualTreeHelper.GetDpi(visual);
#endif
        }
    }

#if NET40 || NET45
    /// <summary>Stores DPI information from which a <see cref="T:System.Windows.Media.Visual" /> or <see cref="T:System.Windows.UIElement" /> is rendered.</summary>
    public struct DpiScale
    {
        /// <summary>Gets the DPI scale on the X axis.</summary>
        /// <returns>The DPI scale for the X axis.</returns>
        public double DpiScaleX { get; private set; }

        /// <summary>Gets the DPI scale on the Yaxis.</summary>
        /// <returns>The DPI scale for the Y axis.</returns>
        public double DpiScaleY { get; private set; }

        /// <summary>Get or sets the PixelsPerDip at which the text should be rendered.</summary>
        /// <returns>The current <see cref="P:System.Windows.DpiScale.PixelsPerDip" /> value.</returns>
        public double PixelsPerDip
        {
            get
            {
                return DpiScaleY;
            }
        }

        /// <summary>Gets the DPI along X axis.</summary>
        /// <returns>The DPI along the X axis.</returns>
        public double PixelsPerInchX
        {
            get
            {
                return 96.0 * DpiScaleX;
            }
        }

        /// <summary>Gets the DPI along Y axis.</summary>
        /// <returns>The DPI along the Y axis.</returns>
        public double PixelsPerInchY
        {
            get
            {
                return 96.0 * DpiScaleY;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Windows.DpiScale" /> structure.</summary>
        /// <param name="dpiScaleX">The DPI scale on the X axis.</param>
        /// <param name="dpiScaleY">The DPI scale on the Y axis. </param>
        public DpiScale(double dpiScaleX, double dpiScaleY)
        {
            DpiScaleX = dpiScaleX;
            DpiScaleY = dpiScaleY;
        }
    }
#endif
}
