using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using HalconDotNet;

namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.HTResistor0402
{
    public partial class HTResistor0402UC : UserControl
    {
        // ===========================================================================================================
        public void UC_DisplayResult(HObject ho_SelectedRegions)
        {
            HOperatorSet.DispObj(ho_image, hSmartWindowControl1.HalconWindow);

            // Local control variables 
            HTuple hv_Row = null, hv_Column = null;
            HTuple hv_Area = null, hv_Row1 = null, hv_Row2 = null, hv_Col1 = null, hv_Col2 = null;

            HOperatorSet.AreaCenter(ho_SelectedRegions, out hv_Area, out hv_Row, out hv_Column);
            HOperatorSet.SmallestRectangle1(ho_SelectedRegions, out hv_Row1, out hv_Col1, out hv_Row2, out hv_Col2);

            //if (hv_Area == null)
            if ((int)(new HTuple(hv_Area.TupleEqual(new HTuple()))) == 1)
            {
                HOperatorSet.DispText(hSmartWindowControl1.HalconWindow, "OK", "image", 20, 20, "black", new HTuple(), new HTuple());
                return;
            }

            HObject ho_Rectangle;
            HOperatorSet.GenEmptyObj(out ho_Rectangle);

            //if (((int)(new HTuple((new HTuple(hv_Row1.TupleLength())).TupleGreater(0))) != 0)) {
            HOperatorSet.SetColor(hSmartWindowControl1.HalconWindow, "red");
            HOperatorSet.SetLineWidth(hSmartWindowControl1.HalconWindow, 2);
            HOperatorSet.SetDraw(hSmartWindowControl1.HalconWindow, "margin");

            ho_Rectangle.Dispose();
            HOperatorSet.GenRectangle1(out ho_Rectangle, hv_Row1 - 30, hv_Col1 - 30, hv_Row2 + 30, hv_Col2 + 30);
            //paint_xld (Rectangle, image, ImageResult, [0, 255, 0, 255, 255, 255])
            ho_Rectangle.DispObj(hSmartWindowControl1.HalconWindow);

            HOperatorSet.DispText(hSmartWindowControl1.HalconWindow, hv_Area, "image", hv_Row1 - 110, hv_Col1 - 40, "black", new HTuple(), new HTuple());
            //set_display_font(hSmartWindowControl1.HalconWindow, 16, "mono", "true", "false");
            //disp_message(hSmartWindowControl1.HalconWindow, hv_Area, "image", hv_Row1 - 120, hv_Column1 - 40, "red", "false");

            ho_Rectangle.Dispose();
        }

        // ===========================================================================================================
        private void btnWhiteBlobBL_Click(object sender, EventArgs e)
        {
            float th_scale;
            if (!float.TryParse(this.txtboxThresholdScale_WhiteBlobBL.Text, out th_scale)) return;

            bool vert_close = this.chkboxVertClose_WhiteBlobBL.Checked;

            int blob_height, area_lower, area_upper;
            if (!int.TryParse(this.txtboxBlobHeight_WhiteBlobBL.Text, out blob_height)) return;
            if (!Int32.TryParse(this.txtboxAreaLower_WhiteBlobBL.Text, out area_lower)) return;
            if (!Int32.TryParse(this.txtboxAreaUpper_WhiteBlobBL.Text, out area_upper)) return;

            //HOperatorSet.ClearWindow(hSmartWindowControl1.HalconWindow);
            HOperatorSet.DispObj(ho_image, hSmartWindowControl1.HalconWindow);

            HTuple hv_Area = null, hv_Row1 = null, hv_Col1 = null, hv_Row2 = null, hv_Col2 = null;
            HObject ho_Region = HTResistor0201_method.WhiteBlobFL(ho_image, th_scale, blob_height, vert_close, area_lower, area_upper);
            UC_DisplayResult(ho_Region);
        }

        // ===========================================================================================================
        private void btnBlackBlobCL_Click(object sender, EventArgs e)
        {
            float th_scale;
            if (!float.TryParse(this.txtboxThresholdScale_BlackBlobCL.Text, out th_scale)) return;

            int intValue, area_lower, area_upper;
            if (!int.TryParse(this.txtboxMaskSize_BlackBlobCL.Text, out intValue)) return;
            if (!Int32.TryParse(this.txtboxAreaLower_BlackBlobCL.Text, out area_lower)) return;
            if (!Int32.TryParse(this.txtboxAreaUpper_BlackBlobCL.Text, out area_upper)) return;

            //HOperatorSet.ClearWindow(hSmartWindowControl1.HalconWindow);
            HOperatorSet.DispObj(ho_image, hSmartWindowControl1.HalconWindow);

            HTuple hv_Area = null, hv_Row1 = null, hv_Col1 = null, hv_Row2 = null, hv_Col2 = null;
            HObject ho_Region = HTResistor0201_method.BlackBlobCL(ho_image, intValue, th_scale, area_lower, area_upper);
            UC_DisplayResult(ho_Region);
        }

        // ===========================================================================================================
        private void btnWhiteCrackFL_Click(object sender, EventArgs e)
        {
            float th_scale;
            if (!float.TryParse(this.txtboxThresholdScale_WhiteCrackFL.Text, out th_scale)) return;

            bool vert_close = this.chkboxVertClose_WhiteCrackFL.Checked;

            int crack_width, area_lower, area_upper;
            if (!int.TryParse(this.txtboxCrackWidth_WhiteCrackFL.Text, out crack_width)) return;
            if (!Int32.TryParse(this.txtboxAreaLower_WhiteCrackFL.Text, out area_lower)) return;
            if (!Int32.TryParse(this.txtboxAreaUpper_WhiteCrackFL.Text, out area_upper)) return;

            //HOperatorSet.ClearWindow(hSmartWindowControl1.HalconWindow);
            HOperatorSet.DispObj(ho_image, hSmartWindowControl1.HalconWindow);

            HTuple hv_Area = null, hv_Row1 = null, hv_Col1 = null, hv_Row2 = null, hv_Col2 = null;
            HObject ho_Region = HTResistor0201_method.WhiteCrackFL(ho_image, th_scale, crack_width, vert_close, area_lower, area_upper);
            UC_DisplayResult(ho_Region);

            //HOperatorSet.ClearWindow(hSmartWindowControl1.HalconWindow);
            //HOperatorSet.SetDraw(hSmartWindowControl1.HalconWindow, "fill");
            //HOperatorSet.DispObj(ho_Region, hSmartWindowControl1.HalconWindow);
        }
    }
}
