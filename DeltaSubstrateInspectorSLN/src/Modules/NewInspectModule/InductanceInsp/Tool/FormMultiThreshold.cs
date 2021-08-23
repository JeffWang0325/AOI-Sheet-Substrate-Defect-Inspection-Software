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
using System.Runtime.Serialization.Formatters.Binary;

namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.InductanceInsp
{
    public partial class FormMultiThreshold : Form
    {
        public HObject SrcImg = null;
        public clsRecipe.clsAOIParam Recipe;
        List<HObject> ImgList;
        InductanceInspRole Param;

        public void UpdateList()
        {
            cbxListMultiTh.Items.Clear();
            for (int i = 0; i < Recipe.MultiTHList.Count; i++)
            {
                cbxListMultiTh.Items.Add((i + 1).ToString());
            }
        }

        public void UpdateControl(int SelectIndex)
        {
            txbImgIndex.Text = Recipe.MultiTHList[SelectIndex].ImageID.ToString();
            nudLTh.Value = Recipe.MultiTHList[SelectIndex].LTH;
            nudHTH.Value = Recipe.MultiTHList[SelectIndex].HTH;
            comboBoxMultiTHBand.SelectedIndex = (int)Recipe.MultiTHList[SelectIndex].Band;
        }

        public clsRecipe.clsAOIParam GetList()
        {
            return Recipe;
        }

        public FormMultiThreshold(HObject pmSraImg, clsRecipe.clsAOIParam pmRecipe,List<HObject> pmImgList,InductanceInspRole pmParam)
        {
            InitializeComponent();

            if (pmSraImg != null)
                this.SrcImg = pmSraImg.Clone();
            this.Recipe = clsStaticTool.Clone<clsRecipe.clsAOIParam>(pmRecipe);
            this.ImgList = pmImgList;
            this.Param = pmParam;

            UpdateList();
        }

        private void FormMultiThreshold_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SrcImg != null)
            {
                SrcImg.Dispose();
                SrcImg = null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void txbImgIndex_TextChanged(object sender, EventArgs e)
        {
            if (cbxListMultiTh.SelectedIndices.Count <= 0)
            {
                gbxThParam.Enabled = false;
                return;
            }
            int ID = 0;
            if (int.TryParse(txbImgIndex.Text.ToString(), out ID))
            {
                Recipe.MultiTHList[cbxListMultiTh.SelectedIndices[0]].ImageID = ID;
            }
            else
            {
                MessageBox.Show("影像ID錯誤,請勿輸入非數字字元");
                txbImgIndex.Text = "0";
                Recipe.MultiTHList[cbxListMultiTh.SelectedIndices[0]].ImageID = 0;
            }
        }

        private void bthAdd_Click(object sender, EventArgs e)
        {
            clsRecipe.clsMultiTh TH = new clsRecipe.clsMultiTh();
            Recipe.MultiTHList.Add(TH);

            UpdateList();
            if (cbxListMultiTh.SelectedIndices.Count <= 0)
            {
                gbxThParam.Enabled = false;
                return;
            }

        }

        private void bthRemove_Click(object sender, EventArgs e)
        {
            if (cbxListMultiTh.SelectedIndices.Count <= 0)
            {
                gbxThParam.Enabled = false;
                return;
            }

            Recipe.MultiTHList.RemoveAt(cbxListMultiTh.SelectedIndices[0]);

            UpdateList();

            if (cbxListMultiTh.SelectedIndices.Count <= 0)
            {
                gbxThParam.Enabled = false;
                return;
            }
        }

        private void FormMultiThreshold_Load(object sender, EventArgs e)
        {
            if (cbxListMultiTh.SelectedIndices.Count <= 0)
            {
                gbxThParam.Enabled = false;
            }
        }
        
        private void nudLTh_ValueChanged(object sender, EventArgs e)
        {
            if (cbxListMultiTh.SelectedIndices.Count <= 0)
            {
                gbxThParam.Enabled = false;
                return;
            }
            NumericUpDown Obj = (NumericUpDown)sender;

            Recipe.MultiTHList[cbxListMultiTh.SelectedIndices[0]].LTH = int.Parse(nudLTh.Value.ToString());
        }

        private void nudHTH_ValueChanged(object sender, EventArgs e)
        {
            if (cbxListMultiTh.SelectedIndices.Count <= 0)
            {
                gbxThParam.Enabled = false;
                return;
            }
            NumericUpDown Obj = (NumericUpDown)sender;

            Recipe.MultiTHList[cbxListMultiTh.SelectedIndices[0]].HTH = int.Parse(nudHTH.Value.ToString());
        }

        private void cbxListMultiTh_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            ListView Obj = (ListView)sender;

            if (Obj.SelectedIndices.Count <= 0)
            {
                gbxThParam.Enabled = false;
                return;
            }

            gbxThParam.Enabled = true;
            UpdateControl(cbxListMultiTh.SelectedIndices[0]);
        }

        private void btnThSetup_Click(object sender, EventArgs e)
        {
            if (SrcImg == null)
            {
                MessageBox.Show("請先讀取影像");
                return;
            }

            HObject SrcImg_R, SrcImg_G, SrcImg_B;

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out SrcImg_R);
            HOperatorSet.GenEmptyObj(out SrcImg_G);
            HOperatorSet.GenEmptyObj(out SrcImg_B);
            
            HObject Input;
            if (ImgList.Count<= Recipe.MultiTHList[cbxListMultiTh.SelectedIndices[0]].ImageID)
            {
                MessageBox.Show("請先載入第" + Recipe.MultiTHList[cbxListMultiTh.SelectedIndices[0]].ImageID + "張影像");
                SrcImg_R.Dispose();
                SrcImg_G.Dispose();
                SrcImg_B.Dispose();
                return;
            }
            HOperatorSet.CopyImage(ImgList[Recipe.MultiTHList[cbxListMultiTh.SelectedIndices[0]].ImageID], out Input);

            HTuple Ch;
            HOperatorSet.CountChannels(Input, out Ch);
            HObject InspImage;

            if (Ch == 1)
                InspImage = Input.Clone();
            else
            {
                HOperatorSet.Decompose3(Input, out SrcImg_R, out SrcImg_G, out SrcImg_B);
                if (comboBoxMultiTHBand.SelectedIndex == 0)
                {
                    InspImage = SrcImg_R;
                }
                else if (comboBoxMultiTHBand.SelectedIndex == 1)
                {
                    InspImage = SrcImg_G;
                }
                else if (comboBoxMultiTHBand.SelectedIndex == 2)
                {
                    InspImage = SrcImg_B;
                }
                else
                {
                    HObject GrayImg;
                    HOperatorSet.Rgb1ToGray(SrcImg, out GrayImg);
                    InspImage = GrayImg.Clone();
                    GrayImg.Dispose();
                }
            }
            
            FormThresholdAdjust MyForm = new FormThresholdAdjust(InspImage,
                                                                 int.Parse(nudLTh.Value.ToString()),
                                                                 int.Parse(nudHTH.Value.ToString()), FormThresholdAdjust.enType.Dual);
            MyForm.ShowDialog();

            if (MyForm.DialogResult == DialogResult.OK)
            {
                int ThMin, ThMax;
                MyForm.GetThreshold(out ThMin, out ThMax);

                nudHTH.Value = ThMax;
                nudLTh.Value = ThMin;
            }
            InspImage.Dispose();
            Input.Dispose();
            SrcImg_R.Dispose();
            SrcImg_G.Dispose();
            SrcImg_B.Dispose();
        }

        private void comboBoxMultiTHBand_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Obj = (ComboBox)sender;
            if (Obj.SelectedIndex >= 0 && cbxListMultiTh.SelectedIndices[0] >= 0)
            {
                Recipe.MultiTHList[cbxListMultiTh.SelectedIndices[0]].Band = (enuBand)Obj.SelectedIndex;
            }
        }
    }
}
