using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DeltaSubstrateInspector.FileSystem.FileSystem;
using HalconDotNet;
using System.Drawing;
using DeltaSubstrateInspector.FileSystem;

namespace DeltaSubstrateInspector.src.FileSystem.StaticFile
{
    public class CSVFileManager : FileManager
    {
        private string header_;
        private string start_time_ = "";
        private string end_time_ = "";
        private int CellCount = 0;

        public CSVFileManager()
        {
            this.type_ = "csv";
        }

        override public void set_filename(int chip_id)
        {
            string folder = ModuleParamDirectory + HistoryDirectory + DateTime.Now.ToString("yyyyMMdd") + "\\" + PartNumber + "\\";
           

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            //this.filename_ = folder + ModuleName + "-" + chip_id.ToString() + ".csv";
            this.filename_ = folder + PartNumber + "_Result.csv";
        }

        override public void set_filemap() { }

        override public void format_setting() { }

        override public void save_data(string[] data)
        {
            FileStream fs = null;
            StreamWriter sw = null;
            StreamReader SFile = null;
            try
            {
                clsStaticTool.CloseLibreOffice();
                bool IsFirst = false;
                if (!File.Exists(this.filename_))
                {
                    fs = new FileStream(this.filename_, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                    sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    IsFirst = true;
                }
                else
                {
                    fs = new FileStream(this.filename_, System.IO.FileMode.Append, System.IO.FileAccess.Write);
                    sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    IsFirst = false;
                }

                string result = "";

                DateTime S = DateTime.Parse(start_time_);
                DateTime E = DateTime.Parse(end_time_);
                //string Spain = E.Subtract(S).ToString(@"mm\:ss");
                string Spain = E.Subtract(S).TotalMinutes.ToString("#.##");

                result = Date + "," + LotID + "," + ModuleName + "," + CellCount + "," + ID + "," + start_time_ + "," + end_time_ + "," + Spain + ",";

                if (IsFirst)
                    sw.WriteLine(header_);

                // Set infomation
                for (int i = 0; i < data.Length; i++)
                {
                    result = result + data[i];
                    if (i != data.Length - 1)
                        result += ",";
                }

                sw.WriteLine(result);

                sw.Close();
                fs.Close();

                #region Header Fix

                if (!IsFirst)
                {
                    SFile = new StreamReader(this.filename_, Encoding.Default);
                    string HString = SFile.ReadLine();
                    string[] HeaderArray = HString.Split(',');
                    string[] THeaderString = header_.Split(',');
                    if (SFile != null)
                        SFile.Close();
                    if (HeaderArray.Length < THeaderString.Length)
                    {
                        var lines = File.ReadLines(this.filename_, Encoding.Default).ToList();
                        lines[0] = header_;
                        File.WriteAllLines(this.filename_, lines, Encoding.Default);
                        lines.DisposeAll();
                    }
                }

                #endregion

            }
            catch
            {
                if (sw != null)
                    sw.Close();
                if (fs != null)
                    fs.Close();
                if (SFile != null)
                    SFile.Close();
            }

            //try
            //{
            //    //FileStream fs = new FileStream(this.filename_, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            //    //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            //    FileStream fs;
            //    StreamWriter sw;
            //    bool IsFirst = false;
            //    if (!File.Exists(this.filename_))
            //    {
            //        fs = new FileStream(this.filename_, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            //        sw = new StreamWriter(fs, System.Text.Encoding.Default);
            //        IsFirst = true;
            //    }
            //    else
            //    {
            //        fs = new FileStream(this.filename_, System.IO.FileMode.Append, System.IO.FileAccess.Write);
            //        sw = new StreamWriter(fs, System.Text.Encoding.Default);
            //        IsFirst = false;
            //    }

            //    string result = "";
            //    //result = ModuleName + "," + ModuleType + "," + start_time_ + "," + end_time_ + ","; //181026,andy

            //    result = ModuleName + "," + ID + "," + start_time_ + "," + end_time_ + ",";

            //    // Set header
            //    //for (int i = 0; i < header_.Length; i++)
            //    //{
            //    //    result = result + header_[i];
            //    //    if (i != header_.Length - 1)
            //    //        result += ",";
            //    //}
            //    if (IsFirst)
            //        sw.WriteLine(header_);

            //    // Set infomation
            //    for (int i = 0; i < data.Length; i++)
            //    {
            //        result = result + data[i];
            //        if (i != data.Length - 1)
            //            result += ",";
            //    }
            //    sw.WriteLine(result);

            //    sw.Close();
            //    fs.Close();
            //    // MessageBox.Show("CSV文件保存成功！"); // 181026, andy
            //}
            //catch
            //{
            //    // MessageBox.Show("CSV文件保存未成功！");
            //}

        }

        public void set_time(string str_time, string end_time)
        {
            start_time_ = str_time;
            end_time_ = end_time;
        }
        string ID = "";
        public void SetSB_ID(string pmID)
        {
            this.ID = pmID;
        }
        public void set_header(string header) // (20190720) Jeff Revised!
        {
            {
                header_ = "日期," + "批號," + "參數," + "排板顆," + "序號," + "起始時間," + "結束時間," + "總時間," + "運行結果," + "判定結果," + "良率%," + "異常總數,";
                header_ += header;
            }
            //if (!File.Exists(this.filename_))
            //{
            //    header_ = "工單號碼," + "序號," + "檢驗起始時間," + "檢驗結束時間," + "運行結果," + "判定結果," + "良率%," + "異常總數,";
            //    header_ += header;
            //}
            //else // (20190720) Jeff Revised!
            //{

            //}
        }
        public void SetCellCount(int Count)
        {
            this.CellCount = Count;
        }

        string LotID = "Default_1234";
        string Date = "1990-1-1";
        public void SetDataParam(string pmLotID, string pmDate)
        {
            this.LotID = pmLotID;
            this.Date = pmDate;
        }

        public class clsSummaryDefect
        {
            public string Name = "";
            public int DefectCount = 0;
            public List<Point> Defect_Index;
            public HashSet<Point> Real_Defect_Index;

            public clsSummaryDefect(string pmName)
            {
                this.Name = string.Copy(pmName);
                this.Defect_Index = new List<Point>();
            }
        }

        public void Set_Defect_header(Modules.Chip current_chip_,ref string[] last_info)
        {
            //Chris 20190425
            List<Modules.ResultModule.Record> RecoreList = current_chip_.GetRecordList();
            List<Modules.Defect> defectList = new List<Modules.Defect>();
            List<clsSummaryDefect> SumList = new List<clsSummaryDefect>();

            if (RecoreList.Count > 0)
            {
                string AddHeader = "";
                foreach (string key in RecoreList[0].DicDefectLst.Keys)
                {
                    if (RecoreList[0].DicDefectLst[key].Count > 0)
                    {
                        foreach (var _defectList in RecoreList[0].DicDefectLst[key])
                        {
                            if (!_defectList.B_defect)
                                continue;
                            clsSummaryDefect Type = new clsSummaryDefect(_defectList.DefectName);
                            SumList.Add(Type);
                            defectList.Add(_defectList);
                        }
                    }
                }

                for (int i = 0; i < defectList.Count; i++)
                {
                    AddHeader += defectList[i].DefectName + ",";
                }
                //header_ = header_ + "產品總顆數,";
                header_ = header_ + AddHeader;


                for (int i = 0; i < RecoreList.Count; i++)
                {
                    foreach (string key in RecoreList[i].DicDefectLst.Keys)
                    {
                        if (RecoreList[i].DicDefectLst[key].Count > 0)
                        {
                            foreach (var _defectList in RecoreList[i].DicDefectLst[key])
                            {
                                if (!_defectList.B_defect)
                                    continue;
                                for (int j = 0; j < SumList.Count; j++)
                                {
                                    if (_defectList.DefectName == SumList[j].Name)
                                    {
                                        for (int k = 0; k < _defectList.Location.Count; k++)
                                        {
                                            SumList[j].Defect_Index.Add(_defectList.Location[k]);
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                for (int i = 0; i < SumList.Count; i++)
                {
                    SumList[i].Real_Defect_Index = new HashSet<Point>(SumList[i].Defect_Index);
                }
                int ArrayIndex = last_info.Length;
                System.Array.Resize(ref last_info, last_info.Length + SumList.Count);
                for (int i = 0; i < SumList.Count; i++)
                {
                    last_info[ArrayIndex] = SumList[i].Real_Defect_Index.Count.ToString();
                    ArrayIndex++;
                }

            }

            defectList.Clear();
            defectList = null;
            RecoreList.Clear();
            RecoreList = null;
            SumList.Clear();
            SumList = null;
            ////Chris 20190425
            //List<Modules.ResultModule.Record> RecoreList = current_chip_.GetRecordList();
            //List<Modules.Defect> defectList = new List<Modules.Defect>();
            //List<clsSummaryDefect> SumList = new List<clsSummaryDefect>();

            //if (RecoreList.Count > 0)
            //{
            //    string AddHeader = "";
            //    foreach (string key in RecoreList[0].DicDefectLst.Keys)
            //    {
            //        if (RecoreList[0].DicDefectLst[key].Count > 0)
            //        {
            //            foreach (var _defectList in RecoreList[0].DicDefectLst[key])
            //            {
            //                clsSummaryDefect Type = new clsSummaryDefect(_defectList.DefectName);
            //                SumList.Add(Type);
            //                defectList.Add(_defectList);
            //            }
            //        }
            //    }

            //    for (int i = 0; i < defectList.Count; i++)
            //    {
            //        AddHeader += defectList[i].DefectName + ",";
            //    }

            //    // CSV欄位標題 (第一列)
            //    header_ = header_ + AddHeader;

            //    for (int i = 0; i < RecoreList.Count; i++) // RecoreList.Count: 總取像位置
            //    {
            //        foreach (string key in RecoreList[i].DicDefectLst.Keys)
            //        {
            //            if (RecoreList[i].DicDefectLst[key].Count > 0)
            //            {
            //                foreach (var _defectList in RecoreList[i].DicDefectLst[key])
            //                {
            //                    for (int j = 0; j < SumList.Count; j++)
            //                    {
            //                        if (_defectList.DefectName == SumList[j].Name)
            //                        {
            //                            for (int k = 0; k < _defectList.Location.Count; k++)
            //                            {
            //                                SumList[j].Defect_Index.Add(_defectList.Location[k]);
            //                            }
            //                            break;
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }

            //    for (int i = 0; i < SumList.Count; i++)
            //    {
            //        SumList[i].Real_Defect_Index = new HashSet<Point>(SumList[i].Defect_Index);
            //    }
            //    int ArrayIndex = last_info.Length;
            //    System.Array.Resize(ref last_info, last_info.Length + SumList.Count);
            //    for (int i = 0; i < SumList.Count; i++)
            //    {
            //        last_info[ArrayIndex] = SumList[i].Real_Defect_Index.Count.ToString();
            //        ArrayIndex++;
            //    }

            //}

            //defectList.Clear();
            //defectList = null;
            //RecoreList.Clear();
            //RecoreList = null;
            //SumList.Clear();
            //SumList = null;
        }

        /// <summary>
        /// 如果瑕疵類型變更，存取時會有問題!
        /// </summary>
        /// <param name="current_chip_"></param>
        /// <param name="last_info"></param>
        public void Set_Defect_header_New(Modules.Chip current_chip_, ref string[] last_info) // (20190720) Jeff Revised!
        {
            if (!FinalInspectParam.b_NG_classification)
                return;

            Dictionary<string, Modules.DefectsResult>DefectsClassify = current_chip_.Get_DefectsClassify;
            List<clsSummaryDefect> SumList = new List<clsSummaryDefect>();

            if (DefectsClassify.Count == 0)
                return;

            string AddHeader = "";
            if (header_ != null)
            {
                int index = 0;
                foreach (string key in DefectsClassify.Keys)
                {
                    clsSummaryDefect Type = new clsSummaryDefect(key);
                    SumList.Add(Type);
                    // 紀錄各類型瑕疵名稱
                    AddHeader += key + ",";

                    // 紀錄各類型瑕疵所在座標
                    if (FinalInspectParam.b_NG_priority)
                        SumList[index++].Defect_Index = DefectsClassify[key].all_defect_id_Priority;
                    else
                        SumList[index++].Defect_Index = DefectsClassify[key].all_defect_id_Origin;
                }
            }

            // CSV欄位標題 (第一列)
            header_ = header_ + AddHeader;
                
            for (int i = 0; i < SumList.Count; i++)
            {
                SumList[i].Real_Defect_Index = new HashSet<Point>(SumList[i].Defect_Index);
            }
            int ArrayIndex = last_info.Length;
            System.Array.Resize(ref last_info, last_info.Length + SumList.Count);
            for (int i = 0; i < SumList.Count; i++)
            {
                last_info[ArrayIndex] = SumList[i].Real_Defect_Index.Count.ToString();
                ArrayIndex++;
            }

        }

    }
}
