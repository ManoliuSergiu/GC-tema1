using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace WpfApp1
{
    public enum Pattern
    {
        a1,
        a2,
        a3,
        a4,

    }
    static class Engine
    {
        static int sqSize;
        static int x, y;
        static int cx, cy;
        static int incrE, incrNE, cond, dx, dy, d, inid;
        static int lastx1, lasty1, lastx2, lasty2;
        static bool firststep;
        static bool[,] stateMap;
        internal static int dval1, dval2, condval1, condval2, incrEval1, incrEval2, incrNEval1, incrNEval2;
        internal static double cv1,cv2,cv3;

        #region graphics
        public static Image InitBackground(int x, int y, int maxx, int maxy)
        {
            InitValues(x, y);
            FormattedText t1 = new FormattedText(
                        "(0,0)",
                        System.Globalization.CultureInfo.InvariantCulture,
                        FlowDirection.LeftToRight,
                        new Typeface("Arial"),
                        20,
                        Brushes.Black,
                        96
                        );
            FormattedText t2 = new FormattedText(
                        string.Format("({0},{1})", x, y),
                        System.Globalization.CultureInfo.InvariantCulture,
                        FlowDirection.LeftToRight,
                        new Typeface("Arial"),
                        20,
                        Brushes.Black,
                        96
                        );
            x++;
            y++;
            sqSize = Math.Min((int)((maxx - t2.Width) / x), (int)((maxy - t1.Height) / y));
            DrawingGroup dg = new DrawingGroup();
            using (DrawingContext dc = dg.Open())
            {
                dc.DrawRectangle(Brushes.PeachPuff, null, new Rect(0, 0, maxx, maxy));
                for (int i = 0; i <= x; i++)
                    dc.DrawLine(new Pen(Brushes.Black, 1), new Point(i * sqSize, y * sqSize), new Point(i * sqSize, 0));
                for (int i = 0; i <= y; i++)
                    dc.DrawLine(new Pen(Brushes.Black, 1), new Point(0, i * sqSize), new Point(x * sqSize, i * sqSize));
                dc.DrawText(t1, new Point(0, y * sqSize));
                dc.DrawText(t2, new Point(x * sqSize, 0));

            }
            Image image = new Image();
            DrawingImage drawingImage = new DrawingImage(dg);
            drawingImage.Freeze();
            image.Source = drawingImage;
            return image;
        }
        public static Image Frame()
        {
            DrawingGroup dg = new DrawingGroup();
            using (DrawingContext dc = dg.Open())
            {
                dc.DrawRectangle(Brushes.Transparent, null, new Rect(0, 0, x * sqSize, y * sqSize));
                for (int i = 0; i <= x; i++)
                {
                    for (int j = 0; j <= y; j++)
                    {
                        if (stateMap[i, j])
                        {
                            dc.DrawRectangle(Brushes.Black, null, new Rect(i * sqSize, (y - j) * sqSize, sqSize, sqSize));
                        }
                    }
                }
                if (lastx1<=x&&lasty1<=y)
                {
                    dc.DrawRectangle(Brushes.Gold, null, new Rect(lastx1 * sqSize, (y - lasty1) * sqSize, sqSize, sqSize));
                }
                if (lastx2 <= x && lasty2 <= y)
                {
                    dc.DrawRectangle(Brushes.Gold, null, new Rect(lastx2 * sqSize, (y - lasty2) * sqSize, sqSize, sqSize));
                }
            }
            Image image = new Image();
            DrawingImage drawingImage = new DrawingImage(dg);
            drawingImage.Freeze();
            image.Source = drawingImage;
            return image;
        }
        #endregion
        private static void InitValues(int x, int y)
        {
            Engine.x = x;
            Engine.y = y;
            lastx1 = lastx2 = lasty1 = lasty2 = 0;
            stateMap = new bool[x + 1, y + 1];
            cx = 0;
            cy = 0;
            firststep = true;
            dx = x - cx;
            dy = y - cy;
            incrE = incrEval1 * dy - incrEval2 * dx;
            incrNE = incrNEval1 * dy - incrNEval2 * dx;
            d = dval1 * dy - dval2 * dx;
            cond = condval1 * dy - condval2 * dx;
        }
        public static void Step()
        {
            if (firststep)
            {
                firststep = false;
                stateMap[0, 0] = true;
                
            }
            else
            {
                if (d < cv1*cond)
                {
                    DrawPixels(Pattern.a1);
                    d += 2 * incrE;
                }
                else
                {
                    if (d <= cv2 * cond)
                    {
                        DrawPixels(Pattern.a2);
                        cy++;
                        d += incrNE;
                        d += incrE;

                    }
                    else
                    {
                        if (d <= cv3*cond)
                        {
                            DrawPixels(Pattern.a3);
                            cy++;
                            d += incrNE;
                            d += incrE;
                        }
                        else
                        {
                            DrawPixels(Pattern.a4);
                            cy += 2;
                            d += 2 * incrNE;
                        }

                    }
                }
                cx += 2;  
            }
        }

        private static void DrawPixels(Pattern pat)
        {
            try
            {
                switch (pat)
                {
                    case Pattern.a1:
                        stateMap[cx + 1, cy] = true;
                        lasty1 = cy;
                        stateMap[cx + 2, cy] = true;
                        lasty2 = cy;
                        break;
                    case Pattern.a2:
                        stateMap[cx + 1, cy] = true;
                        lasty1 = cy;
                        stateMap[cx + 2, cy + 1] = true;
                        lasty2 = cy+1;
                        break;
                    case Pattern.a3:
                        stateMap[cx + 1, cy + 1] = true;
                        lasty1 = cy+1;
                        stateMap[cx + 2, cy + 1] = true;
                        lasty2 = cy+1;
                        break;
                    case Pattern.a4:
                        stateMap[cx + 1, cy + 1] = true;
                        lasty1 = cy+1;
                        stateMap[cx + 2, cy + 2] = true;
                        lasty2 = cy+2;
                        break;
                    default:
                        break;
                } 
            }
            catch
            {

            }
            lastx1 = cx + 1;
            lastx2 = cx + 2;
        }
    }
}
