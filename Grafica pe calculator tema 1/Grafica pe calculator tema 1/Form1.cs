using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grafica_pe_calculator_tema_1
{
    public enum Pattern
    {
        a1,
        a2,
        a3,
        b1,
        b2,
        b3
    }
    public partial class Form1 : Form
    {
        int x0, x1, y0, y1; // punctul a(x0,y0),b(x1,y1)
        long tick = 0, tick1 = 0;
        Bitmap map;
        Bitmap bg_map;
        Timer aux; //Timer pentru limitarea randarii 
       

        private void MakeLine()
        {
            map = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            AlgorithmSelector(x0, y0, x1, y1, trackBar1.Value,map);
            pictureBox1.Image = map;

        }

        private void AlgorithmSelector(int x0, int y0, int x1, int y1, int thickness,Bitmap map)
        {
            if (Math.Abs(y1 - y0) < Math.Abs(x1 - x0))
            {
                if (x0 > x1)
                    DoubleStepLow(x1, y1, x0, y0,map);
                else
                    DoubleStepLow(x0, y0, x1, y1,map);
            }
            else
            {
                if (y0 > y1)
                    DoubleStepHigh(x1, y1, x0, y0, map);
                else
                    DoubleStepHigh(x0, y0, x1, y1, map);

            }
        }

        //private void PlotLineLow(int x0, int y0, int x1, int y1, int thickness, Bitmap map)
        //{
        //    int dx = x1 - x0;
        //    int dy = y1 - y0;
        //    int xi = 1;
        //    if (dx < 0)
        //    {
        //        xi = -1;
        //        dx = -dx;
        //    }
        //    int D = 2 * dx - dy;
        //    int x = x0;
        //    for (int y = y0; y < y1; y++)
        //    {
        //        for (int i = -(thickness) / 2; i <= (thickness) / 2; i++)
        //        {
        //            map.SetPixel(x + i, y, Color.Black);
        //        }
        //        if (D > 0)
        //        {
        //            x += xi;
        //            D -= 2 * dy;
        //        }
        //        D += 2 * dx;
        //    }
        //}

        //private void PlotLineHigh(int x0, int y0, int x1, int y1, int thickness, Bitmap map)
        //{
        //    int dx = x1 - x0;
        //    int dy = y1 - y0;
        //    int yi = 1;
        //    if (dy < 0)
        //    {
        //        yi = -1;
        //        dy = -dy;
        //    }
        //    int D = 2 * dy - dx;
        //    int y = y0;
        //    for (int x = x0; x < x1; x++)
        //    {
        //        for (int i = -(thickness) / 2; i <= (thickness) / 2; i++)
        //        {

        //            map.SetPixel(x, y+i, Color.Black);
                    
        //        }
        //        if (D > 0)
        //        {
        //            y += yi;
        //            D -= 2 * dx;
        //        }
        //        D += 2 * dy;
        //    }
        //}

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            aux.Start();
            x0 = e.X;
            y0 = e.Y;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (tick < tick1)
                {
                    tick = tick1;

                    // ifuri-le asta sunt pentru limitarea  x si y in zona de desenat
                    if (e.X > trackBar1.Value && e.X < pictureBox1.Width)
                        x1 = e.X;
                    else if (e.X >= pictureBox1.Width)
                        x1 = pictureBox1.Width - 1 - trackBar1.Value;
                    else
                        x1 = 1 + trackBar1.Value;
                    if (e.Y > trackBar1.Value && e.Y < pictureBox1.Height)
                        y1 = e.Y;
                    else if (e.Y >= pictureBox1.Height)
                        y1 = pictureBox1.Height - 1 - trackBar1.Value;
                    else
                        y1 = 1 + trackBar1.Value;

                    MakeLine();
                }

            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            MakeLinePermanent();
            if (e.Button == MouseButtons.Right) // Clear la zona de desenat
            {
                bg_map = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                pictureBox1.BackgroundImage = bg_map;
                map = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                pictureBox1.Image = map;
            }

        }

        private void MakeLinePermanent()
        {
            pictureBox1.BackgroundImage = bg_map;
            AlgorithmSelector(x0, y0, x1, y1, trackBar1.Value,bg_map);
            pictureBox1.BackgroundImage = bg_map;
        }

        private void DoubleStepLow(int x0, int y0, int x1, int y1, Bitmap map)// pentru panta intre 1 si -1;
        {
            int current_x, current_y, incrE, incrNE, cond, dx, dy, d;
            dx = x1 - x0;
            dy = y1 - y0;
            int sign = 1;
            if (dy<0)
            {
                sign = -1;
                dy = -dy;
            }
            current_x = x0;
            current_y = y0;
            incrE = 2 * dy;
            incrNE = 2 * (dy - dx);
            cond = 4 * dy;
            d = 4 * dy - dx;
            map.SetPixel(current_x,current_y,Color.Black);
            while (current_x < x1)
            {
                if(d < 0)
                {
                    DrawPixels(Pattern.a1, current_x, current_y, map, sign);
                    d += 2*incrE;
                }
                else
                {
                    if (d<=cond)
                    {
                        DrawPixels(Pattern.a2, current_x, current_y, map, sign);
                        current_y += 1 * sign;
                        d += incrNE;
                        d += incrE;

                    }
                    else
                    {
                        DrawPixels(Pattern.a3, current_x, current_y, map, sign);
                        current_y += 2*sign;
                        d += 2*incrNE;

                    }
                }
                current_x += 2;
            }
        }
        private void DoubleStepHigh(int x0, int y0, int x1, int y1, Bitmap map)
        {
            int current_x, current_y, incrE, incrNE, cond, dx, dy, d;
            dx = x1 - x0;
            dy = y1 - y0;
            int sign = 1;
            if (dx < 0)
            {
                sign = -1;
                dx = -dx;
            }
            current_x = x0;
            current_y = y0;
            incrE = 2 * dx;
            incrNE = 2 * (dx - dy);
            cond = 4 * dx;
            d = 4 * dx - dy;
            map.SetPixel(current_x, current_y, Color.Black);
            while (current_y < y1)
            {
                if (d < 0)
                {
                    DrawPixels(Pattern.b1, current_x, current_y, map, sign);
                    d += 2 * incrE;
                }
                else
                {
                    if (d <= cond)
                    {
                        DrawPixels(Pattern.b2, current_x, current_y, map, sign);
                        current_x += 1 * sign;
                        d += incrNE;
                        d += incrE;

                    }
                    else
                    {
                        DrawPixels(Pattern.b3, current_x, current_y, map, sign);
                        current_x += 2 * sign;
                        d += 2 * incrNE;

                    }
                }
                current_y += 2;
            }
        }
        private void DrawPixels(Pattern pattern, int x, int y, Bitmap map, int sign)
        {
            try
            {
                switch (pattern)
                {
                    case Pattern.a1:
                        map.SetPixel(x + 1, y, Color.Black);
                        map.SetPixel(x + 2, y, Color.Black);
                        break;
                    case Pattern.a2:
                        map.SetPixel(x + 1, y, Color.Black);
                        map.SetPixel(x + 2, y + (1 * sign), Color.Black);
                        break;
                    case Pattern.a3:
                        map.SetPixel(x + 1, y + (1 * sign), Color.Black);
                        map.SetPixel(x + 2, y + (2 * sign), Color.Black);
                        break;
                    case Pattern.b1:
                        map.SetPixel(x, y + 1, Color.Black);
                        map.SetPixel(x, y + 2, Color.Black);
                        break;
                    case Pattern.b2:
                        map.SetPixel(x, y + 1, Color.Black);
                        map.SetPixel(x + (1 * sign), y + 2, Color.Black);
                        break;
                    case Pattern.b3:
                        map.SetPixel(x + (1 * sign), y + 1, Color.Black);
                        map.SetPixel(x + (2 * sign), y + 2, Color.Black);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

            }
            
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            map = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            bg_map = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            aux = new Timer();
            aux.Interval = 16;
            aux.Tick += Aux_Tick;
        }

        private void Aux_Tick(object sender, EventArgs e)
        {
            tick1++;
        }
    }
}
