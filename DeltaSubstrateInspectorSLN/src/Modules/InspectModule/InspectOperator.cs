using HalconDotNet;
using DeltaSubstrateInspector.FileSystem;
using DeltaSubstrateInspector.src.InspectionForms;
using DeltaSubstrateInspector.src.Roles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Extension;
using DeltaSubstrateInspector.src.Modules.InspectModule.ImageProcess;
using static DeltaSubstrateInspector.FileSystem.FileSystem;

namespace DeltaSubstrateInspector.src.Modules.InspectModule
{
    public class InspectOperator
    {
        // Attribute
        private List<InspectRole> op_roles_; // Include all inspect roles.
        /// <summary>
        /// 該位置影像之總瑕疵
        /// </summary>
        private List<Defect> TotalDefectReg_MoveIndex = new List<Defect>();
        private AfineTransform1 trans = new AfineTransform1();
        private HObject result;
        private string[] operator_info_;
        private double afine_angle;
        private string position_addition_info_ = "";
        private string laser_position_addition_info_ = "";
        public OperateEngine op_engine_; // (20180905) Jeff Revised!

        /// <summary>
        /// 該位置影像之各類型瑕疵結果
        /// </summary>
        private Dictionary<string, List<Defect>> dicDefectList = new Dictionary<string, List<Defect>>();

        // 之後要改成private
        public PnlSide side_ = 0;

        // Functions
        public InspectOperator(string name, PnlSide side)
        {
            op_roles_ = new List<InspectRole>();
            side_ = side;
            operator_info_ = new string[4];
            op_engine_ = new OperateEngine(); // (20180905) Jeff Revised!
        }

        public void create_roles()
        {
            using (SettingForm param_setting = new SettingForm())
            {
                param_setting.ShowDialog(); // ?
                op_roles_ = param_setting.get_inspect_roles();
               string afine_method_ = param_setting.get_afine_method_name();
            }
        }

        public void load()
        {
            using (SettingForm param_setting = new SettingForm())
            {
               
                param_setting.load(op_roles_);

                param_setting.ShowDialog(); // ?                

            }
        }

        public void get_afine_angle(HObject src_img)
        {
            HObject dst_img;
            double angle = 0;
            trans.create_afffine_trans(src_img, out angle, out dst_img);
        }

        public void excute_roles(List<HObject> image)
        {
            // Set up the channel image(HObject) by light type.
            List<String> light_name_lst_ = new List<string>();
            light_name_lst_.Add("BL");
            light_name_lst_.Add("OL");

            List<InspectRole> color_sep_role = op_roles_.Where(x => x.Method == "MultiThreshold").ToList();

            if (color_sep_role.Count > 0)
                op_engine_.set_color_sep(color_sep_role, image, light_name_lst_);



            TotalDefectReg_MoveIndex.Clear();
            dicDefectList.Clear();
            for (int i = 0; i < op_roles_.Count; i++) // 檢測演算法數量 (20190716) Jeff Revised!
            {
                InspectRole role = op_roles_[i];
                HObject region = op_engine_.get_role_result(role, image);  // 瑕疵Region
                HObject cellRegion = op_engine_.get_role_cellRegion(role, image); // 181203, andy, 每個Cell region
                List<Defect> defectResult = op_engine_.get_role_defect_result(role, image); // 181218, andy

                //HOperatorSet.WriteImage(image[0], "tiff", 0, "image0"); // For debug! (20191216) MIL Jeff Revised!
                //HOperatorSet.WriteRegion(region, "defects_region"); // For debug! (20191216) MIL Jeff Revised!
                //HOperatorSet.WriteRegion(cellRegion, "cellRegion"); // For debug! (20191216) MIL Jeff Revised!
                Defect defect = new Defect(role.InspectName, result, region, cellRegion); // (20181207) Jeff Revised!
                TotalDefectReg_MoveIndex.Add(defect);
                
                dicDefectList.Add(role.InspectName, defectResult); // 181218, andy
                
            }

        }

        public Dictionary<string, DefectsResult> Initialize_DefectsClassify() // (20190716) Jeff Revised!
        {
            return op_engine_.get_role_Initialize_DefectsClassify(op_roles_[0]);
        }

        /// <summary>
        /// 取得該位置影像之總瑕疵
        /// </summary>
        /// <returns></returns>
        public List<Defect> get_TotalResult()
        {
            return TotalDefectReg_MoveIndex;
        }

        /// <summary>
        /// 取得該位置影像之各類型瑕疵結果
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<Defect>> get_dicDefectList()
        {
            return dicDefectList;
        }
        
        public HObject get_result_img()
        {
            return result;
        }

        public List<Defect> get_empty_result()
        {
            List<Defect> defect_list = new List<Defect>();
            try
            {
                for (int i = 0; i < op_roles_.Count; i++)
                {
                    InspectRole role = op_roles_[i];
                    HObject region;
                    HObject cell_region;
                    HOperatorSet.GenEmptyObj(out region);
                    HOperatorSet.GenEmptyObj(out cell_region);
                    HOperatorSet.PaintRegion(region, region, out result, 255, "fill");
                    //HOperatorSet.WriteImage(result, "tiff", 0, "C:/Users/WANG/Desktop/台達電子AOI/華新科技片檢機_評估案/點膠機Based Demo/Program/result2"); // For debug! (20180820) Jeff Revised!
                    Defect defect = new Defect(role.InspectName, result, region, cell_region);
                    defect_list.Add(defect);
                }
            }
            catch
            {
                MessageBox.Show("By Pass 位置內部計算異常，請通知工程人員查悉");
            }
            return defect_list;
        }

        // 181218, andy
        public Dictionary<string, List<Defect>> get_empty_dicDefectList()
        {
            Dictionary<string, List<Defect>> empty_list = new Dictionary<string, List<Defect>>();

            try
            {
                for (int i = 0; i < op_roles_.Count; i++)
                {
                    InspectRole role = op_roles_[i];
                    HObject region;
                    HObject cell_region;
                    HOperatorSet.GenEmptyObj(out region);
                    HOperatorSet.GenEmptyObj(out cell_region);
                    HOperatorSet.PaintRegion(region, region, out result, 255, "fill");
                    //HOperatorSet.WriteImage(result, "tiff", 0, "C:/Users/WANG/Desktop/台達電子AOI/華新科技片檢機_評估案/點膠機Based Demo/Program/result2"); // For debug! (20180820) Jeff Revised!
                    Defect defect = new Defect(role.InspectName, result, region, cell_region);


                    // 181218, andy
                    List<Defect> defectResult = new List<Defect>();
                    defectResult.Add(defect);
                    empty_list.Add(role.InspectName, defectResult);
                    
                }

            }
            catch
            {
                MessageBox.Show("By Pass 位置內部計算異常，請通知工程人員查悉");
            }

            return empty_list;
        }


        public List<InspectRole> Roles
        {
            get { return op_roles_; }
        }



        ///<summery>
         ///<param name="input_image_">0: Operator Name | 1 : 定位方法 | 2 : 量測或檢測 | 3 : 大範圍檢測或單一元件檢測 </param>
        /// <param name="dev_angle_">原始影像偏差角度</param>
        /// <param name="affine_image_">扭正影像</param>
        ///</summery>
        public string[] OperatorInfo
        {
            get { return this.operator_info_; }
            set { this.operator_info_ = value; }
        }

        public List<string> get_roles_name()
        {
            return op_roles_.Select(x => x.InspectName).ToList();
        }

        public List<string> get_methods_name()
        {
            return op_roles_.Select(x => x.Method).ToList();
        }

        public void release()
        {
            foreach (var item in TotalDefectReg_MoveIndex)
            {
                item.release();
            }
            TotalDefectReg_MoveIndex = null;
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            TotalDefectReg_MoveIndex = new List<Defect>();
        }

        public string PosAddInfo
        {
            get { return this.position_addition_info_; }
            set { this.position_addition_info_ = value; }
        }

        public string LaserPosAddInfo
        {
            get { return this.laser_position_addition_info_; }
            set { this.laser_position_addition_info_ = value; }
        }
        
    }
}
