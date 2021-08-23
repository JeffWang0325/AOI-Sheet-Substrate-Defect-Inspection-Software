using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DeltaSubstrateInspector.FileSystem.FileSystem;
using System.IO;

namespace DeltaSubstrateInspector.src.Modules.StorageModule
{
    public class StorageManager
    {
        private static string nowSBIDDir = "";
        private static string base_filemap_ = ModuleParamDirectory + ImageFileDirectory + DateTime.Now.ToString("yyyyMMdd") + ModuleName + "\\" + SB_ID.ToString();
        private static int InerCounter = 0;
        
        private static void update_info(bool _UseInerCounterIDMode = false)
        {
            if (_UseInerCounterIDMode == false)
            {
                if (SB_ID != "")
                {
                    nowSBIDDir = SB_ID;
                }
                else
                {
                    nowSBIDDir = "99999999";
                }
            }
            else
            {
                nowSBIDDir = SB_InerCountID;
            }


            // 建立yyyyMMdd 日期資料夾
            string dateStr = DateTime.Now.ToString("yyyyMMdd");
            string saveDir = ModuleParamDirectory + ImageFileDirectory + dateStr;
            if (!Directory.Exists(saveDir))
                Directory.CreateDirectory(saveDir);

            // 建立生產料號資料夾
            saveDir = saveDir + "\\" + PartNumber;
            if (!Directory.Exists(saveDir))
                Directory.CreateDirectory(saveDir);

            base_filemap_ = ModuleParamDirectory + ImageFileDirectory + dateStr + "\\" + PartNumber + "\\" + ModuleName + "\\" + nowSBIDDir.ToString();

        }
        


        public static void write_source_img(ImageObj img_set, bool _UseInerCounterIDMode = false)
        {
            update_info(_UseInerCounterIDMode);
          
            //if (img_set.MoveIndex == 1)
            {
                if (!Directory.Exists(base_filemap_))
                {
                    Directory.CreateDirectory(base_filemap_);
                }
            }

            img_set.save_image(base_filemap_ += "\\" + nowSBIDDir.ToString());
            

        }
        
    }
}
