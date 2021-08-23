using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension
{
    public static class HObjectMedthods
    {
        //private static extern void CopyMemory(int Destination, int Source, int length);

        public static HObject TryToGetEmptyAndDisposeObject()
        {
            HObject hObject;
            HOperatorSet.GenEmptyObj(out hObject);
            hObject.Dispose();
            return hObject;
        }

        public static void ReplaceTheOtherHObject(this HObject reference, ref HObject source)
        {
            if (source == null || reference == null)
                return;

            source.Dispose();
            source = reference;
        }

        public static void ReleaseHObject(ref HObject hObject)
        {
            // 無法清空為 null， 如果沒有加 ref.
            if (hObject != null)
            {
                hObject.Dispose();
                hObject = null;
            }
        }

        public static void ShowImageOnWindow(this HObject hObject)
        {
            if (hObject == null) return;

            int width, height;
            HTuple windowsHandle;

            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            form.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            form.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            form.AutoSize = true;
            form.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            form.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            form.ClientSize = new System.Drawing.Size(1000, 700);
            form.ForeColor = System.Drawing.SystemColors.Control;
            form.Text = "Show Image";

            HImage img = new HImage(hObject);

            HImage img2 = img.ZoomImageSize(form.ClientSize.Width, form.ClientSize.Height, "constant");

            img.GetImageSize(out width, out height);

            HOperatorSet.OpenWindow(0, 0, width, height, form.Handle, "visible", "", out windowsHandle);

            HOperatorSet.DispObj(img2, windowsHandle);

            form.ShowDialog();
        }

        public static void SaveImage(this HObject hObject, string fileName = "D:\\image.bmp", int fillColor = 0, bool isOpenFile = true)
        {
            string folder = Path.GetDirectoryName(fileName);

            if (!folder.EndsWith("\\")) folder += "\\";

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            if (!Directory.Exists(folder))
                return;

            string extension = Path.GetExtension(fileName);
            extension = extension.Substring(1, extension.Length - 1);

            if (File.Exists(fileName))
                File.Delete(fileName);

            HOperatorSet.WriteImage(hObject, extension, fillColor, fileName);
            if (isOpenFile)
                System.Diagnostics.Process.Start(fileName);
        }

        public static void SaveRegion(this HObject hObject, string fileName = "D:\\region.tiff", bool isOpenFile = true)
        {
            string directory = Path.GetDirectoryName(fileName);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (Directory.Exists(directory))
            {
                HOperatorSet.WriteRegion(hObject, fileName);
                if (isOpenFile)
                    System.Diagnostics.Process.Start(fileName);
            }
        }

        public static void SaveRegionFromContour(this HObject contour, string fileName = "D:\\contour.tiff")
        {
            HObject region = null, union = null;
            try
            {
                HOperatorSet.GenRegionContourXld(contour, out region, "filled");
                HOperatorSet.Union1(region, out union);
                HOperatorSet.WriteRegion(union, fileName);
                System.Diagnostics.Process.Start(fileName);
            }
            finally
            {
                Extension.HObjectMedthods.ReleaseHObject(ref region);
                Extension.HObjectMedthods.ReleaseHObject(ref union);
            }
        }

        public static void SaveRegionWithUnion(this HObject hObject, string fileName = "R:\\union.tiff", bool isOpenFile = true)
        {
            string directory = Path.GetDirectoryName(fileName);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (Directory.Exists(directory))
            {
                HObject unionObject;
                HOperatorSet.Union1(hObject, out unionObject);
                HOperatorSet.WriteRegion(unionObject, fileName);
                ReleaseHObject(ref unionObject);
                if (isOpenFile)
                    System.Diagnostics.Process.Start(fileName);
            }
        }

        public static void SeeRegionRangeOnImage(this HObject region, int width = 6576, int height = 4384)
        {
            HObject imgSource, imgResult;
            HOperatorSet.GenImage1(out imgSource, "byte", width, height, 0);
            HOperatorSet.PaintRegion(region, imgSource, out imgResult, 255, "fill");
            imgResult.SaveImage();
            Extension.HObjectMedthods.ReleaseHObject(ref imgSource);
            Extension.HObjectMedthods.ReleaseHObject(ref imgResult);
        }

        public static void SeeRegionRangeOnAsignImage(this HObject region, HObject image)
        {
            HObject imgResult;
            HOperatorSet.PaintRegion(region, image, out imgResult, 255, "fill");
            imgResult.SaveImage();
            Extension.HObjectMedthods.ReleaseHObject(ref imgResult);
        }

        public static bool HasObject(this HObject region)
        {
            if ((region == null) || (region.Key.ToInt64() == 0))
                return false;

            HTuple area, areaIndex, findObjectIndex, length;

            HOperatorSet.RegionFeatures(region, "area", out area);
            HOperatorSet.TupleGreaterElem(area, 0, out areaIndex);
            HOperatorSet.TupleFind(areaIndex, 1, out findObjectIndex);
            HOperatorSet.TupleLength(findObjectIndex, out length);

            return (length.TupleGreater(0) == true) && (findObjectIndex.TupleSelect(0).TupleGreaterEqual(0));
        }

        public static bool HasObject(this HObject region, out HTuple findObjectIndex)
        {
            if ((region == null) || (region.Key.ToInt64() == 0))
            {
                findObjectIndex = new HTuple(-1);
                return false;
            }

            HTuple area, areaIndex, length;

            HOperatorSet.RegionFeatures(region, "area", out area);
            HOperatorSet.TupleGreaterElem(area, 0, out areaIndex);
            HOperatorSet.TupleFind(areaIndex, 1, out findObjectIndex);
            HOperatorSet.TupleLength(findObjectIndex, out length);

            return (length.TupleGreater(0) == true) && (findObjectIndex.TupleSelect(0).TupleGreaterEqual(0));
        }

        public static bool HasObject(this HObject region, out HTuple findObjectIndex, out HTuple length)
        {
            if ((region == null) || (region.Key.ToInt64() == 0))
            {
                findObjectIndex = new HTuple(-1);
                length = new HTuple(0);
                return false;
            }

            HTuple area, areaIndex;

            HOperatorSet.RegionFeatures(region, "area", out area);
            HOperatorSet.TupleGreaterElem(area, 0, out areaIndex);
            HOperatorSet.TupleFind(areaIndex, 1, out findObjectIndex);
            HOperatorSet.TupleLength(findObjectIndex, out length);

            return (length.TupleGreater(0) == true) && (findObjectIndex.TupleSelect(0).TupleGreaterEqual(0));
        }

        public static bool HasObject(this HObject region, out HTuple findObjectIndex, out HTuple length, out HTuple areaIndex)
        {
            if ((region == null) || (region.Key.ToInt64() == 0))
            {
                findObjectIndex = new HTuple(-1);
                length = new HTuple(0);
                areaIndex = new HTuple(-1);
                return false;
            }

            HTuple area;

            HOperatorSet.RegionFeatures(region, "area", out area);
            HOperatorSet.TupleGreaterElem(area, 0, out areaIndex);
            HOperatorSet.TupleFind(areaIndex, 1, out findObjectIndex);
            HOperatorSet.TupleLength(findObjectIndex, out length);

            return (length.TupleGreater(0) == true) && (findObjectIndex.TupleSelect(0).TupleGreaterEqual(0));
        }

        public static void CopyAdressTo(this HObject hObject, ref HObject SourceObject)
        {
            if (hObject == null || SourceObject == null)
                return;

            SourceObject.Dispose();
            SourceObject = null;
            SourceObject = hObject;
        }

        public static void DoNotEqualDispose(ref HObject hObject)
        {
            if ((hObject == null) || (hObject.Key.ToInt64() == 0))
                HOperatorSet.GenEmptyObj(out hObject);
        }

        public static void CopyObject(this HObject source, ref HObject result)
        {
            if ((source == null) || (source.Key.ToInt64() == 0))
                HOperatorSet.GenEmptyObj(out result);
            else
                HOperatorSet.CopyObj(source, out result, 1, -1);
        }

        public static Bitmap HObjectTransToBitmap(this HObject image)
        {
            Bitmap bitmap;

            HTuple width, height;
            HObject objR, objG, objB;
            HImage imgR, imgG, imgB;

            HOperatorSet.GetImageSize(image, out width, out height);
            HOperatorSet.Decompose3(image, out objR, out objG, out objB);

            int w, h;
            string type;

            imgR = new HImage(objR);
            IntPtr ptrR = imgR.GetImagePointer1(out type, out w, out h);

            imgG = new HImage(objG);
            IntPtr ptrG = imgG.GetImagePointer1(out type, out w, out h);

            imgB = new HImage(objB);
            IntPtr ptrB = imgB.GetImagePointer1(out type, out w, out h);

            bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            unsafe
            {
                BitmapData data = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                Bitmap imageR = new Bitmap(width, height, data.Stride / 3, PixelFormat.Format8bppIndexed, ptrR);
                Bitmap imageG = new Bitmap(width, height, data.Stride / 3, PixelFormat.Format8bppIndexed, ptrG);
                Bitmap imageB = new Bitmap(width, height, data.Stride / 3, PixelFormat.Format8bppIndexed, ptrB);

                BitmapData dataR = imageR.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
                BitmapData dataG = imageG.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
                BitmapData dataB = imageB.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

                for (int y = 0; y < height; y++)
                {
                    byte* byteData = (byte*)(data.Scan0 + y * data.Stride);

                    byte* byteR = (byte*)(dataR.Scan0 + y * dataR.Stride);
                    byte* byteG = (byte*)(dataG.Scan0 + y * dataG.Stride);
                    byte* byteB = (byte*)(dataB.Scan0 + y * dataB.Stride);

                    for (int x = 0; x < width; x++)
                    {
                        byteData[(x * 3) + 0] = *(byteB + x);
                        byteData[(x * 3) + 1] = *(byteG + x);
                        byteData[(x * 3) + 2] = *(byteR + x);
                    }
                }

                imageR.UnlockBits(dataR);
                imageG.UnlockBits(dataG);
                imageB.UnlockBits(dataB);
                bitmap.UnlockBits(data);

            }

            return bitmap;
        }

        //public static Bitmap GenertateGrayBitmap(this HObject image)
        //{
        //    HTuple hpoint, type, width, height;
        //    const int Alpha = 255;
        //    int[] ptr = new int[2];
        //    HOperatorSet.GetImagePointer1(image, out hpoint, out type, out width, out height);
        //    Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

        //    ColorPalette pal = bitmap.Palette;

        //    for (int i = 0; i <= 255; i++)
        //    {
        //        pal.Entries[i] = Color.FromArgb(Alpha, i, i, i);
        //    }
        //    bitmap.Palette = pal;

        //    unsafe
        //    {
        //        Rectangle rect = new Rectangle(0, 0, width, height);
        //        BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
        //        int PixelSize = Bitmap.GetPixelFormatSize(bitmapData.PixelFormat) / 8;
        //        ptr[0] = bitmapData.Scan0.ToInt32();
        //        ptr[1] = hpoint.I;
        //        if (width % 4 == 0)
        //            CopyMemory(ptr[0], ptr[1], width * height * PixelSize);
        //        else
        //        {
        //            for (int i = 0; i < height - 1; i++)
        //            {
        //                ptr[1] += width;
        //                CopyMemory(ptr[0], ptr[1], width * PixelSize);
        //                ptr[0] += bitmapData.Stride;
        //            }
        //        }
        //        bitmap.UnlockBits(bitmapData);
        //    }

        //    return bitmap;
        //}
        
        public static Bitmap GetRGBBitmap(this HObject image, double Scale = 4.0) // (20190624) Jeff Revised!
        {
            if (image == null) return null;
            HObject Resize;
            HTuple OrgW, OrgH;
            //double Scale = 4;
            HOperatorSet.GetImageSize(image, out OrgW, out OrgH);
            HOperatorSet.ZoomImageSize(image, out Resize, OrgW / Scale, OrgH / Scale, "nearest_neighbor");
            HTuple num, ptrR, ptrG, ptrB, type, width, height;

            HOperatorSet.CountChannels(Resize, out num);
            if (num == 1)
            {
                HOperatorSet.Compose3(Resize, Resize, Resize, out Resize);
            }
            HOperatorSet.GetImagePointer3(Resize, out ptrR, out ptrG, out ptrB, out type, out width, out height);
            
            int w = width.I, h = height.I;

            Bitmap bitmap = new Bitmap(w, h, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* pointerR = (byte*)ptrR.IP;
                byte* pointerG = (byte*)ptrG.IP;
                byte* pointerB = (byte*)ptrB.IP;

                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                for (int y = 0; y < h; y++)
                {
                    byte* ptr = (byte*)(bitmapData.Scan0 + bitmapData.Stride * y);
                    for (int x = 0; x < w; x++)
                    {
                        ptr[(x * 3) + 0] = (byte)*(pointerB + (y * w) + x);
                        ptr[(x * 3) + 1] = (byte)*(pointerG + (y * w) + x);
                        ptr[(x * 3) + 2] = (byte)*(pointerR + (y * w) + x);
                    }
                }
                bitmap.UnlockBits(bitmapData);
            }
            return bitmap;
        }

        public static Bitmap GetGrayBitmap(this HObject image)
        {
            HTuple hptr, type, width, height;

            HOperatorSet.GetImagePointer1(image, out hptr, out type, out width, out height);

            int w = width.I, h = height.I;

            Bitmap bitmap = new Bitmap(w, h, PixelFormat.Format8bppIndexed);

            unsafe
            {
                byte* pointer = (byte*)hptr.IP;

                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

                for (int y = 0; y < h; y++)
                {
                    byte* ptr = (byte*)(bitmapData.Scan0 + bitmapData.Stride * y);

                    for (int x = 0; x < bitmapData.Stride; x++)
                    {
                        if (x > w) continue; //padding

                        ptr[x] = (byte)*(pointer + (y * w) + x);
                    }
                }
                bitmap.UnlockBits(bitmapData);
            }
            return bitmap;
        }

        public static HTuple GetImageGrayValue(this HObject image)
        {
            HTuple ptr, type, width, height, grayvalue = 0;

            HOperatorSet.GetImagePointer1(image, out ptr, out type, out width, out height);

            unsafe
            {
                int w = width.I;
                int h = height.I;
                byte* bptr = (byte*)ptr.IP;
                long value = 0;

                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        value += *(bptr + (y * w) + x);
                    }
                }
                grayvalue = (double)value / (w * h);
            }

            return grayvalue;
        }

        public static byte GetImageGrayAverage(this HObject grayImage)
        {
            if (grayImage == null) return 0;

            int count = 0;
            double value = 0;
            HTuple type, width, height, ptr;
            HOperatorSet.GetImagePointer1(grayImage, out ptr, out type, out width, out height);

            try
            {
                int w = width.I;
                int h = height.I;
                unsafe
                {
                    byte* bptr = (byte*)ptr.IP;
                    for (int y = 0; y < h; y++)
                        for (int x = 0; x < w; x++)
                        {
                            value += *(bptr + (y * w) + x);
                            count++;
                        }
                }
            }
            catch { }

            return (count == 0) ? byte.MinValue : (byte)(value / count);
        }

        public static void Dispose(HObject hObject)
        {
            if (hObject != null)
                hObject.Dispose();
        }

        public static HObject GetRectangleImage(this HObject image, Rectangle rec)
        {
            HObject imgRec = null;
            if (image == null)
            {
                return imgRec;
            }
            HOperatorSet.CropRectangle1(image, out imgRec, rec.Y, rec.X, rec.Y + rec.Height - 1, rec.X + rec.Width - 1);
            return imgRec;
        }
    }

    public class FastMath
    {
        public static int Abs(int x)
        {
            return (x ^ (x >> 31)) - (x >> 31);
        }

        public static float Abs(float x)
        {
            return x > 0 ? x : -x;
        }

        public static int Max(int x1, int x2)
        {
            return x1 > x2 ? x1 : x2;
        }

        public static int Min(int x1, int x2)
        {
            return x1 < x2 ? x1 : x2;
        }

        public static void Swap(int x1, int x2)
        {
            x1 ^= x2 ^= x1 ^= x2;
        }
    }

    public static class BitmapOperate
    {
        public static Bitmap GetRectangleImage(this Bitmap source, Rectangle rectangle)
        {
            if (source == null)
                return null;

            if (source.PixelFormat != PixelFormat.Format24bppRgb && source.PixelFormat == PixelFormat.Format8bppIndexed)
                return null;

            Bitmap result = new Bitmap(rectangle.Width, rectangle.Height, source.PixelFormat);

            BitmapData sourceData = source.LockBits(rectangle, ImageLockMode.ReadOnly, source.PixelFormat);
            BitmapData resultData = result.LockBits(new Rectangle(0, 0, rectangle.Width, rectangle.Height), ImageLockMode.WriteOnly, source.PixelFormat);

            unsafe
            {
                if (source.PixelFormat == PixelFormat.Format24bppRgb)
                {
                    for (int y = 0; y < rectangle.Height; y++)
                    {
                        byte* srcptr = (byte*)sourceData.Scan0 + (sourceData.Stride * y);
                        byte* dstptr = (byte*)resultData.Scan0 + (resultData.Stride * y);
                        for (int x = 0; x < rectangle.Width; x++)
                        {
                            *(dstptr + ((x * 3) + 0)) = *(srcptr + ((x * 3) + 0));
                            *(dstptr + ((x * 3) + 1)) = *(srcptr + ((x * 3) + 1));
                            *(dstptr + ((x * 3) + 2)) = *(srcptr + ((x * 3) + 2));
                        }
                    }
                }
                else
                {
                    for (int y = 0; y < rectangle.Height; y++)
                    {
                        byte* srcptr = (byte*)sourceData.Scan0 + (sourceData.Stride * y);
                        byte* dstptr = (byte*)resultData.Scan0 + (resultData.Stride * y);
                        for (int x = 0; x < rectangle.Width; x++)
                        {
                            *(dstptr + (x + 0)) = *(srcptr + (x + 0));
                        }
                    }
                }
            }

            source.UnlockBits(sourceData);
            result.UnlockBits(resultData);

            return result;
        }

        public static void SetImageToRectangleRange(this Bitmap source, Bitmap result, Rectangle rectangle)
        {
            if (source == null || result == null)
                return;

            if (source.PixelFormat != PixelFormat.Format24bppRgb && source.PixelFormat == PixelFormat.Format8bppIndexed)
                return;

            BitmapData sourceData = source.LockBits(rectangle, ImageLockMode.WriteOnly, source.PixelFormat);
            BitmapData resultData = result.LockBits(new Rectangle(0, 0, rectangle.Width, rectangle.Height), ImageLockMode.ReadOnly, source.PixelFormat);

            unsafe
            {
                if (source.PixelFormat == PixelFormat.Format24bppRgb)
                {
                    for (int y = 0; y < rectangle.Height; y++)
                    {
                        byte* srcptr = (byte*)sourceData.Scan0 + (sourceData.Stride * y);
                        byte* dstptr = (byte*)resultData.Scan0 + (resultData.Stride * y);
                        for (int x = 0; x < rectangle.Width; x++)
                        {
                            *(srcptr + ((x * 3) + 0)) = *(dstptr + ((x * 3) + 0));
                            *(srcptr + ((x * 3) + 1)) = *(dstptr + ((x * 3) + 1));
                            *(srcptr + ((x * 3) + 2)) = *(dstptr + ((x * 3) + 2));
                        }
                    }
                }
                else
                {
                    for (int y = 0; y < rectangle.Height; y++)
                    {
                        byte* srcptr = (byte*)sourceData.Scan0 + (sourceData.Stride * y);
                        byte* dstptr = (byte*)resultData.Scan0 + (resultData.Stride * y);
                        for (int x = 0; x < rectangle.Width; x++)
                        {
                            *(srcptr + (x + 0)) = *(dstptr + (x + 0));
                        }
                    }
                }
            }

            source.UnlockBits(sourceData);
            result.UnlockBits(resultData);
        }

        public static Bitmap GetCompareImage(this Bitmap source, Bitmap result)
        {
            if (source == null || result == null)
                return null;

            if (source.Width != result.Width || source.Height != result.Height)
                return null;

            if (source.PixelFormat != result.PixelFormat)
                return null;

            if (source.PixelFormat != PixelFormat.Format8bppIndexed && source.PixelFormat != PixelFormat.Format24bppRgb)
                return null;

            Bitmap compare = new Bitmap(source.Width, source.Height, source.PixelFormat);

            Rectangle rectangle = new Rectangle(0, 0, source.Width, source.Height);

            BitmapData srcData = source.LockBits(rectangle, ImageLockMode.ReadOnly, source.PixelFormat);
            BitmapData dstData = result.LockBits(rectangle, ImageLockMode.ReadOnly, source.PixelFormat);
            BitmapData compareData = compare.LockBits(rectangle, ImageLockMode.WriteOnly, source.PixelFormat);


            unsafe
            {
                if (source.PixelFormat == PixelFormat.Format24bppRgb)
                {
                    for (int y = 0; y < rectangle.Height; y++)
                    {
                        byte* srcptr = (byte*)srcData.Scan0 + (srcData.Stride * y);
                        byte* dstptr = (byte*)dstData.Scan0 + (dstData.Stride * y);
                        byte* compareptr = (byte*)compareData.Scan0 + (compareData.Stride * y);
                        for (int x = 0; x < rectangle.Width; x++)
                        {
                            *(compareptr + ((x * 3) + 0)) = (byte)FastMath.Abs(*(dstptr + ((x * 3) + 0)) - *(srcptr + ((x * 3) + 0)));
                            *(compareptr + ((x * 3) + 1)) = (byte)FastMath.Abs(*(dstptr + ((x * 3) + 1)) - *(srcptr + ((x * 3) + 1)));
                            *(compareptr + ((x * 3) + 2)) = (byte)FastMath.Abs(*(dstptr + ((x * 3) + 2)) - *(srcptr + ((x * 3) + 2)));
                        }
                    }
                }
                else
                {
                    for (int y = 0; y < rectangle.Height; y++)
                    {
                        byte* srcptr = (byte*)srcData.Scan0 + (srcData.Stride * y);
                        byte* dstptr = (byte*)dstData.Scan0 + (dstData.Stride * y);
                        byte* compareptr = (byte*)compareData.Scan0 + (compareData.Stride * y);
                        for (int x = 0; x < rectangle.Width; x++)
                        {
                            *(compareptr + x) = (byte)FastMath.Abs(*(dstptr + x) - *(srcptr + x));
                        }
                    }
                }
            }

            source.UnlockBits(srcData);
            result.UnlockBits(dstData);
            compare.UnlockBits(compareData);

            return compare;
        }

        public static Bitmap GetGrayImage(this Bitmap source)
        {
            if (source == null || source.PixelFormat != PixelFormat.Format24bppRgb)
                return null;

            Bitmap result = new Bitmap(source.Width, source.Height, PixelFormat.Format24bppRgb);

            Rectangle rectangle = new Rectangle(0, 0, source.Width, source.Height);

            BitmapData sourceData = source.LockBits(rectangle, ImageLockMode.ReadOnly, source.PixelFormat);
            BitmapData resultData = result.LockBits(new Rectangle(0, 0, rectangle.Width, rectangle.Height), ImageLockMode.WriteOnly, source.PixelFormat);

            unsafe
            {
                for (int y = 0; y < rectangle.Height; y++)
                {
                    byte* srcptr = (byte*)sourceData.Scan0 + (sourceData.Stride * y);
                    byte* dstptr = (byte*)resultData.Scan0 + (resultData.Stride * y);
                    for (int x = 0; x < rectangle.Width; x++)
                    {
                        byte value = (byte)(0.114 * *(srcptr + ((x * 3) + 0)) + 0.587 * *(srcptr + ((x * 3) + 1)) + 0.299 * *(srcptr + ((x * 3) + 2)));
                        *(dstptr + (x * 3) + 0) = value;
                        *(dstptr + (x * 3) + 1) = value;
                        *(dstptr + (x * 3) + 2) = value;
                    }
                }
            }

            source.UnlockBits(sourceData);
            result.UnlockBits(resultData);

            return result;
        }

        public static Bitmap GetThresholdImage(this Bitmap source, byte min, byte max)
        {
            if (source == null || source.PixelFormat != PixelFormat.Format24bppRgb)
                return null;

            Bitmap result = new Bitmap(source.Width, source.Height, PixelFormat.Format24bppRgb);

            Rectangle rectangle = new Rectangle(0, 0, source.Width, source.Height);

            BitmapData sourceData = source.LockBits(rectangle, ImageLockMode.ReadOnly, source.PixelFormat);
            BitmapData resultData = result.LockBits(new Rectangle(0, 0, rectangle.Width, rectangle.Height), ImageLockMode.WriteOnly, source.PixelFormat);

            unsafe
            {
                for (int y = 0; y < rectangle.Height; y++)
                {
                    byte* srcptr = (byte*)sourceData.Scan0 + (sourceData.Stride * y);
                    byte* dstptr = (byte*)resultData.Scan0 + (resultData.Stride * y);
                    for (int x = 0; x < rectangle.Width; x++)
                    {
                        byte value = (*(srcptr + (x * 3)) >= min && *(srcptr + (x * 3)) <= max) ? byte.MaxValue : byte.MinValue;
                        *(dstptr + (x * 3) + 0) = value;
                        *(dstptr + (x * 3) + 1) = value;
                        *(dstptr + (x * 3) + 2) = value;
                    }
                }
            }

            source.UnlockBits(sourceData);
            result.UnlockBits(resultData);

            return result;
        }

        public static Bitmap Trans8BitTo24BitBitmap(this Bitmap source)
        {
            if (source.PixelFormat != PixelFormat.Format8bppIndexed)
                return null;

            Bitmap result = new Bitmap(source.Width, source.Height, PixelFormat.Format24bppRgb);

            unsafe
            {
                Rectangle rec = new Rectangle(0, 0, source.Width, source.Height);

                BitmapData srcData = source.LockBits(rec, ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
                BitmapData rstData = result.LockBits(rec, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                for (int y = 0; y < source.Height; y++)
                {
                    for (int x = 0; x < source.Width; x++)
                    {
                        byte value = (byte)*((byte*)srcData.Scan0 + srcData.Stride * y + x);

                        *((byte*)rstData.Scan0 + rstData.Stride * y + (x * 3) + 0) = value;
                        *((byte*)rstData.Scan0 + rstData.Stride * y + (x * 3) + 1) = value;
                        *((byte*)rstData.Scan0 + rstData.Stride * y + (x * 3) + 2) = value;
                    }
                }

                source.UnlockBits(srcData);
                result.UnlockBits(rstData);
            }

            return result;
        }

        public static HObject GetHObjectFromRGBBitmap(this Bitmap bitmap)
        {
            if (bitmap.PixelFormat != PixelFormat.Format24bppRgb)
                return null;

            HObject image;

            BitmapData bmp_data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);

            byte[] arrayR = new byte[bmp_data.Width * bmp_data.Height];
            byte[] arrayG = new byte[bmp_data.Width * bmp_data.Height];
            byte[] arrayB = new byte[bmp_data.Width * bmp_data.Height];

            unsafe
            {
                byte* pBmp = (byte*)bmp_data.Scan0;
                for (int y = 0; y < bmp_data.Height; y++)
                {
                    byte* pRow = pBmp + bmp_data.Stride * y;
                    for (int x = 0; x < bmp_data.Width; x++)
                    {
                        arrayR[y * bmp_data.Width + x] = *(pRow + (x * 3) + 2);
                        arrayG[y * bmp_data.Width + x] = *(pRow + (x * 3) + 1);
                        arrayB[y * bmp_data.Width + x] = *(pRow + (x * 3) + 0);
                    }
                }

                fixed (byte* pR = arrayR, pG = arrayG, pB = arrayB)
                {
                    HOperatorSet.GenImage3(out image, "byte", bmp_data.Width, bmp_data.Height, new IntPtr(pR), new IntPtr(pG), new IntPtr(pB));
                }
            }

            bitmap.UnlockBits(bmp_data);
            return image;
        }

        public static HObject GetHObjectFromGrayBitmap(this Bitmap bitmap)
        {
            if (bitmap.PixelFormat != PixelFormat.Format8bppIndexed)
                return null;

            HObject image;

            BitmapData bmp_data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);

            byte[] array = new byte[bmp_data.Width * bmp_data.Height];

            unsafe
            {
                for (int y = 0; y < bmp_data.Height; y++)
                {
                    byte* pRow = (byte*)bmp_data.Scan0 + bmp_data.Stride * y;
                    for (int x = 0; x < bmp_data.Width; x++)
                    {
                        array[y * bmp_data.Width + x] = *(pRow + (x * 3) + 0);
                    }
                }

                fixed (byte* pGray = array)
                {
                    HOperatorSet.GenImage1(out image, "byte", bmp_data.Width, bmp_data.Height, new IntPtr(pGray));
                }
            }

            bitmap.UnlockBits(bmp_data);
            return image;
        }
    }

}
