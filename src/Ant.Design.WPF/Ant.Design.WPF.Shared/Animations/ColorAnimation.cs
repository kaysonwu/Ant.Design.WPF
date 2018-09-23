using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Antd.Animations
{
    /// <summary>
    /// Animates the value of a Color property using linear interpolation
    /// between two values.  The values are determined by the combination of
    /// From, To, or By values that are set on the animation.
    /// </summary>
    public partial class ColorAnimation : ColorAnimationBase
    {
        #region Data

        /// <summary>
        /// This is used if the user has specified From, To, and/or By values.
        /// </summary>
        private Color[] _keyValues;

        private AnimationType _animationType;
        private bool _isAnimationFunctionValid;

        #endregion

        #region Properties

        public static readonly DependencyProperty FromProperty = DependencyProperty.Register(
            "From", 
            typeof(object), 
            typeof(ColorAnimation), 
            new PropertyMetadata(null, OnPropertyChanged, OnCoerceValue));

        /// <summary>
        /// Gets or sets the animation's starting value.
        /// </summary>
        public Color? From
        {
            get { return (Color?)GetValue(FromProperty); }
            set { SetValue(FromProperty, value); }
        }

        public static readonly DependencyProperty ToProperty = DependencyProperty.Register(
            "To",
            typeof(object),
            typeof(ColorAnimation),
            new PropertyMetadata(null, OnPropertyChanged, OnCoerceValue));

        /// <summary>
        /// Gets or sets the animation's ending value.
        /// </summary>
        public object To
        {
            get { return GetValue(ToProperty); }
            set { SetValue(ToProperty, value); }
        }

        public static readonly DependencyProperty ByProperty = DependencyProperty.Register(
            "By",
            typeof(object),
            typeof(ColorAnimation),
            new PropertyMetadata(null, OnPropertyChanged, OnCoerceValue));

        /// <summary>
        /// Gets or sets the total amount by which the animation changes its starting value.
        /// </summary>
        public object By
        {
            get { return GetValue(ByProperty); }
            set { SetValue(ByProperty, value); }
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var  a = d as ColorAnimation;

            if (a != null)
            {
                a._isAnimationFunctionValid = false;
            }
        }

        private static object OnCoerceValue(DependencyObject d, object value)
        {
            if (value is SolidColorBrush)
            {
                return ((SolidColorBrush)value).Color;
            }

            return value;
        }

        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register("EasingFunction", typeof(IEasingFunction), typeof(ColorAnimation));

        public IEasingFunction EasingFunction
        {
            get { return (IEasingFunction)GetValue(EasingFunctionProperty); }
            set { SetValue(EasingFunctionProperty, value); }
        }

        /// <summary>
        /// If this property is set to true the animation will add its value to
        /// the base value instead of replacing it entirely.
        /// </summary>
        public bool IsAdditive
        {
            get { return (bool)GetValue(IsAdditiveProperty); }
            set { SetValue(IsAdditiveProperty, value); }
        }

        /// <summary>
        /// It this property is set to true, the animation will accumulate its
        /// value over repeats.  For instance if you have a From value of 0.0 and
        /// a To value of 1.0, the animation return values from 1.0 to 2.0 over
        /// the second reteat cycle, and 2.0 to 3.0 over the third, etc.
        /// </summary>
        public bool IsCumulative
        {
            get { return (bool)GetValue(IsCumulativeProperty); }
            set { SetValue(IsCumulativeProperty, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new ColorAnimation with all properties set to
        /// their default values.
        /// </summary>
        public ColorAnimation()
            : base() {}

        /// <summary>
        /// Creates a new ColorAnimation that will animate a
        /// Color property from its base value to the value specified
        /// by the "toValue" parameter of this constructor.
        /// </summary>
        public ColorAnimation(Color toValue, Duration duration)
            : this()
        {
            To = toValue;
            Duration = duration;
        }

        /// <summary>
        /// Creates a new ColorAnimation that will animate a
        /// Color property from its base value to the value specified
        /// by the "toValue" parameter of this constructor.
        /// </summary>
        public ColorAnimation(Color toValue, Duration duration, FillBehavior fillBehavior)
            : this()
        {
            To = toValue;
            Duration = duration;
            FillBehavior = fillBehavior;
        }

        /// <summary>
        /// Creates a new ColorAnimation that will animate a
        /// Color property from the "fromValue" parameter of this constructor
        /// to the "toValue" parameter.
        /// </summary>
        public ColorAnimation(Color fromValue, Color toValue, Duration duration)
            : this()
        {
            From = fromValue;
            To = toValue;
            Duration = duration;
        }

        /// <summary>
        /// Creates a new ColorAnimation that will animate a
        /// Color property from the "fromValue" parameter of this constructor
        /// to the "toValue" parameter.
        /// </summary>
        public ColorAnimation(Color fromValue, Color toValue, Duration duration, FillBehavior fillBehavior)
            : this()
        {
            From = fromValue;
            To = toValue;
            Duration = duration;
            FillBehavior = fillBehavior;
        }

        #endregion

        #region Freezable

        /// <summary>
        /// Creates a copy of this ColorAnimation
        /// </summary>
        /// <returns>The copy</returns>
        public new ColorAnimation Clone()
        {
            return (ColorAnimation)base.Clone();
        }

        //
        // Note that we don't override the Clone virtuals (CloneCore, CloneCurrentValueCore,
        // GetAsFrozenCore, and GetCurrentValueAsFrozenCore) even though this class has state
        // not stored in a DP.
        // 
        // We don't need to clone _animationType and _keyValues because they are the the cached 
        // results of animation function validation, which can be recomputed.  The other remaining
        // field, isAnimationFunctionValid, defaults to false, which causes this recomputation to happen.
        //

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new ColorAnimation();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates the value this animation believes should be the current value for the property.
        /// </summary>
        /// <param name="defaultOriginValue">
        /// This value is the suggested origin value provided to the animation
        /// to be used if the animation does not have its own concept of a
        /// start value. If this animation is the first in a composition chain
        /// this value will be the snapshot value if one is available or the
        /// base property value if it is not; otherise this value will be the 
        /// value returned by the previous animation in the chain with an 
        /// animationClock that is not Stopped.
        /// </param>
        /// <param name="defaultDestinationValue">
        /// This value is the suggested destination value provided to the animation
        /// to be used if the animation does not have its own concept of an
        /// end value. This value will be the base value if the animation is
        /// in the first composition layer of animations on a property; 
        /// otherwise this value will be the output value from the previous 
        /// composition layer of animations for the property.
        /// </param>
        /// <param name="animationClock">
        /// This is the animationClock which can generate the CurrentTime or
        /// CurrentProgress value to be used by the animation to generate its
        /// output value.
        /// </param>
        /// <returns>
        /// The value this animation believes should be the current value for the property.
        /// </returns>
        protected override Color GetCurrentValueCore(Color defaultOriginValue, Color defaultDestinationValue, AnimationClock animationClock)
        {
            Debug.Assert(animationClock.CurrentState != ClockState.Stopped);

            if (!_isAnimationFunctionValid)
            {
                ValidateAnimationFunction();
            }

            double progress = animationClock.CurrentProgress.Value;

            IEasingFunction easingFunction = EasingFunction;
            if (easingFunction != null)
            {
                progress = easingFunction.Ease(progress);
            }

            Color from = new Color();
            Color to = new Color();
            Color accumulated = new Color();
            Color foundation = new Color();

            switch (_animationType)
            {
                case AnimationType.Automatic:
                    from = defaultOriginValue;
                    to = defaultDestinationValue;
                    break;

                case AnimationType.From:
                    from = _keyValues[0];
                    to = defaultDestinationValue;
                    break;

                case AnimationType.To:
                    from = defaultOriginValue;
                    to = _keyValues[0];
                    break;

                case AnimationType.By:

                    // According to the SMIL specification, a By animation is
                    // always additive.  But we don't force this so that a
                    // user can re-use a By animation and have it replace the
                    // animations that precede it in the list without having
                    // to manually set the From value to the base value.
                    to = _keyValues[0];
                    foundation = defaultOriginValue;
                    break;

                case AnimationType.FromTo:

                    from = _keyValues[0];
                    to = _keyValues[1];

                    if (IsAdditive)
                    {
                        foundation = defaultOriginValue;
                    }

                    break;

                case AnimationType.FromBy:

                    from = _keyValues[0];
                    to = _keyValues[0] + _keyValues[1];

                    if (IsAdditive)
                    {
                        foundation = defaultOriginValue;
                    }

                    break;

                default:
                    Debug.Fail("Unknown animation type.");
                    break;
            }

            if (IsCumulative)
            {
                double currentRepeat = (double)(animationClock.CurrentIteration - 1);

                if (currentRepeat > 0.0)
                {
                    Color accumulator = to - from;
                    accumulated = accumulator * (Single)currentRepeat;
                }
            }

            return foundation + accumulated + from + ((to - from) * (Single)progress);
        }

        private void ValidateAnimationFunction()
        {
            _animationType = AnimationType.Automatic;
            _keyValues = null;

            if (From != null)
            {
                if (To != null)
                {
                    _animationType = AnimationType.FromTo;
                    _keyValues = new Color[2];
                    _keyValues[0] = (Color)From;
                    _keyValues[1] = (Color)To;
                }
                else if (By != null)
                {
                    _animationType = AnimationType.FromBy;
                    _keyValues = new Color[2];
                    _keyValues[0] = (Color)From;
                    _keyValues[1] = (Color)By;
                }
                else
                {
                    _animationType = AnimationType.From;
                    _keyValues = new Color[1];
                    _keyValues[0] = (Color)From;
                }
            }
            else if (To != null)
            {
                _animationType = AnimationType.To;
                _keyValues = new Color[1];
                _keyValues[0] = (Color)To;
            }
            else if (By != null)
            {
                _animationType = AnimationType.By;
                _keyValues = new Color[1];
                _keyValues[0] = (Color)By;
            }

            _isAnimationFunctionValid = true;
        }

        #endregion
    }
}
