using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeltaSubstrateInspector.src.Roles;
using DeltaSubstrateInspector.src.Modules.OperateMethods.InspectModule;

using HalconDotNet;

namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.HTResistor0201
{
    public partial class HTResistor0201 : OperateMethod
    {
        // ==================================================================================
        public override HObject get_defect_rgn(InspectRole role, List<HObject> src_imgs)
        {
            HObject DisplayRegion;

            // 設定參數
            set_parameter((HTResistor0201Role)role);

            // 執行演算法
            execute(src_imgs, out DisplayRegion);
            HOperatorSet.GenEmptyObj(out DisplayRegion); // (20181219) Jeff Revised!
            return DisplayRegion;
        }

        public override HObject get_cell_rgn(InspectRole role, List<HObject> src_imgs) // (20181219) Jeff Revised!
        {
            HObject CellRegion;
            CellRegion = null;
            HOperatorSet.GenEmptyObj(out CellRegion);

            // Get...
            // 設定輸出 Region           
            

            // Return 
            return CellRegion;
        }

        // ==================================================================================
        public void set_parameter(HTResistor0201Role role)
        {
            methRole = role;
        }

        // ==================================================================================
        public void execute(List<HObject> ho_ImgList, out HObject ho_Region_HTResistor0201)
        {
            ho_Region_HTResistor0201 = null;

            // ************ Fill real AI prediction function start
            HObject ho_Img = null, ho_Region, ho_RegionUnion;
            HOperatorSet.GenEmptyObj(out ho_Img);

            int inspectImgIndex = methRole.InspectImgIndex;
            ho_Img = ho_ImgList[inspectImgIndex].Clone();

            switch (inspectImgIndex)
            {
                case 1:
                    ho_RegionUnion = BrightBlobBL(ho_Img, int.Parse(methRole.strThresholdOffset_BrightBlobBL), 
                        int.Parse(methRole.strAreaLower_BrightBlobBL), int.Parse(methRole.strAreaUpper_BrightBlobBL));
                    break;
                case 2:
                    ho_RegionUnion = BlackBlobCL(ho_Img, int.Parse(methRole.strMaskSize_BlackBlobCL), float.Parse(methRole.strThresholdScale_BlackBlobCL),
                        int.Parse(methRole.strAreaLower_BlackBlobCL), int.Parse(methRole.strAreaUpper_BlackBlobCL));
                    break;
                case 3:
                    ho_Region = WhiteBlobFL(ho_Img, float.Parse(methRole.strThresholdScale_WhiteBlobFL), int.Parse(methRole.strBlobHeight_WhiteBlobFL), 
                        methRole.bolVertClose_WhiteBlobFL, int.Parse(methRole.strAreaLower_WhiteBlobFL), int.Parse(methRole.strAreaUpper_WhiteBlobFL));
                    ho_RegionUnion = WhiteCrackFL(ho_Img, float.Parse(methRole.strThresholdScale_WhiteCrackFL), int.Parse(methRole.strCrackWidth_WhiteCrackFL), 
                        methRole.bolVertClose_WhiteCrackFL, int.Parse(methRole.strAreaLower_WhiteCrackFL), int.Parse(methRole.strAreaUpper_WhiteCrackFL));
                    HOperatorSet.ConcatObj(ho_RegionUnion, ho_Region, out ho_RegionUnion);

                    ho_Region = BlackCrackFL(ho_Img, float.Parse(methRole.strThresholdScale_BlackCrackFL), int.Parse(methRole.strCrackHeight_BlackCrackFL), 
                        methRole.bolVertClose_BlackCrackFL, int.Parse(methRole.strAreaLower_BlackCrackFL), int.Parse(methRole.strAreaUpper_BlackCrackFL));
                    HOperatorSet.ConcatObj(ho_RegionUnion, ho_Region, out ho_RegionUnion);
                    break;
            }
        }

        // ==================================================================================
        public void action(List<HObject> input_ImgList, out HObject result_image)
        {
            result_image = null;
        }

        public HTResistor0201Role methRole = new HTResistor0201Role();
        // ==================================================================================
        public HObject F_LocalThresholding(HObject ho_image, HTuple lightDark, int mask_size, float scale)
        {
            HObject ho_GrayImage, ho_Region;

            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_GrayImage);

            HTuple hv_Width = null, hv_Height = null;
            HOperatorSet.GetImageSize(ho_image, out hv_Width, out hv_Height);

            ho_GrayImage.Dispose();
            HOperatorSet.Rgb1ToGray(ho_image, out ho_GrayImage);

            ho_Region.Dispose();
            HOperatorSet.LocalThreshold(ho_GrayImage, out ho_Region, "adapted_std_deviation", lightDark,
                (new HTuple("mask_size")).TupleConcat("scale"), ((((((hv_Width / mask_size)).TupleRound()) * 2) + 1)).TupleConcat(scale));
            return ho_Region;
        }

        // ==================================================================================
        public HObject F_CrackSegmentation(HObject ho_Region, int numValue, bool vert_close)
        {
            HObject ho_RegionClosing, ho_RegionDifference, ho_RegionOpening;

            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);

            ho_RegionClosing.Dispose();
            if (vert_close)
                HOperatorSet.ClosingRectangle1(ho_Region, out ho_RegionClosing, 3, numValue);
            else
                HOperatorSet.ClosingRectangle1(ho_Region, out ho_RegionClosing, numValue, 3);

            ho_RegionDifference.Dispose();
            HOperatorSet.Difference(ho_RegionClosing, ho_Region, out ho_RegionDifference);

            ho_RegionOpening.Dispose();
            if (vert_close)
                HOperatorSet.OpeningRectangle1(ho_RegionDifference, out ho_RegionOpening, 5, 1);
            else
                HOperatorSet.OpeningRectangle1(ho_RegionDifference, out ho_RegionOpening, 1, 5);

            return ho_RegionOpening;
        }
    }
}
