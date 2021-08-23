using HalconDotNet;
using DeltaSubstrateInspector.src.Modules.InspectModule;
using DeltaSubstrateInspector.src.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaSubstrateInspector.src.Modules.OperateMethods.InspectModule
{
    public class OperateMethod
    {
        public OperateMethod()
        {
           //// this.parent_ = parent;
        }

        virtual public HObject get_defect_rgn(InspectRole role, List<HObject> src_imgs)
        {
            return null;
        }

        /// <summary>
        /// 181203, andy: 演算法回傳每個cell的 region
        /// </summary>
        /// <returns></returns>
        virtual public HObject get_cell_rgn(InspectRole role, List<HObject> src_imgs)
        {
            //return null;
            HObject hobj;
            HOperatorSet.GenEmptyObj(out hobj);

            return hobj;
        }

        /// <summary>
        /// 演算法回傳完整計算結果 (總瑕疵)
        /// </summary>
        /// <param name="role"></param>
        /// <param name="src_imgs"></param>
        /// <returns></returns>
        virtual public List<Defect> get_defect_result(InspectRole role, List<HObject> src_imgs)
        {
            //return null;
            List<Defect> defectResult = new List<Defect>();
            return defectResult;
        }

        virtual public Dictionary<string, DefectsResult> Initialize_DefectsClassify(InspectRole role) // (20190716) Jeff Revised!
        {
            Dictionary<string, DefectsResult> defectsClassify = new Dictionary<string, DefectsResult>();
            return defectsClassify;
        }
    }

    public class ColorSepMethod
    {
        virtual public List<HObject> get_channel_imgs_(HObject src)
        {
            return null;
        }

    }

    public class ToolMethod
    {
        virtual public HObject get_operate_result(HObject src)
        {
            return null;
        }
    }
}