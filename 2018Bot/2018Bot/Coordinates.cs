using Flattiverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2018Bot
{
    public class Coordinates
    {
        public float Magnify;
        public Vector LeftMargin;
        public Vector RightMargin;
        public Vector Offset;
        public List<float> simulation = new List<float>();
        public string scrollDirection;

        private object syncScroll = new object();
        private Vector mousePosition;

        public Coordinates(Vector leftMargin, Vector rightMargin)
        {
            RightMargin = rightMargin;
            LeftMargin = leftMargin;
            Magnify = 1;
            Offset = new Vector(0, 0);
        }

        public Vector Mouse
        {
            set
            {
                mousePosition = value;
            }
        }

        public void CheckPosition(float width, float height)
        {
            if (mousePosition == null)
                return;

            float number = 10;

            if (simulation.Count > 0)
                return;

            if (mousePosition.X < 8)
                Offset.X -= number;

            if (mousePosition.X > width - 8)
                Offset.X += number;

            if (mousePosition.Y < 8)
                Offset.Y -= number;

            if (mousePosition.Y > height - 8)
                Offset.Y += number;
        }

        public Vector Translate(Vector xy)
        {
            return (xy - LeftMargin - Offset) / Magnify;
        }

        public Vector TranslateBack(Vector xy)
        {
            return xy * Magnify + LeftMargin + Offset;
        }

        public Vector Translate(Vector xy, float radius)
        {
            return (xy - LeftMargin - Offset) / Magnify;
        }

        public float TranslateRadius(float radius)
        {
            return radius / Magnify;
        }
    }
}

