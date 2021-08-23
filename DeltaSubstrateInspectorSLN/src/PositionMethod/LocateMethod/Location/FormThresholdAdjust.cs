using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using HalconDotNet;

namespace DeltaSubstrateInspector.src.PositionMethod.LocateMethod.Location
{ 
    public partial class FormThresholdAdjust : Form
    {
        public enum enType
        {
            Bright,
            Dark,
            Dual
        }
        #region //===================== 區域變數設置 =====================
        private HObject SrcImg;
        HObject Reg;
        #endregion

        public FormThresholdAdjust(HObject pmSrcImg, int ThMin,int ThMax, enType UseType)
        {
            InitializeComponent();
            //calc histogram
            this.DisplayWindow.MouseWheel += DisplayWindow.HSmartWindowControl_MouseWheel;
            HOperatorSet.CopyObj(pmSrcImg, out SrcImg, 1, 1);

            HOperatorSet.DispObj(SrcImg, DisplayWindow.HalconWindow); // (20190619) Jeff Revised!

            HOperatorSet.Threshold(SrcImg,out Reg, ThMin, ThMax);
            HOperatorSet.DispObj(Reg, DisplayWindow.HalconWindow);
            HOperatorSet.SetPart(DisplayWindow.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part (20190619) Jeff Revised!

            HTuple width, height;
            HObject R;
            HOperatorSet.GetImageSize(SrcImg, out width, out height);
            HOperatorSet.GenRectangle1(out R, 0, 0, height, width);

            HTuple AH, RH;
            HOperatorSet.GrayHisto(R, SrcImg, out AH, out RH);

            NumUpDownThresholdMax.Value = ThMax;
            NumUpDownThresholdMin.Value = ThMin;

            DataPointCollection PointsHist = chtHist.Series[0].Points;
            PointsHist.Clear();
            for (int i = 0; i < AH.Length; i++)
            {
                PointsHist.AddY(AH[i]);
            }

            SelectType(UseType);
        }

        public void SelectType(enType UseType)
        {
            switch (UseType)
            {
                case enType.Bright:
                    {
                        NumUpDownThresholdMax.Value = 255;
                        NumUpDownThresholdMax.Visible = false;
                        labMaxThreshold.Visible = false;
                    }
                    break;
                case enType.Dark:
                    {
                        NumUpDownThresholdMin.Value = 0;
                        NumUpDownThresholdMin.Visible = false;
                        labMinThreshold.Visible = false;
                    }
                    break;
                case enType.Dual:
                    {

                    }
                    break;
            }
        }

        public void GetThreshold(out int ThMin,out int ThMax)
        {
            ThMin = (int)NumUpDownThresholdMin.Value;
            ThMax = (int)NumUpDownThresholdMax.Value;
        }

        private void tbrThreshold_ValueChanged(object sender, EventArgs e)
        {
            TrackBar Obj = (TrackBar)sender;
            NumUpDownThresholdMin.Value = Obj.Value;
        }

        private void NumUpDownThreshold_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown NumUpDown = (NumericUpDown)sender;
            int ThresholdValueMin = (int)NumUpDownThresholdMin.Value;
            int ThresholdValueMax = (int)NumUpDownThresholdMax.Value;
            if (ThresholdValueMin >= ThresholdValueMax)
            {
                NumUpDown.Value = ThresholdValueMax - 1;
                ThresholdValueMin = (int)NumUpDown.Value;
            }

            HOperatorSet.ClearWindow(DisplayWindow.HalconWindow);

            HOperatorSet.Threshold(SrcImg, out Reg, ThresholdValueMin, ThresholdValueMax);
            HOperatorSet.DispObj(Reg, DisplayWindow.HalconWindow);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void FormThresholdAdjust_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (SrcImg != null)
            {
                SrcImg.Dispose();
                SrcImg = null;
            }
            if (Reg != null)
            {
                Reg.Dispose();
                Reg = null;
            }
        }

        private void NumUpDownThresholdMax_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown NumUpDown = (NumericUpDown)sender;
            int ThresholdValueMin = (int)NumUpDownThresholdMin.Value;
            int ThresholdValueMax = (int)NumUpDownThresholdMax.Value;

            if (ThresholdValueMax <= ThresholdValueMin)
            {
                NumUpDown.Value = ThresholdValueMin + 1;
                ThresholdValueMax = (int)NumUpDown.Value;
            }

            HOperatorSet.ClearWindow(DisplayWindow.HalconWindow);

            HOperatorSet.Threshold(SrcImg, out Reg, ThresholdValueMin, ThresholdValueMax);
            HOperatorSet.DispObj(Reg, DisplayWindow.HalconWindow);
        }
    }
}
