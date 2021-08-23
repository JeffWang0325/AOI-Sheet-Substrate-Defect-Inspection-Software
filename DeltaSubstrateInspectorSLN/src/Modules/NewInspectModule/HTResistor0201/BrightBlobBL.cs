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
        public HObject F_DynamicThresholding(HObject ho_image, int th_offset)
        {
            HTuple hv_Width = null, hv_Height = null;
            HObject ho_GrayImage, ho_Region, ho_Mean;
            HOperatorSet.GetImageSize(ho_image, out hv_Width, out hv_Height);

            HOperatorSet.GenEmptyObj(out ho_Mean);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_GrayImage);

            ho_GrayImage.Dispose();
            HOperatorSet.Rgb1ToGray(ho_image, out ho_GrayImage);
            //threshold (GrayImage, Region, 235, 255)
            ho_Mean.Dispose();
            HOperatorSet.MeanImage(ho_GrayImage, out ho_Mean, ((((hv_Width / 128)).TupleRound()) * 2) + 1, ((((hv_Height / 128)).TupleRound()) * 2) + 1);

            ho_Region.Dispose();
            HOperatorSet.DynThreshold(ho_GrayImage, ho_Mean, out ho_Region, th_offset, "light");
            //local_threshold (GrayImage, Region, 'adapted_std_deviation', 'dark', ['mask_size', 'scale', 'range'], [round(Width/8) * 2 + 1, .95, 128])
            return ho_Region;
        }

        // ==================================================================================
        public HObject BrightBlobBL(HObject ho_image, int th_offset, int area_lower, int area_upper)
        {
            // Local iconic variables 
            HObject ho_ConnectedRegions, ho_SelectedRegions;

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);

            // Local control variables 
            //HTuple hv_Row = null, hv_Column = null;
            //hv_Area = null; hv_Row1 = null; hv_Row2 = null; hv_Col1 = null; hv_Col2 = null;

            //***** detect the brightest blobs ******
            HObject ho_Region = F_DynamicThresholding(ho_image, th_offset);
            //if (ho_Region == null) return false;

            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_Region, out ho_ConnectedRegions);

            ho_SelectedRegions.Dispose();
            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area", "and", area_lower, area_upper);

            //HOperatorSet.AreaCenter(ho_SelectedRegions, out hv_Area, out hv_Row, out hv_Column);
            //HOperatorSet.SmallestRectangle1(ho_SelectedRegions, out hv_Row1, out hv_Col1, out hv_Row2, out hv_Col2);

            ho_Region.Dispose();
            ho_ConnectedRegions.Dispose();
            //ho_SelectedRegions.Dispose();

            return ho_SelectedRegions;

            //if ((int)(new HTuple(hv_Area.TupleEqual(new HTuple()))) == 0)
            //    return true;
            //else
            //    return false;
        }
    }
}
