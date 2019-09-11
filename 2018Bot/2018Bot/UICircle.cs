using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D2D = SharpDX.Direct2D1;

namespace _2018Bot
{
    public static class UICircle
    {

        public static void DrawCircle(WindowRenderTarget renderTarget, float radius, SharpDX.Mathematics.Interop.RawVector2 center, D2D.Brush brush, float strokeWidth = 1f, StrokeStyle strokeStyle = null)
        {
            Ellipse ellipse = new Ellipse(center, radius, radius);

            if (strokeStyle == null)
                renderTarget.DrawEllipse(ellipse, brush, strokeWidth);
            else
                renderTarget.DrawEllipse(ellipse, brush, strokeWidth, strokeStyle);
        }
    }
}
