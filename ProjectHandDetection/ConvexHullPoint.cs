using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHandDetection
{
    class ConvexHullPoint
    {
        public int indexOnContour;
        public Point chPoint;

        public ConvexHullPoint(int index,Point p)
        {
            indexOnContour = index;
            chPoint = p;
        }
    }
}
