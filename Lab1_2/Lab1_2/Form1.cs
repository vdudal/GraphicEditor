using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Lab1_2
{
    public partial class GraphicEditorForm : Form
    {
        public static Color brushColor = Color.Black;
        private Shape shape;
        public Point[] point = new Point[0];
        private static Bitmap BitmapMain, BitmapBuff;
        public static int penWidth;
        public PictureBox pictureBox;
        private Dictionary<string, Shape> instr = new Dictionary<string, Shape>();
        private List<Shape> allFigs = new List<Shape>();
        private Stack<Bitmap> undo = new Stack<Bitmap>();
        private Stack<Bitmap> redo = new Stack<Bitmap>();

        public GraphicEditorForm()
        {
            instr.Add("Линия", new CLine());
            instr.Add("Прямоугольник", new CRectangle());
            instr.Add("Квадрат", new CSquare());
            instr.Add("Треугольник", new CTriangle());
            instr.Add("Круг", new CCircle());
            instr.Add("Овал", new CEllipse());
            InitializeComponent();
            pictureBox = canvas;
            BitmapMain = new Bitmap(canvas.Width, canvas.Height);
            BitmapBuff = new Bitmap(canvas.Width, canvas.Height);
            
        }

            private void changeColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                brushColor = colorDialog.Color;
            }
            Array.Resize(ref point, 0);
        }

        private void canvas_MouseClick(object sender, MouseEventArgs e)
        {
            int i = 1;
            Array.Resize(ref point, point.Length + 1);
            point[point.Length - 1].X = e.X;
            point[point.Length - 1].Y = e.Y;
            var copy = BitmapBuff.Clone(new Rectangle(0, 0, BitmapBuff.Width, BitmapBuff.Height), BitmapBuff.PixelFormat);
            undo.Push(copy);
            BitmapMain = new Bitmap(BitmapBuff);
            pictureBox.Image = BitmapMain;
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            // выбираем количество точек
            // в треугольнике - 3, в остальных - 2
            if (figure.Text != "Треугольник")
            {
                if (point.Length > 1)
                {
                    Array.Resize(ref point, 0);
                }
            }
            else
            {
                if (point.Length > 2)
                {
                    Array.Resize(ref point, 0);
                }
            }

            if (point.Length > 0)
            {
                Array.Resize(ref point, point.Length + 1);
                point[point.Length - 1].X = e.X;
                point[point.Length - 1].Y = e.Y;
            }

            BitmapBuff = new Bitmap(BitmapMain);

            // Создайм новый элемент
            instr.TryGetValue(figure.Text, out shape);

            if (shape != null && point.Length > 1)
            {
                shape.Coords = point;
                shape.Draw(BitmapBuff);
                //shape.Draw(BitmapBuff, point);
            }

            canvas.Image = BitmapBuff;
            if (point.Length > 1)
                Array.Resize(ref point, point.Length - 1);

            //добавляем отрисованную фигуру в лист
            var newItem = shape;
            allFigs.Add(newItem);
        
        }

        // Выбор цвета
        private void color_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                brushColor = colorDialog.Color;
            }
            Array.Resize(ref point, 0);
        }

        private void отменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = 1;
            if (undo.Count > 0)
            { 
                if (i == 1) {
                    BitmapMain = undo.Pop();
                    var copyRedo = BitmapMain.Clone(new Rectangle(0, 0, BitmapMain.Width, BitmapMain.Height), BitmapMain.PixelFormat);
                    redo.Push(copyRedo);
                    i++;
                }
                else
                {
                    BitmapMain = undo.Pop();
                }
                if (undo.Count > 0)
                {
                    BitmapMain = undo.Pop();
                    var copyRedo = BitmapMain.Clone(new Rectangle(0, 0, BitmapMain.Width, BitmapMain.Height), BitmapMain.PixelFormat);
                    redo.Push(copyRedo);
                    pictureBox.Image = BitmapMain;
                }      
            }
        }

        private void вернутьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = 1;
            if (redo.Count > 0)
            {
                if (i == 1)
                {
                    BitmapMain = redo.Pop();
                    var copy = BitmapMain.Clone(new Rectangle(0, 0, BitmapMain.Width, BitmapMain.Height), BitmapMain.PixelFormat);
                    undo.Push(copy);
                    i++;
                }
                else
                {
                    BitmapMain = undo.Pop();
                }
                if (redo.Count > 0)
                {
                    BitmapMain = redo.Pop();
                    var copy = BitmapMain.Clone(new Rectangle(0, 0, BitmapMain.Width, BitmapMain.Height), BitmapMain.PixelFormat);
                    undo.Push(copy);
                    pictureBox.Image = BitmapMain;
                }
            }
        }

        private void changeWidth_ValueChanged(object sender, EventArgs e)
        {
            penWidth = (int)changeWidth.Value;
            BitmapBuff = new Bitmap(canvas.Width, canvas.Height);
            Array.Resize(ref point, 0);
        }
    }
}
