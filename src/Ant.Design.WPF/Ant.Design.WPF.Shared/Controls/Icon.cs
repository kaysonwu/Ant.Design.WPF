using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using SizeBase = System.Windows.Size;

namespace Antd.Controls
{

    /// <summary>
    /// Semantic vector graphics.
    /// </summary>
    [TemplateVisualState(Name = "Spun", GroupName = "SpinStates")]
    [TemplateVisualState(Name = "Unspun", GroupName = "SpinStates")]
    [TemplatePart(Name = "PART_Content", Type = typeof(ContentPresenter))]
    public class Icon : FrameworkElement, ISpinable
    {
        #region Fields

        private ContentPresenter contentPresenter;

        private Matrix stretchMatrix;

        private Geometry definingGeometry;

        private Geometry renderedGeometry = Geometry.Empty;

        #endregion

        #region Document Properties

        /// <summary>
        /// DependencyProperty for <see cref="FontSize" /> property.
        /// </summary>
        public static readonly DependencyProperty FontSizeProperty =
            TextElement.FontSizeProperty.AddOwner(typeof(Icon));

        /// <summary>
        /// The FontSize property specifies the size of the font.
        /// </summary>
        [TypeConverter(typeof(FontSizeConverter))]
        [Localizability(LocalizationCategory.None)]
        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        /// <summary>
        /// DependencyProperty setter for <see cref="FontSize" /> property.
        /// </summary>
        /// <param name="element">The element to which to write the attached property.</param>
        /// <param name="value">The property value to set</param>
        public static void SetFontSize(DependencyObject element, double value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            element.SetValue(FontSizeProperty, value);
        }

        /// <summary>
        /// DependencyProperty getter for <see cref="FontSize" /> property.
        /// </summary>
        /// <param name="element">The element from which to read the attached property.</param>
        [TypeConverter(typeof(FontSizeConverter))]
        public static double GetFontSize(DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return (double)element.GetValue(FontSizeProperty);
        }

        /// <summary>
        /// DependencyProperty for <see cref="Foreground" /> property.
        /// </summary>
        public static readonly DependencyProperty ForegroundProperty =
            TextElement.ForegroundProperty.AddOwner(typeof(Icon));

        /// <summary>
        /// The Foreground property specifies the foreground brush of an element's text content.
        /// </summary>
        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        /// <summary>
        /// DependencyProperty setter for <see cref="Foreground" /> property.
        /// </summary>
        /// <param name="element">The element to which to write the attached property.</param>
        /// <param name="value">The property value to set</param>
        public static void SetForeground(DependencyObject element, Brush value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            element.SetValue(ForegroundProperty, value);
        }

        /// <summary>
        /// DependencyProperty getter for <see cref="Foreground" /> property.
        /// </summary>
        /// <param name="element">The element from which to read the attached property.</param>
        public static Brush GetForeground(DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return (Brush)element.GetValue(ForegroundProperty);
        }

        /// <summary>
        /// DependencyProperty for <see cref="Background" /> property.
        /// </summary>
        public static readonly DependencyProperty BackgroundProperty =
            TextElement.BackgroundProperty.AddOwner(
                typeof(Icon),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// The Background property defines the brush used to fill the content area.
        /// </summary>
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        /// <summary>
        /// DependencyProperty for <see cref="Padding" /> property.
        /// </summary>
        public static readonly DependencyProperty PaddingProperty =
            Block.PaddingProperty.AddOwner(
                typeof(Icon),
                new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// The Padding property specifies the padding of the element.
        /// </summary>
        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        #endregion

        #region Properties

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(string), typeof(Icon),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnTypeChanged));

        /// <summary>
        /// Gets/sets the type of the ant design icon.
        /// </summary>
        public string Type
        {
            get { return (string)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        private static void OnTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Icon).UpdateDefiningGeometry();
        }

        public static readonly DependencyProperty ThemeProperty =
            DependencyProperty.Register("Theme", typeof(IconTheme), typeof(Icon), new PropertyMetadata(IconTheme.Outlined));

        /// <summary>
        /// Gets/sets the theme of the ant design icon.
        /// </summary>
        public IconTheme Theme
        {
            get { return (IconTheme)GetValue(ThemeProperty); }
            set { SetValue(ThemeProperty, value); }
        }

        public static readonly DependencyProperty SpinProperty =
            DependencyProperty.Register("Spin", typeof(bool?), typeof(Icon), new PropertyMetadata(null, OnSpinChanged));

        /// <summary>
        /// Gets/sets whether the icon has a spin animation.
        /// </summary>
        public bool? Spin
        {
            get { return (bool?)GetValue(SpinProperty); }
            set { SetValue(SpinProperty, value); }
        }

        private static void OnSpinChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Icon).SetSpinAnimation();
        }

        #endregion

        #region Constructors

        static Icon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Icon), new FrameworkPropertyMetadata(typeof(Icon)));
        }

        #endregion

        #region Overrides

        protected override SizeBase MeasureOverride(SizeBase availableSize)
        {
            var newSize = GetRenderSize(new SizeBase(FontSize, FontSize), GetDefiningGeometryBounds());

            if (SizeIsInvalidOrEmpty(newSize))
            {
                // We've encountered a numerical error. Don't draw anything.
                newSize = new SizeBase(0, 0);
                renderedGeometry = Geometry.Empty;
            }

            Console.WriteLine("MeasureOverride|" + newSize + "|" + Type);
            return newSize;
        }

        protected override SizeBase ArrangeOverride(SizeBase finalSize)
        {
            var newSize = GetStretchedRenderSizeAndSetStretchMatrix(finalSize, GetDefiningGeometryBounds());

            if (SizeIsInvalidOrEmpty(newSize))
            {
                // We've encountered a numerical error. Don't draw anything.
                newSize = new SizeBase(0, 0);
                renderedGeometry = Geometry.Empty;
            }
            Console.WriteLine("ArrangeOverride|" + newSize + "|" + Type);
            return newSize;
        }

        protected override void OnRender(DrawingContext dc)
        {
            EnsureRenderedGeometry();

            if (renderedGeometry != Geometry.Empty)
            {
                dc.DrawGeometry(Foreground, null, renderedGeometry);
            }

            // Draw background in rectangle.
            if (Background != null)
            {
                dc.DrawRectangle(Background, null, new Rect(0, 0, RenderSize.Width, RenderSize.Height));
            }
        }

        #endregion

        #region Private Methods

        private bool SizeIsInvalidOrEmpty(SizeBase size)
        {
            return size.IsEmpty || double.IsNaN(size.Width) || double.IsNaN(size.Height);
        }

        private void UpdateDefiningGeometry()
        {
            definingGeometry = GetGeometry();
            SetSpinAnimation();
        }

        private void SetSpinAnimation()
        {
            var spin = Spin ?? !string.IsNullOrEmpty(Type) && Type.ToLower() == "loading";

            if (spin)
            {
                this.BeginSpin(TimeSpan.FromSeconds(1d));
            }
            else
            {
                this.StopSpin();
            }
        }

        /// <summary>
        /// Get geometry by type.
        /// </summary>
        private Geometry GetGeometry()
        {
            if (string.IsNullOrEmpty(Type))
            {
                return Geometry.Empty;
            }

            // TODO Theme
            var key = "anticon." + Type;
            return (TryFindResource(key) as Geometry) ?? Geometry.Empty;
        }

        /// <summary>
        /// Get the bounds of the geometry that defines this icon.
        /// </summary>
        private Rect GetDefiningGeometryBounds()
        {
            Debug.Assert(definingGeometry != null);

            return definingGeometry.Bounds;
        }

        private SizeBase GetRenderSize(SizeBase availableSize, Rect geometryBounds)
        {
            double xScale, yScale, dX, dY;
            SizeBase renderSize;

            GetStretchMetrics(availableSize, geometryBounds,
                                out xScale, out yScale, out dX, out dY, out renderSize);

            return renderSize;
        }

        private SizeBase GetStretchedRenderSizeAndSetStretchMatrix(SizeBase availableSize, Rect geometryBounds)
        {
            double xScale, yScale, dX, dY;
            SizeBase renderSize;

            GetStretchMetrics(availableSize, geometryBounds,
                out xScale, out yScale, out dX, out dY, out renderSize);

            // Construct the matrix
            stretchMatrix = Matrix.Identity;

            Console.WriteLine(xScale + "|" + yScale + "|" + geometryBounds.Location + "|" + dX + "|" + dY);
            stretchMatrix.Scale(0.03515625, 0.03515625);
         //   stretchMatrix.Translate(0, 0);

            ResetRenderedGeometry();
            return renderSize;
        }

        private void GetMetrics(SizeBase availableSize, Rect geometryBounds)
        {
            if (!geometryBounds.IsEmpty)
            {
                var a = geometryBounds.Left / geometryBounds.Right;
                var b = geometryBounds.Top / geometryBounds.Bottom;


            }

        }

        private void GetStretchMetrics(SizeBase availableSize, Rect geometryBounds,
                                out double xScale, out double yScale, out double dX, out double dY, out SizeBase stretchedSize)
        {
            if (!geometryBounds.IsEmpty)
            {
                // double margin = strokeThickness / 2;
                bool hasThinDimension = false;

                // Initialization for mode == Fill
                xScale = Math.Max(availableSize.Width, 0);
                yScale = Math.Max(availableSize.Height, 0);
                dX = -geometryBounds.Left;
                dY = -geometryBounds.Top;

                // Compute the scale factors from the geometry to the size.
                // The scale factors are ratios, and they have already been initialize to the numerators.
                // To prevent fp overflow, we need to make sure that numerator / denomiator < limit;
                // To do that without actually deviding, we check that denominator > numerator / limit.
                // We take 1/epsilon as the limit, so the check is denominator > numerator * epsilon

                // See Dev10 bug #453150.
                // If the scale is infinite in both dimensions, return the natural size.
                // If it's infinite in only one dimension, for non-fill stretch modes we constrain the size based
                // on the unconstrained dimension.
                // If our shape is "thin", i.e. a horizontal or vertical line, we can ignore non-fill stretches.
                if (geometryBounds.Width > xScale * double.Epsilon)
                {
                    xScale /= geometryBounds.Width;
                }
                else
                {
                    xScale = 1;
                    // We can ignore uniform and uniform-to-fill stretches if we have a vertical line.
                    if (geometryBounds.Width == 0)
                    {
                        hasThinDimension = true;
                    }
                }

                if (geometryBounds.Height > yScale * double.Epsilon)
                {
                    yScale /= geometryBounds.Height;
                }
                else
                {
                    yScale = 1;
                    // We can ignore uniform and uniform-to-fill stretches if we have a horizontal line.
                    if (geometryBounds.Height == 0)
                    {
                        hasThinDimension = true;
                    }
                }

                // We are initialized for Fill, but for the other modes
                // If one of our dimensions is thin, uniform stretches are
                // meaningless, so we treat the stretch as fill.
                if (!hasThinDimension)
                {
                    if (yScale > xScale)
                    {
                        // Resize to fit the size's width
                        yScale = xScale;
                    }
                    else // if xScale >= yScale
                    {
                        // Resize to fit the size's height
                        xScale = yScale;
                    }
                }

                stretchedSize = new SizeBase(geometryBounds.Width * xScale, geometryBounds.Height * yScale);
            }
            else
            {
                dX = dY = 0;
                xScale = yScale = 1;
                stretchedSize = new SizeBase(0, 0);
            }
        }

        private void EnsureRenderedGeometry()
        {
            if (renderedGeometry == null)
            {
                renderedGeometry = definingGeometry;

                Debug.Assert(renderedGeometry != null);
                var currentValue = renderedGeometry.CloneCurrentValue();

                if (ReferenceEquals(renderedGeometry, currentValue))
                {
                    renderedGeometry = currentValue.Clone();
                }
                else
                {
                    renderedGeometry = currentValue;
                }

                var transform = renderedGeometry.Transform;
                var matrix    = (stretchMatrix == null) ? Matrix.Identity : stretchMatrix;

                if (transform == null)
                {
                    renderedGeometry.Transform = new MatrixTransform(matrix);
                }
                else
                {
                    renderedGeometry.Transform = new MatrixTransform(transform.Value * matrix);
                }
            }
        }

        private void ResetRenderedGeometry()
        {
            // reset rendered geometry
            renderedGeometry = null;
        }

        #endregion
    }

    public enum IconTheme : byte
    {
        Filled, Outlined, TwoTone
    }
}
