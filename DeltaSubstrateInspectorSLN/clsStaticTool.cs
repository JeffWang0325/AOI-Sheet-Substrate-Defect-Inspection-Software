using PM_24084R;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using DeltaSubstrateInspector.src.Modules.NewInspectModule.InductanceInsp;

using HalconDotNet;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Runtime.Serialization; // (20200429) Jeff Revised!

namespace DeltaSubstrateInspector
{
    public static class clsStaticTool
    {
        #region Function

        /// <summary>
        /// Number of objects in a tuple
        /// </summary>
        /// <param name="hobject"></param>
        /// <returns></returns>
        public static int Count_HObject(HObject hobject) // (20200429) Jeff Revised!
        {
            HTuple count = 0;
            if (hobject != null)
                HOperatorSet.CountObj(hobject, out count);
            return count;
        }

        public static void CloseLibreOffice()
        {
            try
            {
                Process[] CSVProcess = Process.GetProcessesByName("soffice.bin");

                if (CSVProcess.Length > 0)
                {
                    foreach (Process P in CSVProcess)
                    {
                        P.Kill();
                        P.WaitForExit();
                    }
                }
            }
            catch
            { }
        }

        /// <summary>
        /// Color 轉 Halcon Color
        /// </summary>
        /// <param name="ConvertColor"></param>
        /// <returns></returns>
        public static string GetHalconColor(Color ConvertColor) // (20200429) Jeff Revised!
        {
            string colorStr = "";
            try
            {
                colorStr = "#" + ConvertColor.R.ToString("X2") + ConvertColor.G.ToString("X2") + ConvertColor.B.ToString("X2") + ConvertColor.A.ToString("X2");
            }
            catch
            { }
            return colorStr;
        }

        /// <summary>
        /// Halcon Color 轉 Color
        /// </summary>
        /// <param name="str_Color_Halcon"></param>
        /// <returns></returns>
        public static Color GetSystemColor(string str_Color_Halcon) // (20200429) Jeff Revised!
        {
            Color Res = Color.Red;
            try
            {
                if (str_Color_Halcon.Length == 9)
                    Res = Color.FromArgb(Convert.ToInt32(str_Color_Halcon.Substring(7, 2), 16), Convert.ToInt32(str_Color_Halcon.Substring(1, 2), 16),
                                         Convert.ToInt32(str_Color_Halcon.Substring(3, 2), 16), Convert.ToInt32(str_Color_Halcon.Substring(5, 2), 16));
                else
                    Res = Color.FromArgb(Convert.ToInt32(str_Color_Halcon.Substring(1, 2), 16), Convert.ToInt32(str_Color_Halcon.Substring(3, 2), 16), Convert.ToInt32(str_Color_Halcon.Substring(5, 2), 16));
            }
            catch
            {
                Res = Color.Red;
            }
            return Res;
        }

        public static void DisposeAll(this IEnumerable set)
        {
            if (set == null)
                return;
            foreach (Object obj in set)
            {
                IDisposable disp = obj as IDisposable;
                if (disp != null)
                {
                    disp.Dispose();
                    disp = null;
                }
            }
        }

        public static T Clone<T>(T Src)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, Src);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }

        public static void DirectoryCopy(string srcDirName, string dstDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(srcDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exits or could not be found: " + srcDirName);
            }

            if (!Directory.Exists(dstDirName))
            {
                Directory.CreateDirectory(dstDirName);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(dstDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(dstDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        public static string ReadAIClass(string Path, string Part, string Name)
        {
            string Res;
            clsWinAPI.GetPrivateProfileString(Part, Name, "0", clsWinAPI.GetString, 255, Path);
            Res = clsWinAPI.GetString.ToString();
            return Res;
        }

        /// <summary>
        /// HObject 轉 List<HObject>
        /// </summary>
        /// <param name="concatHObject"></param>
        /// <returns></returns>
        public static List<HObject> concatHObject_2_ListHObject(HObject concatHObject) // (20190729) Jeff Revised!
        {
            List<HObject> ListHObject = new List<HObject>();
            if (concatHObject == null)
                return ListHObject;

            for (int i = 1; i <= concatHObject.CountObj(); i++)
            {
                //ListHObject.Add(concatHObject[i]);
                ListHObject.Add(concatHObject.SelectObj(i));
            }

            return ListHObject;
        }

        /// <summary>
        /// List<HObject> 轉 HObject
        /// </summary>
        /// <param name="ListHObject"></param>
        /// <returns></returns>
        public static HObject ListHObject_2_concatHObject(List<HObject> ListHObject) // (20190729) Jeff Revised!
        {
            HObject concatHObject;
            HOperatorSet.GenEmptyObj(out concatHObject);
            foreach (HObject reg in ListHObject)
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.ConcatObj(concatHObject, reg, out ExpTmpOutVar_0);
                concatHObject.Dispose();
                concatHObject = ExpTmpOutVar_0;
            }

            return concatHObject;
        }

        /// <summary>
        /// List<int> 轉 HTuple
        /// </summary>
        /// <param name="ListInt"></param>
        /// <returns></returns>
        public static HTuple ListInt_2_HTuple(List<int> ListInt) // (20190731) Jeff Revised!
        {
            HTuple result = new HTuple();
            foreach (var i in ListInt)
                HOperatorSet.TupleConcat(result, i, out result);

            return result;
        }

        /// <summary>
        /// List<double> 轉 HTuple
        /// </summary>
        /// <param name="ListDouble"></param>
        /// <returns></returns>
        public static HTuple ListDouble_2_HTuple(List<double> ListDouble) // (20190731) Jeff Revised!
        {
            HTuple result = new HTuple();
            foreach (var i in ListDouble)
                HOperatorSet.TupleConcat(result, i, out result);

            return result;
        }

        /// <summary>
        /// HTuple 轉 List<int>
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        public static List<int> HTuple_2_ListInt(HTuple h) // (20190731) Jeff Revised!
        {
            List<int> ListInt = new List<int>();
            for (int i = 0; i < h.Length; i++)
                ListInt.Add(h[i].I);
            return ListInt;
        }

        /// <summary>
        /// HTuple 轉 List<double>
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        public static List<double> HTuple_2_ListDouble(HTuple h) // (20190731) Jeff Revised!
        {
            List<double> ListDouble = new List<double>();
            for (int i = 0; i < h.Length; i++)
                ListDouble.Add(h[i].D);
            return ListDouble;
        }

        /// <summary>
        /// 儲存 XML
        /// </summary>
        /// <param name="SrcProduct"></param>
        /// <param name="PathFile"></param>
        /// <returns></returns>
        public static bool SaveXML(Object SrcProduct, string PathFile) // (20190822) Jeff Revised!
        {
            bool b_status_ = false;
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(PathFile));

            try
            {
                XmlSerializer XmlS = new XmlSerializer(SrcProduct.GetType());
                Stream S = File.Open(PathFile, FileMode.Create);
                XmlS.Serialize(S, SrcProduct);
                S.Close();
                b_status_ = true;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }

            return b_status_;
        }

        /// <summary>
        /// 載入 XML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="PathFile"></param>
        /// <param name="Recipe"></param>
        /// <returns></returns>
        public static bool LoadXML<T>(string PathFile, out T Recipe) // (20200429) Jeff Revised!
        {
            bool b_status_ = false;
            //Recipe = new T();
            Recipe = default(T); // i.e. Recipe = null

            if (File.Exists(PathFile) == false)
                return false;

            try
            {
                //XmlSerializer XmlS = new XmlSerializer(Recipe.GetType());
                XmlSerializer XmlS = new XmlSerializer(typeof(T));
                Stream S = File.Open(PathFile, FileMode.Open);
                Recipe = (T)XmlS.Deserialize(S);
                S.Close();
                b_status_ = true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }

            return b_status_;
        }

        /// <summary>
        /// 物件複製 (深層)
        /// Note: 資料流太長會失敗!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static T DeepClone<T>(this T item) // (20200729) Jeff Revised!
        {
            if (item != null)
            {
                using (var stream = new MemoryStream())
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(stream, item);
                    stream.Seek(0, SeekOrigin.Begin);
                    return (T)formatter.Deserialize(stream);
                }

                /*
                using (var memory = new MemoryStream())
                {
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(memory, item);
                    memory.Seek(0, SeekOrigin.Begin);
                    return (T)(formatter.Deserialize(memory));
                }
                */
            }

            return default(T);
        }

        /// <summary>
        /// 取得影像之Band
        /// </summary>
        /// <param name="SrcImg"></param>
        /// <param name="Band"></param>
        /// <returns></returns>
        public static HObject GetChannelImage(HObject SrcImg, enuBand Band) // (20191226) Jeff Revised!
        {
            HObject OutputImage;
            HOperatorSet.GenEmptyObj(out OutputImage);

            HTuple Ch;
            HOperatorSet.CountChannels(SrcImg, out Ch);
            try
            {
                if (Ch == 1 || Band == enuBand.None) // (20191226) Jeff Revised!
                    OutputImage = SrcImg.Clone();
                else
                {
                    switch (Band)
                    {
                        #region Band類別

                        case enuBand.R:
                            HOperatorSet.AccessChannel(SrcImg, out OutputImage, 1);
                            break;
                        case enuBand.G:
                            HOperatorSet.AccessChannel(SrcImg, out OutputImage, 2);
                            break;
                        case enuBand.B:
                            HOperatorSet.AccessChannel(SrcImg, out OutputImage, 3);
                            break;
                        case enuBand.Gray:
                            HOperatorSet.Rgb1ToGray(SrcImg, out OutputImage);
                            break;
                        case enuBand.y:
                            {
                                HObject R, G, B;
                                HOperatorSet.Decompose3(SrcImg, out R, out G, out B);
                                HObject U, V;
                                HOperatorSet.TransFromRgb(R, G, B, out OutputImage, out U, out V, "yuv");
                                R.Dispose();
                                G.Dispose();
                                B.Dispose();
                                U.Dispose();
                                V.Dispose();
                            }
                            break;
                        case enuBand.u:
                            {
                                HObject R, G, B;
                                HOperatorSet.Decompose3(SrcImg, out R, out G, out B);
                                HObject Y, V;
                                HOperatorSet.TransFromRgb(R, G, B, out Y, out OutputImage, out V, "yuv");
                                R.Dispose();
                                G.Dispose();
                                B.Dispose();
                                Y.Dispose();
                                V.Dispose();
                            }
                            break;
                        case enuBand.v:
                            {
                                HObject R, G, B;
                                HOperatorSet.Decompose3(SrcImg, out R, out G, out B);
                                HObject Y, U;
                                HOperatorSet.TransFromRgb(R, G, B, out Y, out U, out OutputImage, "yuv");
                                R.Dispose();
                                G.Dispose();
                                B.Dispose();
                                Y.Dispose();
                                U.Dispose();
                            }
                            break;
                        case enuBand.v_sub_u:
                            {
                                HObject R, G, B;
                                HOperatorSet.Decompose3(SrcImg, out R, out G, out B);
                                HObject Y, U, V;
                                HOperatorSet.TransFromRgb(R, G, B, out Y, out U, out V, "yuv");
                                HOperatorSet.SubImage(V, U, out OutputImage, 3, 0);
                                R.Dispose();
                                G.Dispose();
                                B.Dispose();
                                Y.Dispose();
                                U.Dispose();
                                V.Dispose();
                            }
                            break;
                        case enuBand.R_sub_B:
                            {
                                HObject R, G, B;
                                HOperatorSet.Decompose3(SrcImg, out R, out G, out B);
                                HOperatorSet.SubImage(R, B, out OutputImage, 1, 128);
                                R.Dispose();
                                G.Dispose();
                                B.Dispose();
                            }
                            break;
                        case enuBand.R_sub_G:
                            {
                                HObject R, G, B;
                                HOperatorSet.Decompose3(SrcImg, out R, out G, out B);
                                HOperatorSet.SubImage(R, G, out OutputImage, 1, 128);
                                R.Dispose();
                                G.Dispose();
                                B.Dispose();
                            }
                            break;
                        case enuBand.B_sub_G:
                            {
                                HObject R, G, B;
                                HOperatorSet.Decompose3(SrcImg, out R, out G, out B);
                                HOperatorSet.SubImage(B, G, out OutputImage, 1, 128);
                                R.Dispose();
                                G.Dispose();
                                B.Dispose();
                            }
                            break;
                        case enuBand.h:
                            {
                                HObject R, G, B;
                                HOperatorSet.Decompose3(SrcImg, out R, out G, out B);
                                HObject h, s, i;
                                HOperatorSet.TransFromRgb(R, G, B, out h, out s, out i, "hsi");
                                OutputImage = h;
                                R.Dispose();
                                G.Dispose();
                                B.Dispose();
                                s.Dispose();
                                i.Dispose();
                            }
                            break;
                        case enuBand.s:
                            {
                                HObject R, G, B;
                                HOperatorSet.Decompose3(SrcImg, out R, out G, out B);
                                HObject h, s, i;
                                HOperatorSet.TransFromRgb(R, G, B, out h, out s, out i, "hsi");
                                OutputImage = s;
                                R.Dispose();
                                G.Dispose();
                                B.Dispose();
                                h.Dispose();
                                i.Dispose();
                            }
                            break;
                        case enuBand.i:
                            {
                                HObject R, G, B;
                                HOperatorSet.Decompose3(SrcImg, out R, out G, out B);
                                HObject h, s, i;
                                HOperatorSet.TransFromRgb(R, G, B, out h, out s, out i, "hsi");
                                OutputImage = i;
                                R.Dispose();
                                G.Dispose();
                                B.Dispose();
                                s.Dispose();
                                h.Dispose();
                            }
                            break;

                            #endregion
                    }

                }
            }
            catch
            { }

            return OutputImage;
        }

        /// <summary>
        /// 影像合成
        /// </summary>
        /// <param name="ImageList"></param>
        /// <param name="Band1"></param>
        /// <param name="Band2"></param>
        /// <param name="Band3"></param>
        /// <param name="Band1Idx"></param>
        /// <param name="Band2Idx"></param>
        /// <param name="Band3Idx"></param>
        /// <param name="b_MixImgBand"></param>
        /// <param name="ImgID"></param>
        /// <param name="CellReg"></param>
        /// <returns></returns>
        public static HObject MixImageBand(List<HObject> ImageList, 
                                           enuBand Band1, enuBand Band2, enuBand Band3, 
                                           int Band1Idx, int Band2Idx, int Band3Idx,
                                           bool b_MixImgBand = true, int ImgID = 0, HObject CellReg = null) // (20190902) Jeff Revised!
        {
            HObject MixImage;
            HOperatorSet.GenEmptyObj(out MixImage);

            try
            {
                List<HObject> ImageList_crop = new List<HObject>();
                if (CellReg != null)
                {
                    HTuple SrcWidth, SrcHeight;
                    HOperatorSet.GetImageSize(ImageList[0], out SrcWidth, out SrcHeight);
                    int Method = 2;
                    if (Method == 1) // 超出影像邊界部分被切掉
                    {
                        // 取出該檢測位置之所有影像
                        HObject Images = ListHObject_2_concatHObject(ImageList);

                        // 計算此Cell Region 位置
                        HTuple hv_Value_R1 = new HTuple(), hv_Value_C1 = new HTuple(), hv_Value_R2 = new HTuple(), hv_Value_C2 = new HTuple();
                        HOperatorSet.RegionFeatures(CellReg, "row1", out hv_Value_R1);
                        HOperatorSet.RegionFeatures(CellReg, "column1", out hv_Value_C1);
                        HOperatorSet.RegionFeatures(CellReg, "row2", out hv_Value_R2);
                        HOperatorSet.RegionFeatures(CellReg, "column2", out hv_Value_C2);

                        // 判斷是否超出影像邊界，如果有則切掉
                        if (hv_Value_R1 < 0) hv_Value_R1 = 0;
                        if (hv_Value_C1 < 0) hv_Value_C1 = 0;
                        if (hv_Value_R2 >= SrcHeight)
                            hv_Value_R2 = SrcHeight - 1;
                        if (hv_Value_C2 >= SrcWidth)
                            hv_Value_C2 = SrcWidth - 1;

                        HObject Imgs_Cells;
                        HOperatorSet.CropRectangle1(Images, out Imgs_Cells, hv_Value_R1, hv_Value_C1, hv_Value_R2, hv_Value_C2);
                        ImageList_crop = concatHObject_2_ListHObject(Imgs_Cells);
                        Imgs_Cells.Dispose();
                        Images.Dispose();
                    }
                    else if (Method == 2) // 超出影像邊界部分無灰階
                    {
                        foreach (HObject img in ImageList)
                        {
                            HObject ReduceImg, CropImg;
                            HOperatorSet.ReduceDomain(img, CellReg, out ReduceImg);
                            HOperatorSet.CropDomain(ReduceImg, out CropImg);
                            ReduceImg.Dispose();
                            HTuple W, H;
                            HOperatorSet.GetImageSize(CropImg, out W, out H);

                            HTuple Width, Height;
                            HOperatorSet.RegionFeatures(CellReg, "width", out Width);
                            HOperatorSet.RegionFeatures(CellReg, "height", out Height);
                            int InspRectWidth = (int)(Width.D), 
                                InspRectHeight = (int)(Height.D);
                            
                            if (W != InspRectWidth || H != InspRectHeight)
                            {
                                HTuple row1, row2, column1, column2;
                                HOperatorSet.RegionFeatures(CellReg, "row1", out row1);
                                HOperatorSet.RegionFeatures(CellReg, "row2", out row2);
                                HOperatorSet.RegionFeatures(CellReg, "column1", out column1);
                                HOperatorSet.RegionFeatures(CellReg, "column2", out column2);

                                if (row2 > SrcHeight)
                                {
                                    HOperatorSet.ChangeFormat(CropImg, out CropImg, InspRectWidth, InspRectHeight);
                                }
                                if (column2 > SrcWidth)
                                {
                                    HOperatorSet.ChangeFormat(CropImg, out CropImg, InspRectWidth, InspRectHeight);
                                }
                                if (row1 < 0)
                                {
                                    HTuple ROffset = -row1;
                                    HTuple COffset = 0;
                                    HOperatorSet.TileImagesOffset(CropImg, out CropImg, ROffset, COffset, -1, -1, -1, -1, InspRectWidth, InspRectHeight);
                                }
                                if (column1 < 0)
                                {
                                    HTuple ROffset = 0;
                                    HTuple COffset = -column1;
                                    HOperatorSet.TileImagesOffset(CropImg, out CropImg, ROffset, COffset, -1, -1, -1, -1, InspRectWidth, InspRectHeight);
                                }
                            }

                            ImageList_crop.Add(CropImg);

                        }
                    }
                    
                }

                if (!b_MixImgBand)
                {
                    MixImage.Dispose();
                    if (CellReg == null)
                        return ImageList[ImgID];
                    else
                    {
                        MixImage = ImageList_crop[ImgID].Clone();
                        DisposeAll(ImageList_crop);
                        return MixImage;
                    }
                }

                // Mix Band
                HObject B1, B2, B3;
                if (CellReg == null)
                {
                    B1 = GetChannelImage(ImageList[Band1Idx], Band1);
                    B2 = GetChannelImage(ImageList[Band2Idx], Band2);
                    B3 = GetChannelImage(ImageList[Band3Idx], Band3);
                }
                else
                {
                    B1 = GetChannelImage(ImageList_crop[Band1Idx], Band1);
                    B2 = GetChannelImage(ImageList_crop[Band2Idx], Band2);
                    B3 = GetChannelImage(ImageList_crop[Band3Idx], Band3);
                    
                    DisposeAll(ImageList_crop);
                }

                HOperatorSet.Compose3(B1.Clone(), B2.Clone(), B3.Clone(), out MixImage);
                B1.Dispose();
                B2.Dispose();
                B3.Dispose();
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }

            return MixImage;
        }

        /// <summary>
        /// 除TriggerCtrl之外，禁能/致能所有其他控制項
        /// Note: 維持 Panel, GroupBox, HSmartWindowControl & HWindowControl 之原先啟用狀態
        /// </summary>
        /// <param name="ParentCtrl"></param>
        /// <param name="TriggerCtrl"></param>
        /// <param name="Enabled"></param>
        public static void EnableAllControls(Control ParentCtrl, Control TriggerCtrl, bool Enabled) // (20200429) Jeff Revised!
        {
            foreach (Control Ctrl in ParentCtrl.Controls)
            {
                if (Ctrl == TriggerCtrl)
                    continue;

                if ((Ctrl is HSmartWindowControl) || (Ctrl is HWindowControl))
                    continue;

                // child control
                clsStaticTool.EnableAllControls(Ctrl, TriggerCtrl, Enabled);

                if ((Ctrl is Panel) || (Ctrl is GroupBox)) // 維持 Panel & GroupBox 之原先啟用狀態
                    continue;

                Ctrl.Enabled = Enabled;
            }
        }

        /// <summary>
        /// 除 TriggerCtrl 及 ListCtrl_Bypass 之外，禁能/致能所有其他控制項
        /// Note: 維持 Panel, GroupBox, HSmartWindowControl & HWindowControl 之原先啟用狀態
        /// </summary>
        /// <param name="ParentCtrl"></param>
        /// <param name="TriggerCtrl"></param>
        /// <param name="Enabled"></param>
        /// <param name="listCtrl_Bypass">維持原先啟用狀態之控制項</param>
        /// <returns>回傳狀態沒被變更之控制項</returns>
        public static List<Control> EnableAllControls_Bypass(Control ParentCtrl, Control TriggerCtrl, bool Enabled, List<Control> listCtrl_Bypass = null) // (20200429) Jeff Revised!
        {
            List<Control> listCtrl_NoChanged = new List<Control>();
            List<Control> listCtl_Bypass_clone = null; // (20191214) Jeff Revised!
            if (listCtrl_Bypass != null)
                listCtl_Bypass_clone = listCtrl_Bypass.ToList();

            foreach (Control Ctrl in ParentCtrl.Controls)
            {
                if (Ctrl == TriggerCtrl)
                    continue;

                if ((Ctrl is HSmartWindowControl) || (Ctrl is HWindowControl))
                    continue;

                // child control
                listCtrl_NoChanged.AddRange(clsStaticTool.EnableAllControls_Bypass(Ctrl, TriggerCtrl, Enabled, listCtl_Bypass_clone));

                if ((Ctrl is Panel) || (Ctrl is GroupBox)) // 維持 Panel & GroupBox 之原先啟用狀態
                    continue;

                if (listCtl_Bypass_clone != null) // 判斷是否為欲維持原先啟用狀態之控制項
                {
                    if (listCtl_Bypass_clone.Remove(Ctrl))
                        continue;
                }

                if (Ctrl.Enabled != Enabled)
                    Ctrl.Enabled = Enabled;
                else // 狀態沒被變更之控制項
                    listCtrl_NoChanged.Add(Ctrl);
            }
            return listCtrl_NoChanged;
        }

        /// <summary>
        /// 創建繪製物件
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Recipe"></param>
        /// <returns></returns>
        public static HDrawingObject GetDrawObj(HDrawingObject.HDrawingObjectType Type, HTuple ImgWidth, HTuple ImgHeight) // (20191226) Jeff Revised!
        {
            HDrawingObject drawing_Rect = new HDrawingObject(0, 0, 100, 100);
            HTuple GoldenCenterX = ImgWidth / 2,
                   GoldenCenterY = ImgHeight / 2;

            if (Type == HDrawingObject.HDrawingObjectType.RECTANGLE1)
            {
                drawing_Rect = HDrawingObject.CreateDrawingObject(Type,
                                                                  GoldenCenterY - 100,
                                                                  GoldenCenterX - 100,
                                                                  GoldenCenterY + 100,
                                                                  GoldenCenterX + 100);
            }
            else if (Type == HDrawingObject.HDrawingObjectType.RECTANGLE2)
            {
                drawing_Rect = HDrawingObject.CreateDrawingObject(Type,
                                                                  GoldenCenterY,
                                                                  GoldenCenterX,
                                                                  0, 100, 100);
            }
            else if (Type == HDrawingObject.HDrawingObjectType.CIRCLE)
            {
                drawing_Rect = HDrawingObject.CreateDrawingObject(Type,
                                                                  GoldenCenterY,
                                                                  GoldenCenterX,
                                                                  100);
            }
            else
            {
                drawing_Rect = HDrawingObject.CreateDrawingObject(Type,
                                                                  GoldenCenterY,
                                                                  GoldenCenterX,
                                                                  0, 100, 100);
            }

            return drawing_Rect;
        }

        /// <summary>
        /// 取得使用者繪製region相關資訊
        /// </summary>
        /// <param name="DrawID"></param>
        /// <param name="Type"></param>
        /// <param name="Region"></param>
        /// <param name="CenterX"></param>
        /// <param name="CenterY"></param>
        public static void GetDrawRegionRes(HTuple DrawID, HDrawingObject.HDrawingObjectType Type, out HObject Region, out HTuple CenterX, out HTuple CenterY) // (20191226) Jeff Revised!
        {
            HOperatorSet.GenEmptyObj(out Region);
            CenterX = 0;
            CenterY = 0;
            if (Type == HDrawingObject.HDrawingObjectType.RECTANGLE1)
            {
                HTuple row1, column1, row2, column2;
                GetRectData(DrawID, out row1, out column1, out row2, out column2);
                HOperatorSet.GenRectangle1(out Region, row1, column1, row2, column2);
                CenterX = (column2 + column1) / 2;
                CenterY = (row2 + row1) / 2;
            }
            else if (Type == HDrawingObject.HDrawingObjectType.RECTANGLE2)
            {
                HTuple row, column, phi, length1, length2;
                GetRect2Data(DrawID, out row, out column, out phi, out length1, out length2);
                HOperatorSet.GenRectangle2(out Region, row, column, phi, length1, length2);
                CenterX = column;
                CenterY = row;
            }
            else if (Type == HDrawingObject.HDrawingObjectType.CIRCLE)
            {
                HTuple row, column, radius;
                GetCircleData(DrawID, out row, out column, out radius);
                HOperatorSet.GenCircle(out Region, row, column, radius);
                CenterX = column;
                CenterY = row;
            }
            else
            {
                HTuple row, column, phi, radius1, radius2;
                GetEllipseData(DrawID, out row, out column, out phi, out radius1, out radius2);
                HOperatorSet.GenEllipse(out Region, row, column, phi, radius1, radius2);
                CenterX = column;
                CenterY = row;
            }
        }

        /// <summary>
        /// 取得 Rectangle1 數據
        /// </summary>
        /// <param name="drawID"></param>
        /// <param name="row1"></param>
        /// <param name="column1"></param>
        /// <param name="row2"></param>
        /// <param name="column2"></param>
        public static void GetRectData(HTuple drawID, out HTuple row1, out HTuple column1, out HTuple row2, out HTuple column2) // (20191226) Jeff Revised!
        {
            try
            {
                HOperatorSet.GetDrawingObjectParams(drawID, new HTuple("row1"), out row1);
                HOperatorSet.GetDrawingObjectParams(drawID, new HTuple("column1"), out column1);
                HOperatorSet.GetDrawingObjectParams(drawID, new HTuple("row2"), out row2);
                HOperatorSet.GetDrawingObjectParams(drawID, new HTuple("column2"), out column2);
            }
            catch
            {
                row1 = column1 = row2 = column2 = null;
            }
        }

        /// <summary>
        /// 取得 Rectangle2 數據
        /// </summary>
        /// <param name="drawID"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="phi"></param>
        /// <param name="length1"></param>
        /// <param name="length2"></param>
        public static void GetRect2Data(HTuple drawID, out HTuple row, out HTuple column, out HTuple phi, out HTuple length1, out HTuple length2) // (20191226) Jeff Revised!
        {
            try
            {
                HOperatorSet.GetDrawingObjectParams(drawID, new HTuple("row"), out row);
                HOperatorSet.GetDrawingObjectParams(drawID, new HTuple("column"), out column);
                HOperatorSet.GetDrawingObjectParams(drawID, new HTuple("phi"), out phi);
                HOperatorSet.GetDrawingObjectParams(drawID, new HTuple("length1"), out length1);
                HOperatorSet.GetDrawingObjectParams(drawID, new HTuple("length2"), out length2);
            }
            catch
            {
                row = column = phi = length1 = length2 = null;
            }
        }

        /// <summary>
        /// 取得 Circle 數據
        /// </summary>
        /// <param name="drawID"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="radius"></param>
        public static void GetCircleData(HTuple drawID, out HTuple row, out HTuple column, out HTuple radius) // (20191226) Jeff Revised!
        {
            try
            {
                HOperatorSet.GetDrawingObjectParams(drawID, new HTuple("row"), out row);
                HOperatorSet.GetDrawingObjectParams(drawID, new HTuple("column"), out column);
                HOperatorSet.GetDrawingObjectParams(drawID, new HTuple("radius"), out radius);
            }
            catch
            {
                row = column = radius = null;
            }
        }

        /// <summary>
        /// 取得 Circle 數據
        /// </summary>
        /// <param name="drawID"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="phi"></param>
        /// <param name="radius1"></param>
        /// <param name="radius2"></param>
        public static void GetEllipseData(HTuple drawID, out HTuple row, out HTuple column, out HTuple phi, out HTuple radius1, out HTuple radius2) // (20191226) Jeff Revised!
        {
            try
            {
                HOperatorSet.GetDrawingObjectParams(drawID, new HTuple("row"), out row);
                HOperatorSet.GetDrawingObjectParams(drawID, new HTuple("column"), out column);
                HOperatorSet.GetDrawingObjectParams(drawID, new HTuple("phi"), out phi);
                HOperatorSet.GetDrawingObjectParams(drawID, new HTuple("radius1"), out radius1);
                HOperatorSet.GetDrawingObjectParams(drawID, new HTuple("radius2"), out radius2);
            }
            catch
            {
                row = column = phi = radius1 = radius2 = null;
            }
        }

        /// <summary>
        /// Get all image files under the given path
        /// </summary>
        /// <param name="hv_ImageDirectory"></param>
        /// <param name="hv_Extensions"></param>
        /// <param name="hv_Options"></param>
        /// <param name="hv_ImageFiles"></param>
        public static void list_image_files(HTuple hv_ImageDirectory, HTuple hv_Extensions, HTuple hv_Options, out HTuple hv_ImageFiles) // (20200319) Jeff Revised!
        {
            HTuple hv_HalconImages = null, hv_OS = null;
            HTuple hv_Directories = null, hv_Index = null, hv_Length = null;
            HTuple hv_NetworkDrive = null, hv_Substring = new HTuple();
            HTuple hv_FileExists = new HTuple(), hv_AllFiles = new HTuple();
            HTuple hv_i = new HTuple(), hv_Selection = new HTuple();
            HTuple hv_Extensions_COPY_INP_TMP = hv_Extensions.Clone();
            HTuple hv_ImageDirectory_COPY_INP_TMP = hv_ImageDirectory.Clone();

            // Initialize local and output iconic variables 
            //This procedure returns all files in a given directory
            //with one of the suffixes specified in Extensions.
            //
            //Input parameters:
            //ImageDirectory: as the name says
            //   If a tuple of directories is given, only the images in the first
            //   existing directory are returned.
            //   If a local directory is not found, the directory is searched
            //   under %HALCONIMAGES%/ImageDirectory. If %HALCONIMAGES% is not set,
            //   %HALCONROOT%/images is used instead.
            //Extensions: A string tuple containing the extensions to be found
            //   e.g. ['png','tif',jpg'] or others
            //If Extensions is set to 'default' or the empty string '',
            //   all image suffixes supported by HALCON are used.
            //Options: as in the operator list_files, except that the 'files'
            //   option is always used. Note that the 'directories' option
            //   has no effect but increases runtime, because only files are
            //   returned.
            //
            //Output parameter:
            //ImageFiles: A tuple of all found image file names
            //
            if ((int)((new HTuple((new HTuple(hv_Extensions_COPY_INP_TMP.TupleEqual(new HTuple()))).TupleOr(
                new HTuple(hv_Extensions_COPY_INP_TMP.TupleEqual(""))))).TupleOr(new HTuple(hv_Extensions_COPY_INP_TMP.TupleEqual(
                "default")))) != 0)
            {
                hv_Extensions_COPY_INP_TMP = new HTuple();
                hv_Extensions_COPY_INP_TMP[0] = "ima";
                hv_Extensions_COPY_INP_TMP[1] = "tif";
                hv_Extensions_COPY_INP_TMP[2] = "tiff";
                hv_Extensions_COPY_INP_TMP[3] = "gif";
                hv_Extensions_COPY_INP_TMP[4] = "bmp";
                hv_Extensions_COPY_INP_TMP[5] = "jpg";
                hv_Extensions_COPY_INP_TMP[6] = "jpeg";
                hv_Extensions_COPY_INP_TMP[7] = "jp2";
                hv_Extensions_COPY_INP_TMP[8] = "jxr";
                hv_Extensions_COPY_INP_TMP[9] = "png";
                hv_Extensions_COPY_INP_TMP[10] = "pcx";
                hv_Extensions_COPY_INP_TMP[11] = "ras";
                hv_Extensions_COPY_INP_TMP[12] = "xwd";
                hv_Extensions_COPY_INP_TMP[13] = "pbm";
                hv_Extensions_COPY_INP_TMP[14] = "pnm";
                hv_Extensions_COPY_INP_TMP[15] = "pgm";
                hv_Extensions_COPY_INP_TMP[16] = "ppm";
                //
            }
            if ((int)(new HTuple(hv_ImageDirectory_COPY_INP_TMP.TupleEqual(""))) != 0)
            {
                hv_ImageDirectory_COPY_INP_TMP = ".";
            }
            HOperatorSet.GetSystem("image_dir", out hv_HalconImages);
            HOperatorSet.GetSystem("operating_system", out hv_OS);
            if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Win"))) != 0)
            {
                hv_HalconImages = hv_HalconImages.TupleSplit(";");
            }
            else
            {
                hv_HalconImages = hv_HalconImages.TupleSplit(":");
            }
            hv_Directories = hv_ImageDirectory_COPY_INP_TMP.Clone();
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_HalconImages.TupleLength()
                )) - 1); hv_Index = (int)hv_Index + 1)
            {
                hv_Directories = hv_Directories.TupleConcat(((hv_HalconImages.TupleSelect(hv_Index)) + "/") + hv_ImageDirectory_COPY_INP_TMP);
            }
            HOperatorSet.TupleStrlen(hv_Directories, out hv_Length);
            HOperatorSet.TupleGenConst(new HTuple(hv_Length.TupleLength()), 0, out hv_NetworkDrive);
            if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Win"))) != 0)
            {
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Length.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
                {
                    if ((int)(new HTuple(((((hv_Directories.TupleSelect(hv_Index))).TupleStrlen()
                        )).TupleGreater(1))) != 0)
                    {
                        HOperatorSet.TupleStrFirstN(hv_Directories.TupleSelect(hv_Index), 1, out hv_Substring);
                        if ((int)((new HTuple(hv_Substring.TupleEqual("//"))).TupleOr(new HTuple(hv_Substring.TupleEqual(
                            "\\\\")))) != 0)
                        {
                            if (hv_NetworkDrive == null)
                                hv_NetworkDrive = new HTuple();
                            hv_NetworkDrive[hv_Index] = 1;
                        }
                    }
                }
            }
            hv_ImageFiles = new HTuple();
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Directories.TupleLength()
                )) - 1); hv_Index = (int)hv_Index + 1)
            {
                HOperatorSet.FileExists(hv_Directories.TupleSelect(hv_Index), out hv_FileExists);
                if ((int)(hv_FileExists) != 0)
                {
                    HOperatorSet.ListFiles(hv_Directories.TupleSelect(hv_Index), (new HTuple("files")).TupleConcat(
                        hv_Options), out hv_AllFiles);
                    hv_ImageFiles = new HTuple();
                    for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv_Extensions_COPY_INP_TMP.TupleLength()
                        )) - 1); hv_i = (int)hv_i + 1)
                    {
                        HOperatorSet.TupleRegexpSelect(hv_AllFiles, (((".*" + (hv_Extensions_COPY_INP_TMP.TupleSelect(
                            hv_i))) + "$")).TupleConcat("ignore_case"), out hv_Selection);
                        hv_ImageFiles = hv_ImageFiles.TupleConcat(hv_Selection);
                    }
                    HOperatorSet.TupleRegexpReplace(hv_ImageFiles, (new HTuple("\\\\")).TupleConcat(
                        "replace_all"), "/", out hv_ImageFiles);
                    if ((int)(hv_NetworkDrive.TupleSelect(hv_Index)) != 0)
                    {
                        HOperatorSet.TupleRegexpReplace(hv_ImageFiles, (new HTuple("//")).TupleConcat(
                            "replace_all"), "/", out hv_ImageFiles);
                        hv_ImageFiles = "/" + hv_ImageFiles;
                    }
                    else
                    {
                        HOperatorSet.TupleRegexpReplace(hv_ImageFiles, (new HTuple("//")).TupleConcat(
                            "replace_all"), "/", out hv_ImageFiles);
                    }

                    return;
                }
            }

            return;
        }

        public static void set_display_font(HTuple hv_WindowHandle, HTuple hv_Size, HTuple hv_Font, HTuple hv_Bold, HTuple hv_Slant) // (20200717) Jeff Revised!
        {
            // Local control variables 
            HTuple hv_OS = null, hv_Fonts = new HTuple();
            HTuple hv_Style = null, hv_Exception = new HTuple(), hv_AvailableFonts = null;
            HTuple hv_Fdx = null, hv_Indices = new HTuple();
            HTuple hv_Font_COPY_INP_TMP = hv_Font.Clone();
            HTuple hv_Size_COPY_INP_TMP = hv_Size.Clone();

            // Initialize local and output iconic variables 
            //This procedure sets the text font of the current window with
            //the specified attributes.
            //
            //Input parameters:
            //WindowHandle: The graphics window for which the font will be set
            //Size: The font size. If Size=-1, the default of 16 is used.
            //Bold: If set to 'true', a bold font is used
            //Slant: If set to 'true', a slanted font is used
            //
            HOperatorSet.GetSystem("operating_system", out hv_OS);
            // dev_get_preferences(...); only in hdevelop
            // dev_set_preferences(...); only in hdevelop
            if ((int)((new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(new HTuple()))).TupleOr(new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(-1)))) != 0)
            {
                hv_Size_COPY_INP_TMP = 16;
            }
            if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Win"))) != 0)
            {
                //Restore previous behaviour
                hv_Size_COPY_INP_TMP = ((1.13677 * hv_Size_COPY_INP_TMP)).TupleInt();
            }
            if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("Courier"))) != 0)
            {
                hv_Fonts = new HTuple();
                hv_Fonts[0] = "Courier";
                hv_Fonts[1] = "Courier 10 Pitch";
                hv_Fonts[2] = "Courier New";
                hv_Fonts[3] = "CourierNew";
            }
            else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))) != 0)
            {
                hv_Fonts = new HTuple();
                hv_Fonts[0] = "Consolas";
                hv_Fonts[1] = "Menlo";
                hv_Fonts[2] = "Courier";
                hv_Fonts[3] = "Courier 10 Pitch";
                hv_Fonts[4] = "FreeMono";
            }
            else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
            {
                hv_Fonts = new HTuple();
                hv_Fonts[0] = "Luxi Sans";
                hv_Fonts[1] = "DejaVu Sans";
                hv_Fonts[2] = "FreeSans";
                hv_Fonts[3] = "Arial";
            }
            else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
            {
                hv_Fonts = new HTuple();
                hv_Fonts[0] = "Times New Roman";
                hv_Fonts[1] = "Luxi Serif";
                hv_Fonts[2] = "DejaVu Serif";
                hv_Fonts[3] = "FreeSerif";
                hv_Fonts[4] = "Utopia";
            }
            else
            {
                hv_Fonts = hv_Font_COPY_INP_TMP.Clone();
            }
            hv_Style = "";
            if ((int)(new HTuple(hv_Bold.TupleEqual("true"))) != 0)
            {
                hv_Style = hv_Style + "Bold";
            }
            else if ((int)(new HTuple(hv_Bold.TupleNotEqual("false"))) != 0)
            {
                hv_Exception = "Wrong value of control parameter Bold";
                throw new HalconException(hv_Exception);
            }
            if ((int)(new HTuple(hv_Slant.TupleEqual("true"))) != 0)
            {
                hv_Style = hv_Style + "Italic";
            }
            else if ((int)(new HTuple(hv_Slant.TupleNotEqual("false"))) != 0)
            {
                hv_Exception = "Wrong value of control parameter Slant";
                throw new HalconException(hv_Exception);
            }
            if ((int)(new HTuple(hv_Style.TupleEqual(""))) != 0)
            {
                hv_Style = "Normal";
            }
            HOperatorSet.QueryFont(hv_WindowHandle, out hv_AvailableFonts);
            hv_Font_COPY_INP_TMP = "";
            for (hv_Fdx = 0; (int)hv_Fdx <= (int)((new HTuple(hv_Fonts.TupleLength())) - 1); hv_Fdx = (int)hv_Fdx + 1)
            {
                hv_Indices = hv_AvailableFonts.TupleFind(hv_Fonts.TupleSelect(hv_Fdx));
                if ((int)(new HTuple((new HTuple(hv_Indices.TupleLength())).TupleGreater(0))) != 0)
                {
                    if ((int)(new HTuple(((hv_Indices.TupleSelect(0))).TupleGreaterEqual(0))) != 0)
                    {
                        hv_Font_COPY_INP_TMP = hv_Fonts.TupleSelect(hv_Fdx);
                        break;
                    }
                }
            }
            if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual(""))) != 0)
            {
                throw new HalconException("Wrong value of control parameter Font");
            }
            hv_Font_COPY_INP_TMP = (((hv_Font_COPY_INP_TMP + "-") + hv_Style) + "-") + hv_Size_COPY_INP_TMP;
            HOperatorSet.SetFont(hv_WindowHandle, hv_Font_COPY_INP_TMP);
            // dev_set_preferences(...); only in hdevelop

            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hv_WindowHandle"></param>
        /// <param name="hv_String"></param>
        /// <param name="hv_CoordSystem">"window" or "image"</param>
        /// <param name="hv_Row"></param>
        /// <param name="hv_Column"></param>
        /// <param name="hv_Color"></param>
        /// <param name="hv_Box">"true" or "false"</param>
        public static void disp_message(HTuple hv_WindowHandle, HTuple hv_String, HTuple hv_CoordSystem, HTuple hv_Row, HTuple hv_Column, HTuple hv_Color, HTuple hv_Box) // (20200717) Jeff Revised!
        {
            // Local control variables 
            HTuple hv_GenParamName = null, hv_GenParamValue = null;
            HTuple hv_Color_COPY_INP_TMP = hv_Color.Clone();
            HTuple hv_Column_COPY_INP_TMP = hv_Column.Clone();
            HTuple hv_CoordSystem_COPY_INP_TMP = hv_CoordSystem.Clone();
            HTuple hv_Row_COPY_INP_TMP = hv_Row.Clone();

            // Initialize local and output iconic variables 
            //This procedure displays text in a graphics window.
            //
            //Input parameters:
            //WindowHandle: The WindowHandle of the graphics window, where
            //   the message should be displayed
            //String: A tuple of strings containing the text message to be displayed
            //CoordSystem: If set to 'window', the text position is given
            //   with respect to the window coordinate system.
            //   If set to 'image', image coordinates are used.
            //   (This may be useful in zoomed images.)
            //Row: The row coordinate of the desired text position
            //   A tuple of values is allowed to display text at different
            //   positions.
            //Column: The column coordinate of the desired text position
            //   A tuple of values is allowed to display text at different
            //   positions.
            //Color: defines the color of the text as string.
            //   If set to [], '' or 'auto' the currently set color is used.
            //   If a tuple of strings is passed, the colors are used cyclically...
            //   - if |Row| == |Column| == 1: for each new textline
            //   = else for each text position.
            //Box: If Box[0] is set to 'true', the text is written within an orange box.
            //     If set to' false', no box is displayed.
            //     If set to a color string (e.g. 'white', '#FF00CC', etc.),
            //       the text is written in a box of that color.
            //     An optional second value for Box (Box[1]) controls if a shadow is displayed:
            //       'true' -> display a shadow in a default color
            //       'false' -> display no shadow
            //       otherwise -> use given string as color string for the shadow color
            //
            //It is possible to display multiple text strings in a single call.
            //In this case, some restrictions apply:
            //- Multiple text positions can be defined by specifying a tuple
            //  with multiple Row and/or Column coordinates, i.e.:
            //  - |Row| == n, |Column| == n
            //  - |Row| == n, |Column| == 1
            //  - |Row| == 1, |Column| == n
            //- If |Row| == |Column| == 1,
            //  each element of String is display in a new textline.
            //- If multiple positions or specified, the number of Strings
            //  must match the number of positions, i.e.:
            //  - Either |String| == n (each string is displayed at the
            //                          corresponding position),
            //  - or     |String| == 1 (The string is displayed n times).
            //
            //
            //Convert the parameters for disp_text.
            if ((int)((new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(new HTuple()))).TupleOr(new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(new HTuple())))) != 0)
            {
                return;
            }
            if ((int)(new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Row_COPY_INP_TMP = 12;
            }
            if ((int)(new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Column_COPY_INP_TMP = 12;
            }
            //
            //Convert the parameter Box to generic parameters.
            hv_GenParamName = new HTuple();
            hv_GenParamValue = new HTuple();
            if ((int)(new HTuple((new HTuple(hv_Box.TupleLength())).TupleGreater(0))) != 0)
            {
                if ((int)(new HTuple(((hv_Box.TupleSelect(0))).TupleEqual("false"))) != 0)
                {
                    //Display no box
                    hv_GenParamName = hv_GenParamName.TupleConcat("box");
                    hv_GenParamValue = hv_GenParamValue.TupleConcat("false");
                }
                else if ((int)(new HTuple(((hv_Box.TupleSelect(0))).TupleNotEqual("true"))) != 0)
                {
                    //Set a color other than the default.
                    hv_GenParamName = hv_GenParamName.TupleConcat("box_color");
                    hv_GenParamValue = hv_GenParamValue.TupleConcat(hv_Box.TupleSelect(0));
                }
            }
            if ((int)(new HTuple((new HTuple(hv_Box.TupleLength())).TupleGreater(1))) != 0)
            {
                if ((int)(new HTuple(((hv_Box.TupleSelect(1))).TupleEqual("false"))) != 0)
                {
                    //Display no shadow.
                    hv_GenParamName = hv_GenParamName.TupleConcat("shadow");
                    hv_GenParamValue = hv_GenParamValue.TupleConcat("false");
                }
                else if ((int)(new HTuple(((hv_Box.TupleSelect(1))).TupleNotEqual("true"))) != 0)
                {
                    //Set a shadow color other than the default.
                    hv_GenParamName = hv_GenParamName.TupleConcat("shadow_color");
                    hv_GenParamValue = hv_GenParamValue.TupleConcat(hv_Box.TupleSelect(1));
                }
            }
            //Restore default CoordSystem behavior.
            if ((int)(new HTuple(hv_CoordSystem_COPY_INP_TMP.TupleNotEqual("window"))) != 0)
            {
                hv_CoordSystem_COPY_INP_TMP = "image";
            }
            //
            if ((int)(new HTuple(hv_Color_COPY_INP_TMP.TupleEqual(""))) != 0)
            {
                //disp_text does not accept an empty string for Color.
                hv_Color_COPY_INP_TMP = new HTuple();
            }
            //
            HOperatorSet.DispText(hv_WindowHandle, hv_String, hv_CoordSystem_COPY_INP_TMP, hv_Row_COPY_INP_TMP, hv_Column_COPY_INP_TMP, hv_Color_COPY_INP_TMP, hv_GenParamName, hv_GenParamValue);

            return;
        }

        #endregion
    }
}
