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

namespace DeltaSubstrateInspector.src.Modules
{
    public class LaserPositionModel // (20181102) Jeff Revised!
    {
        private HObject laser_locate_used_img_ = null;
        private List<HObject> laser_locate_used_img_new = new List<HObject>();
        private LaserLocateMethod laser_locate_method_ = new LaserLocateMethod();
        private HObject affine_model_ = new HObject();
        private HObject region_ = new HObject();
        private List<Point> all_locate_in_move = new List<Point>();
        private int total_cell_count = 0;
        private bool status_ = false;

        public List<PointF> laser_mark_position_lst_ = new List<PointF>(); // 181102, Andy, 視覺對位座標

        public List<HObject> Now_Mark_ResultImg = new List<HObject>(); // 190213, andy

        public void initialize()
        {
            laser_locate_used_img_new.Clear();
        }

        public void set_locate_method(LaserLocateMethod method)
        {
            laser_locate_method_ = method;
        }

        public void save()
        {
            laser_locate_method_.save();
        }

        public void load()
        {
            if (laser_locate_method_.load())
                status_ = true;
        }

        public void set_type_name(string type_name)
        {
            laser_locate_method_.BoardType = type_name;
        }

    
        public HObject laser_locateUseImgs
        {
            get { return this.laser_locate_used_img_; }
            set { this.laser_locate_used_img_ = value; }
        }
        

        public List<HObject> laser_locateUseImgs_new
        {
            get { return this.laser_locate_used_img_new; }
            set { this.laser_locate_used_img_new = value; }
        }

        public List<PointF> get_laser_locate(bool b_save_MarkImage = false) // (20181119) Jeff Revised!
        {
            laser_locate_method_.find_marks_center(laser_locate_used_img_.Clone(), true, b_save_MarkImage); // (20181224) Jeff Revised!
            return laser_locate_method_.get_total_marks_pos();
        }

        public void get_laser_locates_new(bool b_save_MarkImage = false, bool b_draw_Image = false) // (20190213) Jeff Revised!
        {
            int nowTrgCount = laser_locate_used_img_new.Count;

            laser_mark_position_lst_.Clear(); 
            Now_Mark_ResultImg.Clear(); // 190213, andy
            foreach (HObject img in laser_locate_used_img_new)
            {
                // Calc
                laser_locate_method_.find_marks_center(img.Clone(), true, b_save_MarkImage, b_draw_Image); // (20190213) Jeff Revised!

                // Add point
                laser_mark_position_lst_.AddRange(laser_locate_method_.get_total_marks_pos());

                // 190213, andy: Add Result image
                Now_Mark_ResultImg.Add(laser_locate_method_.Now_ResultImg);                

            }
        }

        public void get_laser_locates_new() //181102, andy
        {
            int nowTrgCount = laser_locate_used_img_new.Count;

            laser_mark_position_lst_.Clear();
            foreach (HObject img in laser_locate_used_img_new)
            {
                // Calc
                laser_locate_method_.find_marks_center(img.Clone()); // (20181224) Jeff Revised!

                // Add
                laser_mark_position_lst_.AddRange (laser_locate_method_.get_total_marks_pos());

            }
            
        }


        public bool Status
        {
            get { return status_; }
        }
    }
}
