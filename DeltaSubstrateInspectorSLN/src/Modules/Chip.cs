using HalconDotNet;
using DeltaSubstrateInspector.src.Modules.ResultModule;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extension;
namespace DeltaSubstrateInspector.src.Modules
{
    public class Chip
    {
        private bool is_update = false;
        private int chip_id_ = 0;
        //private Dictionary<int, List<Defect>> defect_list_ = new Dictionary<int, List<Defect>>();
        //private Dictionary<int, ImageObj> image_list = new Dictionary<int, ImageObj>();
        private List<Record> result_record_lst_ = new List<Record>();
        private int num_record_ = 0;
        private Dictionary<string, int> defect_count_table = new Dictionary<string, int>();
        /// <summary>
        /// 瑕疵分類
        /// </summary>
        private Dictionary<string, DefectsResult> DefectsClassify = new Dictionary<string, DefectsResult>(); // (20190720) Jeff Revised!
        public Chip(int id)
        {
            this.chip_id_ = id;
        }

        public void update(ImageObj images, List<Defect> defects)
        {
            Record result_record = new Record(images, defects);
            result_record_lst_.Add(result_record);
            set_up_defect_table();
            num_record_ += 1;
        }

        public void update(ImageObj images, List<Defect> defects, Dictionary<string, List<Defect>> dic_defects)
        {
            Record result_record = new Record(images, defects, dic_defects);
            result_record_lst_.Add(result_record);
            set_up_defect_table();
            num_record_ += 1;
        }

        /// <summary>
        /// 更新瑕疵分類
        /// </summary>
        /// <param name="defect_count_table"></param>
        public void update_DefectsClassify(Dictionary<string, DefectsResult> defectsClassify) // (20190720) Jeff Revised!
        {
            DefectsClassify = defectsClassify;
        }

        /// <summary>
        /// 瑕疵分類
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, DefectsResult> Get_DefectsClassify // (20190720) Jeff Revised!
        {
            get { return DefectsClassify; }
        }

        /// <summary>
        /// 將瑕疵繪於影像上
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public HObject get_TotalDefect_region_image(int index) // (20190718) Jeff Revised!
        {
            HObject color_image = result_record_lst_[index].SrcImage.Source[0];
            // 轉成RGB image (20181227) Jeff Revised!
            HTuple Channels;
            HOperatorSet.CountChannels(color_image, out Channels);
            if (Channels == 1)
            {
                HOperatorSet.Compose3(color_image.Clone(), color_image.Clone(), color_image.Clone(), out color_image);
            }
            HObject result;
            HOperatorSet.GenEmptyObj(out result);
            List<Defect> defects = result_record_lst_[index].DefectLst;
            
            // 註解掉 For 華新trim痕檢測 (20181001) Jeff Revised!
            if (defects.Count == 1)
            {
                if (defects[0].DefectNum == 0)
                    return color_image;
            }
            else
            {
                List<int> num = defects.Select(x => x.DefectNum).ToList();
                int count = 0;
                foreach (var item in num)
                {
                    count += item;
                }
                if (count == 0)
                    return color_image;
            }

            // When defect exist
            HObject union_res = defects[0].DefectRegion;            
            for (int i = 1; i < defects.Count; i++)
                HOperatorSet.Union2(union_res, defects[i].DefectRegion, out union_res);

            try
            {
                if (union_res != null)
                {
                    /*
                    HObject union_resShow;
                    ZoomRegionFixOriCenter(union_res, out union_resShow, 100);
                    HOperatorSet.PaintRegion(union_resShow, color_image, out result, ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "fill");
                    */
                    // 將總瑕疵畫在影像上 (紅色)
                    HOperatorSet.PaintRegion(union_res, color_image, out result, ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "fill");
                }
            }
            catch (Exception ex)
            { }
            finally
            { }

            return result;
        }

        /// <summary>
        /// 總瑕疵region
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public HObject get_TotalDefect_region(int index) // (20190718) Jeff Revised!
        {
            HObject TotalDefect;
            HOperatorSet.GenEmptyObj(out TotalDefect);

            List<Defect> defects = result_record_lst_[index].DefectLst;
            if (defects.Count == 1)
            {
                if (defects[0].DefectNum == 0)
                    return TotalDefect;
            }
            else
            {
                List<int> num = defects.Select(x => x.DefectNum).ToList();
                int count = 0;
                foreach (var item in num)
                {
                    count += item;
                }
                if (count == 0)
                    return TotalDefect;
            }

            // When defect exist
            for (int i = 0; i < defects.Count; i++)
                HOperatorSet.Union2(TotalDefect, defects[i].DefectRegion, out TotalDefect);

            return TotalDefect;
        }

        public List<Record> GetRecordList()
        {
            return result_record_lst_;
        }

        public List<Defect> get_defect_rgns(int index)
        {
            return result_record_lst_[index - 1].DefectLst;
        }

        public Dictionary<string, List<Defect>> get_dic_defect(int index)
        {
            return result_record_lst_[index - 1].DicDefectLst;
        }

        private void set_up_defect_table()
        {
            int last_record = result_record_lst_.Count - 1;
            string[] defect_names = result_record_lst_[last_record].DefectLst.Select(x => x.DefectName).ToArray();

            foreach (var record in result_record_lst_[last_record].DefectLst)
            {
                if (defect_count_table.ContainsKey(record.DefectName))
                    defect_count_table[record.DefectName] = defect_count_table[record.DefectName] + record.DefectNum;
                else
                    defect_count_table.Add(record.DefectName, record.DefectNum);
            }
        }

        public int get_defect_count(string defect_name)
        {
            return defect_count_table[defect_name];
        }

        public int NumRecord
        {
            get { return this.num_record_; }
            set { this.num_record_ = value; }
        }

        public int Id
        {
            get { return chip_id_; }
        }

        public void release_all()
        {
            foreach (var item in result_record_lst_)
            {
                item.release();
            }
            result_record_lst_ = null;
            defect_count_table = null;

        }
        //public Dictionary<int, List<Defect>> DefectList
        //{
        //    get { return this.defect_list_; }
        //    set { this.defect_list_ = value; }
        //}

        //public Dictionary<int, ImageObj> ImageList
        //{
        //    get { return this.image_list; }
        //    set { this.image_list = value; }
        //}

        public void ZoomRegionFixOriCenter(HObject ho_oriRegion, out HObject ho_zoomRegion, HTuple hv_resizeFactor)
        {
            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 
            HObject ho_tempZoomRegion, ho_ObjectSelected = null;
            HObject ho_ObjectSelectedZoom = null;

            // Local control variables 
            HTuple hv_HomMat2D = null, hv_Area = null;
            HTuple hv_Row = null, hv_Column = null, hv_Number = null;
            HTuple hv_Index = null, hv_selectR = new HTuple(), hv_selectC = new HTuple();
            HTuple hv_HomMat2DScale = new HTuple();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_zoomRegion);
            HOperatorSet.GenEmptyObj(out ho_tempZoomRegion);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelectedZoom);
            HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
            HOperatorSet.AreaCenter(ho_oriRegion, out hv_Area, out hv_Row, out hv_Column);
            HOperatorSet.CountObj(ho_oriRegion, out hv_Number);
            ho_tempZoomRegion.Dispose();
            HOperatorSet.GenEmptyObj(out ho_tempZoomRegion);
            HTuple end_val4 = hv_Number;
            HTuple step_val4 = 1;
            for (hv_Index = 1; hv_Index.Continue(end_val4, step_val4); hv_Index = hv_Index.TupleAdd(step_val4))
            {
                ho_ObjectSelected.Dispose();
                HOperatorSet.SelectObj(ho_oriRegion, out ho_ObjectSelected, hv_Index);
                hv_selectR = hv_Row.TupleSelect(hv_Index - 1);
                hv_selectC = hv_Column.TupleSelect(hv_Index - 1);
                HOperatorSet.HomMat2dScale(hv_HomMat2D, hv_resizeFactor, hv_resizeFactor, hv_selectR,
                    hv_selectC, out hv_HomMat2DScale);
                ho_ObjectSelectedZoom.Dispose();
                HOperatorSet.AffineTransRegion(ho_ObjectSelected, out ho_ObjectSelectedZoom, hv_HomMat2DScale, "false");
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_tempZoomRegion, ho_ObjectSelectedZoom, out ExpTmpOutVar_0);
                    ho_tempZoomRegion.Dispose();
                    ho_tempZoomRegion = ExpTmpOutVar_0;
                }
            }

            ho_zoomRegion.Dispose();
            HOperatorSet.CopyObj(ho_tempZoomRegion, out ho_zoomRegion, 1, -1);

            ho_tempZoomRegion.Dispose();
            ho_ObjectSelected.Dispose();
            ho_ObjectSelectedZoom.Dispose();

            return;

        }


    }
}
