using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static DeltaSubstrateInspector.FileSystem.FileSystem;


// 加速DataGridView更新速度
using System.Globalization;
using System.Reflection;

namespace DeltaSubstrateInspector.src.MainPanels
{
    public partial class LaserPositionCreate : UserControl
    {
        List<int> CombineAllGlobalIndices = new List<int>(); // 最終1D 瑕疵index

        DataTable Result_dt = new DataTable();
        DataTable ResultUI_dt = new DataTable();
        DataTable InsInfo_dt = new DataTable(); // 檢測歷史資料
        private List<string> ResultReport = new List<string>(); // 檢測結果報告

        int realTotalCount = 0;
        int realNgCount = 0;
        double realNgRatio = 0;

        public LaserPositionCreate()
        {
            InitializeComponent();

            
            Result_dt.Columns.Add(new DataColumn("X Index", typeof(string)));
            Result_dt.Columns.Add(new DataColumn("Y Index", typeof(string)));
            dataGridView_DefectIndex.DataSource = Result_dt;


            #region Update now recipe file name
            string path = ModuleParamDirectory + RecipeFileDirectory;

            // 檢查執行檔目錄是否存在
            if (!System.IO.Directory.Exists(path))
            {
                //MessageBox.Show("尚未建立: " + path);
                return;
            }

            string[] dirs = System.IO.Directory.GetDirectories(path);
            comboBox_nowRecipes.Items.Clear();
            foreach (string item in dirs)
            {
                comboBox_nowRecipes.Items.Add(System.IO.Path.GetFileNameWithoutExtension(item));
            }
            #endregion

            // DataGridView 加速更新速度
            dataGridView_DefectMap.DoubleBuffered(true);

        }

        private void textBox_ProcessResultStr_TextChanged(object sender, EventArgs e)
        {

        }

        private void button_Save_Click(object sender, EventArgs e)
        {

        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {

        }

        private void button_CombineIdicesAndTransfer_Click(object sender, EventArgs e)
        {

        }

        private void button_LoadIndicsFromFile_Click(object sender, EventArgs e)
        {

        }

        private void button_LoadListAndUpdate_Click(object sender, EventArgs e)
        {

        }

        private void button_FastLoadMultiFileAndCombine_Click(object sender, EventArgs e)
        {

        }

        private void buttonClear_Click(object sender, EventArgs e)
        {

        }

        private void buttonAdd_Click_1(object sender, EventArgs e)
        {
            int XMax = Convert.ToInt32(textBox_MaxColNum.Text);
            int YMax = Convert.ToInt32(textBox_MaxRowNum.Text);

            int X = Convert.ToInt32(textBox_XIndex.Text);
            int Y = Convert.ToInt32(textBox_YIndex.Text);

            if(X >= XMax)
            {
                textBox_XIndex.Text = (XMax - 1).ToString();
            }
            if (Y >= YMax)
            {
                textBox_YIndex.Text = (YMax - 1).ToString();
            }

            // 181220, new 格式化
            //string XIndexStr = String.Format("{0:00000000}", Convert.ToInt32(textBox_XIndex.Text));
            //string YIndexStr = String.Format("{0:00000000}", Convert.ToInt32(textBox_YIndex.Text));
            string XIndexStr = String.Format("{0:00000.000}", Convert.ToInt32(textBox_XIndex.Text));
            string YIndexStr = String.Format("{0:00000.000}", Convert.ToInt32(textBox_YIndex.Text));

            // Fill to table           
            Result_dt.Rows.Add(new string[] { XIndexStr, YIndexStr });


            // Update
            UpdateDefectInfo();
            
        }

        private void buttonClear_Click_1(object sender, EventArgs e)
        {
            Result_dt.Clear();
            ResultUI_dt.Clear();
            ResultReport.Clear();

            // Update
            UpdateDefectInfo();
        }

       
        private void UpdateDefectInfo()
        {
            string combine = "";
            for (int i = 0; i < Result_dt.Rows.Count; i++)
            {
                string x = Result_dt.Rows[i][0].ToString();
                string y = Result_dt.Rows[i][1].ToString();
                combine = combine + "(" + x + "," + y + ");";
            }

            // 判斷            
            realTotalCount = Locate_Method_FS.get_total_cell_count();
            realNgCount = Result_dt.Rows.Count;
            

            // Upload string
            textBox_ProcessResultStr.Text = "SUCCESS";            
            textBox_WorkResultStr.Text = "OK";
            textBox_TotalCntStr.Text = String.Format("{0:00000000}", realTotalCount);
            textBox_NgCountStr.Text = String.Format("{0:00000000}", realNgCount);
            textBox_DefectIdStr.Text = combine;

            // Display fix, 只顯示100筆
            if (realNgCount <= 100)
            {
                textBox_DefectIdStr.Text = combine;
            }
            else
            {
                textBox_DefectIdStr.Text = "超過 TextBox 顯示長度，已暫存內部記憶體!";
            }

           
            // Set transer string
            TransferStr = combine;

            
        }


        private void SaveFinalDefectReport()
        {
            // set save filename
            //string savePathRep = SetSavePath(appName, ResultReportDirName, "txt"); // "ResultReport"
            //string savePathLst = SetSavePath(appName, ResultListDirName, "dat");  // "ResultList"
            string savePathRep= "LaserPositionRep.txt";
            string savePathLst = "LaserPosition.dat";

            // 01. 檢測報告 Real save for user
            string totalStr = "總數: " + realTotalCount.ToString("0");
            string ngStr = "不良數: " + realNgCount.ToString("0");
            ResultReport.Insert(0, totalStr);
            ResultReport.Insert(1, ngStr);
            System.IO.File.WriteAllLines(savePathRep, ResultReport);

            // 02. List save for show (由List int轉List string)
            System.IO.File.WriteAllLines(savePathLst, CombineAllGlobalIndices.ConvertAll<string>(x => x.ToString()));

            // 03. 160617, 單體式AOI與單體式Laser交握
            /*
            if (mStaticCaptureTest == 1)
            {
                // !!!!~~~~!!!! 檢測結果為NG時，是否要直接清空File內容，以預防人員沒有注意到，往下傳!????
                string savePathLst4FileShare = SaveFileDisk + appName + "\\" + ResultListDirName + "\\" + HandShakeFileName;
                System.IO.File.WriteAllLines(savePathLst4FileShare, CombineAllGlobalIndices.ConvertAll<string>(x => x.ToString()));

            }*/


        }

        private string SetSavePath(string appName, string dirName, string expName)
        {
            /*
            string saveDir = SaveFileDisk + appName;

            // 建立E:\\LTCC資料夾
            if (!Directory.Exists(saveDir))
                Directory.CreateDirectory(saveDir);

            // 建立E:\\LTCC\\ResultReport資料夾
            saveDir = saveDir + "\\" + dirName; //ResultReport";
            if (!Directory.Exists(saveDir))
                Directory.CreateDirectory(saveDir);

            // 建立E:\\LTCC\\ResultReport\\yyyyMMdd
            string dateStr = DateTime.Now.ToString("yyyyMMdd");
            saveDir = saveDir + "\\" + dateStr;
            if (!Directory.Exists(saveDir))
                Directory.CreateDirectory(saveDir);

            // 建立E:\\LTCC\\ResultReport\\yyyyMMdd\\工單
            string workOrder = textBox_simWorkOrder.Text;
            saveDir = saveDir + "\\" + workOrder;
            if (!Directory.Exists(saveDir))
                Directory.CreateDirectory(saveDir);

            // fileName           
            string nowID = (sorterID == "") ? textBox_simID.Text : sorterID;
            string saveFN = dateStr + "_" + workOrder + "_" + nowID + "." + expName; //".txt";
            string savePath = saveDir + "\\" + saveFN;

            // return
            return savePath;
            */

            return "";
        }
        #region 取得傳送字串

        //------------------------------------------------------------------------------
        // 回傳目前的Defect index string
        //------------------------------------------------------------------------------
        public string GetDefectIndexStr()
        {
            //return textBox_DefectIdStr.Text;
            return TransferStr;
        }

        //------------------------------------------------------------------------------
        // 回傳目前的DefecCount string
        //------------------------------------------------------------------------------
        public string GetDefectCountStr()
        {
            return textBox_NgCountStr.Text;
        }

        //------------------------------------------------------------------------------
        // 回傳目前的TotalCount string
        //------------------------------------------------------------------------------
        public string GetTotalCountStr()
        {
            return textBox_TotalCntStr.Text;
        }

        //------------------------------------------------------------------------------
        // 回傳目前的WorkResult
        //------------------------------------------------------------------------------
        public string GetWorkResultStr()
        {
            return textBox_WorkResultStr.Text;
        }

        //------------------------------------------------------------------------------
        // 回傳目前的ProcessResult
        //------------------------------------------------------------------------------
        public string GetPorcessResultStr()
        {
            return textBox_ProcessResultStr.Text;
        }

        public bool GetLaserPostionTest()
        {
            return checkBox_LaserPositionTest.Checked;
        }
       
        public string GetAssignRecipeName()
        {
            return nowRecipe;
        }
        
        #endregion

        private void checkBox_LaserPositionTest_CheckedChanged(object sender, EventArgs e)
        {

        }

        
        private void LaserPositionCreate_VisibleChanged(object sender, EventArgs e)
        {

          

        }

        string nowRecipe = "";
        private void comboBox_nowRecipes_SelectedIndexChanged(object sender, EventArgs e)
        {
            nowRecipe = comboBox_nowRecipes.Text;

            

        }

        public void SetXYCount(int _XCount, int _YCount)
        {
            textBox_MaxRowNum.Text = _YCount.ToString();
            textBox_MaxColNum.Text = _XCount.ToString();
        }
       
        private void button_SaveAsStr_Click(object sender, EventArgs e)
        {
            string file_direction = "";
            SaveFileDialog file_dialog = new SaveFileDialog();
            file_dialog.Filter = "TXT files (*.txt)|*.txt";
            if (file_dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            file_direction = file_dialog.FileName;

            System.IO.File.WriteAllText(file_direction, TransferStr);

        }

        private void button_LoadStr_Click(object sender, EventArgs e)
        {
            string file_direction = "";
            OpenFileDialog file_dialog = new OpenFileDialog();
            file_dialog.Filter = "TXT files (*.txt)|*.txt";
            if (file_dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            file_direction = file_dialog.FileName;

            TransferStr = System.IO.File.ReadAllText(file_direction);

            // Display fix
            if (TransferStr.Length <= textBox_DefectIdStr.MaxLength)
            {
                textBox_DefectIdStr.Text = TransferStr;               
            }
            else
            {
                textBox_DefectIdStr.Text = "超過TextBox字串長度，已暫存內部記憶體!";
            }
           
        }

        private void button_CreateNewEmptyMap_Click(object sender, EventArgs e)
        {
            buttonClear_Click_1(sender,e);

            textBox_DefectIdStr.Text = "";
            dataGridView_DefectMap.Columns.Clear();
            dataGridView_DefectMap.Rows.Clear();

            // 選擇紅色          
            dataGridView_DefectMap.DefaultCellStyle.SelectionBackColor = Color.Red;


            // 讓使用者無法調整寬高
            dataGridView_DefectMap.AllowUserToResizeRows = false;
            dataGridView_DefectMap.AllowUserToResizeColumns = false;

            // Get now Row Column number
            int nowRowNumber = Convert.ToInt32(textBox_MaxRowNum.Text);
            int nowColNumber = Convert.ToInt32(textBox_MaxColNum.Text);

            // 設定每個Cell寬高
            int miniSize = 13;
            int CellSize_Height = dataGridView_DefectMap.Height / nowRowNumber;
            int CellSize_Width = dataGridView_DefectMap.Width / nowColNumber;
            int CellSizeSet = ((CellSize_Height < CellSize_Width) ? CellSize_Height : CellSize_Width) - 1;
            CellSizeSet = (CellSizeSet < miniSize) ? miniSize : CellSizeSet;

            // 必須先設定Row height
            dataGridView_DefectMap.RowTemplate.Height = CellSizeSet;


            // Create            
            dataGridView_DefectMap.RowHeadersVisible = false;
            dataGridView_DefectMap.ColumnHeadersVisible = false;
            dataGridView_DefectMap.RowCount = nowRowNumber;
            dataGridView_DefectMap.ColumnCount = nowColNumber;

            // 再設定 Column width
            foreach (DataGridViewColumn gCol in dataGridView_DefectMap.Columns)
            {
                gCol.Width = CellSizeSet;
            }
        }

        private string TransferStr = "";
        private void button_CreateDefect_Click(object sender, EventArgs e)
        {
            
            int selectCount = dataGridView_DefectMap.SelectedCells.Count;
            
            Result_dt.Clear();
            for (int i = 0; i < selectCount; i++)
            {
                int nowYID = dataGridView_DefectMap.SelectedCells[i].RowIndex;
                int nowXID = dataGridView_DefectMap.SelectedCells[i].ColumnIndex;
                string XIndexStr = nowXID.ToString("00000.000");
                string YIndexStr = nowYID.ToString("00000.000");
              
                // Fill to table           
                Result_dt.Rows.Add(new string[] { XIndexStr, YIndexStr });
                
            }

            // Update
            UpdateDefectInfo();
                                              
        }

        private void button_SaveBin_Click(object sender, EventArgs e)
        {
            string file_direction = "";
            SaveFileDialog file_dialog = new SaveFileDialog();
            file_dialog.Filter = "BIN files (*.bin)|*.bin";
            if (file_dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            file_direction = file_dialog.FileName; // + ".bin";


            using (BinaryWriter bw = new BinaryWriter(File.Open(file_direction, FileMode.Create)))
            {
                bw.Write(dataGridView_DefectIndex.Columns.Count);
                bw.Write(dataGridView_DefectIndex.Rows.Count);
                foreach (DataGridViewRow dgvR in dataGridView_DefectIndex.Rows)
                {
                    for (int j = 0; j < dataGridView_DefectIndex.Columns.Count; ++j)
                    {
                        object val = dgvR.Cells[j].Value;
                        if (val == null)
                        {
                            bw.Write(false);
                            bw.Write(false);
                        }
                        else
                        {
                            bw.Write(true);
                            bw.Write(val.ToString());
                        }
                    }
                }


            }
        }

        private void button_LoadBin_Click(object sender, EventArgs e)
        {
            string file_direction = "";
            OpenFileDialog file_dialog = new OpenFileDialog();
            file_dialog.Filter = "BIN files (*.bin)|*.bin";
            if (file_dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            file_direction = file_dialog.FileName;


            //DataGridView dgv = ...
            Result_dt.Clear();                     
            using (BinaryReader bw = new BinaryReader(File.Open(file_direction, FileMode.Open)))
            {
                int n = bw.ReadInt32();
                int m = bw.ReadInt32();
                for (int i = 0; i < m-1; ++i)
                {                    
                    string XIndexStr = "";
                    string YIndexStr = "";
                    for (int j = 0; j < n; ++j)
                    {
                        if (bw.ReadBoolean())
                        {                            
                            string nowStr = bw.ReadString();
                            if (j==0)
                            {
                                XIndexStr = nowStr;
                            }
                            else
                            {
                                YIndexStr = nowStr;
                            }
                        }
                        else
                        {
                            bw.ReadBoolean();
                        }


                    }

                    Result_dt.Rows.Add(new string[] { XIndexStr, YIndexStr });

                }
            }

            // Update
            UpdateDefectInfo();



        }

        private void dataGridView_DefectMap_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridView_DefectMap_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
           
            label_Index.Text = "X: " + e.ColumnIndex + " , Y: " + e.RowIndex;
            
        }

       
    }


    #region DataGridView 加速更新速度: 此Class一定要放在Form之後，不然設計畫面會無法顯示

    public static class ExtensionMethods
        {
            public static void DoubleBuffered(this DataGridView dgv, bool setting)
            {
                Type dgvType = dgv.GetType();
                PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                pi.SetValue(dgv, setting, null);
            }
        }


    #endregion

}
