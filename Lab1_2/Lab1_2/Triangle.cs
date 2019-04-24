using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Lab1_2
{
    class CTriangle : Shape
    {
        //public override void Draw(Bitmap bitmap, Point[] points)
        public override void Draw(Bitmap bitmap)
        {
            base.Draw(bitmap);
            paint.DrawPolygon(pen, base.Coords);
        }
    }
}
