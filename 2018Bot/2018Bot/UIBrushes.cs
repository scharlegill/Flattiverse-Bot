using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using D2D = SharpDX.Direct2D1;

namespace _2018Bot
{
    public class UIBrushes
    {
        public D2D.Brush White;
        public D2D.Brush Green;
        public D2D.Brush GreenYellow;
        public D2D.Brush Yellow;
        public D2D.Brush Orange;
        public D2D.Brush Red;
        public D2D.Brush DarkRed;
        public D2D.Brush Blue;
        public D2D.Brush Cyan;
        public D2D.Brush Gray;

        public UIBrushes(WindowRenderTarget renderTarget)
        {
            White = new D2D.SolidColorBrush(renderTarget, new SharpDX.Mathematics.Interop.RawColor4(1f, 1f, 1f, 1f));
            Green = new D2D.SolidColorBrush(renderTarget, new SharpDX.Mathematics.Interop.RawColor4(0f, 1f, 0f, 1f));
            GreenYellow = new D2D.SolidColorBrush(renderTarget, new SharpDX.Mathematics.Interop.RawColor4(0.6431373f, 1f, 0f, 1f));
            Yellow = new D2D.SolidColorBrush(renderTarget, new SharpDX.Mathematics.Interop.RawColor4(1f, 1f, 0f, 1f));
            Orange = new D2D.SolidColorBrush(renderTarget, new SharpDX.Mathematics.Interop.RawColor4(1f, 0.5f, 0f, 1f));
            Red = new D2D.SolidColorBrush(renderTarget, new SharpDX.Mathematics.Interop.RawColor4(1f, 0f, 0f, 1f));
            DarkRed = new D2D.SolidColorBrush(renderTarget, new SharpDX.Mathematics.Interop.RawColor4(0.75f, 0f, 0f, 1f));
            Blue = new D2D.SolidColorBrush(renderTarget, new SharpDX.Mathematics.Interop.RawColor4(0f, 0.5f, 1f, 1f));
            Cyan = new D2D.SolidColorBrush(renderTarget, new SharpDX.Mathematics.Interop.RawColor4(0f, 1f, 1f, 1f));
            Gray = new D2D.SolidColorBrush(renderTarget, new SharpDX.Mathematics.Interop.RawColor4(0.6666667f, 0.6666667f, 0.6666667f, 1f));

        }

        public D2D.SolidColorBrush GetCustomColorBrush(Color color, RenderTarget renderTarget)
        {
            return new D2D.SolidColorBrush(renderTarget, new SharpDX.Color(color.R, color.G, color.B));
        }
    }
}
