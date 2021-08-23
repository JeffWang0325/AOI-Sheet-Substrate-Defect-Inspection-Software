using HalconDotNet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extension;
using System.Windows.Forms;
using DeltaSubstrateInspector.src.FileSystem.StaticFile;
using System.Drawing;

using System.Windows.Forms;
using static DeltaSubstrateInspector.FileSystem.FileSystem;

namespace DeltaSubstrateInspector.src.Modules.ResultModule
{
    public class ResultManager
    {
        private delegate bool DelegateDrawInspectionResult(DataGridView pboard);
        private DelegateDrawInspectionResult delegateDrawInspectionResult;


        private CSVFileManager csvfile_manager = new CSVFileManager();
        private StaticBoard static_board_ = new StaticBoard();
        private Chip current_chip_ = new Chip(0); // (20190902) Jeff Revised!
        private static int chip_count_ = 0; // (20190902) Jeff Revised!
        private int move_bound_ = 0;
        private string end_time_ = "";
        private string start_time_ = "";
        /// <summary>
        /// 基板總Cell顆數
        /// </summary>
        private int total = 0;
        private string[] last_info;
        private int current_defect_count = 0; // 目前瑕疵總Cell顆數 (20181119) Jeff Revised!
        private int CellCount = 0;
        private int MatchFailCount = 0;
        /// <summary>
        /// 運行結果
        /// </summary>
        private string current_Process_result { get; set; } = "";
        /// <summary>
        /// 判定結果
        /// </summary>
        private string current_Work_result = "";

        public static void ResetInerChipCount()
        {
            chip_count_ = 1;
            SB_InerCountID = String.Format("{0:00000000}", chip_count_);
        }

        public void store_up_chip() // (20190902) Jeff Revised!
        {
            // Staticboard create new row

            //chip_result.Enqueue(current_chip_);
            end_time_ = DateTime.Now.ToString("hh:mm:ss");
            save_static_file();

            //Set_InerCountID(); // (20190902) Jeff Revised!

            current_chip_.release_all();
            current_chip_ = null;
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// 設定基板序號 (內部計數ID模式)
        /// </summary>
        public void Set_InerCountID() // (20190902) Jeff Revised!
        {
            chip_count_ += 1;
            // reset counter
            if (chip_count_ >= 99999999)
            {
                chip_count_ = 1;
            }
            // Set iner count ID
            SB_InerCountID = String.Format("{0:00000000}", chip_count_);
        }

        public void SetMatchFailCount(int Count)
        {
            this.MatchFailCount = Count;
        }

        public void new_chip()
        {
            current_chip_ = new Chip(chip_count_);
            static_board_.new_row();
            start_time_ = DateTime.Now.ToString("hh:mm:ss");
            MatchFailCount = 0;
            current_Process_result = "";
            current_Work_result = "";
        }

        public void setup_static_board(DataGridView board)
        {
            static_board_.set_table_ref(board);       

        }

     
        public void setup_static_board_head(List<string> header_list)
        {
            static_board_.set_header(header_list);
        }

        public void setup_static_board_total(int val)
        {
            static_board_.set_total(val);
            total = val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="images"></param>
        /// <param name="defects"></param>
        /// <param name="defect_locate_with_name">該位置瑕疵id座標</param>
        /// <param name="dic_defects"></param>
        public void update_chip(ImageObj images, List<Defect> defects, Dictionary<string, List<Point>> defect_locate_with_name, Dictionary<string, List<Point>> All_defect_locate, Dictionary<string, List<Defect>> dic_defects = null) // (20190718) Jeff Revised!
        {
            //if (images.MoveIndex % move_bound_ == 1)
            //    current_chip_ = new Chip(chip_count_);
            for (int i = 0; i < defects.Count; i++)
            {
                defects[i].Location = defect_locate_with_name[defects[i].DefectName];
                defects[i].set_up_rgn();
            }

            if (dic_defects == null)
                current_chip_.update(images, defects);
            else
            {
                foreach (string key in dic_defects.Keys)
                {
                    for (int i = 0; i < dic_defects[key].Count; i++)
                    {
                        if (!dic_defects[key][i].B_defect)
                            continue;
                        dic_defects[key][i].Location = All_defect_locate[dic_defects[key][i].DefectName];
                    }
                }
                current_chip_.update(images, defects, dic_defects);
            }

            update_board();
        }

        public void update_chip(ImageObj images, List<Defect> defects, Dictionary<string, List<Point>> defect_locate_with_name, Dictionary<string, List<Defect>> dic_defects, Dictionary<string, List<Point>> All_defect_locate)
        {

            //if (images.MoveIndex % move_bound_ == 1)
            //    current_chip_ = new Chip(chip_count_);
            for (int i = 0; i < defects.Count; i++)
            {
                defects[i].Location = defect_locate_with_name[defects[i].DefectName];
                defects[i].set_up_rgn();
            }
            //Chris 20190425
            foreach (string key in dic_defects.Keys)
            {
                for (int i = 0; i < dic_defects[key].Count; i++)
                {
                    dic_defects[key][i].Location = All_defect_locate[dic_defects[key][i].DefectName];
                }
            }

            current_chip_.update(images, defects, dic_defects);
            update_board();
        }

        /// <summary>
        /// 更新瑕疵分類
        /// </summary>
        /// <param name="defect_count_table"></param>
        public void update_DefectsClassify(Dictionary<string, DefectsResult> defectsClassify) // (20190720) Jeff Revised!
        {
            current_chip_.update_DefectsClassify(defectsClassify);
        }

        /// <summary>
        /// 目前瑕疵總Cell顆數
        /// </summary>
        /// <param name="val"></param>
        public void update_current_defect_count(int val) // (20181119) Jeff Revised!
        {
            current_defect_count = val;
            
            // !!!! 181213, andy !!!!! 
            update_board();

        }

        public void updat_currenct_result(string _current_process_result, string _current_work_result)
        {
            current_Process_result = _current_process_result;
            current_Work_result = _current_work_result;

            // !!!! 181213, andy !!!!! 
            update_board();

        }

        public HObject get_last_defect_img(int move_index)
        {
            return current_chip_.get_TotalDefect_region_image(move_index);
        }

        public HObject get_last_defect(int move_index) // (20190718) Jeff Revised!
        {
            return current_chip_.get_TotalDefect_region(move_index);
        }

        public List<Defect> get_defect_rgn(int move_index)
        {
            if (move_index == move_bound_)
                return current_chip_.get_defect_rgns(move_bound_);
            return current_chip_.get_defect_rgns(move_index);
        }

        public Dictionary<string, List<Defect>> get_dic_defect(int move_index)
        {
            if (move_index == move_bound_)
                return current_chip_.get_dic_defect(move_bound_);
            return current_chip_.get_dic_defect(move_index);

        }

        public void SetCellCount(int Count)
        {
            this.CellCount = Count;
        }

        public void save_static_file()
        {
            try
            {
                csvfile_manager.set_filename(current_chip_.Id);
                csvfile_manager.set_header("定位異常,");
                csvfile_manager.Set_Defect_header(current_chip_, ref last_info);
                csvfile_manager.set_time(start_time_, end_time_);
                csvfile_manager.SetCellCount(CellCount);
                if (!B_SB_InerCountID)
                    csvfile_manager.SetSB_ID(SB_ID);
                else
                    csvfile_manager.SetSB_ID(SB_InerCountID);
                csvfile_manager.SetDataParam(PartNumber, DateTime.Now.ToString("yyyy-MM-dd"));

                csvfile_manager.save_data(last_info);
            }
            catch (Exception Ex)
            {
                
            }

            //csvfile_manager.set_filename(current_chip_.Id);
            ////csvfile_manager.set_header(static_board_.get_header());
            //csvfile_manager.set_header("瑕疵種類,");
            ////csvfile_manager.Set_Defect_header(current_chip_, ref last_info);
            //csvfile_manager.Set_Defect_header_New(current_chip_, ref last_info); // (20190720) Jeff Revised!
            //csvfile_manager.set_time(start_time_, end_time_);
            //csvfile_manager.SetSB_ID(SB_ID);

            ////string[] header = static_board_.HeaderNames;
            ////for (int i = 0; i < header.Length; i++)
            ////{
            ////    header[i] = current_chip_.get_defect_count(header[i]).ToString();
            ////}

            //csvfile_manager.save_data(last_info);
        }

        private void update_board()
        {

            string[] header = static_board_.HeaderNames;
            int defect_total = 0;

            for (int i = 0; i < header.Length; i++)
            {
                // defect_total += current_chip_.get_defect_count(header[i]);
                // 避免瑕疵統計數量重複計算之問題 (不同位置但是同一顆cell) (20181119) Jeff Revised!
                defect_total = current_defect_count;
            }

            List<string> info = new List<string>();

            // 增加 處理結果與判定結果
            info.Add(current_Process_result);
            info.Add(current_Work_result);


            // 增加 良率
            if (current_Process_result == "FAIL")
            {
                total = 0;
                defect_total = 0;
            }
            double goodRatio = (total == 0) ? 0 : (((double)(total - defect_total) / total) * 100);
            string goodRatioStr = goodRatio.ToString("0.00");
            info.Add(goodRatioStr);
            //info.Add(Math.Round(((double)(total - defect_total) / total),3).ToString());

            // 增加 NG顆數
            info.Add(defect_total.ToString());
            for (int i = 0; i < header.Length; i++)
            {
                // 181213, andy
                //info.Add( current_chip_.get_defect_count(header[i]).ToString());
                info.Add(MatchFailCount.ToString());
            }
            //info.Add(total.ToString());

            // For excel save file content
            last_info = info.ToArray();

            // Update board
            static_board_.update_board(info.ToArray());


            //string[] header = static_board_.HeaderNames;
            //int defect_total = 0;

            //for (int i = 0; i < header.Length; i++)
            //{
            //    // defect_total += current_chip_.get_defect_count(header[i]);
            //    // 避免瑕疵統計數量重複計算之問題 (不同位置但是同一顆cell) (20181119) Jeff Revised!
            //    defect_total = current_defect_count;
            //}

            //List<string> info = new List<string>();

            //// 增加 處理結果與判定結果
            //info.Add(current_Process_result);
            //info.Add(current_Work_result);

            //// 增加 良率
            //if (current_Process_result == "FAIL")
            //{
            //    total = 0;
            //    defect_total = 0;
            //}
            //double goodRatio = (total == 0) ? 0 : (((double)(total - defect_total) / total) * 100);
            //string goodRatioStr = goodRatio.ToString("0.00");
            //info.Add(goodRatioStr);
            ////info.Add(Math.Round(((double)(total - defect_total) / total),3).ToString());

            //// 增加 NG顆數
            //info.Add(defect_total.ToString());
            //for (int i = 0; i < header.Length; i++)
            //{
            //    // 181213, andy
            //    //info.Add( current_chip_.get_defect_count(header[i]).ToString());
            //    info.Add("保留");
            //}

            //// For excel save file content
            //last_info = info.ToArray();

            //// Update board
            //static_board_.update_board(info.ToArray());

        }

        public DataGridView StaticTable
        {
            get { return static_board_.ResultTable; }
        }

        public int MoveBound
        {
            set { move_bound_ = value; }
        }
    }
}
