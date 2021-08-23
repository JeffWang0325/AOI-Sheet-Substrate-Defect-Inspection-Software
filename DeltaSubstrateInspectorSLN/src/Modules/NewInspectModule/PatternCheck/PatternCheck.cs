using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeltaSubstrateInspector.src.Modules.OperateMethods.InspectModule;
using DeltaSubstrateInspector.src.Roles;

using HalconDotNet;

using System.Diagnostics;
using System.IO;
using static DeltaSubstrateInspector.FileSystem.FileSystem;
using System.Windows.Forms;


using Prediction; // DAVS

namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.PatternCheck
{
    public class PatternCheck : OperateMethod
    {
        // 標準函示框架 **************************************************************************************************************
       
        #region 標準函示框架
        /// <summary>
        /// 與主軟體對接的視覺演算法
        /// </summary>
        /// <param name="role">演算法參數</param>
        /// <param name="src_imgs">每個取像位置的影像集合</param>
        /// <returns></returns>
        public override HObject get_defect_rgn(InspectRole role, List<HObject> src_imgs)
        {

            HObject DisplayRegion;

            // 設定參數
            set_parameter((PatternCheckRole)role);

            // 執行演算法
            execute(src_imgs, out DisplayRegion);
          
            // 回傳檢測結果
            return DisplayRegion;

        }

        /// <summary>
        /// 181203, andy: 演算法回傳每個cell的 region
        /// </summary>
        /// <param name="role"></param>
        /// <param name="src_imgs"></param>
        /// <returns></returns>        
        public override HObject get_cell_rgn(InspectRole role, List<HObject> src_imgs)
        {
            HObject CellRegion;
            CellRegion = null;
            HOperatorSet.GenEmptyObj(out CellRegion);

            // Get...
            // 設定輸出 Region           
            HOperatorSet.CopyObj(PaternMatchRegions, out CellRegion, 1, -1);
            HOperatorSet.ShapeTrans(CellRegion, out CellRegion, "inner_center"); // 中心Region

            // Return 
            return CellRegion;
        }

        HObject defectObj1_rgnCenter = null;
        // 181218, andy
        public override List<Defect> get_defect_result(InspectRole role, List<HObject> src_imgs)
        {
            List<Defect> defectResult = new List<Defect>();

            // 設定參數
            set_parameter((PatternCheckRole)role);

            // Defect1: DarkDefect
            string defectName1 = "MyDefect1";
            HObject defectObj1 = null;            
            defectObj1 = PaternMatchRegions;          
            HObject defectObj1_rgnCenter = null;           
            HOperatorSet.GenEmptyObj(out defectObj1_rgnCenter);
            HOperatorSet.ShapeTrans(defectObj1, out defectObj1_rgnCenter, "rectangle2");           
            Defect defect1 = new Defect(defectName1, src_imgs[0].Clone(), defectObj1, defectObj1_rgnCenter, defectObj1_rgnCenter);

            // Defect2: DarkDefect
            string defectName2 = "MyDefect2";
            HObject defectObj2 = null;
            HOperatorSet.GenCircle(out defectObj2, 20, 20, 50);
            HObject defectObj2_rgnCenter = null;
            HOperatorSet.ShapeTrans(defectObj2, out defectObj2_rgnCenter, "inner_center");
            Defect defect2 = new Defect(defectName2, src_imgs[1].Clone(), defectObj2, defectObj2_rgnCenter, defectObj2_rgnCenter);

            // Defect3: DarkDefect
            string defectName3 = "MyDefect3";
            HObject defectObj3 = null;
            HOperatorSet.GenCircle(out defectObj3, 200, 200, 50);
            HObject defectObj3_rgnCenter = null;
            HOperatorSet.ShapeTrans(defectObj3, out defectObj3_rgnCenter, "inner_center");
            HObject defectObj3_rgnDefectCenter = null;
            HOperatorSet.GenEmptyObj(out defectObj3_rgnDefectCenter);
            HOperatorSet.ConcatObj(defectObj2, defectObj3, out defectObj3_rgnDefectCenter);
            Defect defect3 = new Defect(defectName3, src_imgs[0].Clone(), defectObj3, defectObj3_rgnDefectCenter, defectObj3_rgnDefectCenter);

            // Defect2: DarkDefect
            string defectName4 = "MyDefect4";
            HObject defectObj4 = null;
            HOperatorSet.GenCircle(out defectObj4, 2000, 1000, 100);
            HObject defectObj4_rgnCenter = null;
            HOperatorSet.ShapeTrans(defectObj4, out defectObj4_rgnCenter, "inner_center");
            Defect defect4 = new Defect(defectName4, src_imgs[1].Clone(), defectObj4, defectObj4_rgnCenter, defectObj4_rgnCenter);

            // Add to list
            defectResult.Add(defect1);
            defectResult.Add(defect2);
            defectResult.Add(defect3);
            defectResult.Add(defect4);

            // return
            return defectResult;

        }
        


        /// <summary>
        /// 設定參數
        /// </summary>
        /// <param name="role"></param>
        public void set_parameter(PatternCheckRole role)
        {
            methRole = role;
        }
        
        /// <summary>
        /// 演算法實作
        /// </summary>
        /// <param name="ho_ImgList"></param>
        /// <param name="ho_Region_PatternCheck"></param>
        public void execute(List<HObject> ho_ImgList, out HObject ho_Region_PatternCheck)
        {
            
            swTotal_temp.Reset();
            swTotal_temp.Start();
            //==================================================================================

            ho_Region_PatternCheck = null;
            HOperatorSet.GenEmptyObj(out ho_Region_PatternCheck);
           

            #region 清空暫存
            HOperatorSet.GenEmptyObj(out PaternMatchRegions);
            HOperatorSet.GenEmptyObj(out PaternMatchSegRegions);
            //HOperatorSet.GenEmptyObj(out ModelID_contour);
            //HOperatorSet.GenEmptyObj(out dxfModelID_contour);
            HOperatorSet.GenEmptyObj(out SingleChipImgList);
            HOperatorSet.GenEmptyObj(out AIPredictionDefectRegions);
            HOperatorSet.GenEmptyObj(out AIPredictionOKRegions);
            #endregion

            if (methRole.InspectBypass==true)
            {
                swTotal_temp.Stop();
                calc_MS = swTotal_temp.ElapsedMilliseconds;
                return;
            }

            // ************ Fill real AI prediction function start
            HObject ho_Img = null;
            HOperatorSet.GenEmptyObj(out ho_Img);
            int inspectImgIndex = methRole.InspectImgIndex;
            ho_Img = ho_ImgList[inspectImgIndex].Clone();

            #region 完整計算

            // (1) 樣板比對
            if (!find_shape_model(ho_Img)) return;


            // (2) AOI, AI
            switch (methRole.DAVS_Mode)
            {
                case 0: // 不啟用 AI
                    {
                        // 設定輸出 Region                       
                        HOperatorSet.CopyObj(PaternMatchRegions, out ho_Region_PatternCheck, 1, -1);

                    }
                break;

                case 1: // 線上檢測
                    {
                                                
                        // 切割小圖與儲存
                        if (!segment_and_save_single_chip_images(ho_Img)) return;

                        // 預測
                        if (!PredictMode_prediction()) return;                                            

                        // 設定輸出Region
                        HOperatorSet.CopyObj(AIPredictionDefectRegions, out ho_Region_PatternCheck, 1, -1);

                    }
                    break;

                case 2: // 離線學習, 儲存影像
                    {
                        // 切割小圖
                        if (!segment_single_chip(ho_Img)) return;

                        // 儲存影像
                        if (methRole.DAVS_TModeSaveAll == true)
                        {
                            if (!TrainMode_save_single_chip_images()) return;
                        }

                        // 設定輸出Region
                        HOperatorSet.CopyObj(PaternMatchSegRegions, out ho_Region_PatternCheck, 1, -1);

                    }

                    break;
            }

            #endregion

            // ************ Fill real AI prediction function end 

            HOperatorSet.ShapeTrans(ho_Region_PatternCheck, out ho_Region_PatternCheck, "inner_center");


            //==================================================================================
            swTotal_temp.Stop();
            calc_MS = swTotal_temp.ElapsedMilliseconds;
            
        }

        /// <summary>
        /// UserControl測試演算法完整流程
        /// </summary>
        /// <param name="input_ImgList"></param>
        /// <param name="result_image"></param>
        public void action(List<HObject> input_ImgList, out HObject result_image)
        {
            result_image = null;

            // Local iconic variables 
            HObject ho_Region_PatternCheck;

            int inspectImgIndex = methRole.InspectImgIndex;
            result_image = input_ImgList[inspectImgIndex].Clone();

            execute(input_ImgList, out ho_Region_PatternCheck);

            // 將各種瑕疵區域畫在原始影像上
            HOperatorSet.OverpaintRegion(result_image, ho_Region_PatternCheck, ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "margin"); // green

            ho_Region_PatternCheck.Dispose();


        }

        /// <summary>
        /// 物件初始化
        /// </summary>
        public PatternCheck()
        {
            // 初始化
            HOperatorSet.GenEmptyObj(out PaternMatchRegions);
            HOperatorSet.GenEmptyObj(out PaternMatchSegRegions);
            HOperatorSet.GenEmptyObj(out ModelID_contour);
            HOperatorSet.GenEmptyObj(out dxfModelID_contour);
            HOperatorSet.GenEmptyObj(out SingleChipImgList);
            HOperatorSet.GenEmptyObj(out AIPredictionDefectRegions);
            HOperatorSet.GenEmptyObj(out AIPredictionOKRegions);
            
        }

        #endregion

        //  參數與公用變數 **************************************************************************************************************
  
        #region Role        
        public PatternCheckRole methRole = new PatternCheckRole();

        #endregion


        #region 計時器
        private Stopwatch sw_temp = new Stopwatch();
        private Stopwatch swTotal_temp = new Stopwatch();
        public double calc_MS;
        #endregion


        #region 樣板比對相關參數
        public HObject PaternMatchRegions = null;     // from In-Line PatternMatch result or Off-Line PatternMatch result         
        public HObject PaternMatchSegRegions = null;  // 切割用的Region (Rec2)
        private HTuple ModelID = null;
        public HObject ModelID_contour = null;
        public HObject dxfModelID_contour = null;
        private string ModelFileName = "SegSingleModel.shm";
        private string DxfModelFileName = "SegSingleContour.dxf"; // 增強型用
        private HObject SingleChipImgList = null; // 單一元件影像集
        #endregion


        #region DAVS相關參數
        private string ImgHSDirName = "ImgHS"; //"H:/ImgHS"// AOI與AI交握影像資料夾
        private TensorflowSharp_Prediction tfPrediction;
        private string deepLearningLayerName;
        private string deepLearningModelName;
        private string deepLearningParameterName;
        private int deepLearningClassNumber;
        private string[] deepLearningClassName;
        private string[] prediction_paths;
        private string AIParamPath = "AIParam";
        private bool davs_initOK = false;
        public List<bool> ai_resultList = new List<bool>();
        public HObject AIPredictionDefectRegions = null; // AI predition result NG regions 
        public HObject AIPredictionOKRegions = null;     // AI predition result OK regions
        private HTuple defectRegionIDList = new HTuple(); // 紀錄NG 的 index list
        private HTuple OKRegionIDList = new HTuple();     // 紀錄OK 的 index list
        
        #endregion


        #region 存檔相關參數
        private string RootPathName = "D:/DSI_PatternCheckImage";
        private string nowRecipeNameDir = "Default";
        private string nowSBIDDir = "00000000";
        private string nowMOVEID = "0";
        #endregion

        // 演算法  **************************************************************************************************************

        #region 樣板比對演算法

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool read_shape_model()
        {
            bool b_status = false;

            #region 讀取 Model file
            bool b_status_1 = false;
            string FixModelFileName = ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\PatternCheck\\" + ModelFileName;
            if (File.Exists(FixModelFileName))
            {
                HOperatorSet.ReadShapeModel(FixModelFileName, out ModelID);

                ModelID_contour.Dispose();
                HOperatorSet.GetShapeModelContours(out ModelID_contour, ModelID, 1);

                b_status_1 = true;
            }
            else
            {
                b_status_1 = false;
            }

            #endregion

            #region 讀取 DXF file

            bool b_status_2 = false;
            string FixDxfModelFileName = ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\PatternCheck\\" + DxfModelFileName;
            if (File.Exists(FixDxfModelFileName))
            {
                
                dxfModelID_contour.Dispose();
                HTuple hv_DxfStatus = null;
                HOperatorSet.ReadContourXldDxf(out dxfModelID_contour, FixDxfModelFileName, new HTuple(), new HTuple(), out hv_DxfStatus);

                b_status_2 = true;
            }
            else
            {
                b_status_2 = false;
            }

            #endregion

            b_status = b_status_1 & b_status_2;
            return b_status;
        }

        /// <summary>
        /// 樣板搜尋, 從樣板比對獲得切割要用的Regions (PaternMatchRegions)
        /// </summary>
        /// <param name="srcimage"></param>
        /// <returns></returns>
        public bool find_shape_model(HObject srcimage)
        {
            sw_temp.Reset();
            sw_temp.Start();
            //==================================================================================

            bool b_status = false;

            if (ModelID == null)
            {
                b_status = read_shape_model();
                if(b_status == false)
                    return b_status;
            }

            if(srcimage == null)
            {
                b_status = false;
                return b_status;
            }

            HTuple find_row, find_col, find_angle, find_score;
            HTuple hv_I = new HTuple(), hv_Number = new HTuple(), hv_Index = new HTuple();
            HTuple hv_HomMat2D = null;

            HObject ho_TransContours;
            HObject ho_dxfTransContours;
            HObject ho_Region;
            HObject ho_RegionUnion;
            HOperatorSet.GenEmptyObj(out ho_TransContours);
            HOperatorSet.GenEmptyObj(out ho_dxfTransContours);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);

            // ASA-07-133 專用
            /*
            HOperatorSet.FindShapeModel(srcimage, ModelID, 
                                        new HTuple(0).TupleRad(), new HTuple(360).TupleRad(), 
                                        match_score, 
                                        0, 
                                        0.5, 
                                        "least_squares",
                                        (new HTuple(5)).TupleConcat(1),
                                        0.9,
                                        out find_row, out find_col, out find_angle, out find_score);
            */

            // ASA-07-160 專用: A Side           
            /*
            HOperatorSet.FindShapeModel(srcimage, ModelID, 
                                        new HTuple(0).TupleRad(), new HTuple(360).TupleRad(),
                                        methRole.hv_MinScore, 
                                        0, 
                                        0.5, 
                                        "least_squares",
                                        (new HTuple(5)).TupleConcat(1),
                                        0.9,
                                        out find_row, out find_col, out find_angle, out find_score);
                                        */

            // ASA-07-160 專用: A/B Side 可            
            HOperatorSet.FindShapeModel(srcimage, ModelID,
                                        new HTuple(0).TupleRad(), new HTuple(5).TupleRad(),
                                        methRole.hv_MinScore,
                                        0,
                                        methRole.hv_MaxOverlap,
                                        "least_squares",
                                        (new HTuple(5)).TupleConcat(1),
                                        0.9,
                                        out find_row, out find_col, out find_angle, out find_score);
                                        


            HOperatorSet.GenEmptyObj(out PaternMatchRegions);
            for (hv_I = 0; (int)hv_I <= (int)((new HTuple(find_score.TupleLength())) - 1); hv_I = (int)hv_I + 1)
            {
                HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
                HOperatorSet.HomMat2dRotate(hv_HomMat2D, find_angle.TupleSelect(hv_I), 0, 0, out hv_HomMat2D);
                HOperatorSet.HomMat2dTranslate(hv_HomMat2D, find_row.TupleSelect(hv_I), find_col.TupleSelect(hv_I), out hv_HomMat2D);
              
                //// 轉成Region
                if ((int)(new HTuple(methRole.hv_EnhancedPatternMatch.TupleEqual(0))) != 0)
                {
                    ho_TransContours.Dispose();
                    HOperatorSet.AffineTransContourXld(ModelID_contour, out ho_TransContours, hv_HomMat2D);

                    ho_Region.Dispose();
                    HOperatorSet.GenRegionContourXld(ho_TransContours, out ho_Region, "filled");
                    ho_RegionUnion.Dispose();
                    HOperatorSet.Union1(ho_Region, out ho_RegionUnion);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(PaternMatchRegions, ho_RegionUnion, out ExpTmpOutVar_0);
                        PaternMatchRegions.Dispose();
                        PaternMatchRegions = ExpTmpOutVar_0;
                    }
                }
                else
                {
                    ho_dxfTransContours.Dispose();
                    HOperatorSet.AffineTransContourXld(dxfModelID_contour, out ho_dxfTransContours, hv_HomMat2D);
                    ho_Region.Dispose();
                    HOperatorSet.GenRegionContourXld(ho_dxfTransContours, out ho_Region, "filled");
                    ho_RegionUnion.Dispose();
                    HOperatorSet.Union1(ho_Region, out ho_RegionUnion);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(PaternMatchRegions, ho_RegionUnion, out ExpTmpOutVar_0);
                        PaternMatchRegions.Dispose();
                        PaternMatchRegions = ExpTmpOutVar_0;
                    }
                }

            }

            #region Halcon object delete

            ho_TransContours.Dispose();
            ho_Region.Dispose();
            ho_RegionUnion.Dispose();

            #endregion

            //==================================================================================
            sw_temp.Stop();
            calc_MS = sw_temp.ElapsedMilliseconds;

            b_status = true;
            return b_status;

        }

        public bool segment_and_save_single_chip_images(HObject srcimage)
        {
            sw_temp.Reset();
            sw_temp.Start();
            //==================================================================================

            bool b_status = false;

            if (srcimage == null)
                b_status = false;

            if (!segment_single_chip(srcimage))
                b_status = false;

            if (!save_single_chip_images())
                b_status = false;

            //==================================================================================
            sw_temp.Stop();
            calc_MS = sw_temp.ElapsedMilliseconds;

            b_status = true;
            return b_status;
        }

        /// <summary>
        /// 將原始影像切割為單一元件影像
        /// </summary>
        /// <returns></returns>
        private bool segment_single_chip(HObject srcimage)
        {
            bool b_status = false;

            if (PaternMatchRegions == null || srcimage == null)
                b_status = false;

            HTuple hv_I = new HTuple(), hv_Number = new HTuple(), hv_Index = new HTuple();
            HTuple hv_nowID = new HTuple();
            HObject ho_ObjectSelected = null, ho_RegionTrans = null;
            HObject ho_ImageReduced = null, ho_ImagePart = null;
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_RegionTrans);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_ImagePart);

            HOperatorSet.CountObj(PaternMatchRegions, out hv_Number);
            HTuple end_val87 = hv_Number;
            HTuple step_val87 = 1;

            // !!!!! 將Pattern matching 結果Region 轉換成Rec2 for 切割小圖 !!!!!
            PaternMatchSegRegions.Dispose();
            HOperatorSet.ShapeTrans(PaternMatchRegions, out PaternMatchSegRegions, "rectangle2");

            // 切割
            SingleChipImgList.Dispose();
            HOperatorSet.GenEmptyObj(out SingleChipImgList);
            for (hv_Index = 1; hv_Index.Continue(end_val87, step_val87); hv_Index = hv_Index.TupleAdd(step_val87))
            {

                hv_nowID = hv_Index.Clone();
                ho_ObjectSelected.Dispose();
                HOperatorSet.SelectObj(PaternMatchSegRegions, out ho_ObjectSelected, hv_nowID);

                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(srcimage, ho_ObjectSelected, out ho_ImageReduced);
                ho_ImagePart.Dispose();
                HOperatorSet.CropDomain(ho_ImageReduced, out ho_ImagePart);

                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(SingleChipImgList, ho_ImagePart, out ExpTmpOutVar_0);
                    SingleChipImgList.Dispose();
                    SingleChipImgList = ExpTmpOutVar_0;
                }

            }


            #region Halcon object delete

            ho_ObjectSelected.Dispose();
            ho_RegionTrans.Dispose();
            ho_ImageReduced.Dispose();
            ho_ImagePart.Dispose();

            #endregion


            b_status = true;
            return b_status;
        }

        /// <summary>
        /// 儲存單一元件影像列表 for AOI與AI 影像交換
        /// </summary>
        /// <returns></returns>
        private bool save_single_chip_images()
        {

            bool b_status = false;

            if (SingleChipImgList == null)
                b_status = false;

            HTuple hv_FileExists = new HTuple();

            HOperatorSet.FileExists(ImgHSDirName, out hv_FileExists);
            if ((int)(new HTuple(hv_FileExists.TupleNotEqual(1))) != 0)
            {
                // 若無此資料夾，則新增
                HOperatorSet.MakeDir(ImgHSDirName);
            }
            else
            {
                // 若有此資料夾，則刪除裡面的影像
                DeleteFileInDirectory(ImgHSDirName);
            }


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

                hv_nowFileName = ((ImgHSDirName + "/Img_") + (hv_nowID.TupleString("04d"))) + "." + extFN;
                //hv_nowFileName = (( "D:/Img_") + (hv_nowID.TupleString("04d"))) + ".tif";

                ho_ObjectSelected1.Dispose();
                HOperatorSet.SelectObj(SingleChipImgList, out ho_ObjectSelected1, hv_nowID);


                //HOperatorSet.WriteImage(ho_ObjectSelected1, "jpeg", 0, hv_nowFileName);
                HOperatorSet.WriteImage(ho_ObjectSelected1, extFN4Halcon, 0, hv_nowFileName); // !!!! 建議使用未壓縮過的tiff格式

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


        /// <summary>
        ///  儲存影像格式設定
        /// </summary>
        string extFN = "";
        string extFN4Halcon = "";
        private bool SetNowExtFileName()
        {           
            if (methRole.DAVS_ImageFmt == 0)
            {
                extFN = "jpg";
                extFN4Halcon = "jpeg";
            }
            else if (methRole.DAVS_ImageFmt == 1)
            {
                extFN = "tif";
                extFN4Halcon = "tiff";
            }

            return true;
        }

        #endregion


        #region DAVS 相關


        public bool DAVS_Initial()
        {
            bool status = false;

            try
            {

                string rootPath = ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\PatternCheck\\" + AIParamPath ;
                
                if (Directory.Exists(rootPath))
                {

                    deepLearningLayerName = string.Format("{0}\\{1}", rootPath, "\\layers_name.txt");
                    deepLearningModelName = string.Format("{0}\\{1}", rootPath, "\\navs.pb");
                    deepLearningParameterName = string.Format("{0}\\{1}", rootPath, "\\Parsms.ini");

                    // New
                    tfPrediction = new TensorflowSharp_Prediction();
                    tfPrediction.LoadLayerName(deepLearningLayerName);
                    tfPrediction.LoadModel(deepLearningModelName);
                    tfPrediction.LoadParameter(deepLearningParameterName);

                    // Set
                    deepLearningClassNumber = tfPrediction.ClassNumber;
                    deepLearningClassName = tfPrediction.ClassName;

                    // Set path: 因為AI prediction function 要使用絕對路徑
                    //ImgHSDirName4AI = Application.StartupPath + "\\" +ImgHSDirName;
                   
                    davs_initOK = true;
                    status = true;

                }
                else
                {
                    davs_initOK = false;
                    status = false;
                }

            }
            catch (System.Exception)
            {
                davs_initOK = false;
                status = false;               
            }

            
            return status;

        }


        public bool DAVS_execute()
        {

            sw_temp.Reset();
            sw_temp.Start();
            //==================================================================================


            bool b_status = false;

            // (2) AOI, AI
            switch (methRole.DAVS_Mode)
            {
                case 0: // 不啟用AI
                    {
                        // 設定輸出Region
                        //HOperatorSet.CopyObj(PaternMatchRegions, out ho_Region_PatternCheck, 1, -1);

                    }
                    break;

                case 1: // 線上檢測
                    {
                        //if (!segment_and_save_single_chip_images(ho_Img)) return;

                        //if (!ai_prediction()) return;

                        if(!PredictMode_prediction())
                        {
                            sw_temp.Stop();
                            calc_MS = sw_temp.ElapsedMilliseconds;

                            b_status = false;
                            return b_status;
                        }

                    }
                    break;

                case 2: // 離線學習, 儲存影像
                    {
                        // 切割小圖
                        if (SingleChipImgList==null) return false;

                        // 儲存影像
                        if (methRole.DAVS_TModeSaveAll == true)
                        {
                            if (!TrainMode_save_single_chip_images())
                            {
                                sw_temp.Stop();
                                calc_MS = sw_temp.ElapsedMilliseconds;

                                b_status = false;
                                return b_status;
                            }
                        }

                        // 設定輸出Region
                        //HOperatorSet.CopyObj(PaternMatchSegRegions, out ho_Region_PatternCheck, 1, -1);

                    }

                    break;
            }


            //==================================================================================
            sw_temp.Stop();
            calc_MS = sw_temp.ElapsedMilliseconds;


            b_status = true;
            return b_status;

        }


        private bool TrainMode_save_single_chip_images()
        {
            bool b_status = false;

            if (SingleChipImgList == null)
                b_status = false;

            #region Create DIR

            HTuple hv_FileExists = new HTuple();
            HOperatorSet.FileExists(RootPathName, out hv_FileExists);
            if ((int)(new HTuple(hv_FileExists.TupleNotEqual(1))) != 0)
            {
                // 若無此資料夾，則新增
                HOperatorSet.MakeDir(RootPathName);
            }

            // Get RecipeName !!!!!!!!!!
            if (ModuleName != "") nowRecipeNameDir = ModuleName;
            string tempFileName = RootPathName + "/" + nowRecipeNameDir;
            HOperatorSet.FileExists(tempFileName, out hv_FileExists);
            if ((int)(new HTuple(hv_FileExists.TupleNotEqual(1))) != 0)
            {
                // 若無此資料夾，則新增
                HOperatorSet.MakeDir(tempFileName);
            }

            // Get ID !!!!!!!!!!
            if (SB_ID != "") nowSBIDDir = SB_ID;
            tempFileName = tempFileName + "/" + nowSBIDDir;
            HOperatorSet.FileExists(tempFileName, out hv_FileExists);
            if ((int)(new HTuple(hv_FileExists.TupleNotEqual(1))) != 0)
            {
                // 若無此資料夾，則新增
                HOperatorSet.MakeDir(tempFileName);
            }

            // Train mode資料夾
            tempFileName = tempFileName + "/" + "T-Mode";
            HOperatorSet.FileExists(tempFileName, out hv_FileExists);
            if ((int)(new HTuple(hv_FileExists.TupleNotEqual(1))) != 0)
            {
                // 若無此資料夾，則新增
                HOperatorSet.MakeDir(tempFileName);
            }

            #endregion

            // Get base name
            if (MOVE_ID != "") nowMOVEID = MOVE_ID;        
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
                    // Write
                    HOperatorSet.WriteImage(ho_ObjectSelected1, extFN4Halcon, 0, hv_nowFileName); // !!!! 建議使用未壓縮過的tiff格式

                }
                catch (Exception e)
                {
                    string fff = e.ToString();
                    return false;
                }
                

            }


            #region Halcon object delete

            ho_ObjectSelected1.Dispose();

            #endregion



            b_status = true;
            return b_status;
        }


        private bool PredictMode_prediction()
        {
            bool b_status = false;

            #region 清除暫存

            ai_resultList.Clear();

            AIPredictionDefectRegions.Dispose();
            HOperatorSet.GenEmptyObj(out AIPredictionDefectRegions);

            AIPredictionOKRegions.Dispose();
            HOperatorSet.GenEmptyObj(out AIPredictionOKRegions);

            defectRegionIDList = new HTuple();
            OKRegionIDList = new HTuple();

            #endregion

            // 初始化 DAVS
            if (davs_initOK == false)
            {
                b_status = DAVS_Initial();
                if (b_status == false)
                    return b_status;
            }


            // 獲得檔案路徑
            SetNowExtFileName();
            if (!Directory.Exists(ImgHSDirName))
            {
                b_status = false;
                return b_status;
            }
            else
            {
                string FilePath = ImgHSDirName;
                prediction_paths = Directory.GetFiles(FilePath, "*." + extFN);
            }

            // Check 是否有待檢測圖檔
            if(prediction_paths.Length==0)
            {
                b_status = false;
                return b_status;
            }


            // AI prediction!!!!!! --> 輸出資料夾需要有指定的DLL檔, 快速貼上, XCOPY     
            if (methRole.DAVS_PredictMode == 0)
            {
                b_status = tfPrediction.Prediction(prediction_paths);
            }
            else if (methRole.DAVS_PredictMode == 1)
            {
                b_status = tfPrediction.PredictionRgb(prediction_paths);
            }
            if (b_status == false) return b_status;


            string ff = tfPrediction.stRunTime;

            // Update ai result list
            int predictCnt = prediction_paths.Length;
            for (int i = 0; i < predictCnt; i++)
            {                             
                bool nowResult = (tfPrediction.fResult[i, 0] > tfPrediction.fResult[i, 1]) ? true : false;     // 0:NG, 1:OK              
                ai_resultList.Add(nowResult);
            }

            /*
            ai_resultList[0] = false;
            ai_resultList[2] = false;
            ai_resultList[4] = false;
            ai_resultList[6] = false;
            */

            // Update ai prediction regions
            ai_GetDefectRegions();

            // Save ng ok images
            PredictMode_save_ngok_single_chip_image();


            b_status = true;
            return b_status;

        }


        private bool ai_GetDefectRegions()
        {

            for (int i = 0; i < ai_resultList.Count; i++)
            {
                if (ai_resultList[i] == true) // NG
                {
                    HOperatorSet.TupleConcat(defectRegionIDList, i + 1, out defectRegionIDList);
                }
                else // OK
                {
                    HOperatorSet.TupleConcat(OKRegionIDList, i + 1, out OKRegionIDList);
                }
            }

            HOperatorSet.SelectObj(PaternMatchSegRegions, out AIPredictionDefectRegions, defectRegionIDList);
            HOperatorSet.SelectObj(PaternMatchSegRegions, out AIPredictionOKRegions, OKRegionIDList);

            #region 選擇輸出樣式, 中心點、圓形

            int opt = 1;
            switch (opt)
            {
                case 0: // 顯示結果轉成中心點 (單點)
                    HOperatorSet.ShapeTrans(AIPredictionDefectRegions, out AIPredictionDefectRegions, "inner_center");
                    HOperatorSet.ShapeTrans(AIPredictionOKRegions, out AIPredictionOKRegions, "inner_center");
                    break;

                case 1: // 顯示結果轉成圓形
                    
                    int radius = 10;
                    HTuple area_, row_, col_, hv_Number;
                    HTuple hv_Newtuple = null;
                    HOperatorSet.AreaCenter(AIPredictionDefectRegions, out area_, out row_, out col_);
                    HOperatorSet.CountObj(AIPredictionDefectRegions, out hv_Number);
                    if (hv_Number > 0)
                    {
                        HOperatorSet.TupleGenConst(hv_Number, radius, out hv_Newtuple);
                        HOperatorSet.GenCircle(out AIPredictionDefectRegions, row_, col_, hv_Newtuple);
                    }

                    HTuple area2_, row2_, col2_, hv2_Number;
                    HTuple hv2_Newtuple = null;
                    HOperatorSet.AreaCenter(AIPredictionOKRegions, out area2_, out row2_, out col2_);
                    HOperatorSet.CountObj(AIPredictionOKRegions, out hv2_Number);
                    if (hv2_Number > 0)
                    {
                        HOperatorSet.TupleGenConst(hv2_Number, radius, out hv2_Newtuple);
                        HOperatorSet.GenCircle(out AIPredictionOKRegions, row2_, col2_, hv2_Newtuple);
                    }
                    break;

            }

            #endregion

            return true;
        }


        private bool PredictMode_save_ngok_single_chip_image()
        {
            bool b_status = false;

            if (SingleChipImgList == null)
                b_status = false;
          
            #region Create DIR

            HTuple hv_FileExists = new HTuple();
            HOperatorSet.FileExists(RootPathName, out hv_FileExists);
            if ((int)(new HTuple(hv_FileExists.TupleNotEqual(1))) != 0)
            {
                // 若無此資料夾，則新增
                HOperatorSet.MakeDir(RootPathName);
            }

            // Get RecipeName !!!!!!!!!!
            if (ModuleName != "") nowRecipeNameDir = ModuleName;
            string tempFileName = RootPathName + "/" + nowRecipeNameDir;
            HOperatorSet.FileExists(tempFileName, out hv_FileExists);
            if ((int)(new HTuple(hv_FileExists.TupleNotEqual(1))) != 0)
            {
                // 若無此資料夾，則新增
                HOperatorSet.MakeDir(tempFileName);
            }

            // Get ID !!!!!!!!!!
            if (SB_ID != "") nowSBIDDir = SB_ID;
            tempFileName = tempFileName + "/" + nowSBIDDir;
            HOperatorSet.FileExists(tempFileName, out hv_FileExists);
            if ((int)(new HTuple(hv_FileExists.TupleNotEqual(1))) != 0)
            {
                // 若無此資料夾，則新增
                HOperatorSet.MakeDir(tempFileName);
            }

            // Predict mode資料夾
            tempFileName = tempFileName + "/" + "P-Mode";
            HOperatorSet.FileExists(tempFileName, out hv_FileExists);
            if ((int)(new HTuple(hv_FileExists.TupleNotEqual(1))) != 0)
            {
                // 若無此資料夾，則新增
                HOperatorSet.MakeDir(tempFileName);
            }

            // NG、OK 資料夾
            string tempFileNameNG = tempFileName + "/" + "NG";
            HOperatorSet.FileExists(tempFileNameNG, out hv_FileExists);
            if ((int)(new HTuple(hv_FileExists.TupleNotEqual(1))) != 0)
            {
                // 若無此資料夾，則新增
                HOperatorSet.MakeDir(tempFileNameNG);
            }

            string tempFileNameOK = tempFileName + "/" + "OK";
            HOperatorSet.FileExists(tempFileNameOK, out hv_FileExists);
            if ((int)(new HTuple(hv_FileExists.TupleNotEqual(1))) != 0)
            {
                // 若無此資料夾，則新增
                HOperatorSet.MakeDir(tempFileNameOK);
            }

            #endregion

            // Get base name
            if (MOVE_ID != "") nowMOVEID = MOVE_ID;
            string baseFileNameNG = tempFileNameNG + "/" + nowSBIDDir + "_" + String.Format("{0:00000}", Convert.ToInt32(nowMOVEID));
            string baseFileNameOK = tempFileNameOK + "/" + nowSBIDDir + "_" + String.Format("{0:00000}", Convert.ToInt32(nowMOVEID));


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
               
                // Select obj
                ho_ObjectSelected1.Dispose();
                HOperatorSet.SelectObj(SingleChipImgList, out ho_ObjectSelected1, hv_nowID);

                string baseFileName = "";
                int now_ai_resultListID = hv_nowID - 1;
                if (ai_resultList[now_ai_resultListID] == true) // NG
                {
                    baseFileName = baseFileNameNG;
                    
                }
                else // OK
                {
                    baseFileName = baseFileNameOK;

                }

                // Set filename
                hv_nowFileName = ((baseFileName + "_") + (hv_nowID.TupleString("04d"))) + "." + extFN;

                // Write
                if ((ai_resultList[now_ai_resultListID] == true && methRole.DAVS_PModeSaveNG == true) || (ai_resultList[now_ai_resultListID] == false && methRole.DAVS_PModeSaveOK == true))
                {
                    HOperatorSet.WriteImage(ho_ObjectSelected1, extFN4Halcon, 0, hv_nowFileName); // !!!! 建議使用未壓縮過的tiff格式
                }

               
            }

            b_status = true;
            return b_status;

        }

        #endregion


    }


}
