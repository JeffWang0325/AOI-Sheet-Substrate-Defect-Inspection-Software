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

namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.ResistPanel
{
    public class clsSaveImg
    {
        public class clsPacket
        {
            public string ModuleName;
            public string SB_ID;
            public string MOVE_ID;
            public HObject SrcImg;
            public ResistPanelRole Recipe;
            public string Path;
            public HObject NotsureNGRegion;
            public HObject OKRegion;
            public string PartNumber;
            public HObject AbsoluteNGRegion;


            public clsPacket(HObject pmSrcImg, ResistPanelRole pmRecipe, HObject pmNotsureNGRegion, HObject pmOKRegion,HObject pmAbsoluteNGRegion, string pmSaveImgPath,string pmModuleName,string pmSB_ID,string pmMOVE_ID, string pmPartNumber)
            {
                this.Recipe = pmRecipe;
                this.SrcImg = pmSrcImg.Clone();
                this.Path = string.Copy(pmSaveImgPath);
                this.ModuleName = string.Copy(pmModuleName);
                this.MOVE_ID = string.Copy(pmMOVE_ID);
                this.SB_ID = string.Copy(pmSB_ID);
                if (pmNotsureNGRegion != null)
                    this.NotsureNGRegion = pmNotsureNGRegion.Clone();
                if (pmOKRegion != null)
                    this.OKRegion = pmOKRegion.Clone();
                this.PartNumber = string.Copy(pmPartNumber);
                if (pmAbsoluteNGRegion != null)
                    this.AbsoluteNGRegion = pmAbsoluteNGRegion.Clone();
            }

            public void Dispose()
            {
                if (SrcImg != null)
                {
                    SrcImg.Dispose();
                    SrcImg = null;
                }
                if (NotsureNGRegion != null)
                {
                    NotsureNGRegion.Dispose();
                    NotsureNGRegion = null;
                }
                if (OKRegion != null)
                {
                    OKRegion.Dispose();
                    OKRegion = null;
                }
                if (AbsoluteNGRegion != null)
                {
                    AbsoluteNGRegion.Dispose();
                    AbsoluteNGRegion = null;
                }
            }
        }

        #region //===================== 區域變數設置 =====================

        //thread variables
        private Thread Worker;
        private volatile bool Exit = false;
        private const int BufMaxSize = 150;
        private ConcurrentQueue<clsPacket> Buf = new ConcurrentQueue<clsPacket>();

        #endregion

        #region //===================== public 函式設置 ==================
        public clsSaveImg()
        {
            Worker = new Thread(new ThreadStart(WorkerFunc));
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
        public void Convert2RGB(HObject SrcImg, out HObject RGBImg)
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
        public void SaveAOISegImg(HObject SrcImg, HObject ABSNGRegion, HObject OKRegion,HObject NotSureRegion, ResistPanelRole Recipe, string Path, string pmModuleName, string pmSBID, string pmMoveID,string pmPartNumber)
        {
            clsLog Log = new clsLog();
            Log.WriteLog("SaveImg Start");
            HObject RGBImg;
            Convert2RGB(SrcImg, out RGBImg);
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
            string OKPath = SaveImgPath + "OK\\";
            string AbsoluteNGPath = SaveImgPath + "AbsoluteNG\\";
            string NSPath = SaveImgPath + "NG\\";
            if (Recipe.Param.SaveAOIImgEnabled)
            {
                string NGFileName = AbsoluteNGPath + nowSBIDDir + "_" + String.Format("{0:00000}", Convert.ToInt32(nowMOVEID));
                string OKFileName = OKPath + nowSBIDDir + "_" + String.Format("{0:00000}", Convert.ToInt32(nowMOVEID));
                string NotSureFileName = NSPath + nowSBIDDir + "_" + String.Format("{0:00000}", Convert.ToInt32(nowMOVEID));

                #region Count

                HTuple OKObjCount, NGObjCount,NotsureCount;
                if (NotSureRegion != null)
                    HOperatorSet.CountObj(NotSureRegion, out NotsureCount);
                else
                    NotsureCount = 0;

                if (ABSNGRegion != null)
                    HOperatorSet.CountObj(ABSNGRegion, out NGObjCount);
                else
                    NGObjCount = 0;

                if (OKRegion != null)
                    HOperatorSet.CountObj(OKRegion, out OKObjCount);
                else
                    OKObjCount = 0;

                //HOperatorSet.CountObj(ABSNGRegion, out NGObjCount);
                //HOperatorSet.CountObj(OKRegion, out OKObjCount);

                #endregion

                switch (Recipe.Param.SaveAOIImgType)
                {
                    case 0:
                        {
                            #region 儲存所有影像

                            #region 判斷路徑是否存在
                            if (!Directory.Exists(OKPath))
                            {
                                Directory.CreateDirectory(OKPath);
                            }
                            if (!Directory.Exists(AbsoluteNGPath))
                            {
                                Directory.CreateDirectory(AbsoluteNGPath);
                            }
                            if (!Directory.Exists(NSPath))
                            {
                                Directory.CreateDirectory(NSPath);
                            }
                            #endregion

                            #region 儲存NotSure

                            if (NotsureCount > 0)
                            {
                                HTuple NowFileNames = new HTuple();
                                for (int i = 0; i < NotsureCount; i++)
                                {
                                    HTuple nowID = i;
                                    string hv_nowFileName = ((NotSureFileName + "_") + (nowID.TupleString("04d")));
                                    HOperatorSet.TupleConcat(NowFileNames, hv_nowFileName, out NowFileNames);
                                }

                                HTuple hv_R1 = new HTuple(), hv_C1 = new HTuple(), hv_R2 = new HTuple(), hv_C2 = new HTuple();
                                HOperatorSet.RegionFeatures(NotSureRegion, "row1", out hv_R1);
                                HOperatorSet.RegionFeatures(NotSureRegion, "column1", out hv_C1);
                                HOperatorSet.RegionFeatures(NotSureRegion, "row2", out hv_R2);
                                HOperatorSet.RegionFeatures(NotSureRegion, "column2", out hv_C2);
                                HObject CropImgs;
                                HOperatorSet.CropRectangle1(RGBImg, out CropImgs, hv_R1, hv_C1, hv_R2, hv_C2);
                                HOperatorSet.WriteImage(CropImgs, "tiff", 0, NowFileNames);
                                CropImgs.Dispose();
                            }

                            #endregion

                            #region 儲存NG
                            // Method 1:
                            //for (int i = 0; i < NGObjCount; i++)
                            //{
                            //    HObject ReduceImg, CropImg;
                            //    HObject SelectRegion;
                            //    HTuple nowID = i;
                            //    string hv_nowFileName = ((NGFileName + "_") + (nowID.TupleString("04d")));
                            //    HOperatorSet.SelectObj(NGRegion, out SelectRegion, nowID + 1);
                            //    HOperatorSet.ReduceDomain(RGBImg, SelectRegion, out ReduceImg);
                            //    HOperatorSet.CropDomain(ReduceImg, out CropImg);
                            //    HOperatorSet.WriteImage(CropImg, "tiff", 0, hv_nowFileName);
                            //    ReduceImg.Dispose();
                            //    CropImg.Dispose();
                            //    SelectRegion.Dispose();
                            //}

                            // Method 2: 時間長! (20190411) Jeff Revised!
                            //if (NGObjCount > 0)
                            //{
                            //    HObject ReduceImgs;
                            //    HOperatorSet.GenEmptyObj(out ReduceImgs);
                            //    HTuple NowFileNames = new HTuple();
                            //    for (int i = 0; i < NGObjCount; i++)
                            //    {
                            //        HTuple nowID = i;
                            //        string hv_nowFileName = ((NGFileName + "_") + (nowID.TupleString("04d")));
                            //        HOperatorSet.TupleConcat(NowFileNames, hv_nowFileName, out NowFileNames);

                            //        HObject SelectRegion, ReduceImg;
                            //        HOperatorSet.SelectObj(NGRegion, out SelectRegion, nowID + 1);
                            //        HOperatorSet.ReduceDomain(RGBImg, SelectRegion, out ReduceImg);
                            //        HOperatorSet.ConcatObj(ReduceImgs, ReduceImg, out ReduceImgs);
                            //        SelectRegion.Dispose();
                            //        ReduceImg.Dispose();
                            //    }

                            //    HObject CropImgs;
                            //    HOperatorSet.CropDomain(ReduceImgs, out CropImgs);
                            //    HOperatorSet.WriteImage(CropImgs, "tiff", 0, NowFileNames);
                            //    ReduceImgs.Dispose();
                            //    CropImgs.Dispose();
                            //}

                            // Method 3: (20190411) Jeff Revised!
                            //if (NGObjCount > 0)
                            //{
                            //    HObject CropImgs;
                            //    HOperatorSet.GenEmptyObj(out CropImgs);
                            //    HTuple NowFileNames = new HTuple();
                            //    for (int i = 0; i < NGObjCount; i++)
                            //    {
                            //        HTuple nowID = i;
                            //        string hv_nowFileName = ((NGFileName + "_") + (nowID.TupleString("04d")));
                            //        HOperatorSet.TupleConcat(NowFileNames, hv_nowFileName, out NowFileNames);

                            //        HObject SelectRegion, ReduceImg, CropImg;
                            //        HOperatorSet.SelectObj(NGRegion, out SelectRegion, nowID + 1);
                            //        HOperatorSet.ReduceDomain(RGBImg, SelectRegion, out ReduceImg);
                            //        HOperatorSet.CropDomain(ReduceImg, out CropImg);
                            //        HOperatorSet.ConcatObj(CropImgs, CropImg, out CropImgs);
                            //        SelectRegion.Dispose();
                            //        ReduceImg.Dispose();
                            //        CropImg.Dispose();
                            //    }

                            //    HOperatorSet.WriteImage(CropImgs, "tiff", 0, NowFileNames);
                            //    CropImgs.Dispose();
                            //}

                            // Method 4: (20190411) Jeff Revised!
                            if (NGObjCount > 0)
                            {
                                HTuple NowFileNames = new HTuple();
                                for (int i = 0; i < NGObjCount; i++)
                                {
                                    HTuple nowID = i;
                                    string hv_nowFileName = ((NGFileName + "_") + (nowID.TupleString("04d")));
                                    HOperatorSet.TupleConcat(NowFileNames, hv_nowFileName, out NowFileNames);
                                }

                                HTuple hv_R1 = new HTuple(), hv_C1 = new HTuple(), hv_R2 = new HTuple(), hv_C2 = new HTuple();
                                HOperatorSet.RegionFeatures(ABSNGRegion, "row1", out hv_R1);
                                HOperatorSet.RegionFeatures(ABSNGRegion, "column1", out hv_C1);
                                HOperatorSet.RegionFeatures(ABSNGRegion, "row2", out hv_R2);
                                HOperatorSet.RegionFeatures(ABSNGRegion, "column2", out hv_C2);
                                HObject CropImgs;
                                HOperatorSet.CropRectangle1(RGBImg, out CropImgs, hv_R1, hv_C1, hv_R2, hv_C2);
                                HOperatorSet.WriteImage(CropImgs, "tiff", 0, NowFileNames);
                                CropImgs.Dispose();
                            }
                            #endregion

                            #region 儲存OK
                            // Method 1:
                            //for (int i = 0; i < OKObjCount; i++)
                            //{
                            //    HObject ReduceImg, CropImg;
                            //    HObject SelectRegion;
                            //    HTuple nowID = i;
                            //    string hv_nowFileName = ((OKFileName + "_") + (nowID.TupleString("04d")));
                            //    HOperatorSet.SelectObj(OKRegion, out SelectRegion, nowID + 1);
                            //    HOperatorSet.ReduceDomain(RGBImg, SelectRegion, out ReduceImg);
                            //    HOperatorSet.CropDomain(ReduceImg, out CropImg);
                            //    HOperatorSet.WriteImage(CropImg, "tiff", 0, hv_nowFileName);
                            //    ReduceImg.Dispose();
                            //    CropImg.Dispose();
                            //    SelectRegion.Dispose();
                            //}

                            // Method 2: 時間長! (20190411) Jeff Revised!
                            //if (OKObjCount > 0)
                            //{
                            //    HObject ReduceImgs;
                            //    HOperatorSet.GenEmptyObj(out ReduceImgs);
                            //    HTuple NowFileNames = new HTuple();
                            //    for (int i = 0; i < OKObjCount; i++)
                            //    {
                            //        HTuple nowID = i;
                            //        string hv_nowFileName = ((OKFileName + "_") + (nowID.TupleString("04d")));
                            //        HOperatorSet.TupleConcat(NowFileNames, hv_nowFileName, out NowFileNames);

                            //        HObject SelectRegion, ReduceImg;
                            //        HOperatorSet.SelectObj(OKRegion, out SelectRegion, nowID + 1);
                            //        HOperatorSet.ReduceDomain(RGBImg, SelectRegion, out ReduceImg);
                            //        HOperatorSet.ConcatObj(ReduceImgs, ReduceImg, out ReduceImgs);
                            //        SelectRegion.Dispose();
                            //        ReduceImg.Dispose();
                            //    }

                            //    HObject CropImgs;
                            //    HOperatorSet.CropDomain(ReduceImgs, out CropImgs);
                            //    HOperatorSet.WriteImage(CropImgs, "tiff", 0, NowFileNames);
                            //    ReduceImgs.Dispose();
                            //    CropImgs.Dispose();
                            //}

                            // Method 3: (20190411) Jeff Revised!
                            //if (OKObjCount > 0)
                            //{
                            //    HObject CropImgs;
                            //    HOperatorSet.GenEmptyObj(out CropImgs);
                            //    HTuple NowFileNames = new HTuple();
                            //    for (int i = 0; i < OKObjCount; i++)
                            //    {
                            //        HTuple nowID = i;
                            //        string hv_nowFileName = ((OKFileName + "_") + (nowID.TupleString("04d")));
                            //        HOperatorSet.TupleConcat(NowFileNames, hv_nowFileName, out NowFileNames);

                            //        HObject SelectRegion, ReduceImg, CropImg;
                            //        HOperatorSet.SelectObj(OKRegion, out SelectRegion, nowID + 1);
                            //        HOperatorSet.ReduceDomain(RGBImg, SelectRegion, out ReduceImg);
                            //        HOperatorSet.CropDomain(ReduceImg, out CropImg);
                            //        HOperatorSet.ConcatObj(CropImgs, CropImg, out CropImgs);
                            //        SelectRegion.Dispose();
                            //        ReduceImg.Dispose();
                            //        CropImg.Dispose();
                            //    }

                            //    HOperatorSet.WriteImage(CropImgs, "tiff", 0, NowFileNames);
                            //    CropImgs.Dispose();
                            //}

                            // Method 4: (20190411) Jeff Revised!
                            if (OKObjCount > 0)
                            {
                                HTuple nowID;
                                HTuple NowFileNames = new HTuple();
                                for (int i = 0; i < OKObjCount; i++)
                                {
                                    nowID = i;
                                    string hv_nowFileName = ((OKFileName + "_") + (nowID.TupleString("04d")));
                                    HOperatorSet.TupleConcat(NowFileNames, hv_nowFileName, out NowFileNames);
                                }

                                HTuple hv_R1 = new HTuple(), hv_C1 = new HTuple(), hv_R2 = new HTuple(), hv_C2 = new HTuple();
                                HOperatorSet.RegionFeatures(OKRegion, "row1", out hv_R1);
                                HOperatorSet.RegionFeatures(OKRegion, "column1", out hv_C1);
                                HOperatorSet.RegionFeatures(OKRegion, "row2", out hv_R2);
                                HOperatorSet.RegionFeatures(OKRegion, "column2", out hv_C2);
                                HObject CropImgs;
                                HOperatorSet.CropRectangle1(RGBImg, out CropImgs, hv_R1, hv_C1, hv_R2, hv_C2);
                                HOperatorSet.WriteImage(CropImgs, "tiff", 0, NowFileNames);
                                CropImgs.Dispose();
                            }
                            #endregion

                            #endregion
                        }
                        break;
                    case 1:
                        {
                            #region 儲存NG影像

                            #region 判斷路徑是否存在
                            if (!Directory.Exists(AbsoluteNGPath))
                            {
                                Directory.CreateDirectory(AbsoluteNGPath);
                            }
                            if (!Directory.Exists(NSPath))
                            {
                                Directory.CreateDirectory(NSPath);
                            }
                            #endregion

                            #region 儲存NotSure

                            if (NotsureCount > 0)
                            {
                                HTuple NowFileNames = new HTuple();
                                for (int i = 0; i < NotsureCount; i++)
                                {
                                    HTuple nowID = i;
                                    string hv_nowFileName = ((NotSureFileName + "_") + (nowID.TupleString("04d")));
                                    HOperatorSet.TupleConcat(NowFileNames, hv_nowFileName, out NowFileNames);
                                }

                                HTuple hv_R1 = new HTuple(), hv_C1 = new HTuple(), hv_R2 = new HTuple(), hv_C2 = new HTuple();
                                HOperatorSet.RegionFeatures(NotSureRegion, "row1", out hv_R1);
                                HOperatorSet.RegionFeatures(NotSureRegion, "column1", out hv_C1);
                                HOperatorSet.RegionFeatures(NotSureRegion, "row2", out hv_R2);
                                HOperatorSet.RegionFeatures(NotSureRegion, "column2", out hv_C2);
                                HObject CropImgs;
                                HOperatorSet.CropRectangle1(RGBImg, out CropImgs, hv_R1, hv_C1, hv_R2, hv_C2);
                                HOperatorSet.WriteImage(CropImgs, "tiff", 0, NowFileNames);
                                CropImgs.Dispose();
                            }

                            #endregion

                            #region 儲存NG

                            if (NGObjCount > 0)
                            {
                                HTuple NowFileNames = new HTuple();
                                for (int i = 0; i < NGObjCount; i++)
                                {
                                    HTuple nowID = i;
                                    string hv_nowFileName = ((NGFileName + "_") + (nowID.TupleString("04d")));
                                    HOperatorSet.TupleConcat(NowFileNames, hv_nowFileName, out NowFileNames);
                                }

                                HTuple hv_R1 = new HTuple(), hv_C1 = new HTuple(), hv_R2 = new HTuple(), hv_C2 = new HTuple();
                                HOperatorSet.RegionFeatures(ABSNGRegion, "row1", out hv_R1);
                                HOperatorSet.RegionFeatures(ABSNGRegion, "column1", out hv_C1);
                                HOperatorSet.RegionFeatures(ABSNGRegion, "row2", out hv_R2);
                                HOperatorSet.RegionFeatures(ABSNGRegion, "column2", out hv_C2);
                                HObject CropImgs;
                                HOperatorSet.CropRectangle1(RGBImg, out CropImgs, hv_R1, hv_C1, hv_R2, hv_C2);
                                HOperatorSet.WriteImage(CropImgs, "tiff", 0, NowFileNames);
                                CropImgs.Dispose();
                            }

                            //for (int i = 0; i < NGObjCount; i++)
                            //{
                            //    HObject ReduceImg, CropImg;
                            //    HObject SelectRegion;
                            //    HTuple nowID = i;
                            //    string hv_nowFileName = ((NGFileName + "_") + (nowID.TupleString("04d")));
                            //    HOperatorSet.SelectObj(NGRegion, out SelectRegion, nowID + 1);
                            //    HOperatorSet.ReduceDomain(RGBImg, SelectRegion, out ReduceImg);
                            //    HOperatorSet.CropDomain(ReduceImg, out CropImg);
                            //    HOperatorSet.WriteImage(CropImg, "tiff", 0, hv_nowFileName);
                            //    ReduceImg.Dispose();
                            //    CropImg.Dispose();
                            //    SelectRegion.Dispose();
                            //}
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
                            if (!Directory.Exists(NSPath))
                            {
                                Directory.CreateDirectory(NSPath);
                            }
                            #endregion

                            #region 儲存NotSure

                            if (NotsureCount > 0)
                            {
                                HTuple NowFileNames = new HTuple();
                                for (int i = 0; i < NotsureCount; i++)
                                {
                                    HTuple nowID = i;
                                    string hv_nowFileName = ((NotSureFileName + "_") + (nowID.TupleString("04d")));
                                    HOperatorSet.TupleConcat(NowFileNames, hv_nowFileName, out NowFileNames);
                                }

                                HTuple hv_R1 = new HTuple(), hv_C1 = new HTuple(), hv_R2 = new HTuple(), hv_C2 = new HTuple();
                                HOperatorSet.RegionFeatures(NotSureRegion, "row1", out hv_R1);
                                HOperatorSet.RegionFeatures(NotSureRegion, "column1", out hv_C1);
                                HOperatorSet.RegionFeatures(NotSureRegion, "row2", out hv_R2);
                                HOperatorSet.RegionFeatures(NotSureRegion, "column2", out hv_C2);
                                HObject CropImgs;
                                HOperatorSet.CropRectangle1(RGBImg, out CropImgs, hv_R1, hv_C1, hv_R2, hv_C2);
                                HOperatorSet.WriteImage(CropImgs, "tiff", 0, NowFileNames);
                                CropImgs.Dispose();
                            }

                            #endregion

                            #region 儲存OK

                            if (OKObjCount > 0)
                            {
                                HTuple nowID;
                                HTuple NowFileNames = new HTuple();
                                for (int i = 0; i < OKObjCount; i++)
                                {
                                    nowID = i;
                                    string hv_nowFileName = ((OKFileName + "_") + (nowID.TupleString("04d")));
                                    HOperatorSet.TupleConcat(NowFileNames, hv_nowFileName, out NowFileNames);
                                }

                                HTuple hv_R1 = new HTuple(), hv_C1 = new HTuple(), hv_R2 = new HTuple(), hv_C2 = new HTuple();
                                HOperatorSet.RegionFeatures(OKRegion, "row1", out hv_R1);
                                HOperatorSet.RegionFeatures(OKRegion, "column1", out hv_C1);
                                HOperatorSet.RegionFeatures(OKRegion, "row2", out hv_R2);
                                HOperatorSet.RegionFeatures(OKRegion, "column2", out hv_C2);
                                HObject CropImgs;
                                HOperatorSet.CropRectangle1(RGBImg, out CropImgs, hv_R1, hv_C1, hv_R2, hv_C2);
                                HOperatorSet.WriteImage(CropImgs, "tiff", 0, NowFileNames);
                                CropImgs.Dispose();
                            }

                            //for (int i = 0; i < OKObjCount; i++)
                            //{
                            //    HObject ReduceImg, CropImg;
                            //    HObject SelectRegion;
                            //    HTuple nowID = i;
                            //    string hv_nowFileName = ((OKFileName + "_") + (nowID.TupleString("04d")));
                            //    HOperatorSet.SelectObj(OKRegion, out SelectRegion, nowID + 1);
                            //    HOperatorSet.ReduceDomain(RGBImg, SelectRegion, out ReduceImg);
                            //    HOperatorSet.CropDomain(ReduceImg, out CropImg);
                            //    HOperatorSet.WriteImage(CropImg, "tiff", 0, hv_nowFileName);
                            //    ReduceImg.Dispose();
                            //    CropImg.Dispose();
                            //    SelectRegion.Dispose();
                            //}

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
            Log.WriteLog("SaveImg end");
        }


        #endregion

        #region //=======================  函式設置 ======================
        private void WorkerFunc()
        {
            while (Exit == false)
            {
                SpinWait.SpinUntil(() => false, 5);

                clsPacket Packet;
                if (Buf.TryDequeue(out Packet) == false)
                    continue;

                SaveAOISegImg(Packet.SrcImg,
                              Packet.AbsoluteNGRegion,
                              Packet.OKRegion,
                              Packet.NotsureNGRegion,
                              Packet.Recipe,
                              Packet.Path,
                              Packet.ModuleName,
                              Packet.SB_ID,
                              Packet.MOVE_ID,
                              Packet.PartNumber);

                Packet.Dispose();
            }
        }
        #endregion

    }
}
