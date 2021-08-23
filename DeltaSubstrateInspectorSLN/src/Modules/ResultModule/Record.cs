using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extension;

namespace DeltaSubstrateInspector.src.Modules.ResultModule
{
    /*  @Record
     *  每一個位置檢測結束都會產生一筆 Record (ex: 一片有170取像位置，會有170筆Record)
     */
    public class Record
    {
        private ImageObj src_images_;
        /// <summary>
        /// 每一個位置之總瑕疵
        /// </summary>
        private List<Defect> defect_collection_;
        private int move_index_;

        // 181218, andy
        private Dictionary<string, List<Defect>> dic_defect_collection_ = new Dictionary<string, List<Defect>>();

        public Record(ImageObj images, List<Defect> defects)
        {
            src_images_ = images;
            defect_collection_ = defects;
            move_index_ = images.MoveIndex;
            
        }

        public Record(ImageObj images, List<Defect> defects, Dictionary<string, List<Defect>> dic_defects)
        {
            src_images_ = images;
            defect_collection_ = new List<Defect>(defects);
            move_index_ = images.MoveIndex;

            //Chris 20190425
            dic_defect_collection_ = new Dictionary<string, List<Defect>>(dic_defects);
        }

        /// <summary>
        /// 每一個位置之總瑕疵
        /// </summary>
        public List<Defect> DefectLst
        {
            get { return defect_collection_; }
        }

        public Dictionary<string, List<Defect>> DicDefectLst
        {
            get { return dic_defect_collection_; }
        }

        public ImageObj SrcImage
        {
            get { return src_images_; }
        }

        public void release()
        {
            foreach (var item in defect_collection_)
            {
                item.release();
            }
            defect_collection_ = null;
            src_images_.release();
            //GC.Collect();
            //GC.WaitForPendingFinalizers();

            // -------------------------------------
            //foreach (var item in dic_defect_collection_)
            //{
            //    // ...?
            //}
            dic_defect_collection_.Clear();
            dic_defect_collection_ = null;

        }
    }
}
