using HalconDotNet;
using DeltaSubstrateInspector.src.Roles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extension;
using System.Xml;

using DeltaSubstrateInspector.src.Modules.OperateMethods.InspectModule;
using DeltaSubstrateInspector.src.Modules.MotionModule; // For 華新trim痕檢測 (20180905) Jeff Revised!

using DeltaSubstrateInspector.src.Modules.NewInspectModule.PatternCheck;
using DeltaSubstrateInspector.src.Modules.NewInspectModule.DefaultInspect;
using DeltaSubstrateInspector.src.Modules.NewInspectModule.HTResistor0201;
using DeltaSubstrateInspector.src.Modules.NewInspectModule.HTResistor0402;
using DeltaSubstrateInspector.src.Modules.NewInspectModule.InductanceInsp;
using DeltaSubstrateInspector.src.Modules.NewInspectModule.ResistPanel;

namespace DeltaSubstrateInspector.src.Modules.InspectModule
{
    public class OperateEngine
    {
        private Hashtable op_method_table_ = new Hashtable();  // Table for the methods.
        private Hashtable tool_method_table_ = new Hashtable();// Table for the tool methods.  
        private OperateMethod method;

        /// <summary>
        /// OperateEngine
        /// </summary>
        public OperateEngine()
        {
            setup_method_table();
        }

        /// <summary>
        /// 建立檢測方法的table
        /// </summary>
        public void setup_method_table()
        {
          
            op_method_table_.Add("DefaultInspect", new DefaultInspect()); //Andy
            op_method_table_.Add("PatternCheck", new PatternCheck()); //Andy
            op_method_table_.Add("HTResistor0201", new HTResistor0201()); // James Chien
            op_method_table_.Add("HTResistor0402", new HTResistor0402()); // James Chien
            op_method_table_.Add("InductanceInsp", new InductanceInsp()); //Chris
            op_method_table_.Add("ResistPanel", new ResistPanel()); // (20181227) Jeff Revised!

        }

        /// <summary>
        /// set_color_sep
        /// </summary>
        /// <param name="op_roles"></param>
        /// <param name="image"></param>
        /// <param name="light_names"></param>
        public void set_color_sep(List<InspectRole> op_roles, List<HObject> image, List<string> light_names)
        {
           // ....

        }


        /// <summary>
        /// 實際執行時
        /// </summary>
        /// <param name="role"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public HObject get_role_result(InspectRole role, List<HObject> image)
        {
            // 利用 role.Method(字串) 去取得檢測方法物件
            method = (OperateMethod)op_method_table_[role.Method];
            
            // 執行檢測物件的get_defect_rgn()方法
            return method.get_defect_rgn(role, image);
        }

        public HObject get_role_cellRegion(InspectRole role, List<HObject> image)
        {
            // 利用 role.Method(字串) 去取得檢測方法物件
            method = (OperateMethod)op_method_table_[role.Method];

            // get_cell_rgn()方法
            return method.get_cell_rgn(role, image);
        }

        // 181218, andy
        public List<Defect> get_role_defect_result(InspectRole role, List<HObject> image)
        {
            // 利用 role.Method(字串) 去取得檢測方法物件
            method = (OperateMethod)op_method_table_[role.Method];

            // 執行檢測物件的get_defect_result()方法
            return method.get_defect_result(role, image);
        }

        public Dictionary<string, DefectsResult> get_role_Initialize_DefectsClassify(InspectRole role) // (20190716) Jeff Revised!
        {
            // 利用 role.Method(字串) 去取得檢測方法物件
            method = (OperateMethod)op_method_table_[role.Method];
            
            // 執行檢測物件的Initialize_DefectsClassify()方法
            return method.Initialize_DefectsClassify(role);
        }
        
    }
}
