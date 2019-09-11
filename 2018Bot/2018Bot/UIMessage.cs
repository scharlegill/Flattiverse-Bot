using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D2D = SharpDX.Direct2D1;

namespace _2018Bot
{
    public static class UIMessage
    {
        

        static UIMessage()
        {

        }

        public static void WriteChat(D2D.WindowRenderTarget renderTarget, Starbase starbase, SharpDX.DirectWrite.Factory textFactory, TextFormat textFormat, float height, D2D.Brush brush)
        {
            renderTarget.Clear(new SharpDX.Mathematics.Interop.RawColor4(0f, 0f, 0f, 0f));

            lock (starbase.SyncMessages)
            {
                int number = starbase.Messages.Count;
                int offset = 0;
                int showNumber = number - 10 < 0 ? 0 : number - 10;
                int maxNumber = number - showNumber < 10 ? number - showNumber : 10;

                if (number > 0)
                foreach (string message in starbase.Messages.GetRange(showNumber, maxNumber))
                {

                    using (SharpDX.DirectWrite.TextLayout layout = new SharpDX.DirectWrite.TextLayout(textFactory, message, textFormat, 500, 500))
                        renderTarget.DrawTextLayout(new SharpDX.Mathematics.Interop.RawVector2() { X = 10, Y = height - 110 + offset}, layout, brush, D2D.DrawTextOptions.Clip);

                        offset += 10;
                }
            }
        }
    }
}
