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

        #endregion

        #region Properties

        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
            "Background",
            typeof(Brush),
            typeof(Border),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

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
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

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
                 FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender
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
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

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

            // Compute the total size required
            var borderSize = borders.CollapseThickness();
            var paddingSize = Padding.CollapseThickness();

            // Does the ClipBorder have a child?
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
                // Since there is no child, the border requires only the size occupied by the BorderThickness
                // and the Padding
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
            var boundRect = new Rect(finalSize);
            var innerRect = boundRect.Deflate(borders);
            var corners = CornerRadius;

            //  calculate border rendering geometry
            if (!boundRect.Width.IsZero() && !boundRect.Height.IsZero())
            {
                var outerRadii = new Radii(corners, borders, new Thickness(), true);
                var borderGeometry = new StreamGeometry();

                using (var ctx = borderGeometry.Open())
                {
                    GenerateGeometry(ctx, boundRect, outerRadii);
                }

                // Freeze the geometry for better perfomance
                borderGeometry.Freeze();
                borderGeometryCache = borderGeometry;
            }
            else
            {
                borderGeometryCache = null;
            }

            //  calculate background rendering geometry
            if (!innerRect.Width.IsZero() && !innerRect.Height.IsZero())
            {
                var innerRadii = new Radii(corners, borders, new Thickness(), false);
                var backgroundGeometry = new StreamGeometry();

                using (var ctx = backgroundGeometry.Open())
                {
                    GenerateGeometry(ctx, innerRect, innerRadii);
                }

                // Freeze the geometry for better perfomance
                backgroundGeometry.Freeze();
                backgroundGeometryCache = backgroundGeometry;
            }
            else
            {
                backgroundGeometryCache = null;
            }

            //  Arrange the Child and set its clip
            var child = Child;
            if (child != null)
            {
                var padding = Padding;
                var childRect = innerRect.Deflate(padding);
                child.Arrange(childRect);
                // Calculate the Clipping Geometry
                var clipGeometry = new StreamGeometry();
                var childRadii = new Radii(corners, borders, padding, false);
                using (var ctx = clipGeometry.Open())
                {
                    GenerateGeometry(ctx, new Rect(0, 0, childRect.Width, childRect.Height), childRadii);
                }

                // Freeze the geometry for better perfomance
                clipGeometry.Freeze();
                // Apply the clip to the Child
                // child.Clip = clipGeometry;
            }

            return finalSize;
        }

        protected override void OnRender(DrawingContext dc)
        {
            var borders = BorderThickness;
            var borderBrush = BorderBrush;
            var bgBrush = Background;
            var borderGeometry = borderGeometryCache;
            var backgroundGeometry = backgroundGeometryCache;

            if ((borderBrush != null) && (!borders.IsZero()))
            {
                // If both Border and Background are valid
                if (bgBrush != null)
                {
                    // If both the background and border brushes are same,
                    // just draw the filled borderGeometry
                    if (borderBrush.IsEqualTo(bgBrush))
                    {
                        dc.DrawGeometry(borderBrush, GetPen(), borderGeometry);
                    }
                    // If both are opaque SolidColorBrushes, first draw the borderGeometry filled
                    // with borderbrush and then draw the backgroundGeometry filled with background brush
                    else if (borderBrush.IsOpaqueSolidColorBrush() && bgBrush.IsOpaqueSolidColorBrush())
                    {
                        dc.DrawGeometry(borderBrush, GetPen(), borderGeometry);
                        dc.DrawGeometry(bgBrush, null, backgroundGeometry);
                    }
                    // If only the border is opaque, then first draw the borderGeometry filled with
                    // background brush and then draw ONLY the borderOutlineGeometry (obtained by excluding 
                    // backgroundGeometry from borderGeometry) with the border brush.
                    // This will prevent gaps between the border and the background while rendering.
                    else if (borderBrush.IsOpaqueSolidColorBrush())
                    {
                        if ((borderGeometry == null) || (backgroundGeometry == null))
                            return;

                        var borderOutlinePath = borderGeometry.GetOutlinedPathGeometry();
                        var backgroundOutlinePath = backgroundGeometry.GetOutlinedPathGeometry();
                        var borderOutlineGeometry = Geometry.Combine(borderOutlinePath, backgroundOutlinePath,
                            GeometryCombineMode.Exclude, null);

                        dc.DrawGeometry(bgBrush, null, borderGeometry);
                        dc.DrawGeometry(borderBrush, GetPen(), borderOutlineGeometry);
                    }
                    // If none of the above, then it means that the border and the background must be separately drawn.
                    // This might result in small gaps between the border and the background
                    // Draw the borderOutlineGeometry and backgroundGeometry separately with their respective brushes
                    else
                    {
                        if ((borderGeometry == null) || (backgroundGeometry == null))
                            return;

                        var borderOutlinePath = borderGeometry.GetOutlinedPathGeometry();
                        var backgroundOutlinePath = backgroundGeometry.GetOutlinedPathGeometry();
                        var borderOutlineGeometry = Geometry.Combine(borderOutlinePath, backgroundOutlinePath,
                            GeometryCombineMode.Exclude, null);

                        dc.DrawGeometry(borderBrush, GetPen(), borderOutlineGeometry);
                        dc.DrawGeometry(bgBrush, null, backgroundGeometry);
                    }

                    return;
                }

                // Only Border is valid
                if ((borderGeometry != null) && (backgroundGeometry != null))
                {
                    var borderOutlinePath = borderGeometry.GetOutlinedPathGeometry();
                    var backgroundOutlinePath = backgroundGeometry.GetOutlinedPathGeometry();
                    var borderOutlineGeometry = Geometry.Combine(borderOutlinePath, backgroundOutlinePath,
                        GeometryCombineMode.Exclude, null);

                    dc.DrawGeometry(borderBrush, null, borderOutlineGeometry);
                }
                else
                {
                    dc.DrawGeometry(borderBrush, null, borderGeometry);
                }
            }

            // Only Background is valid
            if (bgBrush != null)
            {
                dc.DrawGeometry(bgBrush, null, backgroundGeometry);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Generates a StreamGeometry.
        /// </summary>
        /// <param name="ctx">An already opened StreamGeometryContext.</param>
        /// <param name="rect">Rectangle for geomentry conversion.</param>
        /// <param name="radii">The core points of the border which needs to be used to create
        /// the geometry</param>
        /// <returns>Result geometry.</returns>
        private static void GenerateGeometry(StreamGeometryContext ctx, Rect rect, Radii radii)
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
            ctx.BeginFigure(topLeft, true /* is filled */, true /* is closed */);

            // Top line
            ctx.LineTo(topRight, true /* is stroked */, false /* is smooth join */);

            // Upper-right corners
            var radiusX = rect.TopRight.X - topRight.X;
            var radiusY = rightTop.Y - rect.TopRight.Y;
            if (!radiusX.IsZero() || !radiusY.IsZero())
            {
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

        private Pen GetPen()
        {
            DoubleCollection dashes;

            switch (BorderStyle)
            {
                case BorderStyle.Dotted:
                    dashes = new DoubleCollection { 1d };
                    break;
                case BorderStyle.Dashed:
                    dashes = new DoubleCollection { 4d, 2d };
                    break;
                default:
                    return null;
            }

            return new Pen
            {
                DashStyle = new DashStyle(dashes, 0d)
            };
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

            internal Radii(CornerRadius radii, Thickness borders, Thickness padding, bool outer)
            {
                var left = 0.5 * borders.Left + padding.Left;
                var top = 0.5 * borders.Top + padding.Top;
                var right = 0.5 * borders.Right + padding.Right;
                var bottom = 0.5 * borders.Bottom + padding.Bottom;

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
