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

        // === 背光 ========================================================================================================
        private void btnBrightBlobBL_Click(object sender, EventArgs e)
        {
            int th_offset, area_lower, area_upper;

            if (!Int32.TryParse(this.txtboxAreaLower_BrightBlobBL.Text, out area_lower)) return;
            if (!Int32.TryParse(this.txtboxAreaUpper_BrightBlobBL.Text, out area_upper)) return;
            if (!int.TryParse(this.txtboxThresholdOffset_BrightBlobBL.Text, out th_offset)) return;

            HObject ho_Region = HTResistor0201_method.BrightBlobBL(ho_image, th_offset, area_lower, area_upper);
            UC_DisplayResult(ho_Region);
        }

        // ==================================================================================
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

            HObject ho_Region = HTResistor0201_method.BlackBlobCL(ho_image, intValue, th_scale, area_lower, area_upper);
            UC_DisplayResult(ho_Region);
        }

        // ===========================================================================================================
        private void F_FrontLight(string defect)
        {
            HObject ho_Region, ho_RegionUnion;
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);

            float th_scale;
            bool vert_close, flag = false;
            int crack_widthOrHeight, area_lower, area_upper;

            //HOperatorSet.ClearWindow(hSmartWindowControl1.HalconWindow);
            HOperatorSet.DispObj(ho_image, hSmartWindowControl1.HalconWindow);

            if (defect == "All")
            {
                flag = true;
                defect = "BlackCrack";
            }

            switch (defect)
            {
                case "BlackCrack":
                    vert_close = this.chkboxVertClose_BlackCrackFL.Checked;
                    if (!Int32.TryParse(this.txtboxAreaLower_BlackCrackFL.Text, out area_lower)) return;
                    if (!Int32.TryParse(this.txtboxAreaUpper_BlackCrackFL.Text, out area_upper)) return;
                    if (!float.TryParse(this.txtboxThresholdScale_BlackCrackFL.Text, out th_scale)) return;
                    if (!int.TryParse(this.txtboxCrackHeight_BlackCrackFL.Text, out crack_widthOrHeight)) return;

                    ho_Region = HTResistor0201_method.BlackCrackFL(ho_image, th_scale, crack_widthOrHeight, vert_close, area_lower, area_upper);

                    ho_RegionUnion = ho_Region;
                    if (flag)
                        goto case "WhiteCrack";
                    else
                        break;

                case "WhiteCrack":
                    vert_close = this.chkboxVertClose_WhiteCrackFL.Checked;
                    if (!Int32.TryParse(this.txtboxAreaLower_WhiteCrackFL.Text, out area_lower)) return;
                    if (!Int32.TryParse(this.txtboxAreaUpper_WhiteCrackFL.Text, out area_upper)) return;
                    if (!float.TryParse(this.txtboxThresholdScale_WhiteCrackFL.Text, out th_scale)) return;
                    if (!int.TryParse(this.txtboxCrackWidth_WhiteCrackFL.Text, out crack_widthOrHeight)) return;

                    ho_Region = HTResistor0201_method.WhiteCrackFL(ho_image, th_scale, crack_widthOrHeight, vert_close, area_lower, area_upper);

                    if (flag)
                    {
                        HOperatorSet.ConcatObj(ho_RegionUnion, ho_Region, out ho_RegionUnion);
                        goto case "WhiteBlob";
                    }
                    else
                    {
                        ho_RegionUnion = ho_Region;
                        break;
                    }

                case "WhiteBlob":
                    vert_close = this.chkboxVertClose_WhiteBlobFL.Checked;
                    if (!Int32.TryParse(this.txtboxAreaLower_WhiteBlobFL.Text, out area_lower)) return;
                    if (!Int32.TryParse(this.txtboxAreaUpper_WhiteBlobFL.Text, out area_upper)) return;
                    if (!float.TryParse(this.txtboxThresholdScale_WhiteBlobFL.Text, out th_scale)) return;
                    if (!int.TryParse(this.txtboxBlobHeight_WhiteBlobFL.Text, out crack_widthOrHeight)) return;

                    ho_Region = HTResistor0201_method.WhiteBlobFL(ho_image, th_scale, crack_widthOrHeight, vert_close, area_lower, area_upper);

                    if (flag)
                        HOperatorSet.ConcatObj(ho_RegionUnion, ho_Region, out ho_RegionUnion);
                    else
                        ho_RegionUnion = ho_Region;
                    break;
            }
            UC_DisplayResult(ho_RegionUnion);
        }

        // ===========================================================================================================
        private void btnBlackCrackFL_Click(object sender, EventArgs e)
        {
            if (this.chkboxCombinedTest.Checked)
                F_FrontLight("All");
            else
                F_FrontLight("BlackCrack");
        }

        // ==================================================================================
        private void btnWhiteCrackFL_Click(object sender, EventArgs e)
        {
            if (this.chkboxCombinedTest.Checked)
                F_FrontLight("All");
            else
                F_FrontLight("WhiteCrack");
        }

        // ==================================================================================
        private void btnWhiteBlobFL_Click(object sender, EventArgs e)
        {
            if (this.chkboxCombinedTest.Checked)
                F_FrontLight("All");
            else
                F_FrontLight("WhiteBlob");
        }
    }
}
