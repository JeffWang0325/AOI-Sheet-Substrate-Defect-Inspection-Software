using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;

using static DeltaSubstrateInspector.FileSystem.FileSystem; // (20190424) Jeff Revised!

namespace DeltaSubstrateInspector.src.Modules.ResultModule
{
    public class StaticBoard
    {
        #region Andy
        public delegate void DelegateDisplay(DataGridView ptable);
        public DelegateDisplay delegateDisplay;
        #endregion

        private DataGridView table_;
        private List<string> header_names_ = new List<string>();
        /// <summary>
        /// 基板總Cell顆數
        /// </summary>
        private int total_cell_count = 0;
        private int bound = 10;

        private Mutex saveImageMutex = new Mutex();

        public StaticBoard()
        {

        }
       
        public void set_table_ref(DataGridView table)
        {
            table_ = table;

            
        }

        public void set_header(List<string> header_name_lst)
        {
            header_names_.Clear();
            for (int i = 0; i < header_name_lst.Count; i++)
            {
                string title = header_name_lst[i];
                table_.Columns[i + 5].HeaderText = "定位異常";
                //table_.Columns[i + 5].HeaderText = title;
                header_names_.Add(title);
            }
        }

        public void set_total(int value)
        {
            this.total_cell_count = value;
        }

        public string get_header()
        {
            string result = "";
            for (int i = 0; i < header_names_.Count; i++)
            {
                result = result + header_names_[i];
                if (i != header_names_.Count - 1)
                    result += ",";
            }
            return result;
        }

        public void update_board(string[] info)
        {            
            try
            {
                for (int i = 0; i < info.Length; i++)
                {
                    table_.Rows[table_.RowCount-1].Cells[1+i].Value = info[i];
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show("I got u 1: " + e.ToString());
            }

            table_.BackgroundColor = System.Drawing.Color.FromArgb(254,254,255);
        }

        public void new_row()
        {
            saveImageMutex.WaitOne();

            try
            {
                
                //string id = String.Format("{0:00000000}", table_.RowCount + 1);
                //table_.Rows.Add(id, "");
                //table_.Rows.Add(table_.RowCount+1, "");
                // 判斷是否啟用內部計數ID模式 (20190424) Jeff Revised!
                string SBID = "";
                if (B_SB_InerCountID)
                    SBID = SB_InerCountID;
                else
                    SBID = SB_ID;
                table_.Rows.Add(SBID, "");

                // 自動捲軸
                if (table_.RowCount > 0)
                    table_.CurrentCell = table_[0, table_.RowCount - 1];

                //delegateDisplay(table_);

            }
            catch(Exception e)
            {
                // MessageBox.Show("I got u 2: " + e.ToString());
            }

            saveImageMutex.ReleaseMutex();

        }

        public string[] get_info()
        {
            string[] info_row = new string[table_.ColumnCount];
            for (int i = 0; i < table_.ColumnCount; i++)
            {
                if (table_.Rows[table_.RowCount - 1].Cells[i].Value != "")
                     info_row[i] = table_.Rows[table_.RowCount - 1].Cells[i].Value.ToString();
            }
            return info_row;
        }

        public DataGridView ResultTable
        {
            get { return table_; }
        }

        public string[] HeaderNames
        {
            get { return header_names_.ToArray(); }
        }
    }
}
