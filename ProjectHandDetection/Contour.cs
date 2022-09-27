using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHandDetection
{

    class Contour
    {
        List<Point> contours = new List<Point>();
        Rectangle border = new Rectangle();

        public Contour(List<Point> contours, Rectangle border)
        {
            this.contours = contours;
            this.border = border;
        }

        public List<Point> getContours(){
            return contours;
        }

        public Rectangle getBorder(){
            return border;
        }

        public Point getCenterPoint(){
            return new Point(border.X + border.Width / 2, border.Y + border.Height / 2);
        }
    }
}
