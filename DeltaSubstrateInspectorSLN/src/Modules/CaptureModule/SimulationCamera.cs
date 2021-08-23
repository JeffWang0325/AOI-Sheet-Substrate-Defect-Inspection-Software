using System.Windows.Forms;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extension;
using System.Collections.Concurrent;
using DeltaSubstrateInspector.src.Modules.MotionModule; // (20181009) Jeff Revised!
using System.Threading; // (20181009) Jeff Revised!

using static DeltaSubstrateInspector.FileSystem.FileSystem;

namespace DeltaSubstrateInspector.src.Modules.CaptureModule
{
    public class SimulationCamera : CameraModule
    {
        private List<string> image_files_name_list_;
        private string file_name_ = "";
        private int start_index = 0;
        private int end_index = 0;
        private string simulation_type_ = "";
        Thread thread_; // (20181009) Jeff Revised!

        public SimulationCamera(string type)
        {
            image_files_name_list_ = new List<string>();
            simulation_type_ = type;

            //LightCount = 2; // Trigger Time(Number) for Each Cell (同一位置拍攝影像張數) (20181009) Jeff Revised!
            LightCount = 2; // 181127, andy 初始化

          
            //MovementCount = 65; //2018.05.30
            MovementCount = MovementPos.col * MovementPos.row + MovementPos.locat_pos; // (20181009) Jeff Revised!
        }

        public volatile static string FS = "";

        public override void run()
        {
            if (simulation_type_ == "Cycle")
            {
                #region 循環測試

                load_imgs_from_filemap(FS);

                MovementCount = MovementPos.col * MovementPos.row + MovementPos.locat_pos;
                LightCount = MovementPos.triggerCount;

                start_index = 1;
                end_index = MovementCount;

                thread_ = new Thread(new ThreadStart(create_img_obj));
                thread_.Start();
                #endregion
            }
            else if (simulation_type_ == "Mult")
            {
                #region 多組測試
                load_imgs_from_filemap(FS);

                MovementCount = MovementPos.col * MovementPos.row + MovementPos.locat_pos;
                LightCount = MovementPos.triggerCount;

                start_index = 1;
                end_index = MovementCount;

                thread_ = new Thread(new ThreadStart(create_img_obj));
                thread_.Start();
                #endregion
            }
            else
            {
                #region 離線測試
                string fs = file_select();
                if (fs == "") return;
                load_imgs_from_filemap(fs);

                MovementCount = MovementPos.col * MovementPos.row + MovementPos.locat_pos; // 避免離線測試跑完時按下確定鍵，瑕疵地圖會消失之問題! (20181009) Jeff Revised!
                LightCount = MovementPos.triggerCount; // 181127, andy

                if (simulation_type_ == "All")
                {
                    start_index = 1;
                    end_index = MovementCount;
                }
                else
                {
                    start_index = get_move_index();
                    end_index = start_index;
                    MovementCount = start_index;
                }

                //create_img_obj();
                // (20181009) Jeff Revised!
                thread_ = new Thread(new ThreadStart(create_img_obj));
                thread_.Start();
                #endregion
            }
        }

        public override void create_img_obj()
        {
            for (int i = start_index; i <= end_index; i++)
            {
                ImageObj img_obj = new ImageObj();
                
                img_obj.MoveIndex = i;
                for (int light_index = 1; light_index < LightCount + 1; light_index++)
                {
                    HObject img = null;
                    string file_name = get_file_name(i,light_index);
                    //HOperatorSet.GenEmptyObj(out img);

                    var name_lst = image_files_name_list_.Where(x => x.Contains(file_name)).ToArray();
                    if (name_lst.Length == 0)
                        HOperatorSet.GenEmptyObj(out img);
                    else
                        HOperatorSet.ReadImage(out img, name_lst[0]);

                     img_obj.Source.Add(img);

                   
                }
                HObject[] test = {};
                DeltaSubstrateInspector.src.Modules.clsTriggerImg.trigger_img_.Add(test); 

                // img_obj.MoveIndex = DeltaSubstrateInspector.src.Modules.ImageObj.trigger_img_.Count;
                ImageQueue.Enqueue(img_obj);
            }
            thread_.Abort(); // (20181009) Jeff Revised!
        }

        private string get_file_name(int move_index, int light_index)
        {
            string move_format = string.Format("M{0:d3}", move_index);
            string light_format = string.Format("F{0:d3}", light_index);
            return move_format+"_"+light_format;
        }

        private int get_move_index()
        {
            try
            {
                string[] file_info = file_name_.Split('_');
                int move_length = file_info[1].Length;
                return Convert.ToInt32(file_info[1].Substring(1, move_length - 1));
            }
            catch { return 0; }
        }

        private string file_select()
        {
            string file_direction = "";
            OpenFileDialog open_file_dialog = new OpenFileDialog();
            if (open_file_dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                file_direction = open_file_dialog.FileName;
            }
            return file_direction;
        }

        private void load_imgs_from_filemap(string filemap)
        {
            if (filemap != "")
            {
                string folder_filemap = Path.GetDirectoryName(filemap);
                var extensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".bmp", ".tif", "jpg", "png" };
                var valid_img_filename = Directory.EnumerateFiles(folder_filemap, "*.*").Where(s => extensions.Any(ext => ext == Path.GetExtension(s).ToLower())).OrderBy(s => s);
                file_name_ = Path.GetFileNameWithoutExtension(filemap);
                image_files_name_list_ = valid_img_filename.ToList();
            }
        }

        public string SimulationType
        {
            set { this.simulation_type_ = value; }
        }
    }
}
