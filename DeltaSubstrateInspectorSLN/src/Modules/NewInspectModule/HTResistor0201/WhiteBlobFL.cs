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
        // ==================================================================================
        public HObject WhiteBlobFL(HObject ho_image, float th_scale, int blob_height, bool vert_close, int area_lower, int area_upper)
        {
            // Local iconic variables 
            HObject ho_Region, ho_RegionOpening, ho_ConnectedRegions, ho_SelectedRegions;

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);

            ho_Region.Dispose();
            ho_Region = F_LocalThresholding(ho_image, "dark", 32, th_scale);

            ho_RegionOpening.Dispose();
            ho_RegionOpening = F_CrackSegmentation(ho_Region, blob_height, vert_close);
            //if (ho_RegionOpening == null) return false;

            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectedRegions);

            ho_SelectedRegions.Dispose();
            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area", "and", area_lower, area_upper);

            ho_Region.Dispose();
            ho_RegionOpening.Dispose();
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
