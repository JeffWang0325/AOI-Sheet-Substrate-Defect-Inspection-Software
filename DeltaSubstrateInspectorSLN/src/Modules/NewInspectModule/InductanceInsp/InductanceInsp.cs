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
using DAVS;
using System.Drawing;
using System.Collections.Concurrent;
using System.Threading;
using Prediction;
using System.Windows.Forms;
using DeltaSubstrateInspector.FileSystem;

namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.InductanceInsp
{
    public class InductanceInsp : OperateMethod
    {
        /// <summary>
        /// AOI演算法類型
        /// </summary>
        public enum enuAlgorithm
        {
            ThresholdInsp,
            LaserHoleInsp,
            TextureInsp,
            DAVSInsp,
            LineInsp,
            FeatureInsp
        }

        public enum enuThresholdType
        {
            GeneralThreshold,
            AutoThreshold,
            DynThreshold,
            BinaryThreshold
        }

        public enum enuDynThresholdType
        {
            dark,
            light,
            equal,
            not_equal
        }
        
        static InductanceInsp m_Singleton;
        public InductanceInspRole methRole = InductanceInspRole.GetSingleton();
        public HObject PatternRegion { get; set; } = null;
        List<Defect> defectResult = new List<Defect>();
        clsLog Log = new clsLog();
        object LockObj = new object();

        public InductanceInsp()
        {
            
        }

        public static InductanceInsp GetSingleton(bool b_InductanceInspUC_ = false) // (20191214) Jeff Revised!
        {
            if (m_Singleton == null)
            {
                m_Singleton = new InductanceInsp();
                m_Singleton.b_InductanceInspUC = b_InductanceInspUC_;
            }
            return m_Singleton;
        }

        /// <summary>
        /// 是否是 InductanceInspUC 在執行
        /// </summary>
        private bool b_InductanceInspUC { get; set; } = false; // (20191214) Jeff Revised!

        HObject DefectRegions;
        /// <summary>
        /// 與主軟體對接的視覺演算法
        /// </summary>
        /// <param name="role">演算法參數</param>
        /// <param name="src_imgs">每個取像位置的影像集合</param>
        /// <returns></returns>
        public override HObject get_defect_rgn(InspectRole role, List<HObject> src_imgs)
        {
            //List<clsDAVS.clsResultRegion> ResRegion;
            // 設定參數
            set_parameter((InductanceInspRole)role);


            HOperatorSet.GenEmptyRegion(out DefectRegions);

            if (methRole.DAVS == null)
            {
                methRole.DAVS = new clsDAVS(methRole.DAVSParam, methRole.GetRecipePath());
            }
            string SBID = "";
            if (B_SB_InerCountID)
                SBID = SB_InerCountID;
            else
                SBID = SB_ID;
            methRole.DAVS.SetPathParam(ModuleName, SBID, MOVE_ID, PartNumber);
            clsStaticTool.DisposeAll(defectResult);
            List<int> List;
            // 執行演算法
            if (!executeNew(methRole, src_imgs,false, out DefectRegions,out defectResult,out List))
            {
                Log.WriteLog("Execute Fail");
                return DefectRegions;
            }

            //HOperatorSet.WriteImage(src_imgs[0], "tiff", 0, "src_imgs0"); // For debug! (20191216) MIL Jeff Revised!
            //HOperatorSet.WriteRegion(DefectRegions, "DefectRegions"); // For debug! (20191216) MIL Jeff Revised!

            // 回傳檢測結果
            return DefectRegions;

        }

        public override Dictionary<string, DefectsResult> Initialize_DefectsClassify(InspectRole role) // (20200409) Jeff Revised!
        {
            Dictionary<string, DefectsResult> defectsClassify = new Dictionary<string, DefectsResult>();

            if (FinalInspectParam.b_NG_classification) // 多瑕疵
            {
                try
                {
                    for (int i = 0; i < methRole.Param.PriorityLayerCount + 1; i++)
                    {
                        List<clsRecipe.clsAlgorithm> TempList = methRole.Param.AlgorithmList.FindAll(x => x.Priority == i);
                        clsRecipe.clsAlgorithm Tmp = new clsRecipe.clsAlgorithm();

                        if (TempList.Count > 0)
                        {
                            //defectsClassify.Add(TempList[0].Name, new DefectsResult(TempList[0].Name, TempList[0].Priority, TempList[0].Color));
                            // (20190727) Jeff Revised!
                            defectsClassify.Add(TempList[0].DefectName, DefectsResult.Get_DefectsResult(TempList[0].DefectName, TempList[0].Priority, TempList[0].Color));
                        }
                    }

                    // AI啟用，則要加入AI瑕疵類型
                    if (methRole.DAVSParam.DAVS_Mode == 1)// (20200409) Jeff Revised!
                        defectsClassify.Add("AI_NG", DefectsResult.Get_DefectsResult("AI_NG", methRole.Param.PriorityLayerCount + 1, "#ff00ffff"));
                }
                catch
                {
                    MessageBox.Show("新增瑕疵類型失敗，確請認: \n 1. 瑕疵名稱是否已存在 \n 2. 瑕疵名稱不能為【OK】及【Cell瑕疵】 \n 3. 優先權是否已存在", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            return defectsClassify;
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
            if (PatternRegion != null)
            {
                HOperatorSet.CopyObj(PatternRegion, out CellRegion, 1, -1);
                HOperatorSet.ShapeTrans(CellRegion, out CellRegion, "inner_center"); // 中心Region
            }
            // Return 
            return CellRegion;
        }

        public override List<Defect> get_defect_result(InspectRole role, List<HObject> src_imgs)
        {
            return defectResult;
        }

        /// <summary>
        /// Regions 轉成 圓
        /// </summary>
        /// <param name="Regions"></param>
        /// <param name="CirclrRegions"></param>
        /// <param name="radius">半徑</param>
        public void Convert2Circle(HObject Regions, out HObject CirclrRegions, int radius = 8) // (20200319) Jeff Revised!
        {
            HOperatorSet.GenEmptyObj(out CirclrRegions);
            HTuple area_, row_, col_, hv_Number;
            HTuple hv_Newtuple = null;
            
            HOperatorSet.AreaCenter(Regions, out area_, out row_, out col_);
            HOperatorSet.CountObj(Regions, out hv_Number);
            if (hv_Number > 0)
            {
                HOperatorSet.TupleGenConst(hv_Number, radius, out hv_Newtuple);
                HOperatorSet.GenCircle(out CirclrRegions, row_, col_, hv_Newtuple);
            }
        }

        public bool RisitanseInsp(List<HObject> Input_ImgList, InductanceInspRole Recipe, HObject CellRegion, HObject ThinRegion, HObject ScratchRegion, HObject StainRegion,
                                  out HObject ThinDefectRegions,out HObject HoleDefectRegion, out HObject ScratchDefectRegions, out HObject StainDefectRegions, out HObject RDefectRegions,
                                  out HObject ThinMap,out HObject HoleMap, out HObject ScratchMap, out HObject StainMap, out HObject RDefectMap,out HObject RShiftDefectMap)
        {
            bool Res = false;
            HOperatorSet.GenEmptyObj(out ThinDefectRegions);
            HOperatorSet.GenEmptyObj(out ScratchDefectRegions);
            HOperatorSet.GenEmptyObj(out StainDefectRegions);
            HOperatorSet.GenEmptyObj(out RDefectRegions);
            HOperatorSet.GenEmptyObj(out ThinMap);
            HOperatorSet.GenEmptyObj(out ScratchMap);
            HOperatorSet.GenEmptyObj(out StainMap);
            HOperatorSet.GenEmptyObj(out RDefectMap);
            HOperatorSet.GenEmptyObj(out RShiftDefectMap);
            HOperatorSet.GenEmptyObj(out HoleDefectRegion);
            HOperatorSet.GenEmptyObj(out HoleMap);
            try
            {
                HObject Thin_Dark, Thin_Bright;
                HOperatorSet.GenEmptyObj(out Thin_Dark);
                HOperatorSet.GenEmptyObj(out Thin_Bright);

                HObject RDefect_Dark, RDefect_Bright;
                HOperatorSet.GenEmptyObj(out RDefect_Dark);
                HOperatorSet.GenEmptyObj(out RDefect_Bright);

                HObject Stain_Dark, Stain_Bright;
                HOperatorSet.GenEmptyObj(out Stain_Dark);
                HOperatorSet.GenEmptyObj(out Stain_Bright);

                HObject RShift_Dark, RShift_Bright;
                HOperatorSet.GenEmptyObj(out RShift_Dark);
                HOperatorSet.GenEmptyObj(out RShift_Bright);

                PartitionRegions(Input_ImgList, Recipe, ThinRegion, ScratchRegion, StainRegion, out Thin_Dark, out Thin_Bright, out RDefect_Dark, out RDefect_Bright, out Stain_Dark, out Stain_Bright, out RShift_Dark, out RShift_Bright);
                
                #region 薄化

                ThinParam ThinParam = new ThinParam(Input_ImgList[Recipe.Param.RisistAOIParam.ThinParam.InspID], Recipe, CellRegion, Thin_Dark, Thin_Bright);
                var ThinTask = new Task<clsThinInspResult>(ThinInspTask, ThinParam);
                ThinTask.Start();

                #endregion
                
                #region 刮傷
                
                ScratchParam ScratchParam = new ScratchParam(Input_ImgList[Recipe.Param.RisistAOIParam.ScratchParam.InspID], Recipe, CellRegion, ScratchRegion, ThinRegion);
                var ScratchTask = new Task<HObject>(ScratchInspTask, ScratchParam);
                ScratchTask.Start();

                #endregion

                #region 髒污
                
                StainParam StainParam = new StainParam(Input_ImgList[Recipe.Param.RisistAOIParam.StainParam.InspID], Recipe, CellRegion, Stain_Dark, Stain_Bright);
                var StainTask = new Task<HObject>(StainInspTask, StainParam);
                StainTask.Start();

                #endregion

                #region R-異常
                
                RDefectParam RDefectParam = new RDefectParam(Input_ImgList[Recipe.Param.RisistAOIParam.RDefectParam.InspID], Recipe, CellRegion, RDefect_Dark, RDefect_Bright);
                var RDefectTask = new Task<HObject>(RDefectInspTask, RDefectParam);
                RDefectTask.Start();

                #endregion

                #region R-偏移
                
                RShiftParam RShiftParam_Bright = new RShiftParam(Input_ImgList[Recipe.Param.RisistAOIParam.RDefectParam.InspID], RShift_Bright, Recipe);
                var RShiftTask_Bright = new Task<HObject>(RshiftNewInspTask, RShiftParam_Bright);
                RShiftTask_Bright.Start();


                RShiftParam RShiftParam_Dark = new RShiftParam(Input_ImgList[Recipe.Param.RisistAOIParam.RDefectParam.InspID], RShift_Dark, Recipe);
                var RShiftTask_Dark = new Task<HObject>(RshiftNewInspTask, RShiftParam_Dark);
                RShiftTask_Dark.Start();
                HObject Bright_Defect, Dark_Defect;
                HOperatorSet.GenEmptyObj(out Bright_Defect);
                HOperatorSet.GenEmptyObj(out Dark_Defect);
                
                #endregion


                ScratchDefectRegions = ScratchTask.Result;
                StainDefectRegions = StainTask.Result;
                RDefectRegions = RDefectTask.Result;
                ThinDefectRegions = ThinTask.Result.ThinRes;
                HoleDefectRegion = ThinTask.Result.HoleRes;
                Bright_Defect = RShiftTask_Bright.Result;
                Dark_Defect = RShiftTask_Dark.Result;

                HOperatorSet.ConcatObj(Dark_Defect, Bright_Defect, out RShiftDefectMap);

                HOperatorSet.ShapeTrans(RShiftDefectMap, out RShiftDefectMap, "inner_center");
                LocalDefectMappping(RShiftDefectMap, CellRegion, out RShiftDefectMap);
                LocalDefectMappping(ScratchDefectRegions, CellRegion, out ScratchMap);
                LocalDefectMappping(StainDefectRegions, CellRegion, out StainMap);
                LocalDefectMappping(RDefectRegions, CellRegion, out RDefectMap);
                LocalDefectMappping(ThinDefectRegions, CellRegion, out ThinMap);
                LocalDefectMappping(HoleDefectRegion, CellRegion, out HoleMap);

                #region Dispose

                ScratchTask.Dispose();
                RDefectTask.Dispose();
                StainTask.Dispose();
                ThinTask.Dispose();
                RShiftTask_Dark.Dispose();
                RShiftTask_Bright.Dispose();
                #endregion

            }
            catch(Exception Ex)
            {
                Log.WriteLog(Ex.ToString());
                return Res;
            }
            Res = true;
            return Res;
        }

        public void Convert2MapRegions(HObject CRegions, InductanceInspRole Recipe, out HObject MapRegions)
        {
            HTuple Area, CenterY, CenterX;
            HOperatorSet.GenEmptyObj(out MapRegions);
            HOperatorSet.AreaCenter(CRegions, out Area, out CenterY, out CenterX);
            if (Area.Length <= 0)
                return;
            HObject RectRegion, TmpRegion;
            HOperatorSet.GenEmptyObj(out RectRegion);
            HOperatorSet.GenEmptyObj(out TmpRegion);
            {
                TmpRegion.Dispose();
                RectRegion.Dispose();
                HOperatorSet.GenRectangle1(out RectRegion,
                                           CenterY - Recipe.Param.SegParam.InspRectHeight / 2,
                                           CenterX - Recipe.Param.SegParam.InspRectWidth / 2,
                                            CenterY + Recipe.Param.SegParam.InspRectHeight / 2,
                                           CenterX + Recipe.Param.SegParam.InspRectWidth / 2);

                HOperatorSet.ConcatObj(RectRegion, MapRegions, out TmpRegion);
                MapRegions.Dispose();
                MapRegions = TmpRegion;
            }
        }

        public void RotateRegion(HTuple Angle, HObject Region, HTuple CenterX, HTuple CenterY, out HObject RotateRegions)
        {
            HOperatorSet.GenEmptyObj(out RotateRegions);

            HTuple HomMat2DIdentity, HomMat2DRotate;
            HOperatorSet.HomMat2dIdentity(out HomMat2DIdentity);
            HOperatorSet.HomMat2dRotate(HomMat2DIdentity, Angle, CenterY, CenterX, out HomMat2DRotate);

            HOperatorSet.AffineTransRegion(Region, out RotateRegions, HomMat2DRotate, "nearest_neighbor");
        }
        private string sb_InerCountID = "";
        /// <summary>
        /// 演算法實作
        /// </summary>
        /// <param name="ho_ImgList"></param>
        /// <param name="ho_Region_PatternCheck"></param>
        public bool execute(InductanceInspRole Recipe, List<HObject> ho_ImgList, out HObject MergeDefectRegions, out List<Defect> DefectResultList)
        {
            bool Res = false;

            Log.SetLogEnabled(Recipe.Param.LogEnabled);
            if (sb_InerCountID != SB_InerCountID)
            {
                sb_InerCountID = SB_InerCountID;
                Log.WriteLog("※基板序號: " + SB_InerCountID);
            }
            Log.WriteLog("檢測位置: " + MOVE_ID);

            Log.WriteLog("Insp Proc Execute Start");

            #region 變數宣告
            MergeDefectRegions = null;
            List<clsDAVS.clsResultRegion> ResList = new List<clsDAVS.clsResultRegion>();
            List<HObject> InspImageList = new List<HObject>();
            HTuple SB_Angle_Rad;
            Recipe.Init(Recipe.Param.SaveAOIImgEnabled);
            HObject TempRegions;
            HOperatorSet.GenEmptyObj(out TempRegions);
            HOperatorSet.GenEmptyObj(out MergeDefectRegions);
            HObject TempReg;
            HObject AOI_DefectRegions;
            HObject AOI_OKRegions;
            HOperatorSet.GenEmptyObj(out TempReg);
            HOperatorSet.GenEmptyObj(out AOI_DefectRegions);
            HOperatorSet.GenEmptyObj(out AOI_OKRegions);
            DefectResultList = null;
            DefectResultList = new List<Defect>();
            HObject ThinRegion, StainRegion, ScratchRegion;
            HObject PatternRegions = null;
            HObject SegRegions;
            HOperatorSet.GenEmptyObj(out SegRegions);
            HOperatorSet.GenEmptyObj(out PatternRegions);
            HOperatorSet.GenEmptyObj(out ThinRegion);
            HOperatorSet.GenEmptyObj(out StainRegion);
            HOperatorSet.GenEmptyObj(out ScratchRegion);
            HTuple Angle;
            HObject Cell_Center;
            HObject InnerDefectRegions, OuterDefectRegions;
            HObject InnerMapRegions, OuterMapDefect;
            HObject InnerDefectCenter, OuterDefectCenter;
            HObject DefectMap;
            HObject DefectC, SegRegionsC, AOI_NGRegionsC, AOI_OKRegionsC;
            #endregion
            try
            {
                #region 測試模式

                HOperatorSet.TupleRad(Affine_angle_degree, out SB_Angle_Rad);
                Log.WriteLog("RotateImage Start");
                if (Recipe.Param.RotateImageEnabled)
                {
                    GetRotateImageList(-SB_Angle_Rad, Recipe, ho_ImgList, out InspImageList);
                }
                else
                {
                    InspImageList = ho_ImgList.ToList();
                }
                Log.WriteLog("RotateImage End");

                if (Recipe.Param.TestModeEnabled && Recipe.Param.TestModeType == 0)
                {
                    Log.WriteLog("Insp Proc Execute ByPass");
                    HOperatorSet.GenEmptyObj(out MergeDefectRegions);
                    Res = true;
                    return Res;
                }
                #endregion

                #region Pattern Match & Get SegCellRegions

                Log.WriteLog("PtternMatch function & Seg Start");

                if (!PtternMatchAndSegRegion(InspImageList[Recipe.Param.SegParam.SegImgIndex], Recipe, out PatternRegions, out Angle, out SegRegions, out ThinRegion, out StainRegion, out ScratchRegion))
                {
                    Log.WriteLog("Pattern Match & ImageSegmentation Fail");
                    return Res;
                }

                if (Recipe.Param.TestModeEnabled && Recipe.Param.TestModeType == 1)
                {
                    Log.WriteLog("Insp Proc Execute TestModeType 1 ");
                    Convert2Circle(PatternRegions, out MergeDefectRegions);
                    Res = true;
                    return Res;
                }
                Log.WriteLog("PtternMatch function & Seg End");

                if (Recipe.Param.TestModeEnabled && Recipe.Param.TestModeType == 2)
                {
                    Log.WriteLog("Insp Proc Execute TestModeType 2 ");
                    MergeDefectRegions = SegRegions.Clone();
                    Res = true;
                    return Res;
                }
                HOperatorSet.ShapeTrans(SegRegions, out Cell_Center, "inner_center");

                #endregion

                #region 執行AOI

                Log.WriteLog("InnerInsp_Action function Start");
                //InnerInsp_Action(InspImageList[Recipe.Param.InnerInspParam.ImageIndex], InspImageList, PatternRegions, SegRegions, Recipe, out InnerDefectRegions, out InnerMapRegions);
                InnerInspParam InnerParam = new InnerInspParam(InspImageList[Recipe.Param.InnerInspParam.ImageIndex], InspImageList, PatternRegions, SegRegions, Recipe);
                var InnerTask = new Task<HObject>(InnerInspTask, InnerParam);
                InnerTask.Start();
                Log.WriteLog("InnerInsp_Action function End");


                Log.WriteLog("OuterInsp_Action function Start");
                //OuterInsp_Action(InspImageList[Recipe.Param.OuterInspParam.ImageIndex], InspImageList, PatternRegions, SegRegions, Recipe, out OuterDefectRegions, out OuterMapDefect);

                OuterInspParam OuterParam = new OuterInspParam(InspImageList[Recipe.Param.OuterInspParam.ImageIndex], InspImageList, PatternRegions, SegRegions, Recipe);
                var OuterTask = new Task<HObject>(OuterInspTask, OuterParam);
                OuterTask.Start();

                Log.WriteLog("OuterInsp_Action function End");



                #region 電阻檢測
                Log.WriteLog("Risist Insp Start");
                HObject Stain, Scratch, Thin, RDefect, RShift, Hole;

                HOperatorSet.GenEmptyObj(out Stain);
                HOperatorSet.GenEmptyObj(out Scratch);
                HOperatorSet.GenEmptyObj(out Thin);
                HOperatorSet.GenEmptyObj(out RDefect);
                HOperatorSet.GenEmptyObj(out RShift);
                HOperatorSet.GenEmptyObj(out Hole);

                HObject StainMap, ScratchMap, ThinMap, RDefectMap, HoleMap;
                HOperatorSet.GenEmptyObj(out StainMap);
                HOperatorSet.GenEmptyObj(out ScratchMap);
                HOperatorSet.GenEmptyObj(out ThinMap);
                HOperatorSet.GenEmptyObj(out RDefectMap);
                HOperatorSet.GenEmptyObj(out HoleMap);

                if (Recipe.Param.RisistAOIParam.ThinParam.Enabled || Recipe.Param.RisistAOIParam.ScratchParam.Enabled || Recipe.Param.RisistAOIParam.StainParam.Enabled || Recipe.Param.RisistAOIParam.RDefectParam.Enabled || Recipe.Param.RisistAOIParam.RShiftNewParam.Enabled)
                {
                    RisitanseInsp(InspImageList, Recipe, SegRegions, ThinRegion, ScratchRegion, StainRegion, out Thin, out Hole, out Scratch, out Stain, out RDefect, out ThinMap, out HoleMap, out ScratchMap, out StainMap, out RDefectMap, out RShift);
                }

                Log.WriteLog("Risist Insp End");

                #endregion

                #region 取得AOI結果

                InnerDefectRegions = InnerTask.Result;
                LocalDefectMappping(InnerDefectRegions, SegRegions, out InnerMapRegions);

                OuterDefectRegions = OuterTask.Result;
                LocalDefectMappping(OuterDefectRegions, SegRegions, out OuterMapDefect);

                InnerTask.Dispose();
                OuterTask.Dispose();


                HObject RisistDefect;
                HOperatorSet.GenEmptyObj(out RisistDefect);
                HObject RShiftCenter;
                Convert2Circle(RShift, out RShiftCenter);
                HOperatorSet.ConcatObj(ThinMap, RisistDefect, out RisistDefect);
                HOperatorSet.ConcatObj(RisistDefect, ScratchMap, out RisistDefect);
                HOperatorSet.ConcatObj(RisistDefect, StainMap, out RisistDefect);
                HOperatorSet.ConcatObj(RisistDefect, RDefectMap, out RisistDefect);
                HOperatorSet.ConcatObj(RisistDefect, RShiftCenter, out RisistDefect);
                HOperatorSet.ConcatObj(RisistDefect, HoleMap, out RisistDefect);

                HObject ThinDefectCenter, ScratchDefectCenter, StainDefectCenter, RDefectCenter, HoleDefectCenter;
                HOperatorSet.ShapeTrans(InnerMapRegions, out InnerDefectCenter, "inner_center");
                HOperatorSet.ShapeTrans(OuterMapDefect, out OuterDefectCenter, "inner_center");
                HOperatorSet.ShapeTrans(ThinMap, out ThinDefectCenter, "inner_center");
                HOperatorSet.ShapeTrans(ScratchMap, out ScratchDefectCenter, "inner_center");
                HOperatorSet.ShapeTrans(StainMap, out StainDefectCenter, "inner_center");
                HOperatorSet.ShapeTrans(RDefectMap, out RDefectCenter, "inner_center");
                HOperatorSet.ShapeTrans(HoleMap, out HoleDefectCenter, "inner_center");

                #endregion


                #region 加入AOI檢測結果

                if (Recipe.Param.InnerInspParam.Enabled)
                {
                    Defect InnerRes = new Defect(Recipe.Param.InnerInspParam.InspName, InspImageList[methRole.Param.InnerInspParam.ImageIndex], InnerDefectRegions, InnerDefectCenter, Cell_Center, true, "#ff0000ff");
                    DefectResultList.Add(InnerRes);
                }
                if (Recipe.Param.OuterInspParam.Enabled)
                {
                    Defect OuterRes = new Defect(Recipe.Param.OuterInspParam.InspName, InspImageList[methRole.Param.OuterInspParam.ImageIndex], OuterDefectRegions, OuterDefectCenter, Cell_Center, true, "#ff0000ff");
                    DefectResultList.Add(OuterRes);
                }
                if (Recipe.Param.RisistAOIParam.ThinParam.Enabled)
                {
                    Defect ThinRes = new Defect(Recipe.Param.RisistAOIParam.ThinParam.Name, InspImageList[Recipe.Param.RisistAOIParam.ThinParam.InspID], Thin, ThinDefectCenter, Cell_Center, true, "#ff0000ff");
                    DefectResultList.Add(ThinRes);
                }
                if (Recipe.Param.RisistAOIParam.ThinParam.TopHatEnabled)
                {
                    Defect HoleRes = new Defect(Recipe.Param.RisistAOIParam.ThinParam.HoleName, InspImageList[Recipe.Param.RisistAOIParam.ThinParam.InspID], Hole, HoleDefectCenter, Cell_Center, true, "#ff0000ff");
                    DefectResultList.Add(HoleRes);
                }
                if (Recipe.Param.RisistAOIParam.ScratchParam.Enabled)
                {
                    Defect ScratchRes = new Defect(Recipe.Param.RisistAOIParam.ScratchParam.Name, InspImageList[Recipe.Param.RisistAOIParam.ScratchParam.InspID], Scratch, ScratchDefectCenter, Cell_Center, true, "#ff0000ff");
                    DefectResultList.Add(ScratchRes);
                }

                if (Recipe.Param.RisistAOIParam.StainParam.Enabled)
                {
                    Defect StainRes = new Defect(Recipe.Param.RisistAOIParam.StainParam.Name, InspImageList[Recipe.Param.RisistAOIParam.StainParam.InspID], Stain, StainDefectCenter, Cell_Center, true, "#ff0000ff");
                    DefectResultList.Add(StainRes);
                }

                if (Recipe.Param.RisistAOIParam.RDefectParam.Enabled)
                {
                    Defect RDefectRes = new Defect(Recipe.Param.RisistAOIParam.RDefectParam.Name, InspImageList[Recipe.Param.RisistAOIParam.RDefectParam.InspID], RDefect, RDefectCenter, Cell_Center, true, "#ff0000ff");
                    DefectResultList.Add(RDefectRes);
                }

                if (Recipe.Param.RisistAOIParam.RShiftNewParam.Enabled)
                {
                    Defect RShiftRes = new Defect(Recipe.Param.RisistAOIParam.RShiftNewParam.Name, InspImageList[Recipe.Param.RisistAOIParam.RDefectParam.InspID], RShiftCenter, RShiftCenter, Cell_Center, true, "#ff0000ff");
                    DefectResultList.Add(RShiftRes);
                }

                #endregion

                #endregion

                #region 取得AOI OK & Defect Regions

                HOperatorSet.ConcatObj(InnerMapRegions, OuterMapDefect, out TempReg);
                HOperatorSet.ConcatObj(RisistDefect, TempReg, out DefectMap);
                Convert2Circle(SegRegions, out SegRegionsC);
                Convert2Circle(DefectMap, out DefectC);
                HOperatorSet.SetSystem("tsp_store_empty_region", "false");
                HOperatorSet.Difference(SegRegionsC, DefectC, out AOI_OKRegionsC);
                HOperatorSet.Difference(SegRegionsC, AOI_OKRegionsC, out AOI_NGRegionsC);
                HOperatorSet.SetSystem("tsp_store_empty_region", "true");
                Convert2MapRegions(AOI_OKRegionsC, Recipe, out AOI_OKRegions);
                Convert2MapRegions(AOI_NGRegionsC, Recipe, out AOI_DefectRegions);

                #endregion

                #region 儲存AOI影像

                if (Recipe.Param.SaveAOIImgEnabled)
                {
                    DateTime Time = DateTime.Now;
                    string ImgPath = ModuleParamDirectory + ImageFileDirectory + "AOI_Cell_Image" + "\\" + Time.ToString("yyyyMMdd") + "\\"; // D disk

                    string SBID = "";
                    if (B_SB_InerCountID)
                        SBID = SB_InerCountID;
                    else
                        SBID = SB_ID;

                    clsSaveImg.clsPacket SavePacket = new clsSaveImg.clsPacket(InspImageList[Recipe.Param.DAVSImgID],
                                                                               AOI_DefectRegions, AOI_OKRegions,
                                                                               Recipe.Param.SaveAOIImgEnabled, Recipe.Param.SaveAOIImgType,
                                                                               Recipe.Param.SegParam.InspRectWidth, Recipe.Param.SegParam.InspRectHeight,
                                                                               ImgPath, ModuleName, SBID, MOVE_ID, PartNumber);
                    AOIMainForm.SaveAOICellImgThread.PushImg(SavePacket);
                }

                #endregion


                // 若AI不啟用
                if (Recipe.DAVSParam.DAVS_Mode == 0 && Recipe.Param.DAVSInspType != 0)
                {
                    Convert2Circle(AOI_DefectRegions, out MergeDefectRegions);

                    #region Dispose

                    SetPatternRegion(PatternRegions);
                    TempRegions.Dispose();
                    TempReg.Dispose();
                    PatternRegions.Dispose();
                    SegRegions.Dispose();
                    ThinRegion.Dispose();
                    StainRegion.Dispose();
                    ScratchRegion.Dispose();
                    AOI_OKRegions.Dispose();
                    AOI_DefectRegions.Dispose();
                    TempReg.Dispose();
                    TempRegions.Dispose();
                    TempReg.Dispose();


                    #endregion

                    Res = true;
                    return Res;
                }


                #region DAVS檢測方式
                Log.WriteLog("DAVS Start");
                switch (Recipe.Param.DAVSInspType)
                {
                    case 0:
                        {
                            #region 單獨執行AI
                            HObject TmpAIDefectRegions;
                            HOperatorSet.GenEmptyObj(out TmpAIDefectRegions);
                            if (Recipe.DAVSParam.DAVS_Mode != 0)
                            {
                                Recipe.DAVS.InsertPaternMatchSegRegions(SegRegions);

                                if (!Recipe.DAVS.DAVS_execute(Recipe.DAVSParam, InspImageList[methRole.Param.SegParam.SegImgIndex]))
                                {
                                    Log.WriteLog("DAVS DAVS_execute Fail");
                                    return Res;
                                }

                                ResList = Recipe.DAVS.GetPredictionRegionList();

                                for (int i = 0; i < ResList.Count; i++)
                                {
                                    if (ResList[i].Name != "OK")
                                    {
                                        HObject ExpTmpOutVar_0;
                                        HOperatorSet.Union2(TmpAIDefectRegions, ResList[i].ClassResultRegion, out ExpTmpOutVar_0);
                                        TmpAIDefectRegions.Dispose();
                                        TmpAIDefectRegions = ExpTmpOutVar_0;
                                    }
                                }

                                #region 加入檢測結果

                                if (Recipe.DAVSParam.DAVS_Mode == 1)
                                {
                                    Defect AIRes = new Defect("AI_NG", InspImageList[methRole.Param.SegParam.SegImgIndex], TmpAIDefectRegions, TmpAIDefectRegions, Cell_Center, true, "#ff0000ff");
                                    DefectResultList.Add(AIRes);
                                }

                                #endregion
                            }
                            HObject RealDefectRegion;
                            HOperatorSet.GenEmptyRegion(out RealDefectRegion);

                            HOperatorSet.ConcatObj(RealDefectRegion, InnerDefectRegions, out RealDefectRegion);
                            HOperatorSet.ConcatObj(RealDefectRegion, OuterDefectRegions, out RealDefectRegion);
                            HOperatorSet.ConcatObj(RealDefectRegion, Thin, out RealDefectRegion);
                            HOperatorSet.ConcatObj(RealDefectRegion, Scratch, out RealDefectRegion);
                            HOperatorSet.ConcatObj(RealDefectRegion, Stain, out RealDefectRegion);
                            HOperatorSet.ConcatObj(RealDefectRegion, RDefect, out RealDefectRegion);
                            HOperatorSet.ConcatObj(RealDefectRegion, Hole, out RealDefectRegion);



                            HOperatorSet.ConcatObj(TmpAIDefectRegions, InnerMapRegions, out MergeDefectRegions);
                            HOperatorSet.ConcatObj(MergeDefectRegions, OuterMapDefect, out MergeDefectRegions);

                            HOperatorSet.ConcatObj(ThinMap, MergeDefectRegions, out MergeDefectRegions);
                            HOperatorSet.ConcatObj(MergeDefectRegions, StainMap, out MergeDefectRegions);
                            HOperatorSet.ConcatObj(MergeDefectRegions, ScratchMap, out MergeDefectRegions);
                            HOperatorSet.ConcatObj(MergeDefectRegions, RDefectMap, out MergeDefectRegions);
                            HOperatorSet.ConcatObj(MergeDefectRegions, RShiftCenter, out MergeDefectRegions);
                            HOperatorSet.ConcatObj(MergeDefectRegions, HoleMap, out MergeDefectRegions);

                            HOperatorSet.Connection(MergeDefectRegions, out TempRegions);

                            Convert2Circle(TempRegions, out MergeDefectRegions);

                            HOperatorSet.ConcatObj(MergeDefectRegions, RealDefectRegion, out MergeDefectRegions);

                            TempRegions.Dispose();
                            #endregion
                        }
                        break;
                    case 1:
                        {
                            #region 覆判NG

                            Recipe.DAVS.InsertPaternMatchSegRegions(AOI_DefectRegions);

                            #region 若AI不啟用
                            if (Recipe.DAVSParam.DAVS_Mode == 2)
                            {
                                if (!Recipe.DAVS.DAVS_execute(Recipe.DAVSParam, InspImageList[methRole.Param.SegParam.SegImgIndex]))
                                {
                                    Log.WriteLog("DAVS DAVS_execute Fail");
                                    return Res;
                                }
                                Convert2Circle(AOI_DefectRegions, out MergeDefectRegions);

                                Res = true;
                                return Res;
                            }
                            #endregion

                            if (!Recipe.DAVS.DAVS_execute(Recipe.DAVSParam, InspImageList[methRole.Param.SegParam.SegImgIndex]))
                            {
                                Log.WriteLog("DAVS DAVS_execute Fail");
                                return Res;
                            }
                            HObject TmpAIDefectRegions;
                            HOperatorSet.GenEmptyObj(out TmpAIDefectRegions);
                            ResList = Recipe.DAVS.GetPredictionRegionList();

                            for (int i = 0; i < ResList.Count; i++)
                            {
                                if (ResList[i].Name != "OK")
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.Union2(TmpAIDefectRegions, ResList[i].ClassResultRegion, out ExpTmpOutVar_0);
                                    TmpAIDefectRegions.Dispose();
                                    TmpAIDefectRegions = ExpTmpOutVar_0;
                                }
                            }

                            Defect AIRes = new Defect("AI_NG", InspImageList[methRole.Param.SegParam.SegImgIndex], TmpAIDefectRegions, TmpAIDefectRegions, Cell_Center, true, "#ff0000ff");
                            DefectResultList.Add(AIRes);

                            TempRegions.Dispose();
                            HOperatorSet.Connection(TmpAIDefectRegions, out TempRegions);
                            Convert2Circle(TempRegions, out MergeDefectRegions);
                            #endregion
                        }
                        break;
                    case 2:
                        {
                            #region 覆判OK


                            Recipe.DAVS.InsertPaternMatchSegRegions(AOI_OKRegions);

                            if (!Recipe.DAVS.DAVS_execute(Recipe.DAVSParam, InspImageList[methRole.Param.SegParam.SegImgIndex]))
                            {
                                Log.WriteLog("DAVS DAVS_execute Fail");
                                return Res;
                            }

                            ResList = Recipe.DAVS.GetPredictionRegionList();

                            HObject TmpAIDefectRegions;
                            HOperatorSet.GenEmptyObj(out TmpAIDefectRegions);

                            for (int i = 0; i < ResList.Count; i++)
                            {
                                if (ResList[i].Name != "OK")
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.Union2(TmpAIDefectRegions, ResList[i].ClassResultRegion, out ExpTmpOutVar_0);
                                    TmpAIDefectRegions.Dispose();
                                    TmpAIDefectRegions = ExpTmpOutVar_0;
                                }
                            }

                            Defect AIRes = new Defect("AI_NG", InspImageList[methRole.Param.SegParam.SegImgIndex], TmpAIDefectRegions, TmpAIDefectRegions, Cell_Center, true, "#ff0000ff");
                            DefectResultList.Add(AIRes);

                            HOperatorSet.ConcatObj(TmpAIDefectRegions, InnerMapRegions, out MergeDefectRegions);
                            HOperatorSet.ConcatObj(MergeDefectRegions, OuterMapDefect, out MergeDefectRegions);
                            HOperatorSet.ConcatObj(ThinMap, MergeDefectRegions, out MergeDefectRegions);
                            HOperatorSet.ConcatObj(MergeDefectRegions, StainMap, out MergeDefectRegions);
                            HOperatorSet.ConcatObj(MergeDefectRegions, ScratchMap, out MergeDefectRegions);
                            HOperatorSet.ConcatObj(MergeDefectRegions, RDefectMap, out MergeDefectRegions);
                            HOperatorSet.ConcatObj(MergeDefectRegions, RShiftCenter, out MergeDefectRegions);

                            HOperatorSet.Connection(MergeDefectRegions, out TempRegions);

                            MergeDefectRegions.Dispose();
                            Convert2Circle(TempRegions, out MergeDefectRegions);
                            #endregion
                        }
                        break;

                    default:
                        break;


                }
                Log.WriteLog("DAVS End");
                #endregion

                Log.WriteLog("Insp Proc Execute End");

                #region Dispose

                SetPatternRegion(PatternRegions);
                TempRegions.Dispose();
                TempReg.Dispose();
                PatternRegions.Dispose();
                SegRegions.Dispose();
                ThinRegion.Dispose();
                StainRegion.Dispose();
                ScratchRegion.Dispose();
                AOI_OKRegions.Dispose();
                AOI_DefectRegions.Dispose();
                TempReg.Dispose();
                TempRegions.Dispose();
                AOI_OKRegionsC.Dispose();
                AOI_NGRegionsC.Dispose();
                DefectC.Dispose();
                TempReg.Dispose();


                #endregion
			}
            catch(Exception Ex)
            {
                Log.WriteLog(Ex.ToString());
                return Res;
            }

            Res = true;
            return Res;
        }

        public class clsAOIResult
        {
            public HObject DefectRegion;
            public HObject DefectMapRegion;
            public string Name;
            public int ImageID;
            public int Priority;
            public string Color = "";
        }

        /// <summary>
        /// 演算法實作
        /// </summary>
        /// <param name="Recipe"></param>
        /// <param name="ho_ImgList"></param>
        /// <param name="bOutputIndex"></param>
        /// <param name="MergeDefectRegions">瑕疵Region (顯示於該顆中心點)</param>
        /// <param name="DefectResultList">各類型瑕疵或顯示Region</param>
        /// <param name="DefectMethodIndex"></param>
        /// <returns></returns>
        public bool executeNew(InductanceInspRole Recipe, List<HObject> ho_ImgList, bool bOutputIndex, out HObject MergeDefectRegions, out List<Defect> DefectResultList, out List<int> DefectMethodIndex) // (20200130) Jeff Revised!
        {
            bool Res = false;
            DefectMethodIndex = new List<int>();
            Log.SetLogEnabled(Recipe.Param.LogEnabled);
            if (sb_InerCountID != SB_InerCountID)
            {
                sb_InerCountID = SB_InerCountID;
                Log.WriteLog("※基板序號: " + SB_InerCountID);
            }
            Log.WriteLog("檢測位置: " + MOVE_ID);
            Log.WriteLog(MOVE_ID + "_Insp_Start");

            #region 變數宣告

            List<clsDAVS.clsResultRegion> ResList = new List<clsDAVS.clsResultRegion>();
            MergeDefectRegions = null;
            List<HObject> InspImageList = new List<HObject>();
            List<HObject> ResultImages = new List<HObject>(); // (20200130) Jeff Revised!
            List<HObject> ResultRegions = new List<HObject>(); // (20200729) Jeff Revised!
            List<HObject> ResultImages_UsedRegion = new List<HObject>(); // (20200130) Jeff Revised!
            List<HObject> ResultRegions_UsedRegion = new List<HObject>(); // (20200729) Jeff Revised!
            List<HObject> InspRegionList = new List<HObject>(); // (20200130) Jeff Revised!
            Recipe.Init(Recipe.Param.SaveAOIImgEnabled);
            HTuple SB_Angle_Rad;
            DefectResultList = null;
            DefectResultList = new List<Defect>();
            HTuple Row = null, Column = null, Angle = null, Score = null; // Match結果
            HObject CellRegion;//Cell Region
            HObject DefectMap;
            HObject CellCenter;//所有Cell中心位置
            HObject AOI_OKRegions = null, AOI_DefectRegions = null;
            var Task = new List<Task<HObject>>(); // Task List
            HObject PatternRegions;

            HOperatorSet.GenEmptyObj(out CellCenter);
            HOperatorSet.GenEmptyObj(out DefectMap);
            HOperatorSet.GenEmptyObj(out CellRegion);
            HOperatorSet.GenEmptyObj(out MergeDefectRegions);

            HObject Rect1_CellRegion = null;
            HOperatorSet.GenEmptyRegion(out Rect1_CellRegion); // (20200429) Jeff Revised!

            #endregion

            #region 測試模式

            HOperatorSet.TupleRad(Affine_angle_degree, out SB_Angle_Rad);

            if (Recipe.Param.RotateImageEnabled) // 影像旋轉
            {
                GetRotateImageList(-SB_Angle_Rad, Recipe, ho_ImgList, out InspImageList);
            }
            else
            {
                InspImageList = ho_ImgList.ToList();
            }

            if (Recipe.Param.TestModeEnabled && Recipe.Param.TestModeType == 0)
            {
                Log.WriteLog("Insp Proc Execute ByPass");
                HOperatorSet.GenEmptyObj(out MergeDefectRegions);
                Res = true;
                return Res;
            }

            #endregion

            try
            {
                #region 影像演算法A (不使用範圍列表) // (20200130) Jeff Revised!

                Log.WriteLog(MOVE_ID + "_影像演算法A Start");

                Recipe.Param.AlgoImg.ImageSource = InspImageList;
                if (!(Recipe.Param.AlgoImg.Execute(out ResultImages, out ResultRegions))) // (20200729) Jeff Revised!
                    throw new Exception("執行影像演算法A失敗");

                Log.WriteLog(MOVE_ID + "_影像演算法A End");

                #endregion

                #region 取得Match中心 &  Cell Region 

                Log.WriteLog(MOVE_ID + "_Pattern Match Start");

                HObject SrcImg_Match; // (20200130) Jeff Revised!
                if (Recipe.Param.SegParam.ImageSource_SegImg == enu_ImageSource.原始)
                    SrcImg_Match = InspImageList[Recipe.Param.SegParam.SegImgIndex];
                else
                    SrcImg_Match = ResultImages[Recipe.Param.SegParam.SegImgIndex];

                if (!(PtternMatchAndSegRegion_New(SrcImg_Match, Recipe, out PatternRegions, out Angle, out Row, out Column, out CellRegion))) // (20200130) Jeff Revised!
                    throw new Exception("Pattern Match失敗");

                HOperatorSet.ShapeTrans(CellRegion, out CellCenter, "inner_center");

                if (Recipe.Param.AdvParam.MatchNumber != 0 && Column.Length != Recipe.Param.AdvParam.MatchNumber)
                {
                    //Res = false;
                    BtnLog.WriteLog("個數錯誤_" + Column.Length);
                    //return Res;
                    throw new Exception("個數錯誤_" + Column.Length); // (20200130) Jeff Revised!
                }

                Log.WriteLog(MOVE_ID + "_Pattern Match End");

                #endregion

                #region 取得所有使用Region

                Log.WriteLog(MOVE_ID + "_Get Insp Region Start");
                
                List<string> name_UsedRegionList = new List<string>(); // (20200130) Jeff Revised!
                for (int i = 0; i < Recipe.Param.UsedRegionList.Count; i++)
                {
                    HObject InspRegion;
                    HOperatorSet.GenEmptyObj(out InspRegion);
                    if (!GetUsedRegion(PatternRegions, Row, Column, Angle, Score, Recipe, i, CellRegion, out InspRegion))
                    {
                        continue;
                    }
                    InspRegionList.Add(InspRegion);
                    name_UsedRegionList.Add(Recipe.Param.UsedRegionList[i].Name);
                }

                Log.WriteLog(MOVE_ID + "_Get Insp Region End");

                #endregion

                #region 影像區域演算法B (使用範圍列表) // (20200130) Jeff Revised!

                Log.WriteLog(MOVE_ID + "_影像區域演算法B Start");

                List<HObject> InspRegionList_clone = new List<HObject>(); // (20200429) Jeff Revised!
                foreach (HObject reg in InspRegionList)
                    InspRegionList_clone.Add(reg.Clone());
                Recipe.Param.AlgoImg_UsedRegion.Algo_Image_Constructor(true, InspRegionList_clone, name_UsedRegionList); // 避免記憶體被釋放導致檢視Region時會有例外狀況 (20200429) Jeff Revised!
                Recipe.Param.AlgoImg_UsedRegion.ImageSource = InspImageList;
                if (!(Recipe.Param.AlgoImg_UsedRegion.Execute(out ResultImages_UsedRegion, out ResultRegions_UsedRegion))) // (20200729) Jeff Revised!
                    throw new Exception("執行影像區域演算法B失敗");

                Log.WriteLog(MOVE_ID + "_影像區域演算法B End");

                #endregion

                #region 檢測開始 & 取得檢測結果

                Log.WriteLog(MOVE_ID + "_Algorithm Task Start");

                // All Inspection Task Start
                for (int i = 0; i < Recipe.Param.AlgorithmList.Count; i++)
                {
                    InspParam InspParam = new InspParam(InspImageList, InspRegionList[Recipe.Param.AlgorithmList[i].SelectRegionIndex], Recipe, i, Locate_Method_FS.pixel_resolution_, Recipe.Param.AlgorithmList[i].Type, ResultImages, ResultImages_UsedRegion); // (20200130) Jeff Revised!
                    var SubTask = new Task<HObject>(AlgorithmTask, InspParam, TaskCreationOptions.LongRunning);
                    SubTask.ConfigureAwait(false);
                    Task.Add(SubTask);
                    Task[i].Start();
                }

                // 取得所有檢測結果
                List<clsAOIResult> AOIResList = new List<clsAOIResult>();
                HObject EnableDAVSInspRegion, DisableDAVSInspRegion;
                HOperatorSet.GenEmptyObj(out EnableDAVSInspRegion);
                HOperatorSet.GenEmptyObj(out DisableDAVSInspRegion);

                for (int i = 0; i < Recipe.Param.AlgorithmList.Count; i++)
                {
                    clsAOIResult TmpRes = new clsAOIResult();
                    if (Recipe.Param.AlgorithmList[i].ImageSource == enu_ImageSource.原始) // (20200130) Jeff Revised!
                        TmpRes.ImageID = Recipe.Param.AlgorithmList[i].InspImageIndex;
                    else
                        TmpRes.ImageID = 0;
                    TmpRes.DefectRegion = Task[i].Result;
                    TmpRes.Name = (Recipe.Param.AlgorithmList[i].DefectName);
                    TmpRes.Priority = Recipe.Param.AlgorithmList[i].Priority;
                    TmpRes.Color = Recipe.Param.AlgorithmList[i].Color;
                    HObject MapRegion;
                    LocalDefectMappping(TmpRes.DefectRegion, CellRegion, out MapRegion);
                    HOperatorSet.ConcatObj(DefectMap, MapRegion, out DefectMap);
                    HOperatorSet.ShapeTrans(MapRegion, out TmpRes.DefectMapRegion, "inner_center");
                    if (bOutputIndex)
                    {
                        HTuple C;
                        HOperatorSet.CountObj(TmpRes.DefectMapRegion, out C);
                        if (C != 0)
                            DefectMethodIndex.Add(i);
                    }
                    AOIResList.Add(TmpRes);
                    if (Recipe.Param.AlgorithmList[i].bUsedDAVS)
                        HOperatorSet.ConcatObj(EnableDAVSInspRegion, MapRegion, out EnableDAVSInspRegion);
                    else
                        HOperatorSet.ConcatObj(DisableDAVSInspRegion, MapRegion, out DisableDAVSInspRegion);
                }

                for (int i = 0; i < Recipe.Param.UsedRegionList.Count; i++)
                {
                    if (!Recipe.Param.UsedRegionList[i].bShowRegion)
                        continue;

                    HObject InspCenter;
                    HOperatorSet.GenEmptyObj(out InspCenter);
                    Defect ShowMatchRes = new Defect(Recipe.Param.UsedRegionList[i].Name, InspImageList[Recipe.Param.UsedRegionList[i].ShowImageID], InspRegionList[i], InspCenter, CellCenter, false, Recipe.Param.UsedRegionList[i].Color, 0);
                    DefectResultList.Add(ShowMatchRes);
                }

                // Merge Same Result
                List<clsAOIResult> MergeAOIResult = new List<clsAOIResult>();

                for (int i = 0; i < Recipe.Param.PriorityLayerCount + 1; i++)
                {
                    List<clsAOIResult> TempList = AOIResList.FindAll(x => x.Priority == i);
                    clsAOIResult Tmp = new clsAOIResult();
                    HOperatorSet.GenEmptyObj(out Tmp.DefectRegion);
                    HOperatorSet.GenEmptyObj(out Tmp.DefectMapRegion);
                    if (TempList.Count > 0)
                    {
                        Tmp.Color = TempList[0].Color;
                        Tmp.Name = TempList[0].Name;
                        Tmp.ImageID = TempList[0].ImageID;
                        Tmp.Priority = TempList[0].Priority;
                        for (int j = 0; j < TempList.Count; j++)
                        {
                            HOperatorSet.Union2(Tmp.DefectRegion, TempList[j].DefectRegion, out Tmp.DefectRegion);
                            HOperatorSet.Union2(Tmp.DefectMapRegion, TempList[j].DefectMapRegion, out Tmp.DefectMapRegion);
                        }
                    }
                    HOperatorSet.Connection(Tmp.DefectMapRegion, out Tmp.DefectMapRegion);
                    MergeAOIResult.Add(Tmp);
                }

                // 寫入檢測結果
                int Index = 0;
                foreach (clsAOIResult AOIRes in MergeAOIResult)
                {
                    if (AOIRes.Name == null)
                        continue;
                    Defect AllAOIRes = new Defect(AOIRes.Name, InspImageList[AOIRes.ImageID], AOIRes.DefectRegion, AOIRes.DefectMapRegion, CellCenter, true, AOIRes.Color, Index);
                    DefectResultList.Add(AllAOIRes);
                    Index++;
                }

                Log.WriteLog(MOVE_ID + "_Algorithm Task End");

                #endregion

                #region 取得AOI OK & Defect Region

                HObject EnabledDAVS_OK, EnabledDAVS_Defect;

                HObject DisableDAVS_OK, DisableDAVS_Defect;

                GetMapRegion(CellRegion, DefectMap, Recipe, out AOI_OKRegions, out AOI_DefectRegions);

                GetMapRegion(CellRegion, EnableDAVSInspRegion, Recipe, out EnabledDAVS_OK, out EnabledDAVS_Defect);

                GetMapRegion(CellRegion, DisableDAVSInspRegion, Recipe, out DisableDAVS_OK, out DisableDAVS_Defect);

                #endregion

                #region 儲存AOI影像

                if (Recipe.Param.SaveAOIImgEnabled)
                {
                    DateTime Time = DateTime.Now;
                    string ImgPath = ModuleParamDirectory + ImageFileDirectory + "AOI_Cell_Image" + "\\" + Time.ToString("yyyyMMdd") + "\\";

                    string SBID = "";
                    if (B_SB_InerCountID)
                        SBID = SB_InerCountID;
                    else
                        SBID = SB_ID;

                    HObject SaveImage;
                    if (!Recipe.Param.bDAVSMixImgBand)
                        SaveImage = InspImageList[Recipe.Param.DAVSImgID];
                    else
                        SaveImage = clsStaticTool.MixImageBand(InspImageList, Recipe.Param.DAVSBand1, Recipe.Param.DAVSBand2, Recipe.Param.DAVSBand3, Recipe.Param.DAVSBand1ImgIndex, Recipe.Param.DAVSBand2ImgIndex, Recipe.Param.DAVSBand3ImgIndex);



                    clsSaveImg.clsPacket SavePacket = new clsSaveImg.clsPacket(SaveImage,
                                                                               AOI_DefectRegions, AOI_OKRegions,
                                                                               Recipe.Param.SaveAOIImgEnabled, Recipe.Param.SaveAOIImgType,
                                                                               Recipe.Param.SegParam.InspRectWidth, Recipe.Param.SegParam.InspRectHeight,
                                                                               ImgPath, ModuleName, SBID, MOVE_ID, PartNumber);
                    AOIMainForm.SaveAOICellImgThread.PushImg(SavePacket);
                }

                #endregion

                #region Get Rect1 to AI Region

                Extension.HObjectMedthods.ReleaseHObject(ref Rect1_CellRegion); // (20200429) Jeff Revised!
                HOperatorSet.ConcatObj(AOI_OKRegions, AOI_DefectRegions, out Rect1_CellRegion);

                //if (FinalInspectParam.b_Recheck || FinalInspectParam.b_AICellImg_Enable) // 人工覆判 或 AI分圖 (20200429) Jeff Revised!
                //{
                //    if (!b_InductanceInspUC)
                //    {
                //        lock (LockObj)
                //            CellReg_MoveIndex_FS.Add(Rect1_CellRegion);
                //    }
                //}

                #endregion

                if (Recipe.DAVSParam.DAVS_Mode == 0)
                {
                    #region DAVS 不啟用

                    this.Convert2Circle(AOI_DefectRegions, out MergeDefectRegions);

                    #endregion
                }
                else
                {
                    #region DAVS 檢測Type

                    switch (Recipe.Param.DAVSInspType)
                    {
                        case 0:
                            {
                                #region 單獨執行AI

                                HObject AIDefectRegions;

                                #region 設定檢測範圍 &  執行DAVS



                                Recipe.DAVS.InsertPaternMatchSegRegions(Rect1_CellRegion);

                                HObject SaveImage;
                                if (!Recipe.Param.bDAVSMixImgBand)
                                    SaveImage = InspImageList[Recipe.Param.DAVSImgID];
                                else
                                    SaveImage = clsStaticTool.MixImageBand(InspImageList, Recipe.Param.DAVSBand1, Recipe.Param.DAVSBand2, Recipe.Param.DAVSBand3, Recipe.Param.DAVSBand1ImgIndex, Recipe.Param.DAVSBand2ImgIndex, Recipe.Param.DAVSBand3ImgIndex);


                                if (!Recipe.DAVS.DAVS_execute(Recipe.DAVSParam, SaveImage))
                                {
                                    Log.WriteLog("DAVS DAVS_execute Fail");
                                    return Res;
                                }
                                #endregion

                                #region 取得DAVS檢測結果

                                GetDAVSDefectResult(Recipe.DAVS.GetPredictionRegionList(), out AIDefectRegions);

                                #endregion

                                #region 加入檢測結果

                                if (Recipe.DAVSParam.DAVS_Mode == 1)
                                {
                                    Defect AIRes = new Defect("AI_NG", InspImageList[Recipe.Param.DAVSImgID], AIDefectRegions, AIDefectRegions, CellCenter, true, "#ff0000ff");
                                    DefectResultList.Add(AIRes);
                                }

                                #endregion


                                #region 合併結果

                                HObject AOIMergeDAVS, Connection;
                                HOperatorSet.ConcatObj(AOI_DefectRegions, AIDefectRegions, out AOIMergeDAVS);
                                HOperatorSet.Connection(AOIMergeDAVS, out Connection);
                                Convert2Circle(Connection, out MergeDefectRegions);

                                #endregion

                                #endregion
                            }
                            break;
                        case 1:
                            {
                                #region 覆判NG

                                HTuple NGCount;
                                HOperatorSet.CountObj(EnabledDAVS_Defect, out NGCount);

                                Recipe.DAVS.InsertPaternMatchSegRegions(EnabledDAVS_Defect);

                                #region DAVS 離線存圖
                                if (Recipe.DAVSParam.DAVS_Mode == 2)
                                {
                                    HObject NGSaveImage;
                                    if (!Recipe.Param.bDAVSMixImgBand)
                                        NGSaveImage = InspImageList[Recipe.Param.DAVSImgID];
                                    else
                                        NGSaveImage = clsStaticTool.MixImageBand(InspImageList, Recipe.Param.DAVSBand1, Recipe.Param.DAVSBand2, Recipe.Param.DAVSBand3, Recipe.Param.DAVSBand1ImgIndex, Recipe.Param.DAVSBand2ImgIndex, Recipe.Param.DAVSBand3ImgIndex);


                                    if (!Recipe.DAVS.DAVS_execute(Recipe.DAVSParam, NGSaveImage))
                                    {
                                        Log.WriteLog("DAVS DAVS_execute Fail");
                                        return Res;
                                    }

                                    Convert2Circle(AOI_DefectRegions, out MergeDefectRegions);

                                    Res = true;
                                    return Res;
                                }
                                #endregion

                                #region 執行DAVS檢測

                                HObject SaveImage;
                                if (!Recipe.Param.bDAVSMixImgBand)
                                    SaveImage = InspImageList[Recipe.Param.DAVSImgID];
                                else
                                    SaveImage = clsStaticTool.MixImageBand(InspImageList, Recipe.Param.DAVSBand1, Recipe.Param.DAVSBand2, Recipe.Param.DAVSBand3, Recipe.Param.DAVSBand1ImgIndex, Recipe.Param.DAVSBand2ImgIndex, Recipe.Param.DAVSBand3ImgIndex);


                                HObject AIDefectRegions;
                                HOperatorSet.GenEmptyObj(out AIDefectRegions);
                                if (NGCount.D == 0)
                                {
                                    Defect AIResNG = new Defect("AI_NG", InspImageList[Recipe.Param.DAVSImgID], AIDefectRegions, AIDefectRegions, CellCenter, true, "#ff0000ff");
                                    DefectResultList.Add(AIResNG);

                                    HObject NGMergeRegion;
                                    HOperatorSet.ConcatObj(AIDefectRegions, DisableDAVS_Defect, out NGMergeRegion);

                                    HObject NGConnection;
                                    HOperatorSet.Connection(NGMergeRegion, out NGConnection);
                                    Convert2Circle(NGConnection, out MergeDefectRegions);
                                    break;
                                }

                                if (!Recipe.DAVS.DAVS_execute(Recipe.DAVSParam, SaveImage))
                                {
                                    Log.WriteLog("DAVS DAVS_execute Fail");
                                    return Res;
                                }

                                #endregion

                                #region 取得DAVS檢測結果

                                //HObject AIDefectRegions;
                                GetDAVSDefectResult(Recipe.DAVS.GetPredictionRegionList(), out AIDefectRegions);
                                HOperatorSet.Connection(AIDefectRegions, out AIDefectRegions);

                                #endregion

                                #region 加入檢測結果

                                Defect AIRes = new Defect("AI_NG", InspImageList[Recipe.Param.DAVSImgID], AIDefectRegions, AIDefectRegions, CellCenter, true, "#ff0000ff");
                                DefectResultList.Add(AIRes);

                                #endregion

                                #region 合併結果

                                //AOI DisableDAVS Result + AI Result
                                HObject MergeRegion;
                                HOperatorSet.ConcatObj(AIDefectRegions, DisableDAVS_Defect, out MergeRegion);

                                HObject Connection;
                                HOperatorSet.Connection(MergeRegion, out Connection);
                                Convert2Circle(Connection, out MergeDefectRegions);

                                #endregion

                                #endregion
                            }
                            break;
                        case 2:
                            {
                                #region 覆判OK

                                #region 執行DAVS檢測

                                HTuple OKCount;
                                HOperatorSet.CountObj(EnabledDAVS_OK, out OKCount);


                                Recipe.DAVS.InsertPaternMatchSegRegions(EnabledDAVS_OK);

                                HObject SaveImage;
                                if (!Recipe.Param.bDAVSMixImgBand)
                                    SaveImage = InspImageList[Recipe.Param.DAVSImgID];
                                else
                                    SaveImage = clsStaticTool.MixImageBand(InspImageList, Recipe.Param.DAVSBand1, Recipe.Param.DAVSBand2, Recipe.Param.DAVSBand3, Recipe.Param.DAVSBand1ImgIndex, Recipe.Param.DAVSBand2ImgIndex, Recipe.Param.DAVSBand3ImgIndex);

                                HObject AIDefectRegions;
                                HOperatorSet.GenEmptyObj(out AIDefectRegions);
                                if (OKCount.D == 0)
                                {
                                    Defect NOOKRes = new Defect("AI_NG", InspImageList[Recipe.Param.DAVSImgID], AIDefectRegions, AIDefectRegions, CellCenter, true, "#ff0000ff");
                                    DefectResultList.Add(NOOKRes);

                                    HObject NAOIMergeDAVS, NConnection;
                                    HOperatorSet.ConcatObj(AOI_DefectRegions, AIDefectRegions, out NAOIMergeDAVS);
                                    HOperatorSet.Connection(NAOIMergeDAVS, out NConnection);
                                    Convert2Circle(NConnection, out MergeDefectRegions);
                                    break;
                                }

                                if (!Recipe.DAVS.DAVS_execute(Recipe.DAVSParam, SaveImage))
                                {
                                    Log.WriteLog("DAVS DAVS_execute Fail");
                                    return Res;
                                }

                                #endregion

                                #region 取得DAVS檢測結果

                                GetDAVSDefectResult(Recipe.DAVS.GetPredictionRegionList(), out AIDefectRegions);
                                HOperatorSet.Connection(AIDefectRegions, out AIDefectRegions);

                                #endregion

                                #region 加入檢測結果

                                Defect AIRes = new Defect("AI_NG", InspImageList[Recipe.Param.DAVSImgID], AIDefectRegions, AIDefectRegions, CellCenter, true, "#ff0000ff");
                                DefectResultList.Add(AIRes);

                                #endregion

                                #region 合併結果

                                HObject AOIMergeDAVS, Connection;
                                HOperatorSet.ConcatObj(AOI_DefectRegions, AIDefectRegions, out AOIMergeDAVS);
                                HOperatorSet.Connection(AOIMergeDAVS, out Connection);
                                Convert2Circle(Connection, out MergeDefectRegions);

                                #endregion

                                #endregion
                            }
                            break;

                        default:
                            break;
                    }

                    #endregion

                }

                Res = true;
            }
            catch (Exception Ex)
            {
                Log.WriteLog(Ex.ToString());
                //return Res;
            }
            finally // (20200130) Jeff Revised!
            {
                #region 人工覆判 或 AI分圖

                if (FinalInspectParam.b_Recheck || FinalInspectParam.b_AICellImg_Enable) // (20200429) Jeff Revised!
                {
                    if (!b_InductanceInspUC)
                    {
                        // Cell Region
                        if (Rect1_CellRegion != null)
                        {
                            lock (LockObj)
                                CellReg_MoveIndex_FS.Add(Rect1_CellRegion.Clone());
                        }
                        else // For debug! // (20200429) Jeff Revised!
                            ;

                        // 瑕疵 Region
                        HObject RealDefectRegion = MergeDefectRegions.Clone();
                        for (int i = 0; i < DefectResultList.Count; i++)
                        {
                            if (!DefectResultList[i].B_defect)
                                continue;

                            HOperatorSet.Union2(RealDefectRegion, DefectResultList[i].DefectRegion, out RealDefectRegion);
                        }
                        lock (LockObj)
                            DefReg_MoveIndex_FS.Add(RealDefectRegion);
                    }
                }

                #endregion

                #region Dispose

                // (20200130) Jeff Revised!
                if (Recipe.Param.RotateImageEnabled) // 影像旋轉
                {
                    clsStaticTool.DisposeAll(InspImageList);
                }

                // (20200130) Jeff Revised!
                clsStaticTool.DisposeAll(ResultImages);
                clsStaticTool.DisposeAll(ResultRegions); // (20200729) Jeff Revised!
                clsStaticTool.DisposeAll(ResultImages_UsedRegion);
                clsStaticTool.DisposeAll(ResultRegions_UsedRegion); // (20200729) Jeff Revised!
                //clsStaticTool.DisposeAll(InspRegionList); // (20200319) Jeff Revised!

                SetPatternRegion(CellCenter);
                Extension.HObjectMedthods.ReleaseHObject(ref AOI_OKRegions);
                Extension.HObjectMedthods.ReleaseHObject(ref AOI_DefectRegions);

                //for (int i = 0; i < Task.Count; i++)
                //{
                //    if (Task[i] != null)
                //    {
                //        Task[i].Dispose();
                //        Task[i] = null;
                //    }
                //}
                clsStaticTool.DisposeAll(Task);

                Extension.HObjectMedthods.ReleaseHObject(ref Rect1_CellRegion); // (20200429) Jeff Revised!

                #endregion
            }

            Log.WriteLog(MOVE_ID + "Insp_End");
            return Res;
        }

        public void GetMapRegion(HObject CellRegion, HObject DefectMap, InductanceInspRole Recipe, out HObject OKRegions, out HObject DefectRegions)
        {
            HOperatorSet.GenEmptyObj(out OKRegions);
            HOperatorSet.GenEmptyObj(out DefectRegions);
            HObject DefectC, CellRegionC;
            HObject AOI_OKRegionsC, AOI_NGRegionsC;
            Convert2Circle(CellRegion, out CellRegionC);
            Convert2Circle(DefectMap, out DefectC);
            HOperatorSet.SetSystem("tsp_store_empty_region", "false");
            HOperatorSet.Difference(CellRegionC, DefectC, out AOI_OKRegionsC);
            HOperatorSet.Difference(CellRegionC, AOI_OKRegionsC, out AOI_NGRegionsC);
            HOperatorSet.SetSystem("tsp_store_empty_region", "true");
            Convert2MapRegions(AOI_OKRegionsC, Recipe, out OKRegions);
            Convert2MapRegions(AOI_NGRegionsC, Recipe, out DefectRegions);
        }
        

        public static void GetDAVSDefectResult(List<clsDAVS.clsResultRegion> ResList, out HObject DAVSDefectRegion)
        {
            HOperatorSet.GenEmptyObj(out DAVSDefectRegion);
            for (int i = 0; i < ResList.Count; i++)
            {
                if (ResList[i].Name != "OK")
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Union2(DAVSDefectRegion, ResList[i].ClassResultRegion, out ExpTmpOutVar_0);
                    DAVSDefectRegion.Dispose();
                    DAVSDefectRegion = ExpTmpOutVar_0;
                }
            }
        }

        public static void GetDAVSInspDefectResult(List<clsDAVSInspMethod.clsResultRegion> ResList, out HObject DAVSDefectRegion)
        {
            HOperatorSet.GenEmptyObj(out DAVSDefectRegion);
            for (int i = 0; i < ResList.Count; i++)
            {
                if (ResList[i].Name != "OK")
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Union2(DAVSDefectRegion, ResList[i].ClassResultRegion, out ExpTmpOutVar_0);
                    DAVSDefectRegion.Dispose();
                    DAVSDefectRegion = ExpTmpOutVar_0;
                }
            }
        }

        public static void GetCellRegion_New(HObject PatternRegion, HTuple Row, HTuple Column, HTuple Angle, HTuple Score, InductanceInspRole Recipe, out HObject CellRegion)
        {
            HOperatorSet.GenEmptyObj(out CellRegion);
            if (Score.Length <= 0)
            {
                return;
            }


            HTuple Area, CenterY, CenterX;
            HOperatorSet.AreaCenter(PatternRegion, out Area, out CenterY, out CenterX);
            HTuple AWidth, AHeight;
            HOperatorSet.TupleGenConst(Row.Length, Recipe.Param.SegParam.InspRectWidth, out AWidth);
            HOperatorSet.TupleGenConst(Row.Length, Recipe.Param.SegParam.InspRectHeight, out AHeight);

            HOperatorSet.GenRectangle2(out CellRegion, CenterY - Recipe.Param.SegParam.HotspotY, CenterX - Recipe.Param.SegParam.HotspotX, Angle, AWidth / 2, AHeight / 2);

            //HOperatorSet.GenRectangle1(out CellRegion, SY, SX, EY, EX);
            if (!Recipe.Param.bInspOutboundary)
            {
                HOperatorSet.SelectShape(CellRegion, out CellRegion, "row1", "and", 0, Recipe.Param.ImageHeight);
                HOperatorSet.SelectShape(CellRegion, out CellRegion, "row2", "and", 0, Recipe.Param.ImageHeight);
                HOperatorSet.SelectShape(CellRegion, out CellRegion, "column1", "and", 0, Recipe.Param.ImageWidth);
                HOperatorSet.SelectShape(CellRegion, out CellRegion, "column2", "and", 0, Recipe.Param.ImageWidth);
            }
        }

        public static void GetCellRegion(HTuple Row, HTuple Column, HTuple Angle, HTuple Score, InductanceInspRole Recipe, out HObject CellRegion)
        {
            HOperatorSet.GenEmptyObj(out CellRegion);
            if (Score.Length <= 0)
            {
                return;
            }


            HTuple CX;
            HTuple CY;

            HTuple SY;
            HTuple SX;
            HTuple EY;
            HTuple EX;

            CX = Column / Recipe.Param.AdvParam.MatchSacleSize - Recipe.Param.SegParam.CellMatchHotspotX / Recipe.Param.AdvParam.MatchSacleSize;
            CY = Row / Recipe.Param.AdvParam.MatchSacleSize - Recipe.Param.SegParam.CellMatchHotspotY / Recipe.Param.AdvParam.MatchSacleSize;

            SY = CY - Recipe.Param.SegParam.InspRectHeight / 2;
            SX = CX - Recipe.Param.SegParam.InspRectWidth / 2;
            EY = CY + Recipe.Param.SegParam.InspRectHeight / 2;
            EX = CX + Recipe.Param.SegParam.InspRectWidth / 2;

            HTuple AWidth, AHeight;
            HOperatorSet.TupleGenConst(Row.Length, Recipe.Param.SegParam.InspRectWidth, out AWidth);
            HOperatorSet.TupleGenConst(Row.Length, Recipe.Param.SegParam.InspRectHeight, out AHeight);

            HOperatorSet.GenRectangle2(out CellRegion, Row, Column, Angle, AWidth / 2, AHeight / 2);


            //HOperatorSet.GenRectangle1(out CellRegion, SY, SX, EY, EX);
            if (!Recipe.Param.bInspOutboundary)
            {
                HOperatorSet.SelectShape(CellRegion, out CellRegion, "row1", "and", 0, Recipe.Param.ImageHeight);
                HOperatorSet.SelectShape(CellRegion, out CellRegion, "row2", "and", 0, Recipe.Param.ImageHeight);
                HOperatorSet.SelectShape(CellRegion, out CellRegion, "column1", "and", 0, Recipe.Param.ImageWidth);
                HOperatorSet.SelectShape(CellRegion, out CellRegion, "column2", "and", 0, Recipe.Param.ImageWidth);
            }
        }

        /// <summary>
        /// 取得 使用範圍列表 中之單一Region
        /// </summary>
        /// <param name="PatternRegion"></param>
        /// <param name="Row"></param>
        /// <param name="Column"></param>
        /// <param name="Angle"></param>
        /// <param name="Score"></param>
        /// <param name="Recipe"></param>
        /// <param name="SelectRegionIndex"></param>
        /// <param name="CellRegion"></param>
        /// <param name="InspRegion"></param>
        /// <returns></returns>
        public static bool GetUsedRegion(HObject PatternRegion,HTuple Row, HTuple Column, HTuple Angle, HTuple Score, InductanceInspRole Recipe, int SelectRegionIndex, HObject CellRegion, out HObject InspRegion)
        {
            bool Status = false;
            HOperatorSet.GenEmptyObj(out InspRegion);

            try
            {
                if (Angle.Length <= 0)
                {
                    Status = true;
                    return Status;
                }

                HObject LoadRegion = Recipe.Param.UsedRegionList[SelectRegionIndex].Region;

                HTuple hv_HomMat2D;
                HObject Region;
                HOperatorSet.GenEmptyObj(out Region);

                HTuple RegionCount;
                HOperatorSet.CountObj(LoadRegion, out RegionCount);
                //HOperatorSet.SortRegion(LoadRegion, out LoadRegion, "character", "true", "row");

                HTuple hv_RefRow, hv_RefColumn, hv_ModelRegionArea;
                HOperatorSet.AreaCenter(LoadRegion, out hv_ModelRegionArea, out hv_RefRow, out hv_RefColumn);

                HTuple Pat_A, Pat_R, Pat_C;
                HOperatorSet.AreaCenter(PatternRegion, out Pat_A, out Pat_R, out Pat_C);

                for (int i = 0; i < (int)((new HTuple(Angle.TupleLength()))); i++)
                {

                    HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
                    //HOperatorSet.HomMat2dTranslate(hv_HomMat2D, -Recipe.Param.SegParam.GoldenCenterY / Recipe.Param.AdvParam.MatchSacleSize, -Recipe.Param.SegParam.GoldenCenterX / Recipe.Param.AdvParam.MatchSacleSize, out hv_HomMat2D);
                    HOperatorSet.HomMat2dTranslate(hv_HomMat2D, -hv_RefRow[0].D, -hv_RefColumn[0].D, out hv_HomMat2D);
                    HOperatorSet.HomMat2dRotate(hv_HomMat2D, Angle.TupleSelect(i), 0, 0, out hv_HomMat2D);



                    HTuple cosA, sinA;
                    HOperatorSet.TupleCos(Angle, out cosA);
                    HOperatorSet.TupleSin(Angle, out sinA);
                    HTuple HX = Recipe.Param.UsedRegionList[SelectRegionIndex].HotspotX * cosA[i].D + Recipe.Param.UsedRegionList[SelectRegionIndex].HotspotY * sinA[i].D;
                    HTuple HY = -Recipe.Param.UsedRegionList[SelectRegionIndex].HotspotX * sinA[i].D + Recipe.Param.UsedRegionList[SelectRegionIndex].HotspotY * cosA[i].D;



                    HTuple ShiftRow = Pat_R.TupleSelect(i) - HY;
                    HTuple ShiftColumn = Pat_C.TupleSelect(i) - HX;


                    //HTuple ShiftRow = Pat_R.TupleSelect(i) - Recipe.Param.UsedRegionList[SelectRegionIndex].HotspotY;
                    //HTuple ShiftColumn = Pat_C.TupleSelect(i) - Recipe.Param.UsedRegionList[SelectRegionIndex].HotspotX;
                    HOperatorSet.HomMat2dTranslate(hv_HomMat2D, ShiftRow, ShiftColumn, out hv_HomMat2D);

                    Region.Dispose();
                    HOperatorSet.AffineTransRegion(LoadRegion, out Region, hv_HomMat2D, "nearest_neighbor");

                    //HTuple row1, row2, column1, column2;
                    //HOperatorSet.RegionFeatures(Region, "row1", out row1);
                    //HOperatorSet.RegionFeatures(Region, "row2", out row2);
                    //HOperatorSet.RegionFeatures(Region, "column1", out column1);
                    //HOperatorSet.RegionFeatures(Region, "column2", out column2);
                    //if (!Recipe.Param.bInspOutboundary)
                    //{
                    //    if (row1 < 0 || column1 < 0 || row2 > Recipe.Param.ImageHeight || column2 > Recipe.Param.ImageWidth)
                    //        continue;
                    //}

                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(InspRegion, Region, out ExpTmpOutVar_0);
                        InspRegion.Dispose();
                        InspRegion = ExpTmpOutVar_0;
                    }
                }
                //HObject InterSection;
                HOperatorSet.Intersection(InspRegion, CellRegion, out InspRegion);
                //InspRegion = InterSection.Clone();
                //InspRegion.Dispose();
                //HOperatorSet.SortRegion(InterSection, out InspRegion, "character", "true", "row");

                //InterSection.Dispose();
            }
            catch (Exception Ex)
            {
                return Status;
            }

            Status = true;
            return Status;
        }
        
        public static bool GetSelectEditRegion(HObject PatternRegion,HTuple Row, HTuple Column, HTuple Angle, HTuple Score, InductanceInspRole Recipe, int SelectRegionIndex, HObject CellRegion, out HObject InspRegion)
        {
            bool Status = false;
            HOperatorSet.GenEmptyObj(out InspRegion);

            try
            {
                if (Angle.Length <= 0)
                {
                    Status = true;
                    return Status;
                }

                HObject LoadRegion = Recipe.Param.EditRegionList[SelectRegionIndex].Region;

                HTuple hv_HomMat2D;
                HObject Region;
                HOperatorSet.GenEmptyObj(out Region);

                HTuple RegionCount;
                HOperatorSet.CountObj(LoadRegion, out RegionCount);
                //HOperatorSet.SortRegion(LoadRegion, out LoadRegion, "character", "true", "row");

                HTuple hv_RefRow, hv_RefColumn, hv_ModelRegionArea;
                HOperatorSet.AreaCenter(LoadRegion, out hv_ModelRegionArea, out hv_RefRow, out hv_RefColumn);

                HTuple Pat_A, Pat_R, Pat_C;
                HOperatorSet.AreaCenter(PatternRegion, out Pat_A, out Pat_R, out Pat_C);

                HTuple cosA, sinA;
                HOperatorSet.TupleCos(Angle, out cosA);
                HOperatorSet.TupleSin(Angle, out sinA);

                for (int i = 0; i < (int)((new HTuple(Angle.TupleLength()))); i++)
                {
                    HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
                    HOperatorSet.HomMat2dTranslate(hv_HomMat2D, -hv_RefRow[0].D, -hv_RefColumn[0].D, out hv_HomMat2D);
                    HOperatorSet.HomMat2dRotate(hv_HomMat2D, Angle.TupleSelect(i), 0, 0, out hv_HomMat2D);
                    
                    HTuple HX = Recipe.Param.EditRegionList[SelectRegionIndex].HotspotX * cosA[i] + Recipe.Param.EditRegionList[SelectRegionIndex].HotspotY * sinA[i];
                    HTuple HY = -Recipe.Param.EditRegionList[SelectRegionIndex].HotspotX * sinA[i] + Recipe.Param.EditRegionList[SelectRegionIndex].HotspotY * cosA[i];
                    
                    HTuple ShiftRow = Pat_R.TupleSelect(i) - HY;
                    HTuple ShiftColumn = Pat_C.TupleSelect(i) - HX;

                    HOperatorSet.HomMat2dTranslate(hv_HomMat2D, ShiftRow, ShiftColumn, out hv_HomMat2D);

                    Region.Dispose();
                    HOperatorSet.AffineTransRegion(LoadRegion, out Region, hv_HomMat2D, "nearest_neighbor");
                    
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(InspRegion, Region, out ExpTmpOutVar_0);
                        InspRegion.Dispose();
                        InspRegion = ExpTmpOutVar_0;
                    }
                }
                HOperatorSet.Intersection(InspRegion, CellRegion, out InspRegion);
            }
            catch (Exception Ex)
            {
                return Status;
            }

            Status = true;
            return Status;
        }

        public void SetPatternRegion(HObject Region)
        {
            PatternRegion = Region.Clone();
        }
        public static void Rotate(double x, double y, double Angle, out double NewX, out double NewY)
        {
            NewX = (x * Math.Cos(Angle)) + (y * Math.Sin(Angle));
            NewY = -(x * Math.Sin(Angle)) + (y * Math.Cos(Angle));
        }

        public static PointD RotateFromCenter(double OrgX, double OrgY, double CenterX, double CenterY, double Angle)
        {
            double VecX = 0, VecY = 0;
            Rotate(OrgX - CenterX,
                     OrgY - CenterY,
                     Angle, out VecX, out VecY);
            PointD Res = new PointD(VecX + CenterX, VecY + CenterY);
            return Res;
        }

        public class PointD
        {
            public double X = 0.0;
            public double Y = 0.0;

            public PointD(double pmX, double pmY)
            {
                X = pmX;
                Y = pmY;
            }
        }

        public static HTuple GetMetric(InductanceInspRole Recipe)
        {
            switch (Recipe.Param.AdvParam.Metric)
            {
                case 0:
                    return "ignore_color_polarity";
                case 1:
                    return "ignore_global_polarity";
                case 2:
                    return "ignore_local_polarity";
                case 3:
                    return "use_polarity";
                default:
                    return "use_polarity";
            }
        }

        public static HTuple GetOptimization(InductanceInspRole Recipe)
        {
            switch (Recipe.Param.AdvParam.Optimization)
            {
                case 0:
                    return "auto";
                case 1:
                    return "no_pregeneration";
                case 2:
                    return "none";
                case 3:
                    return new HTuple("point_reduction_high").TupleConcat("no_pregeneration");
                //return "point_reduction_high";
                case 4:
                    return new HTuple("point_reduction_low").TupleConcat("no_pregeneration");
                //return "point_reduction_low";
                case 5:
                    return new HTuple("point_reduction_medium").TupleConcat("no_pregeneration");
                //return "point_reduction_medium";
                case 6:
                    return "pregeneration";
                default:
                    return "auto";
            }
        }

        public static HTuple GetSubpixel(InductanceInspRole Recipe)
        {
            switch (Recipe.Param.AdvParam.SubPixel)
            {
                case 0:
                    return "none";
                case 1:
                    return "interpolation";
                case 2:
                    return "least_squares";
                case 4:
                    return "least_squares_high";
                case 5:
                    return "least_squares_very_high";
                default:
                    return "none";
            }
        }

        public static HTuple GetAngleStep(InductanceInspRole Recipe)
        {
            if (Recipe.Param.AdvParam.AngleStepAuto)
            {
                return "auto";
            }
            else
            {
                return Recipe.Param.AdvParam.AngleStep;
            }
        }

        public static HTuple GetContrast(InductanceInspRole Recipe)
        {
            if (Recipe.Param.AdvParam.ContrastAuto)
            {
                return "auto";
            }
            else
            {
                HTuple Contrast = ((new HTuple(Recipe.Param.AdvParam.ContrastSmall)).TupleConcat(Recipe.Param.AdvParam.ContrastLarge)).TupleConcat(Recipe.Param.AdvParam.MinObjSize);
                return Contrast;
            }
        }

        public static HTuple GetMinContrast(InductanceInspRole Recipe)
        {
            if (Recipe.Param.AdvParam.MinContrastAuto)
            {
                return "auto";
            }
            else
            {
                return Recipe.Param.AdvParam.MinContrast;
            }
        }

        public string GetImgFmt(int Index)
        {
            string Res = "tiff";
            switch (Index)
            {
                case 0:
                    {
                        Res = "tiff";
                        return Res;
                    }

                case 1:
                    {
                        Res = "bmp";
                        return Res;
                    }
                case 2:
                    {
                        Res = "jpg";
                        return Res;
                    }
                default:
                    return Res;
            }
        }
        public static void SelectDefectRegionsOperation(HObject DefectRegions, bool AreaEnabled, bool WidthEnabled, bool HeightEnabled, int AreaMin, int WMin, int HMin, int AreaMax, int WMax, int HMax, enuOperation AOP, enuOperation WOP, enuOperation HOP, double Resolution, out HObject SelectedDefectRegions)
        {
            HOperatorSet.SetSystem("tsp_store_empty_region", "false");
            HOperatorSet.GenEmptyRegion(out SelectedDefectRegions);
            HObject AreaSelect, WSelect, HSelect;
            try
            {
                #region All And OR
                if (AOP == enuOperation.and && HOP == enuOperation.and && WOP == enuOperation.and)
                {
                    HTuple Max, Min;
                    HTuple Feature = GetFeature(AreaEnabled, WidthEnabled, HeightEnabled, AreaMin, WMin, HMin, AreaMax, WMax, HMax, Resolution, out Min, out Max);
                    if (Feature.Length > 0)
                        HOperatorSet.SelectShape(DefectRegions, out SelectedDefectRegions, Feature, "and", Min, Max);
                    else
                        SelectedDefectRegions = DefectRegions.Clone();
                    HOperatorSet.SetSystem("tsp_store_empty_region", "true");
                    return;
                }
                else if (AOP == enuOperation.or && HOP == enuOperation.or && WOP == enuOperation.or)
                {
                    HTuple Max, Min;
                    HTuple Feature = GetFeature(AreaEnabled, WidthEnabled, HeightEnabled, AreaMin, WMin, HMin, AreaMax, WMax, HMax, Resolution, out Min, out Max);
                    if (Feature.Length > 0)
                        HOperatorSet.SelectShape(DefectRegions, out SelectedDefectRegions, Feature, "or", Min, Max);
                    else
                        SelectedDefectRegions = DefectRegions.Clone();
                    HOperatorSet.SetSystem("tsp_store_empty_region", "true");
                    return;
                }
                #endregion

                #region GetSelectRegion
                if (AreaEnabled)
                    HOperatorSet.SelectShape(DefectRegions, out AreaSelect, (new HTuple("area")), "and", AreaMin / (Resolution * Resolution), AreaMax / (Resolution * Resolution));
                else
                    AreaSelect = DefectRegions.Clone();


                if (WidthEnabled)
                    HOperatorSet.SelectShape(DefectRegions, out WSelect, (new HTuple("width")), "and", WMin / (Resolution), WMax / (Resolution));
                else
                    WSelect = DefectRegions.Clone();


                if (HeightEnabled)
                    HOperatorSet.SelectShape(DefectRegions, out HSelect, (new HTuple("height")), "and", HMin / (Resolution), HMax / (Resolution));
                else
                    HSelect = DefectRegions.Clone();
                #endregion

                #region Conditaion
                {
                    HObject TmpRegions;
                    HOperatorSet.GenEmptyRegion(out TmpRegions);


                    if (AOP == enuOperation.and && HOP == enuOperation.and && WOP == enuOperation.or)
                    {
                        HOperatorSet.Intersection(AreaSelect, HSelect, out TmpRegions);
                        HOperatorSet.Union2(TmpRegions, WSelect, out SelectedDefectRegions);
                    }
                    else if (AOP == enuOperation.and && HOP == enuOperation.or && WOP == enuOperation.and)
                    {
                        HOperatorSet.Intersection(AreaSelect, WSelect, out TmpRegions);
                        HOperatorSet.Union2(TmpRegions, HSelect, out SelectedDefectRegions);
                    }
                    else if (AOP == enuOperation.or && HOP == enuOperation.and && WOP == enuOperation.and)
                    {
                        HOperatorSet.Intersection(HSelect, WSelect, out TmpRegions);
                        HOperatorSet.Union2(TmpRegions, AreaSelect, out SelectedDefectRegions);
                    }
                    else if (AOP == enuOperation.and && HOP == enuOperation.or && WOP == enuOperation.or)
                    {
                        HOperatorSet.Union2(HSelect, WSelect, out TmpRegions);
                        HOperatorSet.Intersection(TmpRegions, AreaSelect, out SelectedDefectRegions);
                    }
                    else if (AOP == enuOperation.or && HOP == enuOperation.and && WOP == enuOperation.or)
                    {
                        HOperatorSet.Union2(AreaSelect, WSelect, out TmpRegions);
                        HOperatorSet.Intersection(TmpRegions, HSelect, out SelectedDefectRegions);
                    }
                    else if (AOP == enuOperation.or && HOP == enuOperation.or && WOP == enuOperation.and)
                    {
                        HOperatorSet.Union2(AreaSelect, HSelect, out TmpRegions);
                        HOperatorSet.Intersection(TmpRegions, WSelect, out SelectedDefectRegions);
                    }


                    TmpRegions.Dispose();
                }
                #endregion
            }
            catch (Exception Ex)
            {

            }
            HOperatorSet.SetSystem("tsp_store_empty_region", "true");

        }

        public static HTuple GetFeature(bool AEnabled, bool WEnabled, bool HEnabled, int AreaMin, int WMin, int HMin, int AreaMax, int WMax, int HMax, double Resolution, out HTuple Min, out HTuple Max)
        {
            HTuple Res = new HTuple();
            Min = new HTuple();
            Max = new HTuple();
            try
            {
                if (AEnabled)
                {
                    Res = Res.TupleConcat("area");
                    Min = Min.TupleConcat(AreaMin / (Resolution * Resolution));
                    Max = Max.TupleConcat(AreaMax / (Resolution * Resolution));
                }
                if (WEnabled)
                {
                    Res = Res.TupleConcat("width");
                    Min = Min.TupleConcat(WMin / (Resolution));
                    Max = Max.TupleConcat(WMax / (Resolution));
                }
                if (HEnabled)
                {
                    Res = Res.TupleConcat("height");
                    Min = Min.TupleConcat(HMin / (Resolution));
                    Max = Max.TupleConcat(HMax / (Resolution));
                }
            }
            catch
            {

            }

            return Res;
        }

        public static void SelectDefectRegions(HObject DefectRegions, bool AreaEnabled, bool WidthEnabled, bool HeightEnabled, int AreaMin, int WMin, int HMin, int AreaMax, int WMax, int HMax, double Resolution, out HObject SelectedDefectRegions)
        {
            HOperatorSet.SetSystem("tsp_store_empty_region", "false");
            HOperatorSet.GenEmptyRegion(out SelectedDefectRegions);
            HObject AreaSelect, WSelect, HSelect;

            if (AreaEnabled)
                HOperatorSet.SelectShape(DefectRegions, out AreaSelect, (new HTuple("area")), "and", AreaMin / (Resolution * Resolution), AreaMax / (Resolution * Resolution));
            else
                AreaSelect = DefectRegions.Clone();


            if (WidthEnabled)
                HOperatorSet.SelectShape(DefectRegions, out WSelect, (new HTuple("width")), "and", WMin / (Resolution), WMax / (Resolution));
            else
                WSelect = DefectRegions.Clone();


            if (HeightEnabled)
                HOperatorSet.SelectShape(DefectRegions, out HSelect, (new HTuple("height")), "and", HMin / (Resolution), HMax / (Resolution));
            else
                HSelect = DefectRegions.Clone();

            {
                HObject TmpRegions;
                HOperatorSet.GenEmptyRegion(out TmpRegions);
                HOperatorSet.Intersection(AreaSelect, WSelect, out TmpRegions);
                HOperatorSet.Intersection(TmpRegions, HSelect, out SelectedDefectRegions);
            }
            HOperatorSet.SetSystem("tsp_store_empty_region", "true");

        }

        public void set_parameter(InductanceInspRole role)
        {
            methRole = role;
        }

        public static void PreMatchMask(HObject Image, out HObject MatchImage, int ThValueMax, int ThValueMin, InductanceInspRole Recipe)
        {
            HObject HistoEqImg;
            HOperatorSet.GenEmptyObj(out MatchImage);

            HObject blackRegions, WhiteRegions;
            if (Recipe.Param.SegParam.MaskHistoeq)
                HOperatorSet.EquHistoImage(Image, out HistoEqImg);
            else
                HistoEqImg = Image.Clone();

            HOperatorSet.Threshold(HistoEqImg, out blackRegions, ThValueMin, ThValueMax);

            //HOperatorSet.Connection(blackRegions, out blackRegions);
            //HOperatorSet.SelectShape(blackRegions, out blackRegions, "area", "and", 100, 999999999);
            //HOperatorSet.Union1(blackRegions, out blackRegions);



            HOperatorSet.OpeningRectangle1(blackRegions, out blackRegions, Recipe.Param.SegParam.OpeningWidth, Recipe.Param.SegParam.OpeningHeight);
            HOperatorSet.ClosingRectangle1(blackRegions, out blackRegions, Recipe.Param.SegParam.ClosingingWidth, Recipe.Param.SegParam.ClosingingHeight);
            HOperatorSet.Complement(blackRegions, out WhiteRegions);
            HOperatorSet.ReduceDomain(Image, WhiteRegions, out MatchImage);

            HOperatorSet.MeanImage(MatchImage, out MatchImage, 2, 2);

            HistoEqImg.Dispose();
            Image.Dispose();
            blackRegions.Dispose();
            WhiteRegions.Dispose();
        }

        /// <summary>
        /// 二值化 (圖像教導)
        /// </summary>
        /// <param name="SrcImg"></param>
        /// <param name="Recipe"></param>
        /// <returns></returns>
        public static HObject MatchTH(HObject SrcImg, InductanceInspRole Recipe) // (20200130) Jeff Revised!
        {
            HObject MatchTH;
            HOperatorSet.GenEmptyObj(out MatchTH);
            MatchTH = clsStaticTool.GetChannelImage(SrcImg, (enuBand)Recipe.Param.SegParam.BandIndex);
            
            HTuple W, H;
            HOperatorSet.GetImageSize(MatchTH, out W, out H);

            if (Recipe.Param.SegParam.TeachMask)
                PreMatchMask(MatchTH, out MatchTH, Recipe.Param.SegParam.MaskThValueMax, Recipe.Param.SegParam.MaskThValueMin, Recipe);
            
            if (Recipe.Param.SegParam.ThEnabled) // 二值化
            {
                HObject tmp;
                HOperatorSet.Threshold(MatchTH, out tmp, Recipe.Param.SegParam.THMin, Recipe.Param.SegParam.ThMax);  

                HObject Closing, Opening;
                HOperatorSet.ClosingRectangle1(tmp, out Closing, Recipe.Param.AdvParam.ClosingNum, Recipe.Param.AdvParam.ClosingNum);
                HOperatorSet.OpeningRectangle1(Closing, out Opening, Recipe.Param.AdvParam.OpeningNum, Recipe.Param.AdvParam.OpeningNum);

                HObject ProcessRegion; // (20200130) Jeff Revised!
                if (Recipe.Param.AdvParam.bFillupEnabled) // 填滿
                {
                    HTuple TransType = Recipe.Param.AdvParam.FillupType.ToString();
                    HOperatorSet.FillUpShape(Opening, out ProcessRegion, TransType, Recipe.Param.AdvParam.FillupMin, Recipe.Param.AdvParam.FillupMax);
                }
                else
                    ProcessRegion = Opening;

                MatchTH.Dispose();
                HOperatorSet.RegionToBin(ProcessRegion, out MatchTH, 255, 0, W, H);
                
                tmp.Dispose();
                Closing.Dispose();
                Opening.Dispose();
                ProcessRegion.Dispose();
            }

            return MatchTH;
        }

        public void GetRotateImageList(HTuple AngleArray, InductanceInspRole Recipe, List<HObject> Input_ImgList, out List<HObject> RotateImageList)
        {
            HTuple AngleMean, Deg;
            RotateImageList = new List<HObject>();
            HOperatorSet.TupleMean(AngleArray, out AngleMean);
            HOperatorSet.TupleDeg(AngleMean, out Deg);

            for (int i = 0; i < Input_ImgList.Count; i++)
            {
                HObject Img;
                HOperatorSet.GenEmptyObj(out Img);
                HOperatorSet.RotateImage(Input_ImgList[i], out Img, -Deg, "nearest_neighbor");
                RotateImageList.Add(Img);
            }
        }

        public void GetRotateInfo(HTuple AngleArray, InductanceInspRole Recipe, List<HObject> Input_ImgList, HObject PatternRegions, out HObject RotatePatternregions)
        {
            HTuple AngleMean, Deg;
            HOperatorSet.GenEmptyObj(out RotatePatternregions);
            HOperatorSet.TupleMean(AngleArray, out AngleMean);
            HOperatorSet.TupleDeg(AngleMean, out Deg);

            HTuple Count;
            HOperatorSet.CountObj(PatternRegions, out Count);
            for (int i = 1; i <= Count; i++)
            {
                HObject SelectRegion;

                HOperatorSet.SelectObj(PatternRegions, out SelectRegion, i);

                HTuple Area, CenterY, CenterX;
                HOperatorSet.AreaCenter(SelectRegion, out Area, out CenterY, out CenterX);

                HTuple ImageCenterX, ImageCenterY, W, H;
                HOperatorSet.GetImageSize(Input_ImgList[Recipe.Param.SegParam.SegImgIndex], out W, out H);
                ImageCenterX = W / 2;
                ImageCenterY = H / 2;

                PointD PatternRotatePos = RotateFromCenter(CenterX,
                                                           CenterY,
                                                           ImageCenterX, ImageCenterY, -AngleMean);

                double X = PatternRotatePos.X;
                double Y = PatternRotatePos.Y;

                HObject CenterRegion;
                HOperatorSet.GenCircle(out CenterRegion, Y, X, 1);
                {
                    HObject TmpRegions;
                    HOperatorSet.ConcatObj(CenterRegion, RotatePatternregions, out TmpRegions);
                    RotatePatternregions.Dispose();
                    RotatePatternregions = TmpRegions;
                }
            }
        }

        public bool TeachGolden(HObject SrcImage, InductanceInspRole Recipe, out HObject PatternRegions, out HTuple Angle, out HTuple hv_Row, out HTuple hv_Column)
        {
            bool Res = false;
            #region 變數宣告

            HObject ho_TransContours, ho_ModelContours;
            hv_Row = 0;
            hv_Column = 0;
            HTuple hv_HomMat2D = null;
            HTuple hv_Angle = null, hv_Score = null, hv_I = null;
            HObject ho_Region = null;
            HObject SrcImg_R, SrcImg_G, SrcImg_B;
            Angle = null;
            HObject EmptyRegion;
            HTuple IsEmpty;
            HOperatorSet.GenEmptyRegion(out EmptyRegion);

            HOperatorSet.GenEmptyObj(out ho_ModelContours);
            HOperatorSet.GenEmptyObj(out PatternRegions);
            HOperatorSet.GenEmptyObj(out ho_TransContours);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out SrcImg_R);
            HOperatorSet.GenEmptyObj(out SrcImg_G);
            HOperatorSet.GenEmptyObj(out SrcImg_B);

            #endregion

            if (Recipe.ModelID == null && !Recipe.Param.SegParam.IsNCCMode)
                return Res;

            if (Recipe.ModelID_NCC == null && Recipe.Param.SegParam.IsNCCMode)
                return Res;

            //if (Recipe.ModelID_NCC.Length == 0 && Recipe.Param.SegParam.IsNCCMode)
            //    return Res;

            HObject InputImg;

            if (Recipe.Param.SegParam.ThEnabled)
                InputImg = MatchTH(SrcImage, Recipe);
            else
            {
                HObject GrayImg;
                HOperatorSet.Rgb1ToGray(SrcImage, out GrayImg);
                InputImg = GrayImg.Clone();
                GrayImg.Dispose();
            }

            ho_ModelContours.Dispose();
            if (!Recipe.Param.SegParam.IsNCCMode)
                HOperatorSet.GetShapeModelContours(out ho_ModelContours, Recipe.ModelID, 1);

            if (Recipe.Param.SegParam.IsNCCMode)
            {
                HOperatorSet.FindNccModel(InputImg,
                                          Recipe.ModelID_NCC,
                                          new HTuple(Recipe.Param.AdvParam.AngleStart).TupleRad(),
                                          new HTuple(Recipe.Param.AdvParam.AngleExtent).TupleRad(),
                                          0.1,
                                          1,
                                          Recipe.Param.AdvParam.Overlap,
                                          "false",
                                          Recipe.Param.AdvParam.NumLevels,
                                          out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
            }
            else
            {
                HOperatorSet.FindShapeModel(InputImg,
                                            Recipe.ModelID,
                                            new HTuple(Recipe.Param.AdvParam.AngleStart).TupleRad(),
                                            new HTuple(Recipe.Param.AdvParam.AngleExtent).TupleRad(),
                                            0.1,
                                            1,
                                            Recipe.Param.AdvParam.Overlap,
                                            InductanceInsp.GetSubpixel(Recipe),
                                            Recipe.Param.AdvParam.NumLevels,
                                            Recipe.Param.AdvParam.Greediness,
                                            out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
            }

            HObject Pattern_Rect;
            HOperatorSet.GenRectangle1(out Pattern_Rect, Recipe.Param.SegParam.Patternrow1, Recipe.Param.SegParam.Patterncolumn1, Recipe.Param.SegParam.Patternrow2, Recipe.Param.SegParam.Patterncolumn2);

            HOperatorSet.GenEmptyRegion(out PatternRegions);
            Angle = hv_Angle;
            HTuple hv_RefRow, hv_RefColumn, hv_ModelRegionArea;
            //Matching 02: Get the reference position
            HOperatorSet.AreaCenter(Pattern_Rect, out hv_ModelRegionArea, out hv_RefRow, out hv_RefColumn);

            for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Score.TupleLength())) - 1); hv_I = (int)hv_I + 1)
            {
                if (Recipe.Param.SegParam.IsNCCMode)
                {
                    //ho_TransContours.Dispose();
                    //HOperatorSet.GenCrossContourXld(out ho_TransContours, hv_Row.TupleSelect(hv_I), hv_Column.TupleSelect(hv_I), 20, hv_Angle);
                    HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
                    HOperatorSet.HomMat2dScale(hv_HomMat2D, 1.0 / Recipe.Param.AdvParam.MatchSacleSize, 1.0 / Recipe.Param.AdvParam.MatchSacleSize, 0, 0, out hv_HomMat2D);
                    HOperatorSet.HomMat2dTranslate(hv_HomMat2D, -hv_RefRow / Recipe.Param.AdvParam.MatchSacleSize, -hv_RefColumn / Recipe.Param.AdvParam.MatchSacleSize, out hv_HomMat2D);
                    HOperatorSet.HomMat2dRotate(hv_HomMat2D, hv_Angle.TupleSelect(hv_I), 0, 0, out hv_HomMat2D);
                    HOperatorSet.HomMat2dTranslate(hv_HomMat2D, hv_Row.TupleSelect(hv_I) / Recipe.Param.AdvParam.MatchSacleSize, hv_Column.TupleSelect(hv_I) / Recipe.Param.AdvParam.MatchSacleSize, out hv_HomMat2D);
                    //Matching 02: Display the aligned model region

                    ho_Region.Dispose();
                    HOperatorSet.AffineTransRegion(Pattern_Rect, out ho_Region, hv_HomMat2D, "nearest_neighbor");
                }
                else
                {
                    HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
                    HOperatorSet.HomMat2dScale(hv_HomMat2D, 1.0 / Recipe.Param.AdvParam.MatchSacleSize, 1.0 / Recipe.Param.AdvParam.MatchSacleSize, 0, 0, out hv_HomMat2D);
                    HOperatorSet.HomMat2dRotate(hv_HomMat2D, hv_Angle.TupleSelect(hv_I), 0, 0, out hv_HomMat2D);
                    HOperatorSet.HomMat2dTranslate(hv_HomMat2D, hv_Row.TupleSelect(hv_I) / Recipe.Param.AdvParam.MatchSacleSize, hv_Column.TupleSelect(hv_I) / Recipe.Param.AdvParam.MatchSacleSize, out hv_HomMat2D);


                    ho_TransContours.Dispose();
                    HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_TransContours, hv_HomMat2D);
                    ho_Region.Dispose();
                    HOperatorSet.GenRegionContourXld(ho_TransContours, out ho_Region, "filled");

                    //HTuple A;
                    //HOperatorSet.AreaCenter(ho_Region, out A, out hv_Row, out hv_Column);
                }


                HObject Un1;
                HOperatorSet.Union1(ho_Region, out Un1);
                {
                    PatternRegions = Un1.Clone();
                }

                Un1.Dispose();
            }



            #region Dispose
            EmptyRegion.Dispose();
            ho_ModelContours.Dispose();
            ho_TransContours.Dispose();
            ho_Region.Dispose();
            SrcImg_R.Dispose();
            SrcImg_G.Dispose();
            SrcImg_B.Dispose();
            #endregion
            Res = true;
            return Res;
        }

        /// <summary>
        /// Pattern對位 輸出Region
        /// </summary>
        /// <param name="SrcImage">輸入影像</param>
        /// <param name="ModelID">輸入Model</param>
        /// <param name="ModelRegion">輸入Modlle範圍</param>
        /// <param name="PatternRegions">輸出尋找之Pattern Regions</param>
        public bool PtternMatch(HObject SrcImage, InductanceInspRole Recipe, out HObject PatternRegions, out HTuple Angle)
        {
            bool Res = false;
            #region 變數宣告

            HObject ho_TransContours, ho_ModelContours;
            HTuple hv_HomMat2D = null, hv_Row = null, hv_Column = null;
            HTuple hv_Angle = null, hv_Score = null, hv_I = null;
            HObject ho_Region = null;
            Angle = null;
            HObject EmptyRegion;
            HTuple IsEmpty;
            HOperatorSet.GenEmptyRegion(out EmptyRegion);

            HOperatorSet.GenEmptyObj(out ho_ModelContours);
            HOperatorSet.GenEmptyObj(out PatternRegions);
            HOperatorSet.GenEmptyObj(out ho_TransContours);
            HOperatorSet.GenEmptyObj(out ho_Region);

            #endregion

            if (Recipe.ModelID == null && !Recipe.Param.SegParam.IsNCCMode)
                return Res;


            if (Recipe.ModelID_NCC == null && Recipe.Param.SegParam.IsNCCMode)
                return Res;

            HObject InputImg;
            HObject Pattern_Rect;
            HOperatorSet.GenRectangle1(out Pattern_Rect, Recipe.Param.SegParam.Patternrow1, Recipe.Param.SegParam.Patterncolumn1, Recipe.Param.SegParam.Patternrow2, Recipe.Param.SegParam.Patterncolumn2);

            HObject ScaleImage;

            HOperatorSet.ZoomImageFactor(SrcImage, out ScaleImage, Recipe.Param.AdvParam.MatchSacleSize, Recipe.Param.AdvParam.MatchSacleSize, "nearest_neighbor");

            if (Recipe.Param.SegParam.MatchPreProcessEnabled)
                InputImg = MatchTH(ScaleImage, Recipe);
            else
            {
                InputImg = clsStaticTool.GetChannelImage(ScaleImage, (enuBand)Recipe.Param.SegParam.BandIndex);
            }
            HOperatorSet.GrayOpeningRect(InputImg, out InputImg, Recipe.Param.AdvParam.OpeningNum, Recipe.Param.AdvParam.OpeningNum);
            if (!Recipe.Param.SegParam.IsNCCMode)
            {
                ho_ModelContours.Dispose();
                HOperatorSet.GetShapeModelContours(out ho_ModelContours, Recipe.ModelID, 1);
            }

            if (Recipe.Param.SegParam.IsNCCMode)
            {
                HOperatorSet.FindNccModel(InputImg,
                                          Recipe.ModelID_NCC,
                                          new HTuple(Recipe.Param.AdvParam.AngleStart).TupleRad(),
                                          new HTuple(Recipe.Param.AdvParam.AngleExtent).TupleRad(),
                                          Recipe.Param.SegParam.MinScore,
                                          0,
                                          Recipe.Param.AdvParam.Overlap,
                                          "false",
                                          Recipe.Param.AdvParam.NumLevels,
                                          out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
            }
            else
            {
                HOperatorSet.FindShapeModel(InputImg,
                                            Recipe.ModelID,
                                            new HTuple(Recipe.Param.AdvParam.AngleStart).TupleRad(),
                                            new HTuple(Recipe.Param.AdvParam.AngleExtent).TupleRad(),
                                            Recipe.Param.SegParam.MinScore,
                                            0,
                                            Recipe.Param.AdvParam.Overlap,
                                            InductanceInsp.GetSubpixel(Recipe),
                                            Recipe.Param.AdvParam.NumLevels,
                                            Recipe.Param.AdvParam.Greediness,
                                            out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
            }

            HOperatorSet.GenEmptyRegion(out PatternRegions);
            Angle = hv_Angle;
            HTuple hv_RefRow, hv_RefColumn, hv_ModelRegionArea;
            HOperatorSet.AreaCenter(Pattern_Rect, out hv_ModelRegionArea, out hv_RefRow, out hv_RefColumn);

            for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Score.TupleLength())) - 1); hv_I = (int)hv_I + 1)
            {
                if (Recipe.Param.SegParam.IsNCCMode)
                {
                    //ho_TransContours.Dispose();
                    //HOperatorSet.GenCrossContourXld(out ho_TransContours, hv_Row.TupleSelect(hv_I), hv_Column.TupleSelect(hv_I), 20, hv_Angle);
                    HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
                    HOperatorSet.HomMat2dScale(hv_HomMat2D, 1.0 / Recipe.Param.AdvParam.MatchSacleSize, 1.0 / Recipe.Param.AdvParam.MatchSacleSize, 0, 0, out hv_HomMat2D);
                    HOperatorSet.HomMat2dTranslate(hv_HomMat2D, -hv_RefRow / Recipe.Param.AdvParam.MatchSacleSize, -hv_RefColumn / Recipe.Param.AdvParam.MatchSacleSize, out hv_HomMat2D);
                    HOperatorSet.HomMat2dRotate(hv_HomMat2D, hv_Angle.TupleSelect(hv_I), 0, 0, out hv_HomMat2D);
                    HOperatorSet.HomMat2dTranslate(hv_HomMat2D, hv_Row.TupleSelect(hv_I) / Recipe.Param.AdvParam.MatchSacleSize, hv_Column.TupleSelect(hv_I) / Recipe.Param.AdvParam.MatchSacleSize, out hv_HomMat2D);
                    ho_Region.Dispose();
                    HOperatorSet.AffineTransRegion(Pattern_Rect, out ho_Region, hv_HomMat2D, "nearest_neighbor");
                }
                else
                {
                    HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
                    HOperatorSet.HomMat2dScale(hv_HomMat2D, 1.0 / Recipe.Param.AdvParam.MatchSacleSize, 1.0 / Recipe.Param.AdvParam.MatchSacleSize, 0, 0, out hv_HomMat2D);
                    HOperatorSet.HomMat2dRotate(hv_HomMat2D, hv_Angle.TupleSelect(hv_I), 0, 0, out hv_HomMat2D);
                    HOperatorSet.HomMat2dTranslate(hv_HomMat2D, hv_Row.TupleSelect(hv_I) / Recipe.Param.AdvParam.MatchSacleSize, hv_Column.TupleSelect(hv_I) / Recipe.Param.AdvParam.MatchSacleSize, out hv_HomMat2D);
                    ho_TransContours.Dispose();
                    HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_TransContours, hv_HomMat2D);
                    ho_Region.Dispose();
                    HOperatorSet.GenRegionContourXld(ho_TransContours, out ho_Region, "filled");
                }
                HObject Un1;
                HOperatorSet.Union1(ho_Region, out Un1);
                {
                    HObject ExpTmpOutVar_0;

                    HOperatorSet.TestEqualRegion(PatternRegions, EmptyRegion, out IsEmpty);
                    if (IsEmpty)
                        PatternRegions = Un1.Clone();
                    else
                    {
                        HOperatorSet.ConcatObj(PatternRegions, Un1, out ExpTmpOutVar_0);
                        PatternRegions.Dispose();
                        PatternRegions = ExpTmpOutVar_0;
                    }
                }
                Un1.Dispose();
            }

            #region Dispose

            Pattern_Rect.Dispose();
            //InputImg.Dispose();
            EmptyRegion.Dispose();
            ho_ModelContours.Dispose();
            ho_TransContours.Dispose();
            ho_Region.Dispose();

            #endregion
            Res = true;
            return Res;
        }

        public bool TestSegRegions(List<Point> PatternCenter, InductanceInspRole Recipe, HObject CropSrcImg, out HObject SegRegions)
        {
            bool Res = false;
            SegRegions = null;
            HOperatorSet.GenEmptyObj(out SegRegions);

            int hv_Index;
            HTuple step_val87 = 1;

            for (hv_Index = 0; hv_Index < PatternCenter.Count; hv_Index++)
            {

                HTuple CenterY = PatternCenter[hv_Index].Y;
                HTuple CenterX = PatternCenter[hv_Index].X;

                double InspCenterX = CenterX - Recipe.Param.SegParam.HotspotX;
                double InspCenterY = CenterY - Recipe.Param.SegParam.HotspotY;

                double StartY = InspCenterY - Recipe.Param.SegParam.InspRectHeight / 2;
                double StartX = InspCenterX - Recipe.Param.SegParam.InspRectWidth / 2;
                double EndY = InspCenterY + Recipe.Param.SegParam.InspRectHeight / 2;
                double EndX = InspCenterX + Recipe.Param.SegParam.InspRectWidth / 2;
                HObject InspRect;
                HOperatorSet.GenRectangle1(out InspRect, StartY, StartX, EndY, EndX);
                HTuple W, H;
                HOperatorSet.GetImageSize(CropSrcImg, out W, out H);
                if (StartY < 0 || StartX < 0 || EndX > W || EndY > H)
                    continue;
                {
                    HObject TempReg;
                    HOperatorSet.ConcatObj(SegRegions, InspRect, out TempReg);
                    SegRegions.Dispose();
                    SegRegions = TempReg;
                }
            }
            Res = true;
            return Res;
        }

        public class RDefectParam
        {
            public HObject InspImage;
            public InductanceInspRole Recipe;
            public HObject InspRegion;
            public HObject RDefect_Dark;
            public HObject RDefect_Bright;

            public RDefectParam(HObject pmInspImage, InductanceInspRole pmRecipe, HObject pmInspRegion, HObject pmRDefect_Dark, HObject pmRDefect_Bright)
            {
                this.InspImage = pmInspImage;
                this.Recipe = pmRecipe;
                this.InspRegion = pmInspRegion;
                this.RDefect_Dark = pmRDefect_Dark;
                this.RDefect_Bright = pmRDefect_Bright;
            }
        }

        public static HObject RDefectInspTask(object Obj)
        {
            HObject RDefectRegions;
            HOperatorSet.GenEmptyObj(out RDefectRegions);

            #region Convert Obj

            HObject InspImage = ((RDefectParam)Obj).InspImage;
            InductanceInspRole Recipe = ((RDefectParam)Obj).Recipe;
            HObject InspRegion = ((RDefectParam)Obj).InspRegion;
            HObject RDefect_Dark = ((RDefectParam)Obj).RDefect_Dark;
            HObject RDefect_Bright = ((RDefectParam)Obj).RDefect_Bright;

            #endregion

            if (!Recipe.Param.RisistAOIParam.RDefectParam.Enabled)
            {
                return RDefectRegions;
            }

            HObject Bright_Image, Dark_Image;
            HOperatorSet.GenEmptyObj(out Bright_Image);
            HOperatorSet.GenEmptyObj(out Dark_Image);
            HObject RegionExpand_Dark, RegionExpand_Bright;
            HObject RegionSkip_Dark, RegionSkip_Bright;

            HObject Union_B, Union_D;

            HOperatorSet.Union1(RDefect_Dark, out Union_D);
            HOperatorSet.Union1(RDefect_Bright, out Union_B);

            HOperatorSet.DilationRectangle1(Union_D, out RegionExpand_Dark, Recipe.Param.RisistAOIParam.RDefectParam.ExtWidth, Recipe.Param.RisistAOIParam.RDefectParam.ExtHeight);
            HOperatorSet.ErosionRectangle1(RegionExpand_Dark, out RegionSkip_Dark, Recipe.Param.RisistAOIParam.RDefectParam.SkipWidth, Recipe.Param.RisistAOIParam.RDefectParam.SkipHeight);

            HOperatorSet.DilationRectangle1(Union_B, out RegionExpand_Bright, Recipe.Param.RisistAOIParam.RDefectParam.ExtWidth, Recipe.Param.RisistAOIParam.RDefectParam.ExtHeight);
            HOperatorSet.ErosionRectangle1(RegionExpand_Bright, out RegionSkip_Bright, Recipe.Param.RisistAOIParam.RDefectParam.SkipWidth, Recipe.Param.RisistAOIParam.RDefectParam.SkipHeight);
            try
            {
                HOperatorSet.ReduceDomain(InspImage, RegionSkip_Bright, out Bright_Image);
                HOperatorSet.ReduceDomain(InspImage, RegionSkip_Dark, out Dark_Image);

                HObject Dark_DefectRegions, Bright_DefectRegions;
                HOperatorSet.Threshold(Dark_Image, out Dark_DefectRegions, Recipe.Param.RisistAOIParam.RDefectParam.Dark_ThMin, Recipe.Param.RisistAOIParam.RDefectParam.Dark_ThMax);//65  100

                HOperatorSet.Threshold(Bright_Image, out Bright_DefectRegions, Recipe.Param.RisistAOIParam.RDefectParam.Bright_ThMin, Recipe.Param.RisistAOIParam.RDefectParam.Bright_ThMax);//110 140


                HOperatorSet.Union2(Dark_DefectRegions, Bright_DefectRegions, out RDefectRegions);

                HOperatorSet.ClosingRectangle1(RDefectRegions, out RDefectRegions, 10, 10);
                HObject DefectReg;
                HOperatorSet.Connection(RDefectRegions, out DefectReg);

                double MaxW = 2500000 * Locate_Method_FS.pixel_resolution_;
                double MaxA = 2500000 * (Locate_Method_FS.pixel_resolution_ * Locate_Method_FS.pixel_resolution_);
                SelectDefectRegions(DefectReg,
                                    Recipe.Param.RisistAOIParam.RDefectParam.AreaEnabled,
                                    Recipe.Param.RisistAOIParam.RDefectParam.WidthEnabled,
                                    Recipe.Param.RisistAOIParam.RDefectParam.HeightEnabled,
                                    Recipe.Param.RisistAOIParam.RDefectParam.AreaMin,
                                    Recipe.Param.RisistAOIParam.RDefectParam.WidthMin,
                                    Recipe.Param.RisistAOIParam.RDefectParam.HeightMin,
                                    (int)MaxA, (int)MaxW, (int)MaxW, Locate_Method_FS.pixel_resolution_,
                                    out RDefectRegions);

                Dark_DefectRegions.Dispose();
                Bright_DefectRegions.Dispose();
                DefectReg.Dispose();
            }

            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
                return RDefectRegions;
            }
            Union_B.Dispose();
            Union_D.Dispose();
            Bright_Image.Dispose();
            Dark_Image.Dispose();

            return RDefectRegions;

        }

        public bool RDefectInsp(HObject InspImage, InductanceInspRole Recipe, HObject InspRegion, HObject RDefect_Dark, HObject RDefect_Bright, out HObject RDefectRegions, out HObject RDefectMapRegion)
        {
            bool Res = false;

            HOperatorSet.GenEmptyObj(out RDefectRegions);
            HOperatorSet.GenEmptyObj(out RDefectMapRegion);

            if (!Recipe.Param.RisistAOIParam.RDefectParam.Enabled)
            {
                Res = true;
                return Res;
            }

            HObject Bright_Image, Dark_Image;
            HOperatorSet.GenEmptyObj(out Bright_Image);
            HOperatorSet.GenEmptyObj(out Dark_Image);
            HObject RegionExpand_Dark, RegionExpand_Bright;

            HObject Union_B, Union_D;

            HOperatorSet.Union1(RDefect_Dark, out Union_D);
            HOperatorSet.Union1(RDefect_Bright, out Union_B);

            HOperatorSet.DilationRectangle1(Union_D, out RegionExpand_Dark, Recipe.Param.RisistAOIParam.RDefectParam.ExtWidth, Recipe.Param.RisistAOIParam.RDefectParam.ExtHeight);
            HOperatorSet.DilationRectangle1(Union_B, out RegionExpand_Bright, Recipe.Param.RisistAOIParam.RDefectParam.ExtWidth, Recipe.Param.RisistAOIParam.RDefectParam.ExtHeight);
            try
            {
                HOperatorSet.ReduceDomain(InspImage, RegionExpand_Bright, out Bright_Image);
                HOperatorSet.ReduceDomain(InspImage, RegionExpand_Dark, out Dark_Image);

                HObject Dark_DefectRegions, Bright_DefectRegions;
                HOperatorSet.Threshold(Dark_Image, out Dark_DefectRegions, Recipe.Param.RisistAOIParam.RDefectParam.Dark_ThMin, Recipe.Param.RisistAOIParam.RDefectParam.Dark_ThMax);//65  100

                HOperatorSet.Threshold(Bright_Image, out Bright_DefectRegions, Recipe.Param.RisistAOIParam.RDefectParam.Bright_ThMin, Recipe.Param.RisistAOIParam.RDefectParam.Bright_ThMax);//110 140


                HOperatorSet.Union2(Dark_DefectRegions, Bright_DefectRegions, out RDefectRegions);

                HOperatorSet.ClosingRectangle1(RDefectRegions, out RDefectRegions, 10, 10);
                HObject DefectReg;
                HOperatorSet.Connection(RDefectRegions, out DefectReg);

                double MaxW = 2500000 * Locate_Method_FS.pixel_resolution_;
                double MaxA = 2500000 * (Locate_Method_FS.pixel_resolution_ * Locate_Method_FS.pixel_resolution_);
                SelectDefectRegions(DefectReg,
                                    Recipe.Param.RisistAOIParam.RDefectParam.AreaEnabled,
                                    Recipe.Param.RisistAOIParam.RDefectParam.WidthEnabled,
                                    Recipe.Param.RisistAOIParam.RDefectParam.HeightEnabled,
                                    Recipe.Param.RisistAOIParam.RDefectParam.AreaMin,
                                    Recipe.Param.RisistAOIParam.RDefectParam.WidthMin,
                                    Recipe.Param.RisistAOIParam.RDefectParam.HeightMin,
                                    (int)MaxA, (int)MaxW, (int)MaxW, Locate_Method_FS.pixel_resolution_,
                                    out RDefectRegions);

                LocalDefectMappping(RDefectRegions, InspRegion, out RDefectMapRegion);
                Dark_DefectRegions.Dispose();
                Bright_DefectRegions.Dispose();
                DefectReg.Dispose();
            }

            catch
            {
                return Res;
            }
            Union_B.Dispose();
            Union_D.Dispose();
            Bright_Image.Dispose();
            Dark_Image.Dispose();

            Res = true;
            return Res;
        }

        public class StainParam
        {
            public HObject InspImage;
            public InductanceInspRole Recipe;
            public HObject InspRegion;
            public HObject Stain_Dark;
            public HObject Stain_Bright;

            public StainParam(HObject pmInspImage, InductanceInspRole pmRecipe, HObject pmInspRegion, HObject pmStain_Dark, HObject pmStain_Bright)
            {
                this.InspImage = pmInspImage;
                this.Recipe = pmRecipe;
                this.InspRegion = pmInspRegion;
                this.Stain_Dark = pmStain_Dark;
                this.Stain_Bright = pmStain_Bright;
            }
        }

        public static HObject StainInspTask(object Obj)
        {
            HObject DefectStain;
            HOperatorSet.GenEmptyObj(out DefectStain);

            #region Convert Obj

            HObject InspImage = ((StainParam)Obj).InspImage;
            InductanceInspRole Recipe = ((StainParam)Obj).Recipe;
            HObject InspRegion = ((StainParam)Obj).InspRegion;
            HObject Stain_Dark = ((StainParam)Obj).Stain_Dark;
            HObject Stain_Bright = ((StainParam)Obj).Stain_Bright;

            #endregion

            if (!Recipe.Param.RisistAOIParam.StainParam.Enabled)
            {
                return DefectStain;
            }
            HObject Dark_InspImg, Bright_InspImage;
            HObject Dark_DefectRegions, Bright_DefectRegions;
            HOperatorSet.GenEmptyObj(out Dark_InspImg);
            HOperatorSet.GenEmptyObj(out Bright_InspImage);
            HOperatorSet.GenEmptyObj(out Dark_DefectRegions);
            HOperatorSet.GenEmptyObj(out Bright_DefectRegions);
            try
            {
                HOperatorSet.ReduceDomain(InspImage, Stain_Dark, out Dark_InspImg);
                HOperatorSet.Threshold(Dark_InspImg, out Dark_DefectRegions, 0, Recipe.Param.RisistAOIParam.StainParam.Dark_Th);

                HOperatorSet.ReduceDomain(InspImage, Stain_Bright, out Bright_InspImage);
                HOperatorSet.Threshold(Bright_InspImage, out Bright_DefectRegions, 0, Recipe.Param.RisistAOIParam.StainParam.Bright_Th);


                HOperatorSet.Union2(Dark_DefectRegions, Bright_DefectRegions, out DefectStain);
                HObject DefectReg;
                HOperatorSet.Connection(DefectStain, out DefectReg);
                double MaxW = 2500000 * Locate_Method_FS.pixel_resolution_;
                double MaxA = 2500000 * (Locate_Method_FS.pixel_resolution_ * Locate_Method_FS.pixel_resolution_);
                SelectDefectRegions(DefectReg,
                                    Recipe.Param.RisistAOIParam.StainParam.AreaEnabled,
                                    Recipe.Param.RisistAOIParam.StainParam.WidthEnabled,
                                    Recipe.Param.RisistAOIParam.StainParam.HeightEnabled,
                                    Recipe.Param.RisistAOIParam.StainParam.AreaMin,
                                    Recipe.Param.RisistAOIParam.StainParam.WidthMin,
                                    Recipe.Param.RisistAOIParam.StainParam.HeightMin,
                                    (int)MaxA, (int)MaxW, (int)MaxW, Locate_Method_FS.pixel_resolution_,
                                    out DefectStain);
                DefectReg.Dispose();
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
                return DefectStain;
            }

            Dark_InspImg.Dispose();
            Bright_InspImage.Dispose();
            Dark_DefectRegions.Dispose();
            Bright_DefectRegions.Dispose();

            return DefectStain;
        }

        public bool StainInsp(HObject InspImage, InductanceInspRole Recipe, HObject InspRegion, HObject Stain_Dark, HObject Stain_Bright, out HObject DefectStain, out HObject StainMapRegion)
        {
            bool Res = false;
            HOperatorSet.GenEmptyObj(out DefectStain);
            HOperatorSet.GenEmptyObj(out StainMapRegion);
            if (!Recipe.Param.RisistAOIParam.StainParam.Enabled)
            {
                Res = true;
                return Res;
            }
            HObject Dark_InspImg, Bright_InspImage;
            HObject Dark_DefectRegions, Bright_DefectRegions;
            HOperatorSet.GenEmptyObj(out Dark_InspImg);
            HOperatorSet.GenEmptyObj(out Bright_InspImage);
            HOperatorSet.GenEmptyObj(out Dark_DefectRegions);
            HOperatorSet.GenEmptyObj(out Bright_DefectRegions);
            try
            {
                HOperatorSet.ReduceDomain(InspImage, Stain_Dark, out Dark_InspImg);
                HOperatorSet.Threshold(Dark_InspImg, out Dark_DefectRegions, 0, Recipe.Param.RisistAOIParam.StainParam.Dark_Th);//61

                HOperatorSet.ReduceDomain(InspImage, Stain_Bright, out Bright_InspImage);
                HOperatorSet.Threshold(Bright_InspImage, out Bright_DefectRegions, 0, Recipe.Param.RisistAOIParam.StainParam.Bright_Th);//75


                HOperatorSet.Union2(Dark_DefectRegions, Bright_DefectRegions, out DefectStain);
                HObject DefectReg;
                HOperatorSet.Connection(DefectStain, out DefectReg);
                double MaxW = 2500000 * Locate_Method_FS.pixel_resolution_;
                double MaxA = 2500000 * (Locate_Method_FS.pixel_resolution_ * Locate_Method_FS.pixel_resolution_);
                SelectDefectRegions(DefectReg,
                                    Recipe.Param.RisistAOIParam.StainParam.AreaEnabled,
                                    Recipe.Param.RisistAOIParam.StainParam.WidthEnabled,
                                    Recipe.Param.RisistAOIParam.StainParam.HeightEnabled,
                                    Recipe.Param.RisistAOIParam.StainParam.AreaMin,
                                    Recipe.Param.RisistAOIParam.StainParam.WidthMin,
                                    Recipe.Param.RisistAOIParam.StainParam.HeightMin,
                                    (int)MaxA, (int)MaxW, (int)MaxW, Locate_Method_FS.pixel_resolution_,
                                    out DefectStain);

                LocalDefectMappping(DefectStain, InspRegion, out StainMapRegion);

                DefectReg.Dispose();
            }
            catch
            {
                return Res;
            }
            Dark_InspImg.Dispose();
            Bright_InspImage.Dispose();
            Dark_DefectRegions.Dispose();
            Bright_DefectRegions.Dispose();

            Res = true;
            return Res;
        }


        public class ScratchParam
        {
            public HObject InspImage;
            public InductanceInspRole Recipe;
            public HObject InspRegion;
            public HObject ScratchRegion;
            public HObject ThinRegion;


            public ScratchParam(HObject pmInspImage, InductanceInspRole pmRecipe, HObject pmInspRegion, HObject pmScratchRegion, HObject pmThinRegion)
            {
                this.InspImage = pmInspImage;
                this.Recipe = pmRecipe;
                this.InspRegion = pmInspRegion;
                this.ScratchRegion = pmScratchRegion;
                this.ThinRegion = pmThinRegion;
            }
        }

        public static HObject ScratchInspTask(object Obj)
        {
            HObject ScratchDefectRegions;
            HOperatorSet.GenEmptyObj(out ScratchDefectRegions);

            #region Convert Obj

            HObject InspImage = ((ScratchParam)Obj).InspImage;
            InductanceInspRole Recipe = ((ScratchParam)Obj).Recipe;
            HObject InspRegion = ((ScratchParam)Obj).InspRegion;
            HObject ScratchRegion = ((ScratchParam)Obj).ScratchRegion;
            HObject ThinRegion = ((ScratchParam)Obj).ThinRegion;

            #endregion

            if (!Recipe.Param.RisistAOIParam.ScratchParam.Enabled)
            {
                return ScratchDefectRegions;
            }

            HObject InInspRegions, OutInspRegions;
            HObject InspImgOut, InspImgIn;
            try
            {

                HOperatorSet.Union1(ThinRegion, out InInspRegions);
                HOperatorSet.ReduceDomain(InspImage, InInspRegions, out InspImgIn);

                HObject DiffRegion, EX;
                HOperatorSet.Union1(ScratchRegion, out OutInspRegions);
                HOperatorSet.DilationRectangle1(InInspRegions, out EX, Recipe.Param.RisistAOIParam.ThinParam.TopHatEdgeExpandWidth, Recipe.Param.RisistAOIParam.ThinParam.TopHatEdgeExpandHeight);
                HOperatorSet.Difference(OutInspRegions, EX, out DiffRegion);
                HOperatorSet.ReduceDomain(InspImage, DiffRegion, out InspImgOut);
                EX.Dispose();
                HObject InThRegions, OutThRegions;

                HOperatorSet.Threshold(InspImgOut, out OutThRegions, Recipe.Param.RisistAOIParam.ScratchParam.Out_Th, 255);

                HOperatorSet.Threshold(InspImgIn, out InThRegions, Recipe.Param.RisistAOIParam.ScratchParam.In_Th, 255);

                HObject InConnect, OutConnect, InClose, OutClose;

                HOperatorSet.Connection(InThRegions, out InConnect);
                HOperatorSet.Connection(OutThRegions, out OutConnect);
                HOperatorSet.ClosingRectangle1(InConnect, out InClose, 5, 5);
                HOperatorSet.ClosingRectangle1(OutConnect, out OutClose, 5, 5);

                HObject InDefectReg, OutDefectRegion;

                double MaxW = 2500000 * Locate_Method_FS.pixel_resolution_;
                double MaxA = 2500000 * (Locate_Method_FS.pixel_resolution_ * Locate_Method_FS.pixel_resolution_);
                SelectDefectRegions(InClose,
                                    Recipe.Param.RisistAOIParam.ScratchParam.AreaEnabled,
                                    Recipe.Param.RisistAOIParam.ScratchParam.WidthEnabled,
                                    Recipe.Param.RisistAOIParam.ScratchParam.HeightEnabled,
                                    Recipe.Param.RisistAOIParam.ScratchParam.AreaMin,
                                    Recipe.Param.RisistAOIParam.ScratchParam.WidthMin,
                                    Recipe.Param.RisistAOIParam.ScratchParam.HeightMin,
                                    (int)MaxA, (int)MaxW, (int)MaxW, Locate_Method_FS.pixel_resolution_,
                                    out InDefectReg);

                SelectDefectRegions(OutClose,
                                    Recipe.Param.RisistAOIParam.ScratchParam.OuterAreaEnabled,
                                    Recipe.Param.RisistAOIParam.ScratchParam.OuterWidthEnabled,
                                    Recipe.Param.RisistAOIParam.ScratchParam.OuterHeightEnabled,
                                    Recipe.Param.RisistAOIParam.ScratchParam.OuterAreaMin,
                                    Recipe.Param.RisistAOIParam.ScratchParam.OuterWidthMin,
                                    Recipe.Param.RisistAOIParam.ScratchParam.OuterHeightMin,
                                    (int)MaxA, (int)MaxW, (int)MaxW, Locate_Method_FS.pixel_resolution_,
                                    out OutDefectRegion);

                HObject DefectUnion;
                HOperatorSet.Union2(InDefectReg, OutDefectRegion, out DefectUnion);
                HOperatorSet.Connection(DefectUnion, out ScratchDefectRegions);

                #region Dispose
                InspImgOut.Dispose();
                InspImgIn.Dispose();
                DefectUnion.Dispose();
                InDefectReg.Dispose();
                OutDefectRegion.Dispose();
                InConnect.Dispose();
                OutConnect.Dispose();
                InClose.Dispose();
                OutClose.Dispose();
                InThRegions.Dispose();
                OutThRegions.Dispose();
                DiffRegion.Dispose();
                InInspRegions.Dispose();
                OutInspRegions.Dispose();
                #endregion
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }

            return ScratchDefectRegions;
        }

        public bool ScratchInsp(HObject InspImage, InductanceInspRole Recipe, HObject InspRegion, HObject ScratchRegion, HObject ThinRegion, out HObject ScratchDefectRegions, out HObject ScratchMapRegion)
        {
            bool Res = false;
            HOperatorSet.GenEmptyObj(out ScratchDefectRegions);
            HOperatorSet.GenEmptyObj(out ScratchMapRegion);

            if (!Recipe.Param.RisistAOIParam.ScratchParam.Enabled)
            {
                Res = true;
                return Res;
            }

            HObject InInspRegions, OutInspRegions;
            HObject InspImgOut, InspImgIn;


            HOperatorSet.Union1(ThinRegion, out InInspRegions);
            HOperatorSet.ReduceDomain(InspImage, InInspRegions, out InspImgIn);

            HObject DiffRegion, EX;
            HOperatorSet.Union1(ScratchRegion, out OutInspRegions);
            HOperatorSet.DilationRectangle1(InInspRegions, out EX, Recipe.Param.RisistAOIParam.ThinParam.TopHatEdgeExpandWidth, Recipe.Param.RisistAOIParam.ThinParam.TopHatEdgeExpandHeight);
            HOperatorSet.Difference(OutInspRegions, EX, out DiffRegion);
            HOperatorSet.ReduceDomain(InspImage, DiffRegion, out InspImgOut);
            EX.Dispose();
            HObject InThRegions, OutThRegions;

            HOperatorSet.Threshold(InspImgOut, out OutThRegions, Recipe.Param.RisistAOIParam.ScratchParam.Out_Th, 255);

            HOperatorSet.Threshold(InspImgIn, out InThRegions, Recipe.Param.RisistAOIParam.ScratchParam.In_Th, 255);
            HObject InConnect, OutConnect, InClose, OutClose;

            HOperatorSet.Connection(InThRegions, out InConnect);
            HOperatorSet.Connection(OutThRegions, out OutConnect);
            HOperatorSet.ClosingRectangle1(InConnect, out InClose, 5, 5);
            HOperatorSet.ClosingRectangle1(OutConnect, out OutClose, 5, 5);

            HObject InDefectReg, OutDefectRegion;

            double MaxW = 2500000 * Locate_Method_FS.pixel_resolution_;
            double MaxA = 2500000 * (Locate_Method_FS.pixel_resolution_ * Locate_Method_FS.pixel_resolution_);
            SelectDefectRegions(InClose,
                                Recipe.Param.RisistAOIParam.ScratchParam.AreaEnabled,
                                Recipe.Param.RisistAOIParam.ScratchParam.WidthEnabled,
                                Recipe.Param.RisistAOIParam.ScratchParam.HeightEnabled,
                                Recipe.Param.RisistAOIParam.ScratchParam.AreaMin,
                                Recipe.Param.RisistAOIParam.ScratchParam.WidthMin,
                                Recipe.Param.RisistAOIParam.ScratchParam.HeightMin,
                                (int)MaxA, (int)MaxW, (int)MaxW, Locate_Method_FS.pixel_resolution_,
                                out InDefectReg);

            SelectDefectRegions(OutClose,
                                Recipe.Param.RisistAOIParam.ScratchParam.OuterAreaEnabled,
                                Recipe.Param.RisistAOIParam.ScratchParam.OuterWidthEnabled,
                                Recipe.Param.RisistAOIParam.ScratchParam.OuterHeightEnabled,
                                Recipe.Param.RisistAOIParam.ScratchParam.OuterAreaMin,
                                Recipe.Param.RisistAOIParam.ScratchParam.OuterWidthMin,
                                Recipe.Param.RisistAOIParam.ScratchParam.OuterHeightMin,
                                (int)MaxA, (int)MaxW, (int)MaxW, Locate_Method_FS.pixel_resolution_,
                                out OutDefectRegion);

            HObject DefectUnion;
            HOperatorSet.Union2(InDefectReg, OutDefectRegion, out DefectUnion);
            HOperatorSet.Connection(DefectUnion, out ScratchDefectRegions);

            LocalDefectMappping(ScratchDefectRegions, InspRegion, out ScratchMapRegion);

            #region Dispose
            InspImgOut.Dispose();
            InspImgIn.Dispose();
            DefectUnion.Dispose();
            InDefectReg.Dispose();
            OutDefectRegion.Dispose();
            InConnect.Dispose();
            OutConnect.Dispose();
            InClose.Dispose();
            OutClose.Dispose();
            InThRegions.Dispose();
            OutThRegions.Dispose();
            DiffRegion.Dispose();
            InInspRegions.Dispose();
            OutInspRegions.Dispose();
            #endregion
            Res = true;
            return Res;

        }

        //public bool RShiftInsp(HObject SrcImg,HObject CellRegion , InductanceInspRole Recipe,out HObject DefectRegion)
        //{
        //Log.WriteLog("RShift Start");
        //bool Res = false;
        //HOperatorSet.GenEmptyObj(out DefectRegion);
        //if (!Recipe.Param.RisistAOIParam.RShiftParam.Enabled)
        //{
        //    Res = true;
        //    return Res;
        //}
        //try
        //{
        //    HObject CellRegionList;
        //    HOperatorSet.Connection(CellRegion, out CellRegionList);

        //    HTuple Count;

        //    HOperatorSet.CountObj(CellRegionList, out Count);
        //    ConcurrentStack<HObject> resultData = new ConcurrentStack<HObject>();
        //    Parallel.For(1, Count + 1, i =>
        //    {
        //        HObject SelectRegion;
        //        HOperatorSet.SelectObj(CellRegionList, out SelectRegion, i);
        //        HTuple StartY, StartX, EndX;
        //        HOperatorSet.RegionFeatures(SelectRegion, "row1", out StartY);
        //        HOperatorSet.RegionFeatures(SelectRegion, "column1", out StartX);
        //        HOperatorSet.RegionFeatures(SelectRegion, "column2", out EndX);
        //        HObject InspLine;
        //        HOperatorSet.GenRegionLine(out InspLine, StartY, StartX, StartY, EndX);

        //        HTuple Vproj, HProj;
        //        HOperatorSet.GrayProjections(InspLine, SrcImg, "simple", out HProj, out Vproj);
        //        int FIndex = 0;
        //        int BIndex = 0;
        //        FIndex = Array.FindIndex(Vproj.DArr, p => p <= Recipe.Param.RisistAOIParam.RShiftParam.GrayValue) - 1;
        //        BIndex = Array.FindLastIndex(Vproj.DArr, p => p <= Recipe.Param.RisistAOIParam.RShiftParam.GrayValue) + 1;
        //        int L = FIndex + 1;
        //        int R = Vproj.Length - BIndex;
        //        if (Math.Abs(L - R) >= (Recipe.Param.RisistAOIParam.RShiftParam.ShiftStandard - Recipe.Param.RisistAOIParam.RShiftParam.FixDiffValue))
        //        {
        //            resultData.Push(SelectRegion);
        //        }
        //    });
        //    {
        //        HObject Region;
        //        while (resultData.TryPop(out Region))
        //        {
        //            HObject A;
        //            HOperatorSet.ConcatObj(Region, DefectRegion, out A);
        //            DefectRegion.Dispose();
        //            DefectRegion = A;
        //        }
        //    }
        //}
        //catch
        //{
        //    return Res;
        //}
        //Log.WriteLog("RShift End");
        //Res = true;
        //return Res;
        //}


        public class RShiftParam
        {
            public HObject SrcImg;
            public HObject CellRegion;
            public InductanceInspRole Recipe;

            public RShiftParam(HObject pmSrcImg, HObject pmCellRegion, InductanceInspRole pmRecipe)
            {
                this.SrcImg = pmSrcImg;
                this.CellRegion = pmCellRegion;
                this.Recipe = pmRecipe;
            }
        }

        public static HObject RshiftNewInsp(HObject SrcImg, HObject CellRegion, InductanceInspRole Recipe, out HObject AutoTgRegion, out HObject TargetE, out HObject InterRegion)
        {

            HObject DefectRegion;
            HOperatorSet.GenEmptyObj(out DefectRegion);
            HOperatorSet.GenEmptyObj(out TargetE);
            HOperatorSet.GenEmptyObj(out AutoTgRegion);
            HOperatorSet.GenEmptyObj(out InterRegion);
            if (!Recipe.Param.RisistAOIParam.RShiftNewParam.Enabled)
            {
                return DefectRegion;
            }
            try
            {
                HObject UnionCellRegion;
                HOperatorSet.Union1(CellRegion, out UnionCellRegion);

                HObject ReduceImg;
                HOperatorSet.ReduceDomain(SrcImg, UnionCellRegion, out ReduceImg);

                HObject OpenImg;
                HOperatorSet.GrayOpeningRect(ReduceImg, out OpenImg, 31, 31);

                HOperatorSet.AutoThreshold(OpenImg, out AutoTgRegion, Recipe.Param.RisistAOIParam.RShiftNewParam.AutoThSigma);

                HOperatorSet.ClosingRectangle1(AutoTgRegion, out AutoTgRegion, Recipe.Param.RisistAOIParam.RShiftNewParam.CloseSize, Recipe.Param.RisistAOIParam.RShiftNewParam.CloseSize);

                HObject ConnectionRegion1;
                HOperatorSet.Connection(AutoTgRegion, out ConnectionRegion1);

                HObject SelectArea;
                HOperatorSet.SelectShape(ConnectionRegion1, out SelectArea, "area", "and", Recipe.Param.RisistAOIParam.RShiftNewParam.SelectArea, 9999999999);

                HOperatorSet.ShapeTrans(SelectArea, out TargetE, "inner_rectangle1");

                HObject UnionRegion;
                HOperatorSet.Union1(AutoTgRegion, out UnionRegion);

                HObject DiffRegion;
                HOperatorSet.Difference(UnionRegion, SelectArea, out DiffRegion);

                HObject ConRegion;
                HOperatorSet.Connection(DiffRegion, out ConRegion);

                HObject SelectWidth;
                HOperatorSet.SelectShape(ConRegion, out SelectWidth, "width", "and", Recipe.Param.RisistAOIParam.RShiftNewParam.SelectWidth, 9999999999);

                HOperatorSet.Intersection(CellRegion, SelectWidth, out InterRegion);

                HObject TargetR;
                HOperatorSet.ShapeTrans(InterRegion, out TargetR, "convex");

                HObject TR, TE;

                HOperatorSet.SortRegion(TargetR, out TR, "character", "true", "column");
                HOperatorSet.SortRegion(TargetE, out TE, "character", "true", "column");

                HTuple AreaE, RowE, ColumnE;
                HTuple AreaR, RowR, ColumnR;

                HOperatorSet.AreaCenter(TR, out AreaR, out RowR, out ColumnR);
                HOperatorSet.AreaCenter(TE, out AreaE, out RowE, out ColumnE);


                for (int i = 0; i < ColumnE.Length; i++)
                {
                    if (Math.Abs(ColumnR[i].D - ColumnE[i].D) >= Recipe.Param.RisistAOIParam.RShiftNewParam.ShiftStandard)
                    {
                        HObject Region;
                        HOperatorSet.GenRectangle1(out Region, RowR[i].D - 5, ColumnR[i].D - 5, RowR[i].D + 5, ColumnR[i].D + 5);
                        HOperatorSet.ConcatObj(DefectRegion, Region, out DefectRegion);
                    }
                }
                UnionCellRegion.Dispose();
                ReduceImg.Dispose();
                OpenImg.Dispose();
                ConnectionRegion1.Dispose();
                SelectArea.Dispose();
                UnionRegion.Dispose();
                DiffRegion.Dispose();
                ConRegion.Dispose();
                SelectWidth.Dispose();
                TargetR.Dispose();
                TR.Dispose();
                TE.Dispose();
            }
            catch
            {
                return DefectRegion;
            }

            return DefectRegion;

        }

        public static HObject RshiftNewInspTask(object Obj)
        {
            #region Convert Obj

            HObject SrcImg = ((RShiftParam)Obj).SrcImg;
            HObject CellRegion = ((RShiftParam)Obj).CellRegion;
            InductanceInspRole Recipe = ((RShiftParam)Obj).Recipe;

            #endregion

            HObject DefectRegion;
            HOperatorSet.GenEmptyObj(out DefectRegion);
            if (!Recipe.Param.RisistAOIParam.RShiftNewParam.Enabled)
            {
                return DefectRegion;
            }
            try
            {
                HObject UnionCellRegion;
                HOperatorSet.Union1(CellRegion, out UnionCellRegion);

                HObject ReduceImg;
                HOperatorSet.ReduceDomain(SrcImg, UnionCellRegion, out ReduceImg);

                HObject OpenImg;
                HOperatorSet.GrayOpeningRect(ReduceImg, out OpenImg, 31, 31);

                HObject AutoTgRegion;
                HOperatorSet.AutoThreshold(OpenImg, out AutoTgRegion, Recipe.Param.RisistAOIParam.RShiftNewParam.AutoThSigma);

                HOperatorSet.ClosingRectangle1(AutoTgRegion, out AutoTgRegion, Recipe.Param.RisistAOIParam.RShiftNewParam.CloseSize, Recipe.Param.RisistAOIParam.RShiftNewParam.CloseSize);

                HObject ConnectionRegion1;
                HOperatorSet.Connection(AutoTgRegion, out ConnectionRegion1);

                HObject SelectArea;
                HOperatorSet.SelectShape(ConnectionRegion1, out SelectArea, "area", "and", Recipe.Param.RisistAOIParam.RShiftNewParam.SelectArea, 9999999999);

                HObject TargetE;
                HOperatorSet.ShapeTrans(SelectArea, out TargetE, "inner_rectangle1");

                HObject UnionRegion;
                HOperatorSet.Union1(AutoTgRegion, out UnionRegion);

                HObject DiffRegion;
                HOperatorSet.Difference(UnionRegion, SelectArea, out DiffRegion);

                HObject ConRegion;
                HOperatorSet.Connection(DiffRegion, out ConRegion);

                HObject SelectWidth;
                HOperatorSet.SelectShape(ConRegion, out SelectWidth, "width", "and", Recipe.Param.RisistAOIParam.RShiftNewParam.SelectWidth, 9999999999);

                HObject InterRegion;
                HOperatorSet.Intersection(CellRegion, SelectWidth, out InterRegion);

                HObject TargetR;
                HOperatorSet.ShapeTrans(InterRegion, out TargetR, "rectangle1");

                HObject TR, TE;

                HOperatorSet.SortRegion(TargetR, out TR, "character", "true", "column");
                HOperatorSet.SortRegion(TargetE, out TE, "character", "true", "column");


                HTuple AreaE, RowE, ColumnE;
                HTuple AreaR, RowR, ColumnR;

                HOperatorSet.AreaCenter(TR, out AreaR, out RowR, out ColumnR);
                HOperatorSet.AreaCenter(TE, out AreaE, out RowE, out ColumnE);



                for (int i = 0; i < ColumnE.Length; i++)
                {
                    if (Math.Abs(ColumnR[i].D - ColumnE[i].D) >= Recipe.Param.RisistAOIParam.RShiftNewParam.ShiftStandard)
                    {
                        HObject Region;
                        HOperatorSet.GenRectangle1(out Region, RowR[i].D - 5, ColumnR[i].D - 5, RowR[i].D + 5, ColumnR[i].D + 5);
                        HOperatorSet.ConcatObj(DefectRegion, Region, out DefectRegion);
                    }
                }
                UnionCellRegion.Dispose();
                ReduceImg.Dispose();
                AutoTgRegion.Dispose();
                //FullupRegion.Dispose();
                OpenImg.Dispose();
                ConnectionRegion1.Dispose();
                SelectArea.Dispose();
                TargetE.Dispose();
                UnionRegion.Dispose();
                DiffRegion.Dispose();
                ConRegion.Dispose();
                SelectWidth.Dispose();
                InterRegion.Dispose();
                TargetR.Dispose();
                TR.Dispose();
                TE.Dispose();
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
                return DefectRegion;
            }

            return DefectRegion;

        }
        //public static HObject RShiftInspTask(object Obj)
        //{
        //    #region Convert Obj

        //    HObject SrcImg = ((RShiftParam)Obj).SrcImg;
        //    HObject CellRegion = ((RShiftParam)Obj).CellRegion;
        //    InductanceInspRole Recipe = ((RShiftParam)Obj).Recipe;

        //    #endregion

        //    HObject DefectRegion;
        //    HOperatorSet.GenEmptyObj(out DefectRegion);
        //    if (!Recipe.Param.RisistAOIParam.RShiftParam.Enabled)
        //    {
        //        return DefectRegion;
        //    }
        //    try
        //    {
        //        HObject CellRegionList;
        //        HOperatorSet.Connection(CellRegion, out CellRegionList);
        //        HObject MeanImg;

        //        HOperatorSet.MeanImage(SrcImg, out MeanImg, 5, 5);


        //        HTuple Count;

        //        HOperatorSet.CountObj(CellRegionList, out Count);
        //        ConcurrentStack<HObject> resultData = new ConcurrentStack<HObject>();
        //        Parallel.For(1, Count + 1, i =>
        //        {
        //            HObject SelectRegion;
        //            HOperatorSet.SelectObj(CellRegionList, out SelectRegion, i);
        //            HTuple StartY, StartX, EndX;
        //            HOperatorSet.RegionFeatures(SelectRegion, "row1", out StartY);
        //            HOperatorSet.RegionFeatures(SelectRegion, "column1", out StartX);
        //            HOperatorSet.RegionFeatures(SelectRegion, "column2", out EndX);
        //            HObject InspLine;
        //            HOperatorSet.GenRegionLine(out InspLine, StartY, StartX, StartY, EndX);

        //            HTuple Vproj, HProj;
        //            HOperatorSet.GrayProjections(InspLine, MeanImg, "simple", out HProj, out Vproj);
        //            int FIndex = 0;
        //            int BIndex = 0;

        //            FIndex = Array.FindIndex(Vproj.DArr, p => p <= Recipe.Param.RisistAOIParam.RShiftParam.GrayValue) - 1;
        //            BIndex = Array.FindLastIndex(Vproj.DArr, p => p <= Recipe.Param.RisistAOIParam.RShiftParam.GrayValue) + 1;

        //            int L = FIndex + 1;
        //            int R = Vproj.Length - BIndex;
        //            if (Math.Abs(L - R) >= (Recipe.Param.RisistAOIParam.RShiftParam.ShiftStandard - Recipe.Param.RisistAOIParam.RShiftParam.FixDiffValue))
        //            {
        //                resultData.Push(SelectRegion);
        //            }
        //        });
        //        {
        //            HObject Region;
        //            while (resultData.TryPop(out Region))
        //            {
        //                HObject A;
        //                HOperatorSet.ConcatObj(Region, DefectRegion, out A);
        //                DefectRegion.Dispose();
        //                DefectRegion = A;
        //            }
        //        }
        //        MeanImg.Dispose();
        //    }
        //    catch
        //    {
        //        return DefectRegion;
        //    }

        //    return DefectRegion;
        //}

        public class ThinScratchParam
        {
            public HObject InspImage;
            public HObject InspRegion;
            public HTuple TextureInspectionModel;
            public bool InspEnabled;
            public int OpenSizeW;
            public int CloseSizeW;
            public int OpenSizeH;
            public int CloseSizeH;
            public int EdgeSkipWidth;
            public int EdgeSkipHeight;
            public int EdgeExWidth;
            public int EdgeExHeight;


            public ThinScratchParam(HObject pmInspImage, HObject pmInspRegion, HTuple pmTextureInspectionModel, bool pmInspEnabled, int pmOpenSizeW, int pmCloseSizeW, int pmOpenSizeH, int pmCloseSizeH, int pmEdgeSkipWidth, int pmEdgeSkipHeight, int pmEdgeExWidth, int pmEdgeExHeight)
            {
                this.InspImage = pmInspImage;
                this.InspRegion = pmInspRegion;
                this.TextureInspectionModel = pmTextureInspectionModel;
                this.InspEnabled = pmInspEnabled;
                this.OpenSizeW = pmOpenSizeW;
                this.CloseSizeW = pmCloseSizeW;
                this.OpenSizeH = pmOpenSizeH;
                this.CloseSizeH = pmCloseSizeH;
                this.EdgeSkipWidth = pmEdgeSkipWidth;
                this.EdgeSkipHeight = pmEdgeSkipHeight;
                this.EdgeExWidth = pmEdgeExWidth;
                this.EdgeExHeight = pmEdgeExHeight;
            }
        }

        static HObject ThinScratchInspTask(object Obj)
        {
            HObject DefectRegion;
            HOperatorSet.GenEmptyObj(out DefectRegion);

            #region Convert Obj

            HObject InspImage = ((ThinScratchParam)Obj).InspImage;
            HObject InspRegion = ((ThinScratchParam)Obj).InspRegion;
            HTuple TextureInspectionModel = ((ThinScratchParam)Obj).TextureInspectionModel;
            bool InspEnabled = ((ThinScratchParam)Obj).InspEnabled;
            int OpenSizeW = ((ThinScratchParam)Obj).OpenSizeW;
            int CloseSizeW = ((ThinScratchParam)Obj).CloseSizeW;
            int OpenSizeH = ((ThinScratchParam)Obj).OpenSizeH;
            int CloseSizeH = ((ThinScratchParam)Obj).CloseSizeH;
            int EdgeSkipWidth = ((ThinScratchParam)Obj).EdgeSkipWidth;
            int EdgeSkipHeight = ((ThinScratchParam)Obj).EdgeSkipHeight;
            int EdgeExWidth = ((ThinScratchParam)Obj).EdgeExWidth;
            int EdgeExHeight = ((ThinScratchParam)Obj).EdgeExHeight;

            #endregion

            if (TextureInspectionModel == null)
            {
                return DefectRegion;
            }

            try
            {
                HObject ThisInspRegionSkip;

                HOperatorSet.ErosionRectangle1(InspRegion, out ThisInspRegionSkip, EdgeSkipWidth, EdgeSkipHeight);

                HObject ThisInspRegionEx;

                HOperatorSet.DilationRectangle1(ThisInspRegionSkip, out ThisInspRegionEx, EdgeExWidth, EdgeExHeight);

                HOperatorSet.Connection(ThisInspRegionEx, out ThisInspRegionEx);

                if (InspEnabled)
                {
                    HObject Defect, Open;
                    HOperatorSet.GenEmptyObj(out Defect);
                    HOperatorSet.GenEmptyObj(out Open);

                    HTuple ResultID;
                    HObject CellDefectRegion;

                    HObject CropImg;
                    HTuple Column1, Row1, Row2, Column2;
                    HOperatorSet.RegionFeatures(ThisInspRegionEx, "row1", out Row1);
                    HOperatorSet.RegionFeatures(ThisInspRegionEx, "column1", out Column1);
                    HOperatorSet.RegionFeatures(ThisInspRegionEx, "row2", out Row2);
                    HOperatorSet.RegionFeatures(ThisInspRegionEx, "column2", out Column2);


                    //HObject T1, Entropy, I;
                    //HOperatorSet.DerivateGauss(InspImage, out T1, 7, "laplace");
                    //HOperatorSet.ScaleImageMax(T1, out T1);
                    //HOperatorSet.EntropyImage(InspImage, out Entropy, 11, 11);
                    //HOperatorSet.SubImage(T1, Entropy, out I, 2, 128);

                    HOperatorSet.CropRectangle1(InspImage, out CropImg, Row1, Column1, Row2, Column2);

                    HOperatorSet.ApplyTextureInspectionModel(CropImg, out CellDefectRegion, TextureInspectionModel, out ResultID);

                    HTuple Count;
                    HOperatorSet.CountObj(CellDefectRegion, out Count);
                    for (int i = 0; i < Count; i++)
                    {
                        HObject SelectRegion, Tmp;
                        HOperatorSet.SelectObj(CellDefectRegion, out SelectRegion, i + 1);
                        HOperatorSet.MoveRegion(SelectRegion, out Tmp, Row1[i], Column1[i]);
                        HOperatorSet.ConcatObj(Tmp, Defect, out Defect);
                    }

                    Open.Dispose();
                    HOperatorSet.Connection(Defect, out Open);
                    HObject Noise;
                    HOperatorSet.RemoveNoiseRegion(Open, out Noise, "n_4");
                    Open.Dispose();
                    HOperatorSet.OpeningRectangle1(Noise, out Open, OpenSizeW, OpenSizeH);
                    HOperatorSet.ClosingRectangle1(Open, out DefectRegion, CloseSizeW, CloseSizeH);
                    Open.Dispose();
                    Noise.Dispose();
                    Defect.Dispose();
                }

            }
            catch
            {
                return DefectRegion;
            }

            return DefectRegion;
        }


        public bool ThinScratchInsp(HObject InspImage, HObject InspRegion, HTuple TextureInspectionModel, bool InspEnabled, int OpenSizeW, int CloseSizeW, int OpenSizeH, int CloseSizeH, int EdgeSkipWidth, int EdgeSkipHeight, out HObject DefectRegion)
        {
            bool Res = false;
            HOperatorSet.GenEmptyObj(out DefectRegion);
            try
            {
                HObject ThisInspRegionSkip;

                HOperatorSet.ErosionRectangle1(InspRegion, out ThisInspRegionSkip, EdgeSkipWidth, EdgeSkipHeight);

                HOperatorSet.Connection(ThisInspRegionSkip, out ThisInspRegionSkip);

                if (InspEnabled)
                {
                    HTuple Count;
                    HOperatorSet.CountObj(ThisInspRegionSkip, out Count);
                    HTuple ResultID;
                    HObject Defect, Open;
                    HOperatorSet.GenEmptyObj(out Defect);
                    HOperatorSet.GenEmptyObj(out Open);
                    for (int i = 1; i <= Count; i++)
                    {
                        HObject SelectObj, ReducImg, CellDefectRegion;
                        HOperatorSet.SelectObj(ThisInspRegionSkip, out SelectObj, i);
                        HOperatorSet.ReduceDomain(InspImage, SelectObj, out ReducImg);
                        HOperatorSet.ApplyTextureInspectionModel(ReducImg, out CellDefectRegion, TextureInspectionModel, out ResultID);
                        {
                            HObject Tmp;
                            HOperatorSet.ConcatObj(Defect, CellDefectRegion, out Tmp);
                            Defect.Dispose();
                            Defect = Tmp;
                        }
                        HOperatorSet.ClearTextResult(ResultID);
                        ReducImg.Dispose();
                        SelectObj.Dispose();
                    }
                    Open.Dispose();
                    HOperatorSet.Connection(Defect, out Open);
                    HObject Noise;
                    HOperatorSet.RemoveNoiseRegion(Open, out Noise, "n_4");
                    Open.Dispose();
                    HOperatorSet.OpeningRectangle1(Noise, out Open, OpenSizeW, OpenSizeH);
                    HOperatorSet.ClosingRectangle1(Open, out DefectRegion, CloseSizeW, CloseSizeH);
                    Open.Dispose();
                    Noise.Dispose();
                    Defect.Dispose();
                }

            }
            catch
            {
                return Res;
            }

            Res = true;
            return Res;
        }

        static bool ThinMeanInsp(HObject InspImage, HObject InspRegion, int Th, bool InspEnabled, int EdgeSkipWidth, int EdgeSkipHeight, out HObject DefectRegion, out double MeanValue)
        {
            bool Res = false;
            HOperatorSet.GenEmptyObj(out DefectRegion);
            MeanValue = 0;
            try
            {
                HObject ThisInspRegionSkip;
                //HTuple Mean, Dev;
                HObject ThinInspImg;
                HOperatorSet.ErosionRectangle1(InspRegion, out ThisInspRegionSkip, EdgeSkipWidth, EdgeSkipHeight);
                HOperatorSet.ReduceDomain(InspImage, ThisInspRegionSkip, out ThinInspImg);
                //HOperatorSet.Intensity(ThisInspRegionSkip, ThinInspImg, out Mean, out Dev);
                //CalcMean(Mean, Dev, ThisInspRegionSkip, ThinInspImg, out MeanValue);
                HTuple Med;
                HOperatorSet.GrayFeatures(ThisInspRegionSkip, ThinInspImg, "median", out Med);
                MeanValue = Med.D;
                if (InspEnabled)
                {
                    HOperatorSet.Threshold(ThinInspImg, out DefectRegion, MeanValue + Th, 255);
                }
                ThinInspImg.Dispose();
            }
            catch
            {
                return Res;
            }

            Res = true;
            return Res;
        }

        public class HoleParam
        {
            public HObject InspImage;
            public HObject InspRegion;
            public bool InspEnabled;
            public int SESizeWidth;
            public int SESizeHeight;
            public int TopHatEdgeExpandWidth;
            public int TopHatEdgeExpandHeight;
            public double Mean;
            public int ExtH;
            public int Th;
            public int ExtThAdd;
            public int BrightDetectTh;

            public HoleParam(HObject pmInspImage, HObject pmInspRegion, bool pmInspEnabled, int pmSESizeWidth, int pmSESizeHeight, int pmTopHatEdgeExpandWidth, int pmTopHatEdgeExpandHeight, double pmMean, int pmExtH, int pmTh, int pmExtThAdd, int pmBrightDetectTh)
            {
                this.InspImage = pmInspImage;
                this.InspRegion = pmInspRegion;
                this.InspEnabled = pmInspEnabled;
                this.SESizeWidth = pmSESizeWidth;
                this.SESizeHeight = pmSESizeHeight;
                this.TopHatEdgeExpandWidth = pmTopHatEdgeExpandWidth;
                this.TopHatEdgeExpandHeight = pmTopHatEdgeExpandHeight;
                this.Mean = pmMean;
                this.ExtH = pmExtH;
                this.Th = pmTh;
                this.ExtThAdd = pmExtThAdd;
                this.BrightDetectTh = pmBrightDetectTh;
            }
        }

        static HObject HoleTopHatInspTask(object Obj)
        {

            HObject DefectRegion;
            HOperatorSet.GenEmptyObj(out DefectRegion);
            #region ConvertParam

            HObject InspImage = ((HoleParam)Obj).InspImage;
            HObject InspRegion = ((HoleParam)Obj).InspRegion;
            bool InspEnabled = ((HoleParam)Obj).InspEnabled;
            int SESizeWidth = ((HoleParam)Obj).SESizeWidth;
            int SESizeHeight = ((HoleParam)Obj).SESizeHeight;
            int TopHatEdgeExpandWidth = ((HoleParam)Obj).TopHatEdgeExpandWidth;
            int TopHatEdgeExpandHeight = ((HoleParam)Obj).TopHatEdgeExpandHeight;
            double Mean = ((HoleParam)Obj).Mean;
            int ExtH = ((HoleParam)Obj).ExtH;
            int Th = ((HoleParam)Obj).Th;
            int ExtThAdd = ((HoleParam)Obj).ExtThAdd;
            int BrightDetectTh = ((HoleParam)Obj).BrightDetectTh;

            #endregion


            try
            {
                if (InspEnabled)
                {
                    HObject TopHatInspImg;
                    HObject TopHatImg, SE;
                    HObject TopHatRegionExpand;
                    HOperatorSet.GenDiscSe(out SE, "byte", SESizeWidth, SESizeHeight, 0);
                    HOperatorSet.DilationRectangle1(InspRegion, out TopHatRegionExpand, TopHatEdgeExpandWidth, TopHatEdgeExpandHeight);


                    HObject NoiseTh, InspImg;
                    HOperatorSet.Threshold(InspImage, out NoiseTh, 0, Mean - 15);
                    InspImg = InspImage.Clone();
                    HOperatorSet.OverpaintRegion(InspImg, NoiseTh, Mean, "fill");


                    HObject ExtHRegion, DiffRegion;
                    HObject ExtHInspImage, ExtHImage;
                    HObject EdgeDefect;
                    HOperatorSet.DilationRectangle1(TopHatRegionExpand, out ExtHRegion, 1, ExtH);
                    HOperatorSet.Difference(ExtHRegion, TopHatRegionExpand, out DiffRegion);
                    HOperatorSet.ReduceDomain(InspImg, DiffRegion, out ExtHInspImage);
                    HOperatorSet.GrayTophat(ExtHInspImage, SE, out ExtHImage);
                    HOperatorSet.Threshold(ExtHImage, out EdgeDefect, Th + ExtThAdd, 255);

                    HOperatorSet.ReduceDomain(InspImg, TopHatRegionExpand, out TopHatInspImg);
                    HObject OrgTopRegion;
                    HOperatorSet.GrayTophat(TopHatInspImg, SE, out TopHatImg);
                    HOperatorSet.Threshold(TopHatImg, out OrgTopRegion, Th, 255);

                    HObject BrigthRegion, MergeRegion;
                    HOperatorSet.Threshold(TopHatInspImg, out BrigthRegion, BrightDetectTh, 255);

                    HOperatorSet.Union2(EdgeDefect, BrigthRegion, out MergeRegion);

                    HOperatorSet.Union2(MergeRegion, OrgTopRegion, out DefectRegion);

                    BrigthRegion.Dispose();
                    ExtHInspImage.Dispose();
                    TopHatInspImg.Dispose();
                    TopHatImg.Dispose();
                    InspImg.Dispose();
                    SE.Dispose();
                    NoiseTh.Dispose();
                    ExtHRegion.Dispose();
                    DiffRegion.Dispose();
                    ExtHImage.Dispose();
                    EdgeDefect.Dispose();
                    MergeRegion.Dispose();
                }
            }
            catch
            {
                return DefectRegion;
            }
            return DefectRegion;
        }


        public bool HoleTopHatInsp(HObject InspImage, HObject InspRegion, bool InspEnabled, int SESizeWidth, int SESizeHeight, int TopHatEdgeExpandWidth, int TopHatEdgeExpandHeight, double Mean, int ExtH, int Th, int ExtThAdd, int BrightDetectTh, out HObject DefectRegion)
        {
            bool Res = false;
            HOperatorSet.GenEmptyObj(out DefectRegion);
            try
            {
                if (InspEnabled)
                {
                    HObject TopHatInspImg;
                    HObject TopHatImg, SE;
                    HObject TopHatRegionExpand;
                    HOperatorSet.GenDiscSe(out SE, "byte", SESizeWidth, SESizeHeight, 0);
                    HOperatorSet.DilationRectangle1(InspRegion, out TopHatRegionExpand, TopHatEdgeExpandWidth, TopHatEdgeExpandHeight);


                    HObject NoiseTh, InspImg;
                    HOperatorSet.Threshold(InspImage, out NoiseTh, 0, Mean - 15);
                    InspImg = InspImage.Clone();
                    HOperatorSet.OverpaintRegion(InspImg, NoiseTh, Mean, "fill");


                    HObject ExtHRegion, DiffRegion;
                    HObject ExtHInspImage, ExtHImage;
                    HObject EdgeDefect;
                    HOperatorSet.DilationRectangle1(TopHatRegionExpand, out ExtHRegion, 1, ExtH);
                    HOperatorSet.Difference(ExtHRegion, TopHatRegionExpand, out DiffRegion);
                    HOperatorSet.ReduceDomain(InspImg, DiffRegion, out ExtHInspImage);
                    HOperatorSet.GrayTophat(ExtHInspImage, SE, out ExtHImage);
                    HOperatorSet.Threshold(ExtHImage, out EdgeDefect, Th + ExtThAdd, 255);

                    HOperatorSet.ReduceDomain(InspImg, TopHatRegionExpand, out TopHatInspImg);
                    HObject OrgTopRegion;
                    HOperatorSet.GrayTophat(TopHatInspImg, SE, out TopHatImg);
                    HOperatorSet.Threshold(TopHatImg, out OrgTopRegion, Th, 255);

                    HObject BrigthRegion, MergeRegion;
                    HOperatorSet.Threshold(TopHatInspImg, out BrigthRegion, BrightDetectTh, 255);

                    HOperatorSet.Union2(EdgeDefect, BrigthRegion, out MergeRegion);

                    HOperatorSet.Union2(MergeRegion, OrgTopRegion, out DefectRegion);

                    BrigthRegion.Dispose();
                    ExtHInspImage.Dispose();
                    TopHatInspImg.Dispose();
                    TopHatImg.Dispose();
                    InspImg.Dispose();
                    SE.Dispose();
                    NoiseTh.Dispose();
                    ExtHRegion.Dispose();
                    DiffRegion.Dispose();
                    ExtHImage.Dispose();
                    EdgeDefect.Dispose();
                    MergeRegion.Dispose();
                }
            }
            catch
            {
                return Res;
            }

            Res = true;
            return Res;
        }

        public class TopHatParam
        {
            public HObject InspImage;
            public HObject InspRegion;
            public bool InspEnabled;
            public int ThinInspWidrh;
            public int ThinInspHeight;
            public double Mean;
            public int ThinHatEdgeSkipWidth;
            public int ThinHatEdgeSkipHeight;
            public int Th;
            public int ThinHatOpenWidth;
            public int ThinHatOpenHeight;
            public int ThinHatCloseWidth;
            public int ThinHatCloseHeight;
            public TopHatParam() { }
            public TopHatParam(HObject pmInspImage, HObject pmInspRegion, bool pmInspEnabled, int pmThinInspWidrh, int pmThinInspHeight, double pmMean, int pmThinHatEdgeSkipWidth, int pmThinHatEdgeSkipHeight, int pmTh, int pmThinHatOpenWidth, int pmThinHatOpenHeight, int pmThinHatCloseWidth, int pmThinHatCloseHeight)
            {
                this.InspImage = pmInspImage;
                this.InspRegion = pmInspRegion;
                this.InspEnabled = pmInspEnabled;
                this.ThinInspWidrh = pmThinInspWidrh;
                this.ThinInspHeight = pmThinInspHeight;
                this.Mean = pmMean;
                this.ThinHatEdgeSkipWidth = pmThinHatEdgeSkipWidth;
                this.ThinHatEdgeSkipHeight = pmThinHatEdgeSkipHeight;
                this.Th = pmTh;
                this.ThinHatOpenWidth = pmThinHatOpenWidth;
                this.ThinHatOpenHeight = pmThinHatOpenHeight;
                this.ThinHatCloseWidth = pmThinHatCloseWidth;
                this.ThinHatCloseHeight = pmThinHatCloseHeight;

            }
        }

        static HObject ThinTopHatInspTask(object Obj)
        {
            HObject DefectRegion;
            HOperatorSet.GenEmptyObj(out DefectRegion);
            #region Convert Obj

            HObject InspImage = ((TopHatParam)Obj).InspImage;
            HObject InspRegion = ((TopHatParam)Obj).InspRegion;
            bool InspEnabled = ((TopHatParam)Obj).InspEnabled;
            int ThinInspHeight = ((TopHatParam)Obj).ThinInspHeight;
            int ThinInspWidrh = ((TopHatParam)Obj).ThinInspWidrh;
            double Mean = ((TopHatParam)Obj).Mean;
            int ThinHatEdgeSkipWidth = ((TopHatParam)Obj).ThinHatEdgeSkipWidth;
            int ThinHatEdgeSkipHeight = ((TopHatParam)Obj).ThinHatEdgeSkipHeight;
            int Th = ((TopHatParam)Obj).Th;
            int ThinHatOpenWidth = ((TopHatParam)Obj).ThinHatOpenWidth;
            int ThinHatOpenHeight = ((TopHatParam)Obj).ThinHatOpenHeight;
            int ThinHatCloseWidth = ((TopHatParam)Obj).ThinHatCloseWidth;
            int ThinHatCloseHeight = ((TopHatParam)Obj).ThinHatCloseHeight;

            #endregion


            try
            {
                if (InspEnabled)
                {
                    HObject TopHatInspImg;
                    HObject TopHatImg, SE;
                    HObject TopHatRegionSkip;
                    HObject Rect1;

                    int H = ThinInspHeight / 2;
                    int W = ThinInspWidrh / 2;
                    HOperatorSet.GenRectangle1(out Rect1, 0, 0, H, W);
                    HOperatorSet.RegionToBin(Rect1, out SE, 0, 0, W, H);

                    HObject NoiseTh, InspImg;
                    HOperatorSet.Threshold(InspImage, out NoiseTh, 0, Mean - 15);
                    InspImg = InspImage.Clone();
                    HOperatorSet.OverpaintRegion(InspImg, NoiseTh, Mean, "fill");


                    HOperatorSet.ErosionRectangle1(InspRegion, out TopHatRegionSkip, ThinHatEdgeSkipWidth, ThinHatEdgeSkipHeight);

                    HOperatorSet.ReduceDomain(InspImg, TopHatRegionSkip, out TopHatInspImg);
                    HObject ThRegions;
                    HObject OpenREgion;
                    HOperatorSet.GrayTophat(TopHatInspImg, SE, out TopHatImg);
                    HOperatorSet.Threshold(TopHatImg, out ThRegions, Th, 255);

                    HOperatorSet.ErosionRectangle1(ThRegions, out OpenREgion, ThinHatOpenWidth, ThinHatOpenHeight);
                    HOperatorSet.DilationRectangle1(OpenREgion, out DefectRegion, ThinHatCloseWidth, ThinHatCloseHeight);

                    InspImg.Dispose();
                    TopHatInspImg.Dispose();
                    TopHatImg.Dispose();
                    SE.Dispose();
                    TopHatRegionSkip.Dispose();
                    Rect1.Dispose();
                    NoiseTh.Dispose();
                    ThRegions.Dispose();
                    OpenREgion.Dispose();
                }
            }
            catch
            {
                return DefectRegion;
            }

            return DefectRegion;
        }

        public bool ThinTopHatInsp(HObject InspImage, HObject InspRegion, bool InspEnabled, int ThinInspWidrh, int ThinInspHeight, double Mean, int ThinHatEdgeSkipWidth, int ThinHatEdgeSkipHeight, int Th, int ThinHatOpenWidth, int ThinHatOpenHeight, int ThinHatCloseWidth, int ThinHatCloseHeight, out HObject DefectRegion)
        {
            bool Res = false;
            HOperatorSet.GenEmptyObj(out DefectRegion);
            try
            {
                if (InspEnabled)
                {
                    HObject TopHatInspImg;
                    HObject TopHatImg, SE;
                    HObject TopHatRegionSkip;
                    HObject Rect1;

                    int H = ThinInspHeight / 2;
                    int W = ThinInspWidrh / 2;
                    HOperatorSet.GenRectangle1(out Rect1, 0, 0, H, W);
                    HOperatorSet.RegionToBin(Rect1, out SE, 0, 0, W, H);

                    HObject NoiseTh, InspImg;
                    HOperatorSet.Threshold(InspImage, out NoiseTh, 0, Mean - 15);
                    InspImg = InspImage.Clone();
                    HOperatorSet.OverpaintRegion(InspImg, NoiseTh, Mean, "fill");


                    HOperatorSet.ErosionRectangle1(InspRegion, out TopHatRegionSkip, ThinHatEdgeSkipWidth, ThinHatEdgeSkipHeight);

                    HOperatorSet.ReduceDomain(InspImg, TopHatRegionSkip, out TopHatInspImg);
                    HObject ThRegions;
                    HObject OpenREgion;
                    HOperatorSet.GrayTophat(TopHatInspImg, SE, out TopHatImg);
                    HOperatorSet.Threshold(TopHatImg, out ThRegions, Th, 255);

                    HOperatorSet.ErosionRectangle1(ThRegions, out OpenREgion, ThinHatOpenWidth, ThinHatOpenHeight);
                    HOperatorSet.DilationRectangle1(OpenREgion, out DefectRegion, ThinHatCloseWidth, ThinHatCloseHeight);

                    InspImg.Dispose();
                    TopHatInspImg.Dispose();
                    TopHatImg.Dispose();
                    SE.Dispose();
                    TopHatRegionSkip.Dispose();
                    Rect1.Dispose();
                    NoiseTh.Dispose();
                    ThRegions.Dispose();
                    OpenREgion.Dispose();
                }
            }
            catch
            {
                return Res;
            }

            Res = true;
            return Res;
        }

        public class HistoParam
        {
            public HObject InspImage;
            public HObject InspRegionDark;
            public HObject InspRegionBright;
            public bool InspEnabled;
            public int HistoEdgeSkipWidth;
            public int HistoEdgeSkipHight;
            public int HistoEqOpeningWidth;
            public int HistoEqOpeningHeight;
            public int HistoEqClosingWidth;
            public int HistoEqClosingHeight;
            public int Th;

            public HistoParam(HObject pmInspImage, HObject pmInspRegionDark, HObject pmInspRegionBright, bool pmInspEnabled, int pmHistoEdgeSkipWidth, int pmHistoEdgeSkipHight, int pmHistoEqOpeningWidth, int pmHistoEqOpeningHeight, int pmHistoEqClosingWidth, int pmHistoEqClosingHeight, int pmTh)
            {
                this.InspImage = pmInspImage;
                this.InspRegionDark = pmInspRegionDark;
                this.InspRegionBright = pmInspRegionBright;
                this.InspEnabled = pmInspEnabled;
                this.HistoEdgeSkipWidth = pmHistoEdgeSkipWidth;
                this.HistoEdgeSkipHight = pmHistoEdgeSkipHight;
                this.HistoEqOpeningWidth = pmHistoEqOpeningWidth;
                this.HistoEqOpeningHeight = pmHistoEqOpeningHeight;
                this.HistoEqClosingWidth = pmHistoEqClosingWidth;
                this.HistoEqClosingHeight = pmHistoEqClosingHeight;
                this.Th = pmTh;
            }
        }

        static HObject HistoEqThinInspTask(object Obj)
        {
            #region Convert Obj

            HObject InspImage = ((HistoParam)Obj).InspImage;
            HObject InspRegionDark = ((HistoParam)Obj).InspRegionDark;
            HObject InspRegionBright = ((HistoParam)Obj).InspRegionBright;
            bool InspEnabled = ((HistoParam)Obj).InspEnabled;
            int HistoEdgeSkipWidth = ((HistoParam)Obj).HistoEdgeSkipWidth;
            int HistoEdgeSkipHight = ((HistoParam)Obj).HistoEdgeSkipHight;
            int HistoEqOpeningWidth = ((HistoParam)Obj).HistoEqOpeningWidth;
            int HistoEqOpeningHeight = ((HistoParam)Obj).HistoEqOpeningHeight;
            int HistoEqClosingWidth = ((HistoParam)Obj).HistoEqClosingWidth;
            int HistoEqClosingHeight = ((HistoParam)Obj).HistoEqClosingHeight;
            int Th = ((HistoParam)Obj).Th;

            #endregion

            HObject HistoEqRegion;
            HOperatorSet.GenEmptyObj(out HistoEqRegion);
            try
            {
                if (InspEnabled)
                {
                    HObject ErosionRegion;
                    HObject InspRegion;
                    HOperatorSet.Union2(InspRegionDark, InspRegionBright, out InspRegion);

                    HOperatorSet.ErosionRectangle1(InspRegion, out ErosionRegion, HistoEdgeSkipWidth, HistoEdgeSkipHight);
                    HObject ConRegion;
                    HOperatorSet.Connection(ErosionRegion, out ConRegion);
                    HTuple CountR;
                    HOperatorSet.CountObj(ConRegion, out CountR);

                    HObject HistoEqImg, open, close;
                    HObject CellDefect;
                    HObject CropImgList;
                    HTuple Column1, Row1, Row2, Column2;
                    HOperatorSet.RegionFeatures(ConRegion, "row1", out Row1);
                    HOperatorSet.RegionFeatures(ConRegion, "column1", out Column1);
                    HOperatorSet.RegionFeatures(ConRegion, "row2", out Row2);
                    HOperatorSet.RegionFeatures(ConRegion, "column2", out Column2);
                    HOperatorSet.CropRectangle1(InspImage, out CropImgList, Row1, Column1, Row2, Column2);
                    HOperatorSet.EquHistoImage(CropImgList, out HistoEqImg);
                    HOperatorSet.GrayOpeningRect(HistoEqImg, out open, HistoEqOpeningHeight, HistoEqOpeningWidth);
                    HOperatorSet.GrayClosingRect(open, out close, HistoEqClosingHeight, HistoEqClosingWidth);
                    HOperatorSet.Threshold(close, out CellDefect, Th, 255);
                    HTuple Count;
                    HOperatorSet.CountObj(CellDefect, out Count);
                    HObject MoveRegion;
                    HOperatorSet.GenEmptyObj(out MoveRegion);
                    for (int i = 0; i < Count; i++)
                    {
                        HObject SelectRegion, Tmp;
                        HOperatorSet.SelectObj(CellDefect, out SelectRegion, i + 1);

                        HOperatorSet.MoveRegion(SelectRegion, out Tmp, Row1[i], Column1[i]);
                        HOperatorSet.ConcatObj(Tmp, MoveRegion, out MoveRegion);
                    }

                    HOperatorSet.RemoveNoiseRegion(MoveRegion, out HistoEqRegion, "n_4");



                    //ConcurrentStack<HObject> resultData = new ConcurrentStack<HObject>();
                    //Parallel.For(1, CountR + 1, i =>
                    //{
                    //    HObject HistoEqImg, open, close;
                    //    HObject CellDefect;
                    //    HObject CropImg;
                    //    HObject ReImg, SelectObj;
                    //    HOperatorSet.SelectObj(ConRegion, out SelectObj, i);
                    //    HTuple Column, Row;
                    //    HOperatorSet.RegionFeatures(SelectObj, "row1", out Row);
                    //    HOperatorSet.RegionFeatures(SelectObj, "column1", out Column);

                    //    HOperatorSet.ReduceDomain(InspImage, SelectObj, out ReImg);
                    //    HOperatorSet.CropDomain(ReImg, out CropImg);
                    //    HOperatorSet.EquHistoImage(CropImg, out HistoEqImg);
                    //    HOperatorSet.GrayOpeningRect(HistoEqImg, out open, HistoEqOpeningHeight, HistoEqOpeningWidth);
                    //    HOperatorSet.GrayClosingRect(open, out close, HistoEqClosingHeight, HistoEqClosingWidth);
                    //    HOperatorSet.Threshold(close, out CellDefect, Th, 255);
                    //    HObject Tmp;
                    //    HOperatorSet.RemoveNoiseRegion(CellDefect, out Tmp, "n_4");
                    //    HObject MoveRegion;
                    //    HOperatorSet.MoveRegion(Tmp, out MoveRegion, Row, Column);
                    //    {
                    //        resultData.Push(MoveRegion);
                    //    }
                    //    CropImg.Dispose();
                    //    Tmp.Dispose();
                    //    ReImg.Dispose();
                    //    open.Dispose();
                    //    close.Dispose();
                    //    HistoEqImg.Dispose();
                    //    SelectObj.Dispose();
                    //});

                    //{
                    //    HObject CellRes;

                    //    while (resultData.TryPop(out CellRes))
                    //    {
                    //        HObject TempRegion;
                    //        HOperatorSet.ConcatObj(CellRes, HistoEqRegion, out TempRegion);
                    //        HistoEqRegion.Dispose();
                    //        HistoEqRegion = TempRegion;

                    //        CellRes.Dispose();
                    //    }
                    //}

                    HOperatorSet.Connection(HistoEqRegion, out HistoEqRegion);
                    //resultData.DisposeAll();
                    InspRegion.Dispose();
                    ErosionRegion.Dispose();
                    ConRegion.Dispose();
                }
            }
            catch
            {
                return HistoEqRegion;
            }

            return HistoEqRegion;
        }

        public bool HistoEqThinInsp(HObject InspImage, HObject InspRegionDark, HObject InspRegionBright, bool InspEnabled, int HistoEdgeSkipWidth, int HistoEdgeSkipHight, int HistoEqOpeningWidth, int HistoEqOpeningHeight, int HistoEqClosingWidth, int HistoEqClosingHeight, int Th, out HObject HistoEqRegion)
        {
            bool Res = false;
            HOperatorSet.GenEmptyObj(out HistoEqRegion);
            try
            {
                if (InspEnabled)
                {
                    HObject ErosionRegion;
                    HObject InspRegion;
                    HOperatorSet.Union2(InspRegionDark, InspRegionBright, out InspRegion);
                    HOperatorSet.ErosionRectangle1(InspRegion, out ErosionRegion, HistoEdgeSkipWidth, HistoEdgeSkipHight);
                    HObject ConRegion;
                    HOperatorSet.Connection(ErosionRegion, out ConRegion);
                    HTuple CountR;
                    HOperatorSet.CountObj(ConRegion, out CountR);


                    ConcurrentStack<HObject> resultData = new ConcurrentStack<HObject>();
                    Parallel.For(1, CountR + 1, i =>
                    {
                        HObject HistoEqImg, open, close;
                        HObject CellDefect;

                        HObject ReImg, SelectObj;
                        HOperatorSet.SelectObj(ConRegion, out SelectObj, i);
                        HOperatorSet.ReduceDomain(InspImage, SelectObj, out ReImg);
                        HOperatorSet.EquHistoImage(ReImg, out HistoEqImg);
                        HOperatorSet.GrayOpeningRect(HistoEqImg, out open, HistoEqOpeningHeight, HistoEqOpeningWidth);
                        HOperatorSet.GrayClosingRect(open, out close, HistoEqClosingHeight, HistoEqClosingWidth);
                        HOperatorSet.Threshold(close, out CellDefect, Th, 255);
                        HObject Tmp;
                        HOperatorSet.RemoveNoiseRegion(CellDefect, out Tmp, "n_48");
                        {
                            resultData.Push(Tmp);
                        }
                        ReImg.Dispose();
                        open.Dispose();
                        close.Dispose();
                        HistoEqImg.Dispose();
                        SelectObj.Dispose();
                    });

                    {
                        HObject CellRes;

                        while (resultData.TryPop(out CellRes))
                        {
                            HObject TempRegion;
                            HOperatorSet.ConcatObj(CellRes, HistoEqRegion, out TempRegion);
                            HistoEqRegion.Dispose();
                            HistoEqRegion = TempRegion;

                            CellRes.Dispose();
                        }
                    }



                    //for (int i = 1; i <= CountR; i++)
                    //{
                    //    HObject HistoEqImg, open, close;
                    //    HObject CellDefect;

                    //    HObject ReImg, SelectObj;
                    //    HOperatorSet.SelectObj(ConRegion, out SelectObj, i);
                    //    HOperatorSet.ReduceDomain(InspImage, SelectObj, out ReImg);
                    //    HOperatorSet.EquHistoImage(ReImg, out HistoEqImg);

                    //    HOperatorSet.GrayOpeningRect(HistoEqImg, out open, HistoEqOpeningWidth, HistoEqOpeningHeight);
                    //    HOperatorSet.GrayClosingRect(open, out close, HistoEqClosingWidth, HistoEqClosingHeight);
                    //    HOperatorSet.Threshold(close, out CellDefect, Th, 255);
                    //    {
                    //        HObject TempRegion;
                    //        HOperatorSet.ConcatObj(CellDefect, HistoEqRegion, out TempRegion);
                    //        HistoEqRegion.Dispose();
                    //        HistoEqRegion = TempRegion;
                    //    }
                    //    ReImg.Dispose();
                    //    open.Dispose();
                    //    close.Dispose();
                    //    HistoEqImg.Dispose();
                    //    SelectObj.Dispose();
                    //}
                    HOperatorSet.Connection(HistoEqRegion, out HistoEqRegion);
                    resultData.DisposeAll();
                    InspRegion.Dispose();
                    ErosionRegion.Dispose();
                    ConRegion.Dispose();
                }
            }
            catch
            {
                return Res;
            }

            Res = true;
            return Res;
        }

        static HObject EmptyFunction()
        {
            HObject Res;
            HOperatorSet.GenEmptyObj(out Res);
            return Res;
        }

        public class ThinParam
        {
            public HObject InspImage;
            public InductanceInspRole Recipe;
            public HObject InspRegion;
            public HObject Thin_Dark;
            public HObject Thin_Bright;

            public ThinParam(HObject pmInspImage, InductanceInspRole pmRecipe, HObject pmInspRegion, HObject pmThin_Dark, HObject pmThin_Bright)
            {
                this.InspImage = pmInspImage;
                this.Recipe = pmRecipe;
                this.InspRegion = pmInspRegion;
                this.Thin_Dark = pmThin_Dark;
                this.Thin_Bright = pmThin_Bright;
            }
        }
        public class clsThinInspResult
        {
            public HObject ThinRes;
            public HObject HoleRes;

            public clsThinInspResult()
            {
                HOperatorSet.GenEmptyObj(out ThinRes);
                HOperatorSet.GenEmptyObj(out HoleRes);
            }
            public clsThinInspResult(HObject pmThinRes, HObject pmHoleRes)
            {
                this.ThinRes = pmThinRes;
                this.HoleRes = pmHoleRes;
            }
        }
        public static clsThinInspResult ThinInspTask(object Obj)
        {
            clsThinInspResult Res = new clsThinInspResult();

            //HObject ThinDefectRegions;
            //HOperatorSet.GenEmptyObj(out ThinDefectRegions);

            #region Convert Obj

            HObject InspImage = ((ThinParam)Obj).InspImage;
            InductanceInspRole Recipe = ((ThinParam)Obj).Recipe;
            HObject InspRegion = ((ThinParam)Obj).InspRegion;
            HObject Thin_Dark = ((ThinParam)Obj).Thin_Dark;
            HObject Thin_Bright = ((ThinParam)Obj).Thin_Bright;

            #endregion

            #region 變數宣告

            if (!Recipe.Param.RisistAOIParam.ThinParam.Enabled)
            {
                return Res;
            }
            HObject ThinTopHatDarkRegion, ThinTopHatBrightRegion;
            HObject TopHatDarkRegion, TopHatBrightRegion;
            HObject ThinScratchBrightRegion, ThinScratchDarkRegion;

            HOperatorSet.GenEmptyObj(out TopHatDarkRegion);
            HOperatorSet.GenEmptyObj(out TopHatBrightRegion);

            HOperatorSet.GenEmptyObj(out ThinTopHatDarkRegion);
            HOperatorSet.GenEmptyObj(out ThinTopHatBrightRegion);

            HOperatorSet.GenEmptyObj(out ThinScratchBrightRegion);
            HOperatorSet.GenEmptyObj(out ThinScratchDarkRegion);

            #endregion

            try
            {
                #region 計算亮暗區域

                HObject Dark_ThRegions, Bright_ThRegions;

                HOperatorSet.GenEmptyObj(out Dark_ThRegions);
                HOperatorSet.GenEmptyObj(out Bright_ThRegions);
                HObject HistoEqRegion;
                HOperatorSet.GenEmptyObj(out HistoEqRegion);

                HTuple hv_Area_Dark = null, hv_Row_Dark = null, hv_Column_Dark = null;
                HOperatorSet.AreaCenter(Thin_Dark, out hv_Area_Dark, out hv_Row_Dark, out hv_Column_Dark);

                HTuple hv_Area_Bright = null, hv_Row_Bright = null, hv_Column_Bright = null;
                HOperatorSet.AreaCenter(Thin_Bright, out hv_Area_Bright, out hv_Row_Bright, out hv_Column_Bright);

                #endregion

                #region HistoEq Task Start

                HistoParam PHisto = new HistoParam(InspImage, Thin_Dark, Thin_Bright,
                                Recipe.Param.RisistAOIParam.ThinParam.HistoEqEnabled,
                                Recipe.Param.RisistAOIParam.ThinParam.HistoEdgeSkipWidth,
                                Recipe.Param.RisistAOIParam.ThinParam.HistoEdgeSkipHeight,
                                Recipe.Param.RisistAOIParam.ThinParam.HistoEqOpeningWidth,
                                Recipe.Param.RisistAOIParam.ThinParam.HistoEqOpeningHeight,
                                Recipe.Param.RisistAOIParam.ThinParam.HistoEqClosingWidth,
                                Recipe.Param.RisistAOIParam.ThinParam.HistoEqClosingHeight,
                                Recipe.Param.RisistAOIParam.ThinParam.HistoEqTh);

                var TaskHisto = new Task<HObject>(HistoEqThinInspTask, PHisto);
                TaskHisto.Start();

                #endregion

                #region 暗區
                if ((int)(new HTuple(hv_Area_Dark.Length)) != 0)
                {
                    #region Mean
                    double DMeanNew;

                    ThinMeanInsp(InspImage, Thin_Dark, Recipe.Param.RisistAOIParam.ThinParam.Dark_Th, Recipe.Param.RisistAOIParam.ThinParam.MeanEnabled, Recipe.Param.RisistAOIParam.ThinParam.EdgeSkipWidth, Recipe.Param.RisistAOIParam.ThinParam.EdgeSkipHeight, out Dark_ThRegions, out DMeanNew);

                    #endregion

                    #region 劃痕

                    ThinScratchParam PThinScratch = new ThinScratchParam(InspImage,
                                    Thin_Dark,
                                    Recipe.TextureInspectionModelDark,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.OpenScratchSizeW,
                                    Recipe.Param.RisistAOIParam.ThinParam.CloseScratchSizeW,
                                    Recipe.Param.RisistAOIParam.ThinParam.OpenScratchSizeH,
                                    Recipe.Param.RisistAOIParam.ThinParam.CloseScratchSizeH,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeSkipWidth,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeSkipHeight,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeExWidth,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeExHeight);

                    var TaskDarkThinScratch = new Task<HObject>(ThinScratchInspTask, PThinScratch);
                    TaskDarkThinScratch.Start();


                    #endregion

                    #region TopHat
                    HoleParam PHole = new HoleParam(InspImage, Thin_Dark, Recipe.Param.RisistAOIParam.ThinParam.TopHatEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.SESizeWidth,
                                    Recipe.Param.RisistAOIParam.ThinParam.SESizeHeight,
                                    Recipe.Param.RisistAOIParam.ThinParam.TopHatEdgeExpandWidth,
                                    Recipe.Param.RisistAOIParam.ThinParam.TopHatEdgeExpandHeight, DMeanNew,
                                    Recipe.Param.RisistAOIParam.ThinParam.ExtH,
                                    Recipe.Param.RisistAOIParam.ThinParam.TopHatDark_Th,
                                    Recipe.Param.RisistAOIParam.ThinParam.ExtThAdd,
                                    Recipe.Param.RisistAOIParam.ThinParam.BrightDetectTh);

                    var TaskDarkHole = new Task<HObject>(HoleTopHatInspTask, PHole);
                    TaskDarkHole.Start();

                    #endregion

                    #region Thin TopHat Big

                    TopHatParam PTopHat = new TopHatParam(InspImage, Thin_Dark,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatEnabled,
                                   Recipe.Param.SegParam.ThinInspWidrh,
                                   Recipe.Param.SegParam.ThinInspHeight,
                                   DMeanNew,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatEdgeSkipWidth,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatEdgeSkipHeight,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatDarkTh,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatOpenWidth,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatOpenHeight,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatCloseWidth,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatCloseHeight);

                    var TaskDarkTopHat = new Task<HObject>(ThinTopHatInspTask, PTopHat);
                    TaskDarkTopHat.Start();

                    #endregion

                    ThinTopHatDarkRegion = TaskDarkTopHat.Result;
                    TopHatDarkRegion = TaskDarkHole.Result;
                    ThinScratchDarkRegion = TaskDarkThinScratch.Result;

                    TaskDarkTopHat.Dispose();
                    TaskDarkHole.Dispose();
                    TaskDarkThinScratch.Dispose();
                }

                #endregion

                #region 亮區

                if ((int)(new HTuple(hv_Area_Bright.Length)) != 0)
                {
                    #region Mean
                    double BMeanNew;

                    ThinMeanInsp(InspImage, Thin_Bright, Recipe.Param.RisistAOIParam.ThinParam.Bright_Th, Recipe.Param.RisistAOIParam.ThinParam.MeanEnabled, Recipe.Param.RisistAOIParam.ThinParam.EdgeSkipWidth, Recipe.Param.RisistAOIParam.ThinParam.EdgeSkipHeight, out Bright_ThRegions, out BMeanNew);

                    #endregion

                    #region 劃痕

                    ThinScratchBrightRegion.Dispose();
                    ThinScratchParam BPThinScratch = new ThinScratchParam(InspImage,
                                    Thin_Bright,
                                    Recipe.TextureInspectionModel,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.OpenScratchSizeW,
                                    Recipe.Param.RisistAOIParam.ThinParam.CloseScratchSizeW,
                                    Recipe.Param.RisistAOIParam.ThinParam.OpenScratchSizeH,
                                    Recipe.Param.RisistAOIParam.ThinParam.CloseScratchSizeH,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeSkipWidth,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeSkipHeight,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeExWidth,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeExHeight);

                    var TaskBrightThinScratch = new Task<HObject>(ThinScratchInspTask, BPThinScratch);
                    TaskBrightThinScratch.Start();

                    #endregion

                    #region TopHat

                    HoleParam BPHole = new HoleParam(InspImage, Thin_Bright, Recipe.Param.RisistAOIParam.ThinParam.TopHatEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.SESizeWidth,
                                    Recipe.Param.RisistAOIParam.ThinParam.SESizeHeight,
                                    Recipe.Param.RisistAOIParam.ThinParam.TopHatEdgeExpandWidth,
                                    Recipe.Param.RisistAOIParam.ThinParam.TopHatEdgeExpandHeight, BMeanNew,
                                    Recipe.Param.RisistAOIParam.ThinParam.ExtH,
                                    Recipe.Param.RisistAOIParam.ThinParam.TopHatBright_Th,
                                    Recipe.Param.RisistAOIParam.ThinParam.ExtThAdd,
                                    Recipe.Param.RisistAOIParam.ThinParam.BrightDetectTh);

                    var TaskBrightHole = new Task<HObject>(HoleTopHatInspTask, BPHole);
                    TaskBrightHole.Start();

                    #endregion

                    #region Thin TopHat Big

                    TopHatParam BPTopHat = new TopHatParam(InspImage, Thin_Bright,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatEnabled,
                                   Recipe.Param.SegParam.ThinInspWidrh,
                                   Recipe.Param.SegParam.ThinInspHeight,
                                   BMeanNew,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatEdgeSkipWidth,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatEdgeSkipHeight,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatBrightTh,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatOpenWidth,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatOpenHeight,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatCloseWidth,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatCloseHeight);

                    var TaskBrightTopHat = new Task<HObject>(ThinTopHatInspTask, BPTopHat);
                    TaskBrightTopHat.Start();

                    #endregion

                    ThinTopHatBrightRegion = TaskBrightTopHat.Result;
                    TopHatBrightRegion = TaskBrightHole.Result;
                    ThinScratchBrightRegion = TaskBrightThinScratch.Result;

                    TaskBrightTopHat.Dispose();
                    TaskBrightHole.Dispose();
                    TaskBrightThinScratch.Dispose();

                }

                #endregion

                #region 組合亮區 & 暗區
                {
                    HObject TmpRegions;
                    HOperatorSet.Union2(Dark_ThRegions, Bright_ThRegions, out TmpRegions);
                    Res.ThinRes.Dispose();
                    Res.ThinRes = TmpRegions;
                }
                HObject TopHatDefectRegions;
                HOperatorSet.GenEmptyObj(out TopHatDefectRegions);
                {
                    HObject TmpRegions;
                    HOperatorSet.Union2(TopHatDarkRegion, TopHatBrightRegion, out TmpRegions);
                    TopHatDefectRegions.Dispose();
                    TopHatDefectRegions = TmpRegions;
                }
                HObject ThinHatDefectRegions;
                HOperatorSet.GenEmptyObj(out ThinHatDefectRegions);
                {
                    HObject TmpRegions, Tmp2;
                    HOperatorSet.Union2(ThinTopHatDarkRegion, ThinTopHatBrightRegion, out TmpRegions);
                    HOperatorSet.Connection(TmpRegions, out Tmp2);
                    ThinHatDefectRegions.Dispose();
                    ThinHatDefectRegions = Tmp2;
                }
                HObject ThinScratchDefectRegion;
                HOperatorSet.GenEmptyObj(out ThinScratchDefectRegion);
                {
                    HObject TmpRegions, Tmp2;
                    HOperatorSet.Union2(ThinScratchDarkRegion, ThinScratchBrightRegion, out TmpRegions);
                    HOperatorSet.Connection(TmpRegions, out Tmp2);
                    ThinScratchDefectRegion.Dispose();
                    ThinScratchDefectRegion = Tmp2;
                }
                #endregion

                #region HistoEq

                HistoEqRegion = TaskHisto.Result;
                TaskHisto.Dispose();

                #endregion

                #region 條件篩選
                HObject DefectReg, TophatReg;
                HObject OpeningRegion;
                HOperatorSet.OpeningRectangle1(Res.ThinRes, out OpeningRegion, Recipe.Param.RisistAOIParam.ThinParam.MeanOpenRad, Recipe.Param.RisistAOIParam.ThinParam.MeanOpenRad);
                Res.ThinRes.Dispose();
                HOperatorSet.ClosingRectangle1(OpeningRegion, out Res.ThinRes, Recipe.Param.RisistAOIParam.ThinParam.MeanCloseRad, Recipe.Param.RisistAOIParam.ThinParam.MeanCloseRad);
                HOperatorSet.Connection(Res.ThinRes, out DefectReg);

                HOperatorSet.OpeningRectangle1(TopHatDefectRegions, out TopHatDefectRegions, 2, 2);
                HOperatorSet.ClosingRectangle1(TopHatDefectRegions, out TopHatDefectRegions, 3, 3);
                HOperatorSet.Connection(TopHatDefectRegions, out TophatReg);

                HObject BigAreaDefect;
                double MaxW = 2500000 * Locate_Method_FS.pixel_resolution_;
                double MaxA = 2500000 * (Locate_Method_FS.pixel_resolution_ * Locate_Method_FS.pixel_resolution_);


                SelectDefectRegions(DefectReg,
                                    Recipe.Param.RisistAOIParam.ThinParam.AreaEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.WidthEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.HeightEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.AreaMin,
                                    Recipe.Param.RisistAOIParam.ThinParam.WidthMin,
                                    Recipe.Param.RisistAOIParam.ThinParam.HeightMin,
                                    (int)MaxA, (int)MaxW, (int)MaxW, Locate_Method_FS.pixel_resolution_,
                                    out BigAreaDefect);

                SelectDefectRegions(TophatReg,
                                    Recipe.Param.RisistAOIParam.ThinParam.TopHatAreaEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.TopHatWidthEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.TopHatHeightEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.TopHatAreaMin,
                                    Recipe.Param.RisistAOIParam.ThinParam.TopHatWidthMin,
                                    Recipe.Param.RisistAOIParam.ThinParam.TopHatHeightMin,
                                    (int)MaxA, (int)MaxW, (int)MaxW, Locate_Method_FS.pixel_resolution_,
                                    out TopHatDefectRegions);

                HObject HistoEqDefectRegion;
                SelectDefectRegions(HistoEqRegion,
                                    Recipe.Param.RisistAOIParam.ThinParam.HistoEqAreaEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.HistoEqWidth,
                                    Recipe.Param.RisistAOIParam.ThinParam.HistoEqHeightEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.HistoEqAreaMin,
                                    Recipe.Param.RisistAOIParam.ThinParam.HistoEqWidthMin,
                                    Recipe.Param.RisistAOIParam.ThinParam.HistoEqHeightMin,
                                    (int)MaxA, (int)MaxW, (int)MaxW, Locate_Method_FS.pixel_resolution_,
                                    out HistoEqDefectRegion);

                HObject ThinHatRegion;
                SelectDefectRegions(ThinHatDefectRegions,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinHatAreaEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinHatWidthEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinHatHeightEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinHatAreaMin,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinHatWidthMin,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinHatHeightMin,
                                    (int)MaxA, (int)MaxW, (int)MaxW, Locate_Method_FS.pixel_resolution_,
                                    out ThinHatRegion);


                HObject ThinScratchRegion;
                SelectDefectRegions(ThinScratchDefectRegion,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchAreaEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchWidthEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchHeightEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchAreaMin,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchWidthMin,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchHeightMin,
                                    (int)MaxA, (int)MaxW, (int)MaxW, Locate_Method_FS.pixel_resolution_,
                                    out ThinScratchRegion);


                Res.HoleRes = TopHatDefectRegions;

                HObject TEMP;
                Res.ThinRes.Dispose();
                HOperatorSet.Union2(HistoEqDefectRegion, BigAreaDefect, out TEMP);
                HOperatorSet.Union2(TEMP, ThinHatRegion, out Res.ThinRes);
                HOperatorSet.Union2(ThinScratchRegion, Res.ThinRes, out Res.ThinRes);

                TEMP.Dispose();

                #endregion


                HObject SumArea;
                if (Recipe.Param.RisistAOIParam.ThinParam.DefectSumAreaEnabled)
                {
                    SumAreaDefect(InspRegion, Recipe, Res.ThinRes, out SumArea);
                }
                else
                    SumArea = Res.ThinRes.Clone();

                Res.ThinRes.Dispose();
                Res.ThinRes = SumArea;

                DefectReg.Dispose();
                TophatReg.Dispose();
                OpeningRegion.Dispose();
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
                return Res;
            }


            TopHatDarkRegion.Dispose();
            TopHatBrightRegion.Dispose();
            ThinTopHatDarkRegion.Dispose();
            ThinTopHatBrightRegion.Dispose();
            return Res;
        }

        public bool ThinInsp(HObject InspImage, InductanceInspRole Recipe, HObject InspRegion, HObject Thin_Dark, HObject Thin_Bright, out HObject ThinDefectRegions, out HObject ThinMapRegion)
        {
            bool Res = false;

            #region 變數宣告
            HOperatorSet.GenEmptyObj(out ThinMapRegion);
            HOperatorSet.GenEmptyObj(out ThinDefectRegions);
            if (!Recipe.Param.RisistAOIParam.ThinParam.Enabled)
            {
                Res = true;
                return Res;
            }
            HObject ThinTopHatDarkRegion, ThinTopHatBrightRegion;
            HObject TopHatDarkRegion, TopHatBrightRegion;
            HObject ThinScratchBrightRegion, ThinScratchDarkRegion;

            HOperatorSet.GenEmptyObj(out TopHatDarkRegion);
            HOperatorSet.GenEmptyObj(out TopHatBrightRegion);

            HOperatorSet.GenEmptyObj(out ThinTopHatDarkRegion);
            HOperatorSet.GenEmptyObj(out ThinTopHatBrightRegion);

            HOperatorSet.GenEmptyObj(out ThinScratchBrightRegion);
            HOperatorSet.GenEmptyObj(out ThinScratchDarkRegion);

            #endregion

            try
            {
                #region 計算亮暗區域

                //HOperatorSet.Union1(Thin_Bright, out Thin_Bright);
                //HOperatorSet.Union1(Thin_Dark, out Thin_Dark);

                HObject Dark_ThRegions, Bright_ThRegions;

                HOperatorSet.GenEmptyObj(out Dark_ThRegions);
                HOperatorSet.GenEmptyObj(out Bright_ThRegions);
                HObject HistoEqRegion;
                HOperatorSet.GenEmptyObj(out HistoEqRegion);

                HTuple hv_Area_Dark = null, hv_Row_Dark = null, hv_Column_Dark = null;
                HOperatorSet.AreaCenter(Thin_Dark, out hv_Area_Dark, out hv_Row_Dark, out hv_Column_Dark);

                HTuple hv_Area_Bright = null, hv_Row_Bright = null, hv_Column_Bright = null;
                HOperatorSet.AreaCenter(Thin_Bright, out hv_Area_Bright, out hv_Row_Bright, out hv_Column_Bright);

                #endregion

                #region HistoEq Task Start

                HistoParam PHisto = new HistoParam(InspImage, Thin_Dark, Thin_Bright,
                                Recipe.Param.RisistAOIParam.ThinParam.HistoEqEnabled,
                                Recipe.Param.RisistAOIParam.ThinParam.HistoEdgeSkipWidth,
                                Recipe.Param.RisistAOIParam.ThinParam.HistoEdgeSkipHeight,
                                Recipe.Param.RisistAOIParam.ThinParam.HistoEqOpeningWidth,
                                Recipe.Param.RisistAOIParam.ThinParam.HistoEqOpeningHeight,
                                Recipe.Param.RisistAOIParam.ThinParam.HistoEqClosingWidth,
                                Recipe.Param.RisistAOIParam.ThinParam.HistoEqClosingHeight,
                                Recipe.Param.RisistAOIParam.ThinParam.HistoEqTh);

                var TaskHisto = new Task<HObject>(HistoEqThinInspTask, PHisto);
                TaskHisto.Start();

                #endregion

                #region 暗區
                if ((int)(new HTuple(hv_Area_Dark.Length)) != 0)
                {
                    #region Mean
                    double DMeanNew;

                    ThinMeanInsp(InspImage, Thin_Dark, Recipe.Param.RisistAOIParam.ThinParam.Dark_Th, Recipe.Param.RisistAOIParam.ThinParam.MeanEnabled, Recipe.Param.RisistAOIParam.ThinParam.EdgeSkipWidth, Recipe.Param.RisistAOIParam.ThinParam.EdgeSkipHeight, out Dark_ThRegions, out DMeanNew);

                    #endregion

                    #region 劃痕

                    ThinScratchParam PThinScratch = new ThinScratchParam(InspImage,
                                    Thin_Dark,
                                    Recipe.TextureInspectionModelDark,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.OpenScratchSizeW,
                                    Recipe.Param.RisistAOIParam.ThinParam.CloseScratchSizeW,
                                    Recipe.Param.RisistAOIParam.ThinParam.OpenScratchSizeH,
                                    Recipe.Param.RisistAOIParam.ThinParam.CloseScratchSizeH,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeSkipWidth,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeSkipHeight,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeExWidth,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeExHeight);

                    var TaskDarkThinScratch = new Task<HObject>(ThinScratchInspTask, PThinScratch);
                    TaskDarkThinScratch.Start();


                    #endregion

                    #region TopHat
                    HoleParam PHole = new HoleParam(InspImage, Thin_Dark, Recipe.Param.RisistAOIParam.ThinParam.TopHatEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.SESizeWidth,
                                    Recipe.Param.RisistAOIParam.ThinParam.SESizeHeight,
                                    Recipe.Param.RisistAOIParam.ThinParam.TopHatEdgeExpandWidth,
                                    Recipe.Param.RisistAOIParam.ThinParam.TopHatEdgeExpandHeight, DMeanNew,
                                    Recipe.Param.RisistAOIParam.ThinParam.ExtH,
                                    Recipe.Param.RisistAOIParam.ThinParam.TopHatDark_Th,
                                    Recipe.Param.RisistAOIParam.ThinParam.ExtThAdd,
                                    Recipe.Param.RisistAOIParam.ThinParam.BrightDetectTh);

                    var TaskDarkHole = new Task<HObject>(HoleTopHatInspTask, PHole);
                    TaskDarkHole.Start();

                    #endregion

                    #region Thin TopHat Big

                    TopHatParam PTopHat = new TopHatParam(InspImage, Thin_Dark,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatEnabled,
                                   Recipe.Param.SegParam.ThinInspWidrh,
                                   Recipe.Param.SegParam.ThinInspHeight,
                                   DMeanNew,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatEdgeSkipWidth,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatEdgeSkipHeight,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatDarkTh,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatOpenWidth,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatOpenHeight,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatCloseWidth,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatCloseHeight);

                    var TaskDarkTopHat = new Task<HObject>(ThinTopHatInspTask, PTopHat);
                    TaskDarkTopHat.Start();

                    #endregion

                    ThinTopHatDarkRegion = TaskDarkTopHat.Result;
                    TopHatDarkRegion = TaskDarkHole.Result;
                    ThinScratchDarkRegion = TaskDarkThinScratch.Result;

                    TaskDarkTopHat.Dispose();
                    TaskDarkHole.Dispose();
                    TaskDarkThinScratch.Dispose();
                }

                #endregion

                #region 亮區

                if ((int)(new HTuple(hv_Area_Bright.Length)) != 0)
                {
                    #region Mean
                    double BMeanNew;

                    ThinMeanInsp(InspImage, Thin_Bright, Recipe.Param.RisistAOIParam.ThinParam.Bright_Th, Recipe.Param.RisistAOIParam.ThinParam.MeanEnabled, Recipe.Param.RisistAOIParam.ThinParam.EdgeSkipWidth, Recipe.Param.RisistAOIParam.ThinParam.EdgeSkipHeight, out Bright_ThRegions, out BMeanNew);

                    #endregion

                    #region 劃痕

                    ThinScratchBrightRegion.Dispose();
                    ThinScratchParam BPThinScratch = new ThinScratchParam(InspImage,
                                    Thin_Bright,
                                    Recipe.TextureInspectionModel,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.OpenScratchSizeW,
                                    Recipe.Param.RisistAOIParam.ThinParam.CloseScratchSizeW,
                                    Recipe.Param.RisistAOIParam.ThinParam.OpenScratchSizeH,
                                    Recipe.Param.RisistAOIParam.ThinParam.CloseScratchSizeH,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeSkipWidth,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeSkipHeight,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeExWidth,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchEdgeExHeight);

                    var TaskBrightThinScratch = new Task<HObject>(ThinScratchInspTask, BPThinScratch);
                    TaskBrightThinScratch.Start();

                    #endregion

                    #region TopHat

                    HoleParam BPHole = new HoleParam(InspImage, Thin_Bright, Recipe.Param.RisistAOIParam.ThinParam.TopHatEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.SESizeWidth,
                                    Recipe.Param.RisistAOIParam.ThinParam.SESizeHeight,
                                    Recipe.Param.RisistAOIParam.ThinParam.TopHatEdgeExpandWidth,
                                    Recipe.Param.RisistAOIParam.ThinParam.TopHatEdgeExpandHeight, BMeanNew,
                                    Recipe.Param.RisistAOIParam.ThinParam.ExtH,
                                    Recipe.Param.RisistAOIParam.ThinParam.TopHatBright_Th,
                                    Recipe.Param.RisistAOIParam.ThinParam.ExtThAdd,
                                    Recipe.Param.RisistAOIParam.ThinParam.BrightDetectTh);

                    var TaskBrightHole = new Task<HObject>(HoleTopHatInspTask, BPHole);
                    TaskBrightHole.Start();

                    #endregion

                    #region Thin TopHat Big

                    TopHatParam BPTopHat = new TopHatParam(InspImage, Thin_Bright,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatEnabled,
                                   Recipe.Param.SegParam.ThinInspWidrh,
                                   Recipe.Param.SegParam.ThinInspHeight,
                                   BMeanNew,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatEdgeSkipWidth,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatEdgeSkipHeight,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatBrightTh,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatOpenWidth,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatOpenHeight,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatCloseWidth,
                                   Recipe.Param.RisistAOIParam.ThinParam.ThinHatCloseHeight);

                    var TaskBrightTopHat = new Task<HObject>(ThinTopHatInspTask, BPTopHat);
                    TaskBrightTopHat.Start();

                    #endregion

                    ThinTopHatBrightRegion = TaskBrightTopHat.Result;
                    TopHatBrightRegion = TaskBrightHole.Result;
                    ThinScratchBrightRegion = TaskBrightThinScratch.Result;

                    TaskBrightTopHat.Dispose();
                    TaskBrightHole.Dispose();
                    TaskBrightThinScratch.Dispose();

                }

                #endregion

                #region 組合亮區 & 暗區
                {
                    HObject TmpRegions;
                    HOperatorSet.Union2(Dark_ThRegions, Bright_ThRegions, out TmpRegions);
                    ThinDefectRegions.Dispose();
                    ThinDefectRegions = TmpRegions;
                }
                HObject TopHatDefectRegions;
                HOperatorSet.GenEmptyObj(out TopHatDefectRegions);
                {
                    HObject TmpRegions;
                    HOperatorSet.Union2(TopHatDarkRegion, TopHatBrightRegion, out TmpRegions);
                    TopHatDefectRegions.Dispose();
                    TopHatDefectRegions = TmpRegions;
                }
                HObject ThinHatDefectRegions;
                HOperatorSet.GenEmptyObj(out ThinHatDefectRegions);
                {
                    HObject TmpRegions, Tmp2;
                    HOperatorSet.Union2(ThinTopHatDarkRegion, ThinTopHatBrightRegion, out TmpRegions);
                    HOperatorSet.Connection(TmpRegions, out Tmp2);
                    ThinHatDefectRegions.Dispose();
                    ThinHatDefectRegions = Tmp2;
                }
                HObject ThinScratchDefectRegion;
                HOperatorSet.GenEmptyObj(out ThinScratchDefectRegion);
                {
                    HObject TmpRegions, Tmp2;
                    HOperatorSet.Union2(ThinScratchDarkRegion, ThinScratchBrightRegion, out TmpRegions);
                    HOperatorSet.Connection(TmpRegions, out Tmp2);
                    ThinScratchDefectRegion.Dispose();
                    ThinScratchDefectRegion = Tmp2;
                }
                #endregion

                #region HistoEq

                HistoEqRegion = TaskHisto.Result;
                TaskHisto.Dispose();

                #endregion

                #region 條件篩選
                HObject DefectReg, TophatReg;
                HObject OpeningRegion;
                HOperatorSet.OpeningRectangle1(ThinDefectRegions, out OpeningRegion, Recipe.Param.RisistAOIParam.ThinParam.MeanOpenRad, Recipe.Param.RisistAOIParam.ThinParam.MeanOpenRad);
                ThinDefectRegions.Dispose();
                HOperatorSet.ClosingRectangle1(OpeningRegion, out ThinDefectRegions, Recipe.Param.RisistAOIParam.ThinParam.MeanCloseRad, Recipe.Param.RisistAOIParam.ThinParam.MeanCloseRad);
                HOperatorSet.Connection(ThinDefectRegions, out DefectReg);

                HOperatorSet.OpeningRectangle1(TopHatDefectRegions, out TopHatDefectRegions, 2, 2);
                HOperatorSet.ClosingRectangle1(TopHatDefectRegions, out TopHatDefectRegions, 3, 3);
                HOperatorSet.Connection(TopHatDefectRegions, out TophatReg);

                HObject BigAreaDefect;
                double MaxW = 2500000 * Locate_Method_FS.pixel_resolution_;
                double MaxA = 2500000 * (Locate_Method_FS.pixel_resolution_ * Locate_Method_FS.pixel_resolution_);


                SelectDefectRegions(DefectReg,
                                    Recipe.Param.RisistAOIParam.ThinParam.AreaEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.WidthEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.HeightEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.AreaMin,
                                    Recipe.Param.RisistAOIParam.ThinParam.WidthMin,
                                    Recipe.Param.RisistAOIParam.ThinParam.HeightMin,
                                    (int)MaxA, (int)MaxW, (int)MaxW, Locate_Method_FS.pixel_resolution_,
                                    out BigAreaDefect);

                SelectDefectRegions(TophatReg,
                                    Recipe.Param.RisistAOIParam.ThinParam.TopHatAreaEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.TopHatWidthEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.TopHatHeightEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.TopHatAreaMin,
                                    Recipe.Param.RisistAOIParam.ThinParam.TopHatWidthMin,
                                    Recipe.Param.RisistAOIParam.ThinParam.TopHatHeightMin,
                                    (int)MaxA, (int)MaxW, (int)MaxW, Locate_Method_FS.pixel_resolution_,
                                    out TopHatDefectRegions);

                HObject HistoEqDefectRegion;
                SelectDefectRegions(HistoEqRegion,
                                    Recipe.Param.RisistAOIParam.ThinParam.HistoEqAreaEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.HistoEqWidth,
                                    Recipe.Param.RisistAOIParam.ThinParam.HistoEqHeightEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.HistoEqAreaMin,
                                    Recipe.Param.RisistAOIParam.ThinParam.HistoEqWidthMin,
                                    Recipe.Param.RisistAOIParam.ThinParam.HistoEqHeightMin,
                                    (int)MaxA, (int)MaxW, (int)MaxW, Locate_Method_FS.pixel_resolution_,
                                    out HistoEqDefectRegion);

                HObject ThinHatRegion;
                SelectDefectRegions(ThinHatDefectRegions,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinHatAreaEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinHatWidthEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinHatHeightEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinHatAreaMin,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinHatWidthMin,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinHatHeightMin,
                                    (int)MaxA, (int)MaxW, (int)MaxW, Locate_Method_FS.pixel_resolution_,
                                    out ThinHatRegion);


                HObject ThinScratchRegion;
                SelectDefectRegions(ThinScratchDefectRegion,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchAreaEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchWidthEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchHeightEnabled,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchAreaMin,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchWidthMin,
                                    Recipe.Param.RisistAOIParam.ThinParam.ThinScratchHeightMin,
                                    (int)MaxA, (int)MaxW, (int)MaxW, Locate_Method_FS.pixel_resolution_,
                                    out ThinScratchRegion);
                HObject TEMP;
                ThinDefectRegions.Dispose();
                HOperatorSet.Union2(TopHatDefectRegions, BigAreaDefect, out TEMP);
                HOperatorSet.Union2(TEMP, HistoEqDefectRegion, out ThinDefectRegions);
                TEMP.Dispose();
                HOperatorSet.Union2(ThinHatRegion, ThinDefectRegions, out TEMP);
                HOperatorSet.Union2(TEMP, ThinScratchRegion, out ThinDefectRegions);
                TEMP.Dispose();

                #endregion


                HObject SumArea;
                if (Recipe.Param.RisistAOIParam.ThinParam.DefectSumAreaEnabled)
                {
                    SumDefectArea(InspRegion, Recipe, ThinDefectRegions, out SumArea);
                }
                else
                    SumArea = ThinDefectRegions.Clone();

                LocalDefectMappping(SumArea, InspRegion, out ThinMapRegion);
                ThinDefectRegions.Dispose();
                ThinDefectRegions = SumArea;

                DefectReg.Dispose();
                TophatReg.Dispose();
                OpeningRegion.Dispose();
            }
            catch (Exception e)
            {
                return Res;
            }


            TopHatDarkRegion.Dispose();
            TopHatBrightRegion.Dispose();
            ThinTopHatDarkRegion.Dispose();
            ThinTopHatBrightRegion.Dispose();

            Res = true;
            return Res;
        }

        static void CalcMean(HTuple Mean, HTuple Dev, HObject InspRegion, HObject InspImg, out double MeanNew)
        {
            //取中間50%
            double Max = Mean + 0.674 * Dev;
            double Min = Mean - 0.674 * Dev;
            HTuple HistoAbs;

            HOperatorSet.GrayHistoAbs(InspRegion, InspImg, 1.0, out HistoAbs);

            double Sum = 0.0;
            double CountGray = 0.0;
            for (int i = 0; i < HistoAbs.Length; i++)
            {
                if (i > Min)
                {
                    if (i < Max)
                    {
                        Sum = Sum + HistoAbs[i] * i;
                        CountGray = CountGray + HistoAbs[i];
                    }
                    else
                    {
                        break;
                    }
                }
            }
            MeanNew = Sum / CountGray;
        }

        static void SumAreaDefect(HObject InspRegion, InductanceInspRole Recipe, HObject DefectRegions, out HObject SelectDefectRegions)
        {

            HOperatorSet.GenEmptyObj(out SelectDefectRegions);
            HTuple InspRegionCount;
            HObject CellDefectRegions;
            HOperatorSet.GenEmptyObj(out CellDefectRegions);
            HOperatorSet.CountObj(InspRegion, out InspRegionCount);
            HObject CellDefectRegionsUnion;
            HOperatorSet.GenEmptyObj(out CellDefectRegionsUnion);

            for (int i = 1; i < InspRegionCount + 1; i++)
            {
                HObject SelectInspRegion;
                CellDefectRegions.Dispose();
                CellDefectRegionsUnion.Dispose();
                HOperatorSet.SelectObj(InspRegion, out SelectInspRegion, i);
                HOperatorSet.Intersection(SelectInspRegion, DefectRegions, out CellDefectRegions);
                HOperatorSet.Union1(CellDefectRegions, out CellDefectRegionsUnion);
                HTuple Area, Row, Column;
                HOperatorSet.AreaCenter(CellDefectRegionsUnion, out Area, out Row, out Column);

                if (Area.I >= Recipe.Param.RisistAOIParam.ThinParam.DefectSumArea / (Locate_Method_FS.pixel_resolution_ * Locate_Method_FS.pixel_resolution_))
                {
                    HObject TmpRegion;
                    HOperatorSet.ConcatObj(SelectDefectRegions, CellDefectRegionsUnion, out TmpRegion);
                    SelectDefectRegions.Dispose();
                    SelectDefectRegions = TmpRegion;
                }
            }

        }

        public void SumDefectArea(HObject InspRegion, InductanceInspRole Recipe, HObject DefectRegions, out HObject SelectDefectRegions)
        {

            HOperatorSet.GenEmptyObj(out SelectDefectRegions);
            HTuple InspRegionCount;
            HObject CellDefectRegions;
            HOperatorSet.GenEmptyObj(out CellDefectRegions);
            HOperatorSet.CountObj(InspRegion, out InspRegionCount);
            HObject CellDefectRegionsUnion;
            HOperatorSet.GenEmptyObj(out CellDefectRegionsUnion);

            for (int i = 1; i < InspRegionCount + 1; i++)
            {
                HObject SelectInspRegion;
                CellDefectRegions.Dispose();
                CellDefectRegionsUnion.Dispose();
                HOperatorSet.SelectObj(InspRegion, out SelectInspRegion, i);
                HOperatorSet.Intersection(SelectInspRegion, DefectRegions, out CellDefectRegions);
                HOperatorSet.Union1(CellDefectRegions, out CellDefectRegionsUnion);
                HTuple Area, Row, Column;
                HOperatorSet.AreaCenter(CellDefectRegionsUnion, out Area, out Row, out Column);

                if (Area.I >= Recipe.Param.RisistAOIParam.ThinParam.DefectSumArea / (Locate_Method_FS.pixel_resolution_ * Locate_Method_FS.pixel_resolution_))
                {
                    HObject TmpRegion;
                    HOperatorSet.ConcatObj(SelectDefectRegions, CellDefectRegionsUnion, out TmpRegion);
                    SelectDefectRegions.Dispose();
                    SelectDefectRegions = TmpRegion;
                }
            }
            //HOperatorSet.GenEmptyObj(out SelectDefectRegions);
            //HTuple InspRegionCount;
            //HOperatorSet.CountObj(InspRegion, out InspRegionCount);
            //for (int i = 1; i < InspRegionCount + 1; i++)
            //{
            //    HObject SelectInspRegion;
            //    HOperatorSet.SelectObj(InspRegion, out SelectInspRegion, i);
            //    HObject CellDefectRegions;
            //    HObject UnionRegion;
            //    //HObject DefectUnion;
            //    //HOperatorSet.Union1(DefectRegions, out DefectUnion);
            //    HOperatorSet.Intersection(SelectInspRegion, DefectRegions, out CellDefectRegions);

            //    HOperatorSet.Union1(CellDefectRegions, out UnionRegion);
            //    HTuple Area, Row, Column;
            //    HOperatorSet.AreaCenter(UnionRegion, out Area, out Row, out Column);

            //    if (Area.I >= Recipe.Param.RisistAOIParam.ThinParam.DefectSumArea / (Recipe.Param.Resolution * Recipe.Param.Resolution))
            //    {
            //        HObject TmpRegion;
            //        HOperatorSet.ConcatObj(SelectDefectRegions, UnionRegion, out TmpRegion);
            //        SelectDefectRegions.Dispose();
            //        SelectDefectRegions = TmpRegion;
            //    }
            //}
        }

        public void PartitionRegions(List<HObject> ImageList, InductanceInspRole Recipe,
                                     HObject ThinRegion, HObject ScratchRegion, HObject StainRegion,
                                     out HObject Thin_Dark, out HObject Thin_Bright,
                                     out HObject RDefect_Dark, out HObject RDefect_Bright,
                                     out HObject Stain_Dark, out HObject Stain_Bright, out HObject RShift_Dark, out HObject RShift_Bright)
        {

            HOperatorSet.GenEmptyObj(out Thin_Dark);
            HOperatorSet.GenEmptyObj(out Thin_Bright);

            HOperatorSet.GenEmptyObj(out RDefect_Dark);
            HOperatorSet.GenEmptyObj(out RDefect_Bright);

            HOperatorSet.GenEmptyObj(out Stain_Dark);
            HOperatorSet.GenEmptyObj(out Stain_Bright);

            HOperatorSet.GenEmptyObj(out RShift_Dark);
            HOperatorSet.GenEmptyObj(out RShift_Bright);

            HTuple numSeg;
            HObject ThinInspRegions, ScratchInspRegions, StainInspRegions;

            HOperatorSet.Connection(ScratchRegion, out ScratchInspRegions);
            HOperatorSet.Connection(ThinRegion, out ThinInspRegions);
            HOperatorSet.Connection(StainRegion, out StainInspRegions);
            HOperatorSet.CountObj(ThinInspRegions, out numSeg);

            #region R Defect

            HObject RDefectRegion1, RDefectTotalRegion;
            HOperatorSet.Difference(ScratchInspRegions, StainInspRegions, out RDefectRegion1);
            int DiffY = (int)Math.Abs(Recipe.Param.SegParam.StainHotspotY - Recipe.Param.SegParam.ScratchHotspotY) + 10;
            HOperatorSet.ErosionRectangle1(RDefectRegion1, out RDefectTotalRegion, 1, (Recipe.Param.SegParam.ScratchInspHeight - Recipe.Param.SegParam.StainInspHeight) / 2 + DiffY);

            #endregion

            HTuple hv_Mean = null;
            HOperatorSet.GrayFeatures(ThinInspRegions, ImageList[Recipe.Param.RisistAOIParam.ThinParam.InspID], "mean", out hv_Mean);


            HTuple GrayValue = Recipe.Param.SegParam.PartitionGrayValue;
            HTuple SubTuple = hv_Mean - GrayValue;
            HTuple SgnTuple;
            HOperatorSet.TupleSgn(SubTuple, out SgnTuple);

            HTuple DarkIndex, BrightIndex, EqIndex;
            HOperatorSet.TupleFind(SgnTuple, 1, out BrightIndex);
            HOperatorSet.TupleFind(SgnTuple, -1, out DarkIndex);
            HOperatorSet.TupleFind(SgnTuple, 0, out EqIndex);

            #region 暗區

            if (DarkIndex.Length > 0 && DarkIndex >= 0)
            {
                HOperatorSet.SelectObj(ThinInspRegions, out Thin_Dark, DarkIndex + 1);

                HOperatorSet.SelectObj(ScratchInspRegions, out RShift_Dark, DarkIndex + 1);

                HOperatorSet.SelectObj(StainInspRegions, out Stain_Dark, DarkIndex + 1);

                HOperatorSet.SelectObj(RDefectTotalRegion, out RDefect_Dark, DarkIndex + 1);
            }

            #endregion

            #region 亮區

            if (BrightIndex.Length > 0 && BrightIndex >= 0)
            {
                HOperatorSet.SelectObj(ThinInspRegions, out Thin_Bright, BrightIndex + 1);

                HOperatorSet.SelectObj(ScratchInspRegions, out RShift_Bright, BrightIndex + 1);

                HOperatorSet.SelectObj(StainInspRegions, out Stain_Bright, BrightIndex + 1);

                HOperatorSet.SelectObj(RDefectTotalRegion, out RDefect_Bright, BrightIndex + 1);
            }

            #endregion

            #region 等於

            if (EqIndex.Length > 0 && EqIndex >= 0)
            {
                HObject EThin, ERShift, EStain, ERDefect;
                HOperatorSet.SelectObj(ThinInspRegions, out EThin, EqIndex + 1);

                HOperatorSet.SelectObj(ScratchInspRegions, out ERShift, EqIndex + 1);

                HOperatorSet.SelectObj(StainInspRegions, out EStain, EqIndex + 1);

                HOperatorSet.SelectObj(RDefectTotalRegion, out ERDefect, EqIndex + 1);


                HOperatorSet.ConcatObj(EThin, Thin_Dark, out Thin_Dark);

                HOperatorSet.ConcatObj(ERShift, RShift_Dark, out RShift_Dark);

                HOperatorSet.ConcatObj(EStain, Stain_Dark, out Stain_Dark);

                HOperatorSet.ConcatObj(ERDefect, RDefect_Dark, out RDefect_Dark);
            }

            #endregion

            HOperatorSet.Union1(Thin_Dark, out Thin_Dark);
            HOperatorSet.Union1(Stain_Dark, out Stain_Dark);
            HOperatorSet.Union1(Thin_Bright, out Thin_Bright);
            HOperatorSet.Union1(Stain_Bright, out Stain_Bright);


            //for (int i = 1; i <= numSeg; i++)
            //{
            //    HObject ThinSelectRegion,  StainSelectRegion, ScratchSelectRegion, RDefectSelectRegion;
            //    HOperatorSet.SelectObj(ThinInspRegions, out ThinSelectRegion, i);
            //    HOperatorSet.SelectObj(ScratchInspRegions, out ScratchSelectRegion, i);
            //    HOperatorSet.SelectObj(StainInspRegions, out StainSelectRegion, i);
            //    HOperatorSet.SelectObj(RDefectTotalRegion, out RDefectSelectRegion, i);

            //    if (hv_Mean[i-1]< Recipe.Param.SegParam.PartitionGrayValue)
            //    {
            //        {
            //            HOperatorSet.Union2(ThinSelectRegion, Thin_Dark, out Thin_Dark);
            //        }
            //        {
            //            HOperatorSet.ConcatObj(RDefectSelectRegion, RDefect_Dark, out RDefect_Dark);
            //        }
            //        {
            //            HOperatorSet.Union2(StainSelectRegion, Stain_Dark, out Stain_Dark);
            //        }
            //        {
            //            HOperatorSet.ConcatObj(ScratchSelectRegion, RShift_Dark, out RShift_Dark);
            //        }
            //    }
            //    else
            //    {
            //        {
            //            HOperatorSet.Union2(ThinSelectRegion, Thin_Bright, out Thin_Bright);
            //        }
            //        {
            //            HOperatorSet.ConcatObj(RDefectSelectRegion, RDefect_Bright, out RDefect_Bright);
            //        }
            //        {
            //            HOperatorSet.Union2(StainSelectRegion, Stain_Bright, out Stain_Bright);
            //        }
            //        {
            //            HOperatorSet.ConcatObj(ScratchSelectRegion, RShift_Bright, out RShift_Bright);
            //        }
            //    }
            //}

        }

        public bool GetAllInspRegions(HObject PatternRegionList, InductanceInspRole Recipe, HObject CropSrcImg, out HObject CellInspRegions, out HObject ThinInspRegions, out HObject StainInspRegions, out HObject ScratchInspRegions)
        {
            Log.WriteLog("Seg Start");
            bool Res = false;
            HTuple W, H;
            HOperatorSet.GetImageSize(CropSrcImg, out W, out H);
            HOperatorSet.GenEmptyObj(out ThinInspRegions);
            HOperatorSet.GenEmptyObj(out StainInspRegions);
            HOperatorSet.GenEmptyObj(out ScratchInspRegions);
            HOperatorSet.GenEmptyObj(out CellInspRegions);

            HTuple hv_I = new HTuple(), hv_Number = new HTuple(), hv_Index = new HTuple();

            HOperatorSet.CountObj(PatternRegionList, out hv_Number);
            HTuple end_val87 = hv_Number;
            HTuple step_val87 = 1;


            HTuple Area, CenterY, CenterX;
            HOperatorSet.AreaCenter(PatternRegionList, out Area, out CenterY, out CenterX);

            #region Org
            for (hv_Index = 1; hv_Index.Continue(end_val87, step_val87); hv_Index = hv_Index.TupleAdd(step_val87))
            {
                double InspCenterX = 0;
                double InspCenterY = 0;

                double StartY = 0;
                double StartX = 0;
                double EndY = 0;
                double EndX = 0;

                HObject ThinRect, StainRect, ScratchRect;

                #region // 刮傷
                //if (Recipe.Param.RisistAOIParam.ScratchParam.Enabled)
                {
                    InspCenterX = CenterX[hv_Index - 1] - Recipe.Param.SegParam.ScratchHotspotX;
                    InspCenterY = CenterY[hv_Index - 1] - Recipe.Param.SegParam.ScratchHotspotY;

                    StartY = InspCenterY - Recipe.Param.SegParam.ScratchInspHeight / 2;
                    StartX = InspCenterX - Recipe.Param.SegParam.ScratchInspWidrh / 2;
                    EndY = InspCenterY + Recipe.Param.SegParam.ScratchInspHeight / 2;
                    EndX = InspCenterX + Recipe.Param.SegParam.ScratchInspWidrh / 2;

                    HOperatorSet.GenRectangle1(out ScratchRect, StartY, StartX, EndY, EndX);

                    if (StartY < 0 || StartX < 0 || EndX > W || EndY > H)
                        continue;
                }

                #endregion

                #region // Cell

                InspCenterX = CenterX[hv_Index - 1] - Recipe.Param.SegParam.HotspotX;
                InspCenterY = CenterY[hv_Index - 1] - Recipe.Param.SegParam.HotspotY;

                StartY = InspCenterY - Recipe.Param.SegParam.InspRectHeight / 2;
                StartX = InspCenterX - Recipe.Param.SegParam.InspRectWidth / 2;
                EndY = InspCenterY + Recipe.Param.SegParam.InspRectHeight / 2;
                EndX = InspCenterX + Recipe.Param.SegParam.InspRectWidth / 2;

                HObject CellInspRect;
                HOperatorSet.GenRectangle1(out CellInspRect, StartY, StartX, EndY, EndX);

                if (StartY < 0 || StartX < 0 || EndX > W || EndY > H)
                    continue;
                {
                    HObject TempReg;
                    HOperatorSet.ConcatObj(CellInspRegions, CellInspRect, out TempReg);
                    CellInspRegions.Dispose();
                    CellInspRegions = TempReg;
                }
                CellInspRect.Dispose();

                //if (Recipe.Param.RisistAOIParam.ScratchParam.Enabled)
                {
                    {
                        HObject TempReg;
                        HOperatorSet.ConcatObj(ScratchInspRegions, ScratchRect, out TempReg);
                        ScratchInspRegions.Dispose();
                        ScratchInspRegions = TempReg;
                    }
                    ScratchRect.Dispose();
                }
                #endregion

                #region // Thin

                //if (Recipe.Param.RisistAOIParam.ThinParam.Enabled)
                {
                    InspCenterX = CenterX[hv_Index - 1] - Recipe.Param.SegParam.ThinHotspotX;
                    InspCenterY = CenterY[hv_Index - 1] - Recipe.Param.SegParam.ThinHotspotY;

                    StartY = InspCenterY - Recipe.Param.SegParam.ThinInspHeight / 2;
                    StartX = InspCenterX - Recipe.Param.SegParam.ThinInspWidrh / 2;
                    EndY = InspCenterY + Recipe.Param.SegParam.ThinInspHeight / 2;
                    EndX = InspCenterX + Recipe.Param.SegParam.ThinInspWidrh / 2;


                    HOperatorSet.GenRectangle1(out ThinRect, StartY, StartX, EndY, EndX);

                    if (StartY < 0 || StartX < 0 || EndX > W || EndY > H)
                        continue;
                    {
                        HObject TempReg;
                        HOperatorSet.ConcatObj(ThinInspRegions, ThinRect, out TempReg);
                        ThinInspRegions.Dispose();
                        ThinInspRegions = TempReg;
                    }
                    ThinRect.Dispose();
                }
                #endregion

                #region // Stain
                //if (Recipe.Param.RisistAOIParam.StainParam.Enabled)
                {
                    InspCenterX = CenterX[hv_Index - 1] - Recipe.Param.SegParam.StainHotspotX;
                    InspCenterY = CenterY[hv_Index - 1] - Recipe.Param.SegParam.StainHotspotY;

                    StartY = InspCenterY - Recipe.Param.SegParam.StainInspHeight / 2;
                    StartX = InspCenterX - Recipe.Param.SegParam.StainInspWidrh / 2;
                    EndY = InspCenterY + Recipe.Param.SegParam.StainInspHeight / 2;
                    EndX = InspCenterX + Recipe.Param.SegParam.StainInspWidrh / 2;


                    HOperatorSet.GenRectangle1(out StainRect, StartY, StartX, EndY, EndX);

                    if (StartY < 0 || StartX < 0 || EndX > W || EndY > H)
                        continue;

                    {
                        HObject TempReg;
                        HOperatorSet.ConcatObj(StainInspRegions, StainRect, out TempReg);
                        StainInspRegions.Dispose();
                        StainInspRegions = TempReg;
                    }
                    StainRect.Dispose();
                }
                #endregion

            }
            #endregion
            Log.WriteLog("Seg End");
            Res = true;
            return Res;
        }

        //Type 0: Thin  , 1: Scratch , 2: RShift
        public bool GetInspRegions(HObject PatternRegionList, int Type, InductanceInspRole Recipe, HObject CropSrcImg, out HObject InspRegions)
        {
            bool Res = false;
            InspRegions = null;
            HOperatorSet.GenEmptyObj(out InspRegions);

            HTuple hv_I = new HTuple(), hv_Number = new HTuple(), hv_Index = new HTuple();
            HTuple hv_nowID = new HTuple();
            HObject ho_ObjectSelected = null;
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.CountObj(PatternRegionList, out hv_Number);
            HTuple end_val87 = hv_Number;
            HTuple step_val87 = 1;

            for (hv_Index = 1; hv_Index.Continue(end_val87, step_val87); hv_Index = hv_Index.TupleAdd(step_val87))
            {
                hv_nowID = hv_Index.Clone();
                ho_ObjectSelected.Dispose();

                HOperatorSet.SelectObj(PatternRegionList, out ho_ObjectSelected, hv_nowID);

                HTuple Area, CenterY, CenterX;
                HOperatorSet.AreaCenter(ho_ObjectSelected, out Area, out CenterY, out CenterX);


                double InspCenterX = 0;
                double InspCenterY = 0;

                double StartY = 0;
                double StartX = 0;
                double EndY = 0;
                double EndX = 0;

                if (Type == 0)//薄化
                {
                    InspCenterX = CenterX - Recipe.Param.SegParam.ThinHotspotX;
                    InspCenterY = CenterY - Recipe.Param.SegParam.ThinHotspotY;

                    StartY = InspCenterY - Recipe.Param.SegParam.ThinInspHeight / 2;
                    StartX = InspCenterX - Recipe.Param.SegParam.ThinInspWidrh / 2;
                    EndY = InspCenterY + Recipe.Param.SegParam.ThinInspHeight / 2;
                    EndX = InspCenterX + Recipe.Param.SegParam.ThinInspWidrh / 2;
                }
                else if (Type == 1)//刮傷
                {
                    InspCenterX = CenterX - Recipe.Param.SegParam.ScratchHotspotX;
                    InspCenterY = CenterY - Recipe.Param.SegParam.ScratchHotspotY;

                    StartY = InspCenterY - Recipe.Param.SegParam.ScratchInspHeight / 2;
                    StartX = InspCenterX - Recipe.Param.SegParam.ScratchInspWidrh / 2;
                    EndY = InspCenterY + Recipe.Param.SegParam.ScratchInspHeight / 2;
                    EndX = InspCenterX + Recipe.Param.SegParam.ScratchInspWidrh / 2;
                }
                else if (Type == 2)//R-Defect
                {
                    InspCenterX = CenterX - Recipe.Param.SegParam.StainHotspotX;
                    InspCenterY = CenterY - Recipe.Param.SegParam.StainHotspotY;

                    StartY = InspCenterY - Recipe.Param.SegParam.StainInspHeight / 2;
                    StartX = InspCenterX - Recipe.Param.SegParam.StainInspWidrh / 2;
                    EndY = InspCenterY + Recipe.Param.SegParam.StainInspHeight / 2;
                    EndX = InspCenterX + Recipe.Param.SegParam.StainInspWidrh / 2;
                }


                HObject InspRect;
                HOperatorSet.GenRectangle1(out InspRect, StartY, StartX, EndY, EndX);
                HTuple W, H;
                HOperatorSet.GetImageSize(CropSrcImg, out W, out H);
                if (StartY < 0 || StartX < 0 || EndX > W || EndY > H)
                    continue;
                {
                    HObject TempReg;
                    HOperatorSet.ConcatObj(InspRegions, InspRect, out TempReg);
                    InspRegions.Dispose();
                    InspRegions = TempReg;
                }
            }
            Res = true;
            return Res;
        }

        public void ImageSegmentation(HTuple Angle, List<HObject> PatternRegionList, InductanceInspRole Recipe, HObject CropSrcImg, out HObject CropImgList, out HObject SegRegions)
        {
            SegRegions = null;
            CropImgList = null;
            HOperatorSet.GenEmptyObj(out CropImgList);
            HOperatorSet.GenEmptyObj(out SegRegions);

            for (int i = 0; i < PatternRegionList.Count; i++)
            {
                HTuple Area, CenterY, CenterX;
                HOperatorSet.AreaCenter(PatternRegionList[i], out Area, out CenterY, out CenterX);

                double InspCenterX = CenterX.TupleMax() - Recipe.Param.SegParam.HotspotX;
                double InspCenterY = CenterY.TupleMax() - Recipe.Param.SegParam.HotspotY;

                //double CY = InspCenterY;
                //double CX = InspCenterX;
                //int Length1 = Recipe.InspRectWidth;
                //int Length2 = Recipe.InspRectHeight;
                //double phi = (Angle[i] - Recipe.GoldenAngle);
                //HObject InspRect;
                //HOperatorSet.GenRectangle2(out InspRect, CY, CX, 0, Length1/2, Length2/2);
                //HTuple W, H;
                //HOperatorSet.GetImageSize(CropSrcImg, out W, out H);

                double StartY = InspCenterY - Recipe.Param.SegParam.InspRectHeight / 2;
                double StartX = InspCenterX - Recipe.Param.SegParam.InspRectWidth / 2;
                double EndY = InspCenterY + Recipe.Param.SegParam.InspRectHeight / 2;
                double EndX = InspCenterX + Recipe.Param.SegParam.InspRectWidth / 2;
                HObject InspRect;
                HOperatorSet.GenRectangle1(out InspRect, StartY, StartX, EndY, EndX);
                HTuple W, H;
                HOperatorSet.GetImageSize(CropSrcImg, out W, out H);
                if (StartY < 0 || StartX < 0 || EndX > W || EndY > H)
                    continue;
                {
                    HObject TempReg;
                    HOperatorSet.ConcatObj(SegRegions, InspRect, out TempReg);
                    SegRegions.Dispose();
                    SegRegions = TempReg;
                }

                HObject ReduceImg, CropImg;
                HOperatorSet.ReduceDomain(CropSrcImg, InspRect, out ReduceImg);

                HOperatorSet.CropDomain(ReduceImg, out CropImg);

                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(CropImgList, CropImg, out ExpTmpOutVar_0);
                    CropImgList.Dispose();
                    CropImgList = ExpTmpOutVar_0;
                }

            }
        }

        public bool PtternMatchAndSegRegion_Old(HObject SrcImage, InductanceInspRole Recipe, out HObject PatternRegions, out HTuple Angle, out HObject CellInspRegions, out HObject ThinInspRegions, out HObject StainInspRegions, out HObject ScratchInspRegions)
        {
            //Log.WriteLog("Seg Start");
            bool Res = false;
            #region 變數宣告
            HTuple W, H;
            HOperatorSet.GetImageSize(SrcImage, out W, out H);
            HOperatorSet.GenEmptyObj(out ThinInspRegions);
            HOperatorSet.GenEmptyObj(out StainInspRegions);
            HOperatorSet.GenEmptyObj(out ScratchInspRegions);
            HOperatorSet.GenEmptyObj(out CellInspRegions);
            HObject ho_TransContours, ho_ModelContours;
            HTuple hv_HomMat2D = null, hv_Row = null, hv_Column = null;
            HTuple hv_Angle = null, hv_Score = null, hv_I = null;
            HObject ho_Region = null;
            Angle = null;
            HObject EmptyRegion;
            HTuple IsEmpty;
            HOperatorSet.GenEmptyRegion(out EmptyRegion);

            HOperatorSet.GenEmptyObj(out ho_ModelContours);
            HOperatorSet.GenEmptyObj(out PatternRegions);
            HOperatorSet.GenEmptyObj(out ho_TransContours);
            HOperatorSet.GenEmptyObj(out ho_Region);

            #endregion

            #region 前處理
            if (Recipe.ModelID == null && !Recipe.Param.SegParam.IsNCCMode)
                return Res;


            if (Recipe.ModelID_NCC == null && Recipe.Param.SegParam.IsNCCMode)
                return Res;

            HObject InputImg;
            HObject Pattern_Rect;
            HOperatorSet.GenRectangle1(out Pattern_Rect, Recipe.Param.SegParam.Patternrow1, Recipe.Param.SegParam.Patterncolumn1, Recipe.Param.SegParam.Patternrow2, Recipe.Param.SegParam.Patterncolumn2);


            HObject ScaleImage;

            HOperatorSet.ZoomImageFactor(SrcImage, out ScaleImage, Recipe.Param.AdvParam.MatchSacleSize, Recipe.Param.AdvParam.MatchSacleSize, "nearest_neighbor");


            if (Recipe.Param.SegParam.MatchPreProcessEnabled)
                InputImg = MatchTH(ScaleImage, Recipe);
            else
            {
                HTuple Ch;
                HOperatorSet.CountChannels(ScaleImage, out Ch);
                if (Ch == 1)
                    InputImg = ScaleImage;
                else
                {
                    if (Recipe.Param.SegParam.BandIndex == 0)
                    {
                        HOperatorSet.AccessChannel(ScaleImage, out InputImg, 1);
                    }
                    else if (Recipe.Param.SegParam.BandIndex == 1)
                    {
                        HOperatorSet.AccessChannel(ScaleImage, out InputImg, 2);
                    }
                    else if (Recipe.Param.SegParam.BandIndex == 2)
                    {
                        HOperatorSet.AccessChannel(ScaleImage, out InputImg, 3);
                    }
                    else
                    {
                        HOperatorSet.Rgb1ToGray(ScaleImage, out InputImg);
                    }
                }

            }
            HOperatorSet.GrayOpeningRect(InputImg, out InputImg, Recipe.Param.AdvParam.OpeningNum, Recipe.Param.AdvParam.OpeningNum);
            if (!Recipe.Param.SegParam.IsNCCMode)
            {
                ho_ModelContours.Dispose();
                HOperatorSet.GetShapeModelContours(out ho_ModelContours, Recipe.ModelID, 1);
            }


            #endregion

            #region PatternFind

            if (Recipe.Param.SegParam.IsNCCMode)
            {
                HOperatorSet.FindNccModel(InputImg,
                                          Recipe.ModelID_NCC,
                                          new HTuple(Recipe.Param.AdvParam.AngleStart).TupleRad(),
                                          new HTuple(Recipe.Param.AdvParam.AngleExtent).TupleRad(),
                                          Recipe.Param.SegParam.MinScore,
                                          0,
                                          Recipe.Param.AdvParam.Overlap,
                                          "false",
                                          Recipe.Param.AdvParam.NumLevels,
                                          out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
            }
            else
            {
                HOperatorSet.FindShapeModel(InputImg,
                                            Recipe.ModelID,
                                            new HTuple(Recipe.Param.AdvParam.AngleStart).TupleRad(),
                                            new HTuple(Recipe.Param.AdvParam.AngleExtent).TupleRad(),
                                            Recipe.Param.SegParam.MinScore,
                                            0,
                                            Recipe.Param.AdvParam.Overlap,
                                            InductanceInsp.GetSubpixel(Recipe),
                                            Recipe.Param.AdvParam.NumLevels,
                                            Recipe.Param.AdvParam.Greediness,
                                            out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
            }

            #endregion

            HOperatorSet.GenEmptyRegion(out PatternRegions);
            Angle = hv_Angle;
            HTuple hv_RefRow, hv_RefColumn, hv_ModelRegionArea;
            HOperatorSet.AreaCenter(Pattern_Rect, out hv_ModelRegionArea, out hv_RefRow, out hv_RefColumn);

            for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Score.TupleLength())) - 1); hv_I = (int)hv_I + 1)
            {
                if (Recipe.Param.SegParam.IsNCCMode)
                {
                    ho_Region.Dispose();

                    double CX = 0;
                    double CY = 0;

                    double SY = 0;
                    double SX = 0;
                    double EY = 0;
                    double EX = 0;

                    CX = hv_Column[hv_I] / Recipe.Param.AdvParam.MatchSacleSize;
                    CY = hv_Row[hv_I] / Recipe.Param.AdvParam.MatchSacleSize;

                    SY = CY - (Recipe.Param.SegParam.Patternrow2 - Recipe.Param.SegParam.Patternrow1) / 2 * Recipe.Param.AdvParam.MatchSacleSize;
                    SX = CX - (Recipe.Param.SegParam.Patterncolumn2 - Recipe.Param.SegParam.Patterncolumn1) / 2 * Recipe.Param.AdvParam.MatchSacleSize;
                    EY = CY + (Recipe.Param.SegParam.Patternrow2 - Recipe.Param.SegParam.Patternrow1) / 2 * Recipe.Param.AdvParam.MatchSacleSize;
                    EX = CX + (Recipe.Param.SegParam.Patterncolumn2 - Recipe.Param.SegParam.Patterncolumn1) / 2 * Recipe.Param.AdvParam.MatchSacleSize;
                    HOperatorSet.GenRectangle1(out ho_Region, SY, SX, EY, EX);


                    //HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
                    //HOperatorSet.HomMat2dScale(hv_HomMat2D, 1.0 / Recipe.Param.AdvParam.MatchSacleSize, 1.0 / Recipe.Param.AdvParam.MatchSacleSize, 0, 0, out hv_HomMat2D);
                    //HOperatorSet.HomMat2dTranslate(hv_HomMat2D, -hv_RefRow / Recipe.Param.AdvParam.MatchSacleSize, -hv_RefColumn / Recipe.Param.AdvParam.MatchSacleSize, out hv_HomMat2D);
                    //HOperatorSet.HomMat2dRotate(hv_HomMat2D, hv_Angle.TupleSelect(hv_I), 0, 0, out hv_HomMat2D);
                    //HOperatorSet.HomMat2dTranslate(hv_HomMat2D, hv_Row.TupleSelect(hv_I) / Recipe.Param.AdvParam.MatchSacleSize, hv_Column.TupleSelect(hv_I) / Recipe.Param.AdvParam.MatchSacleSize, out hv_HomMat2D);
                    //ho_Region.Dispose();
                    //HOperatorSet.AffineTransRegion(Pattern_Rect, out ho_Region, hv_HomMat2D, "nearest_neighbor");
                }
                else
                {
                    HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
                    HOperatorSet.HomMat2dScale(hv_HomMat2D, 1.0 / Recipe.Param.AdvParam.MatchSacleSize, 1.0 / Recipe.Param.AdvParam.MatchSacleSize, 0, 0, out hv_HomMat2D);
                    HOperatorSet.HomMat2dRotate(hv_HomMat2D, hv_Angle.TupleSelect(hv_I), 0, 0, out hv_HomMat2D);
                    HOperatorSet.HomMat2dTranslate(hv_HomMat2D, hv_Row.TupleSelect(hv_I) / Recipe.Param.AdvParam.MatchSacleSize, hv_Column.TupleSelect(hv_I) / Recipe.Param.AdvParam.MatchSacleSize, out hv_HomMat2D);
                    ho_TransContours.Dispose();
                    HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_TransContours, hv_HomMat2D);
                    ho_Region.Dispose();
                    HOperatorSet.GenRegionContourXld(ho_TransContours, out ho_Region, "filled");
                }

                HObject Un1;
                HOperatorSet.Union1(ho_Region, out Un1);
                {
                    HObject ExpTmpOutVar_0;

                    HOperatorSet.TestEqualRegion(PatternRegions, EmptyRegion, out IsEmpty);
                    if (IsEmpty)
                        PatternRegions = Un1.Clone();
                    else
                    {
                        HOperatorSet.ConcatObj(PatternRegions, Un1, out ExpTmpOutVar_0);
                        PatternRegions.Dispose();
                        PatternRegions = ExpTmpOutVar_0;
                    }
                }

                HTuple Area, CenterY, CenterX;
                HOperatorSet.AreaCenter(Un1, out Area, out CenterY, out CenterX);

                double InspCenterX = 0;
                double InspCenterY = 0;

                double StartY = 0;
                double StartX = 0;
                double EndY = 0;
                double EndX = 0;

                HObject ThinRect, StainRect, ScratchRect;

                #region // 刮傷
                {
                    InspCenterX = CenterX - Recipe.Param.SegParam.ScratchHotspotX;
                    InspCenterY = CenterY - Recipe.Param.SegParam.ScratchHotspotY;

                    StartY = InspCenterY - Recipe.Param.SegParam.ScratchInspHeight / 2;
                    StartX = InspCenterX - Recipe.Param.SegParam.ScratchInspWidrh / 2;
                    EndY = InspCenterY + Recipe.Param.SegParam.ScratchInspHeight / 2;
                    EndX = InspCenterX + Recipe.Param.SegParam.ScratchInspWidrh / 2;

                    HOperatorSet.GenRectangle1(out ScratchRect, StartY, StartX, EndY, EndX);

                    if (StartY < 0 || StartX < 0 || EndX > W || EndY > H)
                        continue;
                }

                #endregion

                #region // Cell

                InspCenterX = CenterX - Recipe.Param.SegParam.HotspotX;
                InspCenterY = CenterY - Recipe.Param.SegParam.HotspotY;

                StartY = InspCenterY - Recipe.Param.SegParam.InspRectHeight / 2;
                StartX = InspCenterX - Recipe.Param.SegParam.InspRectWidth / 2;
                EndY = InspCenterY + Recipe.Param.SegParam.InspRectHeight / 2;
                EndX = InspCenterX + Recipe.Param.SegParam.InspRectWidth / 2;

                HObject CellInspRect;
                HOperatorSet.GenRectangle1(out CellInspRect, StartY, StartX, EndY, EndX);

                if (StartY < 0 || StartX < 0 || EndX > W || EndY > H)
                    continue;
                {
                    HObject TempReg;
                    HOperatorSet.ConcatObj(CellInspRegions, CellInspRect, out TempReg);
                    CellInspRegions.Dispose();
                    CellInspRegions = TempReg;
                }
                CellInspRect.Dispose();

                {
                    {
                        HObject TempReg;
                        HOperatorSet.ConcatObj(ScratchInspRegions, ScratchRect, out TempReg);
                        ScratchInspRegions.Dispose();
                        ScratchInspRegions = TempReg;
                    }
                    ScratchRect.Dispose();
                }
                #endregion

                #region // Thin

                {
                    InspCenterX = CenterX - Recipe.Param.SegParam.ThinHotspotX;
                    InspCenterY = CenterY - Recipe.Param.SegParam.ThinHotspotY;

                    StartY = InspCenterY - Recipe.Param.SegParam.ThinInspHeight / 2;
                    StartX = InspCenterX - Recipe.Param.SegParam.ThinInspWidrh / 2;
                    EndY = InspCenterY + Recipe.Param.SegParam.ThinInspHeight / 2;
                    EndX = InspCenterX + Recipe.Param.SegParam.ThinInspWidrh / 2;


                    HOperatorSet.GenRectangle1(out ThinRect, StartY, StartX, EndY, EndX);

                    if (StartY < 0 || StartX < 0 || EndX > W || EndY > H)
                        continue;
                    {
                        HObject TempReg;
                        HOperatorSet.ConcatObj(ThinInspRegions, ThinRect, out TempReg);
                        ThinInspRegions.Dispose();
                        ThinInspRegions = TempReg;
                    }
                    ThinRect.Dispose();
                }
                #endregion

                #region // Stain
                {
                    InspCenterX = CenterX - Recipe.Param.SegParam.StainHotspotX;
                    InspCenterY = CenterY - Recipe.Param.SegParam.StainHotspotY;

                    StartY = InspCenterY - Recipe.Param.SegParam.StainInspHeight / 2;
                    StartX = InspCenterX - Recipe.Param.SegParam.StainInspWidrh / 2;
                    EndY = InspCenterY + Recipe.Param.SegParam.StainInspHeight / 2;
                    EndX = InspCenterX + Recipe.Param.SegParam.StainInspWidrh / 2;


                    HOperatorSet.GenRectangle1(out StainRect, StartY, StartX, EndY, EndX);

                    if (StartY < 0 || StartX < 0 || EndX > W || EndY > H)
                        continue;

                    {
                        HObject TempReg;
                        HOperatorSet.ConcatObj(StainInspRegions, StainRect, out TempReg);
                        StainInspRegions.Dispose();
                        StainInspRegions = TempReg;
                    }
                    StainRect.Dispose();
                }
                #endregion


                Un1.Dispose();

            }
            //Log.WriteLog("Seg End");
            #region Dispose
            ScaleImage.Dispose();
            Pattern_Rect.Dispose();
            //InputImg.Dispose();
            EmptyRegion.Dispose();
            ho_ModelContours.Dispose();
            ho_TransContours.Dispose();
            ho_Region.Dispose();

            #endregion
            Res = true;
            return Res;
        }

        public static bool PtternMatchAndSegRegion_New(HObject SrcImage, InductanceInspRole Recipe, out HObject PatternRegions, out HTuple Angle,out HTuple hv_Row,out HTuple hv_Column, out HObject CellInspRegions) // (20200130) Jeff Revised!
        {
            bool Res = false;

            #region 變數宣告
            HTuple W, H;
            HOperatorSet.GetImageSize(SrcImage, out W, out H);
            HOperatorSet.GenEmptyObj(out CellInspRegions);
            HObject ho_TransContours, ho_ModelContours;
            HTuple hv_HomMat2D = null;
            hv_Row = null; hv_Column = null;
            HTuple hv_Angle = null, hv_Score = null, hv_I = null;
            HObject ho_Region = null;
            Angle = null;
            HObject EmptyRegion;
            HTuple IsEmpty;
            HOperatorSet.GenEmptyRegion(out EmptyRegion);

            HOperatorSet.GenEmptyObj(out ho_ModelContours);
            HOperatorSet.GenEmptyObj(out PatternRegions);
            HOperatorSet.GenEmptyObj(out ho_TransContours);
            HOperatorSet.GenEmptyObj(out ho_Region);

            #endregion

            #region 前處理

            if (Recipe.ModelID == null && !Recipe.Param.SegParam.IsNCCMode)
                return Res;


            if (Recipe.ModelID_NCC == null && Recipe.Param.SegParam.IsNCCMode)
                return Res;

            HObject InputImg;
            
            HObject ScaleImage;

            HOperatorSet.ZoomImageFactor(SrcImage, out ScaleImage, Recipe.Param.AdvParam.MatchSacleSize, Recipe.Param.AdvParam.MatchSacleSize, "nearest_neighbor");


            if (Recipe.Param.SegParam.MatchPreProcessEnabled)
                InputImg = MatchTH(ScaleImage, Recipe);
            else
            {
                InputImg = clsStaticTool.GetChannelImage(ScaleImage, (enuBand)Recipe.Param.SegParam.BandIndex);
            }
            //HOperatorSet.GrayOpeningRect(InputImg, out InputImg, Recipe.Param.AdvParam.OpeningNum, Recipe.Param.AdvParam.OpeningNum); // (20200130) Jeff Revised!
            if (!Recipe.Param.SegParam.IsNCCMode)
            {
                ho_ModelContours.Dispose();
                HOperatorSet.GetShapeModelContours(out ho_ModelContours, Recipe.ModelID, 1);
            }
            
            #endregion

            #region PatternFind

            if (Recipe.Param.SegParam.IsNCCMode)
            {
                HOperatorSet.FindNccModel(InputImg,
                                          Recipe.ModelID_NCC,
                                          new HTuple(Recipe.Param.AdvParam.AngleStart).TupleRad(),
                                          new HTuple(Recipe.Param.AdvParam.AngleExtent).TupleRad(),
                                          Recipe.Param.SegParam.MinScore,
                                          Recipe.Param.AdvParam.MatchNumber,
                                          Recipe.Param.AdvParam.Overlap,
                                          "false",
                                          Recipe.Param.AdvParam.NumLevels,
                                          out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
            }
            else
            {
                HOperatorSet.FindShapeModel(InputImg,
                                            Recipe.ModelID,
                                            new HTuple(Recipe.Param.AdvParam.AngleStart).TupleRad(),
                                            new HTuple(Recipe.Param.AdvParam.AngleExtent).TupleRad(),
                                            Recipe.Param.SegParam.MinScore,
                                            Recipe.Param.AdvParam.MatchNumber,
                                            Recipe.Param.AdvParam.Overlap,
                                            InductanceInsp.GetSubpixel(Recipe),
                                            Recipe.Param.AdvParam.NumLevels,
                                            Recipe.Param.AdvParam.Greediness,
                                            out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
            }

            #endregion

            HOperatorSet.GenEmptyRegion(out PatternRegions);
            Angle = hv_Angle;

            if (hv_Column.Length <= 0)
            {
                return false;
            }

            #region Get Pattern Region

            if (Recipe.Param.SegParam.IsNCCMode)
            {
                ho_Region.Dispose();

                HTuple CX;
                HTuple CY;

                HTuple SY;
                HTuple SX;
                HTuple EY;
                HTuple EX;

                CX = hv_Column / Recipe.Param.AdvParam.MatchSacleSize;
                CY = hv_Row / Recipe.Param.AdvParam.MatchSacleSize;

                SY = CY - (Recipe.Param.SegParam.Patternrow2 - Recipe.Param.SegParam.Patternrow1) / 2 / Recipe.Param.AdvParam.MatchSacleSize;
                SX = CX - (Recipe.Param.SegParam.Patterncolumn2 - Recipe.Param.SegParam.Patterncolumn1) / 2 / Recipe.Param.AdvParam.MatchSacleSize;
                EY = CY + (Recipe.Param.SegParam.Patternrow2 - Recipe.Param.SegParam.Patternrow1) / 2 / Recipe.Param.AdvParam.MatchSacleSize;
                EX = CX + (Recipe.Param.SegParam.Patterncolumn2 - Recipe.Param.SegParam.Patterncolumn1) / 2 / Recipe.Param.AdvParam.MatchSacleSize;
                HOperatorSet.GenRectangle1(out PatternRegions, SY, SX, EY, EX);

                HOperatorSet.SelectShape(PatternRegions, out PatternRegions, "row1", "and", 0, Recipe.Param.ImageHeight);
                HOperatorSet.SelectShape(PatternRegions, out PatternRegions, "row2", "and", 0, Recipe.Param.ImageHeight);
                HOperatorSet.SelectShape(PatternRegions, out PatternRegions, "column1", "and", 0, Recipe.Param.ImageWidth);
                HOperatorSet.SelectShape(PatternRegions, out PatternRegions, "column2", "and", 0, Recipe.Param.ImageWidth);
            }
            else
            {
                for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Score.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                {
                    HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
                    HOperatorSet.HomMat2dScale(hv_HomMat2D, 1.0 / Recipe.Param.AdvParam.MatchSacleSize, 1.0 / Recipe.Param.AdvParam.MatchSacleSize, 0, 0, out hv_HomMat2D);
                    HOperatorSet.HomMat2dRotate(hv_HomMat2D, hv_Angle.TupleSelect(hv_I), 0, 0, out hv_HomMat2D);
                    HOperatorSet.HomMat2dTranslate(hv_HomMat2D, hv_Row.TupleSelect(hv_I) / Recipe.Param.AdvParam.MatchSacleSize, hv_Column.TupleSelect(hv_I) / Recipe.Param.AdvParam.MatchSacleSize, out hv_HomMat2D);
                    ho_TransContours.Dispose();
                    HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_TransContours, hv_HomMat2D);
                    ho_Region.Dispose();
                    HOperatorSet.GenRegionContourXld(ho_TransContours, out ho_Region, "filled");

                    HObject Un1;
                    HOperatorSet.Union1(ho_Region, out Un1);

                    //HTuple row1, row2, column1, column2;
                    //HOperatorSet.RegionFeatures(Un1, "row1", out row1);
                    //HOperatorSet.RegionFeatures(Un1, "row2", out row2);
                    //HOperatorSet.RegionFeatures(Un1, "column1", out column1);
                    //HOperatorSet.RegionFeatures(Un1, "column2", out column2);

                    //if (row1 < 0 || column1 < 0 || row2 > Recipe.Param.ImageHeight || column2 > Recipe.Param.ImageWidth)
                    //    continue;

                    {
                        HObject ExpTmpOutVar_0;

                        HOperatorSet.TestEqualRegion(PatternRegions, EmptyRegion, out IsEmpty);
                        if (IsEmpty)
                            PatternRegions = Un1.Clone();
                        else
                        {
                            HOperatorSet.ConcatObj(PatternRegions, Un1, out ExpTmpOutVar_0);
                            PatternRegions.Dispose();
                            PatternRegions = ExpTmpOutVar_0;
                        }
                    }
                }

            }

            #endregion

            //HObject SortPatRegion;
            //HOperatorSet.SortRegion(PatternRegions, out SortPatRegion, "character", "true", "row");

            HTuple Area, CenterY, CenterX;
            HOperatorSet.AreaCenter(PatternRegions, out Area, out CenterY, out CenterX);

            #region  Cell
            {
           
                HTuple AWidth, AHeight;
                HOperatorSet.TupleGenConst(hv_Row.Length, Recipe.Param.SegParam.InspRectWidth, out AWidth);
                HOperatorSet.TupleGenConst(hv_Row.Length, Recipe.Param.SegParam.InspRectHeight, out AHeight);
                HTuple A,cosA,sinA;
                HOperatorSet.TupleDeg(hv_Angle, out A);
                HOperatorSet.TupleCos(hv_Angle, out cosA);
                HOperatorSet.TupleSin(hv_Angle, out sinA);

                HTuple HX, HY;
                HOperatorSet.TupleGenConst(hv_Row.Length, Recipe.Param.SegParam.HotspotX, out HX);
                HOperatorSet.TupleGenConst(hv_Row.Length, Recipe.Param.SegParam.HotspotY, out HY);

                HTuple RX = HX * cosA + HY * sinA;
                HTuple RY = -HX * sinA + HY * cosA;
                HOperatorSet.GenRectangle2(out CellInspRegions, CenterY - RY, CenterX - RX, hv_Angle, AWidth / 2, AHeight / 2);
                //HOperatorSet.GenRectangle2(out CellInspRegions, CenterY - Recipe.Param.SegParam.HotspotY, CenterX - Recipe.Param.SegParam.HotspotX, hv_Angle, AWidth / 2, AHeight / 2);

                //HOperatorSet.GenRectangle1(out CellInspRegions, StartY, StartX, EndY, EndX);

                //if (!Recipe.Param.bInspOutboundary)
                //{
                //    HOperatorSet.SelectShape(CellInspRegions, out CellInspRegions, "row1", "and", 0, Recipe.Param.ImageHeight);
                //    HOperatorSet.SelectShape(CellInspRegions, out CellInspRegions, "row2", "and", 0, Recipe.Param.ImageHeight);
                //    HOperatorSet.SelectShape(CellInspRegions, out CellInspRegions, "column1", "and", 0, Recipe.Param.ImageWidth);
                //    HOperatorSet.SelectShape(CellInspRegions, out CellInspRegions, "column2", "and", 0, Recipe.Param.ImageWidth);
                //}
            }
            #endregion
            
            #region Dispose
            ScaleImage.Dispose();

            EmptyRegion.Dispose();
            ho_ModelContours.Dispose();
            ho_TransContours.Dispose();
            ho_Region.Dispose();

            #endregion
            Res = true;
            return Res;
        }


        public bool PtternMatchAndSegRegion(HObject SrcImage, InductanceInspRole Recipe, out HObject PatternRegions, out HTuple Angle, out HObject CellInspRegions, out HObject ThinInspRegions, out HObject StainInspRegions, out HObject ScratchInspRegions)
        {
            bool Res = false;

            #region 變數宣告
            HTuple W, H;
            HOperatorSet.GetImageSize(SrcImage, out W, out H);
            HOperatorSet.GenEmptyObj(out ThinInspRegions);
            HOperatorSet.GenEmptyObj(out StainInspRegions);
            HOperatorSet.GenEmptyObj(out ScratchInspRegions);
            HOperatorSet.GenEmptyObj(out CellInspRegions);
            HObject ho_TransContours, ho_ModelContours;
            HTuple hv_HomMat2D = null, hv_Row = null, hv_Column = null;
            HTuple hv_Angle = null, hv_Score = null, hv_I = null;
            HObject ho_Region = null;
            Angle = null;
            HObject EmptyRegion;
            HTuple IsEmpty;
            HOperatorSet.GenEmptyRegion(out EmptyRegion);

            HOperatorSet.GenEmptyObj(out ho_ModelContours);
            HOperatorSet.GenEmptyObj(out PatternRegions);
            HOperatorSet.GenEmptyObj(out ho_TransContours);
            HOperatorSet.GenEmptyObj(out ho_Region);

            #endregion

            #region 前處理
            if (Recipe.ModelID == null && !Recipe.Param.SegParam.IsNCCMode)
                return Res;


            if (Recipe.ModelID_NCC == null && Recipe.Param.SegParam.IsNCCMode)
                return Res;

            HObject InputImg;
            //HObject Pattern_Rect;
            //HOperatorSet.GenRectangle1(out Pattern_Rect, Recipe.Param.SegParam.Patternrow1, Recipe.Param.SegParam.Patterncolumn1, Recipe.Param.SegParam.Patternrow2, Recipe.Param.SegParam.Patterncolumn2);


            HObject ScaleImage;

            HOperatorSet.ZoomImageFactor(SrcImage, out ScaleImage, Recipe.Param.AdvParam.MatchSacleSize, Recipe.Param.AdvParam.MatchSacleSize, "nearest_neighbor");


            if (Recipe.Param.SegParam.MatchPreProcessEnabled)
                InputImg = MatchTH(ScaleImage, Recipe);
            else
            {
                InputImg = clsStaticTool.GetChannelImage(ScaleImage, (enuBand)Recipe.Param.SegParam.BandIndex);
            }
            HOperatorSet.GrayOpeningRect(InputImg, out InputImg, Recipe.Param.AdvParam.OpeningNum, Recipe.Param.AdvParam.OpeningNum);
            if (!Recipe.Param.SegParam.IsNCCMode)
            {
                ho_ModelContours.Dispose();
                HOperatorSet.GetShapeModelContours(out ho_ModelContours, Recipe.ModelID, 1);
            }


            #endregion

            #region PatternFind

            if (Recipe.Param.SegParam.IsNCCMode)
            {
                HOperatorSet.FindNccModel(InputImg,
                                          Recipe.ModelID_NCC,
                                          new HTuple(Recipe.Param.AdvParam.AngleStart).TupleRad(),
                                          new HTuple(Recipe.Param.AdvParam.AngleExtent).TupleRad(),
                                          Recipe.Param.SegParam.MinScore,
                                          Recipe.Param.AdvParam.MatchNumber,
                                          Recipe.Param.AdvParam.Overlap,
                                          "false",
                                          Recipe.Param.AdvParam.NumLevels,
                                          out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
            }
            else
            {
                HOperatorSet.FindShapeModel(InputImg,
                                            Recipe.ModelID,
                                            new HTuple(Recipe.Param.AdvParam.AngleStart).TupleRad(),
                                            new HTuple(Recipe.Param.AdvParam.AngleExtent).TupleRad(),
                                            Recipe.Param.SegParam.MinScore,
                                            Recipe.Param.AdvParam.MatchNumber,
                                            Recipe.Param.AdvParam.Overlap,
                                            InductanceInsp.GetSubpixel(Recipe),
                                            Recipe.Param.AdvParam.NumLevels,
                                            Recipe.Param.AdvParam.Greediness,
                                            out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
            }

            #endregion

            HOperatorSet.GenEmptyRegion(out PatternRegions);
            Angle = hv_Angle;
            //HTuple hv_RefRow, hv_RefColumn, hv_ModelRegionArea;
            //HOperatorSet.AreaCenter(Pattern_Rect, out hv_ModelRegionArea, out hv_RefRow, out hv_RefColumn);

            if (hv_Column.Length <= 0)
            {
                return false;
            }

            #region Get Pattern Region

            if (Recipe.Param.SegParam.IsNCCMode)
            {
                ho_Region.Dispose();

                HTuple CX;
                HTuple CY;

                HTuple SY;
                HTuple SX;
                HTuple EY;
                HTuple EX;

                CX = hv_Column / Recipe.Param.AdvParam.MatchSacleSize;
                CY = hv_Row / Recipe.Param.AdvParam.MatchSacleSize;

                SY = CY - (Recipe.Param.SegParam.Patternrow2 - Recipe.Param.SegParam.Patternrow1) / 2 / Recipe.Param.AdvParam.MatchSacleSize;
                SX = CX - (Recipe.Param.SegParam.Patterncolumn2 - Recipe.Param.SegParam.Patterncolumn1) / 2 / Recipe.Param.AdvParam.MatchSacleSize;
                EY = CY + (Recipe.Param.SegParam.Patternrow2 - Recipe.Param.SegParam.Patternrow1) / 2 / Recipe.Param.AdvParam.MatchSacleSize;
                EX = CX + (Recipe.Param.SegParam.Patterncolumn2 - Recipe.Param.SegParam.Patterncolumn1) / 2 / Recipe.Param.AdvParam.MatchSacleSize;
                HOperatorSet.GenRectangle1(out PatternRegions, SY, SX, EY, EX);

                HOperatorSet.SelectShape(PatternRegions, out PatternRegions, "row1", "and", 0, Recipe.Param.ImageHeight);
                HOperatorSet.SelectShape(PatternRegions, out PatternRegions, "row2", "and", 0, Recipe.Param.ImageHeight);
                HOperatorSet.SelectShape(PatternRegions, out PatternRegions, "column1", "and", 0, Recipe.Param.ImageWidth);
                HOperatorSet.SelectShape(PatternRegions, out PatternRegions, "column2", "and", 0, Recipe.Param.ImageWidth);
            }
            else
            {
                for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Score.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                {
                    HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
                    HOperatorSet.HomMat2dScale(hv_HomMat2D, 1.0 / Recipe.Param.AdvParam.MatchSacleSize, 1.0 / Recipe.Param.AdvParam.MatchSacleSize, 0, 0, out hv_HomMat2D);
                    HOperatorSet.HomMat2dRotate(hv_HomMat2D, hv_Angle.TupleSelect(hv_I), 0, 0, out hv_HomMat2D);
                    HOperatorSet.HomMat2dTranslate(hv_HomMat2D, hv_Row.TupleSelect(hv_I) / Recipe.Param.AdvParam.MatchSacleSize, hv_Column.TupleSelect(hv_I) / Recipe.Param.AdvParam.MatchSacleSize, out hv_HomMat2D);
                    ho_TransContours.Dispose();
                    HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_TransContours, hv_HomMat2D);
                    ho_Region.Dispose();
                    HOperatorSet.GenRegionContourXld(ho_TransContours, out ho_Region, "filled");

                    HObject Un1;
                    HOperatorSet.Union1(ho_Region, out Un1);

                    HTuple row1, row2, column1, column2;
                    HOperatorSet.RegionFeatures(Un1, "row1", out row1);
                    HOperatorSet.RegionFeatures(Un1, "row2", out row2);
                    HOperatorSet.RegionFeatures(Un1, "column1", out column1);
                    HOperatorSet.RegionFeatures(Un1, "column2", out column2);

                    //if (row1 < 0 || column1 < 0 || row2 > Recipe.Param.ImageHeight || column2 > Recipe.Param.ImageWidth)
                    //    continue;

                    {
                        HObject ExpTmpOutVar_0;

                        HOperatorSet.TestEqualRegion(PatternRegions, EmptyRegion, out IsEmpty);
                        if (IsEmpty)
                            PatternRegions = Un1.Clone();
                        else
                        {
                            HOperatorSet.ConcatObj(PatternRegions, Un1, out ExpTmpOutVar_0);
                            PatternRegions.Dispose();
                            PatternRegions = ExpTmpOutVar_0;
                        }
                    }
                }

            }

            #endregion

            //HObject SortPatRegion;
            //HOperatorSet.SortRegion(PatternRegions, out SortPatRegion, "character", "true", "row");

            HTuple Area, CenterY, CenterX;
            HOperatorSet.AreaCenter(PatternRegions, out Area, out CenterY, out CenterX);

            #region  Cell
            {
                //HTuple InspCenterX;
                //HTuple InspCenterY;

                //HTuple StartY;
                //HTuple StartX;
                //HTuple EndY;
                //HTuple EndX;

                //InspCenterX = CenterX - Recipe.Param.SegParam.HotspotX;
                //InspCenterY = CenterY - Recipe.Param.SegParam.HotspotY;

                //StartY = InspCenterY - Recipe.Param.SegParam.InspRectHeight / 2;
                //StartX = InspCenterX - Recipe.Param.SegParam.InspRectWidth / 2;
                //EndY = InspCenterY + Recipe.Param.SegParam.InspRectHeight / 2;
                //EndX = InspCenterX + Recipe.Param.SegParam.InspRectWidth / 2;

                HTuple AWidth, AHeight;
                HOperatorSet.TupleGenConst(hv_Row.Length, Recipe.Param.SegParam.InspRectWidth, out AWidth);
                HOperatorSet.TupleGenConst(hv_Row.Length, Recipe.Param.SegParam.InspRectHeight, out AHeight);
                
                HOperatorSet.GenRectangle2(out CellInspRegions, CenterY - Recipe.Param.SegParam.HotspotY, CenterX - Recipe.Param.SegParam.HotspotX, hv_Angle, AWidth / 2, AHeight / 2);

                //HOperatorSet.GenRectangle1(out CellInspRegions, StartY, StartX, EndY, EndX);

                //if (!Recipe.Param.bInspOutboundary)
                //{
                //    HOperatorSet.SelectShape(CellInspRegions, out CellInspRegions, "row1", "and", 0, Recipe.Param.ImageHeight);
                //    HOperatorSet.SelectShape(CellInspRegions, out CellInspRegions, "row2", "and", 0, Recipe.Param.ImageHeight);
                //    HOperatorSet.SelectShape(CellInspRegions, out CellInspRegions, "column1", "and", 0, Recipe.Param.ImageWidth);
                //    HOperatorSet.SelectShape(CellInspRegions, out CellInspRegions, "column2", "and", 0, Recipe.Param.ImageWidth);
                //}
            }
            #endregion

            #region 刮傷
            {
                HTuple InspCenterX;
                HTuple InspCenterY;

                HTuple StartY;
                HTuple StartX;
                HTuple EndY;
                HTuple EndX;

                InspCenterX = CenterX - Recipe.Param.SegParam.ScratchHotspotX;
                InspCenterY = CenterY - Recipe.Param.SegParam.ScratchHotspotY;

                StartY = InspCenterY - Recipe.Param.SegParam.ScratchInspHeight / 2;
                StartX = InspCenterX - Recipe.Param.SegParam.ScratchInspWidrh / 2;
                EndY = InspCenterY + Recipe.Param.SegParam.ScratchInspHeight / 2;
                EndX = InspCenterX + Recipe.Param.SegParam.ScratchInspWidrh / 2;

                HOperatorSet.GenRectangle1(out ScratchInspRegions, StartY, StartX, EndY, EndX);
            }
            #endregion

            #region 薄化

            {
                HTuple InspCenterX;
                HTuple InspCenterY;

                HTuple StartY;
                HTuple StartX;
                HTuple EndY;
                HTuple EndX;

                InspCenterX = CenterX - Recipe.Param.SegParam.ThinHotspotX;
                InspCenterY = CenterY - Recipe.Param.SegParam.ThinHotspotY;

                StartY = InspCenterY - Recipe.Param.SegParam.ThinInspHeight / 2;
                StartX = InspCenterX - Recipe.Param.SegParam.ThinInspWidrh / 2;
                EndY = InspCenterY + Recipe.Param.SegParam.ThinInspHeight / 2;
                EndX = InspCenterX + Recipe.Param.SegParam.ThinInspWidrh / 2;

                HOperatorSet.GenRectangle1(out ThinInspRegions, StartY, StartX, EndY, EndX);
            }

            #endregion

            #region 髒污

            {
                HTuple InspCenterX;
                HTuple InspCenterY;

                HTuple StartY;
                HTuple StartX;
                HTuple EndY;
                HTuple EndX;

                InspCenterX = CenterX - Recipe.Param.SegParam.StainHotspotX;
                InspCenterY = CenterY - Recipe.Param.SegParam.StainHotspotY;

                StartY = InspCenterY - Recipe.Param.SegParam.StainInspHeight / 2;
                StartX = InspCenterX - Recipe.Param.SegParam.StainInspWidrh / 2;
                EndY = InspCenterY + Recipe.Param.SegParam.StainInspHeight / 2;
                EndX = InspCenterX + Recipe.Param.SegParam.StainInspWidrh / 2;

                HOperatorSet.GenRectangle1(out StainInspRegions, StartY, StartX, EndY, EndX);
            }

            #endregion


            #region Dispose
            ScaleImage.Dispose();
            //Pattern_Rect.Dispose();

            EmptyRegion.Dispose();
            ho_ModelContours.Dispose();
            ho_TransContours.Dispose();
            ho_Region.Dispose();

            #endregion
            Res = true;
            return Res;
        }
        
        public static bool GetMatchInfo(HObject SrcImage, InductanceInspRole Recipe, out HTuple hv_Row, out HTuple hv_Column, out HTuple hv_Angle, out HTuple hv_Score)
        {
            bool Res = false;

            hv_Row = 0;
            hv_Column = 0;
            hv_Angle = 0;
            hv_Score = 0;

            #region 變數宣告
            HTuple W, H;
            HOperatorSet.GetImageSize(SrcImage, out W, out H);
            HObject ho_ModelContours;
            HOperatorSet.GenEmptyObj(out ho_ModelContours);

            #endregion

            #region 前處理
            if (Recipe.ModelID == null && !Recipe.Param.SegParam.IsNCCMode)
                return Res;


            if (Recipe.ModelID_NCC == null && Recipe.Param.SegParam.IsNCCMode)
                return Res;

            HObject InputImg;
            //HObject Pattern_Rect;
            //HOperatorSet.GenRectangle1(out Pattern_Rect, Recipe.Param.SegParam.Patternrow1, Recipe.Param.SegParam.Patterncolumn1, Recipe.Param.SegParam.Patternrow2, Recipe.Param.SegParam.Patterncolumn2);
            HTuple ImageW, ImageH;
            HOperatorSet.GetImageSize(SrcImage, out ImageW, out ImageH);

            HObject ScaleImage;

            HOperatorSet.ZoomImageFactor(SrcImage, out ScaleImage, Recipe.Param.AdvParam.MatchSacleSize, Recipe.Param.AdvParam.MatchSacleSize, "nearest_neighbor");


            if (Recipe.Param.SegParam.MatchPreProcessEnabled)
                InputImg = MatchTH(ScaleImage, Recipe);
            else
            {
                InputImg = clsStaticTool.GetChannelImage(ScaleImage, (enuBand)Recipe.Param.SegParam.BandIndex);
            }
            HOperatorSet.GrayOpeningRect(InputImg, out InputImg, Recipe.Param.AdvParam.OpeningNum, Recipe.Param.AdvParam.OpeningNum);
            if (!Recipe.Param.SegParam.IsNCCMode)
            {
                ho_ModelContours.Dispose();
                HOperatorSet.GetShapeModelContours(out ho_ModelContours, Recipe.ModelID, 1);
            }


            #endregion

            #region PatternFind

            if (Recipe.Param.SegParam.IsNCCMode)
            {
                HOperatorSet.FindNccModel(InputImg,
                                          Recipe.ModelID_NCC,
                                          new HTuple(Recipe.Param.AdvParam.AngleStart).TupleRad(),
                                          new HTuple(Recipe.Param.AdvParam.AngleExtent).TupleRad(),
                                          Recipe.Param.SegParam.MinScore,
                                          Recipe.Param.AdvParam.MatchNumber,
                                          Recipe.Param.AdvParam.Overlap,
                                          "false",
                                          Recipe.Param.AdvParam.NumLevels,
                                          out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
            }
            else
            {
                HOperatorSet.FindShapeModel(InputImg,
                                            Recipe.ModelID,
                                            new HTuple(Recipe.Param.AdvParam.AngleStart).TupleRad(),
                                            new HTuple(Recipe.Param.AdvParam.AngleExtent).TupleRad(),
                                            Recipe.Param.SegParam.MinScore,
                                            Recipe.Param.AdvParam.MatchNumber,
                                            Recipe.Param.AdvParam.Overlap,
                                            InductanceInsp.GetSubpixel(Recipe),
                                            Recipe.Param.AdvParam.NumLevels,
                                            Recipe.Param.AdvParam.Greediness,
                                            out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
            }

            #endregion


            #region Dispose
            ScaleImage.Dispose();
            //Pattern_Rect.Dispose();

            #endregion
            Res = true;
            return Res;
        }

        public class InspParam // (20200130) Jeff Revised!
        {
            /// <summary>
            /// 原始影像集合
            /// </summary>
            public List<HObject> InspImageList { get; set; }

            /// <summary>
            /// 結果影像集合 (不使用範圍列表)
            /// </summary>
            public List<HObject> ResultImages { get; set; } // (20200130) Jeff Revised!

            /// <summary>
            /// 結果區域集合 (不使用範圍列表)
            /// </summary>
            public List<HObject> ResultRegions { get; set; } // (20200729) Jeff Revised!

            /// <summary>
            /// 結果影像集合 (使用範圍列表)
            /// </summary>
            public List<HObject> ResultImages_UsedRegion { get; set; } // (20200130) Jeff Revised!

            /// <summary>
            /// 結果區域集合 (使用範圍列表)
            /// </summary>
            public List<HObject> ResultRegions_UsedRegion { get; set; } // (20200729) Jeff Revised!

            public InductanceInspRole Recipe { get; set; }
            public int InspIndex { get; set; }
            public double Resolution { get; set; }
            public enuAlgorithm Type { get; set; }

            /// <summary>
            /// 檢測Region
            /// </summary>
            public HObject InspRegion { get; set; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="pmInspImageList"></param>
            /// <param name="pmInspRegion">檢測Region</param>
            /// <param name="pmRecipe"></param>
            /// <param name="pmInspIndex"></param>
            /// <param name="pmResolution"></param>
            /// <param name="pmType"></param>
            /// <param name="resultImages"></param>
            /// <param name="resultImages_UsedRegion"></param>
            public InspParam(List<HObject> pmInspImageList, HObject pmInspRegion, InductanceInspRole pmRecipe, int pmInspIndex, double pmResolution, enuAlgorithm pmType, List<HObject> resultImages = null, List<HObject> resultImages_UsedRegion = null) // (20200130) Jeff Revised!
            {
                this.InspImageList = pmInspImageList;
                this.InspRegion = pmInspRegion;
                this.Recipe = pmRecipe;
                this.InspIndex = pmInspIndex;
                this.Resolution = pmResolution;
                this.Type = pmType;
                
                this.ResultImages = resultImages;
                this.ResultImages_UsedRegion = resultImages_UsedRegion;
            }
        }

        /// <summary>
        /// 各AOI演算法執行Task
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public static HObject AlgorithmTask(object Obj) // (20200130) Jeff Revised!
        {
            HObject OutputRegion;
            HOperatorSet.GenEmptyObj(out OutputRegion);

            #region Convert Obj

            List<HObject> InspImageList = ((InspParam)Obj).InspImageList;
            List<HObject> ResultImages = ((InspParam)Obj).ResultImages; // (20200130) Jeff Revised!
            List<HObject> ResultImages_UsedRegion = ((InspParam)Obj).ResultImages_UsedRegion; // (20200130) Jeff Revised!
            InductanceInspRole Recipe = ((InspParam)Obj).Recipe;
            int InspIndex = ((InspParam)Obj).InspIndex;
            double Resolution = ((InspParam)Obj).Resolution;
            enuAlgorithm AlgorithmType = ((InspParam)Obj).Type;
            HObject InspRegion = ((InspParam)Obj).InspRegion;

            #endregion

            try
            {
                #region 輸入至演算法之影像

                HObject SrcImg; // (20200130) Jeff Revised!
                if (Recipe.Param.AlgorithmList[InspIndex].ImageSource == enu_ImageSource.原始)
                    SrcImg = InspImageList[Recipe.Param.AlgorithmList[InspIndex].InspImageIndex];
                else if (Recipe.Param.AlgorithmList[InspIndex].ImageSource == enu_ImageSource.影像A)
                    SrcImg = ResultImages[Recipe.Param.AlgorithmList[InspIndex].InspImageIndex];
                else
                    SrcImg = ResultImages_UsedRegion[Recipe.Param.AlgorithmList[InspIndex].InspImageIndex];

                #endregion

                switch (AlgorithmType)
                {
                    case enuAlgorithm.ThresholdInsp:
                        {
                            #region GetInspParam

                            clsRecipe.clsThresholdInsp Param = (clsRecipe.clsThresholdInsp)Recipe.Param.AlgorithmList[InspIndex].Param;

                            #endregion

                            #region Bypass

                            if (!Param.Enabled)
                            {
                                return OutputRegion;
                            }

                            #endregion

                            HObject Region;
                            ThresholdInsp(SrcImg, InspImageList[Recipe.Param.AlgorithmList[InspIndex].SecondSrcImgID], InspRegion, Param, Recipe.Param.AlgorithmList[InspIndex], Resolution, out OutputRegion, out Region);

                        }
                        break;

                    case enuAlgorithm.LaserHoleInsp:
                        {
                            #region GetInspParam

                            clsRecipe.clsLaserHoleInsp Param = (clsRecipe.clsLaserHoleInsp)Recipe.Param.AlgorithmList[InspIndex].Param;

                            #endregion

                            #region Bypass

                            if (!Param.Enabled)
                            {
                                return OutputRegion;
                            }

                            #endregion

                            HObject CircleRegion, Cross, SearchRect;
                            HTuple Radius;

                            LaserHoleSearch(SrcImg, InspRegion, Param, Recipe.Param.AlgorithmList[InspIndex], out CircleRegion, out Radius, out Cross, out SearchRect);

                            HTuple Mean, GrayMin, DarkRatio;

                            HoleInsp(InspImageList, InspRegion, CircleRegion, Param, Radius, out OutputRegion, out Mean, out GrayMin, out DarkRatio);

                        }
                        break;
                    case enuAlgorithm.TextureInsp:
                        {
                            #region GetInspParam

                            clsRecipe.clsTextureInsp Param = (clsRecipe.clsTextureInsp)Recipe.Param.AlgorithmList[InspIndex].Param;

                            #endregion

                            #region Bypass

                            if (!Param.Enabled)
                            {
                                return OutputRegion;
                            }

                            #endregion

                            if (!File.Exists(Param.ModelPath + "TextureModel.htim"))
                            {
                                throw new Exception("No Model Exists");
                            }

                            HTuple TextureModel;
                            HOperatorSet.ReadTextureInspectionModel(Param.ModelPath + "TextureModel.htim", out TextureModel);

                            TextureInsp(SrcImg, InspRegion, Param, Recipe.Param.AlgorithmList[InspIndex], TextureModel, Resolution, out OutputRegion);

                            HOperatorSet.ClearTextureInspectionModel(TextureModel);
                        }
                        break;
                    case enuAlgorithm.DAVSInsp:
                        {
                            #region GetInspParam

                            clsRecipe.clsDAVSInsp Param = (clsRecipe.clsDAVSInsp)Recipe.Param.AlgorithmList[InspIndex].Param;

                            #endregion

                            #region Bypass

                            if (!Param.Enabled)
                            {
                                return OutputRegion;
                            }

                            #endregion

                            if (Recipe.DAVSInspArray[InspIndex] == null)
                            {
                                throw new Exception("Error...");
                            }

                            if (!DAVSInsp(SrcImg, Param, InspRegion, Recipe.DAVSInspArray[InspIndex], out OutputRegion))
                            {
                                throw new Exception("Insp Fail...");
                            }
                        }
                        break;

                    case enuAlgorithm.LineInsp:
                        {
                            #region GetInspParam

                            clsRecipe.clsLineSearchInsp Param = (clsRecipe.clsLineSearchInsp)Recipe.Param.AlgorithmList[InspIndex].Param;

                            #endregion

                            #region Bypass

                            if (!Param.Enabled)
                            {
                                return OutputRegion;
                            }

                            #endregion

                            if (!LineInsp(SrcImg, InspRegion, Param, Recipe.Param.AlgorithmList[InspIndex], Resolution, out OutputRegion))
                            {
                                throw new Exception("Insp Fail...");
                            }
                        }
                        break;

                    case enuAlgorithm.FeatureInsp:
                        {
                            #region GetInspParam

                            clsRecipe.clsFeatureInsp Param = (clsRecipe.clsFeatureInsp)Recipe.Param.AlgorithmList[InspIndex].Param;

                            #endregion

                            #region Bypass

                            if (!Param.Enabled)
                            {
                                return OutputRegion;
                            }

                            #endregion

                            if (!File.Exists(Param.ModelPath + "classifier"))
                            {
                                throw new Exception("No Model Exists");
                            }

                            HTuple Model;
                            HOperatorSet.ReadClassMlp(Param.ModelPath + "classifier", out Model);

                            if (!MLPInsp(SrcImg, InspRegion, Param, Recipe.Param.AlgorithmList[InspIndex], Model, Resolution, out OutputRegion))
                            {
                                throw new Exception("Insp Fail...");
                            }

                            HOperatorSet.ClearClassMlp(Model);
                        }
                        break;
                }


            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }

            return OutputRegion;
        }

        public static bool MLPInsp(HObject InspImage, HObject InspRegion, clsRecipe.clsFeatureInsp Param, clsRecipe.clsAlgorithm AlgorithmParam, HTuple Model, double Resolution, out HObject DefectRegion)
        {
            bool Status = false;
            HOperatorSet.GenEmptyObj(out DefectRegion);

            try
            {
                HObject ChannelImage = clsStaticTool.GetChannelImage(InspImage,AlgorithmParam.Band);

                HObject InspSkipRegion;
                HOperatorSet.ErosionRectangle1(InspRegion, out InspSkipRegion, Param.EdgeSkipSizeWidth, Param.EdgeSkipSizeHeight);

                HObject ExtRegion;
                HOperatorSet.DilationRectangle1(InspSkipRegion, out ExtRegion, Param.EdgeExtSizeWidth, Param.EdgeExtSizeHeight);

                HObject Union;
                HOperatorSet.Union1(ExtRegion, out Union);

                HObject ReduceImage;
                HOperatorSet.ReduceDomain(ChannelImage, Union, out ReduceImage);

                HObject InspImg = Convert2TrainingImg(ReduceImage, Param.FeatureLaws);

                HObject ClassRegion;
                HOperatorSet.ClassifyImageClassMlp(InspImg, out ClassRegion, Model, 0.5);

                HObject MLPClassRegion;
                HOperatorSet.SelectObj(ClassRegion, out MLPClassRegion, 1);

                HObject Connection;
                HOperatorSet.Connection(MLPClassRegion, out Connection);

                double MaxW = 9999999 * Resolution;
                double MaxA = 9999999 * (Resolution * Resolution);
                SelectDefectRegions(Connection, Param.AEnabled, Param.WEnabled, Param.HEnabled, Param.AreaMin, Param.WidthMin, Param.HeightMin, (int)MaxA, (int)MaxW, (int)MaxW, Resolution, out DefectRegion);
                
                ChannelImage.Dispose();
                InspSkipRegion.Dispose();
                ExtRegion.Dispose();
                Union.Dispose();
                ReduceImage.Dispose();
                InspImg.Dispose();
                ClassRegion.Dispose();
                Connection.Dispose();
                MLPClassRegion.Dispose();
            }
            catch(Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }

            Status = true;
            return Status;

        }

        public static HObject Convert2TrainingImg(HObject SrcImg, List<clsRecipe.clsFeatureLaws> FeatureLawsList)
        {
            HObject Res;
            HOperatorSet.GenEmptyObj(out Res);

            try
            {
                for (int i = 0; i < FeatureLawsList.Count; i++)
                {
                    HObject TransImg;
                    HOperatorSet.TextureLaws(SrcImg, out TransImg, FeatureLawsList[i].FeatureLaws.ToString(), FeatureLawsList[i].Shift, FeatureLawsList[i].Size);
                    HOperatorSet.AppendChannel(Res, TransImg, out Res);
                    TransImg.Dispose();
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }

            return Res;
        }

        public static bool LineSearch(HObject InspImage, HObject InspRegion,enuBand Band,int EdgeSkipSizeWidth,int EdgeSkipSizeHeight,int EdgeExtSizeWidth,int EdgeExtSizeHeight,
                                      double Sigma,int Low,int High,string LightDark,string LineModel,out HObject LineXLD)
        {
            bool Status = false;
            HOperatorSet.GenEmptyObj(out LineXLD);
            try
            {
                HObject ChannelImage = clsStaticTool.GetChannelImage(InspImage, Band);

                HObject InspSkipRegion;
                HOperatorSet.ErosionRectangle1(InspRegion, out InspSkipRegion, EdgeSkipSizeWidth, EdgeSkipSizeHeight);

                HObject ExtRegion;
                HOperatorSet.DilationRectangle1(InspSkipRegion, out ExtRegion, EdgeExtSizeWidth, EdgeExtSizeHeight);

                HObject Union;
                HOperatorSet.Union1(ExtRegion, out Union);

                HObject ReduceImage;
                HOperatorSet.ReduceDomain(ChannelImage, Union, out ReduceImage);

                HOperatorSet.LinesGauss(ReduceImage, out LineXLD, Sigma, Low, High, LightDark.ToString(), "false", LineModel.ToString(), "false");

                ChannelImage.Dispose();
                InspSkipRegion.Dispose();
                ExtRegion.Dispose();
                Union.Dispose();
                ReduceImage.Dispose();
            }
            catch
            {
                return Status;
            }

            Status = true;
            return Status;

        }

        public static bool UnionAndSelect(HObject LineXLD,int MaxDistAbs,int MaxDistRel,int MaxShift,double MaxAngle,int LengthMin,int LengthMax,out HObject UnionRegion)
        {
            bool Status = false;
            HOperatorSet.GenEmptyObj(out UnionRegion);
            try
            {
                HObject UnionXLD;
                HOperatorSet.UnionCollinearContoursXld(LineXLD, out UnionXLD, MaxDistAbs, MaxDistRel, MaxShift, MaxAngle, "attr_keep");

                HObject SelectXLD;
                HOperatorSet.SelectShapeXld(UnionXLD, out SelectXLD, "contlength", "and", LengthMin, LengthMax);

                HObject XLDRegion;
                HOperatorSet.GenRegionContourXld(SelectXLD, out XLDRegion, "filled");

                HOperatorSet.DilationCircle(XLDRegion, out UnionRegion, 2);
            }
            catch
            {
                return Status;
            }

            Status = true;
            return Status;
        }

        public static bool LineInsp(HObject InspImage, HObject InspRegion, clsRecipe.clsLineSearchInsp Param, clsRecipe.clsAlgorithm AlgorithmParam, double Resolution, out HObject DefectRegion)
        {
            bool Status = false;

            HOperatorSet.GenEmptyObj(out DefectRegion);

            try
            {
                HObject LineXLD;
                if (!LineSearch(InspImage, InspRegion, AlgorithmParam.Band, Param.EdgeSkipSizeWidth, Param.EdgeSkipSizeHeight, Param.EdgeExtSizeWidth, Param.EdgeExtSizeHeight,
                           Param.Sigma, Param.Low, Param.High, Param.LightDark.ToString(), Param.LineModel.ToString(), out LineXLD))
                {
                    return Status;
                }

                HObject UnionRegion;
                if (!UnionAndSelect(LineXLD, Param.MaxDistAbs, Param.MaxDistRel, Param.MaxShift, Param.MaxAngle, Param.LengthMin, Param.LengthMax, out UnionRegion))
                {
                    return Status;
                }
            
                HObject Connection;
                HOperatorSet.Connection(UnionRegion, out Connection);

                SelectDefectRegions(Connection, Param.AEnabled, Param.WEnabled, Param.HEnabled, Param.AreaMin, Param.WidthMin, Param.HeightMin, Param.AreaMax, Param.WidthMax, Param.HeightMax, Resolution, out DefectRegion);

                Connection.Dispose();
                UnionRegion.Dispose();
                LineXLD.Dispose();
            }
            catch
            {
                return Status;
            }

            Status = true;
            return Status;

        }

        public static bool DAVSInsp(HObject InspImage, clsRecipe.clsDAVSInsp Param, HObject InspRegion, clsDAVSInspMethod DAVS, out HObject DefectRegion)
        {
            bool Status = false;
            HOperatorSet.GenEmptyObj(out DefectRegion);
            try
            {
                DAVS.SetPathParam(ModuleName, SB_ID, MOVE_ID, PartNumber);

                DAVS.InsertPaternMatchSegRegions(InspRegion);

                if (!DAVS.DAVS_execute(Param, InspImage))
                {
                    return Status;
                }

                #region 取得DAVS檢測結果

                GetDAVSInspDefectResult(DAVS.GetPredictionRegionList(), out DefectRegion);

                #endregion

            }
            catch
            {
                return Status;
            }

            Status = true;
            return Status;
        }

        public static bool HoleInsp(List<HObject> InspImageList,HObject OrgInspRegion, HObject InspRegion, clsRecipe.clsLaserHoleInsp Param, HTuple Radius, out HObject DefectCircle, out HTuple Mean, out HTuple GrayMin, out HTuple DarkRatio)
        {
            bool Status = false;
            HOperatorSet.GenEmptyObj(out DefectCircle);
            Mean = 0; GrayMin = 0; DarkRatio = 0;
            if (!Param.Enabled)
            {
                Status = true;
                return Status;
            }
            try
            {
                HObject ConnectionRegion;//檢測Region
                HOperatorSet.Connection(InspRegion, out ConnectionRegion);

                #region 未找到圓區域
                HObject DefectNotFound;
                HOperatorSet.GenEmptyObj(out DefectNotFound);


                if (!Param.bSearchFailPass)
                {
                    HObject Difference;
                    HOperatorSet.Difference(OrgInspRegion, InspRegion, out Difference);

                    HObject Erosion;
                    HOperatorSet.ErosionCircle(Difference, out Erosion, 2);

                    HObject Connection;
                    HOperatorSet.Connection(Erosion, out Connection);


                    HOperatorSet.SelectShape(Connection, out DefectNotFound, new HTuple("holes_num").TupleConcat("circularity").TupleConcat("width").TupleConcat("height"), "and", new HTuple(0).TupleConcat(0.8).TupleConcat(5).TupleConcat(5), new HTuple(0.9).TupleConcat(1).TupleConcat(999999999).TupleConcat(999999999999));
                    Difference.Dispose();
                    Connection.Dispose();
                }
                #endregion

                #region 直徑

                HObject DefectRadius;
                HOperatorSet.GenEmptyObj(out DefectRadius);

                if (Param.bRadius)
                {
                    HObject DefectMin;
                    HOperatorSet.GenEmptyObj(out DefectMin);

                    HTuple RadiusMin = Param.RadiusMin * 1.0;

                    HTuple SubMin = (Radius - RadiusMin);

                    HTuple SgnMin;

                    HOperatorSet.TupleSgn(SubMin, out SgnMin);

                    HTuple DefectIndexMin;
                    HOperatorSet.TupleFind(SgnMin, -1, out DefectIndexMin);

                    if (DefectIndexMin.Length > 0 && DefectIndexMin != -1)
                    {
                        HOperatorSet.SelectObj(ConnectionRegion, out DefectMin, DefectIndexMin + 1);
                    }


                    HObject DefectMax;
                    HOperatorSet.GenEmptyObj(out DefectMax);

                    HTuple RadiusMax = Param.RadiusMax * 1.0;

                    HTuple SubMax = Radius - RadiusMax;

                    HTuple SgnMax;

                    HOperatorSet.TupleSgn(SubMax, out SgnMax);

                    HTuple DefectIndexMax;
                    HOperatorSet.TupleFind(SgnMax, 1, out DefectIndexMax);
                    if (DefectIndexMax.Length > 0 && DefectIndexMax != -1)
                    {
                        HOperatorSet.SelectObj(ConnectionRegion, out DefectMax, DefectIndexMax + 1);
                    }

                    HOperatorSet.ConcatObj(DefectMax, DefectMin, out DefectRadius);

                 
                }

                #endregion

                #region 取得Feature

                #region 平均值

                HObject MeanInspImg = clsStaticTool.GetChannelImage(InspImageList[Param.MeanID], Param.MeanBand);

                HOperatorSet.GrayFeatures(ConnectionRegion, MeanInspImg, "mean", out Mean);

                HObject DefectMean;
                HOperatorSet.GenEmptyObj(out DefectMean);
                if (Param.bMeanEnabled)
                {
                    HTuple MeanMin = Param.MeanMin;

                    HTuple Sub = Mean - MeanMin;

                    HTuple Sgn;

                    HOperatorSet.TupleSgn(Sub, out Sgn);

                    HTuple DefectIndex;
                    HOperatorSet.TupleFind(Sgn, -1, out DefectIndex);

                    if (DefectIndex.Length > 0 && DefectIndex != -1)
                        HOperatorSet.SelectObj(ConnectionRegion, out DefectMean, DefectIndex + 1);
                }

                #endregion

                #region 最小值

                HObject GrayMinInspImg = clsStaticTool.GetChannelImage(InspImageList[Param.MinID], Param.GrayMinBand);

                HOperatorSet.GrayFeatures(ConnectionRegion, GrayMinInspImg, "min", out GrayMin);
                
                HObject DefectGrayMin;

                HOperatorSet.GenEmptyObj(out DefectGrayMin);

                if (Param.bGrayMin)
                {
                    HTuple GrayMinMin = Param.GrayMin;

                    HTuple Sub = GrayMin - GrayMinMin;

                    HTuple Sgn;

                    HOperatorSet.TupleSgn(Sub, out Sgn);

                    HTuple DefectIndex;
                    HOperatorSet.TupleFind(Sgn, -1, out DefectIndex);
                    if (DefectIndex.Length > 0 && DefectIndex != -1)
                        HOperatorSet.SelectObj(ConnectionRegion, out DefectGrayMin, DefectIndex + 1);
                }

                #endregion

                #region 黑色比例

                HObject UnionRegion;
                HOperatorSet.Union1(InspRegion, out UnionRegion);

                HObject DarkRatioImg;
                HOperatorSet.ReduceDomain(clsStaticTool.GetChannelImage(InspImageList[Param.RatioID], Param.DarkRatioBand), UnionRegion, out DarkRatioImg);

                HObject ThRegion, DynRegion;
                HOperatorSet.GenEmptyObj(out DynRegion);
                HOperatorSet.GenEmptyObj(out ThRegion);

                if (Param.ThresholdEnabled)
                    HOperatorSet.Threshold(DarkRatioImg, out ThRegion, Param.ThresholdMin, Param.ThresholdMax);

                if (Param.bDynThreshold)
                {
                    HObject MeanImg;
                    HOperatorSet.MeanImage(DarkRatioImg, out MeanImg, Param.MeanImgWidth, Param.MeanImgHeight);

                    HOperatorSet.DynThreshold(DarkRatioImg, MeanImg, out DynRegion, Param.DynOffset, "dark");

                    MeanImg.Dispose();
                }

                HObject DarkRegion;
                HOperatorSet.Union2(ThRegion, DynRegion, out DarkRegion);

                HObject DarkConnection;
                HOperatorSet.Connection(DarkRegion, out DarkConnection);
                HOperatorSet.SetSystem("tsp_store_empty_region", "true");
                HObject InterSection;
                HOperatorSet.Intersection(ConnectionRegion, DarkConnection, out InterSection);

                HTuple AllArea, AllRow, AllColumn;
                HTuple DarkArea, DarkRow, DarkColumn;

                HOperatorSet.AreaCenter(ConnectionRegion, out AllArea, out AllRow, out AllColumn);
                HOperatorSet.AreaCenter(InterSection, out DarkArea, out DarkRow, out DarkColumn);

                HTuple DAllArea = AllArea / 1.0;
                HTuple DDarkArea = DarkArea / 1.0;

                HTuple RemoveIndex;
                HOperatorSet.TupleFind(DAllArea, 0, out RemoveIndex);

                if (RemoveIndex.Length > 0 && RemoveIndex != -1)
                {
                    HOperatorSet.TupleRemove(DAllArea, RemoveIndex, out DAllArea);
                    HOperatorSet.TupleRemove(DDarkArea, RemoveIndex, out DDarkArea);
                }

                DarkRatio = (DDarkArea / DAllArea) * 100.0;

                HObject DefectDarkRatio;
                HOperatorSet.GenEmptyObj(out DefectDarkRatio);

                if (Param.bDarkRatio)
                {
                    HTuple DarkRatioMin = Param.DarkRatioMin;

                    HTuple DarkSub = DarkRatio - DarkRatioMin;

                    HTuple Sgn;

                    HOperatorSet.TupleSgn(DarkSub, out Sgn);

                    HTuple DefectIndex;
                    HOperatorSet.TupleFind(Sgn, 1, out DefectIndex);
                    if (DefectIndex.Length > 0 && DefectIndex != -1)
                        HOperatorSet.SelectObj(ConnectionRegion, out DefectDarkRatio, DefectIndex + 1);
                }

                #endregion

                #endregion

                #region 整合結果

                HObject TmpResult;
                HOperatorSet.GenEmptyObj(out TmpResult);

                HOperatorSet.ConcatObj(TmpResult, DefectNotFound, out TmpResult);
                HOperatorSet.ConcatObj(TmpResult, DefectMean, out TmpResult);
                HOperatorSet.ConcatObj(TmpResult, DefectGrayMin, out TmpResult);
                HOperatorSet.ConcatObj(TmpResult, DefectDarkRatio, out TmpResult);
                HOperatorSet.ConcatObj(TmpResult, DefectRadius, out TmpResult);

                HTuple PartitionCount;
                HOperatorSet.CountObj(OrgInspRegion, out PartitionCount);

                if (Param.bAcceptable_ValueEnabled)
                {
                    for (int i = 1; i <= PartitionCount; i++)
                    {
                        HObject SelectRegion;
                        HOperatorSet.SelectObj(OrgInspRegion, out SelectRegion, i);

                        HObject InterSectionReg;
                        HOperatorSet.Intersection(SelectRegion, TmpResult, out InterSectionReg);

                        HObject Connection;
                        HOperatorSet.Connection(InterSectionReg, out Connection);

                        HTuple DefectCount;
                        HOperatorSet.CountObj(Connection, out DefectCount);

                        if (DefectCount > Param.Acceptable_Value)
                        {
                            HOperatorSet.ConcatObj(InterSectionReg, DefectCircle, out DefectCircle);
                        }

                        SelectRegion.Dispose();
                        InterSectionReg.Dispose();
                        Connection.Dispose();
                    }
                }
                else
                    DefectCircle = TmpResult.Clone();

                #endregion
                
                InterSection.Dispose();
                DynRegion.Dispose();
                DarkRegion.Dispose();
                ThRegion.Dispose();
                ConnectionRegion.Dispose();
                MeanInspImg.Dispose();
                GrayMinInspImg.Dispose();
                DarkRatioImg.Dispose();
                DarkConnection.Dispose();


            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }

            Status = true;
            return Status;
        }

        public static bool LaserHoleSearch(HObject InspImage, HObject InspRegion, clsRecipe.clsLaserHoleInsp Param, clsRecipe.clsAlgorithm AlgorithmParam, out HObject CircleRegion,out HTuple Radius,out HObject Cross,out HObject SearchRect)
        {
            bool Status = false;
            HOperatorSet.GenEmptyObj(out CircleRegion);
            HOperatorSet.GenEmptyObj(out Cross);
            HOperatorSet.GenEmptyObj(out SearchRect);
            Radius = 0;//找到之圓形半徑(pixel)

            
            try
            {
                HObject InspectionImage = clsStaticTool.GetChannelImage(InspImage, AlgorithmParam.Band);

                HTuple ImageWidth, ImageHeight;
                HOperatorSet.GetImageSize(InspImage, out ImageWidth, out ImageHeight);
                
                HObject ConnectionRegion;
                HOperatorSet.Connection(InspRegion, out ConnectionRegion);

                HObject Erosion, Dilation;
                HOperatorSet.ErosionRectangle1(ConnectionRegion, out Erosion, Param.EdgeSkipSizeWidth, Param.EdgeSkipSizeHeight);
                HOperatorSet.DilationRectangle1(Erosion, out Dilation, Param.EdgeExtSizeWidth, Param.EdgeExtSizeHeight);

                HTuple InspArea, InspRow, InspColumn;
                HOperatorSet.AreaCenter(Dilation, out InspArea, out InspRow, out InspColumn);

                if (InspArea.Length <= 0)
                {
                    return true;
                }

                HTuple SearchCircleRadius;
                HOperatorSet.TupleGenConst(InspArea.Length, Param.SearchCircleRadius, out SearchCircleRadius);

                HTuple SearchRadiusTolerance = Param.SearchRadiusTolerance;

                HTuple MetrologyHandle;
                HOperatorSet.CreateMetrologyModel(out MetrologyHandle);

                HTuple SearchCircleIndex;
                HOperatorSet.SetMetrologyModelImageSize(MetrologyHandle, ImageWidth, ImageHeight);
                HOperatorSet.AddMetrologyObjectCircleMeasure(MetrologyHandle, InspRow, InspColumn, SearchCircleRadius, SearchRadiusTolerance, Param.measureLength2, Param.MeasureSigma, Param.MeasureThreshold, new HTuple(), new HTuple(), out SearchCircleIndex);

                HTuple num_instances = Param.num_instances;
                HTuple Trans = Param.Trans.ToString();
                HTuple min_score = Param.min_score;

                HOperatorSet.SetMetrologyObjectParam(MetrologyHandle, SearchCircleIndex, "num_instances", num_instances);
                HOperatorSet.SetMetrologyObjectParam(MetrologyHandle, SearchCircleIndex, "measure_transition", Trans);
                HOperatorSet.SetMetrologyObjectParam(MetrologyHandle, SearchCircleIndex, "min_score", min_score);

                HOperatorSet.ApplyMetrologyModel(InspectionImage, MetrologyHandle);

                HTuple RadiusCalc;
                HOperatorSet.GetMetrologyObjectResult(MetrologyHandle, SearchCircleIndex, "all", "result_type", "radius", out RadiusCalc);

                HObject CircleContour;
                HOperatorSet.GetMetrologyObjectResultContour(out CircleContour, MetrologyHandle, "all", "all", 1);
                
                HTuple RowCross, ColumnCross;
                HOperatorSet.GetMetrologyObjectMeasures(out SearchRect, MetrologyHandle, "all", "all", out RowCross, out ColumnCross);
                HOperatorSet.GenCrossContourXld(out Cross, RowCross, ColumnCross, 3, 0.785);

                HObject CircleFindRegion;
                HOperatorSet.GenRegionContourXld(CircleContour, out CircleFindRegion, "filled");

                HObject ErosionRegion;
                HOperatorSet.ErosionCircle(CircleFindRegion, out ErosionRegion, Param.ErosionSize);
                
                if (Param.bSearchSizePass)
                    HOperatorSet.SelectShape(ErosionRegion, out CircleRegion, "radius", "and", Param.SearchRadiusMin, Param.SearchRadiusMax);
                else
                    CircleRegion = ErosionRegion.Clone();

                HOperatorSet.RegionFeatures(CircleRegion, "radius", out Radius);

                CircleFindRegion.Dispose();
                ErosionRegion.Dispose();
                CircleContour.Dispose();
                InspectionImage.Dispose();
                Erosion.Dispose();
                Dilation.Dispose();
                ConnectionRegion.Dispose();
                HOperatorSet.ClearMetrologyModel(MetrologyHandle);
            }
            catch(Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }

            Status = true;
            return Status;

        }

        /// <summary>
        /// 針對檢測區域做二值化
        /// </summary>
        /// <param name="ThImage"></param>
        /// <param name="InspRegion"></param>
        /// <param name="Param"></param>
        /// <param name="ThRegion"></param>
        /// <returns></returns>
        public static bool GetRegionAndThreshold(HObject ThImage, HObject InspRegion, clsRecipe.clsThresholdInsp Param, out HObject ThRegion)
        {
            bool Status = false;
            HOperatorSet.GenEmptyObj(out ThRegion);
            try
            {

                HObject Union;
                HOperatorSet.Union1(InspRegion, out Union);

                HObject InspSkipRegion, DilationRegion;

                HOperatorSet.GenEmptyObj(out InspSkipRegion);
                HOperatorSet.GenEmptyObj(out DilationRegion);

                if (Param.bEdgeEdit)
                {
                    HOperatorSet.ErosionRectangle1(Union, out InspSkipRegion, Param.EdgeSkipSizeWidth, Param.EdgeSkipSizeHeight);

                    HOperatorSet.DilationCircle(InspSkipRegion, out DilationRegion, Param.ExtSizeCircle);
                }
                else
                {
                    DilationRegion = Union.Clone();
                }

                HObject ReduceImage;
                HOperatorSet.ReduceDomain(ThImage, DilationRegion, out ReduceImage);


                #region 二值化方法
                switch (Param.ThType)
                {
                    case enuThresholdType.GeneralThreshold:
                        {
                            HOperatorSet.Threshold(ReduceImage, out ThRegion, Param.LTH, Param.HTH);
                        }
                        break;
                    case enuThresholdType.AutoThreshold:
                        {
                            HOperatorSet.AutoThreshold(ReduceImage, out ThRegion, Param.Sigma);

                            HTuple FeatureMean, MeanMin, MeanMax;
                            HOperatorSet.GrayFeatures(ThRegion, ReduceImage, "mean", out FeatureMean);

                            HTuple ObjCount;
                            HOperatorSet.CountObj(ThRegion, out ObjCount);
                            if (ObjCount > 1)
                            {
                                if (Param.SelectMean == 0)
                                {
                                    HOperatorSet.TupleMin(FeatureMean, out MeanMin);
                                    HOperatorSet.SelectGray(ThRegion, ReduceImage, out ThRegion, "mean", "and", 0, MeanMin + 1);
                                }
                                else
                                {
                                    HOperatorSet.TupleMax(FeatureMean, out MeanMax);
                                    HOperatorSet.SelectGray(ThRegion, ReduceImage, out ThRegion, "mean", "and", MeanMax - 1, 255);
                                }
                            }
                            else
                            {
                                ThRegion.Dispose();
                                HOperatorSet.GenEmptyObj(out ThRegion);
                            }
                        }
                        break;
                    case enuThresholdType.DynThreshold:
                        {
                            HObject MeanImage;
                            HOperatorSet.MeanImage(ReduceImage, out MeanImage, Param.GrayMeanWidth, Param.GrayMeanHeight);
                            HOperatorSet.DynThreshold(ReduceImage, MeanImage, out ThRegion, Param.Offset, Param.DynType.ToString());
                            MeanImage.Dispose();
                        }
                        break;
                    case enuThresholdType.BinaryThreshold:
                        {
                            HTuple ThValue;
                            HOperatorSet.BinaryThreshold(ReduceImage, out ThRegion, new HTuple(Param.BinaryThType.ToString()), new HTuple(Param.BinaryThSearchType.ToString()), out ThValue);
                        }
                        break;
                }

                //HOperatorSet.ClosingCircle(ThRegion, out ThRegion, 3);
                //HOperatorSet.OpeningCircle(ThRegion, out ThRegion, 1);
                #endregion

                #region Dispose

                ReduceImage.Dispose();
                Union.Dispose();
                DilationRegion.Dispose();
                InspSkipRegion.Dispose();

                #endregion
            }
            catch
            {
                return Status;
            }



            Status = true;
            return Status;

        }

        /// <summary>
        /// Threshold 檢測演算法
        /// </summary>
        /// <param name="InspImage"></param>
        /// <param name="SecondSrcImg"></param>
        /// <param name="InspRegion"></param>
        /// <param name="Param"></param>
        /// <param name="AlgorithmParam"></param>
        /// <param name="Resolution"></param>
        /// <param name="DefectRegion"></param>
        /// <param name="FinalSelectRegion"></param>
        /// <returns></returns>
        public static bool ThresholdInsp(HObject InspImage,HObject SecondSrcImg, HObject InspRegion, clsRecipe.clsThresholdInsp Param, clsRecipe.clsAlgorithm AlgorithmParam, double Resolution, out HObject DefectRegion,out HObject FinalSelectRegion)
        {
            bool Status = false;
            HOperatorSet.GenEmptyObj(out DefectRegion);
            HOperatorSet.GenEmptyObj(out FinalSelectRegion);

            try
            {
                HObject ThRegion;
                HOperatorSet.GenEmptyObj(out ThRegion);
                HObject ThImage = clsStaticTool.GetChannelImage(InspImage, AlgorithmParam.Band);

                GetRegionAndThreshold(ThImage, InspRegion, Param, out ThRegion);
                
                #region 特徵篩選

                HObject Connection;
                HOperatorSet.Connection(ThRegion, out Connection);

                HObject SelectGray;
                if (Param.GrayEnabled)
                {
                    HOperatorSet.SelectGray(Connection, ThImage, out SelectGray, Param.GraySelectType.ToString(), "and", Param.GrayMin, Param.GrayMax);
                }
                else
                    SelectGray = Connection;

                HObject SelectCircle;
                if (Param.CircularityEnabled)
                {
                    HOperatorSet.SelectShape(SelectGray, out SelectCircle, "circularity", "and", Param.CircularityMin, Param.CircularityMax);
                }
                else
                    SelectCircle = SelectGray;

                HObject SelectRoundness;
                if (Param.RoundnessEnabled)
                {
                    HOperatorSet.SelectShape(SelectCircle, out SelectRoundness, "roundness", "and", Param.RoundnessMin, Param.RoundnessMax);
                }
                else
                    SelectRoundness = SelectCircle;

                HObject SelectConvexity;
                if (Param.ConvexityEnabled)
                {
                    HOperatorSet.SelectShape(SelectRoundness, out SelectConvexity, "convexity", "and", Param.ConvexityMin, Param.ConvexityMax);
                }
                else
                    SelectConvexity = SelectRoundness;

                HObject Selectrectangularity;

                if (Param.RectangularityEnabled)
                {
                    HOperatorSet.SelectShape(SelectConvexity, out Selectrectangularity, "rectangularity", "and", Param.RectangularityMin, Param.RectangularityMax);
                }
                else
                    Selectrectangularity = SelectConvexity;

                HObject SelectHoleNums;
                if (Param.Holes_numEnabled)
                {
                    HOperatorSet.SelectShape(Selectrectangularity, out SelectHoleNums, "holes_num", "and", Param.Holes_numMin, Param.Holes_numMax);
                }
                else
                    SelectHoleNums = Selectrectangularity;

                HObject SelectInnerCircle;

                if (Param.bInnerCircleEnabled)
                {
                    HOperatorSet.SelectShape(SelectHoleNums, out SelectInnerCircle, "inner_radius", "and", Param.Inner_radiusMin, Param.Inner_radiusMax);
                }
                else
                    SelectInnerCircle = SelectHoleNums;

                HObject anisometry;
                if (Param.banisometryEnabled)
                {
                    HOperatorSet.SelectShape(SelectInnerCircle, out anisometry, "anisometry", "and", Param.anisometryMin, Param.anisometryMax);
                }
                else
                    anisometry = SelectInnerCircle;

                #endregion

                //HObject FinalSelectRegion;
                SelectDefectRegionsOperation(anisometry, Param.AEnabled, Param.WEnabled, Param.HEnabled, Param.AreaMin, Param.WidthMin, Param.HeightMin, Param.AreaMax, Param.WidthMax, Param.HeightMax, Param.AreaOperation, Param.WidthOperation, Param.HeightOperation, Resolution, out FinalSelectRegion);

                #region 第二張影像覆判條件

                HObject SecondSelectGray;

                if (Param.bSecondSrcEnabled)
                {
                    HObject ConnectionRegion;

                    HOperatorSet.Connection(FinalSelectRegion, out ConnectionRegion);

                    HObject SecondImg = clsStaticTool.GetChannelImage(SecondSrcImg, AlgorithmParam.SecondImgBand);

                    HOperatorSet.SelectGray(ConnectionRegion, SecondImg, out SecondSelectGray, Param.SecondSrcGraySelectType.ToString(), "and", Param.SecondSrcGrayMin, Param.SecondSrcGrayMax);

                    SecondImg.Dispose();
                }
                else
                    SecondSelectGray = FinalSelectRegion;

                #endregion

                #region 允收值條件

                HOperatorSet.SetSystem("tsp_store_empty_region", "false");

                HTuple PartitionCount;
                HOperatorSet.CountObj(InspRegion, out PartitionCount);

                if (Param.bAcceptable_ValueEnabled)
                {
                    for (int i = 1; i <= PartitionCount; i++)
                    {
                        HObject SelectRegionTmp;
                        HOperatorSet.SelectObj(InspRegion, out SelectRegionTmp, i);

                        HObject RConnection, RDilation, RErosion;
                        HOperatorSet.Connection(SelectRegionTmp, out RConnection);

                        HOperatorSet.GenEmptyObj(out RDilation);
                        HOperatorSet.GenEmptyObj(out RErosion);

                        if (Param.bEdgeEdit)
                        {
                            HOperatorSet.ErosionRectangle1(RConnection, out RErosion, Param.EdgeSkipSizeWidth, Param.EdgeSkipSizeHeight);

                            HOperatorSet.DilationCircle(RErosion, out RDilation, Param.ExtSizeCircle);
                        }
                        else
                        {
                            RDilation = RConnection.Clone();
                        }

                        HObject InterSectionReg;
                        HOperatorSet.Intersection(RDilation, SecondSelectGray, out InterSectionReg);

                        if (Param.AEnabled)
                            HOperatorSet.SelectShape(InterSectionReg, out InterSectionReg, "area", "and", Param.AreaMin / (Resolution * Resolution), Param.AreaMax / (Resolution * Resolution));


                        HTuple DefectCount;
                        HOperatorSet.CountObj(InterSectionReg, out DefectCount);

                        if (DefectCount > Param.Acceptable_Value)
                        {
                            HOperatorSet.ConcatObj(InterSectionReg, DefectRegion, out DefectRegion);
                        }

                        SelectRegionTmp.Dispose();
                        InterSectionReg.Dispose();
                        RConnection.Dispose();
                        RDilation.Dispose();
                        RErosion.Dispose();
                    }
                }
                else
                    DefectRegion = SecondSelectGray.Clone();

                #endregion
                
                #region Dispose

                SelectCircle.Dispose();
                SelectGray.Dispose();
                SelectRoundness.Dispose();
                SelectConvexity.Dispose();
                Selectrectangularity.Dispose();
                SelectHoleNums.Dispose();
                ThImage.Dispose();
                Connection.Dispose();
                SelectInnerCircle.Dispose();
                //SecondSelectGray.Dispose();

                #endregion
            }
            catch
            {
                return Status;
            }

            Status = true;
            return Status;
        }

        public static void SelectGray(HObject ConnectionRegion,bool MeanEnabled,bool MinEnabled,bool MaxEnabled,int MeanMin,int MeanMax,int MinMin,int MinMax,int MaxMin,int MaxMax,out HObject SelectRegion)
        {
            HOperatorSet.GenEmptyObj(out SelectRegion);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InspImage"></param>
        /// <param name="InspRegion"></param>
        /// <param name="Param"></param>
        /// <param name="AlgorithmParam"></param>
        /// <param name="TextureModel"></param>
        /// <param name="Resolution"></param>
        /// <param name="DefectRegion"></param>
        /// <param name="b_TextureInspTest">是否只做Texture Inspection測試</param>
        /// <returns></returns>
        public static bool TextureInsp(HObject InspImage, HObject InspRegion, clsRecipe.clsTextureInsp Param, clsRecipe.clsAlgorithm AlgorithmParam, HTuple TextureModel, double Resolution, out HObject DefectRegion, bool b_TextureInspTest = false) // (20200319) Jeff Revised!
        {
            bool Status = false;

            HOperatorSet.GenEmptyObj(out DefectRegion);

            if (!Param.Enabled && b_TextureInspTest == false)
            {
                Status = true;
                return Status;
            }

            try
            {
                HObject ResultRegion;
                HOperatorSet.GenEmptyObj(out ResultRegion);
                HObject ChannelImage = clsStaticTool.GetChannelImage(InspImage, AlgorithmParam.Band);

                HObject InspSkipRegion;
                HOperatorSet.ErosionRectangle1(InspRegion, out InspSkipRegion, Param.EdgeSkipSizeWidth, Param.EdgeSkipSizeHeight);

                HObject ExtRegion;
                HOperatorSet.DilationRectangle1(InspSkipRegion, out ExtRegion, Param.EdgeExtSizeWidth, Param.EdgeExtSizeHeight);

                HObject Union;
                HOperatorSet.Union1(ExtRegion, out Union);

                HObject ReduceImage;
                HOperatorSet.ReduceDomain(ChannelImage, Union, out ReduceImage);

                // Texture Inspection
                HTuple ResultID;
                HOperatorSet.ApplyTextureInspectionModel(ReduceImage, out ResultRegion, TextureModel, out ResultID);

                if (b_TextureInspTest) // (20200319) Jeff Revised!
                {
                    DefectRegion = ResultRegion;

                    #region Dispose

                    //ResultRegion.Dispose();
                    ChannelImage.Dispose();
                    InspSkipRegion.Dispose();
                    ExtRegion.Dispose();
                    Union.Dispose();
                    ReduceImage.Dispose();
                    HOperatorSet.ClearTextureInspectionResult(ResultID);

                    #endregion

                    return true;
                }

                // 檢測ROI (20200319) Jeff Revised!
                if (Param.ROI_Enabled)
                {
                    HObject roi;
                    HOperatorSet.ErosionRectangle1(Union, out roi, Param.ROI_SkipWidth, Param.ROI_SkipHeight);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.Intersection(roi, ResultRegion, out ExpTmpOutVar_0);
                        ResultRegion.Dispose();
                        ResultRegion = ExpTmpOutVar_0;
                    }
                    roi.Dispose();
                }

                HObject Connection;
                HOperatorSet.Connection(ResultRegion, out Connection);

                HObject SelectRegion;

                double MaxW = 9999999 * Resolution;
                double MaxA = 9999999 * (Resolution * Resolution);
                SelectDefectRegions(Connection, Param.AEnabled, Param.WEnabled, Param.HEnabled, Param.LowArea, Param.LowWidth, Param.LowHeight, (int)MaxA, (int)MaxW, (int)MaxW, Resolution, out SelectRegion);

                if (Param.anisometryEnabled)
                {
                    HOperatorSet.SelectShape(SelectRegion, out DefectRegion, (new HTuple("anisometry")), "and", Param.anisometryMin, 99999999999);
                }
                else
                    DefectRegion = SelectRegion.Clone();

                #region Dispose

                ResultRegion.Dispose();
                InspSkipRegion.Dispose();
                ReduceImage.Dispose();
                Union.Dispose();
                ChannelImage.Dispose();
                ExtRegion.Dispose();
                SelectRegion.Dispose();
                HOperatorSet.ClearTextureInspectionResult(ResultID);

                #endregion
            }
            catch
            {
                return Status;
            }

            Status = true;
            return Status;
        }

        /// <summary>
        /// 【取得結果】
        /// </summary>
        /// <param name="InspImage"></param>
        /// <param name="InspRegion"></param>
        /// <param name="Param"></param>
        /// <param name="AlgorithmParam"></param>
        /// <param name="TextureModel"></param>
        /// <param name="Image"></param>
        /// <param name="Region"></param>
        /// <returns></returns>
        public static bool TextureLevelResult(HObject InspImage, HObject InspRegion, clsRecipe.clsTextureInsp Param, clsRecipe.clsAlgorithm AlgorithmParam, HTuple TextureModel, out HObject Image, out HObject Region) // (20200319) Jeff Revised!
        {
            bool Status = false;

            HOperatorSet.GenEmptyObj(out Image);
            HOperatorSet.GenEmptyObj(out Region);
            //if (!Param.Enabled) // (20200319) Jeff Revised!
            //{
            //    Status = true;
            //    return Status;
            //}

            try
            {
                HOperatorSet.SetTextureInspectionModelParam(TextureModel, "gen_result_handle", "true");
                HObject ResultRegion;
                HOperatorSet.GenEmptyObj(out ResultRegion);
                HObject ChannelImage = clsStaticTool.GetChannelImage(InspImage, AlgorithmParam.Band);

                HObject InspSkipRegion;
                HOperatorSet.ErosionRectangle1(InspRegion, out InspSkipRegion, Param.EdgeSkipSizeWidth, Param.EdgeSkipSizeHeight);

                HObject ExtRegion;
                HOperatorSet.DilationRectangle1(InspSkipRegion, out ExtRegion, Param.EdgeExtSizeWidth, Param.EdgeExtSizeHeight);

                HObject Union;
                HOperatorSet.Union1(ExtRegion, out Union);

                HObject ReduceImage;
                HOperatorSet.ReduceDomain(ChannelImage, Union, out ReduceImage);


                HTuple ResultID;
                HOperatorSet.ApplyTextureInspectionModel(ReduceImage, out ResultRegion, TextureModel, out ResultID);

                HOperatorSet.GetTextureInspectionResultObject(out Image, ResultID, "novelty_score_image");
                HOperatorSet.GetTextureInspectionResultObject(out Region, ResultID, "novelty_region");

                #region Dispose

                InspSkipRegion.Dispose();
                ReduceImage.Dispose();
                Union.Dispose();
                ChannelImage.Dispose();
                ExtRegion.Dispose();
                HOperatorSet.ClearTextureInspectionResult(ResultID);

                #endregion
            }
            catch
            {
                return Status;
            }

            Status = true;
            return Status;
        }

        #region //=====================  AOI Function =====================

        public static void FindDarkDefect(HObject ho_SrcImage,
                                   List<HObject> ImgList,
                                   clsRecipe.clsAOIParam Recipe,
                                   HObject ho_SegmentRegions,
                                   HTuple hv_EdgeSkipSize,
                                   HTuple hv_ThresholdLow,
                                   HTuple hv_ThresholdHigh,
                                   double Resolution,
                                   InductanceInspRole Param,
                                   out HObject ho_DefectRegions)
        {
            HTuple Ch;
            HObject InspImg;
            HOperatorSet.CountChannels(ho_SrcImage, out Ch);
            if (Ch == 1)
                InspImg = ho_SrcImage.Clone();
            else
            {
                if (Param.Param.InnerInspParam.Band == enuBand.R)
                {
                    HOperatorSet.AccessChannel(ho_SrcImage, out InspImg, 1);
                }
                else if (Param.Param.InnerInspParam.Band == enuBand.G)
                {
                    HOperatorSet.AccessChannel(ho_SrcImage, out InspImg, 2);
                }
                else if (Param.Param.InnerInspParam.Band == enuBand.B)
                {
                    HOperatorSet.AccessChannel(ho_SrcImage, out InspImg, 3);
                }
                else
                {
                    HOperatorSet.Rgb1ToGray(ho_SrcImage, out InspImg);
                }
            }

            // Local iconic variables 
            HObject ho_SegmentRegions_ec, ho_RegionUnion;
            HObject ho_ImageReduced, ho_Region, ho_ConnectedRegions;

            // Local control variables 

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_DefectRegions);
            HOperatorSet.GenEmptyObj(out ho_SegmentRegions_ec);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            try
            {
                //*******************************************************************************************************************
                //取出待處理區域
                //*******************************************************************************************************************
                ho_SegmentRegions_ec.Dispose();
                HOperatorSet.ErosionRectangle1(ho_SegmentRegions, out ho_SegmentRegions_ec, hv_EdgeSkipSize * 2, Recipe.EdgeSkipSizeHeight * 2);
                ho_RegionUnion.Dispose();
                HOperatorSet.Union1(ho_SegmentRegions_ec, out ho_RegionUnion);
                HObject MultiThRegions;
                HOperatorSet.GenEmptyObj(out MultiThRegions);
                if (Recipe.MultiTHList.Count > 0)
                {
                    for (int i = 0; i < Recipe.MultiTHList.Count; i++)
                    {
                        HObject InspI;
                        if (ImgList.Count <= Recipe.MultiTHList[i].ImageID)
                        {
                            continue;
                        }
                        HTuple C;
                        HOperatorSet.CountChannels(ImgList[Recipe.MultiTHList[i].ImageID], out C);

                        if (C == 1)
                            InspI = ImgList[Recipe.MultiTHList[i].ImageID].Clone();
                        else
                        {
                            if (Recipe.MultiTHList[i].Band == enuBand.R)
                            {
                                HOperatorSet.AccessChannel(ImgList[Recipe.MultiTHList[i].ImageID], out InspI, 1);
                            }
                            else if (Recipe.MultiTHList[i].Band == enuBand.G)
                            {
                                HOperatorSet.AccessChannel(ImgList[Recipe.MultiTHList[i].ImageID], out InspI, 2);
                            }
                            else if (Recipe.MultiTHList[i].Band == enuBand.B)
                            {
                                HOperatorSet.AccessChannel(ImgList[Recipe.MultiTHList[i].ImageID], out InspI, 3);
                            }
                            else
                            {
                                HOperatorSet.Rgb1ToGray(ho_SrcImage, out InspI);
                            }
                        }

                        ho_ImageReduced.Dispose();
                        HOperatorSet.ReduceDomain(InspI, ho_RegionUnion, out ho_ImageReduced);
                        ho_Region.Dispose();
                        HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, Recipe.MultiTHList[i].LTH, Recipe.MultiTHList[i].HTH);
                        HOperatorSet.ConcatObj(MultiThRegions, ho_Region, out MultiThRegions);
                        InspI.Dispose();
                    }
                }

                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(InspImg, ho_RegionUnion, out ho_ImageReduced);

                //*******************************************************************************************************************
                //瑕疵檢測
                //*******************************************************************************************************************
                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, hv_ThresholdLow, hv_ThresholdHigh);
                HOperatorSet.ConcatObj(MultiThRegions, ho_Region, out ho_Region);

                HObject UnionRegion;
                HOperatorSet.Union1(ho_Region, out UnionRegion);
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(UnionRegion, out ho_ConnectedRegions);
                ho_DefectRegions.Dispose();
                double MaxW = 2500000 * Resolution;
                double MaxA = 2500000 * (Resolution * Resolution);
                SelectDefectRegions(ho_ConnectedRegions, Recipe.AEnabled, Recipe.WEnabled, Recipe.HEnabled, Recipe.LowArea, Recipe.LowWidth, Recipe.LowHeight, (int)MaxA, (int)MaxW, (int)MaxW, Resolution, out ho_DefectRegions);
          
                //*******************************************************************************************************************
                UnionRegion.Dispose();
                ho_SegmentRegions_ec.Dispose();
                ho_RegionUnion.Dispose();
                ho_ImageReduced.Dispose();
                ho_Region.Dispose();
                ho_ConnectedRegions.Dispose();
                InspImg.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_SegmentRegions_ec.Dispose();
                ho_RegionUnion.Dispose();
                ho_ImageReduced.Dispose();
                ho_Region.Dispose();
                ho_ConnectedRegions.Dispose();
                InspImg.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void LocalDefectMappping(HObject ho_DefectRegions,
                                        HObject SelectRegions,
                                        out HObject ho_DefectRegionMarks)
        {
            // Local iconic variables 

            HObject ho_SegmentRegions_rec1, ho_SegmentRegions_rec1_dl;
            HObject ho_RegionIntersection;

            // Local control variables 
            HTuple hv_aValue = null, hv_Greater = null, hv_Indices = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_DefectRegionMarks);
            HOperatorSet.GenEmptyObj(out ho_SegmentRegions_rec1);
            HOperatorSet.GenEmptyObj(out ho_SegmentRegions_rec1_dl);
            HOperatorSet.GenEmptyObj(out ho_RegionIntersection);
            try
            {
                //*******************************************************************************************************************
                //計算瑕疵位於的Region範圍
                //*******************************************************************************************************************
                ho_SegmentRegions_rec1.Dispose();
                HOperatorSet.ShapeTrans(SelectRegions, out ho_SegmentRegions_rec1, "rectangle1");
                ho_SegmentRegions_rec1_dl.Dispose();
                ho_RegionIntersection.Dispose();
                HOperatorSet.Intersection(ho_SegmentRegions_rec1, ho_DefectRegions, out ho_RegionIntersection);
                HOperatorSet.RegionFeatures(ho_RegionIntersection, "area", out hv_aValue);
                HOperatorSet.TupleGreaterEqualElem(hv_aValue, 1, out hv_Greater);
                HOperatorSet.TupleFind(hv_Greater, 1, out hv_Indices);

                ho_DefectRegionMarks.Dispose();
                HOperatorSet.GenEmptyObj(out ho_DefectRegionMarks);
                if ((int)(new HTuple(hv_Indices.TupleNotEqual(-1))) != 0)
                {
                    ho_DefectRegionMarks.Dispose();
                    HOperatorSet.SelectObj(ho_SegmentRegions_rec1, out ho_DefectRegionMarks, hv_Indices + 1);
                }

                //*******************************************************************************************************************
                ho_SegmentRegions_rec1.Dispose();
                ho_SegmentRegions_rec1_dl.Dispose();
                ho_RegionIntersection.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_SegmentRegions_rec1.Dispose();
                ho_SegmentRegions_rec1_dl.Dispose();
                ho_RegionIntersection.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public static void FindLightDefect(HObject ho_SrcImage,
                                    List<HObject> ImgList,
                                    clsRecipe.clsAOIParam Recipe, 
                                    HObject ho_PatternRegions,
                                    HObject SegRectRegions,
                                    HTuple hv_EdgeSkipSize, 
                                    HTuple hv_ThresholdLow, 
                                    HTuple hv_ThresholdHigh, 
                                    double Resolution,
                                    InductanceInspRole Param,
                                    out HObject ho_DefectRegions)
        {
            //HObject SrcImg_R, SrcImg_G, SrcImg_B;
            //HOperatorSet.GenEmptyObj(out SrcImg_R);
            //HOperatorSet.GenEmptyObj(out SrcImg_G);
            //HOperatorSet.GenEmptyObj(out SrcImg_B);

            HTuple Ch;
            HObject InspImg;
            HOperatorSet.CountChannels(ho_SrcImage, out Ch);
            if (Ch == 1)
                InspImg = ho_SrcImage.Clone();
            else
            {
                //HOperatorSet.Decompose3(ho_SrcImage, out SrcImg_R, out SrcImg_G, out SrcImg_B);
                if (Param.Param.SegParam.BandIndex == 0)
                {
                    HOperatorSet.AccessChannel(ho_SrcImage, out InspImg, 1);
                    //InspImg = SrcImg_R;
                }
                else if (Param.Param.SegParam.BandIndex == 1)
                {
                    HOperatorSet.AccessChannel(ho_SrcImage, out InspImg, 2);
                    //InspImg = SrcImg_G;
                }
                else if (Param.Param.SegParam.BandIndex == 2)
                {
                    HOperatorSet.AccessChannel(ho_SrcImage, out InspImg, 3);
                    //InspImg = SrcImg_B;
                }
                else
                {
                    HOperatorSet.Rgb1ToGray(ho_SrcImage, out InspImg);
                }
            }

            // Local iconic variables 
            HObject ho_SegmentRegions_dc, ho_SegmentRegions_dc_U;
            HObject ho_SegmentRegions_rec1;
            HObject ho_SegmentRegions_rec1_dl_U, ho_RegionDifference;
            HObject ho_ImageReduced, ho_Region, ho_ConnectedRegions;

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_DefectRegions);
            HOperatorSet.GenEmptyObj(out ho_SegmentRegions_dc);
            HOperatorSet.GenEmptyObj(out ho_SegmentRegions_dc_U);
            HOperatorSet.GenEmptyObj(out ho_SegmentRegions_rec1);
            HOperatorSet.GenEmptyObj(out ho_SegmentRegions_rec1_dl_U);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            try
            {
                //*******************************************************************************************************************
                //取出待處理區域
                //*******************************************************************************************************************
                ho_SegmentRegions_dc.Dispose();
                HOperatorSet.DilationRectangle1(ho_PatternRegions, out ho_SegmentRegions_dc, Recipe.EdgeSkipSizeWidth * 2, Recipe.EdgeSkipSizeHeight * 2);
                //HOperatorSet.DilationCircle(ho_PatternRegions, out ho_SegmentRegions_dc, hv_EdgeSkipSize);
                ho_SegmentRegions_dc_U.Dispose();
                HOperatorSet.Union1(ho_SegmentRegions_dc, out ho_SegmentRegions_dc_U);
                //ho_SegmentRegions_rec1_dl.Dispose();
                //HOperatorSet.DilationRectangle1(SegRectRegions, out ho_SegmentRegions_rec1_dl, hv_XbackgroudDilationSize, hv_YbackgroudDilationSize);
                ho_SegmentRegions_rec1_dl_U.Dispose();
                HOperatorSet.Union1(SegRectRegions, out ho_SegmentRegions_rec1_dl_U);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_SegmentRegions_rec1_dl_U, ho_SegmentRegions_dc_U, out ho_RegionDifference);

                HObject MultiThRegions;
                HOperatorSet.GenEmptyObj(out MultiThRegions);
                if (Recipe.MultiTHList.Count > 0)
                {
                    for (int i = 0; i < Recipe.MultiTHList.Count; i++)
                    {
                        HObject InspI;
                        //HOperatorSet.GenEmptyObj(out R);
                        //HOperatorSet.GenEmptyObj(out G);
                        //HOperatorSet.GenEmptyObj(out B);
                        if (ImgList.Count <= Recipe.MultiTHList[i].ImageID)
                        {
                            //R.Dispose();
                            //G.Dispose();
                            //B.Dispose();
                            continue;
                        }
                        HTuple C;
                        HOperatorSet.CountChannels(ImgList[Recipe.MultiTHList[i].ImageID], out C);

                        if (C == 1)
                            InspI = ImgList[Recipe.MultiTHList[i].ImageID].Clone();
                        else
                        {
                            //HOperatorSet.Decompose3(ImgList[Recipe.MultiTHList[i].ImageID], out R, out G, out B);
                            if (Param.Param.SegParam.BandIndex == 0)
                            {
                                HOperatorSet.AccessChannel(ImgList[Recipe.MultiTHList[i].ImageID], out InspI, 1);
                                //InspI = R;
                            }
                            else if (Param.Param.SegParam.BandIndex == 1)
                            {
                                HOperatorSet.AccessChannel(ImgList[Recipe.MultiTHList[i].ImageID], out InspI, 2);
                                //InspI = G;
                            }
                            else if (Param.Param.SegParam.BandIndex == 2)
                            {
                                HOperatorSet.AccessChannel(ImgList[Recipe.MultiTHList[i].ImageID], out InspI, 3);
                                //InspI = B;
                            }
                            else
                            {
                                HOperatorSet.Rgb1ToGray(ho_SrcImage, out InspI);
                            }
                        }

                        ho_ImageReduced.Dispose();
                        HOperatorSet.ReduceDomain(InspI, ho_RegionDifference, out ho_ImageReduced);
                        ho_Region.Dispose();
                        HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, Recipe.MultiTHList[i].LTH, Recipe.MultiTHList[i].HTH);
                        HOperatorSet.ConcatObj(MultiThRegions, ho_Region, out MultiThRegions);
                        //R.Dispose();
                        //G.Dispose();
                        //B.Dispose();
                        InspI.Dispose();
                    }
                }


                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(InspImg, ho_RegionDifference, out ho_ImageReduced);

                //*******************************************************************************************************************
                //瑕疵檢測
                //*******************************************************************************************************************
                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, hv_ThresholdLow, hv_ThresholdHigh);
                HOperatorSet.ConcatObj(MultiThRegions, ho_Region, out ho_Region);

                HObject UnionRegion;
                HOperatorSet.Union1(ho_Region, out UnionRegion);
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(UnionRegion, out ho_ConnectedRegions);
                ho_DefectRegions.Dispose();

                double MaxW = 2500000 * Resolution;
                double MaxA = 2500000 * (Resolution * Resolution);
                SelectDefectRegions(ho_ConnectedRegions, Recipe.AEnabled, Recipe.WEnabled, Recipe.HEnabled, Recipe.LowArea, Recipe.LowWidth, Recipe.LowHeight, (int)MaxA, (int)MaxW, (int)MaxW, Resolution, out ho_DefectRegions);

                ho_SegmentRegions_dc.Dispose();
                ho_SegmentRegions_dc_U.Dispose();
                ho_SegmentRegions_rec1.Dispose();
                ho_SegmentRegions_rec1_dl_U.Dispose();
                ho_RegionDifference.Dispose();
                ho_ImageReduced.Dispose();
                ho_Region.Dispose();
                ho_ConnectedRegions.Dispose();
                //SrcImg_R.Dispose();
                //SrcImg_G.Dispose();
                //SrcImg_B.Dispose();
                InspImg.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_SegmentRegions_dc.Dispose();
                ho_SegmentRegions_dc_U.Dispose();
                ho_SegmentRegions_rec1.Dispose();
                ho_SegmentRegions_rec1_dl_U.Dispose();
                ho_RegionDifference.Dispose();
                ho_ImageReduced.Dispose();
                ho_Region.Dispose();
                ho_ConnectedRegions.Dispose();
                //SrcImg_R.Dispose();
                //SrcImg_G.Dispose();
                //SrcImg_B.Dispose();
                InspImg.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public class OuterInspParam
        {
            public HObject SrcImg;
            public List<HObject> ImgList;
            public HObject PatternRegions;
            public HObject SegRegions;
            public InductanceInspRole Recipe;

            public OuterInspParam(HObject pmSrcImg, List<HObject> pmImgList, HObject pmPatternRegions, HObject pmSegRegions, InductanceInspRole pmRecipe)
            {
                this.SrcImg = pmSrcImg;
                this.ImgList = pmImgList;
                this.PatternRegions = pmPatternRegions;
                this.SegRegions = pmSegRegions;
                this.Recipe = pmRecipe;
            }
        }

        public static HObject OuterInspTask(object Obj)
        {
            HObject DefectRegion;
            HOperatorSet.GenEmptyObj(out DefectRegion);
            try
            {
                #region Convert Obj

                HObject SrcImg = ((OuterInspParam)Obj).SrcImg;
                List<HObject> ImgList = ((OuterInspParam)Obj).ImgList;
                HObject PatternRegions = ((OuterInspParam)Obj).PatternRegions;
                HObject SegRegions = ((OuterInspParam)Obj).SegRegions;
                InductanceInspRole Recipe = ((OuterInspParam)Obj).Recipe;

                #endregion

                if (SrcImg == null)
                {
                    return DefectRegion;
                }

                if (!Recipe.Param.OuterInspParam.Enabled)
                {
                    return DefectRegion;
                }

                if (Recipe.Param.OuterInspParam.LowThreshold >= Recipe.Param.OuterInspParam.HighThreshold)
                {
                    return DefectRegion;
                }

                FindLightDefect(SrcImg, ImgList, Recipe.Param.OuterInspParam,
                                PatternRegions,
                                SegRegions,
                                Recipe.Param.OuterInspParam.EdgeSkipSizeWidth,
                                Recipe.Param.OuterInspParam.LowThreshold,
                                Recipe.Param.OuterInspParam.HighThreshold,
                                Locate_Method_FS.pixel_resolution_, Recipe,
                                out DefectRegion);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return DefectRegion;
        }

        public bool OuterInsp_Action(HObject pmSrcImg,List<HObject> ImgList, HObject PatternRegions, HObject SegRegions, InductanceInspRole Recipe, out HObject DefectRegion, out HObject MapRegions)
        {
            bool Res = true;
            HOperatorSet.GenEmptyObj(out DefectRegion);
            HOperatorSet.GenEmptyObj(out MapRegions);
            if (pmSrcImg == null)
            {
                Res = false;
                return Res;
            }

            if (!Recipe.Param.OuterInspParam.Enabled)
            {
                return Res;
            }

            HObject SrcImg = pmSrcImg;
            
            if (Recipe.Param.OuterInspParam.LowThreshold >= Recipe.Param.OuterInspParam.HighThreshold)
            {
                Res = false;
                return Res;
            }

            FindLightDefect(SrcImg, ImgList, Recipe.Param.OuterInspParam,
                            PatternRegions,
                            SegRegions,
                            Recipe.Param.OuterInspParam.EdgeSkipSizeWidth,
                            Recipe.Param.OuterInspParam.LowThreshold,
                            Recipe.Param.OuterInspParam.HighThreshold,
                            Locate_Method_FS.pixel_resolution_, Recipe,
                            out DefectRegion);

            LocalDefectMappping(DefectRegion, SegRegions, out MapRegions);
            
            return Res;
        }


        public class InnerInspParam
        {
            public HObject SrcImg;
            public List<HObject> ImgList;
            public HObject PatternRegions;
            public HObject SegRegions;
            public InductanceInspRole Recipe;

            public InnerInspParam(HObject pmSrcImg, List<HObject> pmImgList, HObject pmPatternRegions, HObject pmSegRegions, InductanceInspRole pmRecipe)
            {
                this.SrcImg = pmSrcImg;
                this.ImgList = pmImgList;
                this.PatternRegions = pmPatternRegions;
                this.SegRegions = pmSegRegions;
                this.Recipe = pmRecipe;
            }
        }
        public static HObject InnerInspTask(object Obj)
        {
            HObject DefectRegion;
            HOperatorSet.GenEmptyObj(out DefectRegion);
            try
            {
                #region Convert Obj

                HObject SrcImg = ((InnerInspParam)Obj).SrcImg;
                List<HObject> ImgList = ((InnerInspParam)Obj).ImgList;
                HObject PatternRegions = ((InnerInspParam)Obj).PatternRegions;
                HObject SegRegions = ((InnerInspParam)Obj).SegRegions;
                InductanceInspRole Recipe = ((InnerInspParam)Obj).Recipe;

                #endregion

                if (SrcImg == null)
                {
                    return DefectRegion;
                }

                if (!Recipe.Param.InnerInspParam.Enabled)
                {
                    return DefectRegion;
                }

                if (Recipe.Param.InnerInspParam.LowThreshold >= Recipe.Param.InnerInspParam.HighThreshold)
                {
                    return DefectRegion;
                }

                FindDarkDefect(SrcImg, ImgList, Recipe.Param.InnerInspParam, PatternRegions,
                               Recipe.Param.InnerInspParam.EdgeSkipSizeWidth,
                               Recipe.Param.InnerInspParam.LowThreshold,
                               Recipe.Param.InnerInspParam.HighThreshold,
                               Locate_Method_FS.pixel_resolution_, Recipe,
                               out DefectRegion);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return DefectRegion;
        }

        public bool InnerInsp_Action(HObject pmSrcImg, List<HObject> ImgList, HObject PatternRegions,HObject SegRegions, InductanceInspRole Recipe, out HObject DefectRegion, out HObject MapRegions)
        {
            bool Res = true;
            HOperatorSet.GenEmptyObj(out DefectRegion);
            HOperatorSet.GenEmptyObj(out MapRegions);
            if (pmSrcImg == null)
            {
                Res = false;
                return Res;
            }

            if (!Recipe.Param.InnerInspParam.Enabled)
            {
                return Res;
            }
            HObject SrcImg = pmSrcImg;

            if (Recipe.Param.InnerInspParam.LowThreshold >= Recipe.Param.InnerInspParam.HighThreshold)
            {
                Res = false;
                return Res;
            }

            FindDarkDefect(SrcImg, ImgList, Recipe.Param.InnerInspParam, PatternRegions,
                           Recipe.Param.InnerInspParam.EdgeSkipSizeWidth,
                           Recipe.Param.InnerInspParam.LowThreshold,
                           Recipe.Param.InnerInspParam.HighThreshold,
                           Locate_Method_FS.pixel_resolution_, Recipe,
                           out DefectRegion);

            LocalDefectMappping(DefectRegion, SegRegions, out MapRegions);

            
            return Res;
        }

        #endregion
    }
}
