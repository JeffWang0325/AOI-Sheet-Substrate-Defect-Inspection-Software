using HalconDotNet;
using DeltaSubstrateInspector.src.PositionMethod.LocateMethod.Location;
using DeltaSubstrateInspector.src.PositionMethod.LocateMethod.Rotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extension;
using System.Drawing;

using static DeltaSubstrateInspector.FileSystem.FileSystem; // (20190125) Jeff Revised!

namespace DeltaSubstrateInspector.src.Modules
{
    public class PositionModel
    {
        private List<HObject> locate_used_imgs_ = new List<HObject>();
        private AngleAffineMethod affine_method_ = new AngleAffineMethod();
        private LocateMethod locate_method_ = new LocateMethod();
        private HObject affine_model_ = new HObject();
        private HObject region_ = new HObject();
        //private List<Point> all_locate_in_move = new List<Point>(); // (20181119) Jeff Revised!
        /// <summary>
        /// 總Cell顆數
        /// </summary>
        private int total_cell_count { get; set; } = 0;
        private bool status_ = false;
        /// <summary>
        /// 視覺定位是否成功
        /// </summary>
        private bool find_shpe_model_success = false;

        public List<Point> mark_position_lst_ = new List<Point>(); // Andy, 視覺對位座標
        public List<PointF> mark_position_lst_f { get; set; } = new List<PointF>(); // 181220, Andy, 視覺對位座標

        public PositionModel()
        {
            //locate_method_.LocateMethod_Constructor(); // (20200429) Jeff Revised!
        }

        public void initialize()
        {
            // Release memory (20190405) Jeff Revised!
            for (int i = 0; i < locate_used_imgs_.Count; i++)
            {
                if (locate_used_imgs_[i] != null)
                {
                    locate_used_imgs_[i].Dispose();
                    locate_used_imgs_[i] = null;
                }
            }
            locate_used_imgs_.Clear();
        }
        
        public void set_affine_method(AngleAffineMethod method)
        {
            affine_method_ = method;
        }

        public void set_locate_method(LocateMethod method)
        {
            locate_method_ = method;
        }

        public LocateMethod get_locate_method() // (20181116) Jeff Revised!
        {
            return locate_method_;
        }

        /// <summary>
        /// 計算基板偏斜角度
        /// </summary>
        public void cal_rotation_angle(bool b_save_Image = false, bool b_draw_Image = false, bool b_save_rotatedImage = false) // (20190112) Jeff Revised!
        {
            find_shpe_model_success = false;

            if (affine_method_.ModelEmpty)
                affine_method_.load();

            if (affine_method_.EdgeSearchParam.AlignmentType == AngleAffineMethod.enuAlignmentType.PatternMatch)
            {
                //find_shpe_model_success = affine_method_.find_shpe_model_2(locate_used_imgs_[0].Clone(), locate_used_imgs_[1].Clone(), b_save_Image, b_draw_Image); // (20190105) Jeff Revised!
                find_shpe_model_success = affine_method_.find_shpe_model_2(locate_used_imgs_[0], locate_used_imgs_[1], b_save_Image, b_draw_Image); // (20190412) Jeff Revised!
            }
            else if (affine_method_.EdgeSearchParam.AlignmentType == AngleAffineMethod.enuAlignmentType.EdgeSearch)
            {
                find_shpe_model_success = affine_method_.FindEdgePoins(locate_used_imgs_[0].Clone(), locate_used_imgs_[1].Clone(), b_save_Image);
            }

            if (!find_shpe_model_success) // 定位失敗 (20190402) Jeff Revised!
            {
                locate_method_.center_points_x_ = null;
                locate_method_.center_points_y_ = null;
                //Affine_angle_degree = null;
                mark_position_lst_f.Clear();
                return;
            }
            Affine_angle_degree = affine_method_.Affine_degree_; // (20190125) Jeff Revised!

            #region 是否儲存扭正後影像
            if (b_save_rotatedImage) // (20190112) Jeff Revised!
            {
                HObject rotate_image_1, rotate_image_2;
                affine_method_.rotate_image_(null, locate_used_imgs_[0].Clone(), out rotate_image_1,
                                             true, "RotateImage1.tiff");
                affine_method_.rotate_image_(null, locate_used_imgs_[1].Clone(), out rotate_image_2,
                                             true, "RotateImage2.tiff");

                if (rotate_image_1 != null)
                {
                    rotate_image_1.Dispose();
                    rotate_image_1 = null;
                }

                if (rotate_image_2 != null)
                {
                    rotate_image_2.Dispose();
                    rotate_image_2 = null;
                }

            }
            #endregion

            locate_method_.update_affine(affine_method_);
            locate_method_.initialize(); // (20180829) Jeff Revised!
            total_cell_count = locate_method_.get_total_cell_count();

            // Andy            
            mark_position_lst_f.Clear();
            for (int i = 0; i < 2; i++)
            {              
                float mark_x_FLOAT = (float)affine_method_.mark_x_[i].D;
                float mark_y_FLOAT = (float)affine_method_.mark_y_[i].D;
                mark_position_lst_f.Add(new PointF(mark_x_FLOAT, mark_y_FLOAT));
            }
          
        }

        /// <summary>
        /// 建立 Cell Map (不做視覺定位)
        /// </summary>
        /// <returns></returns>
        public bool initialize_NoPos() // (20191216) Jeff Revised!
        {
            find_shpe_model_success = true;
            locate_method_.update_affine(affine_method_);
            return locate_method_.initialize_NoPos();
        }

        public HObject Now_Mark1_ResultImg
        {
            get{ return affine_method_.Now_Mark1_ResultImg; }
        }

        public HObject Now_Mark2_ResultImg
        {
            get { return affine_method_.Now_Mark2_ResultImg; }
        }

        /// <summary>
        /// 計算總瑕疵之座標
        /// </summary>
        /// <param name="defects_with_names"></param>
        /// <param name="move_index"></param>
        /// <returns></returns>
        public Dictionary<string, List<Point>> get_defect_locate(Dictionary<string, HObject> defects_with_names, int move_index) // (20190716) Jeff Revised!
        {
            Dictionary<string, List<Point>> locates_with_name = new Dictionary<string, List<Point>>();
            foreach (string name in defects_with_names.Keys)
            {
                HObject defect_region_, BackDefect;
                List<Point> locations_ = new List<Point>();
                //HOperatorSet.WriteRegion(defects_with_names[name], "defects_with_names"); // For debug! (20191216) Jeff Revised!
                locate_method_.label_defect(move_index, defects_with_names[name], out defect_region_, out locations_, out BackDefect, true, false);
                locates_with_name.Add(name, locations_);
                defect_region_.Dispose();
                BackDefect.Dispose();
            }
            return locates_with_name;
        }

        /// <summary>
        /// 初始化人工覆判
        /// </summary>
        public void Initialize_Recheck() // (20190719) Jeff Revised!
        {
            locate_method_.Initialize_Recheck();
        }

        /// <summary>
        /// 計算各類型瑕疵之座標
        /// </summary>
        /// <param name="defects_with_names"></param>
        /// <param name="move_index"></param>
        /// <returns></returns>
        public bool get_DefectsClassify_locate(Dictionary<string, HObject> defects_with_names, int move_index) // (20190717) Jeff Revised!
        {
            return locate_method_.label_defect_DefectsClassify(move_index, defects_with_names, false, false);
        }

        public bool update_CellCenterReg(HObject CellRegion, int move_index) // (20181207) Jeff Revised!
        {
            bool b_status_ = false;

            if (locate_method_.label_CellCenter(move_index, CellRegion))
                b_status_ = true;

            return b_status_;
        }

        /// <summary>
        ///  一次紀錄所有未被檢測到的Cell中心點regions之座標，並將之加入至瑕疵座標 all_defect_id_ 中
        /// </summary>
        /// <param name="b_CellDefect">是否考慮未被檢測到的Cell中心點regions之座標</param>
        /// <returns></returns>
        public bool update_all_CellDefect(bool b_CellDefect = false) // (20181207) Jeff Revised!
        {
            return locate_method_.label_all_CellDefect(b_CellDefect);
        }

        public int GetMatchFailCount()
        {
            return locate_method_.Get_all_CellDefect_id_List().Count;
        }

        public HObject get_big_map(List<HObject> all_List_images)
        {
            try
            {
                return locate_method_.tile_images2(all_List_images);
            }
            catch { return null; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="all_concat_images_"></param>
        /// <param name="b_resize_finished">輸入影像all_concat_images_是否已做resize</param>
        /// <returns></returns>
        public HObject get_big_map(HObject all_concat_images_, bool b_resize_finished = false) // (20190606) Jeff Revised!
        {
            try
            {
                return locate_method_.tile_images2(all_concat_images_, b_resize_finished);
            }
            catch { return null; }
        }

        public HObject get_cellmap_affine() // (20181011) Jeff Revised!
        {
            return locate_method_.cellmap_affine_;
        }

        public HObject get_defect_region() // (20181025) Jeff Revised!
        {
            return locate_method_.all_intersection_defect_;
        }

        public HObject get_capture_map() // (20181025) Jeff Revised!
        {
            return locate_method_.capture_map_;
        }

        public void save()
        {
            affine_method_.save();
            locate_method_.save();
        }

        public void load()
        {
            //if (affine_method_.load() && locate_method_.load())
            if (locate_method_.load() && affine_method_.load()) // 比AffineMethodObj.load()先執行，為了先產生一張影像，才能順利載入region (20190509) Jeff Revised!
                status_ = true;
        }

        public void set_type_name(string type_name)
        {
            affine_method_.BoardType = type_name;
            locate_method_.BoardType = type_name;
        }

        public HObject affine_img(HObject img)
        {
            affine_method_.rotate_image_2(img, out img);
            return img;
        }

        /// <summary>
        /// 定位影像集合
        /// </summary>
        public List<HObject> locateUseImgs
        {
            get { return this.locate_used_imgs_; }
            set { this.locate_used_imgs_ = value; }
        }

        public List<Point> get_locates()
        {
            return locate_method_.get_total_defect_pos();
        }

        public HObject get_region()
        {
            return this.region_;
        }

        /// <summary>
        /// 總Cell顆數
        /// </summary>
        public int total // (20191216) Jeff Revised!
        {
            //get { return total_cell_count; }
            
            get
            {
                total_cell_count = locate_method_.get_total_cell_count();
                return total_cell_count;
            }
        }

        public bool Status
        {
            get { return status_; }
        }

        /// <summary>
        /// 視覺定位是否成功
        /// </summary>
        public bool Find_Shpe_Model_Success
        {
            get { return find_shpe_model_success; }
        }

        /// <summary>
        /// 清除所有瑕疵區域及瑕疵座標
        /// </summary>
        /// <returns></returns>
        public bool Clear_all_defects() // (20181112) Jeff Revised!
        {
            locate_method_.clear_all_defects();

            return true;
        }

        /// <summary>
        /// 釋放記憶體
        /// </summary>
        public void Dispose() // (20200429) Jeff Revised!
        {
            locate_method_.Dispose();
        }

        /// <summary>
        /// 設定瑕疵檔路徑
        /// </summary>
        /// <param name="Path_DefectTest"></param>
        public void Set_Path_DefectTest(string Path_DefectTest) // (20200429) Jeff Revised!
        {
            locate_method_.Path_DefectTest = Path_DefectTest;
        }

        public bool Initialize_DefectsClassify(Dictionary<string, DefectsResult> defectsClassify) // (20190716) Jeff Revised!
        {
            return locate_method_.Initialize_DefectsClassify(defectsClassify);
        }

        public int Get_Cell_X_Count() // (20190719) Jeff Revised!
        {
            return locate_method_.cell_col_count_;
        }

        public int Get_Cell_Y_Count() // (20190719) Jeff Revised!
        {
            return locate_method_.cell_row_count_;
        }

        public Dictionary<string, DefectsResult> Get_DefectsClassify() // (20190720) Jeff Revised!
        {
            return locate_method_.DefectsClassify;
        }

    }
}
