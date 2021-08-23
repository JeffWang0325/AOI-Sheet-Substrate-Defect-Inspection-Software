using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extension;
using System.Drawing;

namespace DeltaSubstrateInspector.src.Modules.ResultModule
{
    /// <summary>
    /// 瑕疵地圖各單一位置相關資訊
    /// </summary>
    public class MapItem
    {
        private int status_ { get; set; } = 0;
        private ImageObj img_obj { get; set; } = new ImageObj();
        /// <summary>
        /// 該位置總瑕疵
        /// </summary>
        private List<Defect> defect_rgns_ = new List<Defect>();
        private Point position_;
        private bool is_empty = true;

        /// <summary>
        /// 將瑕疵繪於影像上
        /// </summary>
        private Image defect_pic_ = null;
        /// <summary>
        /// 總瑕疵region
        /// </summary>
        private HObject totalDefect_region; // (20190718) Jeff Revised!

        private List<Image> pic_list_ = new List<Image>();
        private List<Point> defect_pos = new List<Point>();
        /// <summary>
        /// 不同 status_ 顯示之顏色 { 瑕疵地圖各位置未檢測時, , 該位置有NG, 該位置為OK }
        /// </summary>
        private Color[] color_set_ { get; set; } = { Color.FromArgb(243, 244, 248), Color.FromArgb(254, 192, 85), Color.FromArgb(232, 76, 61), Color.FromArgb(27, 188, 155) };

        // 181218, andy
        private Dictionary<string, List<Defect>> dic_defect_ = new Dictionary<string, List<Defect>>();

        /// <summary>
        /// 定義小圖顯示顏色
        /// </summary>
        public void define_status()
        {
            int count = 0;

            foreach (var item in defect_rgns_)
            {
                count = count + item.DefectNum;
            }

            set_map_item_bg(count);
        }

        private void set_map_item_bg(int count)
        {
            if (count > 0)
                status_ = 2;
            else
                status_ = 3;
        }

        public ImageObj ImgObj
        {
            get { return this.img_obj; }
            set
            {
                this.img_obj = value;
                is_empty = false;
            }
        }
        
        /// <summary>
        /// 該位置總瑕疵
        /// </summary>
        public List<Defect> Defect_rgns
        {
            set { this.defect_rgns_ = value; }
        }

        public Dictionary<string, List<Defect>> Dic_Defect
        {
            get { return this.dic_defect_; }
            set { this.dic_defect_ = value; }
        }

        public Point Position
        {
            get { return this.position_; }
            set { this.position_ = value; }
        }
         
        public int Status
        {
            set { this.status_ = value; }
        }

        public Color StatusColor
        {
            get { return color_set_[status_]; }
        }

        public bool Empty
        {
            get { return is_empty; }
        }

        /// <summary>
        /// 將瑕疵繪於影像上
        /// </summary>
        public Image DefectPic
        {
            get { return this.defect_pic_; }
            set { this.defect_pic_ = value;  }
        }

        /// <summary>
        /// 總瑕疵region
        /// </summary>
        public HObject TotalDefect_region // (20190718) Jeff Revised!
        {
            get { return this.totalDefect_region; }
            set { this.totalDefect_region = value; }
        }

        public List<Point> DefectPos
        {
            get { return this.defect_pos; }
            set { this.defect_pos = value; }
        }

        public void release()
        {
            foreach (var item in defect_rgns_)
            {
                item.release();
            }
            defect_rgns_ = null;
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// 該位置所有影像
        /// </summary>
        public List<Image> PicList
        {
            get { return pic_list_; }
            set { this.pic_list_ = value; }
        }
    }
}
