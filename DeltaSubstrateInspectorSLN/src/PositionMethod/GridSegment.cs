using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Risitanse_AOI.src.PositionMethod.GridSegmentation.GridSeg_Algo;

namespace Risitanse_AOI.src.PositionMethod.GridSegmentation
{
    public partial class GridSegment : Form
    {
        public Bitmap sourceImg;
        public int downScalar = 4;
        List<Point> tl_list = new List<Point>();
        List<Point> br_list = new List<Point>();
        public string position_param = "";
        public GridSegment()
        {
            InitializeComponent();
        }

        private void exeB_Click(object sender, EventArgs e)
        {
            update_img();
        }

        private void update_img()
        {
            Bitmap result = new Bitmap(sourceImg);
            GridSeg_Algo.runGSA(ref result, thH_T.Value, thL_T.Value, erH_T.Value, erL_T.Value, downScalar, ref tl_list, ref br_list);
            showResult.Image = result;
        }

        private void loadImgB_Click(object sender, EventArgs e)
        {
            OpenFileDialog openImg = new OpenFileDialog();
            openImg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openImg.Filter = "BMP File| *.bmp|所有檔案 (*.*)|*.*";
            if (openImg.ShowDialog() == DialogResult.OK)
            {
                sourceImg = new Bitmap(openImg.FileName);
                showResult.Image = sourceImg;
            }
        }

        private void thH_T_Scroll(object sender, EventArgs e)
        {
            thH_V.Text = Convert.ToString(thH_T.Value);
            update_img();
        }

        private void thL_T_Scroll(object sender, EventArgs e)
        {
            thL_V.Text = Convert.ToString(thL_T.Value);
            update_img();
        }

        private void downScalar_V_TextChanged(object sender, EventArgs e)
        {
            if (downScalar_V.Text != "")
            {
                try
                {
                    downScalar = Convert.ToInt32(downScalar_V.Text);
                    if (downScalar <= 0) downScalar = 1;
                }
                catch (FormatException)
                {
                    downScalar_V.Text = "1";
                }
                catch (OverflowException)
                {
                    downScalar_V.Text = "1";
                }
            }
            else downScalar_V.Text = "1";
        }

        private void erH_T_Scroll(object sender, EventArgs e)
        {
            erH_V.Text = Convert.ToString(erH_T.Value);
            update_img();
        }

        private void erL_T_Scroll(object sender, EventArgs e)
        {
            erL_V.Text = Convert.ToString(erL_T.Value);
            update_img();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            position_param = thH_T.Value.ToString() + "," + thL_T.Value.ToString() + "," + erH_T.Value.ToString() +","+ erL_T.Value.ToString() + "," + downScalar.ToString();
            this.Close();
        }

        public string get_param()
        {
            return position_param;
        }

        public Point get_tl()
        {
            return tl_list[1];
        }

        public Point get_br()
        {
            return br_list[1];
        }

        public Bitmap get_image()
        {
            return sourceImg;
        }

        public Rectangle roi_rectangle_
        {
            get { return new Rectangle(tl_list[1].X * downScalar, tl_list[1].Y * downScalar, (br_list[1].X - tl_list[1].X) * downScalar, (br_list[1].Y - tl_list[1].Y) * downScalar); }
        }
    }
}
