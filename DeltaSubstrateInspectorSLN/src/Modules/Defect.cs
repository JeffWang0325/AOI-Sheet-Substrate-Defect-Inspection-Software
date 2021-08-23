using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extension;

namespace DeltaSubstrateInspector.src.Modules
{
    public class Defect
    {
        private string defect_name_ = "";
        private HObject result_img_; // 將一張影像之所有瑕疵區域用白色畫在黑色影像上
        private List<Rectangle> defect_rect_list_;
        private HObject defect_rgn_;
        /// <summary>
        /// Cell瑕疵顆數
        /// </summary>
        private int defect_num_;
        private List<Point> defect_location_ = new List<Point>();
        private int Algorithmindex;

        private HObject cell_rgn_; // 181203, andy 每個Cell的Region
        private HObject defect_cellcenter_rgn_; // 181219, andy defect所佔的Cell center region

        /// <summary>
        /// 是否為一種瑕疵，或是僅做為顯示用途
        /// </summary>
        public bool B_defect { get; set; } = true; // (20200429) Jeff Revised!

        public int GetAlgorithmindex
        {
            get { return this.Algorithmindex; }
        }

        private string str_Color_Halcon = "#ff0000ff"; // (20190624) Jeff Revised!
        /// <summary>
        /// 顯示顏色 (16進位格式: #RGBA)
        /// </summary>
        public string Str_Color_Halcon // (20190624) Jeff Revised!
        {
            get { return this.str_Color_Halcon; }
            set { this.str_Color_Halcon = value; }
        }

        private int priority_defect = 0; // (20190627) Jeff Revised!
        public int Priority_defect // (20190627) Jeff Revised!
        {
            get { return this.priority_defect; }
            set { this.priority_defect = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="image"></param>
        /// <param name="df_rgn"></param>
        /// <param name="cl_rgn"></param>
        /// <param name="b_defect">是否為一種瑕疵，或是僅做為顯示用途</param>
        public Defect(string name, HObject image, HObject df_rgn, HObject cl_rgn, bool _b_defect = true, string _str_Color_Halcon = "#ff0000ff")
        {
            defect_name_ = name;
            result_img_ = image;
            defect_rgn_ = df_rgn;
            //set_up_rgn();
            //set_up_defect_rects(); // (20181119) Jeff Revised!
            cell_rgn_ = cl_rgn;
            this.B_defect = _b_defect; // (20190620) Jeff Revised!
            str_Color_Halcon = _str_Color_Halcon; // (20190624) Jeff Revised!
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="image"></param>
        /// <param name="df_rgn"></param>
        /// <param name="df_cellcenter_rgn"></param>
        /// <param name="cl_rgn"></param>
        /// <param name="b_defect">是否為一種瑕疵，或是僅做為顯示用途</param>
        public Defect(string name, HObject image, HObject df_rgn, HObject df_cellcenter_rgn, HObject cl_rgn, bool _b_defect = true, string _str_Color_Halcon = "#ff0000ff")
        {
            defect_name_ = name;
            result_img_ = image;
            defect_rgn_ = df_rgn;
            //set_up_rgn();
            defect_cellcenter_rgn_ = df_cellcenter_rgn;
            //set_up_defect_rects(); // (20181119) Jeff Revised!
            cell_rgn_ = cl_rgn;
            this.B_defect = _b_defect; // (20190620) Jeff Revised!
            str_Color_Halcon = _str_Color_Halcon; // (20190624) Jeff Revised!
        }

        public Defect(string name, HObject image, HObject df_rgn, HObject df_cellcenter_rgn, HObject cl_rgn, bool _b_defect = true, string _str_Color_Halcon = "#ff0000ff", int pmAlgorithmindex = 0)
        {
            defect_name_ = name;
            result_img_ = image;
            defect_rgn_ = df_rgn;
            set_up_rgn();
            defect_cellcenter_rgn_ = df_cellcenter_rgn;
            cell_rgn_ = cl_rgn;
            this.B_defect = _b_defect;
            str_Color_Halcon = _str_Color_Halcon;
            Algorithmindex = pmAlgorithmindex;
        }

        public Defect(string name, HObject image, HObject df_rgn, HObject df_cellcenter_rgn, HObject cl_rgn)
        {
            defect_name_ = name;
            result_img_ = image;
            defect_rgn_ = df_rgn;
            set_up_rgn();
            cell_rgn_ = cl_rgn;
        }

        /// <summary>
        /// ?
        /// </summary>
        private void set_up_defect_rects()
        {
            defect_rect_list_ = new List<Rectangle>();
            HObject region = null;
            HObject result_connection = null;
            try
            {
                HTuple row1, row2, col1, col2;
                HOperatorSet.Threshold(result_img_, out region, 1, 255);
                HOperatorSet.Connection(region, out result_connection);
                HOperatorSet.SelectShape(result_connection, out result_connection, "area", "and", 150, 9999999); // ?
                HOperatorSet.SmallestRectangle1(result_connection, out row1, out col1, out row2, out col2);

                HTuple height = row2 - row1;
                HTuple width = col2 - col1;
                HTuple num;
                HOperatorSet.CountObj(result_connection, out num);
                for (int i = 0; i < num.I; i++)
                {
                    Rectangle rectangle = new Rectangle(row1[i].I, col1[i].I, width[i].I, height[i].I);
                    defect_rect_list_.Add(rectangle);
                }
            }
            catch { }
            Extension.HObjectMedthods.ReleaseHObject(ref region);
            Extension.HObjectMedthods.ReleaseHObject(ref result_connection);
        }

        public void release()
        {
            Extension.HObjectMedthods.ReleaseHObject(ref result_img_);
            Extension.HObjectMedthods.ReleaseHObject(ref defect_rgn_);
            Extension.HObjectMedthods.ReleaseHObject(ref cell_rgn_);
            Extension.HObjectMedthods.ReleaseHObject(ref defect_cellcenter_rgn_);

            defect_rect_list_ = null;
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// 計算Cell瑕疵顆數
        /// </summary>
        public void set_up_rgn()
        {
            HTuple area, row, col;
            if (defect_rgn_ != null)
                HOperatorSet.AreaCenter(defect_rgn_, out area, out row, out col);
            else // (20190326) Jeff Revised!
            {
                HOperatorSet.GenEmptyObj(out defect_rgn_);
                defect_num_ = 0;
                return;
            }

            if (area.Length > 0 && area.I != 0) // area.I: area[0]之面積
            {
                defect_num_ = Location.Count;
            }
            else
            {
                HOperatorSet.GenEmptyObj(out defect_rgn_);
                defect_num_ = 0;
            }
        }

        /// <summary>
        /// 將一張影像之所有瑕疵區域用白色畫在黑色影像上
        /// </summary>
        public HObject ResultIamage
        {
            get { return this.result_img_; }
            set { this.result_img_ = value; }
        }

        /// <summary>
        /// ?
        /// </summary>
        public List<Rectangle> DefectRectangles
        {
            get { return this.defect_rect_list_; }
        }

        /// <summary>
        /// 一張影像之所有瑕疵區域 (單一種類瑕疵)
        /// </summary>
        public HObject DefectRegion
        {
            get { return defect_rgn_; } 
        }

        public HObject DefectCellCenterRegion
        {
            get { return defect_cellcenter_rgn_; }
        }

        /// <summary>
        ///  一張影像之每個Cell的Region
        /// </summary>
        public HObject CellRegion
        {
            get { return cell_rgn_; }
        }


        /// <summary>
        /// 瑕疵種類名稱
        /// </summary>
        public string DefectName
        {
            get { return defect_name_; }
        }

        /// <summary>
        /// Cell瑕疵顆數
        /// </summary>
        public int DefectNum
        {
            get { return defect_num_; }
        }

        /// <summary>
        /// ?
        /// </summary>
        public List<Point> Location
        {
            get { return this.defect_location_; }
            set { this.defect_location_ = value; }
        }
    }
}
