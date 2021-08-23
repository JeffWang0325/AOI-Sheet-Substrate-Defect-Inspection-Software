using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Extension;
using static DeltaSubstrateInspector.FileSystem.FileSystem;

namespace DeltaSubstrateInspector.src.PositionMethod.LocateMethod.Location
{
    public class LaserLocateMethod
    {
        #region 模板建立

        public bool Pretreatment_template(HObject IMG_template)
        {
            bool b_status_ = false;



            return b_status_;
        }

        #endregion



        #region 各種參數
        /// <summary>
        /// 各基板儲存資料夾
        /// </summary>
        private string file_directory_ = "default";

        /// <summary>
        /// 基板型號
        /// </summary>
        private string board_type_ = "default";
        public string BoardType
        {
            get { return this.board_type_; }
            set { this.board_type_ = value; }
        }

        /// <summary>
        /// 目前暫存之結果影像 (20190213) Jeff Revised!
        /// </summary>
        private HObject now_ResultImg = null;
        public HObject Now_ResultImg // (20190213) Jeff Revised!
        {
            get { return now_ResultImg; }
        }

        /* 演算法參數 */
        // 模板建立
        public int b_Pretreatment = 1;
        public int MinGray_Pretreatment = 66;
        public int MaxGray_Pretreatment = 255;
        public int MinContrast = 3;
        /// <summary>
        /// create_shape_model()中的Contrast
        /// </summary>
        public string Contrast_string = "10, 11, 4";
        public HTuple Contrast_HTuple = new HTuple();

        // Find Marks Center
        public int MinScore = 60; // (20190211) Jeff Revised!
        public int Width_ROI = 160; // (20190211) Jeff Revised!
        public int Height_ROI = 160; // (20190211) Jeff Revised!
        public int MinGray = 55;
        public int MaxGray = 255;
        public int MinArea = 3000;
        public int MaxArea = 10000;
        public int MinWidth = 190; // (20181224) Jeff Revised!
        public int MaxWidth = 230; // (20181224) Jeff Revised!
        public int MinHeight = 195; // (20181224) Jeff Revised!
        public int MaxHeight = 235; // (20181224) Jeff Revised!
        public int Radius_opening = 3;
        public int Radius_closing = 11;
        public int count_marks = 9; // (20181102) Jeff Revised!
        public int count_row = 3; // (20181102) Jeff Revised!
        /// <summary>
        /// 每一列分別有幾顆Marks之數量 (20181102) Jeff Revised!
        /// </summary>
        public string count_rowElement_string = "3, 3, 3";
        public HTuple count_rowElement_HTuple = new HTuple();

        /// <summary>
        /// 各Marks座標 (x, y)
        /// </summary>
        public List<PointF> all_marks_id_ = new List<PointF>(); // (20181119) Jeff Revised!

        /// <summary>
        /// 各Marks座標
        /// </summary>
        private HTuple hv_Row = null, hv_Column = null;

        /// <summary>
        /// 根目錄是否有Model存在
        /// </summary>
        private bool b_model_exist_ = false; // (20190211) Jeff Revised!
        public bool B_model_exist_ // (20190211) Jeff Revised!
        {
            get { return b_model_exist_; }
        }

        /// <summary>
        /// 使用者框出ROI的region (For shape model)
        /// </summary>
        private HObject model_region_;
        private HTuple ModelID_ = null;
        private HObject model_contour_;
        private HObject TransContours_template_;
        #endregion

        #region 顯示片段檢測結果 相關變數
        public HObject ho_TransContours_all, ho_Rect_marks, ho_ImageReduced_marks;
        public HObject ho_ConnectedRegions, ho_SelectedRegions, ho_RegionOpening, ho_RegionClosing;
        public HObject ho_Skeleton, ho_EndPoints, ho_JuncPoints;
        private HTuple hv_R_marks = null, hv_C_marks = null, hv_Angle = null, hv_Score = null;
        #endregion

        //*****************************************************************
        public LaserLocateMethod()
        {
            // Convert string into HTuple (20181102) Jeff Revised!
            string_2_HTuple(count_rowElement_string, out count_rowElement_HTuple, true);
        }

        /// <summary>
        /// 將 輸入字串 轉成 HTuple
        /// </summary>
        /// <param name="str_in">輸入字串，各元素用逗號","分隔</param>
        /// <param name="HTuple_out"></param>
        /// <param name="b_int">數值是否為整數</param>
        /// <returns></returns>
        public bool string_2_HTuple(string str_in, out HTuple HTuple_out, bool b_int = false) // (20181102) Jeff Revised!
        {
            try
            {
                if (str_in.Length == 0)
                {
                    HTuple_out = null;
                    return false;
                }

                // str_in 轉成 HTuple_out
                string str_in_rest = string.Copy(str_in);
                HTuple_out = new HTuple();
                while (str_in_rest.Length != 0)
                {
                    int Index = str_in_rest.IndexOf(',');
                    string str_temp;
                    if (Index == -1)
                    {
                        str_temp = string.Copy(str_in_rest);
                        // Update str_in_rest
                        str_in_rest = "";
                    }
                    else
                    {
                        str_temp = str_in_rest.Substring(0, Index);
                        // Update str_in_rest
                        str_in_rest = str_in_rest.Substring(Index + 1);
                    }

                    // 判斷是 數值 (整數, 浮點數) or 字串
                    double d;
                    if (double.TryParse(str_temp, out d)) // 數值
                    {
                        if (b_int)
                            HOperatorSet.TupleConcat(HTuple_out, (int)d, out HTuple_out);
                        else
                            HOperatorSet.TupleConcat(HTuple_out, d, out HTuple_out);
                    }
                    else // 字串
                        HOperatorSet.TupleConcat(HTuple_out, str_temp, out HTuple_out);
                }
            }
            catch
            {
                HTuple_out = null;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 將 輸入HTuple 轉成 單一字串
        /// </summary>
        /// <param name="HTuple_in"></param>
        /// <param name="str_out"></param>
        /// <returns></returns>
        public bool HTuple_2_string(HTuple HTuple_in, out string str_out) // (20181102) Jeff Revised!
        {
            try
            {
                // 轉成 單一字串
                str_out = "";
                HTuple n;
                HOperatorSet.TupleLength(HTuple_in, out n);
                for (int i = 0; i < n; i++)
                {
                    str_out += HTuple_in[i];
                    if (i < n - 1)
                        str_out += ", ";
                }
            }
            catch
            {
                str_out = "";
                return false;
            }

            return true;
        }

        /// <summary>
        /// 從.xml檔讀取變數
        /// </summary>
        /// <returns></returns>
        public bool load()
        {
            bool b_status_ = false;
            // 是否有此Type之基板資料夾
            file_directory_ = ModuleParamDirectory + LaserPositionParam + "\\" + board_type_;
            if (!Directory.Exists(file_directory_))
                Directory.CreateDirectory(file_directory_);
            b_model_exist_ = read_shape_model();

            try
            {
                if (!System.IO.File.Exists(ModuleParamDirectory + LaserPositionParam + "\\Marks center.xml"))
                    return false;
                XmlDocument xml_doc_ = new XmlDocument();
                xml_doc_.Load(ModuleParamDirectory + LaserPositionParam + "\\Marks center.xml");
               
                XmlNode xml_node_ = xml_doc_.SelectSingleNode("Board_Type/Type_" + board_type_);
                if (xml_node_ == null)
                    return false;
                else
                {
                    /* 更新參數 */
                    XmlElement xml_ele_ = (XmlElement)xml_node_.SelectSingleNode("Marks_Center");
                    // 模板建立
                    b_Pretreatment = Convert.ToInt32(xml_ele_.GetAttribute("b_Pretreatment"));
                    MinGray_Pretreatment = Convert.ToInt32(xml_ele_.GetAttribute("MinGray_Pretreatment"));
                    MaxGray_Pretreatment = Convert.ToInt32(xml_ele_.GetAttribute("MaxGray_Pretreatment"));
                    MinContrast = Convert.ToInt32(xml_ele_.GetAttribute("MinContrast"));
                    Contrast_string = xml_ele_.GetAttribute("Contrast_string");
                    string_2_HTuple(Contrast_string, out Contrast_HTuple, true);

                    // Find Marks Center
                    MinScore = Convert.ToInt32(xml_ele_.GetAttribute("MinScore")); // (20190211) Jeff Revised!
                    Width_ROI = Convert.ToInt32(xml_ele_.GetAttribute("Width_ROI")); // (20190211) Jeff Revised!
                    Height_ROI = Convert.ToInt32(xml_ele_.GetAttribute("Height_ROI")); // (20190211) Jeff Revised!
                    MinGray = Convert.ToInt32(xml_ele_.GetAttribute("MinGray"));
                    MaxGray = Convert.ToInt32(xml_ele_.GetAttribute("MaxGray"));
                    MinArea = Convert.ToInt32(xml_ele_.GetAttribute("MinArea"));
                    MaxArea = Convert.ToInt32(xml_ele_.GetAttribute("MaxArea"));
                    MinWidth = Convert.ToInt32(xml_ele_.GetAttribute("MinWidth")); // (20181224) Jeff Revised!
                    MaxWidth = Convert.ToInt32(xml_ele_.GetAttribute("MaxWidth")); // (20181224) Jeff Revised!
                    MinHeight = Convert.ToInt32(xml_ele_.GetAttribute("MinHeight")); // (20181224) Jeff Revised!
                    MaxHeight = Convert.ToInt32(xml_ele_.GetAttribute("MaxHeight")); // (20181224) Jeff Revised!
                    Radius_opening = Convert.ToInt32(xml_ele_.GetAttribute("Radius_opening"));
                    Radius_closing = Convert.ToInt32(xml_ele_.GetAttribute("Radius_closing"));
                    // (20181102) Jeff Revised!
                    count_marks = Convert.ToInt32(xml_ele_.GetAttribute("count_marks"));
                    count_row = Convert.ToInt32(xml_ele_.GetAttribute("count_row"));
                    count_rowElement_string = xml_ele_.GetAttribute("count_rowElement_string");
                    string_2_HTuple(count_rowElement_string, out count_rowElement_HTuple, true);

                    b_status_ = true;
                }
            }
            catch (Exception ex)
            {
                b_status_ = false;
            }

            return b_status_;
        }

        /// <summary>
        /// 儲存變數至.xml檔
        /// </summary>
        /// <returns></returns>
        public bool save()
        {
            bool b_status_ = false;

            try
            {
                XmlDocument xml_doc_ = new XmlDocument();
                if (!System.IO.File.Exists(ModuleParamDirectory + LaserPositionParam + "\\Marks center.xml"))
                    return false;
                else
                    xml_doc_.Load(ModuleParamDirectory + LaserPositionParam + "\\Marks center.xml");

                XmlNode xml_node_ = xml_doc_.SelectSingleNode("Board_Type");
                if (xml_node_ == null)
                {
                    xml_node_ = xml_doc_.CreateElement("Board_Type");
                    xml_doc_.AppendChild(xml_node_);
                }

                XmlNode xml_type_ = xml_doc_.SelectSingleNode("Board_Type/Type_" + board_type_);
                if (xml_type_ == null)
                {
                    xml_type_ = xml_doc_.CreateElement("Type_" + board_type_);
                    xml_node_.AppendChild(xml_type_);

                    XmlElement xml_ele_ = xml_doc_.CreateElement("Marks_Center");
                    /* 儲存參數 */
                    // 模板建立
                    xml_ele_.SetAttribute("b_Pretreatment", b_Pretreatment.ToString());
                    xml_ele_.SetAttribute("MinGray_Pretreatment", MinGray_Pretreatment.ToString());
                    xml_ele_.SetAttribute("MaxGray_Pretreatment", MaxGray_Pretreatment.ToString());
                    xml_ele_.SetAttribute("MinContrast", MinContrast.ToString());
                    HTuple_2_string(Contrast_HTuple, out Contrast_string);
                    xml_ele_.SetAttribute("Contrast_string", Contrast_string);

                    // Find Marks Center
                    xml_ele_.SetAttribute("MinScore", MinScore.ToString()); // (20190211) Jeff Revised!
                    xml_ele_.SetAttribute("Width_ROI", Width_ROI.ToString()); // (20190211) Jeff Revised!
                    xml_ele_.SetAttribute("Height_ROI", Height_ROI.ToString()); // (20190211) Jeff Revised!
                    xml_ele_.SetAttribute("MinGray", MinGray.ToString());
                    xml_ele_.SetAttribute("MaxGray", MaxGray.ToString());
                    xml_ele_.SetAttribute("MinArea", MinArea.ToString());
                    xml_ele_.SetAttribute("MaxArea", MaxArea.ToString());
                    xml_ele_.SetAttribute("MinWidth", MinWidth.ToString()); // (20181224) Jeff Revised!
                    xml_ele_.SetAttribute("MaxWidth", MaxWidth.ToString()); // (20181224) Jeff Revised!
                    xml_ele_.SetAttribute("MinHeight", MinHeight.ToString()); // (20181224) Jeff Revised!
                    xml_ele_.SetAttribute("MaxHeight", MaxHeight.ToString()); // (20181224) Jeff Revised!
                    xml_ele_.SetAttribute("Radius_opening", Radius_opening.ToString());
                    xml_ele_.SetAttribute("Radius_closing", Radius_closing.ToString());
                    // (20181102) Jeff Revised!
                    xml_ele_.SetAttribute("count_marks", count_marks.ToString());
                    xml_ele_.SetAttribute("count_row", count_row.ToString());
                    HTuple_2_string(count_rowElement_HTuple, out count_rowElement_string);
                    xml_ele_.SetAttribute("count_rowElement_string", count_rowElement_string);

                    xml_type_.AppendChild(xml_ele_);
                }
                else
                {
                    XmlNode xml_ele_ = xml_doc_.SelectSingleNode("Board_Type/Type_" + board_type_ + "/Marks_Center");
                    /* 儲存參數 */
                    // 模板建立
                    xml_ele_.Attributes["b_Pretreatment"].Value = b_Pretreatment.ToString();
                    xml_ele_.Attributes["MinGray_Pretreatment"].Value = MinGray_Pretreatment.ToString();
                    xml_ele_.Attributes["MaxGray_Pretreatment"].Value = MaxGray_Pretreatment.ToString();
                    xml_ele_.Attributes["MinContrast"].Value = MinContrast.ToString();
                    HTuple_2_string(Contrast_HTuple, out Contrast_string);
                    xml_ele_.Attributes["Contrast_string"].Value = Contrast_string;

                    // Find Marks Center
                    xml_ele_.Attributes["MinScore"].Value = MinScore.ToString(); // (20190211) Jeff Revised!
                    xml_ele_.Attributes["Width_ROI"].Value = Width_ROI.ToString(); // (20190211) Jeff Revised!
                    xml_ele_.Attributes["Height_ROI"].Value = Height_ROI.ToString(); // (20190211) Jeff Revised!
                    xml_ele_.Attributes["MinGray"].Value = MinGray.ToString();
                    xml_ele_.Attributes["MaxGray"].Value = MaxGray.ToString();
                    xml_ele_.Attributes["MinArea"].Value = MinArea.ToString();
                    xml_ele_.Attributes["MaxArea"].Value = MaxArea.ToString();
                    xml_ele_.Attributes["MinWidth"].Value = MinWidth.ToString(); // (20181224) Jeff Revised!
                    xml_ele_.Attributes["MaxWidth"].Value = MaxWidth.ToString(); // (20181224) Jeff Revised!
                    xml_ele_.Attributes["MinHeight"].Value = MinHeight.ToString(); // (20181224) Jeff Revised!
                    xml_ele_.Attributes["MaxHeight"].Value = MaxHeight.ToString(); // (20181224) Jeff Revised!
                    xml_ele_.Attributes["Radius_opening"].Value = Radius_opening.ToString();
                    xml_ele_.Attributes["Radius_closing"].Value = Radius_closing.ToString();
                    // (20181102) Jeff Revised!
                    xml_ele_.Attributes["count_marks"].Value = count_marks.ToString();
                    xml_ele_.Attributes["count_row"].Value = count_row.ToString();
                    HTuple_2_string(count_rowElement_HTuple, out count_rowElement_string);
                    xml_ele_.Attributes["count_rowElement_string"].Value = count_rowElement_string;
                }

                //xml_doc_.Save(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\Label Defect.xml");
                xml_doc_.Save(ModuleParamDirectory + LaserPositionParam + "\\Marks center.xml");
                b_status_ = true;
            }
            catch (Exception ex)
            {
                b_status_ = false;
            }

            return b_status_;
        }

        /// <summary>
        /// 讀取匹配模塊、模塊區域
        /// </summary>
        /// <returns></returns>
        public bool read_shape_model()
        {
            bool b_status_ = false;
            HTuple area_, row_, column_, HomMat2D_;
            if (File.Exists(file_directory_ + "\\" + board_type_ + "_ModelID") && File.Exists(file_directory_ + "\\" + board_type_ + "_ModelRegion.hobj"))
            {
                clear_shape_model();
                HOperatorSet.ReadShapeModel(file_directory_ + "\\" + board_type_ + "_ModelID", out ModelID_);
                HOperatorSet.ReadRegion(out model_region_, file_directory_ + "\\" + board_type_ + "_ModelRegion.hobj");
                HOperatorSet.GetShapeModelContours(out model_contour_, ModelID_, 1);
                HOperatorSet.AreaCenter(model_region_, out area_, out row_, out column_);
                HOperatorSet.VectorAngleToRigid(0, 0, 0, row_, column_, 0, out HomMat2D_);
                HOperatorSet.AffineTransContourXld(model_contour_, out TransContours_template_, HomMat2D_);

                b_status_ = true;
            }
            return b_status_;
        }

        public void clear_shape_model()
        {
            if (ModelID_ != null)
            {
                HOperatorSet.ClearShapeModel(ModelID_);
                ModelID_ = null;
            }
        }

        private int count_mark_image = 0; // For debug! (20181107) Jeff Revised!
        /// <summary>
        /// 計算各Marks座標
        /// </summary>
        /// <param name="IMG_Lasermarks"></param>
        /// <param name="b_coordinate_sort">是否做座標排序</param>
        /// /// <param name="b_save_MarkImage">是否儲存雷射定位影像及檢測結果</param>
        /// /// <param name="b_draw_Image">是否將檢測結果影像暫存於程式中</param>
        /// <returns></returns>
        public bool find_marks_center(HObject IMG_Lasermarks, bool b_coordinate_sort = true, bool b_save_MarkImage = false, bool b_draw_Image = false) // (20190213) Jeff Revised!
        {
            bool b_status_ = false;

            // Local iconic variables 
            HObject ho_GrayImage, ho_TransContours = null, ho_Regions, ho_Rect_marks_union;
            
            int num_marks = -1; // 找到之Marks個數 (20181107) Jeff Revised!

            try
            {
                HTuple hv_Channels;
                HOperatorSet.CountChannels(IMG_Lasermarks, out hv_Channels);
                //if ((int)(new HTuple(hv_Channels.TupleNotEqual(1))) != 0)
                if (hv_Channels != 1)
                    HOperatorSet.Rgb1ToGray(IMG_Lasermarks, out ho_GrayImage);
                else
                    HOperatorSet.CopyImage(IMG_Lasermarks, out ho_GrayImage);



                /* 初定位: 模板匹配，找出Mark的粗略位置 */
                // 模板匹配
                HOperatorSet.FindShapeModel(ho_GrayImage, ModelID_, (new HTuple(0)).TupleRad(), (new HTuple(360)).TupleRad(), MinScore / 100.0, count_marks, 0, "least_squares", (new HTuple(5)).TupleConcat(1), 0.75, out hv_R_marks, out hv_C_marks, out hv_Angle, out hv_Score);

                // Transform the model contours into the detected positions
                HOperatorSet.GenEmptyObj(out ho_TransContours_all);
                for (int i = 0; i <= (int)((new HTuple(hv_Score.TupleLength())) - 1); i++)
                {
                    HTuple hv_HomMat2D;
                    HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
                    HOperatorSet.HomMat2dRotate(hv_HomMat2D, hv_Angle.TupleSelect(i), 0, 0, out hv_HomMat2D);
                    HOperatorSet.HomMat2dTranslate(hv_HomMat2D, hv_R_marks.TupleSelect(i), hv_C_marks.TupleSelect(i), out hv_HomMat2D);
                    HOperatorSet.AffineTransContourXld(model_contour_, out ho_TransContours, hv_HomMat2D);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_TransContours_all, ho_TransContours, out ExpTmpOutVar_0);
                        ho_TransContours_all.Dispose();
                        ho_TransContours_all = ExpTmpOutVar_0;
                    }
                }

                // 定位Mark粗略位置
                HTuple hv_Radius;
                HOperatorSet.SmallestCircleXld(ho_TransContours_all, out hv_R_marks, out hv_C_marks, out hv_Radius);
                HTuple hv_L1, hv_L2;
                HOperatorSet.TupleGenConst(new HTuple(hv_R_marks.TupleLength()), Width_ROI / 2.0, out hv_L1);
                HOperatorSet.TupleGenConst(new HTuple(hv_R_marks.TupleLength()), Height_ROI / 2.0, out hv_L2);
                HOperatorSet.GenRectangle2(out ho_Rect_marks, hv_R_marks, hv_C_marks, hv_Angle, hv_L1, hv_L2);



                /* 針對初定位區域，計算Marks中心座標 */
                HOperatorSet.Union1(ho_Rect_marks, out ho_Rect_marks_union);
                HOperatorSet.ReduceDomain(ho_GrayImage, ho_Rect_marks_union, out ho_ImageReduced_marks);
                HOperatorSet.Threshold(ho_ImageReduced_marks, out ho_Regions, MinGray, MaxGray);
                HOperatorSet.Connection(ho_Regions, out ho_ConnectedRegions);

                // int 轉 HTuple
                HTuple hv_MinArea = MinArea;
                HTuple hv_MaxArea = MaxArea;
                HTuple hv_MinWidth = MinWidth;
                HTuple hv_MaxWidth = MaxWidth;
                HTuple hv_MinHeight = MinHeight;
                HTuple hv_MaxHeight = MaxHeight;
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, ((new HTuple("area")).TupleConcat("width")).TupleConcat("height"), 
                                         "and", ((hv_MinArea.TupleConcat(hv_MinWidth))).TupleConcat(hv_MinHeight), ((hv_MaxArea.TupleConcat(hv_MaxWidth))).TupleConcat(hv_MaxHeight));

                // 消除突出雜點 (Note: 半徑過大可能會消除Mark區域)
                HOperatorSet.OpeningCircle(ho_SelectedRegions, out ho_RegionOpening, Radius_opening);

                // 填滿內部孔洞 & 平滑化輪廓
                HOperatorSet.ClosingCircle(ho_RegionOpening, out ho_RegionClosing, Radius_closing);

                // 利用marks的骨架，找中點
                HOperatorSet.Skeleton(ho_RegionClosing, out ho_Skeleton);
                HOperatorSet.JunctionsSkeleton(ho_Skeleton, out ho_EndPoints, out ho_JuncPoints);
                HTuple hv_Area;
                HOperatorSet.AreaCenter(ho_JuncPoints, out hv_Area, out hv_Row, out hv_Column);

                // 座標排序 (20181102) Jeff Revised!
                if (b_coordinate_sort)
                {
                    coordinate_sort(hv_Row, hv_Column, count_marks, count_row, count_rowElement_HTuple, out hv_Row, out hv_Column);
                }

                // 儲存至 all_marks_id_
                all_marks_id_.Clear();
                all_marks_id_ = new List<PointF>();
                num_marks = (int)(new HTuple(hv_Row.TupleLength())); // (20181107) Jeff Revised!
                if (num_marks == count_marks) b_status_ = true;
                for (int i = 1; i <= num_marks; i++)
                {
                    //all_marks_id_.Add(new Point((hv_Column.TupleSelect(i - 1)).TupleRound(), (hv_Row.TupleSelect(i - 1)).TupleRound()));
                    all_marks_id_.Add(new PointF(((float)(hv_Column.TupleSelect(i - 1).D)), (float)(hv_Row.TupleSelect(i - 1).D)));
                }

                // Release memories
                ho_GrayImage.Dispose();
                ho_TransContours.Dispose();
                ho_Rect_marks_union.Dispose();
                ho_Regions.Dispose();
            }
            catch
            {

            }

            #region 雷射定位測試
            if (b_save_MarkImage || b_draw_Image) // (20190213) Jeff Revised!
            {
                try
                {
                    if (b_save_MarkImage)
                        count_mark_image++;
                    string path = "D:/LaserPosition test/";
                    if (b_save_MarkImage)
                    {
                        HTuple file_exist_ = 0;
                        HOperatorSet.FileExists(path, out file_exist_);
                        if (file_exist_ == 1) // 資料夾存在
                        {

                        }
                        else
                        {
                            HOperatorSet.MakeDir(path); // Create file
                        }

                        path += count_mark_image;
                        if (!b_status_) path += " (Failure)";
                        HOperatorSet.FileExists(path, out file_exist_);
                        if (file_exist_ == 1) // 資料夾存在
                        {

                        }
                        else
                        {
                            HOperatorSet.MakeDir(path); // Create file
                        }

                        path += "/";
                        // 儲存雷射定位原始影像
                        HOperatorSet.WriteImage(IMG_Lasermarks, "tiff", 0, path + "mark_image.tiff");
                    }

                    // 轉成RGB image
                    HTuple Channels;
                    HOperatorSet.CountChannels(IMG_Lasermarks, out Channels);
                    if (Channels == 1)
                        HOperatorSet.Compose3(IMG_Lasermarks.Clone(), IMG_Lasermarks.Clone(), IMG_Lasermarks.Clone(), out now_ResultImg);
                    else
                        HOperatorSet.CopyImage(IMG_Lasermarks.Clone(), out now_ResultImg);

                    // 儲存雷射定位影像 + 中心點
                    if (num_marks == 0 || num_marks == -1)
                    {
                        if (b_save_MarkImage)
                            HOperatorSet.WriteImage(now_ResultImg, "tiff", 0, path + "mark_image_center.tiff");
                        return b_status_;
                    }
                    HObject mark_center = null;
                    HTuple hv_radius_circle = new HTuple();
                    for (int i = 1; i <= num_marks; i++)
                        HOperatorSet.TupleConcat(hv_radius_circle, 10, out hv_radius_circle);
                    HOperatorSet.GenCircle(out mark_center, hv_Row, hv_Column, hv_radius_circle);
                    
                    //HObject IMG_Lasermarks_center = null;
                    //HOperatorSet.CopyImage(IMG_Lasermarks, out IMG_Lasermarks_center);
                    //HOperatorSet.OverpaintRegion(IMG_Lasermarks_center, mark_center, ((new HTuple(0)).TupleConcat(0)).TupleConcat(255), "fill");
                    //HOperatorSet.WriteImage(IMG_Lasermarks_center, "tiff", 0, path + "mark_image_center.tiff");
                    HOperatorSet.OverpaintRegion(now_ResultImg, mark_center, ((new HTuple(0)).TupleConcat(0)).TupleConcat(255), "fill");
                    if (b_save_MarkImage)
                        HOperatorSet.WriteImage(now_ResultImg, "tiff", 0, path + "mark_image_center.tiff");
                }
                catch
                {
                    MessageBox.Show("儲存雷射定位影像錯誤!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            #endregion

            return b_status_;
        }

        /// <summary>
        /// 顯示各Marks座標
        /// </summary>
        /// <param name="hwindow_"></param>
        /// <param name="IMG_Lasermarks"></param>
        /// <returns></returns>
        public bool disp_marks_center(HSmartWindowControl hwindow_, HObject IMG_Lasermarks)
        {
            bool b_status_ = false;

            try
            {
                HTuple hv_radius_circle = null, hv_Index = null, hv_string_disp = null;
                HTuple hv_num_marks = null; // (20181102) Jeff Revised!

                // 顯示各mark中點及座標
                //hwindow_.SetFullImagePart(new HImage(IMG_Lasermarks));
                HOperatorSet.DispObj(IMG_Lasermarks, hwindow_.HalconWindow);

                hv_radius_circle = new HTuple();
                hv_num_marks = new HTuple(); // (20181102) Jeff Revised!
                for (hv_Index = 1; (int)hv_Index <= (int)(new HTuple(hv_Row.TupleLength())); hv_Index = (int)hv_Index + 1)
                {
                    HOperatorSet.TupleConcat(hv_radius_circle, 5, out hv_radius_circle);
                    HOperatorSet.TupleConcat(hv_num_marks, hv_Index, out hv_num_marks);
                }
                HOperatorSet.SetColor(hwindow_.HalconWindow, "blue");
                HOperatorSet.SetDraw(hwindow_.HalconWindow, "fill");
                if ((int)(new HTuple((new HTuple(hv_Row.TupleLength())).TupleNotEqual(0))) != 0) // (20181102) Jeff Revised!
                {
                    HOperatorSet.DispCircle(hwindow_.HalconWindow, hv_Row, hv_Column, hv_radius_circle);
                    hv_string_disp = ((((hv_num_marks + ". (") + (hv_Column.TupleString(".1f"))) + new HTuple(", ")) + (hv_Row.TupleString(".1f"))) + ")";
                    disp_message(hwindow_.HalconWindow, hv_string_disp, "image", hv_Row - 50, hv_Column + 8, "green", "false");
                }

                b_status_ = true;
            }
            catch (Exception ex)
            {
                b_status_ = false;
            }

            return b_status_;
        }

        public void initialize_params()
        {
            hv_Row = null;
            //hv_Row = new HTuple();
            hv_Column = null;
            //hv_Column = new HTuple();
        }

        public void disp_message(HTuple hv_WindowHandle, HTuple hv_String, HTuple hv_CoordSystem, HTuple hv_Row, HTuple hv_Column, HTuple hv_Color, HTuple hv_Box)
        {
            // Local iconic variables 

            // Local control variables 
            HTuple hv_GenParamName = null, hv_GenParamValue = null;
            HTuple hv_Color_COPY_INP_TMP = hv_Color.Clone();
            HTuple hv_Column_COPY_INP_TMP = hv_Column.Clone();
            HTuple hv_CoordSystem_COPY_INP_TMP = hv_CoordSystem.Clone();
            HTuple hv_Row_COPY_INP_TMP = hv_Row.Clone();

            // Initialize local and output iconic variables 
            //This procedure displays text in a graphics window.
            //
            //Input parameters:
            //WindowHandle: The WindowHandle of the graphics window, where
            //   the message should be displayed
            //String: A tuple of strings containing the text message to be displayed
            //CoordSystem: If set to 'window', the text position is given
            //   with respect to the window coordinate system.
            //   If set to 'image', image coordinates are used.
            //   (This may be useful in zoomed images.)
            //Row: The row coordinate of the desired text position
            //   A tuple of values is allowed to display text at different
            //   positions.
            //Column: The column coordinate of the desired text position
            //   A tuple of values is allowed to display text at different
            //   positions.
            //Color: defines the color of the text as string.
            //   If set to [], '' or 'auto' the currently set color is used.
            //   If a tuple of strings is passed, the colors are used cyclically...
            //   - if |Row| == |Column| == 1: for each new textline
            //   = else for each text position.
            //Box: If Box[0] is set to 'true', the text is written within an orange box.
            //     If set to' false', no box is displayed.
            //     If set to a color string (e.g. 'white', '#FF00CC', etc.),
            //       the text is written in a box of that color.
            //     An optional second value for Box (Box[1]) controls if a shadow is displayed:
            //       'true' -> display a shadow in a default color
            //       'false' -> display no shadow
            //       otherwise -> use given string as color string for the shadow color
            //
            //It is possible to display multiple text strings in a single call.
            //In this case, some restrictions apply:
            //- Multiple text positions can be defined by specifying a tuple
            //  with multiple Row and/or Column coordinates, i.e.:
            //  - |Row| == n, |Column| == n
            //  - |Row| == n, |Column| == 1
            //  - |Row| == 1, |Column| == n
            //- If |Row| == |Column| == 1,
            //  each element of String is display in a new textline.
            //- If multiple positions or specified, the number of Strings
            //  must match the number of positions, i.e.:
            //  - Either |String| == n (each string is displayed at the
            //                          corresponding position),
            //  - or     |String| == 1 (The string is displayed n times).
            //
            //
            //Convert the parameters for disp_text.
            if ((int)((new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(new HTuple()))).TupleOr(new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(new HTuple())))) != 0)
            {
                return;
            }
            if ((int)(new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Row_COPY_INP_TMP = 12;
            }
            if ((int)(new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Column_COPY_INP_TMP = 12;
            }
            //
            //Convert the parameter Box to generic parameters.
            hv_GenParamName = new HTuple();
            hv_GenParamValue = new HTuple();
            if ((int)(new HTuple((new HTuple(hv_Box.TupleLength())).TupleGreater(0))) != 0)
            {
                if ((int)(new HTuple(((hv_Box.TupleSelect(0))).TupleEqual("false"))) != 0)
                {
                    //Display no box
                    hv_GenParamName = hv_GenParamName.TupleConcat("box");
                    hv_GenParamValue = hv_GenParamValue.TupleConcat("false");
                }
                else if ((int)(new HTuple(((hv_Box.TupleSelect(0))).TupleNotEqual("true"))) != 0)
                {
                    //Set a color other than the default.
                    hv_GenParamName = hv_GenParamName.TupleConcat("box_color");
                    hv_GenParamValue = hv_GenParamValue.TupleConcat(hv_Box.TupleSelect(0));
                }
            }
            if ((int)(new HTuple((new HTuple(hv_Box.TupleLength())).TupleGreater(1))) != 0)
            {
                if ((int)(new HTuple(((hv_Box.TupleSelect(1))).TupleEqual("false"))) != 0)
                {
                    //Display no shadow.
                    hv_GenParamName = hv_GenParamName.TupleConcat("shadow");
                    hv_GenParamValue = hv_GenParamValue.TupleConcat("false");
                }
                else if ((int)(new HTuple(((hv_Box.TupleSelect(1))).TupleNotEqual("true"))) != 0)
                {
                    //Set a shadow color other than the default.
                    hv_GenParamName = hv_GenParamName.TupleConcat("shadow_color");
                    hv_GenParamValue = hv_GenParamValue.TupleConcat(hv_Box.TupleSelect(1));
                }
            }
            //Restore default CoordSystem behavior.
            if ((int)(new HTuple(hv_CoordSystem_COPY_INP_TMP.TupleNotEqual("window"))) != 0)
            {
                hv_CoordSystem_COPY_INP_TMP = "image";
            }
            //
            if ((int)(new HTuple(hv_Color_COPY_INP_TMP.TupleEqual(""))) != 0)
            {
                //disp_text does not accept an empty string for Color.
                hv_Color_COPY_INP_TMP = new HTuple();
            }
            //
            HOperatorSet.DispText(hv_WindowHandle, hv_String, hv_CoordSystem_COPY_INP_TMP, hv_Row_COPY_INP_TMP, hv_Column_COPY_INP_TMP, hv_Color_COPY_INP_TMP, hv_GenParamName, hv_GenParamValue);

            return;
        }

        /// <summary>
        /// 座標排序: 將座標順序轉換成由左到右，由上到下之順序。
        /// </summary>
        /// <param name="hv_row_in"></param>
        /// <param name="hv_col_in"></param>
        /// <param name="hv_count_marks">Marks總數量</param>
        /// <param name="hv_count_row">有幾列</param>
        /// <param name="hv_count_rowElement">每一列分別有幾顆Marks之數量</param>
        /// <param name="hv_row_out"></param>
        /// <param name="hv_col_out"></param>
        private void coordinate_sort(HTuple hv_row_in, HTuple hv_col_in, HTuple hv_count_marks, HTuple hv_count_row, HTuple hv_count_rowElement, 
                                     out HTuple hv_row_out, out HTuple hv_col_out) // (20190209) Jeff Revised!
        {
            // Local control variables 
            HTuple hv_Indices = null, hv_i1 = null, hv_i2 = null;
            HTuple hv_r = null, hv_row_Selected = new HTuple(), hv_col_Selected = new HTuple();
            HTuple hv_Indices1 = new HTuple(), hv_index_replace = new HTuple();
            HTuple hv_i = new HTuple();
            // Initialize local and output iconic variables 
            hv_row_out = new HTuple();
            hv_col_out = new HTuple();

            if (hv_count_marks == 1) // (20190209) Jeff Revised!
            {
                // 只有一點，無需做排序，輸出tuple和輸入tuple完全一樣
                hv_row_out = hv_row_in.Clone();
                hv_col_out = hv_col_in.Clone();

                return;
            }

            if ((int)((new HTuple((new HTuple(hv_row_in.TupleLength())).TupleNotEqual(hv_count_marks))).TupleOr(new HTuple(((hv_count_rowElement.TupleSum())).TupleNotEqual(hv_count_marks)))) != 0)
            {
                // 輸入訊息錯誤，輸出tuple和輸入tuple完全一樣
                hv_row_out = hv_row_in.Clone();
                hv_col_out = hv_col_in.Clone();

                return;
            }

            // 先針對row做排序 (由小到大)
            HOperatorSet.TupleSortIndex(hv_row_in, out hv_Indices);
            tuple_relocate_index(hv_row_in, hv_Indices, out hv_row_out);

            // 對應column交換位置
            tuple_relocate_index(hv_col_in, hv_Indices, out hv_col_out);



            /* 針對每一列的marks，對column做排序 (由小到大) */
            hv_i1 = 0;
            hv_i2 = 0;
            HTuple end_val20 = hv_count_row - 1;
            HTuple step_val20 = 1;
            for (hv_r = 0; hv_r.Continue(end_val20, step_val20); hv_r = hv_r.TupleAdd(step_val20))
            {
                hv_i2 = (hv_i2 + (hv_count_rowElement.TupleSelect(hv_r))) - 1;

                // 抽出此列的 row & column
                HOperatorSet.TupleSelectRange(hv_row_out, hv_i1, hv_i2, out hv_row_Selected);
                HOperatorSet.TupleSelectRange(hv_col_out, hv_i1, hv_i2, out hv_col_Selected);

                // 針對此列的column做排序 (由小到大)
                HOperatorSet.TupleSortIndex(hv_col_Selected, out hv_Indices1);
                tuple_relocate_index(hv_col_Selected, hv_Indices1, out hv_col_Selected);

                // 對應row交換位置
                tuple_relocate_index(hv_row_Selected, hv_Indices1, out hv_row_Selected);

                // 將 row_Selected & col_Selected 覆蓋掉原先的 row_out & col_out
                hv_index_replace = hv_i1.Clone();
                HTuple end_val36 = (hv_count_rowElement.TupleSelect(hv_r)) - 1;
                HTuple step_val36 = 1;
                for (hv_i = 1; hv_i.Continue(end_val36, step_val36); hv_i = hv_i.TupleAdd(step_val36))
                {
                    HOperatorSet.TupleConcat(hv_index_replace, hv_i1 + hv_i, out hv_index_replace);
                }
                HOperatorSet.TupleReplace(hv_row_out, hv_index_replace, hv_row_Selected, out hv_row_out);
                HOperatorSet.TupleReplace(hv_col_out, hv_index_replace, hv_col_Selected, out hv_col_out);

                // 下一列參數更新
                hv_i2 = hv_i2 + 1;
                hv_i1 = hv_i2.Clone();
            }

            return;
        }

        /// <summary>
        /// 根據給定Indices，重新配置tuple內容
        /// </summary>
        /// <param name="hv_tuple_in"></param>
        /// <param name="hv_Indices"></param>
        /// <param name="hv_tuple_out"></param>
        private void tuple_relocate_index(HTuple hv_tuple_in, HTuple hv_Indices, out HTuple hv_tuple_out) // (20181102) Jeff Revised!
        {
            // Local control variables 
            HTuple hv_Index = null;

            // Initialize local and output iconic variables 
            hv_tuple_out = new HTuple();

            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_tuple_in.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
            {
                if (hv_tuple_out == null)
                    hv_tuple_out = new HTuple();
                hv_tuple_out[hv_Index] = hv_tuple_in.TupleSelect(hv_Indices.TupleSelect(hv_Index));
            }
            return;
        }

        public List<PointF> get_total_marks_pos()
        {
            return all_marks_id_;
        }
    }
}
