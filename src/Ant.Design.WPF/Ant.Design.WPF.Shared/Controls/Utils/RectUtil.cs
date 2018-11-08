using System;
using System.Windows;

namespace Antd.Controls
{
    internal static class RectUtil
    {
        /// <summary>
        /// Deflates rectangle by given thickness
        /// </summary>
        /// <param name="rect">Rectangle</param>
        /// <param name="thick">Thickness</param>
        /// <returns>Deflated Rectangle</returns>
        internal static Rect Deflate(Rect rect, Thickness thick)
        {
            return new Rect(rect.Left + thick.Left, rect.Top + thick.Top,
                            Math.Max(0.0, rect.Width - thick.Left - thick.Right),
                            Math.Max(0.0, rect.Height - thick.Top - thick.Bottom));
        }

        /// <summary>
        /// Inflates rectangle by given thickness
        /// </summary>
        /// <param name="rect">Rectangle</param>
        /// <param name="thick">Thickness</param>
        /// <returns>Inflated Rectangle</returns>
        internal static Rect Inflate(Rect rect, Thickness thick)
        {
            return new Rect(rect.Left - thick.Left, rect.Top - thick.Top,
                            Math.Max(0.0, rect.Width + thick.Left + thick.Right),
                            Math.Max(0.0, rect.Height + thick.Top + thick.Bottom));
        }
    }
}
