using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Antd.Controls
{
    /// <summary>
    /// Draws a border, background, or both around another element.
    /// </summary>
    public class Border : Decorator
    {
        #region Fields

        private StreamGeometry backgroundGeometryCache;

        private StreamGeometry upperLeftCache;

        private StreamGeometry upperRightCache;

        private StreamGeometry lowerRightCache;

        private StreamGeometry lowerLeftCache;

        private bool useComplexRender;

        private Pen leftPenCache;

        private Pen topPenCache;

        private Pen rightPenCache;

        private Pen bottomPenCache;

        #endregion

        #region Properties

        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
            "Background",
            typeof(Brush),
            typeof(Border),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender
            ));

        /// <summary>
        /// Gets or sets the brush that fills the area between the bounds of a Border.
        /// </summary>
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register(
            "BorderBrush",
            typeof(Brush),
            typeof(Border),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender,
                OnClearPenCache
            ));

        /// <summary>
        /// Gets or sets the brush that draws the outer border color.
        /// </summary>
        public Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register(
            "BorderThickness",
            typeof(Thickness),
            typeof(Border),
            new FrameworkPropertyMetadata(
                 new Thickness(),
                 FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                 OnClearPenCache
            ),
            IsThicknessValid);

        /// <summary>
        /// Gets or sets the relative thickness of a border.
        /// </summary>
        public Thickness BorderThickness
        {
            get { return (Thickness)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        private static void OnClearPenCache(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var border = (Border)d;
            border.leftPenCache = border.topPenCache = border.rightPenCache = border.bottomPenCache = null;
        }

        private static bool IsThicknessValid(object value)
        {
            Thickness t = (Thickness)value;
            return t.IsValid(false, false, false, false);
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius",
            typeof(CornerRadius),
            typeof(Border),
            new FrameworkPropertyMetadata(
                new CornerRadius(),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender
            ),
            IsCornerRadiusValid);

        /// <summary>
        /// Gets or sets a value that represents the degree to which the corners of a Border are rounded.
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        private static bool IsCornerRadiusValid(object value)
        {
            CornerRadius cr = (CornerRadius)value;
            return (cr.IsValid(false, false, false, false));
        }

        public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register(
            "Padding",
            typeof(Thickness),
            typeof(Border),
            new FrameworkPropertyMetadata(
                new Thickness(),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender
            ),
            IsThicknessValid);

        /// <summary>
        /// Gets or sets a thickness value that describes the amount of space between a border and its child element.
        /// </summary>
        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        public static readonly DependencyProperty BorderStyleProperty = DependencyProperty.Register(
            "BorderStyle",
            typeof(BorderStyle),
            typeof(Border),
            new FrameworkPropertyMetadata(
                BorderStyle.Solid,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender,
                OnClearPenCache
            ));

        /// <summary>
        /// Gets or sets the border style.
        /// </summary>
        public BorderStyle BorderStyle
        {
            get { return (BorderStyle)GetValue(BorderStyleProperty); }
            set { SetValue(BorderStyleProperty, value); }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Updates DesiredSize of the Border.  Called by parent UIElement.  This is the first pass of layout.
        /// </summary>
        /// <remarks>
        /// Border determines its desired size it needs from the specified border the child: its sizing
        /// properties, margin, and requested size.
        /// </remarks>
        /// <param name="constraint">Constraint size is an "upper limit" that the return value should not exceed.</param>
        /// <returns>The Decorator's desired size.</returns>
        protected override Size MeasureOverride(Size constraint)
        {
            var child = Child;
            var desiredSize = new Size();
            var borders = BorderThickness;

            if (UseLayoutRounding)
            {
                var dpi = DpiUtil.GetDpi(this);
                borders = new Thickness(RoundLayoutValue(borders.Left, dpi.DpiScaleX), RoundLayoutValue(borders.Top, dpi.DpiScaleY),
                   RoundLayoutValue(borders.Right, dpi.DpiScaleX), RoundLayoutValue(borders.Bottom, dpi.DpiScaleY));
            }
    
            // Compute the total size required
            var borderSize = borders.CollapseThickness();
            var paddingSize = Padding.CollapseThickness();

            // If we have a child
            if (child != null)
            {
                // Combine into total decorating size
                var combined = new Size(borderSize.Width + paddingSize.Width, borderSize.Height + paddingSize.Height);

                // Remove size of border only from child's reference size.
                var childConstraint = new Size(Math.Max(0.0, constraint.Width - combined.Width),
                                                Math.Max(0.0, constraint.Height - combined.Height));


                child.Measure(childConstraint);
                var childSize = child.DesiredSize;

                // Now use the returned size to drive our size, by adding back the margins, etc.
                desiredSize.Width = childSize.Width + combined.Width;
                desiredSize.Height = childSize.Height + combined.Height;
            }
            else
            {
                // Combine into total decorating size
                desiredSize = new Size(borderSize.Width + paddingSize.Width, borderSize.Height + paddingSize.Height);
            }

            return desiredSize;
        }

        /// <summary>
        /// Border computes the position of its single child and applies its child's alignments to the child.
        /// </summary>
        /// <param name="finalSize">The size reserved for this element by the parent</param>
        /// <returns>The actual ink area of the element, typically the same as finalSize</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            var borders = BorderThickness;

            if (UseLayoutRounding)
            {
                var dpi = DpiUtil.GetDpi(this);
                borders = new Thickness(RoundLayoutValue(borders.Left, dpi.DpiScaleX), RoundLayoutValue(borders.Top, dpi.DpiScaleY),
                   RoundLayoutValue(borders.Right, dpi.DpiScaleX), RoundLayoutValue(borders.Bottom, dpi.DpiScaleY));
            }

            var boundRect = new Rect(finalSize);
            var innerRect = RectUtil.Deflate(boundRect, borders);

            //  arrange child
            var child = Child;
            if (child != null)
            {
                Rect childRect = RectUtil.Deflate(innerRect, Padding);
                child.Arrange(childRect);
            }

            var radius = CornerRadius;

            useComplexRender = !radius.IsUniform() || !borders.IsUniform();
            backgroundGeometryCache = upperLeftCache = upperRightCache = lowerRightCache = lowerLeftCache = null;

            if (useComplexRender)
            {
                //  calculate border / background rendering geometry
                if (!boundRect.Width.IsZero() && !boundRect.Height.IsZero())
                {
                    var outerRadii = new Radii(boundRect, radius, borders, true);

                    // Upper-right corner
                    var radiusX = boundRect.TopRight.X - outerRadii.TopRight.X;
                    var radiusY = outerRadii.RightTop.Y - boundRect.TopRight.Y;
                    if (!radiusX.IsZero() || !radiusY.IsZero())
                    {
                        upperRightCache = GenerateRoundedGeometry(outerRadii.TopRight, outerRadii.RightTop, new Size(radiusX, radiusY));
                    }

                    // Lower-right corner
                    radiusX = boundRect.BottomRight.X - outerRadii.BottomRight.X;
                    radiusY = boundRect.BottomRight.Y - outerRadii.RightBottom.Y;
                    if (!radiusX.IsZero() || !radiusY.IsZero())
                    {
                        lowerRightCache = GenerateRoundedGeometry(outerRadii.RightBottom, outerRadii.BottomRight, new Size(radiusX, radiusY));
                    }

                    // Lower-left corner
                    radiusX = outerRadii.BottomLeft.X - boundRect.BottomLeft.X;
                    radiusY = boundRect.BottomLeft.Y - outerRadii.LeftBottom.Y;
                    if (!radiusX.IsZero() || !radiusY.IsZero())
                    {
                        lowerLeftCache = GenerateRoundedGeometry(outerRadii.BottomLeft, outerRadii.LeftBottom, new Size(radiusX, radiusY));
                    }

                    // Upper-left corner
                    radiusX = outerRadii.TopLeft.X - boundRect.TopLeft.X;
                    radiusY = outerRadii.LeftTop.Y - boundRect.TopLeft.Y;
                    if (!radiusX.IsZero() || !radiusY.IsZero())
                    {
                        upperLeftCache = GenerateRoundedGeometry(outerRadii.LeftTop, outerRadii.TopLeft, new Size(radiusX, radiusY));
                    }
                }

                if (!innerRect.Width.IsZero() && !innerRect.Height.IsZero())
                {
                    var innerRadii = new Radii(innerRect, radius, borders, false);
                    var backgroundGeometry = new StreamGeometry();

                    using (StreamGeometryContext sc = backgroundGeometry.Open())
                    {
                        //  create the border geometry
                        sc.BeginFigure(innerRadii.TopLeft, true /* is filled */, true /* is closed */);

                        // Top line
                        sc.LineTo(innerRadii.TopRight, true /* is stroked */, false /* is smooth join */);

                        // Upper-right corners
                        var radiusX = innerRect.TopRight.X - innerRadii.TopRight.X;
                        var radiusY = innerRadii.RightTop.Y - innerRect.TopRight.Y;
                        if (!radiusX.IsZero() || !radiusY.IsZero())
                        {
                            sc.ArcTo(innerRadii.RightTop, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true, false);
                        }

                        // Right line
                        sc.LineTo(innerRadii.RightBottom, true /* is stroked */, false /* is smooth join */);

                        // Lower-right corners
                        radiusX = innerRect.BottomRight.X - innerRadii.BottomRight.X;
                        radiusY = innerRect.BottomRight.Y - innerRadii.RightBottom.Y;
                        if (!radiusX.IsZero() || !radiusY.IsZero())
                        {
                            sc.ArcTo(innerRadii.BottomRight, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true, false);
                        }

                        // Bottom line
                        sc.LineTo(innerRadii.BottomLeft, true /* is stroked */, false /* is smooth join */);

                        // Lower-left corners
                        radiusX = innerRadii.BottomLeft.X - innerRect.BottomLeft.X;
                        radiusY = innerRect.BottomLeft.Y - innerRadii.LeftBottom.Y;
                        if (!radiusX.IsZero() || !radiusY.IsZero())
                        {
                            sc.ArcTo(innerRadii.LeftBottom, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true, false);
                        }

                        // Left line
                        sc.LineTo(innerRadii.LeftTop, true /* is stroked */, false /* is smooth join */);

                        // Upper-left corners
                        radiusX = innerRadii.TopLeft.X - innerRect.TopLeft.X;
                        radiusY = innerRadii.LeftTop.Y - innerRect.TopLeft.Y;
                        if (!radiusX.IsZero() || !radiusY.IsZero())
                        {
                            sc.ArcTo(innerRadii.TopLeft, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true, false);
                        }
                    }

                    backgroundGeometry.Freeze();
                    backgroundGeometryCache = backgroundGeometry;
                }
            }

            return finalSize;
        }

        protected override void OnRender(DrawingContext dc)
        {
            if (useComplexRender)
            {
                ComplexRender(dc);
            } else
            {
                SimpleRender(dc);
            }
        }

        #endregion

        #region Private Methods

        private void SimpleRender(DrawingContext dc)
        {
            var useLayoutRounding = UseLayoutRounding;
            var dpi = DpiUtil.GetDpi(this);

            Brush brush; 
            var borderStyle = BorderStyle;
 
            var border = BorderThickness;
            var cornerRadius = CornerRadius;

            var outerCornerRadius = cornerRadius.TopLeft; // Already validated that all corners have the same radius
            var roundedCorners = !outerCornerRadius.IsZero();

            var width = RenderSize.Width;
            var height = RenderSize.Height;

            // Draw border
            if (!border.IsZero() && (brush = BorderBrush) != null)
            {
                var pen = GetPen(brush, borderStyle, border.Left, dpi.DpiScaleX, useLayoutRounding);
                var penThickness = pen.Thickness;

                double x = penThickness * 0.5;
                var rect = new Rect(x, x, width - penThickness, height - penThickness);

                if (roundedCorners)
                {
                    dc.DrawRoundedRectangle(null, pen, rect, outerCornerRadius, outerCornerRadius);
                }
                else
                {
                    dc.DrawRectangle(null, pen, rect);
                }
            }

            // Draw background in rectangle inside border.
            if ((brush = Background) != null)
            {
                // Intialize background 
                Point ptTL, ptBR;

                if (useLayoutRounding)
                {
                    ptTL = new Point(RoundLayoutValue(border.Left, dpi.DpiScaleX),
                                     RoundLayoutValue(border.Top, dpi.DpiScaleY));
                    ptBR = new Point(width - RoundLayoutValue(border.Right, dpi.DpiScaleX),
                                     height - RoundLayoutValue(border.Bottom, dpi.DpiScaleY));
                }
                else
                {
                    ptTL = new Point(border.Left, border.Top);
                    ptBR = new Point(width - border.Right, height - border.Bottom);
                }

                // Do not draw background if the borders are so large that they overlap.
                if (ptBR.X > ptTL.X && ptBR.Y > ptTL.Y)
                {
                    if (roundedCorners)
                    {
                        // Determine the inner edge radius
                        var innerCornerRadius = Math.Max(0.0, outerCornerRadius - border.Top * 0.5);
                        dc.DrawRoundedRectangle(brush, null, new Rect(ptTL, ptBR), innerCornerRadius, innerCornerRadius);
                    }
                    else
                    {
                        dc.DrawRectangle(brush, null, new Rect(ptTL, ptBR));
                    }
                }
            }
        }

        private void ComplexRender(DrawingContext dc)
        {
            Brush brush;
            var width = RenderSize.Width;
            var height = RenderSize.Height;

            //Draw border
            if (!width.IsZero() && !height.IsZero() && (brush = BorderBrush) != null)
            {
                var useLayoutRounding = UseLayoutRounding;
                var dpi = DpiUtil.GetDpi(this);

                var borders = BorderThickness;
                var borderStyle = BorderStyle;
                var radius = CornerRadius;
                double x, y;

                // Left Line
                if (!borders.Left.IsZero())
                {
                    if (leftPenCache == null)
                    {
                        leftPenCache = GetPen(brush, borderStyle, borders.Left, dpi.DpiScaleX, useLayoutRounding);
                    }

                    x = leftPenCache.Thickness * 0.5;
                    dc.DrawLine(leftPenCache, new Point(x, radius.TopLeft), new Point(x, height - radius.BottomLeft));
                }

                // Top Line
                if (!borders.Top.IsZero())
                {
                    if (topPenCache == null)
                    {
                        topPenCache = GetPen(brush, borderStyle, borders.Top, dpi.DpiScaleY, useLayoutRounding);
                    }

                    y = topPenCache.Thickness * 0.5;
                    dc.DrawLine(topPenCache, new Point(radius.TopLeft, y), new Point(width - radius.TopRight, y));
                }

                // Right Line
                if (!borders.Right.IsZero())
                {
                    if (rightPenCache == null)
                    {
                        rightPenCache = GetPen(brush, borderStyle, borders.Right, dpi.DpiScaleX, useLayoutRounding);
                    }

                    x =  width - rightPenCache.Thickness * 0.5;
                    dc.DrawLine(rightPenCache, new Point(x, radius.TopRight), new Point(x, height - radius.BottomRight));
                }

                // Bottom Line
                if (!borders.Bottom.IsZero())
                {
                    if (bottomPenCache == null)
                    {
                        bottomPenCache = GetPen(brush, borderStyle, borders.Bottom, dpi.DpiScaleY, useLayoutRounding);
                    }

                    y = height - bottomPenCache.Thickness * 0.5;
                    dc.DrawLine(bottomPenCache, new Point(radius.BottomLeft, y), new Point(width - radius.BottomRight, y));
                }

                // Draw Rounded
                Pen pen;

                if (upperLeftCache != null && (pen = GetMaxPen(leftPenCache, topPenCache)) != null)
                {
                    dc.DrawGeometry(null, pen, upperLeftCache);
                }

                if (upperRightCache != null && (pen = GetMaxPen(topPenCache, rightPenCache)) != null)
                {
                    dc.DrawGeometry(null, pen, upperRightCache);
                }

                if (lowerRightCache != null && (pen = GetMaxPen(rightPenCache, bottomPenCache)) != null)
                {
                    dc.DrawGeometry(null, pen, lowerRightCache);
                }

                if (lowerLeftCache != null && (pen = GetMaxPen(bottomPenCache, leftPenCache)) != null)
                {
                    dc.DrawGeometry(null, pen, lowerLeftCache);
                }
            }

            // Draw background in rectangle inside border.
            if (backgroundGeometryCache != null && (brush = Background) != null)
            {
                dc.DrawGeometry(brush, null, backgroundGeometryCache);
            }
        }

        private Pen GetMaxPen(Pen pen1, Pen pen2)
        {
            if (pen2 == null || (pen1 != null && pen2.Thickness < pen1.Thickness))
            {
                return pen1;
            }

            return pen2;
        }

        private static StreamGeometry GenerateRoundedGeometry(Point startPoint, Point endPoint, Size size)
        {
            var streamGeometry = new StreamGeometry();

            using (StreamGeometryContext sc = streamGeometry.Open())
            {
                sc.BeginFigure(startPoint, true, false);
                sc.ArcTo(endPoint, size, 0, false, SweepDirection.Clockwise, true, false);
            }

            streamGeometry.Freeze();
            return streamGeometry;
        }

        private static Pen GetPen(Brush brush, BorderStyle borderStyle, double thickness, double dpi, bool useLayoutRounding)
        {
            var pen = new Pen
            {
                Brush = brush,
                DashCap = PenLineCap.Flat,
                Thickness = useLayoutRounding ? RoundLayoutValue(thickness, dpi) : thickness,
            };

            switch (borderStyle)
            {
                case BorderStyle.Dotted:
                    pen.DashStyle = new DashStyle(new double[] { 1 }, 0d);
                    break;
                case BorderStyle.Dashed:
                    pen.DashStyle = new DashStyle(new double[] { 4, 2 }, 0d);
                    break;
                default:
                    break;
            }

            if (brush.IsFrozen)
            {
                pen.Freeze();
            }

            return pen;
        }

        // TODO 迁移到独立的帮助类里
        private static double RoundLayoutValue(double value, double dpiScale)
        {
            double newValue;

            // If DPI == 1, don't use DPI-aware rounding.
            if (!dpiScale.IsCloseTo(1.0))
            {
                newValue = Math.Round(value * dpiScale) / dpiScale;
                // If rounding produces a value unacceptable to layout (NaN, Infinity or MaxValue), use the original value.
                if (double.IsNaN(newValue) ||
                    double.IsInfinity(newValue) ||
                    newValue.IsCloseTo(double.MaxValue))
                {
                    newValue = value;
                }
            }
            else
            {
                newValue = Math.Round(value);
            }

            return newValue;
        }

        #endregion

        #region Private Structures Classes

        private struct Radii
        {
            #region Fields

            internal readonly Point LeftTop;

            internal readonly Point LeftBottom;

            internal readonly Point TopLeft;

            internal readonly Point TopRight;

            internal readonly Point RightTop;

            internal readonly Point RightBottom;

            internal readonly Point BottomRight;

            internal readonly Point BottomLeft;

            #endregion

            internal Radii(Rect rect, CornerRadius radius, Thickness borders, bool outer)
            {
                var left = borders.Left * 0.5;
                var top = borders.Top * 0.5;
                var right = borders.Right * 0.5;
                var bottom = borders.Bottom * 0.5;

                LeftTop = new Point(0d, 0d);
                LeftBottom = new Point(0d, rect.Height);

                TopLeft = new Point(0d, 0d);
                TopRight = new Point(rect.Width, 0d);

                RightTop = new Point(rect.Width, 0d);
                RightBottom = new Point(rect.Width, rect.Height);

                BottomRight = new Point(rect.Width, rect.Height);
                BottomLeft = new Point(0d, rect.Height);

                if (outer)
                {
                    LeftTop.X = left;
                    LeftBottom.X = left;

                    TopLeft.Y = top;
                    TopRight.Y = top;

                    RightTop.X -= right;
                    RightBottom.X -= right;

                    BottomLeft.Y -= bottom;
                    BottomRight.Y -= bottom;

                    if (!radius.TopLeft.IsZero())
                    {
                        TopLeft.X = radius.TopLeft; // + left;
                        LeftTop.Y = radius.TopLeft;// + top;
                    }

                    if (!radius.TopRight.IsZero())
                    {
                        RightTop.Y = radius.TopRight;// + top;
                        TopRight.X -= radius.TopRight;// + right;
                    }

                    if (!radius.BottomRight.IsZero())
                    {
                        BottomRight.X -= radius.BottomRight;// + right;
                        RightBottom.Y -= radius.BottomRight;// + bottom; ;
                    }

                    if (!radius.BottomLeft.IsZero())
                    {
                        LeftBottom.Y -= radius.BottomLeft; // + bottom;
                        BottomLeft.X = radius.BottomLeft;// + left;
                    }
                } else
                {
                    TopLeft.X = Math.Max(0.0, radius.TopLeft - left);
                    LeftTop.Y = Math.Max(0.0, radius.TopLeft - top);

                    RightTop.Y = Math.Max(0.0, radius.TopRight - top);
                    TopRight.X -= Math.Max(0.0, radius.TopRight - right);

                    BottomRight.X -= Math.Max(0.0, radius.BottomRight - right);
                    RightBottom.Y -= Math.Max(0.0, radius.BottomRight - bottom);

                    LeftBottom.Y -= Math.Max(0.0, radius.BottomLeft - bottom);
                    BottomLeft.X = Math.Max(0.0, radius.BottomLeft - left);
                }

                //  check keypoints for overlap and resolve by partitioning corners according to
                //  the percentage of each one.  

                //  top edge
                if (TopLeft.X > TopRight.X)
                {
                    var v = TopLeft.X / (TopLeft.X + rect.Width - TopRight.X) * rect.Width;
                    TopLeft.X = v;
                    TopRight.X = v;
                }

                //  right edge
                if (RightTop.Y > RightBottom.Y)
                {
                    var v = RightTop.Y / (RightTop.Y + rect.Height - RightBottom.Y) * rect.Height;
                    RightTop.Y = v;
                    RightBottom.Y = v;
                }

                //  bottom edge
                if (BottomRight.X < BottomLeft.X)
                {
                    var v = BottomLeft.X / (BottomLeft.X + rect.Width - BottomRight.X) * rect.Width;
                    BottomRight.X = v;
                    BottomLeft.X = v;
                }

                // left edge
                if (LeftBottom.Y < LeftTop.Y)
                {
                    var v = LeftTop.Y / (LeftTop.Y + rect.Height - LeftBottom.Y) * rect.Height;
                    LeftBottom.Y = v;
                    LeftTop.Y = v;
                }

                // Apply offset
                var offset = new Vector(rect.TopLeft.X, rect.TopLeft.Y);

                LeftTop += offset;
                LeftBottom += offset;

                TopRight += offset;
                TopLeft += offset;

                RightTop += offset;
                RightBottom += offset;

                BottomRight += offset;
                BottomLeft += offset;
            }
        }

        #endregion Private Structures Classes
    }
}
