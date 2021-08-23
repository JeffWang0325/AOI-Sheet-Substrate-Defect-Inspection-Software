using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Extension;

namespace DeltaSubstrateInspector.src.Modules.ResultModule
{
    public class DefectMap
    {
        private int col_ = 10;
        private int row_ = 10;
        private int height_ = 0;
        private int width_ = 0;
        private PictureBox canvas_ = new PictureBox();
        private List<MapItem> listMapItem = new List<MapItem>();
        public List<MapItem> ListMapItem // (20190729) Jeff Revised!
        {
            get { return listMapItem; }
            set { listMapItem = value; }
        }
        private List<Rectangle> rectangle_collection_ = new List<Rectangle>();
        private PaintEventHandler paint_map;// = new PaintEventHandler(draw_rect);
        ImageShowForm show_form = new ImageShowForm();

        // 190221, andy
        public EventHandler ParameterFormShow_clicked;

        // 190625, andy: remote motor
        public delegate void DelegateRemoteMotor(int SendPositionID);
        public DelegateRemoteMotor delegateRemoteMotor;

        public DefectMap(int col, int row, PictureBox picbox)
        {
            this.col_ = col;
            this.row_ = row;
            this.canvas_ = picbox;
            ListMapItem = new List<MapItem>();
            rectangle_collection_ = new List<Rectangle>();
            set_up_map();
            this.canvas_.MouseClick += new System.Windows.Forms.MouseEventHandler(show_up_image);

            // 190114, andy
            show_form.form_closing += new EventHandler(ShowFromClosing);

            // 190221, andy
            show_form.parameterFormShow_clicked += new EventHandler(ParameterFormShow);

        }

        private  void set_up_map()
        {
            //if (map != null)
            //{
            //    rectangle_collection_ = null;
            //    release();
            //    GC.Collect();
            //    GC.WaitForPendingFinalizers();
            //}

            // Set up and creatre map items
            height_ = (int)canvas_.Height / row_;
            width_ = (int)canvas_.Width / col_ ;

            for ( int i = 0; i < row_; i += 1 )
            {
                for (int j = 0; j < col_; j += 1)
                {
                    MapItem map_item = new MapItem();
                    map_item.Position = z_motion_pos(col_ - 1, j, i, width_, height_, 2);
                    ListMapItem.Add(map_item);
                }
            }
            paint_map = new PaintEventHandler(draw_rect);
            this.canvas_.Paint += paint_map;
            this.Canvas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240-col_)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            
        }

        private Point z_motion_pos(int col, int x, int y, int width, int height, int offset)
        {
            if (y % 2 == 1)
                return new Point((col - x) * width , y * height );
            else
            {
                return new Point(x * width, y * height + offset);
            }
        }

        public void setup_item(ImageObj img_obj, List<Defect> defect_rgns, Image defect_pic, List<Point> defect_pos)
        {
            MapItem item = ListMapItem[img_obj.MoveIndex - 1];
            item.ImgObj = img_obj;
            item.Defect_rgns = defect_rgns;
            item.DefectPic = defect_pic;
            foreach (var img in img_obj.Source)
            {
                item.PicList.Add(img.GetRGBBitmap());
            }

            item.DefectPos = defect_pos;
            item.define_status();
            //this.canvas_.Refresh();

            //this.canvas_.Controls.Clear();
            //this.canvas_.Paint += paint_map;
            if (img_obj.MoveIndex % 2 == 0)
            this.canvas_.BackColor = Color.Gray;
            else
                this.canvas_.BackColor = Color.DarkGray;
        }

        
        public void setup_item(ImageObj img_obj, List<Defect> defect_rgns, Image defect_pic, List<Point> defect_pos, Dictionary<string, List<Defect>> dic_defect)
        {
            MapItem item = ListMapItem[img_obj.MoveIndex - 1];
            item.ImgObj = img_obj;          
            item.Defect_rgns = defect_rgns;
            //item.Dic_Defect = dic_defect; // 181218, andy
            item.Dic_Defect = new Dictionary<string, List<Defect>>(dic_defect);
            item.DefectPic = defect_pic;
            foreach (var img in img_obj.Source)
            {
                item.PicList.Add(img.GetRGBBitmap());
            }
            item.DefectPos = defect_pos;
            item.define_status();

            // ?
            if (img_obj.MoveIndex % 2 == 0)
                this.canvas_.BackColor = Color.Gray;
            else
                this.canvas_.BackColor = Color.DarkGray;
        }

        public void setup_item(ImageObj img_obj, List<Defect> defect_rgns, HObject TotalDefect_region, List<Point> defect_pos, Dictionary<string, List<Defect>> dic_defect) // (20190718) Jeff Revised!
        {
            MapItem item = ListMapItem[img_obj.MoveIndex - 1];
            item.ImgObj = img_obj;
            item.Defect_rgns = defect_rgns;
            //item.Dic_Defect = dic_defect; // 181218, andy
            item.Dic_Defect = new Dictionary<string, List<Defect>>(dic_defect);
            item.TotalDefect_region = TotalDefect_region; // (20190718) Jeff Revised!
            foreach (var img in img_obj.Source)
            {
                item.PicList.Add(img.GetRGBBitmap());
            }
            item.DefectPos = defect_pos;
            item.define_status();

            // ?
            if (img_obj.MoveIndex % 2 == 0)
                this.canvas_.BackColor = Color.Gray;
            else
                this.canvas_.BackColor = Color.DarkGray;
        }

        private void draw_rect(object sender, PaintEventArgs e) // Draw inspect map
        {
            Pen p1;
            p1 = new Pen(Color.DarkGray, 4); // 181026, andy
            //p1 = new Pen(Color.DarkGray, 2);
            if (rectangle_collection_ != null)
                rectangle_collection_.Clear();

            int method = 1;

            #region 歪七扭八線條
             
            if (method==0)
            {
                for (int i = 0; i < ListMapItem.Count; i++)
                {
                    Rectangle rect = new Rectangle(ListMapItem[i].Position.X, ListMapItem[i].Position.Y, width_, height_);
                    SolidBrush blueBrush = new SolidBrush(ListMapItem[i].StatusColor);
                    e.Graphics.DrawRectangle(p1, rect);
                    e.Graphics.FillRectangle(blueBrush, rect);
                    rectangle_collection_.Add(rect);
                    //MessageBox.Show("move index : " + map[i].ImgObj.MoveIndex.ToString());
                }
            }

            #endregion

            #region 修正線條

            if (method == 1)
            {
                if (ListMapItem.Count == 0) return;

                int k = 0;
                for (int i = 0; i < row_; i += 1)
                {
                    for (int j = 0; j < col_; j += 1)
                    {
                        Rectangle rect = new Rectangle(1 + j * width_, 1 + i * height_, width_, height_);
                        SolidBrush blueBrush = new SolidBrush(ListMapItem[cal_pos(k)].StatusColor);
                        e.Graphics.DrawRectangle(p1, rect);
                        e.Graphics.FillRectangle(blueBrush, rect);
                        rectangle_collection_.Add(rect);
                        k++;
                       
                    }
                }
               
            }

            #endregion


        }


        private int nowSelectIndex = -1;

        private void show_up_image(object sender, MouseEventArgs e)
        {
            #region 190114, andy: 清除選擇框 

            this.canvas_.Refresh();

            #endregion



            for (int i = 0; i < rectangle_collection_.Count; i ++)
            {
                Rectangle rect = rectangle_collection_[i];
              
                int index = cal_pos(i); //181026,andy

                bool clickInImage = rect.Contains(e.Location);
                if (clickInImage)
                {

                    // 190625, andy: Remote Motor
                    if (delegateRemoteMotor != null)
                        delegateRemoteMotor(index);


                    if (!ListMapItem[index].Empty)
                    {
                        //using (ImageShowForm show_form = new ImageShowForm(map[index]))                       
                        {
                            //show_form.ShowDialog();

                            #region 繪製選擇框

                            nowSelectIndex = i;
                            ShowSelectRec(rect);

                            #endregion


                            show_form.SetMapToForm(ListMapItem[index]);
                            show_form.Show();

                        }
                        break;
                    }
                    
                }
            } 

        }

        // 181026, andy
        private int cal_pos(int index)
        {
            if ((index / col_) % 2 == 1)
                return ((index / col_) * col_ + (col_ - index % col_)) - 1;
            else
                return index;
        }

        public PictureBox Canvas
        {
            get { return canvas_; }
        }

        public void release()
        {
            
            //rectangle_collection_ = null;
            foreach (var item in ListMapItem)
            {
                item.release();
            }
            ListMapItem = null;
            rectangle_collection_ = null;
            //GC.Collect();
            //GC.WaitForPendingFinalizers();

            ListMapItem = new List<MapItem>();
            rectangle_collection_ = new List<Rectangle>();

            // 181217, andy: 取消
            this.canvas_.MouseClick -= show_up_image;

            // 190114, andy
            show_form.form_closing -= ShowFromClosing;

        }

        private void ShowFromClosing(object sender, EventArgs e)
        {

            #region 190114, andy: 清除選擇框 

            nowSelectIndex = -1;
            this.canvas_.Refresh();

            #endregion

        }

        private void ParameterFormShow(object sender, EventArgs e)
        {
            if (ParameterFormShow_clicked != null)
                ParameterFormShow_clicked(sender, e);
        }

        private void ShowSelectRec(Rectangle _rect)
        {
            Pen p1;
            p1 = new Pen(Color.Blue, 6);
            //System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(Color.Pink);
            System.Drawing.Graphics formGraphics;
            formGraphics = canvas_.CreateGraphics();
            formGraphics.DrawRectangle(p1, _rect);
            //myBrush.Dispose();
            formGraphics.Dispose();
        }

    }
}
