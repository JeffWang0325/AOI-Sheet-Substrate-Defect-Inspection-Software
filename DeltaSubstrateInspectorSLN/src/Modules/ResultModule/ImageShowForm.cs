using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Extension;

using HalconDotNet;
using DeltaSubstrateInspector.src.Modules.InspectModule; // For 華新trim痕檢測 (20180905) Jeff Revised!
using System.Reflection; // picturebox圖像縮放 (20180912) Jeff Revised!
using DeltaSubstrateInspector.src.Modules.MotionModule;
using DeltaSubstrateInspector.UIControl;

using DeltaSubstrateInspector.src.Modules.StorageModule;
using static DeltaSubstrateInspector.FileSystem.FileSystem;
using System.Media; // For SystemSounds
using DeltaSubstrateInspector.src.PositionMethod.LocateMethod.Location;
using DeltaSubstrateInspector.FileSystem;

namespace DeltaSubstrateInspector.src.Modules.ResultModule
{
    public partial class ImageShowForm : Form
    {
        private Image defect_img;
        private HObject TotalDefect_region; // (20190718) Jeff Revised!
        private List<Image> img_lst_ = new List<Image>();
        private int light_img_index_ = 0; // 該位置第幾張影像

        private Point mouseDownPoint; // 記錄拖拽過程中鼠標位置 (20180912) Jeff Revised!
        private bool isMove = false; // picturebox圖像拖曳 (20180912) Jeff Revised!

        DataTable ResultUI_dt = new DataTable();

        public EventHandler form_closing;

        // 181218, andy
        //MapItem NowMapItem;
        // 190221, andy
        public static MapItem NowMapItem;

        public EventHandler parameterFormShow_clicked;
        
        public ImageShowForm()
        {
            InitializeComponent();

            this.hSmartWindowControl_ImageShowForm.MouseWheel += hSmartWindowControl_ImageShowForm.HSmartWindowControl_MouseWheel; // (20190718) Jeff Revised!
            this.hWindowControl_InspectResultImage.MouseWheel += hWindowControl_InspectResultImage.HSmartWindowControl_MouseWheel;
            this.hSmartWindowControl_Recheck.MouseWheel += hSmartWindowControl_Recheck.HSmartWindowControl_MouseWheel; // (20190624) Jeff Revised!
            this.TopMost = true;

            Change_dispWindow(); // (20190718) Jeff Revised!

            // 取得初始 pictureBox1 資訊 (20190131) Jeff Revised!
            Initial_pictureBox1Location = pictureBox_ImageShowForm.Location;
            Initial_pictureBox1Width = pictureBox_ImageShowForm.Width;
            Initial_pictureBox1Height = pictureBox_ImageShowForm.Height;

            #region 【人工覆判與統計結果】頁面

            if (Locate_Method_FS.b_Defect_Recheck) // 啟用人工覆判
            {
                radioButton_Recheck.Enabled = true;
                cbx_Recheck.Checked = true;
                groupBox_Recheck.Enabled = true;
            }

            if (Locate_Method_FS.b_Defect_Classify) // 瑕疵分類是否啟用
            {
                cbx_NG_classification.Checked = true;
                groupBox_DefectsClassify.Enabled = true;
            }

            cbx_Priority.Checked = Locate_Method_FS.b_Defect_priority; // 瑕疵優先權是否啟用

            #endregion

        }
        
        /// <summary>
        /// 將小圖資訊設定到此Form (使用者點擊不同小圖位置時，皆會執行一次!)
        /// </summary>
        /// <param name="map_item"></param>
        public void SetMapToForm(MapItem map_item) // (20190718) Jeff Revised!
        {
            img_lst_ = map_item.PicList;
            defect_img = map_item.DefectPic;
            TotalDefect_region = map_item.TotalDefect_region; // (20190718) Jeff Revised!
            NowMapItem = map_item;

            if (FinalInspectParam.dispWindow == "pictureBox") // (20190718) Jeff Revised!
                pictureBox_ImageShowForm.Image = map_item.DefectPic;
            else
                Display();

            // Andy
            int nowLightCount = img_lst_.Count;
            comboBox_LightImage.Items.Clear();
            for (int i = 0; i < nowLightCount; i++)
            {
                string lightName = "Light Image " + (i + 1).ToString();
                comboBox_LightImage.Items.Add(lightName);
            }

            if (light_img_index_ > nowLightCount - 1)
                light_img_index_ = 0;

            comboBox_LightImage.SelectedIndex = light_img_index_;

            // 181217, andy: 最花費時間的部分           
            //update_info2(map_item.DefectPos);  // --> 更新速度較快
            
            // 181218, andy
            //NowMapItem = map_item;
            CreateInspectResultUI();
            UpdateInspectResult();

            #region 【人工覆判與統計結果】頁面
            
            Create_Recheck_UI();

            #endregion

        }



        #region 【一般】頁面

        /// <summary>
        /// 改變圖像顯示視窗類型
        /// </summary>
        public void Change_dispWindow() // (20190718) Jeff Revised!
        {
            if (FinalInspectParam.dispWindow == "pictureBox")
            {
                pictureBox_ImageShowForm.Visible = true;
                hSmartWindowControl_ImageShowForm.Visible = false;
            }
            else
            {
                pictureBox_ImageShowForm.Image = null;
                pictureBox_ImageShowForm.Visible = false;
                hSmartWindowControl_ImageShowForm.Visible = true;
                comboBox_LightImage.Visible = true;
            }
        }

        private Point Initial_pictureBox1Location = new Point(9, 8); // (20190131) Jeff Revised!
        private int Initial_pictureBox1Width = 1197, Initial_pictureBox1Height = 755; // (20190131) Jeff Revised!
        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e) // (20180912) Jeff Revised!
        {
            int zoomStep = 300; // 定義滾輪滑動縮放大小
            int x = e.Location.X;
            int y = e.Location.Y;
            int ow = pictureBox_ImageShowForm.Width;
            int oh = pictureBox_ImageShowForm.Height;
            int VX, VY; // 因縮放產生的位移矢量
            if (e.Delta < 0) // 放大
            {
                // 第①步
                pictureBox_ImageShowForm.Width += zoomStep;
                pictureBox_ImageShowForm.Height += zoomStep;
                // 第②步
                PropertyInfo pInfo = pictureBox_ImageShowForm.GetType().GetProperty("ImageRectangle", BindingFlags.Instance |
                    BindingFlags.NonPublic);
                Rectangle rect = (Rectangle)pInfo.GetValue(pictureBox_ImageShowForm, null);
                // 第③步
                pictureBox_ImageShowForm.Width = rect.Width;
                pictureBox_ImageShowForm.Height = rect.Height;
            }
            if (e.Delta > 0) // 缩小
            {
                // 防止一直縮成負值
                if (pictureBox_ImageShowForm.Width < 300 || pictureBox_ImageShowForm.Height < 300) // (20190131) Jeff Revised!
                    return;

                pictureBox_ImageShowForm.Width -= zoomStep;
                pictureBox_ImageShowForm.Height -= zoomStep;
                PropertyInfo pInfo = pictureBox_ImageShowForm.GetType().GetProperty("ImageRectangle", BindingFlags.Instance | BindingFlags.NonPublic);
                Rectangle rect = (Rectangle)pInfo.GetValue(pictureBox_ImageShowForm, null);
                pictureBox_ImageShowForm.Width = rect.Width;
                pictureBox_ImageShowForm.Height = rect.Height;
            }
            // 第④步，求因縮放產生的位移，進行補償，實現錨點縮放的效果
            VX = (int)((double)x * (ow - pictureBox_ImageShowForm.Width) / ow);
            VY = (int)((double)y * (oh - pictureBox_ImageShowForm.Height) / oh);
            pictureBox_ImageShowForm.Location = new Point(pictureBox_ImageShowForm.Location.X + VX, pictureBox_ImageShowForm.Location.Y + VY);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e) // (20180912) Jeff Revised!
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDownPoint.X = Cursor.Position.X; // 記錄鼠標左鍵按下時位置
                mouseDownPoint.Y = Cursor.Position.Y;
                isMove = true;
                pictureBox_ImageShowForm.Focus(); // 鼠標滾輪事件(縮放時)需要picturebox有焦點
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e) // (20180912) Jeff Revised!
        {
            if (e.Button == MouseButtons.Left)
            {
                isMove = false;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox_ImageShowForm.Focus(); // 鼠標在picturebox上時才有焦點，此時可以縮放
            if (isMove)
            {
                int x, y; // 新的pictureBox1.Location(x,y)
                int moveX, moveY; // X方向，Y方向移動大小。
                moveX = Cursor.Position.X - mouseDownPoint.X;
                moveY = Cursor.Position.Y - mouseDownPoint.Y;
                x = pictureBox_ImageShowForm.Location.X + moveX;
                y = pictureBox_ImageShowForm.Location.Y + moveY;
                pictureBox_ImageShowForm.Location = new Point(x, y);
                mouseDownPoint.X = Cursor.Position.X;
                mouseDownPoint.Y = Cursor.Position.Y;
            }
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e) // (20190131) Jeff Revised!
        {
            pictureBox_ImageShowForm.Location = Initial_pictureBox1Location;
            pictureBox_ImageShowForm.Width = Initial_pictureBox1Width;
            pictureBox_ImageShowForm.Height = Initial_pictureBox1Height;
        }

        /// <summary>
        /// 影像切換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_LightImage_SelectedIndexChanged(object sender, EventArgs e) // (20190718) Jeff Revised!
        {
            light_img_index_ = comboBox_LightImage.SelectedIndex;

            if (FinalInspectParam.dispWindow == "pictureBox")
            {
                if (rbtn_origin.Checked) // 【原圖】
                {
                    try
                    {
                        pictureBox_ImageShowForm.Image = img_lst_[light_img_index_];
                    }
                    catch // (20181009) Jeff Revised!
                    {
                        pictureBox_ImageShowForm.Image = null;
                        MessageBox.Show("影像不存在", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                    pictureBox_ImageShowForm.Image = defect_img;
            }
            else // (20190718) Jeff Revised!
            {
                Display(false);
            }

        }

        /// <summary>
        /// 【瑕疵圖】&【原圖】 切換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Change_image(object sender, EventArgs e) // (20190718) Jeff Revised!
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return; // (20180906) Jeff Revised!

            if (FinalInspectParam.dispWindow == "pictureBox")
            {
                if (rbtn == rbtn_origin) // 【原圖】
                {
                    comboBox_LightImage.Visible = true;

                    try
                    {
                        pictureBox_ImageShowForm.Image = img_lst_[light_img_index_];
                    }
                    catch // (20181009) Jeff Revised!
                    {
                        pictureBox_ImageShowForm.Image = null;
                        MessageBox.Show("影像不存在", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    comboBox_LightImage.Visible = false;

                    pictureBox_ImageShowForm.Image = defect_img;
                }
            }
            else // (20190718) Jeff Revised!
            {
                Display(false);
            }

        }

        /// <summary>
        /// 更新顯示 (【一般】頁面)
        /// </summary>
        /// <param name="b_SetPart">是否做 SetPart</param>
        void Display(bool b_SetPart = true) // (20190718) Jeff Revised!
        {
            HOperatorSet.DispObj(NowMapItem.ImgObj.Source[light_img_index_], hSmartWindowControl_ImageShowForm.HalconWindow);
            if (rbtn_defect.Checked) // 【瑕疵圖】
            {
                HOperatorSet.SetColor(hSmartWindowControl_ImageShowForm.HalconWindow, "red");
                HOperatorSet.SetDraw(hSmartWindowControl_ImageShowForm.HalconWindow, "fill");
                HOperatorSet.DispObj(TotalDefect_region, hSmartWindowControl_ImageShowForm.HalconWindow);
            }
            if (b_SetPart)
                HOperatorSet.SetPart(hSmartWindowControl_ImageShowForm.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
        }

        private void button1_Click(object sender, EventArgs e)
        {
            light_img_index_ ^= 1; //   XOR operator
            //pictureBox1.Image = img_lst_[light_img_index_];
            // (20181119) Jeff Revised!
            if (rbtn_origin.Checked)
            {
                try
                {
                    pictureBox_ImageShowForm.Image = img_lst_[light_img_index_];
                }
                catch // (20181009) Jeff Revised!
                {
                    pictureBox_ImageShowForm.Image = null;
                    MessageBox.Show("影像不存在", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
                pictureBox_ImageShowForm.Image = defect_img;
        }

        #endregion



        #region 【進階】頁面

        List<Defect> defectList = new List<Defect>();
        List<InspectResultUI> inspectRUIList = new List<InspectResultUI>();
        private bool b_initial = false; // (20190131) Jeff Revised!
        private void CreateInspectResultUI()
        {
            defectList.Clear();

            // Get nowDefect list
            foreach (string key in NowMapItem.Dic_Defect.Keys)
            {
                if (NowMapItem.Dic_Defect[key].Count > 0)
                {
                    foreach (var _defectList in NowMapItem.Dic_Defect[key])
                    {
                        defectList.Add(_defectList);
                    }
                }
            }

            // Show 
            if (panel_InspectResult.Controls.Count == 0)  // 190304, andy
            {
                for (int i = 0; i < defectList.Count; i++)
                {
                    // Set up model item's attribute
                    InspectResultUI inspectRUI = new InspectResultUI();
                    inspectRUI.Location = new System.Drawing.Point(2, 2 + (inspectRUI.Height + 2) * i);
                    inspectRUI.SetColor(defectList[i].Str_Color_Halcon); // 設定預先儲存之顏色 (20190624) Jeff Revised!
                    inspectRUI.this_SelectedIndexChanged_event += new System.EventHandler(this.inspectResult_item_SelectedIndexChanged);
                    inspectRUI.this_checkBox_DrawType_CheckedChanged_event += new System.EventHandler(this.inspectResult_item_checkBox_DrawType_CheckedChanged);
                    inspectRUI.Tag = i;
                    inspectRUI.Set_item_name(defectList[i].DefectName);
                    inspectRUI.Set_button_isDefect(defectList[i].B_defect); // (20190624) Jeff Revised!

                    // Add model item into panel: @pnl_insp_model
                    panel_InspectResult.Controls.Add(inspectRUI);
                    inspectRUIList.Add(inspectRUI);
                }
            }

            // 清除所有Region
            HOperatorSet.ClearWindow(hWindowControl_InspectResultImage.HalconWindow);

            // 更新影像
            if (defectList.Count > 0)
            {
                HOperatorSet.DispObj(defectList[0].ResultIamage, hWindowControl_InspectResultImage.HalconWindow);

                if (!b_initial) // (20190131) Jeff Revised!
                {
                    b_initial = true;
                    HOperatorSet.SetPart(hWindowControl_InspectResultImage.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
                }
            }

        }

        /// <summary>
        /// 【顏色設定】 或 顯示物件 變更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inspectResult_item_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateInspectResult();

            // 更新【人工覆判與統計結果】頁面之顏色設定 (20190715) Jeff Revised!
            if (sender is Label)
            {
                Label lb = sender as Label;
                if (Locate_Method_FS.DefectsClassify.ContainsKey(lb.Text))
                {
                    int i_defectList = Dictionary_defectList[lb.Text];
                    Locate_Method_FS.DefectsClassify[lb.Text].SetColor(inspectRUIList[i_defectList].GetColor_ColorType());

                    // 更新 listView_Edit
                    Locate_Method_FS.Update_listView_Edit(this.listView_Edit, this.radioButton_Result, this.radioButton_Recheck);
                }
            }
            comboBox_DefectType_SelectedIndexChanged(null, null);
        }

        /// <summary>
        /// 【填滿】與【輪廓】 切換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inspectResult_item_checkBox_DrawType_CheckedChanged(object sender, EventArgs e)
        {
            UpdateInspectResult();
        }

        private HObject nowImage = null;
        /// <summary>
        /// 更新顯示 (【進階】頁面)
        /// </summary>
        private void UpdateInspectResult() // (20190815) Jeff Revised!
        {
            if (defectList.Count == 0) return;

            // 清除所有Region
            HOperatorSet.ClearWindow(hWindowControl_InspectResultImage.HalconWindow);

            // 更新影像  
            bool Go0 = false;
            nowImage = null;
            for (int i = 0; i < defectList.Count; i++) // 顯示ID數小的
            {
                InspectResultUI item = inspectRUIList[i];
                Go0 = item.GetCheckListBox().GetItemChecked(0); // Image
                nowImage = null;
                if (Go0 == true)
                {
                    nowImage = defectList[i].ResultIamage;
                    HOperatorSet.DispObj(nowImage, hWindowControl_InspectResultImage.HalconWindow);
                    break;
                }                             
            }
            if (Go0 == false) // 全都沒選，則顯示第一張影像
            {
                nowImage = defectList[0].ResultIamage;
                HOperatorSet.DispObj(nowImage, hWindowControl_InspectResultImage.HalconWindow);
            }

            // 更新Region            
            for (int i = 0; i < defectList.Count; i++)
            {
                if (defectList[i].DefectCellCenterRegion == null)
                    continue;

                InspectResultUI item = inspectRUIList[i];
                bool Go1 = item.GetCheckListBox().GetItemChecked(1); // Defect Region
                bool Go2 = item.GetCheckListBox().GetItemChecked(2); // Defect Cell Center
                bool b_DrawDefectFrame = item.Get_DrawDefectFrame(); // 瑕疵外框

                // 判斷
                HObject nowDefectRgn, nowDefectCellCenterRgn, nowDefectFrame;

                if (Go1 == true)
                    HOperatorSet.Connection(defectList[i].DefectRegion, out nowDefectRgn);
                else
                    HOperatorSet.GenEmptyObj(out nowDefectRgn);

                if (Go2 == true)
                    HOperatorSet.Connection(defectList[i].DefectCellCenterRegion, out nowDefectCellCenterRgn);
                else
                    HOperatorSet.GenEmptyObj(out nowDefectCellCenterRgn);
                
                if (b_DrawDefectFrame)
                {
                    // 計算各瑕疵最小外接矩形
                    if (!Go1)
                    {
                        HObject temp;
                        HOperatorSet.Connection(defectList[i].DefectRegion, out temp);
                        HOperatorSet.ShapeTrans(temp, out nowDefectFrame, "rectangle2");
                        temp.Dispose();
                    }
                    else
                        HOperatorSet.ShapeTrans(nowDefectRgn, out nowDefectFrame, "rectangle2");

                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.DilationCircle(nowDefectFrame, out ExpTmpOutVar_0, item.Get_DrawDefectFrame_Length());
                        nowDefectFrame.Dispose();
                        nowDefectFrame = ExpTmpOutVar_0;
                    }
                }
                else
                    HOperatorSet.GenEmptyObj(out nowDefectFrame);

                // 設定顏色與繪圖
                string nowColorStr = item.GetColor();
                string nowDrawType = (item.GetDrawType() == true) ? "fill" : "margin";
                HOperatorSet.SetColor(hWindowControl_InspectResultImage.HalconWindow, nowColorStr);
                HOperatorSet.SetDraw(hWindowControl_InspectResultImage.HalconWindow, nowDrawType);

                HOperatorSet.DispObj(nowDefectRgn, hWindowControl_InspectResultImage.HalconWindow);
                HOperatorSet.DispObj(nowDefectCellCenterRgn, hWindowControl_InspectResultImage.HalconWindow);

                HOperatorSet.SetDraw(hWindowControl_InspectResultImage.HalconWindow, "margin");
                HOperatorSet.SetLineWidth(hWindowControl_InspectResultImage.HalconWindow, 3);
                HOperatorSet.DispObj(nowDefectFrame, hWindowControl_InspectResultImage.HalconWindow);
                HOperatorSet.SetLineWidth(hWindowControl_InspectResultImage.HalconWindow, 1);

                nowDefectRgn.Dispose();
                nowDefectCellCenterRgn.Dispose();
                nowDefectFrame.Dispose();

                // 顯示檢測資訊
                HTuple hv_Number = null;
                HOperatorSet.CountObj(defectList[i].DefectCellCenterRegion, out hv_Number);
                item.SetLog("瑕疵Cell顆數: " + hv_Number.I.ToString());
                
            }
        }

        private void hWindowControl_InspectResultImage_HMouseMove(object sender, HMouseEventArgs e)
        {
            if (nowImage == null) return;
            HTuple width = null, height = null;
            HOperatorSet.GetImageSize(nowImage, out width, out height);

            txt_CursorCoordinate.Text = "(" + String.Format("{0:#}", e.X) + ", " + String.Format("{0:#}", e.Y) + ")";
            try
            {
                if (e.X >= 0 && e.X < width && e.Y >= 0 && e.Y < height)
                {
                    HTuple grayval;
                    //HOperatorSet.GetGrayval(nowImage, e.Y, e.X, out grayval);
                    //txt_RGBValue.Text = (grayval.TupleInt()).ToString();
                    HOperatorSet.GetGrayval(nowImage, e.Y, e.X, out grayval);
                    txt_GrayValue.Text = (grayval.TupleInt()).ToString();
                }
                else
                {
                    //txt_RGBValue.Text = "";
                    txt_GrayValue.Text = "";
                }
            }
            catch
            {
                txt_GrayValue.Text = "";
            }
        }

        #endregion



        #region 【人工覆判與統計結果】頁面

        private int index_capture = 1;
        /// <summary>
        /// 此位置之所有影像
        /// </summary>
        private List<HObject> img_lst_Recheck = new List<HObject>();
        /// <summary>
        /// 某一取像位置之原始影像
        /// </summary>
        private HObject Image_MoveIndex = null;
        /// <summary>
        /// 某一取像位置之完整 Cell regions (平移到原點)
        /// </summary>
        private HObject CellReg_MoveIndex = null;
        /// <summary>
        /// CellReg_MoveIndex經過Dilation運算
        /// </summary>
        private HObject CellReg_MoveIndex_Dila = null;

        /// <summary>
        /// 將大圖瑕疵Cell區域 轉到 某一取像位置 (平移到原點)
        /// </summary>
        //private List<HObject> DefectReg_MoveIndex = new List<HObject>();
        /// <summary>
        /// 將大圖瑕疵Cell區域 轉到 某一取像位置 (平移到原點)
        /// </summary>
        private Dictionary<string, HObject> Dic_DefectReg_MoveIndex { get; set; } = new Dictionary<string, HObject>(); // (20190716) Jeff Revised!

        /// <summary>
        /// 將大圖瑕疵Cell區域 轉到 某一取像位置 (平移到原點) (原始檢測結果)
        /// </summary>
        //private List<HObject> DefectReg_MoveIndex_Orig = new List<HObject>(); // (20190713) Jeff Revised!
        /// <summary>
        /// 將大圖瑕疵Cell區域 轉到 某一取像位置 (平移到原點) (原始檢測結果)
        /// </summary>
        private Dictionary<string, HObject> Dic_DefectReg_MoveIndex_Orig { get; set; } = new Dictionary<string, HObject>(); // (20190716) Jeff Revised!
        /// <summary>
        /// 使用者點選之瑕疵名稱
        /// </summary>
        private List<string> DefectName_select = new List<string>();

        /// <summary>
        /// 已標註Cell
        /// </summary>
        private HObject cellLabelled_MoveIndex = null;

        /// <summary>
        /// 單顆標註模式之瑕疵座標
        /// </summary>
        private List<Point> DefectId_SingleCell = new List<Point>();

        private int light_img_index_Recheck = 0; // 該位置第幾張影像
        
        private List<int> index_defectList = new List<int>(); // 瑕疵類型index轉成【進階】頁面中顯示區域之index (20190624) Jeff Revised!
        private Dictionary<string, int> Dictionary_defectList = new Dictionary<string, int>(); // 瑕疵名稱對應【進階】頁面中顯示區域之index (20190713) Jeff Revised!
        private void Create_Recheck_UI() // (20190715) Jeff Revised!
        {
            // 此位置之所有影像
            img_lst_Recheck = NowMapItem.ImgObj.Source;

            // 此位置影像
            this.cbx_LightImage_Recheck.SelectedIndexChanged -= new System.EventHandler(this.cbx_LightImage_Recheck_SelectedIndexChanged);
            cbx_LightImage_Recheck.Items.Clear();
            for (int i = 0; i < img_lst_Recheck.Count; i++)
            {
                string lightName = "Light Image " + (i + 1).ToString();
                cbx_LightImage_Recheck.Items.Add(lightName);
            }
            if (light_img_index_Recheck > img_lst_Recheck.Count - 1)
                light_img_index_Recheck = 0;
            cbx_LightImage_Recheck.SelectedIndex = light_img_index_Recheck;
            Image_MoveIndex = img_lst_Recheck[light_img_index_Recheck];
            this.cbx_LightImage_Recheck.SelectedIndexChanged += new System.EventHandler(this.cbx_LightImage_Recheck_SelectedIndexChanged);
            
            // 計算此位置Cell
            Extension.HObjectMedthods.ReleaseHObject(ref CellReg_MoveIndex);
            index_capture = NowMapItem.ImgObj.MoveIndex;
            Locate_Method_FS.Compute_CellReg_MoveIndex(index_capture, out CellReg_MoveIndex);
            Extension.HObjectMedthods.ReleaseHObject(ref CellReg_MoveIndex_Dila);
            HOperatorSet.DilationCircle(CellReg_MoveIndex, out CellReg_MoveIndex_Dila, 5);

            if (!B_doing_Recheck) // 沒有在執行【人工覆判】
            {
                DefectName_select.Clear();
                foreach (string name in Locate_Method_FS.DefectsClassify.Keys)
                    DefectName_select.Add(name);
            }

            /* 計算此位置瑕疵 (原始檢測結果) */
            Update_DefectReg_MoveIndex_Orig();

            /* 計算此位置瑕疵 + 計算此位置已標註Cell */
            Update_DefectReg_cellLabelled_MoveIndex();

            // 更新顯示 (總瑕疵)
            //if (!B_doing_Recheck) // 沒有在執行【人工覆判】
            //{
            //    DefectName_select.Clear();
            //    foreach (string name in Locate_Method_FS.DefectsClassify.Keys)
            //        DefectName_select.Add(name);
            //}
            Display_Recheck();

            // 【瑕疵類型:】
            comboBox_DefectType.Items.Clear();
            index_defectList.Clear();
            Dictionary_defectList.Clear();
            for (int i = 0; i < defectList.Count; i++)
            {
                if (defectList[i].B_defect)
                {
                    comboBox_DefectType.Items.Add(defectList[i].DefectName);
                    index_defectList.Add(i);
                    Dictionary_defectList.Add(defectList[i].DefectName, i);
                }
            }
            comboBox_DefectType.SelectedIndex = NowIndex_DefectType;

            // 更新 listView_Edit
            Locate_Method_FS.Update_listView_Edit(this.listView_Edit, this.radioButton_Result, this.radioButton_Recheck);

            // 更新 listView_Result
            Locate_Method_FS.Update_listView_Result(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);

        }

        /// <summary>
        /// 計算此位置瑕疵 + 計算此位置已標註Cell
        /// </summary>
        private void Update_DefectReg_cellLabelled_MoveIndex() // (20190713) Jeff Revised!
        {
            /* 計算此位置瑕疵 */
            foreach (string name in Dic_DefectReg_MoveIndex.Keys)
            {
                if (Dic_DefectReg_MoveIndex[name] != null)
                    Dic_DefectReg_MoveIndex[name].Dispose();
            }
            Dic_DefectReg_MoveIndex.Clear();
            if (!Locate_Method_FS.b_Defect_Classify) // 單一瑕疵
            {
                HObject reg;
                Locate_Method_FS.Compute_DefectReg_MoveIndex(index_capture, Locate_Method_FS.all_intersection_defect_Recheck, out reg);
                Dic_DefectReg_MoveIndex.Add("NG", reg);
                //reg.Dispose();
            }
            else // 多瑕疵
            {
                foreach (string name in DefectName_select)
                {
                    HObject reg;
                    Locate_Method_FS.Compute_DefectReg_MoveIndex(index_capture, Locate_Method_FS.DefectsClassify[name].all_intersection_defect_Recheck, out reg);
                    Dic_DefectReg_MoveIndex.Add(name, reg);
                    //reg.Dispose();
                }
            }

            /* 計算此位置已標註Cell */
            Extension.HObjectMedthods.ReleaseHObject(ref cellLabelled_MoveIndex);
            Locate_Method_FS.Compute_DefectReg_MoveIndex(index_capture, Locate_Method_FS.all_intersection_cellLabelled_Recheck, out cellLabelled_MoveIndex);
        }

        /// <summary>
        /// 計算此位置瑕疵 (原始檢測結果)
        /// </summary>
        private void Update_DefectReg_MoveIndex_Orig() // (20190715) Jeff Revised!
        {
            /* 計算此位置瑕疵 */
            foreach (string name in Dic_DefectReg_MoveIndex_Orig.Keys)
            {
                if (Dic_DefectReg_MoveIndex_Orig[name] != null)
                    Dic_DefectReg_MoveIndex_Orig[name].Dispose();
            }
            Dic_DefectReg_MoveIndex_Orig.Clear();

            int Type_DispResult = 2;
            if (Type_DispResult == 1) // Type 1: 原始瑕疵region
            {
                if (!Locate_Method_FS.b_Defect_Classify) // 單一瑕疵
                {
                    HObject reg;
                    Locate_Method_FS.Compute_DefectReg_MoveIndex(index_capture, Locate_Method_FS.all_intersection_defect_, out reg);
                    Dic_DefectReg_MoveIndex_Orig.Add("NG", reg);
                    //reg.Dispose();
                }
                else // 多瑕疵
                {
                    foreach (string name in DefectName_select)
                    {
                        HObject reg;
                        if (Locate_Method_FS.b_Defect_priority)
                            Locate_Method_FS.Compute_DefectReg_MoveIndex(index_capture, Locate_Method_FS.DefectsClassify[name].all_intersection_defect_Priority, out reg);
                        else
                            Locate_Method_FS.Compute_DefectReg_MoveIndex(index_capture, Locate_Method_FS.DefectsClassify[name].all_intersection_defect_Origin, out reg);

                        Dic_DefectReg_MoveIndex_Orig.Add(name, reg);
                        //reg.Dispose();
                    }
                }
            }
            else if (Type_DispResult == 2) // Type 2: 原始瑕疵所在Cell region (20190715) Jeff Revised!
            {
                if (!Locate_Method_FS.b_Defect_Classify) // 單一瑕疵
                {
                    HObject reg;
                    Locate_Method_FS.Compute_DefectReg_MoveIndex(index_capture, Locate_Method_FS.all_intersection_defect_CellReg, out reg);
                    Dic_DefectReg_MoveIndex_Orig.Add("NG", reg);
                    //reg.Dispose();
                }
                else // 多瑕疵
                {
                    foreach (string name in DefectName_select)
                    {
                        HObject reg;
                        if (Locate_Method_FS.b_Defect_priority)
                            Locate_Method_FS.Compute_DefectReg_MoveIndex(index_capture, Locate_Method_FS.DefectsClassify[name].all_intersection_defect_Priority_CellReg, out reg);
                        else
                            Locate_Method_FS.Compute_DefectReg_MoveIndex(index_capture, Locate_Method_FS.DefectsClassify[name].all_intersection_defect_Origin_CellReg, out reg);

                        Dic_DefectReg_MoveIndex_Orig.Add(name, reg);
                        //reg.Dispose();
                    }
                }
            }
            
        }

        /// <summary>
        /// 【顯示原圖】切換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_orig_CheckedChanged(object sender, EventArgs e) // (20190713) Jeff Revised!
        {
            // 更新顯示 (總瑕疵)
            DefectName_select.Clear();
            foreach (string name in Locate_Method_FS.DefectsClassify.Keys)
                DefectName_select.Add(name);
            Display_Recheck(false);
        }
        
        /// <summary>
        /// 影像切換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_LightImage_Recheck_SelectedIndexChanged(object sender, EventArgs e) // (20190713) Jeff Revised!
        {
            light_img_index_Recheck = cbx_LightImage_Recheck.SelectedIndex;
            Image_MoveIndex = img_lst_Recheck[light_img_index_Recheck];

            // 更新顯示 (總瑕疵)
            DefectName_select.Clear();
            foreach (string name in Locate_Method_FS.DefectsClassify.Keys)
                DefectName_select.Add(name);
            Display_Recheck(false);
        }

        /// <summary>
        /// 【顯示資訊】 切換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_defTest_CheckedChanged(object sender, EventArgs e) // (20190713) Jeff Revised!
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == false) return;

            // 更新顯示
            Display_Recheck(false);

            // 更新 listView_Edit
            Locate_Method_FS.Update_listView_Edit(this.listView_Edit, this.radioButton_Result, this.radioButton_Recheck);

            // 更新 listView_Result
            Locate_Method_FS.Update_listView_Result(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);
        }

        /// <summary>
        /// 更新顯示
        /// </summary>
        /// <param name="b_SetPart">是否做 SetPart</param>
        private void Display_Recheck(bool b_SetPart = true) // (20190713) Jeff Revised!
        {
            if (checkBox_orig.Checked) // 【顯示原圖】
            {
                HOperatorSet.ClearWindow(hSmartWindowControl_Recheck.HalconWindow);
                HOperatorSet.DispObj(Image_MoveIndex, hSmartWindowControl_Recheck.HalconWindow);
                if (b_SetPart)
                    HOperatorSet.SetPart(hSmartWindowControl_Recheck.HalconWindow, 0, 0, -1, -1); // The size of the last displayed image is chosen as the image part
            }
            else
            {
                if (radioButton_Result.Checked) // 【檢測結果】
                {
                    Locate_Method_FS.Disp_Recheck(hSmartWindowControl_Recheck, Image_MoveIndex, CellReg_MoveIndex,
                                                  Dic_DefectReg_MoveIndex_Orig, DefectName_select, null, b_SetPart);
                }
                else if (radioButton_Recheck.Checked) // 【人工覆判結果】
                {
                    Locate_Method_FS.Disp_Recheck(hSmartWindowControl_Recheck, Image_MoveIndex, CellReg_MoveIndex,
                                                  Dic_DefectReg_MoveIndex, DefectName_select, cellLabelled_MoveIndex, b_SetPart);
                }
            }
            
        }

        /// <summary>
        /// 是否正在執行【人工覆判】
        /// </summary>
        public bool B_doing_Recheck = false; // (20190715) Jeff Revised!
        /// <summary>
        /// 【人工覆判】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_Do_Recheck_CheckedChanged(object sender, EventArgs e) // (20190715) Jeff Revised!
        {
            CheckBox Obj = (CheckBox)sender;

            if (Obj.Checked) // 人工覆判
            {
                try
                {
                    DefectName_select.Clear();
                    contextMenuStrip_Recheck.Items.Clear();
                    if (Locate_Method_FS.b_Defect_Classify && radioButton_MultiCells.Checked) // 多瑕疵 & 多顆標註模式
                    {
                        if (listView_Edit.SelectedIndices.Count <= 0)
                        {
                            #region CheckBox狀態復原
                            cbx_Do_Recheck.CheckedChanged -= cbx_Do_Recheck_CheckedChanged;
                            Obj.Checked = false;
                            cbx_Do_Recheck.CheckedChanged += cbx_Do_Recheck_CheckedChanged;
                            #endregion

                            ctrl_timer1 = listView_Edit;
                            BackColor_ctrl_timer1_1 = Color.AliceBlue;
                            BackColor_ctrl_timer1_2 = Color.Green;
                            timer1.Enabled = true;
                            SystemSounds.Exclamation.Play();
                            MessageBox.Show("請選擇瑕疵名稱", "溫馨提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            timer1.Enabled = false;
                            ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                            return;
                        }
                        else
                        {
                            DefectName_select.Add(listView_Edit.SelectedItems[0].Text);
                        }
                    }
                    else if (Locate_Method_FS.b_Defect_Classify && radioButton_SingleCell.Checked) // 多瑕疵 & 單顆標註模式
                    {
                        contextMenuStrip_Recheck.Items.Add(GetMenuItem("OK", Color.Green));
                        foreach (string name in Locate_Method_FS.DefectsClassify.Keys)
                        {
                            DefectName_select.Add(name);
                            contextMenuStrip_Recheck.Items.Add(GetMenuItem(name, Locate_Method_FS.DefectsClassify[name].GetColor()));
                        }
                    }
                    else // 單一瑕疵
                    {
                        DefectName_select.Add("");
                        if (radioButton_SingleCell.Checked) // 單顆標註模式
                        {
                            contextMenuStrip_Recheck.Items.Add(GetMenuItem("OK", Color.Green));
                            contextMenuStrip_Recheck.Items.Add(GetMenuItem("NG", Color.Red));
                        }
                    }

                    // 強制勾選【顯示資訊】為【人工覆判結果】
                    radioButton_Recheck.Checked = true;
                    
                    // 更新顯示
                    Locate_Method_FS.Disp_Recheck(hSmartWindowControl_Recheck, Image_MoveIndex, CellReg_MoveIndex,
                                                  Dic_DefectReg_MoveIndex, DefectName_select, cellLabelled_MoveIndex, true);

                    ListCtrl_Bypass = clsStaticTool.EnableAllControls_Bypass(this.tabPage_Recheck, Obj, false); // (20200429) Jeff Revised!
                    Obj.ForeColor = Color.GreenYellow;
                    Obj.Text = "設定完成";
                    ctrl_timer1 = Obj;
                    BackColor_ctrl_timer1_1 = Color.DarkViolet;
                    BackColor_ctrl_timer1_2 = Color.Lime;
                    timer1.Enabled = true;

                    label_cellID.Enabled = true;
                    txt_cellID.Enabled = true;
                    
                    // 致能滑鼠點擊事件 (人工覆判)
                    this.hSmartWindowControl_Recheck.HMouseDown += new HalconDotNet.HMouseEventHandler(this.hSmartWindowControl_Recheck_HMouseDown);

                    B_doing_Recheck = true;

                }
                catch (Exception ex)
                {
                    #region CheckBox狀態復原
                    cbx_Do_Recheck.CheckedChanged -= cbx_Do_Recheck_CheckedChanged;
                    Obj.Checked = false;
                    cbx_Do_Recheck.CheckedChanged += cbx_Do_Recheck_CheckedChanged;
                    #endregion

                    MessageBox.Show(ex.ToString(), "人工覆判失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

            }
            else // 設定完成
            {
                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                clsStaticTool.EnableAllControls_Bypass(this.tabPage_Recheck, Obj, true, ListCtrl_Bypass); // (20200429) Jeff Revised!
                Obj.ForeColor = Color.White;
                Obj.Text = "人工覆判";

                // 禁能滑鼠點擊事件 (人工覆判)
                this.hSmartWindowControl_Recheck.HMouseDown -= new HalconDotNet.HMouseEventHandler(this.hSmartWindowControl_Recheck_HMouseDown);

                B_doing_Recheck = false;

                try
                {
                    // 更新顯示 (總瑕疵)
                    DefectName_select.Clear();
                    foreach (string name in Locate_Method_FS.DefectsClassify.Keys)
                        DefectName_select.Add(name);
                    Display_Recheck(false);

                    // 更新 listView_Result
                    Locate_Method_FS.Update_listView_Result(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "人工覆判失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
        }

        /// <summary>
        /// 滑鼠點擊事件 (人工覆判)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hSmartWindowControl_Recheck_HMouseDown(object sender, HMouseEventArgs e) // (20190715) Jeff Revised!
        {
            // 游標是否於 Cell region 內
            HTuple hv_Index = new HTuple();
            HOperatorSet.GetRegionIndex(CellReg_MoveIndex, (int)(e.Y + 0.5), (int)(e.X + 0.5), out hv_Index);
            if (hv_Index.Length <= 0)
            {
                txt_cellID.Text = "";
                return;
            }

            HTuple cell_ids = new HTuple();
            List<Point> ListCellID = new List<Point>();
            // 計算此顆Cell座標
            if (!Locate_Method_FS.pos_2_cellID_MoveIndex(index_capture, e.Y, e.X, out cell_ids, out ListCellID))
                return;

            if (e.Button == System.Windows.Forms.MouseButtons.Left) // 滑鼠是否點擊左鍵
            {
                #region 滑鼠點擊左鍵Cell座標 (X, Y)

                // 更新顯示
                string str = "";
                foreach (Point pt in ListCellID)
                    str += "(" + pt.X + ", " + pt.Y + ")";
                txt_cellID.Text = str;
                Locate_Method_FS.Disp_Recheck(hSmartWindowControl_Recheck, Image_MoveIndex, CellReg_MoveIndex,
                                              Dic_DefectReg_MoveIndex, DefectName_select, cellLabelled_MoveIndex, false);

                // 顯示上下文選單
                toolStripMenuItem_ID.Text = str;
                HTuple rowWindow, columnWindow;
                HOperatorSet.ConvertCoordinatesImageToWindow(hSmartWindowControl_Recheck.HalconWindow, e.Y, e.X, out rowWindow, out columnWindow);
                contextMenuStrip_DispID.Show(hSmartWindowControl_Recheck, (int)(columnWindow.D + 0.5), (int)(rowWindow.D + 0.5));

                // 顯示此位置Cell
                HOperatorSet.SetDraw(hSmartWindowControl_Recheck.HalconWindow, "margin");
                HOperatorSet.SetColor(hSmartWindowControl_Recheck.HalconWindow, "#FF00FFFF"); // Magenta
                HTuple line_w;
                HOperatorSet.GetLineWidth(hSmartWindowControl_Recheck.HalconWindow, out line_w);
                HOperatorSet.SetLineWidth(hSmartWindowControl_Recheck.HalconWindow, 3);
                HObject reg;
                HOperatorSet.SelectObj(CellReg_MoveIndex_Dila, out reg, hv_Index);
                HOperatorSet.DispObj(reg, hSmartWindowControl_Recheck.HalconWindow);
                reg.Dispose();
                HOperatorSet.SetLineWidth(hSmartWindowControl_Recheck.HalconWindow, line_w);

                #endregion

                return;
            }

            if (e.Button != System.Windows.Forms.MouseButtons.Right) // 滑鼠是否點擊右鍵
                return;

            if (radioButton_MultiCells.Checked) // 多顆標註模式 (小圖上標註 & 大圖上標註)
            {
                #region 多顆標註模式

                // 判斷是要判定此顆Cell為NG or OK: 利用游標是否於 DefectReg_MoveIndex 內
                bool b_NG = false;
                HOperatorSet.GetRegionIndex(Dic_DefectReg_MoveIndex[DefectName_select[0]], (int)(e.Y + 0.5), (int)(e.X + 0.5), out hv_Index);
                if (hv_Index.Length <= 0)
                    b_NG = true;

                /* 更新人工覆判結果 */
                if (b_NG) // 標註此顆Cell NG
                    Locate_Method_FS.Update_Recheck(null, ListCellID, DefectName_select[0]);
                else // 標註此顆Cell OK
                    Locate_Method_FS.Update_Recheck(ListCellID, null, DefectName_select[0]);

                /* 計算此位置瑕疵 + 計算此位置已標註Cell */
                Update_DefectReg_cellLabelled_MoveIndex();

                // 更新顯示
                Locate_Method_FS.Disp_Recheck(hSmartWindowControl_Recheck, Image_MoveIndex, CellReg_MoveIndex,
                                              Dic_DefectReg_MoveIndex, DefectName_select, cellLabelled_MoveIndex, false);

                // 更新 listView_Result
                Locate_Method_FS.Update_listView_Result(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);

                #endregion

            }
            else if (radioButton_SingleCell.Checked) // 單顆標註模式
            {
                #region 單顆標註模式

                if (ListCellID.Count != 1)
                {
                    SystemSounds.Exclamation.Play();
                    MessageBox.Show("【單顆標註模式】一次僅能選擇一顆Cell做標註", "溫馨提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    DefectId_SingleCell.Clear();
                    DefectId_SingleCell.Add(ListCellID[0]);
                }

                /* 更新上下文選單 */
                if (!Locate_Method_FS.b_Defect_Classify) // 單一瑕疵
                {
                    if (Locate_Method_FS.all_defect_id_Recheck.Contains(DefectId_SingleCell[0])) // NG Cell
                    {
                        ((ToolStripMenuItem)contextMenuStrip_Recheck.Items[0]).Checked = false;
                        ((ToolStripMenuItem)contextMenuStrip_Recheck.Items[1]).Checked = true;
                    }
                    else // OK Cell
                    {
                        ((ToolStripMenuItem)contextMenuStrip_Recheck.Items[0]).Checked = true;
                        ((ToolStripMenuItem)contextMenuStrip_Recheck.Items[1]).Checked = false;
                    }
                }
                else // 多瑕疵
                {
                    // 判斷各瑕疵類型
                    bool b_OKCell = true;
                    for (int i = 1; i <= DefectName_select.Count; i++)
                    {
                        if (Locate_Method_FS.DefectsClassify[DefectName_select[i - 1]].all_defect_id_Recheck.Contains(DefectId_SingleCell[0])) // 包含此瑕疵類型
                        {
                            ((ToolStripMenuItem)contextMenuStrip_Recheck.Items[i]).Checked = true;
                            b_OKCell = false;
                        }
                        else
                            ((ToolStripMenuItem)contextMenuStrip_Recheck.Items[i]).Checked = false;
                    }
                    if (b_OKCell) // OK Cell
                        ((ToolStripMenuItem)contextMenuStrip_Recheck.Items[0]).Checked = true;
                    else // NG Cell
                        ((ToolStripMenuItem)contextMenuStrip_Recheck.Items[0]).Checked = false;

                }

                /* 顯示上下文選單 */
                HTuple rowWindow, columnWindow;
                HOperatorSet.ConvertCoordinatesImageToWindow(hSmartWindowControl_Recheck.HalconWindow, e.Y, e.X, out rowWindow, out columnWindow);
                contextMenuStrip_Recheck.Show(hSmartWindowControl_Recheck, (int)(columnWindow.D + 0.5), (int)(rowWindow.D + 0.5));

                #endregion

            }

        }

        /// <summary>
        /// 生成選單項
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="ForeColor"></param>
        /// <param name="img"></param>
        /// <param name="b_ClickEvent"></param>
        /// <returns></returns>
        private ToolStripMenuItem GetMenuItem(string txt, object ForeColor = null, Image img = null, bool b_ClickEvent = true) // (20190710) Jeff Revised!
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem();

            // 屬性設定
            menuItem.Text = txt;
            if (ForeColor is Color)
                menuItem.ForeColor = (Color)ForeColor;
            if (img != null)
                menuItem.Image = img;

            // 點擊觸發事件設定
            if (b_ClickEvent)
                menuItem.Click += new EventHandler(toolStripMenuItem_Click);

            return menuItem;
        }

        /// <summary>
        /// 選單項事件響應
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem_Click(object sender, EventArgs e) // (20190715) Jeff Revised!
        {
            ToolStripMenuItem menu = sender as ToolStripMenuItem;
            //MessageBox.Show(menu.Text);
            string name = menu.Text;

            // 如果原先狀態為OK啟用，則不改變狀態! (因為不知道要判定哪一種瑕疵類型)
            if (name == "OK" && menu.Checked)
                return;

            // Checked 狀態改變
            menu.Checked = !(menu.Checked);

            /* 更新人工覆判結果 */
            if (name == "OK") // 點擊【OK】
                Locate_Method_FS.Update_All_OK_Recheck(DefectId_SingleCell);
            else // 點擊其他瑕疵類型
            {
                if (menu.Checked) // 標註此種瑕疵
                    Locate_Method_FS.Update_Recheck(null, DefectId_SingleCell, name);
                else // 解除標註此種瑕疵
                    Locate_Method_FS.Update_Recheck(DefectId_SingleCell, null, name);
            }

            /* 計算此位置瑕疵 + 計算此位置已標註Cell */
            Update_DefectReg_cellLabelled_MoveIndex();

            // 更新顯示
            Locate_Method_FS.Disp_Recheck(hSmartWindowControl_Recheck, Image_MoveIndex, CellReg_MoveIndex,
                                          Dic_DefectReg_MoveIndex, DefectName_select, cellLabelled_MoveIndex, false);

            // 更新 listView_Result
            Locate_Method_FS.Update_listView_Result(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);

        }

        List<Control> ListCtrl_Bypass = new List<Control>();
        
        /// <summary>
        /// 【清空人工覆判】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Clear_Recheck_Click(object sender, EventArgs e) // (20190715) Jeff Revised!
        {
            // 新增使用者保護機制
            SystemSounds.Exclamation.Play();
            DialogResult dr = MessageBox.Show("確定要清空人工覆判 ?", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes)
                return;

            Locate_Method_FS.Initialize_Recheck();

            /* 計算此位置瑕疵 + 計算此位置已標註Cell */
            Update_DefectReg_cellLabelled_MoveIndex();

            // 更新顯示 (總瑕疵)
            DefectName_select.Clear();
            foreach (string name in Locate_Method_FS.DefectsClassify.Keys)
                DefectName_select.Add(name);
            Display_Recheck(false);

            // 更新 listView_Edit
            Locate_Method_FS.Update_listView_Edit(this.listView_Edit, this.radioButton_Result, this.radioButton_Recheck);

            // 更新 listView_Result
            Locate_Method_FS.Update_listView_Result(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);
        }

        private Control ctrl_timer1 = null;
        private Color BackColor_ctrl_timer1_1 = Color.Transparent, BackColor_ctrl_timer1_2 = Color.Green;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ctrl_timer1 == null) return;

            if (ctrl_timer1.BackColor == BackColor_ctrl_timer1_1)
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_2;
            else
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
        }

        /// <summary>
        /// 更新顯示瑕疵 (hSmartWindowControl_Recheck)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_Edit_SelectedIndexChanged(object sender, EventArgs e) // (20190715) Jeff Revised!
        {
            if (listView_Edit.SelectedIndices.Count <= 0)
                return;
            
            // 更新顯示 (點選之瑕疵)
            DefectName_select.Clear();
            DefectName_select.Add(listView_Edit.SelectedItems[0].Text);
            Display_Recheck(false);
        }

        /// <summary>
        /// 【編輯顏色】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Edit_DefectsClassify_Click(object sender, EventArgs e) // (20190715) Jeff Revised!
        {
            if (listView_Edit.SelectedIndices.Count <= 0)
            {
                ctrl_timer1 = listView_Edit;
                BackColor_ctrl_timer1_1 = Color.AliceBlue;
                BackColor_ctrl_timer1_2 = Color.Green;
                timer1.Enabled = true;
                SystemSounds.Exclamation.Play();
                MessageBox.Show("請選擇瑕疵名稱", "溫馨提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                return;
            }

            string name = listView_Edit.SelectedItems[0].Text;
            using (Form_editDefect form = new Form_editDefect(Locate_Method_FS, true, name, false))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // 更新 hSmartWindowControl_Recheck
                    listView_Edit_SelectedIndexChanged(sender, e);

                    // 更新 listView_Edit
                    Locate_Method_FS.Update_listView_Edit(this.listView_Edit, this.radioButton_Result, this.radioButton_Recheck);

                    // 更新 listView_Result
                    Locate_Method_FS.Update_listView_Result(this.listView_Result, this.radioButton_Result, this.radioButton_Recheck);

                    // 更新【進階】頁面內對應瑕疵之顏色
                    if (Dictionary_defectList.ContainsKey(name))
                    {
                        int i_defectList = Dictionary_defectList[name];
                        inspectRUIList[i_defectList].SetColor_update(Locate_Method_FS.DefectsClassify[name].GetColor());
                    }
                }
            }
        }

        /// <summary>
        /// 【顏色設定】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_SetColor_Recheck_Click(object sender, EventArgs e)
        {
            if (comboBox_DefectType.SelectedIndex < 0)
            {
                ctrl_timer1 = comboBox_DefectType;
                BackColor_ctrl_timer1_1 = Color.White;
                BackColor_ctrl_timer1_2 = Color.Green;
                timer1.Enabled = true;
                SystemSounds.Exclamation.Play();
                MessageBox.Show("請選擇【瑕疵類型】!", "溫馨提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                timer1.Enabled = false;
                ctrl_timer1.BackColor = BackColor_ctrl_timer1_1;
                return;
            }

            //int i_defectList = index_defectList[comboBox_DefectType.SelectedIndex];
            int i_defectList = Dictionary_defectList[comboBox_DefectType.Text];
            inspectRUIList[i_defectList].Set_button_SetColor_Click();
        }

        private int NowIndex_DefectType = -1;
        /// <summary>
        /// 【瑕疵類型:】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_DefectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_DefectType.SelectedIndex < 0)
                return;
            else
                NowIndex_DefectType = comboBox_DefectType.SelectedIndex;
            
            //int i_defectList = index_defectList[NowIndex_DefectType];
            int i_defectList = Dictionary_defectList[comboBox_DefectType.Text];
            button_SetColor_Recheck.BackColor = inspectRUIList[i_defectList].GetColor_ColorType();
        }
        
        #endregion
        


        #region 其他

        private void update_info(List<Point> pos)
        {
            for (int i = 0; i < pos.Count; i++)
            {
                Label lbl = new Label();
                lbl.AutoSize = true;
                lbl.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                lbl.Location = new System.Drawing.Point(14, 75);
                lbl.Size = new System.Drawing.Size(86, 24);
                lbl.Location = new System.Drawing.Point(12, 42 + 30 * i);
                lbl.Text = "(" + pos[i].Y + "," + pos[i].X + ")";
                panel1.Controls.Add(lbl);
            }
        }

        private void update_info2(List<Point> pos)
        {
            ResultUI_dt.Columns.Clear();

            // 結果初始化 --> for UI
            //ResultUI_dt.Columns.Add(new DataColumn("X", typeof(string)));
            //ResultUI_dt.Columns.Add(new DataColumn("Y", typeof(string)));
            ResultUI_dt.Columns.Add(new DataColumn("瑕疵位置", typeof(string)));

            dataGridView_UI2Dindex.DataSource = ResultUI_dt;
            dataGridView_UI2Dindex.Columns[0].Width = 110;
            //dataGridView_UI2Dindex.Columns[0].Width = 55;
            //dataGridView_UI2Dindex.Columns[1].Width = 55;
            dataGridView_UI2Dindex.ColumnHeadersDefaultCellStyle.Font = new Font("微軟正黑體", 9);
            dataGridView_UI2Dindex.DefaultCellStyle.Font = new Font("微軟正黑體", 9);
            dataGridView_UI2Dindex.ReadOnly = true;
            dataGridView_UI2Dindex.AllowUserToAddRows = false;
            dataGridView_UI2Dindex.AllowUserToDeleteRows = false;
            dataGridView_UI2Dindex.AllowUserToResizeColumns = false;
            dataGridView_UI2Dindex.AllowUserToResizeRows = false;
            dataGridView_UI2Dindex.MultiSelect = false;


            // Show
            ResultUI_dt.Clear();
            for (int i = 0; i < pos.Count; i++)
            {

                string XIndexStrUI = pos[i].X.ToString();
                string YIndexStrUI = pos[i].Y.ToString();

                //ResultUI_dt.Rows.Add(new string[] { XIndexStrUI, YIndexStrUI });
                ResultUI_dt.Rows.Add("(" + XIndexStrUI + "," + YIndexStrUI + ")");

            }

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ImageShowForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
            if (form_closing != null) // (20190124)
                form_closing(sender, e);
        }

        private void ImageShowForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
                this.TopMost = true;
            else
                this.TopMost = false;
        }
        
        /// <summary>
        /// 【參數調整】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_ShowParaForm_Click(object sender, EventArgs e)
        {
            if (parameterFormShow_clicked != null)
            {
                //Dispose();
                this.Close();
                parameterFormShow_clicked(sender, e);
            }
        }

        /// <summary>
        /// 【儲存影像組】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SaveImageList_Click(object sender, EventArgs e)
        {          
            // Save image 
            string temp_SB_ID = SB_ID;
            SB_ID = "";
            StorageManager.write_source_img(NowMapItem.ImgObj);
            SB_ID = temp_SB_ID;
        }

        #endregion

    }
}
