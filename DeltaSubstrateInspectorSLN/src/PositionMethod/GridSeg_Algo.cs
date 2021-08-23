using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Risitanse_AOI.src.PositionMethod.GridSegmentation
{
    unsafe class GridSeg_Algo
    {
        public struct Img
        {
            public IntPtr ptr;
            public int width;
            public int height;
            public int channels;
            public int step;
        } // Img

        public struct CornerPoint
        {
            public Point leftUp;
            public Point rightDown;
        } // Img

        public struct CornerPointArr
        {
            public CornerPoint* corPotPtr;
            public int size;
        } // CornerPointArr

        public static List<CornerPoint> cpl;

        [DllImport("test.dll", CharSet = CharSet.Ansi)]
        private static extern void runAlgo(ref Img bitmap, byte[] data, int thrH, int thrL, int erH, int erL, int downScalar, ref CornerPointArr cpa);


        public static BitmapInfo GetImagePixel(Bitmap source)
        {
            byte[] result;
            int step;
            int iWidth = source.Width;
            int iHeight = source.Height;
            Rectangle rect = new Rectangle(0, 0, iWidth, iHeight);
            System.Drawing.Imaging.BitmapData bmpData = source.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, source.PixelFormat); IntPtr iPtr = bmpData.Scan0;
            int channels = 3;
            if (source.PixelFormat == PixelFormat.Format32bppRgb || source.PixelFormat == PixelFormat.Format32bppArgb)
                channels = 4;
            else if (source.PixelFormat == PixelFormat.Format24bppRgb)
                channels = 3;
            else channels = 1;
            int iBytes = iWidth * iHeight * channels; //根据通道数进行设置
            byte[] PixelValues = new byte[iBytes];
            //int time = Environment.TickCount;
            System.Runtime.InteropServices.Marshal.Copy(iPtr, PixelValues, 0, iBytes); //time = Environment.TickCount - time;
            //Console.WriteLine(time.ToString() + "ms");
            source.UnlockBits(bmpData);
            step = bmpData.Stride;
            result = PixelValues;
            // return result;
            // step = 0;
            BitmapInfo bi = new BitmapInfo { Result = result, Step = step };
            return bi;
        } // GetImagePixel()

        public class BitmapInfo
        {
            public byte[] Result { get; set; }
            public int Step { get; set; }
        }

        private static List<CornerPoint> pointPairArr2List(CornerPointArr input)
        {
            List<CornerPoint> pointPairList = new List<CornerPoint>();
            for (int i = 0; i < input.size; i++)
            {
                CornerPoint temp = new CornerPoint();
                temp.leftUp = input.corPotPtr[i].leftUp;
                temp.rightDown = input.corPotPtr[i].rightDown;
                pointPairList.Add(temp);
            }
            return pointPairList;
        }

        public static void runGSA(ref Bitmap bitmap, int thrH, int thrL, int erH, int erL, int downScalar, ref List<Point> tl_list, ref List<Point> br_list)
        {
            BitmapInfo bi = GetImagePixel(bitmap);
            Img img = new Img();
            img.height = bitmap.Height;
            img.width = bitmap.Width;
            if (bitmap.PixelFormat == PixelFormat.Format32bppRgb || bitmap.PixelFormat == PixelFormat.Format32bppArgb)
                img.channels = 4;
            else if (bitmap.PixelFormat == PixelFormat.Format24bppRgb)
                img.channels = 3;
            else img.channels = 1;

            img.step = bi.Step;
            CornerPointArr cpa = new CornerPointArr();
            runAlgo(ref img, bi.Result, thrH, thrL, erH, erL, downScalar, ref cpa);
            cpl = pointPairArr2List(cpa);
            foreach (var item in cpl)
            {
                tl_list.Add(item.leftUp);
                br_list.Add(item.rightDown);
            }
            bitmap = new Bitmap(img.width, img.height, img.step, System.Drawing.Imaging.PixelFormat.Format32bppRgb, img.ptr);

        } // runGSA()

       
    }
}
