using Haka.Core;
using Kasay.BindableProperty;
using System;
using Xamarin.Forms;
using static System.Type;

namespace Haka.Renderers
{
    public class FrameRipple : Frame
    {
        [Bind] public int BorderWidth { get; set; }

        [Bind] public new Thickness CornerRadius { get; set; }

        [Bind] public float Elevation { get; set; }

        [Bind] public Color RippleColor { get; set; }

        public Action Tap { get; set; }

        public string OnTap { get; set; }

        public FrameRipple()
        {
            this.RegisterMethod(nameof(OnTap), nameof(Tap));

            HasShadow = false;
            Padding = 0;
            RippleColor = Color.LightGray;
        }
    }
}
