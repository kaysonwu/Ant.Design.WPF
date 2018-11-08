// Copyright (c) 2017 Ratish Philip 
//
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is 
// furnished to do so, subject to the following conditions: 
// 
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software. 
// 
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE. 
//
// This file is part of the WPFSpark project: https://github.com/ratishphilip/wpfspark
//
// WPFSpark v1.3.1
// 
using System.Windows;

namespace Antd.Controls
{
    public static class BorderUtil
    {
        #region Thickness



        #endregion

        #region CornerRadius

        /// <summary>
        /// Verifies if this CornerRadius contains only valid values
        /// The set of validity checks is passed as parameters.
        /// </summary>
        /// <param name='corner'>CornerRadius value</param>
        /// <param name='allowNegative'>allows negative values</param>
        /// <param name='allowNaN'>allows Double.NaN</param>
        /// <param name='allowPositiveInfinity'>allows Double.PositiveInfinity</param>
        /// <param name='allowNegativeInfinity'>allows Double.NegativeInfinity</param>
        /// <returns>Whether or not the CornerRadius complies to the range specified</returns>
        public static bool IsValid(this CornerRadius corner, bool allowNegative, bool allowNaN, bool allowPositiveInfinity, bool allowNegativeInfinity)
        {
            if (!allowNegative)
            {
                if (corner.TopLeft < 0d || corner.TopRight < 0d || corner.BottomLeft < 0d || corner.BottomRight < 0d)
                {
                    return (false);
                }
            }

            if (!allowNaN)
            {
                if (DoubleUtil.IsNaN(corner.TopLeft) || DoubleUtil.IsNaN(corner.TopRight) ||
                    DoubleUtil.IsNaN(corner.BottomLeft) || DoubleUtil.IsNaN(corner.BottomRight))
                {
                    return (false);
                }
            }

            if (!allowPositiveInfinity)
            {
                if (double.IsPositiveInfinity(corner.TopLeft) || double.IsPositiveInfinity(corner.TopRight) ||
                    double.IsPositiveInfinity(corner.BottomLeft) || double.IsPositiveInfinity(corner.BottomRight))
                {
                    return (false);
                }
            }

            if (!allowNegativeInfinity)
            {
                if (double.IsNegativeInfinity(corner.TopLeft) || double.IsNegativeInfinity(corner.TopRight) ||
                    double.IsNegativeInfinity(corner.BottomLeft) || double.IsNegativeInfinity(corner.BottomRight))
                {
                    return (false);
                }
            }

            return true;
        }

        /// <summary>
        /// Verifies if the CornerRadius contains only zero values
        /// </summary>
        /// <param name="corner">CornerRadius</param>
        /// <returns>Size</returns>
        public static bool IsZero(this CornerRadius corner)
        {
            return DoubleUtil.IsZero(corner.TopLeft) && DoubleUtil.IsZero(corner.TopRight)
                    && DoubleUtil.IsZero(corner.BottomRight) && DoubleUtil.IsZero(corner.BottomLeft);
        }

        /// <summary>
        /// Verifies if the CornerRadius contains same values
        /// </summary>
        /// <param name="corner">CornerRadius</param>
        /// <returns>true if yes, otherwise false</returns>
        public static bool IsUniform(this CornerRadius corner)
        {
            var topLeft = corner.TopLeft;
            return DoubleUtil.AreClose(topLeft, corner.TopRight)
                    && DoubleUtil.AreClose(topLeft, corner.BottomRight)
                    && DoubleUtil.AreClose(topLeft, corner.BottomLeft);
        }

        #endregion
    }
}
