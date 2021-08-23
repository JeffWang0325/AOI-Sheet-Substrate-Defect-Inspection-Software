using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using System.IO;
using System.Diagnostics;

namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.InductanceInsp
{
    public partial class FormEditRegion : Form
    {
        #region ========================== enum List  ==========================

        public enum enuDisplayRegionType
        {
            fill,
            margin
        }

        public enum enuTools
        {
            RegionSets,
            Morpgology,
            Select
        }

        public enum enuRegionSets
        {
            Union1,
            Union2,
            Difference,
            Intersection,
            Complement,
            Connection,
            fillup,
            Copy,
            Shapetrans,
            FillupShape,
            Concat
        }

        public enum enuMorpgology
        {
            opening_rect1,
            opening_circle,
            closing_rect1,
            closing_circle,
            dilation_rect1,
            dilation_circle,
            erosion_rect1,
            erosion_circle
        }
        
        public enum enuSelect
        {
            Area,
            Width,
            Height,
            GrayMean,
            GrayMin,
            GrayMax,
            GrayDeviation
        }

        #endregion

        #region ==========================  變數設定  ==========================

        int igFormWidth = new int();  //窗口寬度
        int igFormHeight = new int(); //窗口高度
        float fgWidthScaling = new float(); //寬度縮放比例
        float fgHeightScaling = new float(); //高度縮放比例
        List<HObject> InputImageList = new List<HObject>();
        List<clsRecipe.clsMethodRegion> MethodRegion;
        List<clsRecipe.clsMethodRegion> EditRegion;
        enuDisplayRegionType DisplayType = enuDisplayRegionType.fill;
        InductanceInspRole Recipe;

        HObject SelectRegion;
        HTuple hv_Index;
        HTuple SelectIndex = new HTuple();
        HObject UserSetRegion = null;
        HObject ConcatRegion = null;

        #endregion

        #region ==========================  Function  ==========================

        private void ResizeCon(float widthScaling, float heightScaling, Control cons)
        {
            float fTmp = new float();

            foreach (Control con in cons.Controls) //遍歷控件集
            {
                string[] conTag = con.Tag.ToString().Split(new char[] {','});
                fTmp = Convert.ToSingle(conTag[0]) * widthScaling;
                con.Left = (int)fTmp;
                fTmp = Convert.ToSingle(conTag[1]) * heightScaling;
                con.Top = (int)fTmp;
                fTmp = Convert.ToSingle(conTag[2]) * widthScaling;
                con.Width = (int)fTmp;
                fTmp = Convert.ToSingle(conTag[3]) * heightScaling;
                con.Height = (int)fTmp;
                fTmp = Convert.ToSingle(conTag[4]) * widthScaling * heightScaling * 0.8f;
                con.Font = new Font("微軟正黑體", fTmp,FontStyle.Bold);
                if (con.Controls.Count > 0) //處理子控件
                {
                    ResizeCon(widthScaling, heightScaling, con);
                }
            }
        }

        private void InitConTag(Control cons)
        {
            foreach (Control con in cons.Controls) //遍歷控件集
            {
                con.Tag = con.Left + "," + con.Top + "," + con.Width + "," + con.Height + "," + con.Font.Size;
                if (con.Controls.Count > 0) //處理子控件
                {
                    InitConTag(con);
                }
            }
        }

        public FormEditRegion(List<HObject> pmInputImageList, List<clsRecipe.clsMethodRegion> pmMethodRegion, List<clsRecipe.clsMethodRegion> pmEditRegion,InductanceInspRole pmRecipe)
        {
            InitializeComponent();
            HOperatorSet.GenEmptyObj(out SelectRegion);
            igFormWidth = this.Width;
            igFormHeight = this.Height;
            InitConTag(this);

            this.InputImageList = pmInputImageList;
            this.MethodRegion = pmMethodRegion;
            this.EditRegion = pmEditRegion;
            this.Recipe = pmRecipe;

            this.DisplayWindows.MouseWheel += DisplayWindows.HSmartWindowControl_MouseWheel;
            UpdateImageList(InputImageList);
            UpdateRegionList();
        }

        public void UpdateImageList(List<HObject> ImageList)
        {
            for (int i = 0; i < ImageList.Count; i++)
            {
                comboBoxDisplayImg.Items.Add(i.ToString());
            }
            if (comboBoxDisplayImg.Items.Count > 0)
                comboBoxDisplayImg.SelectedIndex = 0;
            if (comboBoxBand.Items.Count > 0)
            {
                comboBoxBand.SelectedIndexChanged -= comboBoxBand_SelectedIndexChanged;
                comboBoxBand.SelectedIndex = 0;
                comboBoxBand.SelectedIndexChanged += comboBoxBand_SelectedIndexChanged;
            }
        }

        public void ClearDisplay(HSmartWindowControl Display,HObject Image)
        {
            HOperatorSet.ClearWindow(Display.HalconWindow);
            HOperatorSet.DispObj(Image, Display.HalconWindow);
        }

        public void UpdateRegionList()
        {
            #region Method Region

            comboBoxSelectRegion1.Items.Clear();
            comboBoxSelectRegion2.Items.Clear();
            foreach (clsRecipe.clsMethodRegion Region in MethodRegion)
            {
                comboBoxSelectRegion1.Items.Add("[對位]" + Region.Name);
                comboBoxSelectRegion2.Items.Add("[對位]" + Region.Name);
            }

            #endregion

            #region Edit Region

            foreach (clsRecipe.clsMethodRegion Region in EditRegion)
            {
                comboBoxSelectRegion1.Items.Add("[編輯]" + Region.Name);
                comboBoxSelectRegion2.Items.Add("[編輯]" + Region.Name);
            }

            #endregion
        }

        public bool Execute(HObject SelectRegion1, HObject SelectRegion2, enuTools SelectTool, object Method, out HObject OutputRegion)
        {
            bool Status = false;
            
            HOperatorSet.GenEmptyObj(out OutputRegion);
            try
            {
                switch(SelectTool)
                {
                    case enuTools.RegionSets:
                        {
                            #region Region操作

                            enuRegionSets Type = (enuRegionSets)Method;
                            switch(Type)
                            {
                                case enuRegionSets.Union1:
                                    {
                                        OutputRegion.Dispose();
                                        HOperatorSet.Union1(SelectRegion1, out OutputRegion);
                                    }
                                    break;
                                case enuRegionSets.Union2:
                                    {
                                        OutputRegion.Dispose();
                                        HOperatorSet.Union2(SelectRegion1, SelectRegion2, out OutputRegion);
                                    }
                                    break;
                                case enuRegionSets.Difference:
                                    {
                                        OutputRegion.Dispose();
                                        HOperatorSet.Difference(SelectRegion1, SelectRegion2, out OutputRegion);
                                    }
                                    break;
                                case enuRegionSets.Intersection:
                                    {
                                        OutputRegion.Dispose();
                                        HOperatorSet.Intersection(SelectRegion1, SelectRegion2, out OutputRegion);
                                    }
                                    break;
                                case enuRegionSets.Complement:
                                    {
                                        OutputRegion.Dispose();
                                        HOperatorSet.SetSystem("clip_region", "true"); // (20200729) Jeff Revised!
                                        HOperatorSet.Complement(SelectRegion1, out OutputRegion);
                                        HOperatorSet.SetSystem("clip_region", "false"); // (20200729) Jeff Revised!
                                    }
                                    break;
                                case enuRegionSets.Connection:
                                    {
                                        OutputRegion.Dispose();
                                        HOperatorSet.Connection(SelectRegion1, out OutputRegion);
                                    }
                                    break;
                                case enuRegionSets.fillup:
                                    {
                                        OutputRegion.Dispose();
                                        HOperatorSet.FillUp(SelectRegion1, out OutputRegion);
                                    }
                                    break;
                                case enuRegionSets.Copy:
                                    {
                                        OutputRegion.Dispose();
                                        OutputRegion = SelectRegion1.Clone();
                                    }
                                    break;
                                case enuRegionSets.Shapetrans:
                                    {
                                        if (comboBoxShapeTrans.SelectedIndex < 0)
                                        {
                                            MessageBox.Show("請先選擇轉換形式");
                                            break;
                                        }
                                        OutputRegion.Dispose();
                                        HTuple TransType = comboBoxShapeTrans.Items[comboBoxShapeTrans.SelectedIndex].ToString();
                                        HOperatorSet.ShapeTrans(SelectRegion1, out OutputRegion, TransType);
                                    }
                                    break;
                                case enuRegionSets.FillupShape:
                                    {
                                        if (comboBoxFillupType.SelectedIndex < 0)
                                        {
                                            MessageBox.Show("Please Select Type");
                                            break;
                                        }
                                        OutputRegion.Dispose();

                                        HTuple TransType = comboBoxFillupType.Items[comboBoxFillupType.SelectedIndex].ToString();
                                        HTuple Min = int.Parse(nudFillupMin.Value.ToString());
                                        HTuple Max = int.Parse(nudFillupMax.Value.ToString());
                                        HOperatorSet.FillUpShape(SelectRegion1, out OutputRegion, TransType, Min, Max);
                                    }
                                    break;
                                case enuRegionSets.Concat:
                                    {
                                        OutputRegion.Dispose();
                                        HOperatorSet.ConcatObj(SelectRegion1, SelectRegion2, out OutputRegion);
                                    }
                                    break;
                                default:
                                    return Status;
                            }

                            #endregion
                        }
                        break;
                    case enuTools.Morpgology:
                        {
                            #region 介面參數

                            HTuple Width, Height, Raduis;
                            Width = int.Parse(nudWidth.Value.ToString());
                            Height = int.Parse(nudHeight.Value.ToString());
                            Raduis = double.Parse(nudRaduis.Value.ToString());

                            #endregion

                            #region 形態學

                            enuMorpgology Type = (enuMorpgology)Method;

                            switch(Type)
                            {
                                case enuMorpgology.opening_rect1:
                                    {
                                        HOperatorSet.OpeningRectangle1(SelectRegion1, out OutputRegion, Width, Height);
                                    }
                                    break;
                                case enuMorpgology.closing_rect1:
                                    {
                                        HOperatorSet.ClosingRectangle1(SelectRegion1, out OutputRegion, Width, Height);
                                    }
                                    break;
                                case enuMorpgology.dilation_rect1:
                                    {
                                        HOperatorSet.DilationRectangle1(SelectRegion1, out OutputRegion, Width, Height);
                                    }
                                    break;
                                case enuMorpgology.erosion_rect1:
                                    {
                                        HOperatorSet.ErosionRectangle1(SelectRegion1, out OutputRegion, Width, Height);
                                    }
                                    break;
                                case enuMorpgology.opening_circle:
                                    {
                                        HOperatorSet.OpeningCircle(SelectRegion1, out OutputRegion, Raduis);
                                    }
                                    break;
                                case enuMorpgology.closing_circle:
                                    {
                                        HOperatorSet.ClosingCircle(SelectRegion1, out OutputRegion, Raduis);
                                    }
                                    break;
                                case enuMorpgology.dilation_circle:
                                    {
                                        HOperatorSet.DilationCircle(SelectRegion1, out OutputRegion, Raduis);
                                    }
                                    break;
                                case enuMorpgology.erosion_circle:
                                    {
                                        HOperatorSet.ErosionCircle(SelectRegion1, out OutputRegion, Raduis);
                                    }
                                    break;
                                default:
                                    {
                                        return Status;
                                    }
                            }

                            #endregion
                        }
                        break;
                    case enuTools.Select:
                        {
                            #region 介面參數

                            HTuple WMin, WMax, HMin, HMax, AMin, AMax;
                            WMin = int.Parse(nudSelectWMin.Value.ToString());
                            WMax = int.Parse(nudSelectWMax.Value.ToString());
                            HMin = int.Parse(nudSelectHMin.Value.ToString());
                            HMax = int.Parse(nudSelectHMax.Value.ToString());
                            AMin = int.Parse(nudSlelceAMin.Value.ToString());
                            AMax = int.Parse(nudSelectAMax.Value.ToString());

                            HTuple GrayMin, GrayMax;
                            GrayMin = int.Parse(nudSelectGrayMIn.Value.ToString());
                            GrayMax = int.Parse(nudSelectGrayMax.Value.ToString());
                            

                            #endregion

                            #region 條件卡控

                            enuSelect Type = (enuSelect)Method;
                            switch (Type)
                            {
                                case enuSelect.Width:
                                    {
                                        HOperatorSet.SelectShape(SelectRegion1, out OutputRegion, (new HTuple("width")), "and", WMin, WMax);
                                    }
                                    break;
                                case enuSelect.Height:
                                    {
                                        HOperatorSet.SelectShape(SelectRegion1, out OutputRegion, (new HTuple("height")), "and", HMin, HMax);
                                    }
                                    break;
                                case enuSelect.Area:
                                    {
                                        HOperatorSet.SelectShape(SelectRegion1, out OutputRegion, (new HTuple("area")), "and", AMin, AMax);
                                    }
                                    break;
                                case enuSelect.GrayMean:
                                    {
                                        if (comboBoxDisplayImg.SelectedIndex < 0)
                                        {
                                            MessageBox.Show("請先選擇影像");
                                            break;
                                        }
                                        if (comboBoxBand.SelectedIndex < 0)
                                        {
                                            MessageBox.Show("請先選擇Band");
                                            break;
                                        }
                                        HObject Image = clsStaticTool.GetChannelImage(InputImageList[comboBoxDisplayImg.SelectedIndex], (enuBand)comboBoxBand.SelectedIndex);
                                        HOperatorSet.SelectGray(SelectRegion1, Image, out OutputRegion, (new HTuple("mean")), "and", GrayMin, GrayMax);
                                        Image.Dispose();
                                    }
                                    break;
                                case enuSelect.GrayMin:
                                    {
                                        if (comboBoxDisplayImg.SelectedIndex < 0)
                                        {
                                            MessageBox.Show("請先選擇影像");
                                            break;
                                        }
                                        if (comboBoxBand.SelectedIndex < 0)
                                        {
                                            MessageBox.Show("請先選擇Band");
                                            break;
                                        }
                                        HObject Image = clsStaticTool.GetChannelImage(InputImageList[comboBoxDisplayImg.SelectedIndex], (enuBand)comboBoxBand.SelectedIndex);
                                        HOperatorSet.SelectGray(SelectRegion1, Image, out OutputRegion, (new HTuple("min")), "and", GrayMin, GrayMax);
                                        Image.Dispose();
                                    }
                                    break;
                                case enuSelect.GrayMax:
                                    {
                                        if (comboBoxDisplayImg.SelectedIndex < 0)
                                        {
                                            MessageBox.Show("請先選擇影像");
                                            break;
                                        }
                                        if (comboBoxBand.SelectedIndex < 0)
                                        {
                                            MessageBox.Show("請先選擇Band");
                                            break;
                                        }
                                        HObject Image = clsStaticTool.GetChannelImage(InputImageList[comboBoxDisplayImg.SelectedIndex], (enuBand)comboBoxBand.SelectedIndex);
                                        HOperatorSet.SelectGray(SelectRegion1, Image, out OutputRegion, (new HTuple("max")), "and", GrayMin, GrayMax);
                                        Image.Dispose();
                                    }
                                    break;
                                case enuSelect.GrayDeviation:
                                    {
                                        if (comboBoxDisplayImg.SelectedIndex < 0)
                                        {
                                            MessageBox.Show("請先選擇影像");
                                            break;
                                        }
                                        if (comboBoxBand.SelectedIndex < 0)
                                        {
                                            MessageBox.Show("請先選擇Band");
                                            break;
                                        }
                                        HObject Image = clsStaticTool.GetChannelImage(InputImageList[comboBoxDisplayImg.SelectedIndex], (enuBand)comboBoxBand.SelectedIndex);
                                        HOperatorSet.SelectGray(SelectRegion1, Image, out OutputRegion, (new HTuple("deviation")), "and", GrayMin, GrayMax);
                                        Image.Dispose();
                                    }
                                    break;
                                default:
                                    return Status;
                            }

                            #endregion
                        }
                        break;
                    default:
                        {
                            return Status;
                        }

                }
            }
            catch (Exception Ex)
            {
                return Status;
            }

            Status = true;
            return Status;
        }

        public HObject GetSelectRegion(int SelectIndex)
        {
            HObject Res;
            HOperatorSet.GenEmptyObj(out Res);

            if (SelectIndex < 0)
            {
                return Res;
            }

            if (SelectIndex < MethodRegion.Count)
            {
                Res = MethodRegion[SelectIndex].Region;
            }
            else
            {
                Res = EditRegion[SelectIndex - MethodRegion.Count].Region;
            }
            return Res;
        }

        private static void EnableROIBtn(Control ParentCtrl, Control TriggerCtrl, HSmartWindowControl Display, Control PassControl, bool Enabled)
        {
            foreach (Control Ctrl in ParentCtrl.Controls)
            {
                if ((Ctrl == TriggerCtrl || Ctrl == Display || Ctrl == PassControl))
                    continue;

                //child control
                EnableROIBtn(Ctrl, TriggerCtrl, Display, PassControl, Enabled);

                if ((Ctrl is Panel) || (Ctrl is GroupBox))
                    continue;

                Ctrl.Enabled = Enabled;
            }
        }

        private void ChangeColor(Color C, CheckBox cbx)
        {
            cbx.BackColor = C;
        }

        #endregion

        #region ==========================   Event    ==========================

        private void comboBoxTool_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Obj = (ComboBox)sender;
            comboBoxArithmetic.Items.Clear();
            switch (Obj.SelectedIndex)
            {
                case ((int)enuTools.RegionSets):
                    {
                        string[] ArrayMethod = Enum.GetNames(typeof(enuRegionSets));
                        foreach (string S in ArrayMethod)
                        {
                            comboBoxArithmetic.Items.Add(S);
                        }
                    }
                    break;
                case ((int)enuTools.Morpgology):
                    {
                        string[] ArrayMethod = Enum.GetNames(typeof(enuMorpgology));
                        foreach (string S in ArrayMethod)
                        {
                            comboBoxArithmetic.Items.Add(S);
                        }
                    }
                    break;
                case ((int)enuTools.Select):
                    {
                        string[] ArrayMethod = Enum.GetNames(typeof(enuSelect));
                        foreach (string S in ArrayMethod)
                        {
                            comboBoxArithmetic.Items.Add(S);
                        }
                    }
                    break;
            }
        }

        private void FormEditRegion_Resize(object sender, EventArgs e)
        {
            if (igFormWidth == 0 || igFormHeight == 0) return;
            fgWidthScaling = (float)this.Width / (float)igFormWidth;
            fgHeightScaling = (float)this.Height / (float)igFormHeight;
            ResizeCon(fgWidthScaling, fgHeightScaling, this);
        }

        private void FormEditRegion_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void FormEditRegion_Load(object sender, EventArgs e)
        {
            comboBoxDisplayType.SelectedIndex = 0;

        }

        private void comboBoxDisplayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Obj = (ComboBox)sender;
            if (Obj.SelectedIndex == 0)
                DisplayType = enuDisplayRegionType.fill;
            else
                DisplayType = enuDisplayRegionType.margin;
        }

        private void comboBoxDisplayImg_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (InputImageList.Count > comboBoxDisplayImg.SelectedIndex)
            {
                ClearDisplay(DisplayWindows, InputImageList[comboBoxDisplayImg.SelectedIndex]);
            }
        }

        private void comboBoxBand_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (InputImageList.Count > 0 && InputImageList.Count > comboBoxDisplayImg.SelectedIndex)
            {
                HTuple Ch;
                HOperatorSet.CountChannels(InputImageList[comboBoxDisplayImg.SelectedIndex], out Ch);
                if (Ch == 1)
                {
                    return;
                }
                HObject SrcImg_R, SrcImg_G, SrcImg_B;

                if (comboBoxBand.SelectedIndex == 0)
                {
                    HOperatorSet.AccessChannel(InputImageList[comboBoxDisplayImg.SelectedIndex], out SrcImg_R, 1);
                    HOperatorSet.DispObj(SrcImg_R, DisplayWindows.HalconWindow);
                    SrcImg_R.Dispose();
                }
                else if (comboBoxBand.SelectedIndex == 1)
                {
                    HOperatorSet.AccessChannel(InputImageList[comboBoxDisplayImg.SelectedIndex], out SrcImg_G, 2);
                    HOperatorSet.DispObj(SrcImg_G, DisplayWindows.HalconWindow);
                    SrcImg_G.Dispose();
                }
                else if (comboBoxBand.SelectedIndex == 2)
                {
                    HOperatorSet.AccessChannel(InputImageList[comboBoxDisplayImg.SelectedIndex], out SrcImg_B, 3);
                    HOperatorSet.DispObj(SrcImg_B, DisplayWindows.HalconWindow);
                    SrcImg_B.Dispose();
                }
                else
                {
                    HObject GrayImg;
                    HOperatorSet.Rgb1ToGray(InputImageList[comboBoxDisplayImg.SelectedIndex], out GrayImg);
                    HOperatorSet.DispObj(GrayImg, DisplayWindows.HalconWindow);
                    GrayImg.Dispose();
                }
            }
        }

        private void comboBoxSelectRegion1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(InputImageList.Count<=0)
            {
                return;
            } 
            ComboBox Obj = (ComboBox)sender;
            ClearDisplay(DisplayWindows, InputImageList[comboBoxDisplayImg.SelectedIndex]);
            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, DisplayType.ToString());
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "yellow");
            if (SelectRegion!=null)
            {
                SelectRegion.Dispose();
                SelectRegion = null;
            }
            if (Obj.SelectedIndex < MethodRegion.Count)
            {
                HOperatorSet.DispObj(MethodRegion[Obj.SelectedIndex].Region, DisplayWindows.HalconWindow);
                SelectRegion = MethodRegion[Obj.SelectedIndex].Region.Clone();
            }
            else
            {
                HOperatorSet.DispObj(EditRegion[Obj.SelectedIndex - MethodRegion.Count].Region, DisplayWindows.HalconWindow);
                SelectRegion = EditRegion[Obj.SelectedIndex - MethodRegion.Count].Region.Clone();
            }

        }

        private void comboBoxSelectRegion2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (InputImageList.Count <= 0)
            {
                return;
            }
            if (SelectRegion != null)
            {
                SelectRegion.Dispose();
                SelectRegion = null;
            }
            ComboBox Obj = (ComboBox)sender;
            ClearDisplay(DisplayWindows, InputImageList[comboBoxDisplayImg.SelectedIndex]);
            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, DisplayType.ToString());
            HOperatorSet.SetColor(DisplayWindows.HalconWindow, "green");
            if (Obj.SelectedIndex < MethodRegion.Count)
            {
                HOperatorSet.DispObj(MethodRegion[Obj.SelectedIndex].Region, DisplayWindows.HalconWindow);
                SelectRegion = MethodRegion[Obj.SelectedIndex].Region.Clone();
            }
            else
            {
                HOperatorSet.DispObj(EditRegion[Obj.SelectedIndex - MethodRegion.Count].Region, DisplayWindows.HalconWindow);
                SelectRegion = EditRegion[Obj.SelectedIndex - MethodRegion.Count].Region.Clone();
            }
        }

        private void btnRegionCalcTest_Click(object sender, EventArgs e)
        {
            HObject ResRegion;
            if (comboBoxTool.SelectedIndex < 0 || comboBoxArithmetic.SelectedIndex < 0)
            {
                return;
            }
            ClearDisplay(DisplayWindows, InputImageList[comboBoxDisplayImg.SelectedIndex]);
            Execute(GetSelectRegion(comboBoxSelectRegion1.SelectedIndex), GetSelectRegion(comboBoxSelectRegion2.SelectedIndex), (enuTools)comboBoxTool.SelectedIndex, comboBoxArithmetic.SelectedIndex, out ResRegion);

            SelectRegion = ResRegion.Clone();

            HOperatorSet.SetDraw(DisplayWindows.HalconWindow, DisplayType.ToString());
            HOperatorSet.SetColored(DisplayWindows.HalconWindow, 12);
            HOperatorSet.DispObj(ResRegion, DisplayWindows.HalconWindow);
        }

        private void bthAddRegion_Click(object sender, EventArgs e)
        {
            string RegionName = txbRegionName.Text;
            if (string.IsNullOrEmpty(RegionName))
            {
                MessageBox.Show("請先輸入Region名稱");
                return;
            }

            bool IsMemberExists = Recipe.Param.EditRegionList.Exists(x => x.Name == RegionName);

            if (IsMemberExists)
            {
                MessageBox.Show("存在相同名稱,請重新命名");
                return;
            }
            DialogResult dialogResult = MessageBox.Show("確認是否新增?", "提示!", MessageBoxButtons.YesNo);

            if (dialogResult != DialogResult.Yes)
            {
                return;
            }
            try
            {
                HObject ResRegion;
                ClearDisplay(DisplayWindows, InputImageList[comboBoxDisplayImg.SelectedIndex]);
                Execute(GetSelectRegion(comboBoxSelectRegion1.SelectedIndex), GetSelectRegion(comboBoxSelectRegion2.SelectedIndex), (enuTools)comboBoxTool.SelectedIndex, comboBoxArithmetic.SelectedIndex, out ResRegion);

                HTuple RegionCount;
                HOperatorSet.CountObj(ResRegion, out RegionCount);


                HOperatorSet.SortRegion(ResRegion, out ResRegion, "character", "true", "row");
                HTuple Area, row, column;
                HOperatorSet.AreaCenter(ResRegion, out Area, out row, out column);
                clsRecipe.clsMethodRegion NewRegion = new clsRecipe.clsMethodRegion();
                NewRegion.HotspotY = Recipe.Param.SegParam.GoldenCenterY - row;
                NewRegion.HotspotX = Recipe.Param.SegParam.GoldenCenterX - column;
                NewRegion.Row = row;
                NewRegion.Column = column;
                NewRegion.Name = RegionName;
                NewRegion.RegionPath = Recipe.EditRegionPath + RegionName;
                NewRegion.Region = ResRegion;
                EditRegion.Add(NewRegion);
                UpdateRegionList();
                txbRegionName.Text = "";
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void DisplayWindows_HMouseMove(object sender, HMouseEventArgs e)
        {
            try
            {
                this.BeginInvoke(new Action(() =>
                {
                    if (InputImageList.Count > 0)
                    {
                        HObject nowImage = InputImageList[comboBoxDisplayImg.SelectedIndex];
                        if (nowImage == null) return;
                        HTuple width = null, height = null;
                        HOperatorSet.GetImageSize(nowImage, out width, out height);
                        txt_CursorCoordinate.Text = "(" + String.Format("{0:#}", e.X) + ", " + String.Format("{0:#}", e.Y) + ")";
                        try
                        {
                            if (e.X >= 0 && e.X < width && e.Y >= 0 && e.Y < height)
                            {
                                HTuple grayval;
                                HOperatorSet.GetGrayval(nowImage, e.Y, e.X, out grayval);
                                txt_ColorValue.Text = (grayval.TupleInt()).ToString();
                                HObject GrayImg;
                                HOperatorSet.Rgb1ToGray(nowImage, out GrayImg);
                                HOperatorSet.GetGrayval(GrayImg, e.Y, e.X, out grayval);
                                txbGrayValue.Text = grayval.TupleInt().ToString();
                                GrayImg.Dispose();
                            }
                            else
                            {
                                txt_ColorValue.Text = "";
                                txbGrayValue.Text = "";
                            }
                        }
                        catch
                        { }



                    }
                }));
            }
            catch
            { }
        }

        private void DisplayWindows_HMouseDown(object sender, HMouseEventArgs e)
        {
            if (cbxUserSetRegion.Checked && SelectRegion != null)
            {
                HTuple RegionCount;

                HOperatorSet.CountObj(SelectRegion, out RegionCount);
                if (RegionCount > 0)
                {
                    try
                    {
                        HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                        HOperatorSet.SetColor(DisplayWindows.HalconWindow, "yellow");
                        HOperatorSet.DispObj(SelectRegion, DisplayWindows.HalconWindow);

                        HOperatorSet.GetRegionIndex(SelectRegion, (int)e.Y, (int)e.X, out hv_Index);
                        if (hv_Index.Length > 0)
                        {
                            HTuple SIndex = SelectIndex.TupleFind(hv_Index);

                            if (SIndex.Length <= 0 || SIndex == -1)
                            {
                                HOperatorSet.TupleConcat(SelectIndex, hv_Index, out SelectIndex);
                            }
                            else
                            {
                                HOperatorSet.TupleRemove(SelectIndex, SIndex, out SelectIndex);
                            }


                        }
                    }
                    catch
                    {

                    }
                }
                HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                HOperatorSet.DispObj(SelectRegion[SelectIndex], DisplayWindows.HalconWindow);
                return;
            }
            HTuple Area, Width, Height;
            this.BeginInvoke(new Action(() =>
            {
                if (SelectRegion != null)
                {
                    HTuple RegionCount;

                    HOperatorSet.CountObj(SelectRegion, out RegionCount);
                    if (RegionCount > 0)
                    {
                        try
                        {

                            HOperatorSet.GetRegionIndex(SelectRegion, (int)e.Y, (int)e.X, out hv_Index);
                            if (hv_Index.Length > 0)
                            {
                                HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                                HOperatorSet.SetColor(DisplayWindows.HalconWindow, "red");
                                HOperatorSet.DispObj(InputImageList[comboBoxDisplayImg.SelectedIndex], DisplayWindows.HalconWindow);
                                HOperatorSet.DispObj(SelectRegion, DisplayWindows.HalconWindow);

                                HOperatorSet.RegionFeatures(SelectRegion[hv_Index], "area", out Area);
                                HOperatorSet.RegionFeatures(SelectRegion[hv_Index], "width", out Width);
                                HOperatorSet.RegionFeatures(SelectRegion[hv_Index], "height", out Height);

                                labW.Text = Width.D.ToString();
                                labH.Text = Height.D.ToString();
                                labA.Text = Area.D.ToString();

                                HTuple Mean, Min, Max;
                                HObject Image = clsStaticTool.GetChannelImage(InputImageList[comboBoxDisplayImg.SelectedIndex], (enuBand)comboBoxBand.SelectedIndex);
                                HOperatorSet.GrayFeatures(SelectRegion[hv_Index], Image, "mean", out Mean);
                                HOperatorSet.GrayFeatures(SelectRegion[hv_Index], Image, "min", out Min);
                                HOperatorSet.GrayFeatures(SelectRegion[hv_Index], Image, "max", out Max);

                                labGrayMean.Text = Mean.D.ToString("#.#");
                                labGrayMin.Text = Min.D.ToString();
                                labGrayMax.Text = Max.D.ToString();

                                HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                                HOperatorSet.SetColor(DisplayWindows.HalconWindow, "blue");
                                HOperatorSet.DispObj(SelectRegion[hv_Index], DisplayWindows.HalconWindow);
                                Image.Dispose();
                            }
                        }
                        catch
                        {

                        }
                    }
                }

            }));

        }

        private void cbxUserSetRegion_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Obj = (CheckBox)sender;

            #region 確認是否可執行

            if (string.IsNullOrEmpty(txbRegionName.Text))
            {
                Obj.CheckedChanged -= cbxUserSetRegion_CheckedChanged;
                Obj.Checked = false;
                Obj.CheckedChanged += cbxUserSetRegion_CheckedChanged;

                EnableROIBtn(this, Obj, DisplayWindows, btnConcat, true);
                ChangeColor(Color.Aqua, Obj);
                MessageBox.Show("請先輸入範圍名稱");
                return;
            }

            if (SelectRegion == null)
            {
                Obj.CheckedChanged -= cbxUserSetRegion_CheckedChanged;
                Obj.Checked = false;
                Obj.CheckedChanged += cbxUserSetRegion_CheckedChanged;

                EnableROIBtn(this, Obj, DisplayWindows, btnConcat, true);
                ChangeColor(Color.Aqua, Obj);
                MessageBox.Show("請先選取要使用的範圍");
                return;
            }

            bool IsMemberExists = Recipe.Param.EditRegionList.Exists(x => x.Name == txbRegionName.Text);

            if (IsMemberExists)
            {
                Obj.CheckedChanged -= cbxUserSetRegion_CheckedChanged;
                Obj.Checked = false;
                Obj.CheckedChanged += cbxUserSetRegion_CheckedChanged;

                EnableROIBtn(this, Obj, DisplayWindows, btnConcat, true);
                ChangeColor(Color.Aqua, Obj);
                MessageBox.Show("存在相同名稱,請重新命名");
                txbRegionName.Text = "";
                return;
            }

            #endregion

            if (Obj.Checked)
            {
                if (UserSetRegion != null)
                {
                    UserSetRegion.Dispose();
                    UserSetRegion = null;
                    HOperatorSet.GenEmptyObj(out UserSetRegion);
                }

                if (ConcatRegion == null)
                {
                    HOperatorSet.GenEmptyObj(out ConcatRegion);
                }
                else
                {
                    ConcatRegion.Dispose();
                    ConcatRegion = null;
                    HOperatorSet.GenEmptyObj(out ConcatRegion);
                }

                SelectIndex = new HTuple();

                #region 設定範圍
                try
                {
                    EnableROIBtn(this, Obj, DisplayWindows, btnConcat, false);
                    ChangeColor(Color.Green, Obj);

                    HOperatorSet.SetDraw(DisplayWindows.HalconWindow, "fill");
                    HOperatorSet.SetColor(DisplayWindows.HalconWindow, "yellow");
                    HOperatorSet.DispObj(SelectRegion, DisplayWindows.HalconWindow);
                }
                catch (Exception ex)
                {
                    Obj.CheckedChanged -= cbxUserSetRegion_CheckedChanged;
                    Obj.Checked = false;
                    Obj.CheckedChanged += cbxUserSetRegion_CheckedChanged;

                    EnableROIBtn(this, Obj, DisplayWindows, btnConcat, true);
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
                    HOperatorSet.SelectObj(SelectRegion, out UserSetRegion, SelectIndex);

                    HOperatorSet.ConcatObj(UserSetRegion, ConcatRegion, out UserSetRegion);

                    HOperatorSet.SortRegion(UserSetRegion, out UserSetRegion, "character", "true", "row");
                    HTuple Area, row, column;
                    HOperatorSet.AreaCenter(UserSetRegion, out Area, out row, out column);
                    clsRecipe.clsMethodRegion NewRegion = new clsRecipe.clsMethodRegion();
                    NewRegion.HotspotY = Recipe.Param.SegParam.GoldenCenterY - row;
                    NewRegion.HotspotX = Recipe.Param.SegParam.GoldenCenterX - column;
                    NewRegion.Row = row;
                    NewRegion.Column = column;
                    NewRegion.Name = txbRegionName.Text;
                    NewRegion.RegionPath = Recipe.EditRegionPath + txbRegionName.Text;
                    NewRegion.Region = UserSetRegion.Clone();
                    EditRegion.Add(NewRegion);
                    UpdateRegionList();

                    MessageBox.Show("新增完成");

                    ChangeColor(Color.Aqua, Obj);
                    EnableROIBtn(this, Obj, DisplayWindows, btnConcat, true);

                    txbRegionName.Text = "";
                }
                catch (Exception ex)
                {
                    Obj.CheckedChanged -= cbxUserSetRegion_CheckedChanged;
                    Obj.Checked = false;
                    Obj.CheckedChanged += cbxUserSetRegion_CheckedChanged;

                    EnableROIBtn(this, Obj, DisplayWindows, btnConcat, true);
                    ChangeColor(Color.Aqua, Obj);
                    MessageBox.Show(ex.ToString());
                }
                #endregion
            }
        }

        private void btnConcat_Click(object sender, EventArgs e)
        {
            if (ConcatRegion == null)
            {
                HOperatorSet.GenEmptyObj(out ConcatRegion);
            }
            HOperatorSet.SelectObj(SelectRegion, out UserSetRegion, SelectIndex);

            HOperatorSet.SortRegion(UserSetRegion, out UserSetRegion, "character", "true", "row");

            HOperatorSet.Union1(UserSetRegion, out UserSetRegion);

            HOperatorSet.ConcatObj(ConcatRegion, UserSetRegion, out ConcatRegion);

            SelectIndex = null;
            SelectIndex = new HTuple();
        }

        #endregion
        
    }
}
