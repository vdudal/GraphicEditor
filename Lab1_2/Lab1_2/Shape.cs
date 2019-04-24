using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Lab1_2
{
    class Shape
    {
        public static Graphics paint;
        public Pen pen;
        private Point[] points;
        public Point[] Coords
        {
            get
            {
                return points;
            }
            set
            {
                points = value;
            }
        }

        //public virtual void Draw(Bitmap bitmap, Point[] points)
        public virtual void Draw(Bitmap bitmap)
        {
            paint = Graphics.FromImage(bitmap);
            pen = new Pen(GraphicEditorForm.brushColor);
            pen.Width = GraphicEditorForm.penWidth;
        }
    }
}
