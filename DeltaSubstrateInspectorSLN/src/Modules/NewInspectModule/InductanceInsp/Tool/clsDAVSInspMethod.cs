using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Prediction;
using HalconDotNet;
using System.Diagnostics;
using System.Threading;

namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.InductanceInsp
{
    [Serializable]
    public class clsDAVSInspMethod
    {
        public class clsResultRegion
        {
            public string Name = "";
            public HObject ClassResultRegion;
            public HTuple IndexList = new HTuple();

            public clsResultRegion(string pmName,HObject pmClassResultRegion)
            {
                this.Name = pmName;
                this.ClassResultRegion = pmClassResultRegion.Clone();
            }

            public void Dispose()
            {
                if (ClassResultRegion!=null)
                {
                    ClassResultRegion.Dispose();
                    ClassResultRegion = null;
                }
            }
            public clsResultRegion() { }
        }
        public class clsAIClass
        {
            public int ClassNUM = 0;

            public List<clsResultRegion> ResultRegionsList = new List<clsResultRegion>();

            public clsAIClass() { }
        }
        
        #region DAVS相關參數
        private clsRecipe.clsDAVSInsp Param;
        private TensorflowSharp_Prediction tfPrediction;
        private string deepLearningLayerName;
        private string deepLearningModelName;
        private string deepLearningParameterName;
        private int deepLearningClassNumber;
        private string[] deepLearningClassName;
        private string[] prediction_paths;
        private bool davs_initOK = false;
        public List<int> ResultList = new List<int>();
        public List<bool> ai_resultList = new List<bool>();
        public HObject AIPredictionDefectRegions = null; // AI predition result NG regions 
        public HObject AIPredictionOKRegions = null;     // AI predition result OK regions
        private HTuple defectRegionIDList = new HTuple(); // 紀錄NG 的 index list
        private HTuple OKRegionIDList = new HTuple();     // 紀錄OK 的 index list
        public clsAIClass AIClass = new clsAIClass();
        public HObject InspRegions = null;  // 切割用的Region
        public HObject SingleChipImgList = null; // 單一元件影像集
        public bool bOutboundary = false;
        #endregion
        
        #region 存檔影像相關參數
        string rootPath = "";
        public string RecipePath = "";
        private string RootPathName = "D://DSI//Image//DAVS//";
        public string ModuleName = ""; // 此為Recipe name
        public string LotID = "Default1234";
        public string SB_ID = "00000000"; // 181114, andy, Substrate ID
        public string MOVE_ID = ""; // 181114, andy 位移ID
        string extFN = "";
        string extFN4Halcon = "";
        #endregion


        public clsDAVSInspMethod(clsRecipe.clsDAVSInsp pmParam,string pmRecipePath,bool pmbOutboundary)
        {
            this.RecipePath = pmRecipePath;
            this.Param = pmParam;
            this.bOutboundary = pmbOutboundary;
            InitAIParam();
            if (Param.DAVS_SaveImgEnabledList.Count != AIClass.ClassNUM)
            {
                Param.DAVS_SaveImgEnabledList.Clear();
                for (int i = 0; i < AIClass.ClassNUM; i++)
                {
                    Param.DAVS_SaveImgEnabledList.Add(true);
                }
            }
        }
        
        private bool InitAIParam()
        {
            rootPath = Param.ModelPath;
            if (Directory.Exists(rootPath))
            {
                deepLearningLayerName = string.Format("{0}{1}", rootPath, "layers_name.txt");
                deepLearningModelName = string.Format("{0}{1}", rootPath, "navs.pb");
                deepLearningParameterName = string.Format("{0}{1}", rootPath, "Parsms.ini");

                AIClass.ClassNUM = int.Parse(clsStaticTool.ReadAIClass(deepLearningParameterName, "ReadParam", "Class_NUM"));
                
                for (int i = 0; i < AIClass.ClassNUM; i++)
                {
                    string Name = "Class_" + (i + 1).ToString();
                    HObject Obj;
                    HOperatorSet.GenEmptyRegion(out Obj);
                    clsResultRegion Tmp = new clsResultRegion(clsStaticTool.ReadAIClass(deepLearningParameterName, "ReadParam", Name), Obj);
                    AIClass.ResultRegionsList.Add(Tmp);
                }
                if (!DAVS_Initial())
                    return false;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetParam(clsRecipe.clsDAVSInsp pmParam)
        {
            this.Param = pmParam;
        }

        public void SetPathParam(string pmModuleName, string pmSB_ID, string pmMOVE_ID,string pmLotID)
        {
            this.ModuleName = pmModuleName;
            this.SB_ID = pmSB_ID;
            this.MOVE_ID = pmMOVE_ID;
            this.LotID = pmLotID;
        }

        public void SetModuleNamme(string pmModuleName)
        {
            this.ModuleName = pmModuleName;
        }

        public void SetSB_ID(string pmSB_ID)
        {
            this.SB_ID = pmSB_ID;
        }

        public void SetMOVE_ID(string pmMOVE_ID)
        {
            this.MOVE_ID = pmMOVE_ID;
        }

        public void SetLotID(string pmLotID)
        {
            this.LotID = pmLotID;
        }

        /// <summary>
        /// 輸入需要切割位置之Regions
        /// </summary>
        /// <param name="pmPaternMatchSegRegions"></param>
        public void InsertPaternMatchSegRegions(HObject pmInspRegions)
        {
            this.InspRegions = pmInspRegions;
        }
        
        public List<clsResultRegion> GetPredictionRegionList()
        {
            return AIClass.ResultRegionsList;
        }

        /// <summary>
        ///  儲存影像格式設定
        /// </summary>
        private bool SetNowExtFileName()
        {
            extFN = "tif";
            extFN4Halcon = "tiff";

            return true;
        }
        
        public void Dispose()
        {

        }

        public bool DAVS_Initial()
        {
            bool status = false;

            try
            {
                rootPath = Param.ModelPath;
                if (Directory.Exists(rootPath))
                {
                    // New
                    tfPrediction = new TensorflowSharp_Prediction();
                    tfPrediction.LoadLayerName(deepLearningLayerName);
                    tfPrediction.LoadModel(deepLearningModelName);
                    tfPrediction.LoadParameter(deepLearningParameterName);

                    // Set
                    deepLearningClassNumber = tfPrediction.ClassNumber;
                    deepLearningClassName = tfPrediction.ClassName;

                    davs_initOK = true;
                    status = true;

                }
                else
                {
                    davs_initOK = false;
                    status = false;
                }

            }
            catch (System.Exception ex)
            {
                davs_initOK = false;
                status = false;
            }
            return status;

        }

        public HObject GetPredictionOKRegion()
        {
            return AIPredictionOKRegions;
        }

        public HObject GetPredictionNGRegion()
        {
            return AIPredictionDefectRegions;
        }

        public void ClearRes()
        {
            for (int i = 0; i < AIClass.ClassNUM; i++)
            {
                AIClass.ResultRegionsList[i].IndexList = new HTuple();
                if (AIClass.ResultRegionsList[i].ClassResultRegion != null)
                {
                    AIClass.ResultRegionsList[i].ClassResultRegion.Dispose();
                    AIClass.ResultRegionsList[i].ClassResultRegion = null;
                }
                HOperatorSet.GenEmptyObj(out AIClass.ResultRegionsList[i].ClassResultRegion);
            }
        }
        
        public bool DAVS_execute(clsRecipe.clsDAVSInsp DavsParam,HObject SrcImage)
        {
            bool b_status = false;
            Param = DavsParam;
            // (2) AOI, AI
            switch (Param.DAVS_InspMode)
            {
                case enuDAVSMode.Online: // 線上檢測
                    {

                        if (!segment_and_save_single_chip_images(SrcImage, true))
                        {
                            return b_status;
                        }
                        if (!PredictMode_prediction())
                        {
                            b_status = false;
                            return b_status;
                        }

                    }
                    break;

                case enuDAVSMode.Offline: // 離線學習, 儲存影像
                    {
                        ClearRes();
                        if (!segment_and_save_single_chip_images(SrcImage, false))
                        {
                            return b_status;
                        }
                        // 切割小圖
                        if (SingleChipImgList == null)
                        {
                            return false;
                        }
                        // 儲存影像
                        {
                            RootPathName = Param.SaveImagePath + DateTime.Now.ToString("yyyyMMdd") + "//";
                            TrainMode_save_single_chip_images();
                        }
 
                    }

                    break;
            }

            b_status = true;
            return b_status;

        }

        private bool TrainMode_save_single_chip_images()
        {
            bool b_status = false;
            string nowRecipeNameDir = "Default", nowSBIDDir = "00000000", nowMOVEID = "0", nowLotID = "Default1234";
            if (SingleChipImgList == null)
            {
                b_status = false;
            }
            #region Create DIR
            if (!string.IsNullOrEmpty(ModuleName)) nowRecipeNameDir = ModuleName;
            if (!string.IsNullOrEmpty(SB_ID)) nowSBIDDir = SB_ID;
            if (!string.IsNullOrEmpty(MOVE_ID)) nowMOVEID = MOVE_ID;
            if (!string.IsNullOrEmpty(LotID)) nowLotID = LotID;
            string tempFileName = RootPathName + "//" + nowLotID + "//" + nowRecipeNameDir + "//" + nowSBIDDir + "//" + "T-Mode//";
            if (!Directory.Exists(tempFileName))
            {
                Directory.CreateDirectory(tempFileName);
            }

            HTuple hv_FileExists = new HTuple();

            #endregion

            // Get base name

            string baseFileName = tempFileName + "/" + nowSBIDDir + "_" + String.Format("{0:00000}", Convert.ToInt32(nowMOVEID));


            // Write image for loop
            HTuple hv_Number1 = new HTuple();
            HTuple hv_Index1 = new HTuple();
            HTuple hv_nowID = new HTuple();
            HTuple hv_nowFileName = new HTuple();
            HObject ho_ObjectSelected1 = null;
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected1);
            HOperatorSet.CountObj(SingleChipImgList, out hv_Number1);
            HTuple end_val111 = hv_Number1;
            HTuple step_val111 = 1;

            SetNowExtFileName();

            for (hv_Index1 = 1; hv_Index1.Continue(end_val111, step_val111); hv_Index1 = hv_Index1.TupleAdd(step_val111))
            {
                hv_nowID = hv_Index1.Clone();

                // Set filename
                hv_nowFileName = ((baseFileName + "_") + (hv_nowID.TupleString("04d"))) + "." + extFN;

                // Select obj
                ho_ObjectSelected1.Dispose();
                HOperatorSet.SelectObj(SingleChipImgList, out ho_ObjectSelected1, hv_nowID);

                try
                {
                    HOperatorSet.WriteImage(ho_ObjectSelected1, extFN4Halcon, 0, hv_nowFileName);
                }
                catch (Exception e)
                {
                    return false;
                }


            }

            #region Halcon object delete

            ho_ObjectSelected1.Dispose();

            #endregion

            b_status = true;
            return b_status;
        }

        private bool DeleteFileInDirectory(string target_dir)
        {
            bool result = false;
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);
            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            result = true;
            return result;
        }

        private bool save_single_chip_images()
        {

            bool b_status = false;

            if (SingleChipImgList == null)
            {
                b_status = false;
                return b_status;
            }

            HTuple hv_FileExists = new HTuple();

            HOperatorSet.FileExists(Param.DAVS_ImgHSDir, out hv_FileExists);
            if ((int)(new HTuple(hv_FileExists.TupleNotEqual(1))) != 0)
            {
                // 若無此資料夾，則新增
                HOperatorSet.MakeDir(Param.DAVS_ImgHSDir);
            }
            else
            {
                // 若有此資料夾，則刪除裡面的影像
                DeleteFileInDirectory(Param.DAVS_ImgHSDir);
            }

            HTuple hv_Number1 = new HTuple();
            HTuple hv_Index1 = new HTuple();
            HTuple hv_nowID = new HTuple();
            
            HOperatorSet.CountObj(SingleChipImgList, out hv_Number1);
            HTuple end_val111 = hv_Number1;
            HTuple step_val111 = 1;

            SetNowExtFileName();

            Parallel.For(1, hv_Number1.I + 1, i =>
              {
                  HTuple hv_nowFileName = ((Param.DAVS_ImgHSDir + "/Img_") + (i.ToString("0000"))) + "." + extFN;
                  HObject ho_ObjectSelected1 = null;
                  HOperatorSet.GenEmptyObj(out ho_ObjectSelected1);

                  HOperatorSet.SelectObj(SingleChipImgList, out ho_ObjectSelected1, i);
                  HOperatorSet.WriteImage(ho_ObjectSelected1, extFN4Halcon, 0, hv_nowFileName); // !!!! 建議使用未壓縮過的tiff格式
                  ho_ObjectSelected1.Dispose();
              });

            
            b_status = true;
            return b_status;
        }

        public bool segment_and_save_single_chip_images(HObject srcimage,bool Online)
        {

            bool b_status = false;

            if (srcimage == null)
            {
                b_status = false;
                return b_status;
            }

            if (!segment_single_chip(srcimage))
            {
                b_status = false;
                return b_status;
            }

            if (Online)
            {
                if (!save_single_chip_images())
                {
                    b_status = false;
                    return b_status;
                }
            }
            b_status = true;
            return b_status;
        }

        private bool PredictMode_prediction()
        {
            bool b_status = false;
            #region 清除暫存 OK & Defect RegionsList
            if (ai_resultList != null)
            {
                ai_resultList.Clear();
                ai_resultList = null;
                ai_resultList = new List<bool>();
            }
            if (ResultList == null)
                ResultList = new List<int>();
            else
            {
                ResultList.Clear();
                ResultList = null;
                ResultList = new List<int>();
            }

            if (AIPredictionDefectRegions != null)
            {
                AIPredictionDefectRegions.Dispose();
                AIPredictionDefectRegions = null;
            }
            HOperatorSet.GenEmptyObj(out AIPredictionDefectRegions);
            if (AIPredictionOKRegions != null)
            {
                AIPredictionOKRegions.Dispose();
                AIPredictionOKRegions = null;
            }
            HOperatorSet.GenEmptyObj(out AIPredictionOKRegions);

            for (int i = 0; i < AIClass.ClassNUM; i++)
            {
                AIClass.ResultRegionsList[i].IndexList = new HTuple();
                if (AIClass.ResultRegionsList[i].ClassResultRegion != null)
                {
                    AIClass.ResultRegionsList[i].ClassResultRegion.Dispose();
                    AIClass.ResultRegionsList[i].ClassResultRegion = null;
                }
                HOperatorSet.GenEmptyObj(out AIClass.ResultRegionsList[i].ClassResultRegion);
            }

            defectRegionIDList = new HTuple();
            OKRegionIDList = new HTuple();
            #endregion

            // 初始化 DAVS
            if (davs_initOK == false)
            {
                b_status = DAVS_Initial();
                if (b_status == false)
                {
                    return b_status;
                }
            }

            // 獲得檔案路徑
            SetNowExtFileName();
            if (!Directory.Exists(Param.DAVS_ImgHSDir))
            {
                b_status = false;
                return b_status;
            }
            else
            {
                string FilePath = Param.DAVS_ImgHSDir;
                prediction_paths = Directory.GetFiles(FilePath, "*." + extFN);
                Array.Sort(prediction_paths);
            }

            // Check 是否有待檢測圖檔
            if (prediction_paths.Length == 0)
            {
                b_status = true;
                return b_status;
            }

            // Update ai result list
            int predictCnt = prediction_paths.Length;

            if (predictCnt == 0)
            {
                b_status = true;
                return b_status;
            }

            // AI prediction!!!!!!
            if (Param.DAVS_PredictMode == enuPredictionMode.BGR)
            {
                b_status = tfPrediction.Prediction(prediction_paths);
            }
            else if (Param.DAVS_PredictMode == enuPredictionMode.RGB)
            {
                b_status = tfPrediction.PredictionRgb(prediction_paths);
            }
            if (b_status == false) return b_status;

            Classify(prediction_paths.Length, tfPrediction);
            
            // Update ai prediction regions
            GetResRegions();

            // Save ng ok images
            if (CheckSaveImg(Param.DAVS_SaveImgEnabledList))
            {
                RootPathName = Param.SaveImagePath + DateTime.Now.ToString("yyyyMMdd") + "//";
                PredictMode_save_Res_single_chip_image();
            }

            b_status = true;
            return b_status;

        }

        private bool PredictMode_save_Res_single_chip_image()
        {
            bool b_status = false;
            string nowRecipeNameDir = "Default", nowSBIDDir = "00000000", nowMOVEID = "0", nowLotID = "Default1234";

            if (SingleChipImgList == null)
                b_status = false;

            #region Create DIR
            if (!string.IsNullOrEmpty(ModuleName)) nowRecipeNameDir = ModuleName;
            if (!string.IsNullOrEmpty(SB_ID)) nowSBIDDir = SB_ID;
            if (!string.IsNullOrEmpty(MOVE_ID)) nowMOVEID = MOVE_ID;
            if (!string.IsNullOrEmpty(LotID)) nowLotID = LotID;
            string tempFileName = RootPathName + "//" + nowLotID + "//" + nowRecipeNameDir + "//" + nowSBIDDir + "//" + "P-Mode//";
            if (!Directory.Exists(tempFileName))
            {
                Directory.CreateDirectory(tempFileName);
            }

            HTuple hv_FileExists = new HTuple();

            for (int i = 0; i < AIClass.ClassNUM; i++)
            {
                string tempClsFileName = tempFileName + "/" + AIClass.ResultRegionsList[i].Name;
                HOperatorSet.FileExists(tempClsFileName, out hv_FileExists);
                if ((int)(new HTuple(hv_FileExists.TupleNotEqual(1))) != 0 && Param.DAVS_SaveImgEnabledList[i] == true)
                {
                    // 若無此資料夾，則新增
                    HOperatorSet.MakeDir(tempClsFileName);
                }
            }
            #endregion

            // Write image for loop
            HTuple hv_Number1 = new HTuple();
            HTuple hv_Index1 = new HTuple();
            HTuple hv_nowID = new HTuple();
            HTuple hv_nowFileName = new HTuple();
            HObject ho_ObjectSelected1 = null;
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected1);
            HOperatorSet.CountObj(SingleChipImgList, out hv_Number1);
            HTuple end_val111 = hv_Number1;
            HTuple step_val111 = 1;

            SetNowExtFileName();

            for (hv_Index1 = 1; hv_Index1.Continue(end_val111, step_val111); hv_Index1 = hv_Index1.TupleAdd(step_val111))
            {
                hv_nowID = hv_Index1.Clone();
                string baseFileName = "";
                int now_ai_resultListID = hv_nowID - 1;

                if (Param.DAVS_SaveImgEnabledList[ResultList[now_ai_resultListID]] == false)
                    continue;

                // Select obj
                ho_ObjectSelected1.Dispose();
                HOperatorSet.SelectObj(SingleChipImgList, out ho_ObjectSelected1, hv_nowID);


                string tempClsFileName = tempFileName + "/" + AIClass.ResultRegionsList[ResultList[now_ai_resultListID]].Name;


                baseFileName = tempClsFileName + "/" + nowSBIDDir + "_" + String.Format("{0:00000}", Convert.ToInt32(nowMOVEID));


                // Set filename
                hv_nowFileName = ((baseFileName + "_") + (hv_nowID.TupleString("04d"))) + "." + extFN;

                // Write
                if (Param.DAVS_SaveImgEnabledList[ResultList[now_ai_resultListID]] == true)
                {
                    HOperatorSet.WriteImage(ho_ObjectSelected1, extFN4Halcon, 0, hv_nowFileName); // !!!! 建議使用未壓縮過的tiff格式
                }
            }

            b_status = true;
            return b_status;

        }

        private bool CheckSaveImg(List<bool> EnableList)
        {
            bool Res = false;
            for (int i = 0; i < EnableList.Count; i++)
            {
                if (EnableList[i])
                {
                    Res = true;
                    return Res;
                }
            }
            return Res;
        }

        private bool GetResRegions()
        {
            for (int i = 0; i < ResultList.Count; i++)
            {
                for (int j = 0; j < AIClass.ClassNUM; j++)
                {
                    if (ResultList[i] == j)
                    {
                        HOperatorSet.TupleConcat(AIClass.ResultRegionsList[j].IndexList, i + 1, out AIClass.ResultRegionsList[j].IndexList);
                    }
                }
            }

            for (int i = 0; i < AIClass.ClassNUM; i++)
            {
                HOperatorSet.SelectObj(InspRegions, out AIClass.ResultRegionsList[i].ClassResultRegion, AIClass.ResultRegionsList[i].IndexList);
            }

            #region 選擇輸出樣式, 中心點、圓形

            int opt = 1;
            switch (opt)
            {
                case 0: // 顯示結果轉成中心點 (單點)
                    for (int i = 0; i < AIClass.ClassNUM; i++)
                    {
                        HOperatorSet.ShapeTrans(AIClass.ResultRegionsList[i].ClassResultRegion, out AIClass.ResultRegionsList[i].ClassResultRegion, "inner_center");
                    }
                    break;

                case 1: // 顯示結果轉成圓形
                    for (int i = 0; i < AIClass.ClassNUM; i++)
                    {
                        if (AIClass.ResultRegionsList[i].IndexList.Length == 0)
                            continue;
                        int radius = 10;
                        HTuple area_, row_, col_, hv_Number;
                        HTuple hv_Newtuple = null;
                        HOperatorSet.AreaCenter(AIClass.ResultRegionsList[i].ClassResultRegion, out area_, out row_, out col_);
                        HOperatorSet.CountObj(AIClass.ResultRegionsList[i].ClassResultRegion, out hv_Number);
                        if (hv_Number > 0)
                        {
                            HOperatorSet.TupleGenConst(hv_Number, radius, out hv_Newtuple);
                            HOperatorSet.GenCircle(out AIClass.ResultRegionsList[i].ClassResultRegion, row_, col_, hv_Newtuple);
                        }
                    }
                    break;

            }

            #endregion

            return true;
        }

        private void Classify(int PredictCnt, TensorflowSharp_Prediction tfPrediction)
        {
            for (int i = 0; i < PredictCnt; i++)
            {
                float Max = float.MinValue;
                int Index = 0;
                for (int j = 0; j < AIClass.ClassNUM; j++)
                {
                    if (tfPrediction.fResult[i, j] > Max)
                    {
                        Max = tfPrediction.fResult[i, j];
                        Index = j;
                    }
                }
                ResultList.Add(Index);
            }
        }

        private bool segment_single_chip(HObject srcimage)
        {
            bool b_status = false;

            if (InspRegions == null || srcimage == null)
            {
                b_status = false;
                return b_status;
            }
            // 切割
            if (SingleChipImgList != null)
            {
                SingleChipImgList.Dispose();
                SingleChipImgList = null;
            }
            HOperatorSet.GenEmptyObj(out SingleChipImgList);

            HTuple SrcWidth, SrcHeight;
            HOperatorSet.GetImageSize(srcimage, out SrcWidth, out SrcHeight);

            HObject Union;
            HOperatorSet.Union1(InspRegions, out Union);

            HObject ReduceImg;
            HOperatorSet.ReduceDomain(srcimage, Union, out ReduceImg);

            try
            {
                HObject Trans;
                if (!Param.bFixSize)
                {
                    HObject Rect1Region;
                    HOperatorSet.ShapeTrans(InspRegions, out Rect1Region, enuTransType.rectangle1.ToString());
                    HTuple A, R, C;
                    HOperatorSet.AreaCenter(Rect1Region, out A, out R, out C);
                    HTuple MW, MH;
                    HOperatorSet.RegionFeatures(Rect1Region, "width", out MW);
                    HOperatorSet.RegionFeatures(Rect1Region, "height", out MH);

                    HTuple W, H;
                    HOperatorSet.TupleMax(MW, out W);
                    HOperatorSet.TupleMax(MH, out H);
                    HOperatorSet.GenRectangle1(out Trans, R - H / 2, C - W / 2, R + H / 2, C + W / 2);
                }
                else
                {
                    HTuple A, R, C;
                    HOperatorSet.AreaCenter(InspRegions, out A, out R, out C);
                    HOperatorSet.GenRectangle1(out Trans, R - Param.FixSizeHeight / 2, C - Param.FixSizeWidth / 2, R + Param.FixSizeHeight / 2, C + Param.FixSizeWidth / 2);
                }

                HObject Dilation, Erosion;
                HOperatorSet.DilationRectangle1(Trans, out Dilation, Param.EdgeExtSizeWidth, Param.EdgeExtSizeHeight);
                HOperatorSet.ErosionRectangle1(Dilation, out Erosion, Param.EdgeSkipSizeWidth, Param.EdgeSkipSizeHeight);
                
                //if (!Param.bOutboundary)
                //{
                //    HTuple Area, Row, Column;
                //    HOperatorSet.AreaCenter(Erosion, out Area, out Row, out Column);
                //    HTuple Width, Height;
                //    HOperatorSet.RegionFeatures(Erosion, "width", out Width);
                //    HOperatorSet.RegionFeatures(Erosion, "height", out Height);

                //    HOperatorSet.CropRectangle1(ReduceImg, out SingleChipImgList, Row - Height / 2, Column - Width / 2, Row + Height / 2, Column + Width / 2);
                //}
                //else
                {
                    HTuple InspRegionCount;
                    HOperatorSet.CountObj(Erosion, out InspRegionCount);

                    HTuple InspRectWidth, InspRectHeight;

                    if (!Param.bFixSize)
                    {
                        HTuple W, H;
                        HOperatorSet.RegionFeatures(Erosion, "width", out W);
                        HOperatorSet.RegionFeatures(Erosion, "height", out H);

                        HOperatorSet.TupleSelect(W, 0, out InspRectWidth);
                        HOperatorSet.TupleSelect(H, 0, out InspRectHeight);
                    }
                    else
                    {
                        InspRectWidth = Param.FixSizeWidth;
                        InspRectHeight = Param.FixSizeHeight;
                    }

                    for (int i = 0; i < InspRegionCount; i++)
                    {
                        HObject CropImg;
                        HObject SelectRegion;
                        HTuple nowID = i;
                        HOperatorSet.SelectObj(Erosion, out SelectRegion, nowID + 1);
                        HObject ReduceImage;
                        HOperatorSet.ReduceDomain(srcimage, SelectRegion, out ReduceImage);

                        HOperatorSet.CropDomain(ReduceImage, out CropImg);
                        HTuple W, H;
                        HOperatorSet.GetImageSize(CropImg, out W, out H);
                        if (W.D != InspRectWidth.D || H.D != InspRectHeight.D)
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

                        HOperatorSet.ConcatObj(SingleChipImgList, CropImg, out SingleChipImgList);
                        ReduceImg.Dispose();
                        SelectRegion.Dispose();
                    }
                }
                Trans.Dispose();
                Dilation.Dispose();
                Erosion.Dispose();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                return b_status;
            }
            ReduceImg.Dispose();
            Union.Dispose();
            b_status = true;
            return b_status;
        }

        public void UpdatePredictionResult(HSmartWindowControl Display,HObject OrgImg)
        {
            switch (Param.DAVS_InspMode)
            {
                case enuDAVSMode.Online: // 線上檢測, 顯示AI: NG結果: Red , OK結果:Green
                    {
                        HOperatorSet.SetDraw(Display.HalconWindow, "margin");

                        for (int i = 0; i < AIClass.ResultRegionsList.Count; i++)
                        {
                            if (AIClass.ResultRegionsList[i].Name == "OK")
                            {
                                HOperatorSet.SetColor(Display.HalconWindow, "green");
                            }
                            else
                            {
                                HOperatorSet.SetColor(Display.HalconWindow, "red");
                            }
                            HOperatorSet.DispObj(AIClass.ResultRegionsList[i].ClassResultRegion, Display.HalconWindow);
                        }
                    }
                    break;

                case enuDAVSMode.Offline: // 離線學習, 顯示切割結果
                    {
                        HOperatorSet.SetDraw(Display.HalconWindow, "margin");
                        HOperatorSet.SetColor(Display.HalconWindow, "blue");
                        HOperatorSet.DispObj(OrgImg, Display.HalconWindow);
                        HOperatorSet.DispObj(InspRegions, Display.HalconWindow);
                    }
                    break;

            }
        }

    }



}
