using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHandDetection
{
    class CHandDetection
    {
        private static int checking;
        //First step sample hand colour from camera
        public static List<Color> sampleHandColour(Bitmap frame, List<Rectangle> sampleAreas)
        {
            List<Color> sampleColours = new List<Color>();
            try
            {
                foreach (Rectangle r in sampleAreas)
                {
                    for (int i = r.X; i < r.X + r.Width; i++)
                    {
                        for (int j = r.Y; j < r.Y + r.Height; j++)
                        {
                            //Optimizing color by not adding already added color
                            if (!sampleColours.Contains(frame.GetPixel(i, j)))
                            {
                                sampleColours.Add(frame.GetPixel(i, j));
                            }
                        }
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            return sampleColours;

        }

        public static Bitmap binirizeByColoursOnRangeToBitmap(Bitmap frame, float Ymin, float Ymax, float Cbmin, float Cbmax, float Crmin, float Crmax)
        {
            Console.WriteLine("Warning Method incomplete");
            Bitmap binirizedFrame = new Bitmap(frame.Width, frame.Height);
            try
            {
                for (int i = 0; i < frame.Width; i++)
                {
                    for (int j = 0; j < frame.Height; j++)
                    {
                        //The range of color to be  checked for skin                        
                        Color c = frame.GetPixel(i, j);
                        float Y = (0.257f * c.R) + (0.504f * c.G) + (0.098f * c.B) + 16;
                        float Cb = (-0.148f * c.R) - (0.291f * c.G) + (0.439f * c.B) + 128;
                        float Cr = (0.439f * c.R) - (0.368f * c.G) - (0.071f * c.B) + 128;

                        if (Y < Ymax && Y > Ymin && Cb > Cbmin && Cb < Cbmax && Cr > Crmin && Cr < Crmax)
                        {
                            binirizedFrame.SetPixel(i, j, Color.White);
                        }
                        else
                        {
                            binirizedFrame.SetPixel(i, j, Color.Black);
                        }
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            return binirizedFrame;
        }

        public static Bitmap binirizeByColoursToBitmap(Bitmap frame, List<Color> sampledColours, float hue, float sat, float bri)
        {
            Bitmap binirizedFrame = new Bitmap(frame.Width, frame.Height);
            try
            {
                for (int i = 0; i < frame.Width; i++)
                {
                    //Console.WriteLine(i);
                    for (int j = 0; j < frame.Height; j++)
                    {
                        bool found = false;

                        foreach (Color color in sampledColours)
                        {
                            float h = color.GetHue();
                            float s = color.GetSaturation();
                            float b = color.GetBrightness();
                            Color fc = frame.GetPixel(i, j);
                            float fh = fc.GetHue();
                            float fs = fc.GetSaturation();
                            float fb = fc.GetBrightness();

                            if ((fh <= h + hue && fh >= h - hue) && (fs <= s + sat && fs >= s - sat) && (fb <= b + bri && fb >= b - bri))
                            {
                                found = true;
                                break;
                            }

                        }
                        if (found)
                        {
                            binirizedFrame.SetPixel(i, j, Color.White);
                        }
                        else
                        {
                            binirizedFrame.SetPixel(i, j, Color.Black);
                        }
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            return binirizedFrame;
        }

        public static int[,] binirizeByColoursOnRangeToIntArray(Bitmap frame, float Ymin, float Ymax, float Cbmin, float Cbmax, float Crmin, float Crmax)
        {
            int[,] binirizedFrame = new int[frame.Width, frame.Height];
            try
            {
                for (int i = 0; i < frame.Width; i++)
                {
                    for (int j = 0; j < frame.Height; j++)
                    {
                        //here for color range to be checked
                        Color c = frame.GetPixel(i, j);
                        float Y = (0.257f * c.R) + (0.504f * c.G) + (0.098f * c.B) + 16;
                        float Cb = (-0.148f * c.R) - (0.291f * c.G) + (0.439f * c.B) + 128;
                        float Cr = (0.439f * c.R) - (0.368f * c.G) - (0.071f * c.B) + 128;

                        if (Y < Ymax && Y > Ymin && Cb > Cbmin && Cb < Cbmax && Cr > Crmin && Cr < Crmax)
                        {
                            binirizedFrame[i, j] = 1;
                        }
                        else
                        {
                            binirizedFrame[i, j] = 0;
                        }
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            return binirizedFrame;
        }

        public static int[,] binirizeByColoursToIntArray(Bitmap frame, List<Color> sampledColours, float hue, float sat, float bri)
        {
            int[,] binirizedFrame = new int[frame.Width, frame.Height];
            try
            {
                for (int i = 0; i < frame.Width; i++)
                {
                    for (int j = 0; j < frame.Height; j++)
                    {
                        bool found = false;

                        foreach (Color color in sampledColours)
                        {
                            float h = color.GetHue();
                            float s = color.GetSaturation();
                            float b = color.GetBrightness();
                            Color fc = frame.GetPixel(i, j);
                            float fh = fc.GetHue();
                            float fs = fc.GetSaturation();
                            float fb = fc.GetBrightness();

                            if ((fh <= h + hue && fh >= h - hue) && (fs <= s + sat && fs >= s - sat) && (fb <= b + bri && fb >= b - bri))
                            {
                                found = true;
                                break;
                            }

                        }
                        if (found)
                        {
                            binirizedFrame[i, j] = 1;
                        }
                        else
                        {
                            binirizedFrame[i, j] = 0;
                        }
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            return binirizedFrame;
        }


        public static List<Contour> getContoursFromBinirizedBitmap(Bitmap binirizedFrame, int minWidthHeight)
        {
            List<Contour> contours = new List<Contour>();
            int[,] checkedPoints = new int[binirizedFrame.Width, binirizedFrame.Height];
            int[,] frame = new int[binirizedFrame.Width, binirizedFrame.Height];

            //Converting Bitmap pixels to two dimentional integer array
            for (int i = 0; i < binirizedFrame.Width; i++)
            {
                for (int j = 0; j < binirizedFrame.Height; j++)
                {
                    if (binirizedFrame.GetPixel(i, j) == Color.White)
                    {
                        frame[i, j] = 1;
                    }
                    else
                    {
                        frame[i, j] = 0;
                    }
                }
            }

            //Finding borders of binirized frame
            for (int i = 0; i < binirizedFrame.Width; i++)
            {
                for (int j = 0; j < binirizedFrame.Height; j++)
                {
                    //Only checks if the cell containing 1 having left cell as a border and which is not already checked
                    if (frame[i, j] != 0 && (j - 1 == -1 || frame[i, j - 1] == 0) && checkedPoints[i, j] != 1)
                    {
                        //Console.WriteLine("Border of "+i);
                        Contour lp = getBorder(new Point(i, j), frame, checkedPoints);
                        if (lp != null && lp.getBorder().Width >= minWidthHeight && lp.getBorder().Height >= minWidthHeight)
                        {
                            contours.Add(lp);
                        }
                    }
                }
            }

            return contours;
        }

        public static List<Contour> getContoursFromBinirizedIntArray(int[,] frame, int minWidthHeight)
        {
            List<Contour> contours = new List<Contour>();
            int[,] checkedPoints = new int[frame.GetLength(0), frame.GetLength(1)];

            //Finding borders of binirized frame            
            for (int i = 0; i < frame.GetLength(0); i++)
            {
                for (int j = 0; j < frame.GetLength(1); j++)
                {
                    //Only checks if the cell containing 1 having left cell as a border and which is not already checked
                    if (frame[i, j] != 0 && (j - 1 == -1 || frame[i, j - 1] == 0) && checkedPoints[i, j] != 1)
                    {
                        //Console.WriteLine("Border of "+i);
                        Contour lp = getBorder(new Point(i, j), frame, checkedPoints);
                        if (lp != null && lp.getBorder().Width >= minWidthHeight && lp.getBorder().Height >= minWidthHeight)
                        {
                            contours.Add(lp);
                        }
                    }
                }
            }

            return contours;
        }

        private static Contour getBorder(Point frameCell, int[,] frame, int[,] check)
        {
            List<Point> borderPoints = new List<Point>();
            int count = 0;
            Point firstCell = new Point(frameCell.X, frameCell.Y);
            Point borderCell = new Point(frameCell.X, frameCell.Y - 1);
            Point previousBCell = new Point(borderCell.X, borderCell.Y);
            //Rectangular Border
            int x1 = borderCell.X;
            int x2 = borderCell.X;
            int y1 = borderCell.Y;
            int y2 = borderCell.Y;

            //Check till the count is less that 8 or last frame cell meets the first frame cell
            while (count <= 8 && (firstCell.X != borderCell.X || firstCell.Y != borderCell.Y))
            {
                //Console.WriteLine("ZXZx"+ count+"-first cell "+firstCell.X+"-"+firstCell.Y+" border cell "+borderCell.X+"-"+borderCell.Y+" previous bCell "+previousBCell.X+"-"+previousBCell.Y);
                if (borderCell.X == firstCell.X && borderCell.Y == firstCell.Y - 1)
                {
                    //First check which is already considered as one of border points                    
                    Point p = new Point(borderCell.X < 0 ? borderCell.X + 1 : borderCell.X, borderCell.Y < 0 ? borderCell.Y + 1 : borderCell.Y);
                    //Recording that a cell is found
                    check[frameCell.X, frameCell.Y] = 1;
                    //Console.WriteLine("First Border "+borderCell.X+"-"+borderCell.Y);
                    if (borderPoints.IndexOf(p) == -1)
                    {
                        borderPoints.Add(p);
                    }
                    borderCell = getNextPointToCheck(frameCell, borderCell);
                    ++count;
                }
                else
                {
                    //If there is a border
                    if (borderCell.X < 0 || borderCell.Y < 0 || borderCell.X >= frame.GetLength(0) || borderCell.Y >= frame.GetLength(1) || frame[borderCell.X, borderCell.Y] == 0)
                    {
                        //Keeping the previous value incase need on finding a non border cell
                        previousBCell.X = borderCell.X;
                        previousBCell.Y = borderCell.Y;
                        //Add current borderCell to the border list
                        Point p = new Point(borderCell.X < 0 ? borderCell.X + 1 : borderCell.X >= frame.GetLength(0) ? borderCell.X - 1 : borderCell.X, borderCell.Y < 0 ? borderCell.Y + 1 : borderCell.Y >= frame.GetLength(1) ? borderCell.Y - 1 : borderCell.Y);
                        //Console.WriteLine("Border " + borderCell.X + "-" + borderCell.Y);
                        if (borderPoints.IndexOf(p) == -1)
                        {
                            x1 = x1 > p.X ? p.X : x1;
                            x2 = x2 < p.X ? p.X : x2;
                            y1 = y1 > p.Y ? p.Y : y1;
                            y2 = y2 < p.Y ? p.Y : x2;
                            borderPoints.Add(p);
                        }

                        //next borderCell is produced to check.
                        borderCell = getNextPointToCheck(frameCell, borderCell);
                        ++count;
                    }
                    //if there is a cell 
                    else
                    {
                        //Since current borderCell has foud to be a non border cell, the cell is taken as frame cell to check for its border                        
                        frameCell.X = borderCell.X;
                        frameCell.Y = borderCell.Y;

                        //If the last border cell is not neighbour cell of current frame cell, then no need to continue checking
                        //It means it finding border for non object part instead of object part, like hole in object part
                        if (Math.Abs(borderCell.X - previousBCell.X) > 1 || Math.Abs(borderCell.Y - previousBCell.Y) > 1)
                        {
                            return null;
                        }

                        //Recording that a cell is found
                        check[frameCell.X, frameCell.Y] = 1;
                        //Console.WriteLine("Cell " + frameCell.X + "-" + frameCell.Y);
                        //Previous borderCell is taken to find the next borderCell of the newly selected frame cell
                        borderCell = getNextPointToCheck(frameCell, previousBCell);
                        //Console.WriteLine("Prev "+previousBCell.X+"-"+previousBCell.Y);
                        //count is Reset for new frame cell
                        count = 1;
                    }
                }
            }
            return new Contour(borderPoints, new Rectangle(x1, y1, x2 - x1, y2 - y1));
        }

        private static Point getNextPointToCheck(Point frameCell, Point currentPoint)
        {
            Point nextPoint = new Point();

            //1
            if (currentPoint.X == frameCell.X && currentPoint.Y == frameCell.Y - 1)
            {
                nextPoint.X = frameCell.X - 1;
                nextPoint.Y = frameCell.Y - 1;
            }
            //2
            else if (currentPoint.X == frameCell.X - 1 && currentPoint.Y == frameCell.Y - 1)
            {
                nextPoint.X = frameCell.X - 1;
                nextPoint.Y = frameCell.Y;
            }
            //3
            else if (currentPoint.X == frameCell.X - 1 && currentPoint.Y == frameCell.Y)
            {
                nextPoint.X = frameCell.X - 1;
                nextPoint.Y = frameCell.Y + 1;
            }
            //4
            else if (currentPoint.X == frameCell.X - 1 && currentPoint.Y == frameCell.Y + 1)
            {
                nextPoint.X = frameCell.X;
                nextPoint.Y = frameCell.Y + 1;
            }
            //5
            else if (currentPoint.X == frameCell.X && currentPoint.Y == frameCell.Y + 1)
            {
                nextPoint.X = frameCell.X + 1;
                nextPoint.Y = frameCell.Y + 1;
            }
            //6
            else if (currentPoint.X == frameCell.X + 1 && currentPoint.Y == frameCell.Y + 1)
            {
                nextPoint.X = frameCell.X + 1;
                nextPoint.Y = frameCell.Y;
            }
            //7
            else if (currentPoint.X == frameCell.X + 1 && currentPoint.Y == frameCell.Y)
            {
                nextPoint.X = frameCell.X + 1;
                nextPoint.Y = frameCell.Y - 1;
            }
            //8
            else if (currentPoint.X == frameCell.X + 1 && currentPoint.Y == frameCell.Y - 1)
            {
                nextPoint.X = frameCell.X;
                nextPoint.Y = frameCell.Y - 1;
            }
            else
            {
                Console.WriteLine("Inside check frame cell " + frameCell.X + "-" + frameCell.Y + " prev " + currentPoint.X + "-" + currentPoint.Y);
            }

            return nextPoint;
        }

        public static Bitmap arrayToBitmap(int[,] array)
        {
            Bitmap retB = new Bitmap(array.GetLength(0), array.GetLength(1));
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (array[i, j] == 1)
                    {
                        retB.SetPixel(i, j, Color.White);
                    }
                    else
                    {
                        retB.SetPixel(i, j, Color.Black);
                    }

                }
            }

            return retB;
        }

        //Grift Wrapping method
        public static List<ConvexHullPoint> getConvexHull(Contour border)
        {

            List<ConvexHullPoint> convexHull = new List<ConvexHullPoint>();
            List<Point> borderPs = border.getContours();
            if (borderPs.Count > 2)
            {
                Point p, q, r;
                q = borderPs.ElementAt(0);
                int index = 0;
                //find left most X to find point p
                for (int i = 1; i < borderPs.Count; i++)
                {
                    Point temp = borderPs.ElementAt(i);
                    if (temp.X < q.X)
                    {
                        q = temp;
                        index = i;
                    }
                }
                //increment y and find another point q
                ConvexHullPoint chp = new ConvexHullPoint(index, q);
                convexHull.Add(chp);
                p = new Point(q.X, q.Y + 1);

                int k = index + 1;
                if (k == borderPs.Count)
                {
                    k = 0;
                }
                r = borderPs.ElementAt(k);

                //Iterate through the other points as r and find the biggest angle it makes.
                Point firstP = q;
                Point lastP = r;

                while (lastP != firstP)
                {

                    int bIndx = k;
                    double bigV = 0;

                    for (int j = 0; j < borderPs.Count; j++)
                    {
                        //Checking angle for biggest value
                        r = borderPs.ElementAt(j);
                        double tempV = getAngle(p, q, r);
                        //&& tempV < 180 may be skipping the last point due to this check.
                        if (bigV < tempV && tempV < 180)
                        {
                            bigV = tempV;
                            bIndx = j;
                        }
                    }
                    //Cheking if already added
                    Boolean found = false;
                    for (int m = 0; m < convexHull.Count; m++)
                    {
                        if (convexHull.ElementAt(m).indexOnContour == bIndx)
                        {
                            //Already added the point
                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {
                        break;
                    }

                    //Adding the Selected Point to the Hull                    
                    chp = new ConvexHullPoint(bIndx, borderPs.ElementAt(bIndx));
                    convexHull.Add(chp);
                    //Take that r point as next q, and q as p 
                    k = bIndx;
                    p = q;
                    q = borderPs.ElementAt(bIndx);
                    lastP = q;
                    
                    ++k;
                    if (k == borderPs.Count)
                    {
                        k = 0;
                    }
                }
            }            
            return convexHull;
        }

        // Center point is q; angle returned in degrees
        public static double getAngle(Point p, Point q, Point r)
        {
            double a = Math.Pow(q.X - p.X, 2) + Math.Pow(q.Y - p.Y, 2);
            double b = Math.Pow(q.X - r.X, 2) + Math.Pow(q.Y - r.Y, 2);
            double c = Math.Pow(r.X - p.X, 2) + Math.Pow(r.Y - p.Y, 2);
            return Math.Acos((a + b - c) / Math.Sqrt(4 * a * b)) * (180 / Math.PI);
        }


        public static List<ConvexityDefectPoint> getConvexityDefects(Contour contour, List<ConvexHullPoint> convexHull)
        {
            List<ConvexityDefectPoint> convexityDefects = new List<ConvexityDefectPoint>();

            List<Point> contPoints = contour.getContours();
            int k1 = convexHull.ElementAt(0).indexOnContour;
            int k2 = convexHull.ElementAt(1).indexOnContour;
            int k3 = convexHull.ElementAt(2).indexOnContour;
            //finding the contour iteration is clockwise or not
            Boolean isClockwiseIterate = k1 < k2 ? k2 < k3 ? true : false : k2 < k3 ? true : false;

            for (int i = 0; i < convexHull.Count; i++)
            {
                int k = i == convexHull.Count - 1 ? 0 : i + 1;
                int startI = convexHull.ElementAt(i).indexOnContour;
                int endI = convexHull.ElementAt(k).indexOnContour;
                Point p = convexHull.ElementAt(i).chPoint;
                Point r = convexHull.ElementAt(k).chPoint;
                double sAngle = 180;
                int index = -1;

                int j = startI;                
                while (j != endI)
                {
                    Point q = contPoints.ElementAt(j);
                    double ang = getAngle(p, q, r);
                    if (ang < sAngle)
                    {
                        sAngle = ang;
                        index = j;
                    }

                    //Update the index j
                    if (isClockwiseIterate)
                    {
                        ++j;
                        if (j == contPoints.Count)
                        {
                            j = 0;
                        }
                    }
                    else
                    {
                        --j;
                        if (j == -1)
                        {
                            j = contPoints.Count - 1;
                        }
                    }                    
                }
                if (index != -1)
                {
                    //Depth still to be calcultated***************************************+/####################
                    ConvexityDefectPoint cdp = new ConvexityDefectPoint(p, r, contPoints.ElementAt(index), sAngle, 0);
                    convexityDefects.Add(cdp);
                }

            }          
            return convexityDefects;
        }


        //Grahams Algorithm altered for finding convex hull
        //Not working, the algorithm has a flaw
        public static List<Point> getConvexHullGrahamsAlgo(Contour border)
        {
            List<Point> convexHull = new List<Point>();
            List<Point> borderPs = border.getContours();
            if (borderPs.Count > 2)
            {
                //Finding the left most point and its index value
                Point init = new Point();
                int x = border.getBorder().X;
                int index = 0;
                for (int i = 0; i < borderPs.Count; i++)
                {
                    Point p = borderPs.ElementAt(i);
                    if (p.X == x)
                    {
                        init = p;
                        index = i;
                        break;
                    }
                }

                //Initial values of the variable
                int k = index + 1;
                Point firstP = init;
                Point middleP = borderPs.ElementAt(k);
                Point lastP = borderPs.ElementAt(k + 1);
                k = k + 2;
                while (init != lastP)
                {
                    //Checking all other points if it is clockwise
                    int returnV = -1;
                    Point startP = firstP;
                    Point p = firstP;
                    Point q = middleP;
                    Point r = lastP;
                    int j = k;
                    while (startP != r)
                    {
                        returnV = isClockWise(p, q, r);
                        if (returnV < 0)
                        {
                            break;
                        }
                        r = borderPs.ElementAt(j);
                        ++j;
                        if (j == borderPs.Count)
                        {
                            j = 0;
                        }
                    }
                    //Checking if it is colinear or clockwise if not anti clockwise
                    if (returnV >= 0)
                    {
                        returnV = isClockWise(p, q, r);
                    }
                    //************************************************************

                    if (returnV > 0)
                    {
                        convexHull.Add(firstP);
                        //move to next p,q,r                        
                        firstP = middleP;
                        middleP = lastP;
                        lastP = borderPs.ElementAt(k);

                        //Cycling k through the point list
                        ++k;
                        if (k == borderPs.Count)
                        {
                            k = 0;
                        }
                    }
                    else if (returnV == 0)
                    {
                        middleP = lastP;
                        lastP = borderPs.ElementAt(k);

                        //Cycling k through the point list
                        ++k;
                        if (k == borderPs.Count)
                        {
                            k = 0;
                        }
                    }
                    else
                    {
                        if (convexHull.Count > 1)
                        {
                            middleP = firstP;
                            firstP = convexHull.ElementAt(convexHull.Count - 1);
                            convexHull.RemoveAt(convexHull.Count - 1);
                        }
                        else
                        {
                            middleP = lastP;
                            lastP = borderPs.ElementAt(k);

                            //Cycling k through the point list
                            ++k;
                            if (k == borderPs.Count)
                            {
                                k = 0;
                            }
                        }

                    }

                }
            }

            return convexHull;
        }

        private static int isClockWise(Point p, Point q, Point r)
        {
            //Find the points p, q, r is in clockwise or not
            //(x2-x1)(y3-y1)-(y2-y1)(x3-x1)
            int val = (q.X - p.X) * (r.Y - p.Y) - (q.Y - p.Y) * (r.X - p.X);
            //Clock wise if the value is less than zero
            return val;
        }

    }

}
