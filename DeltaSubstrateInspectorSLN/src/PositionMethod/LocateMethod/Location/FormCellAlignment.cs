using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace DeltaSubstrateInspector.src.PositionMethod.LocateMethod.Location
{

    public partial class FormCellAlignment : Form
    {
        #region Class & enum
        public enum enuCorner
        {
            LeftTop,
            LeftBot,
            RightTop,
            RightBot,
        }
        [Serializable]
        public class clsCellAlignmentParam
        {
            public bool Enabled = false;

            public clsROI LeftTop_ROI = new clsROI();
            public clsROI LeftBot_ROI = new clsROI();
            public clsROI RightTop_ROI = new clsROI();
            public clsROI RightBot_ROI = new clsROI();

            public int RowCount = 0;
            public int ColumnCount = 0;

        }
        [Serializable]
        public class clsROI
        {
            public int Width = 0;
            public int Height = 0;
            public double CenterX = 0.0;
            public double CenterY = 0.0;

            public clsROI() { }

            public clsROI(int pmWidth, int pmHeight, double pmCenterX, double pmCenterY)
            {
                this.Width = pmWidth;
                this.Height = pmHeight;
                this.CenterX = pmCenterX;
                this.CenterY = pmCenterY;
            }

            public PointD GetCorner(enuCorner Corner)
            {
                PointD Res = new PointD(0, 0);
                switch (Corner)
                {
                    case enuCorner.LeftBot:
                        {
                            Res.X = CenterX - Width / 2.0;
                            Res.Y = CenterY + Height / 2.0;
                        }
                        break;
                    case enuCorner.LeftTop:
                        {
                            Res.X = CenterX - Width / 2.0;
                            Res.Y = CenterY - Height / 2.0;
                        }
                        break;
                    case enuCorner.RightBot:
                        {
                            Res.X = CenterX + Width / 2.0;
                            Res.Y = CenterY + Height / 2.0;
                        }
                        break;
                    case enuCorner.RightTop:
                        {
                            Res.X = CenterX + Width / 2.0;
                            Res.Y = CenterY - Height / 2.0;
                        }
                        break;
                    default:
                        {
                            Res.X = -1;
                            Res.Y = -1;
                        }
                        break;
                }
                return Res;
            }
        }

        [Serializable]
        public class PointD
        {
            public double X { get; set; }
            public double Y { get; set; }
            public PointD() { }
            public PointD(double p_X, double p_Y)
            {
                this.X = p_X;
                this.Y = p_Y;
            }
        }

        #endregion

        #region 變數設定
        private HTuple ModeID;
        private clsCellAlignmentParam Param = new clsCellAlignmentParam();
        private string ParamPath = "";
        private HObject SrcImage = null;
        private HDrawingObject drawing_Rect = new HDrawingObject(100, 100, 210, 210);

        #endregion

        #region Function
        public FormCellAlignment(HObject pmSrcImg,string pmParamPath)
        {
            InitializeComponent();
            this.hSmartWindowControl_map.MouseWheel += hSmartWindowControl_map.HSmartWindowControl_MouseWheel;
            this.SrcImage = pmSrcImg.Clone();
            this.ParamPath = pmParamPath;
            Param = LoadParam(ParamPath);
            if (SrcImage != null)
                HOperatorSet.DispObj(SrcImage, hSmartWindowControl_map.HalconWindow);
            UpdateControl();
        }

        public void UpdateControl()
        {

        }

        public clsCellAlignmentParam LoadParam(string PathFile)
        {
            clsCellAlignmentParam Res = new clsCellAlignmentParam();
            try
            {
                XmlSerializer XmlS = new XmlSerializer(Res.GetType());
                Stream S = File.Open(PathFile, FileMode.Open);
                Res = (clsCellAlignmentParam)XmlS.Deserialize(S);
                S.Close();

            }
            catch (Exception e)
            {

            }
            return Res;
        }

        public void SaveParam(clsCellAlignmentParam SrcProduct, string PathFile)
        {
            clsCellAlignmentParam Product = clsStaticTool.Clone<clsCellAlignmentParam>(SrcProduct);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(PathFile));

            try
            {
                XmlSerializer XmlS = new XmlSerializer(Product.GetType());
                Stream S = File.Open(PathFile, FileMode.Create);
                XmlS.Serialize(S, Product);
                S.Close();
            }
            catch (Exception e)
            {

            }
        }

        private void EnableROIBtn(Control ParentCtrl, Control TriggerCtrl, bool Enabled)
        {
            foreach (Control Ctrl in ParentCtrl.Controls)
            {
                if ((Ctrl == TriggerCtrl) || (Ctrl == this.panelDisplay) || Ctrl == this.hSmartWindowControl_map)
                    continue;

                //child control
                EnableROIBtn(Ctrl, TriggerCtrl, Enabled);

                if ((Ctrl is Panel) || (Ctrl is GroupBox))
                    continue;

                Ctrl.Enabled = Enabled;
            }
        }

        private void ChangeColor(Color C, CheckBox cbx)
        {
            cbx.BackColor = C;
        }

        public void GetRectData(HTuple Rect, out HTuple column1, out HTuple row1, out HTuple column2, out HTuple row2)
        {
            HOperatorSet.GetDrawingObjectParams(Rect, new HTuple("column1"), out column1);
            HOperatorSet.GetDrawingObjectParams(Rect, new HTuple("row1"), out row1);
            HOperatorSet.GetDrawingObjectParams(Rect, new HTuple("column2"), out column2);
            HOperatorSet.GetDrawingObjectParams(Rect, new HTuple("row2"), out row2);
        }

        public void DisplayROIRegions()
        {
            HOperatorSet.SetDraw(hSmartWindowControl_map.HalconWindow, "margin");
            HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, "green");

            #region LT
            if (Param.LeftTop_ROI.Width != 0 && Param.LeftTop_ROI.Height != 0)
            {
                HObject Region;
                PointD LT = Param.LeftTop_ROI.GetCorner(enuCorner.LeftTop);
                PointD RB = Param.LeftTop_ROI.GetCorner(enuCorner.RightBot);
                HOperatorSet.GenRectangle1(out Region, LT.Y, LT.X, RB.Y, RB.X);
                HOperatorSet.DispObj(Region, hSmartWindowControl_map.HalconWindow);
            }
            #endregion

            #region LB
            if (Param.LeftBot_ROI.Width != 0 && Param.LeftBot_ROI.Height != 0)
            {
                HObject Region;
                PointD LT = Param.LeftBot_ROI.GetCorner(enuCorner.LeftTop);
                PointD RB = Param.LeftBot_ROI.GetCorner(enuCorner.RightBot);
                HOperatorSet.GenRectangle1(out Region, LT.Y, LT.X, RB.Y, RB.X);
                HOperatorSet.DispObj(Region, hSmartWindowControl_map.HalconWindow);
            }
            #endregion

            #region RT
            if (Param.RightTop_ROI.Width != 0 && Param.RightTop_ROI.Height != 0)
            {
                HObject Region;
                PointD LT = Param.RightTop_ROI.GetCorner(enuCorner.LeftTop);
                PointD RB = Param.RightTop_ROI.GetCorner(enuCorner.RightBot);
                HOperatorSet.GenRectangle1(out Region, LT.Y, LT.X, RB.Y, RB.X);
                HOperatorSet.DispObj(Region, hSmartWindowControl_map.HalconWindow);
            }
            #endregion

            #region RB
            if (Param.RightBot_ROI.Width != 0 && Param.RightBot_ROI.Height != 0)
            {
                HObject Region;
                PointD LT = Param.RightBot_ROI.GetCorner(enuCorner.LeftTop);
                PointD RB = Param.RightBot_ROI.GetCorner(enuCorner.RightBot);
                HOperatorSet.GenRectangle1(out Region, LT.Y, LT.X, RB.Y, RB.X);
                HOperatorSet.DispObj(Region, hSmartWindowControl_map.HalconWindow);
            }
            #endregion
        }

        public bool Matching(HObject SrcImg, HObject LTRegion, HObject LBRegion, HObject RTRegion, HObject RBRegion, HTuple ModelID, out HObject LT, out HObject LB, out HObject RT, out HObject RB)
        {
            bool Res = false;

            #region 變數宣告

            HOperatorSet.GenEmptyObj(out LT);
            HOperatorSet.GenEmptyObj(out LB);
            HOperatorSet.GenEmptyObj(out RT);
            HOperatorSet.GenEmptyObj(out RB);

            HObject ho_TransContours;
            HTuple hv_HomMat2D = null;
            HObject ho_Region = null;

            HOperatorSet.GenEmptyObj(out ho_TransContours);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HObject ho_ModelContours;
            HOperatorSet.GenEmptyObj(out ho_ModelContours);
            ho_ModelContours.Dispose();
            HOperatorSet.GetShapeModelContours(out ho_ModelContours, ModelID, 1);

            #endregion

            try
            {
                #region LT

                HObject ReduceImg;
                HTuple Row, Column, Score, angle;
                HOperatorSet.ReduceDomain(SrcImg, LTRegion, out ReduceImg);
                HOperatorSet.FindShapeModel(ReduceImg, ModelID, -3, 6, 0.5, 0, 0, "none", 0, 0.9, out Row, out Column, out angle, out Score);

                HObject PatternRegions;
                HOperatorSet.GenEmptyRegion(out PatternRegions);
                HTuple hv_I;
                for (hv_I = 0; (int)hv_I <= (int)((new HTuple(Score.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                {
                    HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
                    HOperatorSet.HomMat2dRotate(hv_HomMat2D, angle.TupleSelect(hv_I), 0, 0, out hv_HomMat2D);
                    HOperatorSet.HomMat2dTranslate(hv_HomMat2D, Row.TupleSelect(hv_I), Column.TupleSelect(hv_I), out hv_HomMat2D);

                    ho_TransContours.Dispose();
                    HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_TransContours, hv_HomMat2D);
                    ho_Region.Dispose();
                    HOperatorSet.GenRegionContourXld(ho_TransContours, out ho_Region, "filled");

                    HObject Un1;
                    HOperatorSet.Union1(ho_Region, out Un1);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(PatternRegions, Un1, out ExpTmpOutVar_0);
                        PatternRegions.Dispose();
                        PatternRegions = ExpTmpOutVar_0;
                    }
                    Un1.Dispose();
                }

                LT = PatternRegions.Clone();
                #endregion

                #region RB

                ReduceImg.Dispose();
                HOperatorSet.ReduceDomain(SrcImg, RBRegion, out ReduceImg);
                HOperatorSet.FindShapeModel(ReduceImg, ModelID, -3, 6, 0.5, 0, 0, "none", 0, 0.9, out Row, out Column, out angle, out Score);

                PatternRegions.Dispose();
                HOperatorSet.GenEmptyRegion(out PatternRegions);
                for (hv_I = 0; (int)hv_I <= (int)((new HTuple(Score.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                {
                    HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
                    HOperatorSet.HomMat2dRotate(hv_HomMat2D, angle.TupleSelect(hv_I), 0, 0, out hv_HomMat2D);
                    HOperatorSet.HomMat2dTranslate(hv_HomMat2D, Row.TupleSelect(hv_I), Column.TupleSelect(hv_I), out hv_HomMat2D);

                    ho_TransContours.Dispose();
                    HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_TransContours, hv_HomMat2D);
                    ho_Region.Dispose();
                    HOperatorSet.GenRegionContourXld(ho_TransContours, out ho_Region, "filled");

                    HObject Un1;
                    HOperatorSet.Union1(ho_Region, out Un1);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(PatternRegions, Un1, out ExpTmpOutVar_0);
                        PatternRegions.Dispose();
                        PatternRegions = ExpTmpOutVar_0;
                    }
                    Un1.Dispose();
                }

                RB = PatternRegions.Clone();
                #endregion

                #region LB

                ReduceImg.Dispose();
                HOperatorSet.ReduceDomain(SrcImg, LBRegion, out ReduceImg);
                HOperatorSet.FindShapeModel(ReduceImg, ModelID, -3, 6, 0.5, 0, 0, "none", 0, 0.9, out Row, out Column, out angle, out Score);
                HTuple IsEmpty;
                HObject EmptyRegion;
                HOperatorSet.GenEmptyObj(out EmptyRegion);
                PatternRegions.Dispose();
                HOperatorSet.GenEmptyObj(out PatternRegions);
                for (hv_I = 0; (int)hv_I <= (int)((new HTuple(Score.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                {
                    HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
                    HOperatorSet.HomMat2dRotate(hv_HomMat2D, angle.TupleSelect(hv_I), 0, 0, out hv_HomMat2D);
                    HOperatorSet.HomMat2dTranslate(hv_HomMat2D, Row.TupleSelect(hv_I), Column.TupleSelect(hv_I), out hv_HomMat2D);

                    ho_TransContours.Dispose();
                    HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_TransContours, hv_HomMat2D);
                    ho_Region.Dispose();
                    HOperatorSet.GenRegionContourXld(ho_TransContours, out ho_Region, "filled");

                    HObject Un1;
                    HOperatorSet.Union1(ho_Region, out Un1);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.TestEqualRegion(PatternRegions, EmptyRegion, out IsEmpty);
                        if (IsEmpty.Length > 0)
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

                 LB = PatternRegions.Clone();
                #endregion

                #region RT

                ReduceImg.Dispose();
                HOperatorSet.ReduceDomain(SrcImg, RTRegion, out ReduceImg);
                HOperatorSet.FindShapeModel(ReduceImg, ModelID, -3, 6, 0.2, 0, 0, "none", 0, 0.9, out Row, out Column, out angle, out Score);

                PatternRegions.Dispose();
                HOperatorSet.GenEmptyObj(out PatternRegions);
                for (hv_I = 0; (int)hv_I <= (int)((new HTuple(Score.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                {
                    HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
                    HOperatorSet.HomMat2dRotate(hv_HomMat2D, angle.TupleSelect(hv_I), 0, 0, out hv_HomMat2D);
                    HOperatorSet.HomMat2dTranslate(hv_HomMat2D, Row.TupleSelect(hv_I), Column.TupleSelect(hv_I), out hv_HomMat2D);

                    ho_TransContours.Dispose();
                    HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_TransContours, hv_HomMat2D);
                    ho_Region.Dispose();
                    HOperatorSet.GenRegionContourXld(ho_TransContours, out ho_Region, "filled");

                    HObject Un1;
                    HOperatorSet.Union1(ho_Region, out Un1);
                    {
                        HOperatorSet.TestEqualRegion(PatternRegions, EmptyRegion, out IsEmpty);
                        if (IsEmpty.Length > 0)
                            PatternRegions = Un1.Clone();
                        else
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(PatternRegions, Un1, out ExpTmpOutVar_0);
                            PatternRegions.Dispose();
                            PatternRegions = ExpTmpOutVar_0;
                        }
                    }
                    Un1.Dispose();
                }

                RT = PatternRegions.Clone();
                #endregion
            }
            catch
            {
                return Res;
            }


            Res = true;
            return Res;
        }

        public bool FindShapeModleCenter(HObject SrcImg,HObject LTRegion, HObject LBRegion, HObject RTRegion, HObject RBRegion, HTuple ModelID,out HObject LT,out HObject LB,out HObject RT,out HObject RB)
        {
            bool Res = false;

            #region 變數宣告

            HOperatorSet.GenEmptyObj(out LT);
            HOperatorSet.GenEmptyObj(out LB);
            HOperatorSet.GenEmptyObj(out RT);
            HOperatorSet.GenEmptyObj(out RB);

            HObject ho_TransContours;
            HTuple hv_HomMat2D = null;
            HObject ho_Region = null;

            HOperatorSet.GenEmptyObj(out ho_TransContours);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HObject ho_ModelContours;
            HOperatorSet.GenEmptyObj(out ho_ModelContours);
            ho_ModelContours.Dispose();
            HOperatorSet.GetShapeModelContours(out ho_ModelContours, ModelID, 1);

            #endregion

            try
            {
                #region LT

                HObject ReduceImg;
                HTuple Row, Column, Score, angle;
                HOperatorSet.ReduceDomain(SrcImg, LTRegion, out ReduceImg);
                HOperatorSet.FindShapeModel(ReduceImg, ModelID, -3, 6, 0.5, 0, 0, "none", 0, 0.9, out Row, out Column, out angle, out Score);

                HObject PatternRegions;
                HOperatorSet.GenEmptyRegion(out PatternRegions);
                HTuple hv_I;
                for (hv_I = 0; (int)hv_I <= (int)((new HTuple(Score.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                {
                    HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
                    HOperatorSet.HomMat2dRotate(hv_HomMat2D, angle.TupleSelect(hv_I), 0, 0, out hv_HomMat2D);
                    HOperatorSet.HomMat2dTranslate(hv_HomMat2D, Row.TupleSelect(hv_I), Column.TupleSelect(hv_I), out hv_HomMat2D);

                    ho_TransContours.Dispose();
                    HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_TransContours, hv_HomMat2D);
                    ho_Region.Dispose();
                    HOperatorSet.GenRegionContourXld(ho_TransContours, out ho_Region, "filled");

                    HObject Un1;
                    HOperatorSet.Union1(ho_Region, out Un1);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(PatternRegions, Un1, out ExpTmpOutVar_0);
                        PatternRegions.Dispose();
                        PatternRegions = ExpTmpOutVar_0;
                    }
                    Un1.Dispose();
                }

                HObject SortRegions;
                HOperatorSet.SortRegion(PatternRegions, out SortRegions, "character", "true", "row");

                HOperatorSet.SelectObj(SortRegions, out LT, 1);
                #endregion

                #region RB

                ReduceImg.Dispose();
                HOperatorSet.ReduceDomain(SrcImg, RBRegion, out ReduceImg);
                HOperatorSet.FindShapeModel(ReduceImg, ModelID, -3, 6, 0.5, 0, 0, "none", 0, 0.9, out Row, out Column, out angle, out Score);

                PatternRegions.Dispose();
                HOperatorSet.GenEmptyRegion(out PatternRegions);
                for (hv_I = 0; (int)hv_I <= (int)((new HTuple(Score.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                {
                    HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
                    HOperatorSet.HomMat2dRotate(hv_HomMat2D, angle.TupleSelect(hv_I), 0, 0, out hv_HomMat2D);
                    HOperatorSet.HomMat2dTranslate(hv_HomMat2D, Row.TupleSelect(hv_I), Column.TupleSelect(hv_I), out hv_HomMat2D);

                    ho_TransContours.Dispose();
                    HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_TransContours, hv_HomMat2D);
                    ho_Region.Dispose();
                    HOperatorSet.GenRegionContourXld(ho_TransContours, out ho_Region, "filled");

                    HObject Un1;
                    HOperatorSet.Union1(ho_Region, out Un1);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(PatternRegions, Un1, out ExpTmpOutVar_0);
                        PatternRegions.Dispose();
                        PatternRegions = ExpTmpOutVar_0;
                    }
                    Un1.Dispose();
                }

                SortRegions.Dispose();
                HOperatorSet.SortRegion(PatternRegions, out SortRegions, "character", "true", "row");
                HTuple ObjCount;
                HOperatorSet.CountObj(SortRegions, out ObjCount);
                HOperatorSet.SelectObj(SortRegions, out RB, ObjCount);
                #endregion

                #region LB

                ReduceImg.Dispose();
                HOperatorSet.ReduceDomain(SrcImg, LBRegion, out ReduceImg);
                HOperatorSet.FindShapeModel(ReduceImg, ModelID, -3, 6, 0.5, 0, 0, "none", 0, 0.9, out Row, out Column, out angle, out Score);
                HTuple IsEmpty;
                HObject EmptyRegion;
                HOperatorSet.GenEmptyObj(out EmptyRegion);
                PatternRegions.Dispose();
                HOperatorSet.GenEmptyObj(out PatternRegions);
                for (hv_I = 0; (int)hv_I <= (int)((new HTuple(Score.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                {
                    HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
                    HOperatorSet.HomMat2dRotate(hv_HomMat2D, angle.TupleSelect(hv_I), 0, 0, out hv_HomMat2D);
                    HOperatorSet.HomMat2dTranslate(hv_HomMat2D, Row.TupleSelect(hv_I), Column.TupleSelect(hv_I), out hv_HomMat2D);

                    ho_TransContours.Dispose();
                    HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_TransContours, hv_HomMat2D);
                    ho_Region.Dispose();
                    HOperatorSet.GenRegionContourXld(ho_TransContours, out ho_Region, "filled");

                    HObject Un1;
                    HOperatorSet.Union1(ho_Region, out Un1);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.TestEqualRegion(PatternRegions, EmptyRegion, out IsEmpty);
                        if (IsEmpty.Length>0)
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

                HTuple ROI_Column1, ROI_Row1, ROI_W, ROI_H;

                HOperatorSet.RegionFeatures(LBRegion, new HTuple("column1"), out ROI_Column1);
                HOperatorSet.RegionFeatures(LBRegion, new HTuple("row1"), out ROI_Row1);
                HOperatorSet.RegionFeatures(LBRegion, new HTuple("width"), out ROI_W);
                HOperatorSet.RegionFeatures(LBRegion, new HTuple("height"), out ROI_H);


                HOperatorSet.CountObj(PatternRegions, out ObjCount);
                HTuple Select = 0;
                HTuple Min = int.MaxValue;
                HTuple ROI_LeftBotX = ROI_Column1;
                HTuple ROI_LeftBotY = ROI_Row1 + ROI_H;
                for (int i = 1; i <= ObjCount; i++)
                {
                    HObject ObjectSelected;
                    HOperatorSet.SelectObj(PatternRegions, out ObjectSelected, i);
                    HTuple Area, col, row;
                    HOperatorSet.AreaCenter(ObjectSelected, out Area, out col, out row);
                    double Dist = Math.Pow(Math.Pow((ROI_LeftBotX - row), 2) + Math.Pow((ROI_LeftBotY - col), 2), 0.5);
                    //HTuple Dist;
                    //HOperatorSet.DistancePp(row, col, ROI_LeftBotY, ROI_LeftBotX, out Dist);
                    if (Dist < Min)
                    {
                        Select = i;
                        Min = Dist;
                    }
                }

                HOperatorSet.SelectObj(PatternRegions, out LB, Select);
                #endregion

                #region RT

                ReduceImg.Dispose();
                HOperatorSet.ReduceDomain(SrcImg, RTRegion, out ReduceImg);
                HOperatorSet.FindShapeModel(ReduceImg, ModelID, -3, 6, 0.2, 0, 0, "none", 0, 0.9, out Row, out Column, out angle, out Score);

                PatternRegions.Dispose();
                HOperatorSet.GenEmptyObj(out PatternRegions);
                for (hv_I = 0; (int)hv_I <= (int)((new HTuple(Score.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                {
                    HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
                    HOperatorSet.HomMat2dRotate(hv_HomMat2D, angle.TupleSelect(hv_I), 0, 0, out hv_HomMat2D);
                    HOperatorSet.HomMat2dTranslate(hv_HomMat2D, Row.TupleSelect(hv_I), Column.TupleSelect(hv_I), out hv_HomMat2D);

                    ho_TransContours.Dispose();
                    HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_TransContours, hv_HomMat2D);
                    ho_Region.Dispose();
                    HOperatorSet.GenRegionContourXld(ho_TransContours, out ho_Region, "filled");

                    HObject Un1;
                    HOperatorSet.Union1(ho_Region, out Un1);
                    {
                        HOperatorSet.TestEqualRegion(PatternRegions, EmptyRegion, out IsEmpty);
                        if (IsEmpty.Length > 0)
                            PatternRegions = Un1.Clone();
                        else
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(PatternRegions, Un1, out ExpTmpOutVar_0);
                            PatternRegions.Dispose();
                            PatternRegions = ExpTmpOutVar_0;
                        }
                    }
                    Un1.Dispose();
                }


                HOperatorSet.RegionFeatures(RTRegion, new HTuple("column1"), out ROI_Column1);
                HOperatorSet.RegionFeatures(RTRegion, new HTuple("row1"), out ROI_Row1);
                HOperatorSet.RegionFeatures(RTRegion, new HTuple("width"), out ROI_W);
                HOperatorSet.RegionFeatures(RTRegion, new HTuple("height"), out ROI_H);


                HOperatorSet.CountObj(PatternRegions, out ObjCount);
                Select = 0;
                Min = int.MaxValue;
                HTuple ROI_RightTopX = ROI_Column1 + ROI_W;
                HTuple ROI_RightTopY = ROI_Row1;
                for (int i = 1; i <= ObjCount; i++)
                {
                    HObject ObjectSelected;
                    HOperatorSet.SelectObj(PatternRegions, out ObjectSelected, i);
                    HTuple Area, col, row;
                    HOperatorSet.AreaCenter(ObjectSelected, out Area, out col, out row);
                    double Dist = Math.Pow(Math.Pow((ROI_RightTopX - row), 2) + Math.Pow((ROI_RightTopY - col), 2), 0.5);
                    //HTuple Dist;
                    //HOperatorSet.DistancePp(row, col, ROI_RightTopY, ROI_RightTopX, out Dist);
                    if (Dist < Min)
                    {
                        Select = i;
                        Min = Dist;
                    }
                }

                HOperatorSet.SelectObj(PatternRegions, out RT, Select);
                #endregion
            }
            catch
            {
                return Res;
            }


            Res = true;
            return Res;
        }

        public void GetSearchRegion(clsCellAlignmentParam ROIParam, out HObject LT, out HObject RT, out HObject LB, out HObject RB)
        {
            HOperatorSet.GenEmptyObj(out LT);
            HOperatorSet.GenEmptyObj(out RT);
            HOperatorSet.GenEmptyObj(out LB);
            HOperatorSet.GenEmptyObj(out RB);

            if (ROIParam.LeftTop_ROI.Width >= 0 && ROIParam.LeftTop_ROI.Height >= 0)
            {
                PointD Start = ROIParam.LeftTop_ROI.GetCorner(enuCorner.LeftTop);
                PointD End = ROIParam.LeftTop_ROI.GetCorner(enuCorner.RightBot);
                HOperatorSet.GenRectangle1(out LT, Start.Y, Start.X, End.Y, End.X);
            }

            if (ROIParam.LeftBot_ROI.Width >= 0 && ROIParam.LeftBot_ROI.Height >= 0)
            {
                PointD Start = ROIParam.LeftBot_ROI.GetCorner(enuCorner.LeftTop);
                PointD End = ROIParam.LeftBot_ROI.GetCorner(enuCorner.RightBot);
                HOperatorSet.GenRectangle1(out LB, Start.Y, Start.X, End.Y, End.X);
            }

            if (ROIParam.RightTop_ROI.Width >= 0 && ROIParam.RightTop_ROI.Height >= 0)
            {
                PointD Start = ROIParam.RightTop_ROI.GetCorner(enuCorner.LeftTop);
                PointD End = ROIParam.RightTop_ROI.GetCorner(enuCorner.RightBot);
                HOperatorSet.GenRectangle1(out RT, Start.Y, Start.X, End.Y, End.X);
            }

            if (ROIParam.RightBot_ROI.Width >= 0 && ROIParam.RightBot_ROI.Height >= 0)
            {
                PointD Start = ROIParam.RightBot_ROI.GetCorner(enuCorner.LeftTop);
                PointD End = ROIParam.RightBot_ROI.GetCorner(enuCorner.RightBot);
                HOperatorSet.GenRectangle1(out RB, Start.Y, Start.X, End.Y, End.X);
            }
        }

        #endregion

        #region UI事件



        private void cbxLeftTopRegion_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;
            
            if (Obj.Checked)
            {
                #region 設定範圍
                try
                {
                    EnableROIBtn(this, Obj, false);
                    ChangeColor(Color.Green, Obj);
                    if (drawing_Rect != null)
                    {
                        drawing_Rect.Dispose();
                        drawing_Rect = null;
                    }
                    if (Param.LeftTop_ROI.Height!=0 && Param.LeftTop_ROI.Width != 0)
                    {
                        drawing_Rect = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE1,
                                                                      Param.LeftTop_ROI.CenterY - Param.LeftTop_ROI.Height / 2,
                                                                      Param.LeftTop_ROI.CenterX - Param.LeftTop_ROI.Width / 2,
                                                                      Param.LeftTop_ROI.CenterY + Param.LeftTop_ROI.Height / 2,
                                                                      Param.LeftTop_ROI.CenterX + Param.LeftTop_ROI.Width / 2);

                    }
                    else
                    {
                        drawing_Rect = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE1,
                                                                      100,
                                                                      100,
                                                                      200,
                                                                      200);
                    }


                    drawing_Rect.SetDrawingObjectParams("color", "green");
                    hSmartWindowControl_map.HalconWindow.AttachDrawingObjectToWindow(drawing_Rect);
                }
                catch (Exception ex)
                {
                    Obj.CheckedChanged -= cbxLeftTopRegion_CheckedChanged;
                    Obj.Checked = false;
                    Obj.CheckedChanged += cbxLeftTopRegion_CheckedChanged;
                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    EnableROIBtn(this, Obj, true);
                    ChangeColor(Color.LightGray, Obj);
                    MessageBox.Show(ex.ToString());
                }
                #endregion
            }
            else
            {
                #region 寫入設定
                try
                {
                    HObject Hotspot_Rect = null;
                    HTuple row1, column1, row2, column2;
                    GetRectData(drawing_Rect.ID, out column1, out row1, out column2, out row2);
                    HOperatorSet.GenRectangle1(out Hotspot_Rect, row1, column1, row2, column2);

                    Param.LeftTop_ROI.CenterX= (double)(column1 + column2) / 2;
                    Param.LeftTop_ROI.CenterY = (double)(row1 + row2) / 2;
                    Param.LeftTop_ROI.Width = column2 - column1;
                    Param.LeftTop_ROI.Height = row2 - row1;
                    
                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    ChangeColor(Color.LightGray, Obj);
                    EnableROIBtn(this, Obj, true);
                }
                catch (Exception ex)
                {
                    Obj.CheckedChanged -= cbxLeftTopRegion_CheckedChanged;
                    Obj.Checked = false;
                    Obj.CheckedChanged += cbxLeftTopRegion_CheckedChanged;
                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    EnableROIBtn(this, Obj, true);
                    ChangeColor(Color.LightGray, Obj);
                    MessageBox.Show(ex.ToString());
                }
                #endregion
            }
        }

        private void cbxLeftBot_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;

            if (Obj.Checked)
            {
                #region 設定範圍
                try
                {
                    EnableROIBtn(this, Obj, false);
                    ChangeColor(Color.Green, Obj);
                    if (drawing_Rect != null)
                    {
                        drawing_Rect.Dispose();
                        drawing_Rect = null;
                    }
                    if (Param.LeftBot_ROI.Height != 0 && Param.LeftBot_ROI.Width != 0)
                    {
                        drawing_Rect = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE1,
                                                                      Param.LeftBot_ROI.CenterY - Param.LeftBot_ROI.Height / 2,
                                                                      Param.LeftBot_ROI.CenterX - Param.LeftBot_ROI.Width / 2,
                                                                      Param.LeftBot_ROI.CenterY + Param.LeftBot_ROI.Height / 2,
                                                                      Param.LeftBot_ROI.CenterX + Param.LeftBot_ROI.Width / 2);

                    }
                    else
                    {
                        drawing_Rect = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE1,
                                                                      100,
                                                                      100,
                                                                      200,
                                                                      200);
                    }


                    drawing_Rect.SetDrawingObjectParams("color", "green");
                    hSmartWindowControl_map.HalconWindow.AttachDrawingObjectToWindow(drawing_Rect);
                }
                catch (Exception ex)
                {
                    Obj.CheckedChanged -= cbxLeftBot_CheckedChanged;
                    Obj.Checked = false;
                    Obj.CheckedChanged += cbxLeftBot_CheckedChanged;
                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    EnableROIBtn(this, Obj, true);
                    ChangeColor(Color.LightGray, Obj);
                    MessageBox.Show(ex.ToString());
                }
                #endregion
            }
            else
            {
                #region 寫入設定
                try
                {
                    HObject Hotspot_Rect = null;
                    HTuple row1, column1, row2, column2;
                    GetRectData(drawing_Rect.ID, out column1, out row1, out column2, out row2);
                    HOperatorSet.GenRectangle1(out Hotspot_Rect, row1, column1, row2, column2);

                    Param.LeftBot_ROI.CenterX = (double)(column1 + column2) / 2;
                    Param.LeftBot_ROI.CenterY = (double)(row1 + row2) / 2;
                    Param.LeftBot_ROI.Width = column2 - column1;
                    Param.LeftBot_ROI.Height = row2 - row1;

                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    ChangeColor(Color.LightGray, Obj);
                    EnableROIBtn(this, Obj, true);
                }
                catch (Exception ex)
                {
                    Obj.CheckedChanged -= cbxLeftBot_CheckedChanged;
                    Obj.Checked = false;
                    Obj.CheckedChanged += cbxLeftBot_CheckedChanged;
                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    EnableROIBtn(this, Obj, true);
                    ChangeColor(Color.LightGray, Obj);
                    MessageBox.Show(ex.ToString());
                }
                #endregion
            }
        }

        private void cbxRightTop_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;

            if (Obj.Checked)
            {
                #region 設定範圍
                try
                {
                    EnableROIBtn(this, Obj, false);
                    ChangeColor(Color.Green, Obj);
                    if (drawing_Rect != null)
                    {
                        drawing_Rect.Dispose();
                        drawing_Rect = null;
                    }
                    if (Param.RightTop_ROI.Height != 0 && Param.RightTop_ROI.Width != 0)
                    {
                        drawing_Rect = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE1,
                                                                      Param.RightTop_ROI.CenterY - Param.RightTop_ROI.Height / 2,
                                                                      Param.RightTop_ROI.CenterX - Param.RightTop_ROI.Width / 2,
                                                                      Param.RightTop_ROI.CenterY + Param.RightTop_ROI.Height / 2,
                                                                      Param.RightTop_ROI.CenterX + Param.RightTop_ROI.Width / 2);

                    }
                    else
                    {
                        drawing_Rect = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE1,
                                                                      100,
                                                                      100,
                                                                      200,
                                                                      200);
                    }


                    drawing_Rect.SetDrawingObjectParams("color", "green");
                    hSmartWindowControl_map.HalconWindow.AttachDrawingObjectToWindow(drawing_Rect);
                }
                catch (Exception ex)
                {
                    Obj.CheckedChanged -= cbxRightTop_CheckedChanged;
                    Obj.Checked = false;
                    Obj.CheckedChanged += cbxRightTop_CheckedChanged;
                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    EnableROIBtn(this, Obj, true);
                    ChangeColor(Color.LightGray, Obj);
                    MessageBox.Show(ex.ToString());
                }
                #endregion
            }
            else
            {
                #region 寫入設定
                try
                {
                    HObject Hotspot_Rect = null;
                    HTuple row1, column1, row2, column2;
                    GetRectData(drawing_Rect.ID, out column1, out row1, out column2, out row2);
                    HOperatorSet.GenRectangle1(out Hotspot_Rect, row1, column1, row2, column2);

                    Param.RightTop_ROI.CenterX = (double)(column1 + column2) / 2;
                    Param.RightTop_ROI.CenterY = (double)(row1 + row2) / 2;
                    Param.RightTop_ROI.Width = column2 - column1;
                    Param.RightTop_ROI.Height = row2 - row1;

                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    ChangeColor(Color.LightGray, Obj);
                    EnableROIBtn(this, Obj, true);
                }
                catch (Exception ex)
                {
                    Obj.CheckedChanged -= cbxRightTop_CheckedChanged;
                    Obj.Checked = false;
                    Obj.CheckedChanged += cbxRightTop_CheckedChanged;
                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    EnableROIBtn(this, Obj, true);
                    ChangeColor(Color.LightGray, Obj);
                    MessageBox.Show(ex.ToString());
                }
                #endregion
            }
        }

        private void cbxRightBot_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;

            if (Obj.Checked)
            {
                #region 設定範圍
                try
                {
                    EnableROIBtn(this, Obj, false);
                    ChangeColor(Color.Green, Obj);
                    if (drawing_Rect != null)
                    {
                        drawing_Rect.Dispose();
                        drawing_Rect = null;
                    }
                    if (Param.RightBot_ROI.Height != 0 && Param.RightBot_ROI.Width != 0)
                    {
                        drawing_Rect = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE1,
                                                                      Param.RightBot_ROI.CenterY - Param.RightBot_ROI.Height / 2,
                                                                      Param.RightBot_ROI.CenterX - Param.RightBot_ROI.Width / 2,
                                                                      Param.RightBot_ROI.CenterY + Param.RightBot_ROI.Height / 2,
                                                                      Param.RightBot_ROI.CenterX + Param.RightBot_ROI.Width / 2);

                    }
                    else
                    {
                        drawing_Rect = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE1,
                                                                      100,
                                                                      100,
                                                                      200,
                                                                      200);
                    }


                    drawing_Rect.SetDrawingObjectParams("color", "green");
                    hSmartWindowControl_map.HalconWindow.AttachDrawingObjectToWindow(drawing_Rect);
                }
                catch (Exception ex)
                {
                    Obj.CheckedChanged -= cbxRightBot_CheckedChanged;
                    Obj.Checked = false;
                    Obj.CheckedChanged += cbxRightBot_CheckedChanged;
                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    EnableROIBtn(this, Obj, true);
                    ChangeColor(Color.LightGray, Obj);
                    MessageBox.Show(ex.ToString());
                }
                #endregion
            }
            else
            {
                #region 寫入設定
                try
                {
                    HObject Hotspot_Rect = null;
                    HTuple row1, column1, row2, column2;
                    GetRectData(drawing_Rect.ID, out column1, out row1, out column2, out row2);
                    HOperatorSet.GenRectangle1(out Hotspot_Rect, row1, column1, row2, column2);

                    Param.RightBot_ROI.CenterX = (double)(column1 + column2) / 2;
                    Param.RightBot_ROI.CenterY = (double)(row1 + row2) / 2;
                    Param.RightBot_ROI.Width = column2 - column1;
                    Param.RightBot_ROI.Height = row2 - row1;

                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    ChangeColor(Color.LightGray, Obj);
                    EnableROIBtn(this, Obj, true);
                }
                catch (Exception ex)
                {
                    Obj.CheckedChanged -= cbxRightBot_CheckedChanged;
                    Obj.Checked = false;
                    Obj.CheckedChanged += cbxRightBot_CheckedChanged;
                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    EnableROIBtn(this, Obj, true);
                    ChangeColor(Color.LightGray, Obj);
                    MessageBox.Show(ex.ToString());
                }
                #endregion
            }
        }

        private void cbxEnabled_CheckedChanged(object sender, EventArgs e)
        {
            Param.Enabled = cbxEnabled.Checked;
        }

        private void btnDisplayRegions_Click(object sender, EventArgs e)
        {
            DisplayROIRegions();
        }

        #endregion

        private void cbxTeachRegionSetting_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;
            HOperatorSet.ClearWindow(hSmartWindowControl_map.HalconWindow);
            HOperatorSet.DispObj(SrcImage, hSmartWindowControl_map.HalconWindow);


            if (Obj.Checked)
            {
                #region 設定範圍
                try
                {
                    EnableROIBtn(this, Obj, false);
                    ChangeColor(Color.Green, Obj);
                    if (drawing_Rect != null)
                    {
                        drawing_Rect.Dispose();
                        drawing_Rect = null;
                    }

                        drawing_Rect = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE1,
                                                                      100,
                                                                      100,
                                                                      200,
                                                                      200);

                    drawing_Rect.SetDrawingObjectParams("color", "green");
                    hSmartWindowControl_map.HalconWindow.AttachDrawingObjectToWindow(drawing_Rect);
                }
                catch (Exception ex)
                {
                    Obj.CheckedChanged -= cbxTeachRegionSetting_CheckedChanged;
                    Obj.Checked = false;
                    Obj.CheckedChanged += cbxTeachRegionSetting_CheckedChanged;
                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    EnableROIBtn(this, Obj, true);
                    ChangeColor(Color.LightGray, Obj);
                    MessageBox.Show(ex.ToString());
                }
                #endregion
            }
            else
            {
                #region 寫入設定
                try
                {
                    HObject Rect = null;
                    HTuple row1, column1, row2, column2;
                    GetRectData(drawing_Rect.ID, out column1, out row1, out column2, out row2);
                    HOperatorSet.GenRectangle1(out Rect, row1, column1, row2, column2);
                    drawing_Rect.Dispose();
                    drawing_Rect = null;
                    ChangeColor(Color.LightGray, Obj);
                    EnableROIBtn(this, Obj, true);

                    #region Teach Mode

                    HObject ReduceImg;
                    HOperatorSet.ReduceDomain(SrcImage, Rect, out ReduceImg);
                    HOperatorSet.CreateShapeModel(ReduceImg, "auto", -3, 6, "auto", "auto", "use_polarity", "auto", "auto", out ModeID);

                    HObject LTSearchRegion, LBSearchRegion, RTSearchRegion, RBSearchRegion;
                    GetSearchRegion(Param, out LTSearchRegion, out RTSearchRegion, out LBSearchRegion, out RBSearchRegion);
                    HObject LT, LB, RT, RB;
                    FindShapeModleCenter(SrcImage, LTSearchRegion, LBSearchRegion, RTSearchRegion, RBSearchRegion, ModeID, out LT, out LB, out RT, out RB);


                    HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, "red");
                    HOperatorSet.SetDraw(hSmartWindowControl_map.HalconWindow, "margin");
                    HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, "blue");
                    HOperatorSet.DispObj(LT, hSmartWindowControl_map.HalconWindow);
                    HOperatorSet.DispObj(LB, hSmartWindowControl_map.HalconWindow);
                    HOperatorSet.DispObj(RT, hSmartWindowControl_map.HalconWindow);
                    HOperatorSet.DispObj(RB, hSmartWindowControl_map.HalconWindow);


                    #endregion
                }
                catch (Exception ex)
                {
                    Obj.CheckedChanged -= cbxTeachRegionSetting_CheckedChanged;
                    Obj.Checked = false;
                    Obj.CheckedChanged += cbxTeachRegionSetting_CheckedChanged;
                    if (drawing_Rect != null)
                    {
                        drawing_Rect.Dispose();
                        drawing_Rect = null;
                    }
                    EnableROIBtn(this, Obj, true);
                    ChangeColor(Color.LightGray, Obj);
                    MessageBox.Show(ex.ToString());
                }
                #endregion
            }
        }

        private void btnMatchingTest_Click(object sender, EventArgs e)
        {
            HObject LTSearchRegion, LBSearchRegion, RTSearchRegion, RBSearchRegion;
            GetSearchRegion(Param, out LTSearchRegion, out RTSearchRegion, out LBSearchRegion, out RBSearchRegion);
            HObject LT, LB, RT, RB;
            Matching(SrcImage, LTSearchRegion, LBSearchRegion, RTSearchRegion, RBSearchRegion, ModeID, out LT, out LB, out RT, out RB);


            HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, "red");
            HOperatorSet.SetDraw(hSmartWindowControl_map.HalconWindow, "margin");
            HOperatorSet.SetColor(hSmartWindowControl_map.HalconWindow, "blue");
            HOperatorSet.DispObj(LT, hSmartWindowControl_map.HalconWindow);
            HOperatorSet.DispObj(LB, hSmartWindowControl_map.HalconWindow);
            HOperatorSet.DispObj(RT, hSmartWindowControl_map.HalconWindow);
            HOperatorSet.DispObj(RB, hSmartWindowControl_map.HalconWindow);
        }
    }
}
