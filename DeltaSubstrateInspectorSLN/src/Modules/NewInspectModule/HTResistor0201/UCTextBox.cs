using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using HalconDotNet;

namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.HTResistor0201
{
    public partial class HTResistor0201UC : UserControl
    {
        private void UC_LocalThresholding(HTuple lightDark, int mask_size, float scale)
        {
            HObject ho_Region = HTResistor0201_method.F_LocalThresholding(ho_image, lightDark, mask_size, scale);

            HOperatorSet.ClearWindow(hSmartWindowControl1.HalconWindow);
            HOperatorSet.SetDraw(hSmartWindowControl1.HalconWindow, "fill");
            HOperatorSet.DispObj(ho_Region, hSmartWindowControl1.HalconWindow);
        }

        // ===========================================================================================================
        private void txtboxThresholdScale_BlackCrackFL_Validated(object sender, EventArgs e)
        {
            float numValue;
            if (!float.TryParse(this.txtboxThresholdScale_BlackCrackFL.Text, out numValue)) return;

            UC_LocalThresholding("light", 64, numValue);
        }

        // ==================================================================================
        private void txtboxThresholdScale_WhiteCrackFL_Validated(object sender, EventArgs e)
        {
            float numValue;
            if (!float.TryParse(this.txtboxThresholdScale_WhiteCrackFL.Text, out numValue)) return;

            UC_LocalThresholding("dark", 64, numValue);
        }

        // ==================================================================================
        private void txtboxThresholdScale_WhiteBlobFL_Validated(object sender, EventArgs e)
        {
            float numValue;
            if (!float.TryParse(this.txtboxThresholdScale_WhiteBlobFL.Text, out numValue)) return;

            UC_LocalThresholding("dark", 32, numValue);
        }

        // ===========================================================================================================
        private void txtboxThresholdScale_BlackBlobCL_Validated(object sender, EventArgs e)
        {
            float numValue;
            if (!float.TryParse(this.txtboxThresholdScale_BlackBlobCL.Text, out numValue)) return;

            int intValue;
            if (!int.TryParse(this.txtboxMaskSize_BlackBlobCL.Text, out intValue)) return;

            UC_LocalThresholding("dark", intValue, numValue);
        }

        // ==================================================================================
        private void txtboxMaskSize_BlackBlobCL_Validated(object sender, EventArgs e)
        {
            float numValue;
            if (!float.TryParse(this.txtboxThresholdScale_BlackBlobCL.Text, out numValue)) return;

            int intValue;
            if (!int.TryParse(this.txtboxMaskSize_BlackBlobCL.Text, out intValue)) return;

            UC_LocalThresholding("dark", intValue, numValue);
        }

        // ===========================================================================================================
        private void txtboxThresholdOffset_BrightBlobBL_Validated(object sender, EventArgs e)
        {
            int th_offset;
            if (!int.TryParse(this.txtboxThresholdOffset_BrightBlobBL.Text, out th_offset)) return;

            HObject ho_Region = HTResistor0201_method.F_DynamicThresholding(ho_image, th_offset);
            if (ho_Region != null)
            {
                HOperatorSet.ClearWindow(hSmartWindowControl1.HalconWindow);
                HOperatorSet.SetDraw(hSmartWindowControl1.HalconWindow, "fill");
                HOperatorSet.DispObj(ho_Region, hSmartWindowControl1.HalconWindow);
            }
        }

        // ===========================================================================================================
        private void UC_CrackSegmentation(HTuple lightDark, int mask_size, float scale, int numValue, bool vert_close)
        {
            HObject ho_Region = HTResistor0201_method.F_LocalThresholding(ho_image, lightDark, mask_size, scale);
            if (ho_Region == null) return;

            HObject ho_RegionDifference = HTResistor0201_method.F_CrackSegmentation(ho_Region, numValue, vert_close);
            if (ho_RegionDifference == null) return;

            HOperatorSet.ClearWindow(hSmartWindowControl1.HalconWindow);
            HOperatorSet.SetDraw(hSmartWindowControl1.HalconWindow, "fill");
            HOperatorSet.DispObj(ho_RegionDifference, hSmartWindowControl1.HalconWindow);
        }

        // ===========================================================================================================
        private void txtboxCrackHeight_BlackCrackFL_Validated(object sender, EventArgs e)
        {
            int intValue;
            if (!int.TryParse(this.txtboxCrackHeight_BlackCrackFL.Text, out intValue)) return;

            bool vert_close = this.chkboxVertClose_BlackCrackFL.Checked;

            float numValue;
            if (!float.TryParse(this.txtboxThresholdScale_BlackCrackFL.Text, out numValue)) return;

            UC_CrackSegmentation("light", 64, numValue, intValue, vert_close);
        }

        // ==================================================================================
        private void txtboxCrackWidth_WhiteCrackFL_Validated(object sender, EventArgs e)
        {
            int intValue;
            if (!int.TryParse(this.txtboxCrackWidth_WhiteCrackFL.Text, out intValue)) return;

            bool vert_close = this.chkboxVertClose_BlackCrackFL.Checked;

            float numValue;
            if (!float.TryParse(this.txtboxThresholdScale_WhiteCrackFL.Text, out numValue)) return;

            UC_CrackSegmentation("dark", 64, numValue, intValue, vert_close);
        }

        // ==================================================================================
        private void txtboxBlobHeight_WhiteBlobFL_Validated(object sender, EventArgs e)
        {
            int intValue;
            if (!int.TryParse(this.txtboxBlobHeight_WhiteBlobFL.Text, out intValue)) return;

            bool vert_close = this.chkboxVertClose_BlackCrackFL.Checked;

            float numValue;
            if (!float.TryParse(this.txtboxThresholdScale_WhiteBlobFL.Text, out numValue)) return;
 
            UC_CrackSegmentation("dark", 32, numValue, intValue, vert_close);
        }
    }
}
