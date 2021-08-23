using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DeltaSubstrateInspector.src.Roles;
using DeltaSubstrateInspector.src.Modules.NewInspectModule;
using DeltaSubstrateInspector.src.Modules.OperateMethods.InspectModule;

using HalconDotNet;

namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.HTResistor0201
{
    public partial class HTResistor0201 : OperateMethod
    {
        public HObject BlackBlobCL(HObject ho_image, int intValue, float th_scale, int area_lower, int area_upper)
        {
            // Local iconic variables 
            HObject ho_GrayImage, ho_Region;
            HObject ho_RegionClosing, ho_ConnectedRegions, ho_SelectedRegions;

            // Local control variables 
            //HTuple hv_Row = null, hv_Column = null;

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_GrayImage);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);

            ho_Region.Dispose();
            ho_Region = F_LocalThresholding(ho_image, "dark", intValue, th_scale);

            ho_RegionClosing.Dispose();
            HOperatorSet.ClosingRectangle1(ho_Region, out ho_RegionClosing, 11, 1);
            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_RegionClosing, out ho_ConnectedRegions);

            ho_SelectedRegions.Dispose();
            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area", "and", area_lower, area_upper);

            //HOperatorSet.AreaCenter(ho_SelectedRegions, out hv_Area, out hv_Row, out hv_Column);
            //HOperatorSet.SmallestRectangle1(ho_SelectedRegions, out hv_Row1, out hv_Col1, out hv_Row2, out hv_Col2);

            ho_GrayImage.Dispose();
            ho_Region.Dispose();
            ho_RegionClosing.Dispose();
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
