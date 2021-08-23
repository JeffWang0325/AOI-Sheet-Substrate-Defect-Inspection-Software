using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using HalconDotNet;

namespace DSI_Reviewer
{
    public partial class FormReviewer : Form
    {
        private bool CloseIsHide = false;
        int nowTabID = 0;

        private string nowReviewRootDir="";
        private DataTable nowReviewDirList_dt = new DataTable();
        private string nowSelectReviewDirName = "";

        #region 視覺定位變數
        private HObject nowImageList = null;
        private List<HObject> Total_nowImageList = new List<HObject>();      
        List<float> Cal_PosMarkX1List = new List<float>();
        List<float> Cal_PosMarkY1List = new List<float>();
        List<float> Cal_PosMarkX2List = new List<float>();
        List<float> Cal_PosMarkY2List = new List<float>();
        #endregion

        public FormReviewer(bool _CloseIsHide=false)
        {
            InitializeComponent();

            CloseIsHide = _CloseIsHide;
            if (CloseIsHide == true)
            {
                this.Text = "Reviewer";
                this.TopMost = true;
            }

            #region DataGrid init
                // 結果初始化 --> for UI
                //ResultUI_dt.Columns.Add(new DataColumn("X", typeof(string)));
                //ResultUI_dt.Columns.Add(new DataColumn("Y", typeof(string)));
            nowReviewDirList_dt.Columns.Add(new DataColumn("名稱", typeof(string)));

            dataGridView_DirList.DataSource = nowReviewDirList_dt;
            dataGridView_DirList.Columns[0].Width = dataGridView_DirList.Width - 10;
            //dataGridView_UI2Dindex.Columns[0].Width = 55;
            //dataGridView_UI2Dindex.Columns[1].Width = 55;
            dataGridView_DirList.ColumnHeadersDefaultCellStyle.Font = new Font("微軟正黑體", 9);
            dataGridView_DirList.DefaultCellStyle.Font = new Font("微軟正黑體", 9);
            dataGridView_DirList.ReadOnly = true;
            dataGridView_DirList.AllowUserToAddRows = false;
            dataGridView_DirList.AllowUserToDeleteRows = false;
            dataGridView_DirList.AllowUserToResizeColumns = false;
            dataGridView_DirList.AllowUserToResizeRows = false;
            dataGridView_DirList.MultiSelect = false;

            #endregion


            #region ImageShow init

            this.hWindowControl_ImageShow01.MouseWheel += hWindowControl_ImageShow01.HSmartWindowControl_MouseWheel;
            this.hWindowControl_ImageShow02.MouseWheel += hWindowControl_ImageShow02.HSmartWindowControl_MouseWheel;

            this.hWindowControl_LaserImageShow01.MouseWheel += hWindowControl_LaserImageShow01.HSmartWindowControl_MouseWheel;

            #endregion


            #region 視覺定位
            richTextBox_Ｈistory.AppendText("Mark1_X" + "\t" + "Mark1_Y" + "\t" + "Mark2_X" + "\t" + "Mark2_Y" + "\n");
            richTextBox_Calc.AppendText("Mark1_X_Diff" + "\t" + "Mark1_Y_Diff" + "\t" + "Mark2_X_Diff" + "\t" + "Mark2_Y_Diff" + "\n");
            #endregion

        }

        private void button_LoadFileList_Click(object sender, EventArgs e)
        {
                   
            // Get now review root dir
            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK) return;            
            nowReviewRootDir = folderBrowserDialog1.SelectedPath;

          
            // 檢查執行檔目錄是否存在
            if (!System.IO.Directory.Exists(nowReviewRootDir))
            {
                MessageBox.Show("尚未建立: " + nowReviewRootDir);
                textBox_nowReviewRoot.Text = "";
                return;
            }
            textBox_nowReviewRoot.Text = nowReviewRootDir;



            // Get root dir content
            string[] sub_dirs = System.IO.Directory.GetDirectories(nowReviewRootDir);

            // Reset 
            button_CalReset_Click(sender, e);

            // Show           
            nowReviewDirList_dt.Clear();
            Total_nowImageList.Clear();            
            for (int i = 0; i < sub_dirs.Count(); i++)
            {               
                nowReviewDirList_dt.Rows.Add(sub_dirs[i]);

                HObject nowImageList;
                try
                {
                    nowImageList = GetImageList(sub_dirs[i], nowTabID);
                }
                catch
                {
                    HOperatorSet.GenEmptyObj(out nowImageList);
                }

                Total_nowImageList.Add(nowImageList.Clone());

                // release 
                Application.DoEvents();

            }


        }

        private void dataGridView_DirList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView_DirList_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

            if (dataGridView_DirList.CurrentCell.Value == null || Total_nowImageList.Count() == 0) return;


            int nowID = dataGridView_DirList.CurrentCell.RowIndex;
            nowImageList = Total_nowImageList[nowID];

            nowSelectReviewDirName = dataGridView_DirList.CurrentRow.Cells[0].Value.ToString();
            textBox_nowSelectDirName.Text = nowSelectReviewDirName;

            switch (nowTabID)
            {
                case 0:
                    try
                    {
                        // 更新影像
                        HObject Image01, Image02;
                        Image01 = new HObject();
                        Image02 = new HObject();
                        HOperatorSet.SelectObj(nowImageList, out Image01, 1);
                        HOperatorSet.SelectObj(nowImageList, out Image02, 2);

                        // Display
                        DisplayVisionPositionMarkImage(Image01, Image02);

                    }
                    catch
                    {
                        //MessageBox.Show("File error");

                        // 清除所有 Mark info
                        richTextBox_Info01.Clear();
                        richTextBox_Info02.Clear();

                        richTextBox_Info01.ForeColor = Color.Red;
                        richTextBox_Info02.ForeColor = Color.Red;
                        richTextBox_Info01.AppendText("File error");
                        richTextBox_Info02.AppendText("File error");
                        

                    }

                break;

                case 1:
                    try
                    {
                        // 更新影像
                        HObject Image;//, Image02;
                        Image = new HObject();
                        //Image02 = new HObject();
                        HOperatorSet.SelectObj(nowImageList, out Image, 1);
                        //HOperatorSet.SelectObj(nowImageList, out Image02, 2);

                        // Display
                        DisplayLaserVisionPositionMarkImage(Image);
                    }
                    catch
                    {
                        // 清除所有 Mark info
                        richTextBox_InfoLaser.Clear();

                        richTextBox_InfoLaser.ForeColor = Color.Red;                       
                        richTextBox_InfoLaser.AppendText("File error");

                    }


                break;


             }

        }

        public bool DisplayVisionPositionMarkImage(HObject ImageMark1, HObject ImageMark2, bool InlineGo=false)
        {
            bool status = false;

            // 清除所有Region
            HOperatorSet.ClearWindow(hWindowControl_ImageShow01.HalconWindow);
            HOperatorSet.ClearWindow(hWindowControl_ImageShow02.HalconWindow);


            // 顯示
            if(ImageMark1!=null &&  ImageMark2 != null)
            {
                HOperatorSet.DispObj(ImageMark1, hWindowControl_ImageShow01.HalconWindow);
                HOperatorSet.DispObj(ImageMark2, hWindowControl_ImageShow02.HalconWindow);

                // Add to Image buffer
                if (InlineGo == true)
                {                   
                    HObject nowImageList;
                    HOperatorSet.GenEmptyObj(out nowImageList);
                    HOperatorSet.ConcatObj(nowImageList, ImageMark1, out nowImageList);
                    HOperatorSet.ConcatObj(nowImageList, ImageMark2, out nowImageList);
                    Total_nowImageList.Add(nowImageList.Clone());                   
                    nowReviewDirList_dt.Rows.Add(String.Format("{0:00000000}", Total_nowImageList.Count));
                }


                status = true;
            }
            else
            {
                status = false;
            }


            // return
            return status;

        }
      
        public bool DisplayVisionPositionMarkPos(List<PointF> PosMark)
        {
            bool status = false;

            // 清除所有 Mark info
            richTextBox_Info01.Clear();
            richTextBox_Info02.Clear();

            try
            {
                // 更新
                if (PosMark.Count > 0)
                {
                    // 各自更新
                    string MARKPOS_X1 = PosMark[0].X.ToString(); // String.Format("{0:00000.000}", PosMark[0].X);
                    string MARKPOS_Y1 = PosMark[0].Y.ToString(); // String.Format("{0:00000.000}", PosMark[0].Y);
                    string MARKPOS_X2 = PosMark[1].X.ToString(); // String.Format("{0:00000.000}", PosMark[1].X);
                    string MARKPOS_Y2 = PosMark[1].Y.ToString(); // String.Format("{0:00000.000}", PosMark[1].Y);
                    string CbMarkPosiStr1 = "(X: " + MARKPOS_X1 + " , Y: " + MARKPOS_Y1 + ")";
                    string CbMarkPosiStr2 = "(X: " + MARKPOS_X2 + " , Y: " + MARKPOS_Y2 + ")";
                    richTextBox_Info01.ForeColor = Color.Black;
                    richTextBox_Info02.ForeColor = Color.Black;
                    richTextBox_Info01.AppendText(CbMarkPosiStr1 + "\n");
                    richTextBox_Info02.AppendText(CbMarkPosiStr2 + "\n");

                    // 歷史紀錄
                    richTextBox_Ｈistory.AppendText(PosMark[0].X.ToString() + "\t" + PosMark[0].Y.ToString() + "\t" + PosMark[1].X.ToString() + "\t" + PosMark[1].Y.ToString() + "\n");
                    richTextBox_Ｈistory.ScrollToCaret();

                    // save to buffer for cal and update
                    Cal_PosMarkX1List.Add(PosMark[0].X);
                    Cal_PosMarkY1List.Add(PosMark[0].Y);
                    Cal_PosMarkX2List.Add(PosMark[1].X);
                    Cal_PosMarkY2List.Add(PosMark[1].Y);
                    float Mark1_X_Diff = Cal_PosMarkX1List.Max() - Cal_PosMarkX1List.Min();
                    float Mark1_Y_Diff = Cal_PosMarkY1List.Max() - Cal_PosMarkY1List.Min();
                    float Mark2_X_Diff = Cal_PosMarkX2List.Max() - Cal_PosMarkX2List.Min();
                    float Mark2_Y_Diff = Cal_PosMarkY2List.Max() - Cal_PosMarkY2List.Min();
                    richTextBox_Calc.AppendText(Mark1_X_Diff.ToString() + "\t" + Mark1_Y_Diff.ToString() + "\t" + Mark2_X_Diff.ToString() + "\t" + Mark2_Y_Diff.ToString() + "\n");
                    richTextBox_Calc.ScrollToCaret();

                }
                else
                {
                    status = false;
                }
            }
            catch(Exception ex)
            {
                ex.Message.ToString();

                status = false;
            }

            // return
            return status;

        }

        public bool DisplayLaserVisionPositionMarkImage(HObject LaserImageMark, bool InlineGo = false)
        {
            bool status = false;

            // 清除所有Region
            HOperatorSet.ClearWindow(hWindowControl_LaserImageShow01.HalconWindow);
            //HOperatorSet.ClearWindow(hWindowControl_ImageShow02.HalconWindow);


            // 顯示
            if (LaserImageMark != null )
            {
                HOperatorSet.DispObj(LaserImageMark, hWindowControl_LaserImageShow01.HalconWindow);
                //HOperatorSet.DispObj(ImageMark2, hWindowControl_ImageShow02.HalconWindow);

                // Add to Image buffer
                if (InlineGo == true)
                {
                    HObject nowImageList;
                    HOperatorSet.GenEmptyObj(out nowImageList);
                    HOperatorSet.ConcatObj(nowImageList, LaserImageMark, out nowImageList);
                    //HOperatorSet.ConcatObj(nowImageList, ImageMark2, out nowImageList);
                    Total_nowImageList.Add(nowImageList.Clone());
                    nowReviewDirList_dt.Rows.Add(String.Format("{0:00000000}", Total_nowImageList.Count));
                }


                status = true;
            }
            else
            {
                status = false;
            }


            // return
            return status;

        }


        private HObject GetImageList(string _nowSelectReviewDirName, int select=0)
        {


            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ImageList, ho_Image = null;

            // Local control variables 

            HTuple hv_Files = null, hv_Index = null, hv_nowFile = new HTuple(), hv_nowID = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageList);
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.ListFiles(new HTuple(_nowSelectReviewDirName), "files", out hv_Files);
            ho_ImageList.Dispose();
            HOperatorSet.GenEmptyObj(out ho_ImageList);
            for (hv_Index = 1; (int)hv_Index <= (int)(new HTuple(hv_Files.TupleLength())); hv_Index = (int)hv_Index + 1)
            {
                /// Only read result image without srcImage
                hv_nowID = hv_Index - 1;

                switch (select)
                {
                    case 0: // 視覺定位
                        if ((int)((new HTuple(hv_nowID.TupleEqual(0))).TupleOr(new HTuple(hv_nowID.TupleEqual(2)))) != 0)
                        {
                            continue;
                        }
                    break;

                    case 1: // 雷射大小校正
                        if ((int)((new HTuple(hv_nowID.TupleEqual(0)))) != 0)
                        {
                            continue;
                        }
                    break;


                }


                hv_nowFile = hv_Files.TupleSelect(hv_nowID);
                ho_Image.Dispose();
                HOperatorSet.ReadImage(out ho_Image, hv_nowFile);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_ImageList, ho_Image, out ExpTmpOutVar_0);
                    ho_ImageList.Dispose();
                    ho_ImageList = ExpTmpOutVar_0;
                }

            }


            // ho_ImageList.Dispose();
            //ho_Image.Dispose();

            return ho_ImageList;

        }

        private void Form_Main_Load(object sender, EventArgs e)
        {

        }

        private void FormReviewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(CloseIsHide==true)
            {
                e.Cancel = true;
                Hide();
            }

        }

        private void button_CalReset_Click(object sender, EventArgs e)
        {
            Cal_PosMarkX1List.Clear();
            Cal_PosMarkY1List.Clear();
            Cal_PosMarkX2List.Clear();
            Cal_PosMarkY2List.Clear();
            richTextBox_Ｈistory.Clear();
            richTextBox_Calc.Clear();

            richTextBox_Ｈistory.AppendText("Mark1_X" + "\t" + "Mark1_Y" + "\t" + "Mark2_X" + "\t" + "Mark2_Y" + "\n");
            richTextBox_Calc.AppendText("Mark1_X_Diff" + "\t" + "Mark1_Y_Diff" + "\t" + "Mark2_X_Diff" + "\t" + "Mark2_Y_Diff" + "\n");

            nowReviewDirList_dt.Clear();
            Total_nowImageList.Clear();

        }

        private void tabControl_Review_SelectedIndexChanged(object sender, EventArgs e)
        {
            nowTabID = tabControl_Review.SelectedIndex;
        }
    }
}
