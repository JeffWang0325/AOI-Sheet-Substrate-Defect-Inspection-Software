using HalconDotNet;
//using Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DeltaSubstrateInspector.src.Modules
{

    //public static class ImageObj
    //{

    //    //private static List<HObject> src_collection = new List<HObject>();
    //    

    //    public static List<HObject[]> trigger_img_ = new List<HObject[]>();

    //}
    public class clsTriggerImg
    {
        public static List<HObject[]> trigger_img_ = new List<HObject[]>();
    }
    public class ImageObj
    {
        private List<HObject> src_collection = new List<HObject>();
        private int move_index = 0;

        public void save_image(string base_file_name)
        {
            try
            {
                for (int i = 1; i <= src_collection.Count; i++)
                {
                    string move_format = string.Format("M{0:d3}", move_index);
                    string light_format_1 = string.Format("F{0:d3}", i);

                    HOperatorSet.WriteImage(src_collection[i - 1], "tiff", 0, base_file_name + "_" + move_format + "_" + light_format_1);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("儲存影像發生錯誤，請確認影像內容");
            }
        }

        public void release()
        {
            //src_collection = null; //190115, reomve for ImageShow save
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// 單一檢測位置之所有影像
        /// </summary>
        public List<HObject> Source
        {
            get { return src_collection; }
            set { this.src_collection = value; }
        }

        /// <summary>
        /// 走停拍移動位置Index
        /// Note: 原始數值為從1開始累加，當影像到第一個檢測位置時，數值會更新從1開始累加
        /// </summary>
        public int MoveIndex
        {
            get { return move_index; }
            set { this.move_index = value; }
        }

    }
}
