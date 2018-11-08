using System.Windows;

namespace Antd.Controls
{
    internal static class ThicknessUtil
    {
        /// <summary>
        /// Verifies if this Thickness contains only valid values
        /// The set of validity checks is passed as parameters.
        /// </summary>
        /// <param name='thick'>Thickness value</param>
        /// <param name='allowNegative'>allows negative values</param>
        /// <param name='allowNaN'>allows Double.NaN</param>
        /// <param name='allowPositiveInfinity'>allows Double.PositiveInfinity</param>
        /// <param name='allowNegativeInfinity'>allows Double.NegativeInfinity</param>
        /// <returns>Whether or not the thickness complies to the range specified</returns>
        public static bool IsValid(Thickness thick, bool allowNegative, bool allowNaN, bool allowPositiveInfinity, bool allowNegativeInfinity)
        {
            if (!allowNegative)
            {
                if (thick.Left < 0d || thick.Right < 0d || thick.Top < 0d || thick.Bottom < 0d)
                    return false;
            }

            if (!allowNaN)
            {
                if (DoubleUtil.IsNaN(thick.Left) || DoubleUtil.IsNaN(thick.Right)
                    || DoubleUtil.IsNaN(thick.Top) || DoubleUtil.IsNaN(thick.Bottom))
                    return false;
            }

            if (!allowPositiveInfinity)
            {
                if (double.IsPositiveInfinity(thick.Left) || double.IsPositiveInfinity(thick.Right)
                    || double.IsPositiveInfinity(thick.Top) || double.IsPositiveInfinity(thick.Bottom))
                {
                    return false;
                }
            }

            if (!allowNegativeInfinity)
            {
                if (double.IsNegativeInfinity(thick.Left) || double.IsNegativeInfinity(thick.Right)
                    || double.IsNegativeInfinity(thick.Top) || double.IsNegativeInfinity(thick.Bottom))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Method to add up the left and right size as width, as well as the top and bottom size as height
        /// </summary>
        /// <param name="thick">Thickness</param>
        /// <returns>Size</returns>
        public static Size CollapseThickness(Thickness thick)
        {
            return new Size(thick.Left + thick.Right, thick.Top + thick.Bottom);
        }

        /// <summary>
        /// Verifies if the Thickness contains only zero values
        /// </summary>
        /// <param name="thick">Thickness</param>
        /// <returns>Size</returns>
        public static bool IsZero(Thickness thick)
        {
            return DoubleUtil.IsZero(thick.Left) && DoubleUtil.IsZero(thick.Top)
                    && DoubleUtil.IsZero(thick.Right) && DoubleUtil.IsZero(thick.Bottom);
        }

        /// <summary>
        /// Verifies if all the values in Thickness are same
        /// </summary>
        /// <param name="thick">Thickness</param>
        /// <returns>true if yes, otherwise false</returns>
        public static bool IsUniform(Thickness thick)
        {
            var left = thick.Left;
            return DoubleUtil.AreClose(left, thick.Top)
                   && DoubleUtil.AreClose(left, thick.Right)
                   && DoubleUtil.AreClose(left, thick.Bottom);
        }
    }
}
