using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using System.Diagnostics;
using DAVS;
using HalconDotNet;
using System.IO;

namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.InductanceInsp
{
    public class clsSaveImg
    {
        public class clsPacket
        {
            public string ModuleName;
            public string SB_ID;
            public string MOVE_ID;
            public HObject SrcImg;
            public string Path;
            public HObject NGRegion;
            public HObject OKRegion;
            public string PartNumber;
            public bool SaveAOIImgEnabled;
            public int SaveAOIImgType;
            public int InspRectWidth;
            public int InspRectHeight;



            public clsPacket(HObject pmSrcImg, HObject pmNGRegion, HObject pmOKRegion, bool pmSaveAOIImgEnabled, int pmSaveAOIImgType, int pmInspRectWidth, int pmInspRectHeight, string pmSaveImgPath, string pmModuleName, string pmSB_ID, string pmMOVE_ID, string pmPartNumber)
            {
                this.SrcImg = pmSrcImg.Clone();
                this.Path = string.Copy(pmSaveImgPath);
                this.ModuleName = string.Copy(pmModuleName);
                this.MOVE_ID = string.Copy(pmMOVE_ID);
                this.SB_ID = string.Copy(pmSB_ID);
                this.NGRegion = pmNGRegion.Clone();
                this.OKRegion = pmOKRegion.Clone();
                this.PartNumber = string.Copy(pmPartNumber);
                this.SaveAOIImgEnabled = pmSaveAOIImgEnabled;
                this.SaveAOIImgType = pmSaveAOIImgType;
                this.InspRectWidth = pmInspRectWidth;
                this.InspRectHeight = pmInspRectHeight;
            }

            public void Dispose()
            {
                if (SrcImg != null)
                {
                    SrcImg.Dispose();
                    SrcImg = null;
                }
                if (NGRegion!=null)
                {
                    NGRegion.Dispose();
                    NGRegion = null;
                }
                if (OKRegion != null)
                {
                    OKRegion.Dispose();
                    OKRegion = null;
                }
            }
        }

        #region //===================== 區域變數設置 =====================

        //thread variables
        private Thread Worker;
        private volatile bool Exit = false;
        private const int BufMaxSize = 300;
        private ConcurrentQueue<clsPacket> Buf = new ConcurrentQueue<clsPacket>();

        #endregion

        #region //===================== public 函式設置 ==================
        public clsSaveImg()
        {
            Worker = new Thread(new ThreadStart(WorkerFunc));
            Worker.IsBackground = true;
            Worker.Start();
        }

        public void Dispose()
        {
            Exit = true;
            Worker.Join();

            //clear buf
            while (true)
            {
                clsPacket Packet;
                if (Buf.TryDequeue(out Packet) == false)
                    break;

                Packet.Dispose();
            }
        }

        public bool PushImg(clsPacket Packet)
        {
            if (Buf.Count > BufMaxSize)
            {
                Packet.Dispose();
                return false;
            }

            Buf.Enqueue(Packet);
            return true;
        }

        public void Convert2RGB(HObject SrcImg,out HObject RGBImg)
        {
            HOperatorSet.GenEmptyObj(out RGBImg);

            HTuple Ch;
            HOperatorSet.CountChannels(SrcImg, out Ch);
            if (Ch == 3)
            {
                RGBImg = SrcImg.Clone();
                return;
            }
            else
            {
                HOperatorSet.Compose3(SrcImg.Clone(), SrcImg.Clone(), SrcImg.Clone(), out RGBImg);
            }
        }

        public static bool DeleteDirectory(string target_dir)
        {
            bool result = false;
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);
            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }
            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }
            Directory.Delete(target_dir, false);
            return result;
        }

        public void SaveAOISegImg(HObject SrcImg, HObject NGRegion, HObject OKRegion, bool SaveAOIImgEnabled, int SaveAOIImgType, int InspRectWidth, int InspRectHeight, string Path, string pmModuleName, string pmSBID, string pmMoveID, string pmPartNumber)
        {
            HObject RGBImg;
            Convert2RGB(SrcImg, out RGBImg);

            HTuple ImgWidth, ImgHeight;

            HOperatorSet.GetImageSize(SrcImg, out ImgWidth, out ImgHeight);

            string nowRecipeNameDir = "Default", nowSBIDDir = "00000000", nowMOVEID = "0", nowPartNumber = "Default1234";
            if (!string.IsNullOrEmpty(pmModuleName)) nowRecipeNameDir = pmModuleName;
            if (!string.IsNullOrEmpty(pmSBID)) nowSBIDDir = pmSBID;
            if (!string.IsNullOrEmpty(pmMoveID)) nowMOVEID = pmMoveID;
            if (!string.IsNullOrEmpty(pmPartNumber)) nowPartNumber = pmPartNumber;
            string SaveImgPath = Path + nowPartNumber + "\\" + nowRecipeNameDir + "\\" + nowSBIDDir + "\\";

            if (!Directory.Exists(SaveImgPath))
            {
                Directory.CreateDirectory(SaveImgPath);
            }
            else if (Directory.Exists(SaveImgPath) && nowMOVEID == "1")
            {
                DeleteDirectory(SaveImgPath);
                Directory.CreateDirectory(SaveImgPath);
            }
            string OKPath = SaveImgPath + "OK\\";
            string NGPath = SaveImgPath + "NG\\";
            if (SaveAOIImgEnabled)
            {
                HTuple SrcWidth, SrcHeight;
                HOperatorSet.GetImageSize(SrcImg, out SrcWidth, out SrcHeight);


                string NGFileName = NGPath + nowSBIDDir + "_" + String.Format("{0:00000}", Convert.ToInt32(nowMOVEID));
                string OKFileName = OKPath + nowSBIDDir + "_" + String.Format("{0:00000}", Convert.ToInt32(nowMOVEID));
                HTuple OKObjCount, NGObjCount;
                HOperatorSet.CountObj(NGRegion, out NGObjCount);
                HOperatorSet.CountObj(OKRegion, out OKObjCount);

                switch (SaveAOIImgType)
                {
                    case 0:
                        {
                            #region 儲存所有影像

                            #region 判斷路徑是否存在
                            if (!Directory.Exists(OKPath))
                            {
                                Directory.CreateDirectory(OKPath);
                            }

                            if (!Directory.Exists(NGPath))
                            {
                                Directory.CreateDirectory(NGPath);
                            }

                            #endregion

                            #region 儲存NG
                            for (int i = 0; i < NGObjCount; i++)
                            {
                                HObject ReduceImg, CropImg;
                                HObject SelectRegion;
                                HTuple nowID = i;
                                string hv_nowFileName = ((NGFileName + "_") + (nowID.TupleString("04d")));
                                HOperatorSet.SelectObj(NGRegion, out SelectRegion, nowID + 1);
                                HOperatorSet.ReduceDomain(RGBImg, SelectRegion, out ReduceImg);
                                HOperatorSet.CropDomain(ReduceImg, out CropImg);
                                HTuple W, H;
                                HOperatorSet.GetImageSize(CropImg, out W, out H);
                                if (W != InspRectWidth || H != InspRectHeight)
                                {
                                    HTuple row1, row2, column1, column2;
                                    HOperatorSet.RegionFeatures(SelectRegion, "row1", out row1);
                                    HOperatorSet.RegionFeatures(SelectRegion, "row2", out row2);
                                    HOperatorSet.RegionFeatures(SelectRegion, "column1", out column1);
                                    HOperatorSet.RegionFeatures(SelectRegion, "column2", out column2);

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
                                HOperatorSet.WriteImage(CropImg, "tiff", 0, hv_nowFileName);
                                ReduceImg.Dispose();
                                CropImg.Dispose();
                                SelectRegion.Dispose();
                            }
                            #endregion

                            #region 儲存OK

                            for (int i = 0; i < OKObjCount; i++)
                            {
                                HObject ReduceImg, CropImg;
                                HObject SelectRegion;
                                HTuple nowID = i;
                                string hv_nowFileName = ((OKFileName + "_") + (nowID.TupleString("04d")));
                                HOperatorSet.SelectObj(OKRegion, out SelectRegion, nowID + 1);
                                HOperatorSet.ReduceDomain(RGBImg, SelectRegion, out ReduceImg);
                                HOperatorSet.CropDomain(ReduceImg, out CropImg);
                                HTuple W, H;
                                HOperatorSet.GetImageSize(CropImg, out W, out H);
                                if (W != InspRectWidth || H != InspRectHeight)
                                {
                                    HTuple row1, row2, column1, column2;
                                    HOperatorSet.RegionFeatures(SelectRegion, "row1", out row1);
                                    HOperatorSet.RegionFeatures(SelectRegion, "row2", out row2);
                                    HOperatorSet.RegionFeatures(SelectRegion, "column1", out column1);
                                    HOperatorSet.RegionFeatures(SelectRegion, "column2", out column2);

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
                                HOperatorSet.WriteImage(CropImg, "tiff", 0, hv_nowFileName);
                                ReduceImg.Dispose();
                                CropImg.Dispose();
                                SelectRegion.Dispose();
                            }
                            #endregion

                            #endregion
                        }
                        break;
                    case 1:
                        {
                            #region 儲存NG影像

                            #region 判斷路徑是否存在
                            if (!Directory.Exists(NGPath))
                            {
                                Directory.CreateDirectory(NGPath);
                            }

                            #endregion

                            #region 儲存NG
                            for (int i = 0; i < NGObjCount; i++)
                            {
                                HObject ReduceImg, CropImg;
                                HObject SelectRegion;
                                HTuple nowID = i;
                                string hv_nowFileName = ((NGFileName + "_") + (nowID.TupleString("04d")));
                                HOperatorSet.SelectObj(NGRegion, out SelectRegion, nowID + 1);
                                HOperatorSet.ReduceDomain(RGBImg, SelectRegion, out ReduceImg);
                                HOperatorSet.CropDomain(ReduceImg, out CropImg);
                                HTuple W, H;
                                HOperatorSet.GetImageSize(CropImg, out W, out H);
                                if (W != InspRectWidth || H != InspRectHeight)
                                {
                                    HObject Image;
                                    HOperatorSet.GenImageConst(out Image, "byte", InspRectWidth, InspRectHeight);
                                    HOperatorSet.Compose3(Image.Clone(), Image.Clone(), Image.Clone(), out Image);

                                    HTuple row1, row2, column1, column2;
                                    HOperatorSet.RegionFeatures(SelectRegion, "row1", out row1);
                                    HOperatorSet.RegionFeatures(SelectRegion, "row2", out row2);
                                    HOperatorSet.RegionFeatures(SelectRegion, "column1", out column1);
                                    HOperatorSet.RegionFeatures(SelectRegion, "column2", out column2);

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
                                HOperatorSet.WriteImage(CropImg, "tiff", 0, hv_nowFileName);
                                ReduceImg.Dispose();
                                CropImg.Dispose();
                                SelectRegion.Dispose();
                            }
                            #endregion

                            #endregion
                        }
                        break;
                    case 2:
                        {
                            #region 儲存OK影像

                            #region 判斷路徑是否存在
                            if (!Directory.Exists(OKPath))
                            {
                                Directory.CreateDirectory(OKPath);
                            }

                            #endregion

                            #region 儲存OK

                            for (int i = 0; i < OKObjCount; i++)
                            {
                                HObject ReduceImg, CropImg;
                                HObject SelectRegion;
                                HTuple nowID = i;
                                string hv_nowFileName = ((OKFileName + "_") + (nowID.TupleString("04d")));
                                HOperatorSet.SelectObj(OKRegion, out SelectRegion, nowID + 1);
                                HOperatorSet.ReduceDomain(RGBImg, SelectRegion, out ReduceImg);
                                HOperatorSet.CropDomain(ReduceImg, out CropImg);
                                HTuple W, H;
                                HOperatorSet.GetImageSize(CropImg, out W, out H);
                                if (W != InspRectWidth || H != InspRectHeight)
                                {
                                    HTuple row1, row2, column1, column2;
                                    HOperatorSet.RegionFeatures(SelectRegion, "row1", out row1);
                                    HOperatorSet.RegionFeatures(SelectRegion, "row2", out row2);
                                    HOperatorSet.RegionFeatures(SelectRegion, "column1", out column1);
                                    HOperatorSet.RegionFeatures(SelectRegion, "column2", out column2);

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
                                HOperatorSet.WriteImage(CropImg, "tiff", 0, hv_nowFileName);
                                ReduceImg.Dispose();
                                CropImg.Dispose();
                                SelectRegion.Dispose();
                            }
                            #endregion

                            #endregion
                        }
                        break;
                    default:
                        {

                        }
                        break;
                }
            }

            RGBImg.Dispose();
        }

        #endregion

        #region //=======================  函式設置 ======================
        private void WorkerFunc()
        {
            while (Exit == false)
            {
                SpinWait.SpinUntil(() => false, 10);

                clsPacket Packet;
                if (Buf.TryDequeue(out Packet) == false)
                    continue;

                SaveAOISegImg(Packet.SrcImg,
                              Packet.NGRegion,
                              Packet.OKRegion,
                              Packet.SaveAOIImgEnabled,
                              Packet.SaveAOIImgType,
                              Packet.InspRectWidth,
                              Packet.InspRectHeight,
                              Packet.Path,
                              Packet.ModuleName,
                              Packet.SB_ID,
                              Packet.MOVE_ID, Packet.PartNumber);

                Packet.Dispose();
            }
        }
        #endregion

    }
}
