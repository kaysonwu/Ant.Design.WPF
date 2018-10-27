using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using SizeBase = System.Windows.Size;

namespace Antd.Controls
{

    /// <summary>
    /// Semantic vector graphics.
    /// </summary>
    public class Icon : FrameworkElement, ISpinable
    {
        #region Fields

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

        #endregion

        #region Properties

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(string), typeof(Icon),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnKeyChanged));

        /// <summary>
        /// Gets/sets the type of the ant design icon.
        /// </summary>
        public string Type
        {
            get { return (string)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        private static void OnKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Icon).UpdateDefiningGeometry();
        }

        public static readonly DependencyProperty ThemeProperty =
            DependencyProperty.Register("Theme", typeof(IconTheme), typeof(Icon), 
                new FrameworkPropertyMetadata(IconTheme.Outlined, FrameworkPropertyMetadataOptions.AffectsRender, OnKeyChanged));

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

        #region ReadOnly Properties

        /// <summary>
        /// Get the geometry that defines this icon.
        /// </summary>
        protected Geometry DefiningGeometry
        {
            get; private set;
        }

        /// <summary>
        /// The RenderedGeometry property returns the final rendered geometry.
        /// </summary>
        public Geometry RenderedGeometry
        {
            get
            {
                EnsureRenderedGeometry();
                var geometry = renderedGeometry.CloneCurrentValue();

                if (geometry == null || geometry == Geometry.Empty)
                {
                    return Geometry.Empty;
                }

                // We need to return a frozen copy
                if (ReferenceEquals(geometry, renderedGeometry))
                {
                    // geometry is a reference to _renderedGeometry, so we need to copy
                    geometry = geometry.Clone();
                    geometry.Freeze();
                }

                return geometry;
            }
        }

        #endregion

        #region Attached Propperties

        public static readonly DependencyProperty ViewBoxProperty =
            DependencyProperty.RegisterAttached("ViewBox", typeof(Rect), typeof(Icon), new PropertyMetadata(new Rect(0, 0, 1024, 1024)));

        [AttachedPropertyBrowsableForType(typeof(Geometry))]
        public static Rect GetViewBox(DependencyObject obj)
        {
            return (Rect)obj.GetValue(ViewBoxProperty);
        }

        public static void SetViewBox(DependencyObject obj, Rect value)
        {
            obj.SetValue(ViewBoxProperty, value);
        }

        #endregion




        #region Constructors

        static Icon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Icon), new FrameworkPropertyMetadata(typeof(Icon)));
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Updates DesiredSize of the icon.  Called by parent UIElement during is the first pass of layout.
        /// </summary>
        /// <param name="constraint">Constraint size is an "upper limit" that should not exceed.</param>
        /// <returns>icon desired size.</returns>
        protected override SizeBase MeasureOverride(SizeBase constraint)
        {

            return GetRenderSize(constraint, FontSize, DefiningGeometry);

            //var geometryBounds = GetDefiningGeometryBounds();

            //if (geometryBounds.IsEmpty || geometryBounds.Width == 0d || geometryBounds.Height == 0d)
            //{
            //    return new SizeBase(0d, 0d);
            //}

            //var width  = Math.Min(constraint.Width, FontSize);
            //var height = Math.Min(constraint.Height, FontSize);

            //return new SizeBase(width, height);
        }

        protected override SizeBase ArrangeOverride(SizeBase finalSize)
        {
            renderedGeometry = null;
            return GetRenderSize(finalSize, FontSize, DefiningGeometry);
            //    var newSize =  GetRenderSize(FontSize, GetDefiningGeometryBounds(), out double scaleX, out double scaleY);

            //    Console.WriteLine("ArrangeOverride|" + newSize + "|" + scaleX + "|" + scaleY + "|" + Type);
            //    // reset rendered geometry
            //    renderedGeometry = null;

            //    // Construct the matrix
            //    stretchMatrix    = Matrix.Identity;
            //    stretchMatrix.ScaleAt(scaleX, scaleY, 0, 0);

            ////    stretchMatrix.Translate(-4, -1.71);

            //    return newSize;
        }

        protected override void OnRender(DrawingContext dc)
        {
            EnsureRenderedGeometry();

            if (renderedGeometry != Geometry.Empty)
            {
                dc.DrawGeometry(Foreground, null, renderedGeometry);
            }
        }

        #endregion

        #region Private Methods

        private void UpdateDefiningGeometry()
        {
            Geometry geometry = null;

            if (!string.IsNullOrEmpty(Type))
            {
                var key = "anticon." + Type;

                if (Theme == IconTheme.Filled)
                {
                    key += ".fill";
                }

                geometry = TryFindResource(key) as Geometry;
            }

            DefiningGeometry = geometry ?? Geometry.Empty;
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

        private void EnsureRenderedGeometry()
        {
            if (renderedGeometry == null)
            {
                var geometry = DefiningGeometry;
                Debug.Assert(geometry != null);

                renderedGeometry = geometry.CloneCurrentValue();

                if (ReferenceEquals(geometry, renderedGeometry))
                {
                    renderedGeometry = renderedGeometry.Clone();
                }

                var transform = renderedGeometry.Transform;
                var matrix    = GetMatrix(renderedGeometry);

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

        private bool IsGeometryEmptyOrInvalid(Geometry geometry)
        {
            return geometry.IsEmpty() || geometry.Bounds.IsEmpty;
        }

        private SizeBase GetRenderSize(SizeBase availableSize, double fontSize, Geometry geometry)
        {
            Debug.Assert(geometry != null);

            if (IsGeometryEmptyOrInvalid(geometry))
            {
                return new SizeBase(0d, 0d);
            }

            var width = Math.Min(availableSize.Width, fontSize);
            var height = Math.Min(availableSize.Height, fontSize);

          //  Console.WriteLine(width + "|" + height);
            return new SizeBase(width, height);
        }

        private Matrix GetMatrix(Geometry geometry)
        {
            var matrix  = Matrix.Identity;

            if (!IsGeometryEmptyOrInvalid(geometry))
            {
                var viewBox = (Rect)geometry.GetValue(ViewBoxProperty);
                var baseVal = Math.Max(FontSize, 0d);

                var scaleX = baseVal / viewBox.Width;
                var scaleY = baseVal / viewBox.Height;

                matrix.Scale(scaleX, scaleY);
                Console.WriteLine(-(scaleX * viewBox.X) + "|" + -(scaleY * viewBox.Y) + "|" + Type);
                matrix.Translate(-(scaleX * viewBox.X), -(scaleY * viewBox.Y));
            }

            return matrix;
        }

        #endregion

        private SizeBase GetRenderSizeA(double fontSize, Rect geometryBounds, out double scaleX, out double scaleY)
        {
            // I personally think of a geometry if its width or height is 0, then it is not a valid value.
            if (geometryBounds.IsEmpty || geometryBounds.Width == 0d || geometryBounds.Height == 0d)
            {
                scaleX = scaleY = 1;
                return new SizeBase(0d, 0d);
            }

            scaleX = scaleY = Math.Max(fontSize, 0d);

            scaleX /= 1024;
            scaleY /= 1024;


            // If there is offset, we need to subtract it.
            //Console.WriteLine(scaleY * (geometryBounds.Top / geometryBounds.Bottom) + "|" + scaleX * (geometryBounds.Left / geometryBounds.Right) + "|" + Type);
            //scaleX  = (scaleX - scaleX * (geometryBounds.Left / geometryBounds.Right)) / geometryBounds.Width;
            //scaleY  = (scaleY - scaleY * (geometryBounds.Top / geometryBounds.Bottom)) / geometryBounds.Height;

            //// icon geometry must be proportional.
            //if (scaleX > scaleY)
            //{
            //    scaleX = scaleY;
            //}
            //else // if (scaleY > scaleX)
            //{
            //    scaleY = scaleX;
            //}

            return new SizeBase(fontSize, fontSize);
        }
    }

    public enum IconTheme : byte
    {
        Filled, Outlined, TwoTone
    }
}
