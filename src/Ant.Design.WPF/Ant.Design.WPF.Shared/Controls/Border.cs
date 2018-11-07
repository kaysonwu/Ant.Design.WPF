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

        private StreamGeometry borderGeometryCache;

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
                var dpi = this.GetDpi();
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
                var dpi = this.GetDpi();
                borders = new Thickness(RoundLayoutValue(borders.Left, dpi.DpiScaleX), RoundLayoutValue(borders.Top, dpi.DpiScaleY),
                   RoundLayoutValue(borders.Right, dpi.DpiScaleX), RoundLayoutValue(borders.Bottom, dpi.DpiScaleY));
            }

            var boundRect = new Rect(finalSize);
            var innerRect = boundRect.Deflate(borders);

            //  arrange child
            var child = Child;
            if (child != null)
            {
                Rect childRect = innerRect.Deflate(Padding);
                child.Arrange(childRect);
            }

            var radii = CornerRadius;
            useComplexRender = !radii.IsUniform() || !borders.IsUniform();

            if (useComplexRender)
            {
                // 取一个边框最大值，然后计算
                if (!boundRect.Width.IsZero() && !boundRect.Height.IsZero())
                {
                    var outerRadii = new Radii(radii, borders, true);
                    var borderGeometry = new StreamGeometry();
                    
                    using (StreamGeometryContext ctx = borderGeometry.Open())
                    {
                        GenerateGeometry(ctx, boundRect, outerRadii, borders, false);
                    }

                    borderGeometry.Freeze();
                    borderGeometryCache = borderGeometry;
                }
                else
                {
                    borderGeometryCache = null;
                }

                //    var innerRadii = new Radii(radii, borders, false);
                //    StreamGeometry backgroundGeometry = null;

                //    //  calculate border / background rendering geometry
                //    if (!innerRect.Width.IsZero() && !innerRect.Height.IsZero())
                //    {
                //        backgroundGeometry = new StreamGeometry();

                //        using (StreamGeometryContext ctx = backgroundGeometry.Open())
                //        {
                //            GenerateGeometry(ctx, innerRect, innerRadii);
                //        }

                //        backgroundGeometry.Freeze();
                //        backgroundGeometryCache = backgroundGeometry;
                //    }
                //    else
                //    {
                //        backgroundGeometryCache = null;
                //    }

            }
            else
            {
                backgroundGeometryCache = borderGeometryCache = null;
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
            var dpi = this.GetDpi();

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

        }
       
        private static void DrawArc(DrawingContext dc, Pen pen, Point startPoint, Point endPoint, Size size)
        {
            var streamGeometry = new StreamGeometry();

            using (StreamGeometryContext sc = streamGeometry.Open())
            {
                sc.BeginFigure(startPoint, true, false);
                sc.ArcTo(endPoint, size, 0, false, SweepDirection.Clockwise, true, false);
            }

            streamGeometry.Freeze();
            dc.DrawGeometry(null, pen, streamGeometry);
        }

        /// <summary>
        /// Generates a StreamGeometry.
        /// </summary>
        /// <param name="ctx">An already opened StreamGeometryContext.</param>
        /// <param name="rect">Rectangle for geomentry conversion.</param>
        /// <param name="radii">The core points of the border which needs to be used to create
        /// the geometry</param>
        /// <returns>Result geometry.</returns>
        private static void GenerateGeometry(StreamGeometryContext ctx, Rect rect, Radii radii, Thickness borderThickness, bool isClosed = true)
        {
            //  compute the coordinates of the key points
            var topLeft = new Point(radii.LeftTop, 0);
            var topRight = new Point(rect.Width - radii.RightTop, 0);
            var rightTop = new Point(rect.Width, radii.TopRight);
            var rightBottom = new Point(rect.Width, rect.Height - radii.BottomRight);
            var bottomRight = new Point(rect.Width - radii.RightBottom, rect.Height);
            var bottomLeft = new Point(radii.LeftBottom, rect.Height);
            var leftBottom = new Point(0, rect.Height - radii.BottomLeft);
            var leftTop = new Point(0, radii.TopLeft);

            //  check keypoints for overlap and resolve by partitioning corners according to
            //  the percentage of each one.  

            //  top edge
            if (topLeft.X > topRight.X)
            {
                var v = (radii.LeftTop) / (radii.LeftTop + radii.RightTop) * rect.Width;
                topLeft.X = v;
                topRight.X = v;
            }

            //  right edge
            if (rightTop.Y > rightBottom.Y)
            {
                var v = (radii.TopRight) / (radii.TopRight + radii.BottomRight) * rect.Height;
                rightTop.Y = v;
                rightBottom.Y = v;
            }

            //  bottom edge
            if (bottomRight.X < bottomLeft.X)
            {
                var v = (radii.LeftBottom) / (radii.LeftBottom + radii.RightBottom) * rect.Width;
                bottomRight.X = v;
                bottomLeft.X = v;
            }

            // left edge
            if (leftBottom.Y < leftTop.Y)
            {
                var v = (radii.TopLeft) / (radii.TopLeft + radii.BottomLeft) * rect.Height;
                leftBottom.Y = v;
                leftTop.Y = v;
            }

            // Apply offset
            var offset = new Vector(rect.TopLeft.X, rect.TopLeft.Y);
            topLeft += offset;
            topRight += offset;
            rightTop += offset;
            rightBottom += offset;
            bottomRight += offset;
            bottomLeft += offset;
            leftBottom += offset;
            leftTop += offset;

            //  create the border geometry
            ctx.BeginFigure(topLeft, true /* is filled */, isClosed /* is closed */);

            // Top line
            ctx.LineTo(topRight, true /* is stroked */, false /* is smooth join */);

            // Upper-right corners
            var radiusX = rect.TopRight.X - topRight.X;
            var radiusY = rightTop.Y - rect.TopRight.Y;
            if (!radiusX.IsZero() || !radiusY.IsZero())
            {
                Console.WriteLine("RightTop:" + topRight + "|" + rightTop);
                ctx.ArcTo(rightTop, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true, false);
            }

            // Right line
            ctx.LineTo(rightBottom, true /* is stroked */, false /* is smooth join */);

            // Lower-right corners
            radiusX = rect.BottomRight.X - bottomRight.X;
            radiusY = rect.BottomRight.Y - rightBottom.Y;
            if (!radiusX.IsZero() || !radiusY.IsZero())
            {
                ctx.ArcTo(bottomRight, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true, false);
            }

            // Bottom line
            ctx.LineTo(bottomLeft, true /* is stroked */, false /* is smooth join */);

            // Lower-left corners
            radiusX = bottomLeft.X - rect.BottomLeft.X;
            radiusY = rect.BottomLeft.Y - leftBottom.Y;
            if (!radiusX.IsZero() || !radiusY.IsZero())
            {
                ctx.ArcTo(leftBottom, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true, false);
            }

            // Left line
            ctx.LineTo(leftTop, true /* is stroked */, false /* is smooth join */);

            // Upper-left corners
            radiusX = topLeft.X - rect.TopLeft.X;
            radiusY = leftTop.Y - rect.TopLeft.Y;
            if (!radiusX.IsZero() || !radiusY.IsZero())
            {
                ctx.ArcTo(topLeft, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true, false);
            }

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

            internal readonly double LeftTop;
            internal readonly double TopLeft;
            internal readonly double TopRight;
            internal readonly double RightTop;
            internal readonly double RightBottom;
            internal readonly double BottomRight;
            internal readonly double BottomLeft;
            internal readonly double LeftBottom;

            #endregion

            internal Radii(CornerRadius radii, Thickness borders, bool outer)
            {
                var left = 0.5 * borders.Left;
                var top = 0.5 * borders.Top;
                var right = 0.5 * borders.Right;
                var bottom = 0.5 * borders.Bottom;

                if (outer)
                {
                    if (radii.TopLeft.IsZero())
                    {
                        LeftTop = TopLeft = 0.0;
                    }
                    else
                    {
                        LeftTop = radii.TopLeft + left;
                        TopLeft = radii.TopLeft + top;
                    }
                    if (radii.TopRight.IsZero())
                    {
                        TopRight = RightTop = 0.0;
                    }
                    else
                    {
                        TopRight = radii.TopRight + top;
                        RightTop = radii.TopRight + right;
                    }
                    if (radii.BottomRight.IsZero())
                    {
                        RightBottom = BottomRight = 0.0;
                    }
                    else
                    {
                        RightBottom = radii.BottomRight + right;
                        BottomRight = radii.BottomRight + bottom;
                    }
                    if (radii.BottomLeft.IsZero())
                    {
                        BottomLeft = LeftBottom = 0.0;
                    }
                    else
                    {
                        BottomLeft = radii.BottomLeft + bottom;
                        LeftBottom = radii.BottomLeft + left;
                    }
                }
                else
                {
                    LeftTop = Math.Max(0.0, radii.TopLeft - left);
                    TopLeft = Math.Max(0.0, radii.TopLeft - top);
                    TopRight = Math.Max(0.0, radii.TopRight - top);
                    RightTop = Math.Max(0.0, radii.TopRight - right);
                    RightBottom = Math.Max(0.0, radii.BottomRight - right);
                    BottomRight = Math.Max(0.0, radii.BottomRight - bottom);
                    BottomLeft = Math.Max(0.0, radii.BottomLeft - bottom);
                    LeftBottom = Math.Max(0.0, radii.BottomLeft - left);
                }
            }
        }

        #endregion Private Structures Classes
    }
}
