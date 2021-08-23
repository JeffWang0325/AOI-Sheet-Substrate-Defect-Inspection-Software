using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DeltaSubstrateInspector.src.SystemSource.WorkSheet;

using static DeltaSubstrateInspector.FileSystem.FileSystem;

using DeltaSubstrateInspector.src.MainSetting;
using System.IO;

namespace DeltaSubstrateInspector.src.SystemSource.Recipe
{
    public partial class RecipeSelectForm : Form
    {
        private string recipe_name_ = "";
        private bool is_set_ =  false;

        // 181107, andy
        private string select_recipeName = "";
        public event EventHandler OnUserControlButtonClicked_SaveAs;
        public event EventHandler OnUserControlButtonClicked_Save;
        private List<string> nowRecipeNameList = new List<string>();
        
        public RecipeSelectForm(List<string> filenames)
        {
            InitializeComponent();
            create_btn(filenames);

            // 181107, andy
            nowRecipeNameList.Clear();
            nowRecipeNameList.AddRange(filenames);

        }

        public void create_btn(List<string> filenames)
        {
            for (int i = 0; i < filenames.Count; i++)
            {
                Button btn_new = new Button();
                btn_new.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
                btn_new.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
                btn_new.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                btn_new.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                btn_new.Location = new System.Drawing.Point(12, 12 + (38 + 10) * i);
                btn_new.Margin = new System.Windows.Forms.Padding(2);
                btn_new.Name = "btn_new" + i;
                //btn_new.Size = new System.Drawing.Size(268, 38);
                btn_new.Size = new System.Drawing.Size(500, 38);
                btn_new.TabIndex = 7;
                btn_new.Text = filenames[i];
                btn_new.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                btn_new.UseVisualStyleBackColor = false;
                btn_new.Click += new System.EventHandler(this.btn_new_Click);
                panel1.Controls.Add(btn_new);
            }

           

        }

        public void btn_new_Click(object sender, EventArgs e)
        {
            //is_set_ = true;
            Button btn = sender as Button;
            // 181107, andy
            // recipe_name_ = btn.Text;         
            // this.Close();


            select_recipeName = btn.Text;
            labelSelectedRecipeName.Text = select_recipeName;
            

        }
        public bool is_set
        {
            get { return this.is_set_; }
        }
        public string RecipeName
        {
            get { return recipe_name_; }
        }

        public void SetNowRecipeName(string nowRecipeName)
        {
            labelCurrentRecipeName.Text = nowRecipeName;
        }

        public string PartNumber
        {
            get { return textBox_PartNumber.Text; }
        }
        public void SetPartNumber(string nowPartNumber)
        {
            textBox_PartNumber.Text = nowPartNumber;
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            string newRecipeName = select_recipeName;
            if (recipe_name_ == newRecipeName || newRecipeName=="") return;

            is_set_ = true;
            recipe_name_ = newRecipeName;
            this.Close();
            MessageBox.Show("成功載入Recipe : " + newRecipeName);
        }

        private bool CheckSameRecipeName(string checkRecipeName)
        {
            string SaveAsParaPath = ModuleParamDirectory + RecipeFileDirectory;          
            string Dest = SaveAsParaPath + checkRecipeName;

            // Check 是否已有相同名稱程式
            if (Directory.Exists(Dest))
            {
                MessageBox.Show("已有相同的程式名稱，請重新命名!", "Error");
                return true;

            }
            else
            {

                return false;
            }
        }

        private void buttonSaveAs_Click(object sender, EventArgs e)
        {
            if (ModuleName == "")
            {
                MessageBox.Show("尚未載入程式!", "Error");
                return;
            }
            BtnLog.WriteLog(" [Recipe Manager] Save As Recipe Click");
            // ----------------------------------------------------------------------------
            using (WorkSheetForm form = new WorkSheetForm( ModuleName+"_new" ))
            {
                // Get recipe name
                form.ShowDialog();                  
                string SaveAsTemp = form.NewRecipeName;
                if (!form.Saved)
                {
                    return;
                }

                // user 按下取消鍵或未輸入時
                if (SaveAsTemp == "")
                {
                    MessageBox.Show("程式名稱不可有空白!", "Error");
                    return;
                }

                // User輸入空白字元時
                if (SaveAsTemp.IndexOf(" ") >= 0)
                {
                    MessageBox.Show("程式名稱不可有空白字元!", "Error");                   
                    return;
                }

                // Set path
                string SaveAsParaPath = ModuleParamDirectory + RecipeFileDirectory;
                string Src = SaveAsParaPath + ModuleName;
                string Dest = SaveAsParaPath + SaveAsTemp;

                // Check 是否已有相同名稱程式
                if (Directory.Exists(Dest))
                {
                    MessageBox.Show("已有相同的程式名稱，請重新命名!", "Error");
                    return;
                }
                BtnLog.WriteLog(" [Recipe Manager] Save As Recipe Name = " + SaveAsTemp);
                // 複製目錄
                clsStaticTool.DirectoryCopy(Src, Dest, true);
              
                // Add recipeNameList and Reload
                nowRecipeNameList.Add(SaveAsTemp);                
                create_btn(nowRecipeNameList);
                BtnLog.WriteLog(" [Recipe Manager] Save As Recipe Done");
            }

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            BtnLog.WriteLog(" [Recipe Manager] Save Recipe Btn Click");
            DialogResult dialogResult = MessageBox.Show("是否儲存當前參數?", "請確認是否執行 Yes / No ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult != DialogResult.Yes)
            {
                BtnLog.WriteLog(" [Recipe Manager] Save Recipe Btn Click Cencel");
                return;
            }
            // 存檔                
            OnUserControlButtonClicked_Save(sender, e);
            BtnLog.WriteLog(" [Recipe Manager] Save Recipe Btn Click Done");
            this.Close();
            MessageBox.Show("儲存成功!");
        }

        private void buttonModify_Click(object sender, EventArgs e)
        {
            if (ModuleName == "")
            {
                MessageBox.Show("尚未載入程式!", "Error");
                return;
            }

            string info = "";
            string[] lights_ = { "CL", "OL", "BL", "BL3" };
            using (InspectModelCreateForm create_form = new InspectModelCreateForm(lights_))
            {

                create_form.update_layout_value();
                create_form.ShowDialog();
                if (create_form.is_save())
                {
                    InspectModelCreateForm.clsRecipeParam Param = create_form.Get_Cuttent_Recipe_Param();
                    BtnLog.WriteLog(" [Recipe Manager] ================= Fix Param =================");
                    BtnLog.WriteLog(" [Recipe Manager] ModeName = " + Param.ModeName);
                    BtnLog.WriteLog(" [Recipe Manager] LocationNum = " + Param.LocationNum);
                    BtnLog.WriteLog(" [Recipe Manager] RowNum = " + Param.RowNum);
                    BtnLog.WriteLog(" [Recipe Manager] ColumnNum = " + Param.ColumnNum);
                    BtnLog.WriteLog(" [Recipe Manager] Bypass = " + Param.Bypass);
                    BtnLog.WriteLog(" [Recipe Manager] light_cnt = " + Param.light_cnt);
                    BtnLog.WriteLog(" [Recipe Manager] ProductName = " + Param.ProductName); // (20200429) Jeff Revised!
                    BtnLog.WriteLog(" [Recipe Manager] VisualPosEnable = " + Param.VisualPosEnable.ToString()); // (20191214) Jeff Revised!
                    BtnLog.WriteLog(" [Recipe Manager] CellCenterInspectEnable = " + Param.CellCenterInspectEnable.ToString());
                    BtnLog.WriteLog(" [Recipe Manager] GlobalMapImgID = " + Param.GlobalMapImgID);
                    BtnLog.WriteLog(" [Recipe Manager] NGRatio = " + Param.NGRatio);

                    // 新增檢測模組 (其他參數) (20190719) Jeff Revised!
                    BtnLog.WriteLog(" [Recipe Manager] PosImgID = " + Param.PosImgID);
                    BtnLog.WriteLog(" [Recipe Manager] b_NG_classification = " + Param.b_NG_classification.ToString());
                    BtnLog.WriteLog(" [Recipe Manager] b_NG_priority = " + Param.b_NG_priority.ToString());
                    BtnLog.WriteLog(" [Recipe Manager] b_Recheck = " + Param.b_Recheck.ToString());
                    BtnLog.WriteLog(" [Recipe Manager] dispWindow = " + Param.dispWindow);
                    BtnLog.WriteLog(" [Recipe Manager] b_saveCSV = " + Param.b_saveCSV.ToString());
                    BtnLog.WriteLog(" [Recipe Manager] b_SaveDefectResult = " + Param.b_SaveDefectResult.ToString());
                    BtnLog.WriteLog(" [Recipe Manager] b_SaveAllCellImage = " + Param.b_SaveAllCellImage.ToString());
                    BtnLog.WriteLog(" [Recipe Manager] b_SaveDefectCellImage = " + Param.b_SaveDefectCellImage.ToString());
                    BtnLog.WriteLog(" [Recipe Manager] Directory_SaveDefectResult = " + Param.Directory_SaveDefectResult); // (20200429) Jeff Revised!
                    BtnLog.WriteLog(" [Recipe Manager] b_Auto_AB = " + Param.b_Auto_AB.ToString()); // (20200429) Jeff Revised!
                    BtnLog.WriteLog(" [Recipe Manager] A_Recipe = " + Param.A_Recipe); // (20200429) Jeff Revised!
                    BtnLog.WriteLog(" [Recipe Manager] B_UpDownInvert_FS = " + Param.B_UpDownInvert_FS.ToString()); // (20200429) Jeff Revised!
                    BtnLog.WriteLog(" [Recipe Manager] B_LeftRightInvert_FS = " + Param.B_LeftRightInvert_FS.ToString()); // (20200429) Jeff Revised!
                    BtnLog.WriteLog(" [Recipe Manager] Directory_defComb = " + Param.Directory_defComb); // (20200429) Jeff Revised!
                    BtnLog.WriteLog(" [Recipe Manager] OutputDirectory_defComb = " + Param.OutputDirectory_defComb); // (20200429) Jeff Revised!

                    BtnLog.WriteLog(" [Recipe Manager] b_AICellImg_Enable = " + Param.b_AICellImg_Enable.ToString()); // (20190830) Jeff Revised!

                    // (20190902) Jeff Revised!
                    BtnLog.WriteLog(" [Recipe Manager] DAVSImgID = " + Param.DAVSImgID.ToString());
                    BtnLog.WriteLog(" [Recipe Manager] b_DAVSImgAlign = " + Param.b_DAVSImgAlign.ToString());
                    BtnLog.WriteLog(" [Recipe Manager] b_DAVSMixImgBand = " + Param.b_DAVSMixImgBand.ToString());
                    BtnLog.WriteLog(" [Recipe Manager] DAVSBand1ImgIndex = " + Param.DAVSBand1ImgIndex.ToString());
                    BtnLog.WriteLog(" [Recipe Manager] DAVSBand2ImgIndex = " + Param.DAVSBand2ImgIndex.ToString());
                    BtnLog.WriteLog(" [Recipe Manager] DAVSBand3ImgIndex = " + Param.DAVSBand3ImgIndex.ToString());
                    BtnLog.WriteLog(" [Recipe Manager] DAVSBand1 = " + Param.DAVSBand1.ToString());
                    BtnLog.WriteLog(" [Recipe Manager] DAVSBand2 = " + Param.DAVSBand2.ToString());
                    BtnLog.WriteLog(" [Recipe Manager] DAVSBand3 = " + Param.DAVSBand3.ToString());

                    BtnLog.WriteLog(" [Recipe Manager] ================= Fix Param =================");
                    // ...                   
                }
            }
        }


        private void buttonNew_Click(object sender, EventArgs e)
        {
            BtnLog.WriteLog(" [Recipe Manager] Create Recipe Btn Click");
            // 181026, andy
            using (WorkSheetForm form = new WorkSheetForm(""))
            {

                form.ShowDialog();

                if (form.NewRecipeName == "" || CheckSameRecipeName(form.NewRecipeName))
                {
                    BtnLog.WriteLog(" [Recipe Manager] Create Recipe Btn Cencel");
                    return;
                }

                // 為了能存檔 !!!!!!!!!!!!!!!!!!!!!!!!!!!!
                ModuleName = form.NewRecipeName;
                BtnLog.WriteLog(" [Recipe Manager] Create Recipe Name : " + ModuleName);
                // 存檔                
                OnUserControlButtonClicked_SaveAs(sender, e);

                // Add recipeNameList
                nowRecipeNameList.Add(form.NewRecipeName);

                // Reload
                create_btn(nowRecipeNameList);

                BtnLog.WriteLog(" [Recipe Manager] Create Recipe Done");
            }
        }
      
        private void buttonRemove_Click(object sender, EventArgs e)
        {
            // Check
            if (nowRecipeNameList.Count <= 1 || ModuleName == select_recipeName || select_recipeName=="")
            {
                MessageBox.Show("刪除操作異常:\n 1.尚未選擇程式\n 2.不可刪除目前程式\n 3.程式總數量小於一筆", "Error");
                return;
            }
            BtnLog.WriteLog(" [Recipe Manager] Remove Recipe Click");
            // Get name
            string SaveAsParaPath = ModuleParamDirectory + RecipeFileDirectory;
            string DeletePara = SaveAsParaPath + select_recipeName;


            // 刪除到回收站
            if (MessageBox.Show("您是否確定要刪除此程式？\n" + select_recipeName, "Warning!", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                BtnLog.WriteLog(" [Recipe Manager] Remove Recipe Cancel");
                return;
            }


            BtnLog.WriteLog(" [Recipe Manager] Remove Recipe Name = " + select_recipeName);
            Directory.Delete(DeletePara, true); // true表示指定目錄與其子目錄一起刪除

            // Re Show    
            nowRecipeNameList.Remove(select_recipeName);
            panel1.Controls.Clear();
            create_btn(nowRecipeNameList);
            BtnLog.WriteLog(" [Recipe Manager] Remove Recipe Done");
        }

        #region 檔案操作



        #endregion

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txbSerchRecipeName.Text))
            {
                panel1.Controls.Clear();
                create_btn(nowRecipeNameList);
                return;
            }

            List<string> SearchList = new List<string>();
            string SearchName = txbSerchRecipeName.Text;
            for (int i = 0; i < nowRecipeNameList.Count; i++)
            {
                if (!nowRecipeNameList[i].Contains(SearchName))
                {
                    continue;
                }
                SearchList.Add(nowRecipeNameList[i]);
            }

            panel1.Controls.Clear();
            create_btn(SearchList);
        }
    }
}
