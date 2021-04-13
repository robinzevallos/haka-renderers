using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Haka.Core;
using Haka.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using FrameRenderer2 = Xamarin.Forms.Platform.Android.AppCompat.FrameRenderer;

[assembly: ExportRenderer(typeof(FrameRipple), typeof(FrameRippleRenderer))]
namespace Haka.Renderers
{
    class FrameRippleRenderer : FrameRenderer2
    {
        public FrameRipple FrameRipple => Element as FrameRipple;

        public FrameRippleRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Initialize();

                FrameRipple
                    .OnChanged(_ => _.BackgroundColor, Initialize)
                    .OnChanged(_ => _.BorderColor, Initialize)
                    .OnChanged(_ => _.BorderWidth, Initialize)
                    .OnChanged(_ => _.CornerRadius, Initialize)
                    .OnChanged(_ => _.Elevation, Initialize)
                    .OnChanged(_ => _.RippleColor, Initialize)
                    ;

                Control.Click += (s, e) =>
                {
                    FrameRipple.Tap?.Invoke();
                };
            }
        }

        void Initialize()
        {
            CardElevation = FrameRipple.Elevation;
            SetBackground(FrameRipple.RippleColor, FrameRipple.BorderWidth);
        }

        void SetCornerRadius(GradientDrawable backgroundGradient)
        {
            var cornerRadius = FrameRipple?.CornerRadius;

            if (!cornerRadius.HasValue) return;

            var topLeftCorner = Context.ToPixels(cornerRadius.Value.Left);
            var topRightCorner = Context.ToPixels(cornerRadius.Value.Top);
            var bottomLeftCorner = Context.ToPixels(cornerRadius.Value.Bottom);
            var bottomRightCorner = Context.ToPixels(cornerRadius.Value.Right);

            var cornerRadii = new[]
            {
                topLeftCorner,
                topLeftCorner,

                topRightCorner,
                topRightCorner,

                bottomRightCorner,
                bottomRightCorner,

                bottomLeftCorner,
                bottomLeftCorner,
            };

            backgroundGradient?.SetCornerRadii(cornerRadii);
        }

        void SetBackground(Color rippleColor, int borderWidth)
        {
            var backgroundColor = Element.BackgroundColor.ToAndroid();
            var enabledBackground = new GradientDrawable(
                GradientDrawable.Orientation.LeftRight, new int[] { backgroundColor, backgroundColor });

            SetCornerRadius(enabledBackground);

            enabledBackground.SetStroke(borderWidth, Element.BorderColor.ToAndroid());

            var mask = new GradientDrawable();
            mask.SetShape(ShapeType.Rectangle);
            mask.SetColor(ColorStateList.ValueOf(Android.Graphics.Color.White)); // for alpha

            SetCornerRadius(mask);

            var stateList = new StateListDrawable();

            var rippleItem = new RippleDrawable(
                ColorStateList.ValueOf(rippleColor.ToAndroid()), enabledBackground, mask);

            stateList.AddState(new[] { Android.Resource.Attribute.StateEnabled }, rippleItem);

            Control.SetBackground(null);
            Control.SetBackground(stateList);
        }
    }
}
