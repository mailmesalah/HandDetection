using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHandDetection
{
    class ConvexityDefectPoint
    {
        public Point startPoint;
        public Point endPoint;
        public double defectAngle;
        public double defectDepth;
        public Point defectPoint;

        public ConvexityDefectPoint(Point startP,Point endP,Point defectP,double angle,double depth)
        {
            startPoint = startP;
            endPoint = endP;
            defectPoint = defectP;
            defectAngle = angle;
            defectDepth = depth;
        }
    }
}
