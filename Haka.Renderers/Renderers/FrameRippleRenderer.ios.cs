using CoreAnimation;
using CoreGraphics;
using Foundation;
using Haka.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(FrameRipple), typeof(FrameRippleRenderer))]

namespace Haka.Renderers
{
    public class FrameRippleRenderer : FrameRenderer
    {
        UIView rippleContainer = null;
        UIView ripple = null;
        bool isUpdateCornerRadius;

        FrameRipple FrameRipple => Element as FrameRipple;

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var tapDetector = new UITapGestureRecognizer(() =>
                {
                    FrameRipple.Tap?.Invoke();
                });

                NativeView.AddGestureRecognizer(tapDetector);
            }
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            RippleEffect(touches);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (!isUpdateCornerRadius)
            {
                isUpdateCornerRadius = true;
                UpdateCornerRadius();
            }
        }

        void RippleEffect(NSSet touches)
        {
            var touch = touches.AnyObject as UITouch;
            var center = touch.LocationInView(NativeView);

            var dimension = Element.Height > Element.Width
                ? (float)Element.Height
                : (float)Element.Width;

            ripple = ripple ?? new UIView(new CGRect(0, 0, dimension, dimension));
            rippleContainer = rippleContainer ?? new UIView(new CGRect(0, 0, Element.Width, Element.Height));

            ripple.Alpha = .5f;
            ripple.Layer.CornerRadius = dimension * .5f;
            ripple.BackgroundColor = FrameRipple.RippleColor.ToUIColor();
            ripple.Center = center;
            ripple.Transform = CGAffineTransform.MakeScale(0, 0);

            rippleContainer.ClipsToBounds = true;
            rippleContainer.InsertSubview(ripple, 0);

            NativeView.InsertSubview(rippleContainer, 1);

            var scale = 4.0f;

            Animate(.6f,
                () =>
                {
                    ripple.Transform = CGAffineTransform.MakeScale(scale, scale);
                    ripple.Alpha = 0;
                },
                () =>
                {
                    rippleContainer.RemoveFromSuperview();
                    ripple.RemoveFromSuperview();
                });
        }

        void UpdateCornerRadius()
        {
            var cornerRadius = FrameRipple?.CornerRadius;

            if (!cornerRadius.HasValue) return;

            var roundedCornerRadius = RetrieveCommonCornerRadius(cornerRadius.Value);

            if (roundedCornerRadius <= 0) return;

            var roundedCorners = RetrieveRoundedCorners(cornerRadius.Value);

            var pathCorners = UIBezierPath.FromRoundedRect(Bounds, roundedCorners, new CGSize(roundedCornerRadius, roundedCornerRadius));

            var mask = new CAShapeLayer
            {
                Path = pathCorners.CGPath
            };

            NativeView.Layer.Mask = mask;

            Layer.BackgroundColor = FrameRipple.BackgroundColor.ToCGColor();

            var shadowLayer = new CAShapeLayer
            {
                Path = pathCorners.CGPath,
                Frame = NativeView.Frame,
                StrokeColor = FrameRipple.BorderColor.ToCGColor(),
                LineWidth = FrameRipple.BorderWidth,
                FillColor = UIColor.White.CGColor
            };

            Element.BorderColor = Color.Transparent;

            //Shadow
            var elevation = FrameRipple.Elevation / 10;
            shadowLayer.ShadowColor = UIColor.DarkGray.CGColor;
            shadowLayer.ShadowOpacity = 0.2f;
            shadowLayer.ShadowRadius = elevation * 4.0f;
            shadowLayer.ShadowOffset = new CGSize(0, elevation);

            NativeView.Superview.Layer.InsertSublayerBelow(shadowLayer, NativeView.Layer);

            var positionX = Layer.Position.X - FrameRipple.AdjustShadowPosition;
            var positionY = Layer.Position.Y - FrameRipple.AdjustShadowPosition;

            shadowLayer.Position = new CGPoint(positionX, positionY);
        }

        UIRectCorner RetrieveRoundedCorners(Thickness cornerRadius)
        {
            var roundedCorners = default(UIRectCorner);

            if (cornerRadius.Left > 0)
            {
                roundedCorners |= UIRectCorner.TopLeft;
            }

            if (cornerRadius.Top > 0)
            {
                roundedCorners |= UIRectCorner.TopRight;
            }

            if (cornerRadius.Right > 0)
            {
                roundedCorners |= UIRectCorner.BottomRight;
            }

            if (cornerRadius.Bottom > 0)
            {
                roundedCorners |= UIRectCorner.BottomLeft;
            }

            return roundedCorners;
        }

        double RetrieveCommonCornerRadius(Thickness cornerRadius)
        {
            var commonCornerRadius = cornerRadius.Left;
            if (commonCornerRadius <= 0)
            {
                commonCornerRadius = cornerRadius.Top;
                if (commonCornerRadius <= 0)
                {
                    commonCornerRadius = cornerRadius.Bottom;
                    if (commonCornerRadius <= 0)
                    {
                        commonCornerRadius = cornerRadius.Right;
                    }
                }
            }

            return commonCornerRadius;
        }
    }
}
