using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing.Imaging;
using System.Threading;
using System.Runtime.InteropServices;

namespace ProjectHandDetection
{
    public partial class FormMain : Form
    {
        private FilterInfoCollection captureDevices;
        private VideoCaptureDevice camera;
        
        List<int> mXList = new List<int>();

       
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            captureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach (FilterInfo cameras in captureDevices)
            {
                cameraCombo.Items.Add(cameras.Name);
            }
            cameraCombo.SelectedIndex = 0;
            //Loading resolution
            loadResolutionToCombo();
            camera = new VideoCaptureDevice();

            processed = true;

        }

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        #region Windows key function
        const int VK_UP = 0x26; //up key
        const int VK_DOWN = 0x28;  //down key
        const int VK_LEFT = 0x25; //left arrow
        const int VK_RIGHT = 0x27; // Right arrow
        const uint KEYEVENTF_KEYUP = 0x0002;
        const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        #endregion

        #region press Left
        void pressLeft()
        {
            //Press the key
            keybd_event((byte)VK_LEFT, 0, KEYEVENTF_EXTENDEDKEY | 0, 0);
            
        }
        #endregion

        #region press Right
        void pressRight()
        {
            //Press the key
            keybd_event((byte)VK_RIGHT, 0, KEYEVENTF_EXTENDEDKEY | 0, 0);
        }
        #endregion

        private void startButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (camera.IsRunning)
                {
                    camera.Stop();
                    camera.WaitForStop();
                }
                camera = new VideoCaptureDevice(captureDevices[cameraCombo.SelectedIndex].MonikerString);
                camera.NewFrame += new NewFrameEventHandler(newFrameCall);
                camera.VideoResolution = camera.VideoCapabilities[resolutionCombo.SelectedIndex];                
                camera.Start();

            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        void newFrameCall(object sender, NewFrameEventArgs eventArgs)
        {
            
            if (processed)
            {
                processed = false;
                try
                {
                    Bitmap mFrame =(Bitmap)eventArgs.Frame.Clone();

                    //BitmapData objectsData = mFrame.LockBits(new Rectangle(0, 0, mFrame.Width, mFrame.Height),ImageLockMode.ReadOnly, mFrame.PixelFormat);                                
                    int[,] arr = CHandDetection.binirizeByColoursOnRangeToIntArray(mFrame, 80, 255, 85, 135, 135, 180);
                    List<Contour> conts = CHandDetection.getContoursFromBinirizedIntArray(arr, 20);

                    if (conts.Count > 0)
                    {
                        Contour bCon = conts.ElementAt(0);
                        int big = 0;
                        for (int i = 0; i < conts.Count; i++)
                        {
                            if (big < conts.ElementAt(i).getBorder().Width)
                            {
                                big = conts.ElementAt(i).getBorder().Width;
                                bCon = conts.ElementAt(i);
                            }
                        }

                        //Boolean found = false;
                        List<ConvexHullPoint> hullP = CHandDetection.getConvexHull(bCon);
                        List<ConvexityDefectPoint> convD = CHandDetection.getConvexityDefects(bCon, hullP);
                        int count = 0;
                        for (int i = 0; i < convD.Count; i++)
                        {
                            if (convD.ElementAt(i).defectAngle < 90)
                            {
                                ++count;
                            }
                        }

                        if (count == 4)
                        {
                            //found = true;
                            //Hand is Found

                            List<System.Drawing.Point> ps = bCon.getContours();
                            foreach (System.Drawing.Point p in ps)
                            {
                                mFrame.SetPixel(p.X, p.Y, Color.Blue);
                            }

                            using (Graphics g = Graphics.FromImage(mFrame))
                            {
                                for (int i = 0; i < hullP.Count - 1; i++)
                                {
                                    Pen ppp = new Pen(Color.Red, 2);
                                    Pen ppg = new Pen(Color.Green, 2);
                                    Pen ppy = new Pen(Color.Yellow, 2);
                                    g.DrawLine(ppp, hullP.ElementAt(i).chPoint, hullP.ElementAt(i + 1).chPoint);
                                    //g.DrawEllipse(ppp, hullP.ElementAt(i).chPoint.X, hullP.ElementAt(i).chPoint.Y, 2, 2);
                                    if (i < convD.Count)
                                    {
                                        if (convD.ElementAt(i).defectAngle<90)
                                        {
                                            g.DrawEllipse(ppy, convD.ElementAt(i).defectPoint.X, convD.ElementAt(i).defectPoint.Y, 2, 2);
                                        }                                        

                                    }

                                    if (i == 0)
                                    {
                                        g.DrawEllipse(ppg, bCon.getCenterPoint().X, bCon.getCenterPoint().Y, 2, 2);
                                    }
                                }
                            }
                            rawCameraFrame.Image = mFrame;

                            //Left or Right Checking
                            mXList.Add(bCon.getCenterPoint().X);
                            if (mXList.Count >= 4)
                            {
                                bool success = true;
                                bool left = (mXList.ElementAt(0) > mXList.ElementAt(1));
                                for (int j = 1; j < mXList.Count - 1; j++)
                                {
                                    if (left)
                                    {
                                        if (mXList.ElementAt(j) < mXList.ElementAt(j + 1))
                                        {
                                            success = false;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (mXList.ElementAt(j) > mXList.ElementAt(j + 1))
                                        {
                                            success = false;
                                            break;
                                        }
                                    }
                                }
                                if (!success)
                                {
                                    mXList.Clear();
                                }
                                else
                                {
                                    if (left)
                                    {
                                        if (mXList.ElementAt(0) - mXList.ElementAt(mXList.Count - 1) > 50)
                                        {
                                            SetText("LEFT");
                                            pressLeft();
                                            mXList.Clear();
                                        }
                                    }
                                    else
                                    {
                                        if (mXList.ElementAt(mXList.Count - 1) - mXList.ElementAt(0) > 50)
                                        {
                                            SetText("RIGHT");
                                            pressRight();
                                            mXList.Clear();
                                        }
                                    }

                                }
                            }
                        }
                        else
                        {
                            rawCameraFrame.Image = mFrame;
                        }


                        

                    }
                    else
                    {
                        rawCameraFrame.Image = mFrame;
                    }



                }
                catch (Exception e) { Console.WriteLine(e.Message); }
                processed = true;
            }

        }

        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.swipeOutput.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.swipeOutput.Text = text;
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (camera.IsRunning)
            {
                camera.Stop();
            }
        }

        private void cameraCombo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            loadResolutionToCombo();
        }

        private void loadResolutionToCombo()
        {
            try
            {
                camera = new VideoCaptureDevice(captureDevices[cameraCombo.SelectedIndex].MonikerString);
                //Adding to combobox
                resolutionCombo.Items.Clear();
                for (int i = 0; i < camera.VideoCapabilities.Length; i++)
                {
                    resolutionCombo.Items.Add(camera.VideoCapabilities[i].FrameSize.Width + " X " + camera.VideoCapabilities[i].FrameSize.Height);
                }
                resolutionCombo.SelectedIndex = 0;
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }
        //Testing***************************
        List<System.Drawing.Point> convexHull = new List<System.Drawing.Point>();
        List<System.Drawing.Point> borderPs = new List<System.Drawing.Point>();
        System.Drawing.Point init = new System.Drawing.Point();
        int k;
        System.Drawing.Point firstP = new System.Drawing.Point();
        System.Drawing.Point middleP = new System.Drawing.Point();
        System.Drawing.Point lastP = new System.Drawing.Point();
        System.Drawing.Point p, q, r;
        List<Contour> conts = new List<Contour>();
        Bitmap bm;
        private bool processed;
        //Testing

        private void button1_Click(object sender, EventArgs e)
        {
            bm = (Bitmap)Image.FromFile(@"i:\load.jpg").Clone();

            int[,] arr = CHandDetection.binirizeByColoursOnRangeToIntArray(bm, 10, 255, 50, 200, 100, 200);
            conts = CHandDetection.getContoursFromBinirizedIntArray(arr, 5);



            convexHull = new List<System.Drawing.Point>();
            Contour border = conts.ElementAt(1);
            borderPs = border.getContours();
            if (borderPs.Count > 2)
            {
                q = borderPs.ElementAt(0);
                int index = 0;
                //find left most X to find point p
                for (int i = 1; i < borderPs.Count; i++)
                {
                    System.Drawing.Point temp = borderPs.ElementAt(i);
                    if (temp.X < q.X)
                    {
                        q = temp;
                        index = i;
                    }
                }
                //increment y and find another point q
                convexHull.Add(q);
                p = new System.Drawing.Point(q.X, q.Y + 1);

                k = index + 1;
                if (k == borderPs.Count)
                {
                    k = 0;
                }
                r = borderPs.ElementAt(k);

                //Iterate through the other points as r and find the biggest angle it makes.
                firstP = q;
                lastP = r;
              
            }
            

            //Console.WriteLine("Conts "+conts.Count);
            /*
            foreach (Contour c in conts)
            {
                List<System.Drawing.Point> ps = c.getContours();
                foreach (System.Drawing.Point p in ps)
                {
                    bm.SetPixel(p.X, p.Y, Color.Blue);
                    processedCameraFrame.Image = bm;                   
                }

            
                //Rectangle r = new Rectangle(ps.ElementAt(0).X, ps.ElementAt(0).Y,1,1);

                //bm.SetPixel(ps.ElementAt(0).X,ps.ElementAt(0).Y,Color.Brown);
                
                //using (Graphics g = Graphics.FromImage(bm))
                //{
                //    Pen p = new Pen(Color.Yellow,5);
                //    g.DrawRectangle(p, r);
                //}
            
                
                ps = CHandDetection.getConvexHull(c);
                System.Drawing.Point[] myArray = ps.ToArray();
                using (Graphics g = Graphics.FromImage(bm))
                {
                    g.DrawPolygon(Pens.Black, myArray);
                }
                /*foreach (System.Drawing.Point p in ps)
                {
                    mFrame.SetPixel(p.X,p.Y,Color.Red);
                }
            }*/
            //processedCameraFrame.Image = (Bitmap)bm.Clone();
            /*
            List<System.Drawing.Point> lp = new List<System.Drawing.Point>();
            lp.Add(new System.Drawing.Point(0, 3));
            lp.Add(new System.Drawing.Point(0, 4));
            lp.Add(new System.Drawing.Point(1, 4));
            lp.Add(new System.Drawing.Point(2, 4));
            lp.Add(new System.Drawing.Point(3, 4));
            lp.Add(new System.Drawing.Point(4, 4));
            lp.Add(new System.Drawing.Point(5, 4));
            lp.Add(new System.Drawing.Point(5, 5));
            lp.Add(new System.Drawing.Point(4, 5));
            lp.Add(new System.Drawing.Point(3, 6));
            lp.Add(new System.Drawing.Point(2, 7));
            lp.Add(new System.Drawing.Point(2, 8));
            lp.Add(new System.Drawing.Point(3, 8));
            lp.Add(new System.Drawing.Point(4, 8));
            lp.Add(new System.Drawing.Point(5, 8));
            lp.Add(new System.Drawing.Point(6, 8));
            lp.Add(new System.Drawing.Point(7, 8));
            lp.Add(new System.Drawing.Point(8, 7));
            lp.Add(new System.Drawing.Point(9, 6));
            lp.Add(new System.Drawing.Point(8, 5));
            lp.Add(new System.Drawing.Point(7, 4));
            lp.Add(new System.Drawing.Point(6, 3));
            lp.Add(new System.Drawing.Point(6, 2));
            lp.Add(new System.Drawing.Point(5, 1));
            lp.Add(new System.Drawing.Point(4, 1));
            lp.Add(new System.Drawing.Point(3, 2));
            lp.Add(new System.Drawing.Point(2, 2));
            lp.Add(new System.Drawing.Point(1, 2));

            Bitmap tt = new Bitmap(10, 10);
            for (int i = 0; i < lp.Count; i++)
            {
                System.Drawing.Point pp = lp.ElementAt(i);
                tt.SetPixel(pp.X,pp.Y,Color.Blue);
            }

            Contour cc = new Contour(lp,new Rectangle());
            lp = CHandDetection.getConvexHull(cc);*/
            //System.Drawing.Point[] myArray = lp.ToArray();
            //using (Graphics g = Graphics.FromImage(tt))
            //{
            //   g.DrawPolygon(Pens.Black, myArray);
            //}
            //foreach (System.Drawing.Point p in lp)
            //{
            //    tt.SetPixel(p.X,p.Y,Color.Red);
            //}

            //System.Drawing.Point[] myArray = lp.ToArray();
            //using (Graphics g = Graphics.FromImage(tt))
            //{
            //   g.DrawPolygon(Pens.Green, myArray);
            //}
            //processedCameraFrame.Image = tt;
        }

        
    }
}
