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

        private Geometry definingGeometry;

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

        public static readonly DependencyProperty BackgroundProperty =
         TextElement.BackgroundProperty.AddOwner(
                 typeof(Icon),
                 new FrameworkPropertyMetadata(
                         Brushes.Transparent,
                         FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// The Background property defines the brush used to fill the content area.
        /// </summary>
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get the geometry that defines this icon.
        /// </summary>
        protected Geometry DefiningGeometry
        {
            get
            {
                if (definingGeometry == null)
                {
                    if (!string.IsNullOrEmpty(Type))
                    {
                        var key = "anticon." + Type;

                        // With theme suffix.
                        if (Theme == IconTheme.Filled)
                        {
                            key += ".fill";
                        } else if (Theme == IconTheme.Colorful)
                        {
                            key += ".colorful";
                        }

                        definingGeometry = TryFindResource(key) as Geometry ?? Geometry.Empty;
                    }
                    else
                    {
                        definingGeometry = Geometry.Empty;
                    }
                }

                return definingGeometry;
            }
        }

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(string), typeof(Icon),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnSpinChanged));

        /// <summary>
        /// Gets/sets the type of the ant design icon.
        /// </summary>
        public string Type
        {
            get { return (string)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public static readonly DependencyProperty ThemeProperty =
            DependencyProperty.Register("Theme", typeof(IconTheme), typeof(Icon), 
                new FrameworkPropertyMetadata(IconTheme.Outlined, FrameworkPropertyMetadataOptions.AffectsRender));


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

        #region Attached Propperties

        /// <summary>
        /// Why is it not a dependency property?
        /// icons are introduced by way of resources. if you define them as dependency properties, you will lose more flexibility.
        /// For example, each icon needs to use different stretch parameters.
        /// </summary>
        public static readonly DependencyProperty ViewBoxProperty =
            DependencyProperty.RegisterAttached("ViewBox", typeof(Rect), typeof(Icon), new PropertyMetadata(new Rect(0, 0, 1024, 1024)), OnViewBoxValidate);

        private static bool OnViewBoxValidate(object value)
        {
            var viewBox = (Rect)value;
            return viewBox.IsEmpty || (viewBox.Width >= 0 && viewBox.Height >= 0);
        }

        /// <summary>
        /// Get the rectangular area of the geometric stretch.
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(Geometry))]
        public static Rect GetViewBox(DependencyObject obj)
        {
            return (Rect)obj.GetValue(ViewBoxProperty);
        }

        /// <summary>
        /// Set a rectangular area for geometric stretch.
        /// </summary>
        public static void SetViewBox(DependencyObject obj, Rect value)
        {
            obj.SetValue(ViewBoxProperty, value);
        }

        /// <summary>
        /// When you need colorful icons, you need to be able to support custom brushes.
        /// </summary>
        public static readonly DependencyProperty FillProperty =
            DependencyProperty.RegisterAttached("Fill", typeof(Brush), typeof(Icon), new PropertyMetadata(null));

        /// <summary>
        /// Get the brush that fill the geometry.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [AttachedPropertyBrowsableForType(typeof(Geometry))]
        public static Brush GetFill(DependencyObject obj)
        {
            return (Brush)obj.GetValue(FillProperty);
        }

        /// <summary>
        /// Set the brush to fill the geometry. Valid when Theme is colorful.
        /// </summary>
        public static void SetFill(DependencyObject obj, Brush value)
        {
            obj.SetValue(FillProperty, value);
        }

        #endregion

        #region Constructors

        static Icon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Icon), new FrameworkPropertyMetadata(typeof(Icon)));
        }

        public Icon()
        {
            Loaded += (s, e) => SetSpinAnimation();
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Notification that a specified property has been invalidated.
        /// </summary>
        /// <param name="e">EventArgs that contains the property, metadata, old value, and new value for this change</param>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue && (e.Property == TypeProperty || e.Property == ThemeProperty))
            {
                // Reset definition geometry.
                definingGeometry = null;
            }

            base.OnPropertyChanged(e);
        }

        /// <summary>
        /// Updates DesiredSize of the icon.  Called by parent UIElement during is the first pass of layout.
        /// </summary>
        /// <param name="constraint">Constraint size is an "upper limit" that should not exceed.</param>
        /// <returns>icon desired size.</returns>
        protected override SizeBase MeasureOverride(SizeBase constraint)
        {
            return GetRenderSize(constraint, FontSize);
        }

        /// <summary>
        /// Compute the rendered geometry.
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        protected override SizeBase ArrangeOverride(SizeBase finalSize)
        {
            return GetRenderSize(finalSize, FontSize);
        }

        /// <summary>
        /// Render callback.
        /// </summary>
        protected override void OnRender(DrawingContext dc)
        {
            Geometry rendered;
            var geometry   = DefiningGeometry;

            Debug.Assert(geometry != null);

            var foreground = Foreground;
            var matrix     = GetStretchMatrix(geometry, FontSize);

            // Need to use colorful render.
            if (geometry is GeometryGroup)
            {
                Brush brush;
                int index        = 0;
                var isSolidColor = foreground is SolidColorBrush;
                var children     = ((GeometryGroup)geometry).Children;
        
                foreach (var child in children)
                {
                    rendered = GetRenderedGeometry(child, matrix);

                    if (rendered != Geometry.Empty)
                    {
                        brush = rendered.GetValue(FillProperty) as Brush;

                        // It may need to be tinted
                        if (brush == null)
                        {
                            if (!isSolidColor || index == 0 || index == 6 || index > 9)
                            {
                                brush = foreground;
                            }else
                            {
                                brush = new SolidColorBrush(ColorPalette.Toning(((SolidColorBrush)foreground).Color, index));
                            }

                            index++;
                        }

                        dc.DrawGeometry(brush, null, rendered);
                    }
                }

            } else
            {
                rendered = GetRenderedGeometry(geometry, matrix);

                if (rendered != Geometry.Empty)
                {
                    dc.DrawGeometry(foreground, null, rendered);
                }
            }

            // Without background, the mouse can penetrate geometry and cause event failure.
            var background = Background;

            if (background != null)
            {
                dc.DrawRectangle(background, null, new Rect(0, 0, RenderSize.Width, RenderSize.Height));
            }
        }

        #endregion

        #region Private Methods

        private void SetSpinAnimation()
        {
            var spin = Spin ?? Type == "loading";

            if (spin)
            {
                this.BeginSpin(1d);
            }
            else
            {
                this.StopSpin();
            }
        }

        private SizeBase GetRenderSize(SizeBase availableSize, double fontSize)
        {
            if (IsGeometryEmpty(DefiningGeometry))
            {
                return new SizeBase(0d, 0d);
            }

            return new SizeBase(Math.Min(availableSize.Width, fontSize), Math.Min(availableSize.Height, fontSize));
        }

        private bool IsGeometryEmpty(Geometry geometry)
        {
            return geometry.IsEmpty() || geometry.Bounds.IsEmpty;
        }

        /// <summary>
        /// Get the rendered geometry.
        /// </summary>
        private Geometry GetRenderedGeometry(Geometry geometry, Matrix matrix)
        {
            var rendered = geometry.CloneCurrentValue();

            if (ReferenceEquals(geometry, rendered))
            {
                rendered = rendered.Clone();
            }

            var transform = rendered.Transform;

            if (transform == null)
            {
                rendered.Transform = new MatrixTransform(matrix);
            }
            else
            {
                rendered.Transform = new MatrixTransform(transform.Value * matrix);
            }

            return rendered;
        }

        /// <summary>
        /// Get the stretch matrix of the geometry.
        /// </summary>
        private Matrix GetStretchMatrix(Geometry geometry, double size)
        {
            var matrix  = Matrix.Identity;

            if (!IsGeometryEmpty(geometry))
            {
                double scaleX, scaleY;
                var viewBox = (Rect)geometry.GetValue(ViewBoxProperty);

                if (viewBox.IsEmpty)
                {
                    viewBox = geometry.Bounds;
                    scaleX  = size / viewBox.Right;
                    scaleY  = size / viewBox.Bottom;

                    if (scaleX > scaleY)
                    {
                        scaleX = scaleY;
                    } else
                    {
                        scaleY = scaleX;
                    }

                } else
                {
                    scaleX = size / viewBox.Width;
                    scaleY = size / viewBox.Height;
                    matrix.Translate(-(scaleX * viewBox.X), -(scaleY * viewBox.Y));
                }

                matrix.Scale(scaleX, scaleY);
            }

            return matrix;
        }

        #endregion
    }

    public enum IconTheme : byte
    {
        Filled, Outlined, Colorful
    }
}
