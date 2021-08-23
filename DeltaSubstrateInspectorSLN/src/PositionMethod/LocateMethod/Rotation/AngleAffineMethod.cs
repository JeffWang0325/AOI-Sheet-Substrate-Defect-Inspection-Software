using HalconDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using static DeltaSubstrateInspector.FileSystem.FileSystem;

using DAVS;

namespace DeltaSubstrateInspector.src.PositionMethod.LocateMethod.Rotation
{
    [Serializable] // (20190824) Jeff Revised!
    public class AngleAffineMethod
    {
        public AngleAffineMethod_New angleAffineMethod_New = new AngleAffineMethod_New(); // (20200429) Jeff Revised!

        [NonSerialized] // (20190824) Jeff Revised!
        clsLog Log = new clsLog();

        [Serializable]
        public class clsEdgeSearchParaam
        {
            public enuAlignmentType AlignmentType = enuAlignmentType.PatternMatch;
            public int Th_Low = 0;
            public int Th_High = 100;
            public int DeNoiseSize = 3;
            public enuSearchType SearchType1 = enuSearchType.Center;
            public enuSearchType SearchType2 = enuSearchType.Center;
            public int OpeningSize = 10;
            public int ClosingSize = 100;
            public string TransType = "convex";

            public clsEdgeSearchParaam() { }

        }
        public enum enuAlignmentType
        {
            PatternMatch,
            EdgeSearch,
        }

        /// <summary>
        /// Quadratic equation with one unknown: ax^2+bx+c=0
        /// </summary>
        [Serializable]
        public class QuadraticEq_1Unknown
        {
            /// <summary>
            /// 公式解
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <param name="c"></param>
            /// <param name="x1">root1 (large)</param>
            /// <param name="x2">root2 (small)</param>
            /// <returns>true(有實數根), false(非一元二次方程式 or 無實數根)</returns>
            public bool roots(double a, double b, double c, out double x1, out double x2)
            {
                if (a == 0) // 非一元二次方程式
                {
                    x1 = x2 = -1;
                    return false;
                }

                double D = b * b - 4 * a * c; // 判別式

                if (D > 0) // 二實數根
                {
                    x1 = ((-b + Math.Sqrt(D)) / (2 * a));
                    x2 = ((-b - Math.Sqrt(D)) / (2 * a));
                }
                else if (D == 0) // 二重根
                {
                    x1 = x2 = -b / (2 * a);
                }
                else // 無實數根
                {
                    x1 = x2 = -1;
                    return false;
                }

                return true;
            }
        }

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
        /// 影像單位轉換成實際距離 (μm/pixel)
        /// </summary>
        public double pixel_resolution_ = 1.72;

        /// <summary>
        /// 是否計算 解析度 (μm/pixel)
        /// </summary>
        private bool b_compute_resolution = false;
        /// <summary>
        /// 是否計算 解析度 (μm/pixel)
        /// </summary>
        public bool B_compute_resolution
        {
            get { return this.b_compute_resolution; }
            set { this.b_compute_resolution = value; }
        }

        /// <summary>
        /// 計算得到之解析度 (μm/pixel)
        /// </summary>
        private double computed_resolution_ = 1.72;
        /// <summary>
        /// 計算得到之解析度 (μm/pixel)
        /// </summary>
        public double Computed_resolution_
        {
            get { return this.computed_resolution_; }
            set { this.computed_resolution_ = value; }
        }

        /// <summary>
        /// 兩定位影像機台移動距離x_hat = x1_hat - x0_hat (mm)
        /// </summary>
        private double motion_shift_dist_x_hat = 0.0;
        /// <summary>
        /// 兩定位影像機台移動距離x_hat = x1_hat - x0_hat (mm)
        /// </summary>
        public double Motion_shift_dist_x_hat
        {
            get { return this.motion_shift_dist_x_hat; }
            set { this.motion_shift_dist_x_hat = value; }
        }

        /// <summary>
        /// 兩定位影像機台移動距離y_hat = y1_hat - y0_hat (mm)
        /// </summary>
        private double motion_shift_dist_y_hat = 97.1;
        /// <summary>
        /// 兩定位影像機台移動距離y_hat = y1_hat - y0_hat (mm)
        /// </summary>
        public double Motion_shift_dist_y_hat
        {
            get { return this.motion_shift_dist_y_hat; }
            set { this.motion_shift_dist_y_hat = value; }
        }
        
        /// <summary>
        /// Mark1基板座標X (mm)
        /// </summary>
        private double mark1_rpos_sample_X = 0.0;
        /// <summary>
        /// Mark1基板座標X (mm)
        /// </summary>
        public double Mark1_rpos_sample_X
        {
            get { return this.mark1_rpos_sample_X; }
            set { this.mark1_rpos_sample_X = value; }
        }

        /// <summary>
        /// Mark1基板座標Y (mm)
        /// </summary>
        private double mark1_rpos_sample_Y = 0.0;
        /// <summary>
        /// Mark1基板座標Y (mm)
        /// </summary>
        public double Mark1_rpos_sample_Y
        {
            get { return this.mark1_rpos_sample_Y; }
            set { this.mark1_rpos_sample_Y = value; }
        }

        /// <summary>
        /// Mark2基板座標X (mm)
        /// </summary>
        private double mark2_rpos_sample_X = 0.0;
        /// <summary>
        /// Mark2基板座標X (mm)
        /// </summary>
        public double Mark2_rpos_sample_X
        {
            get { return this.mark2_rpos_sample_X; }
            set { this.mark2_rpos_sample_X = value; }
        }

        /// <summary>
        /// Mark2基板座標Y (mm)
        /// </summary>
        private double mark2_rpos_sample_Y = 62.7;
        /// <summary>
        /// Mark2基板座標Y (mm)
        /// </summary>
        public double Mark2_rpos_sample_Y
        {
            get { return this.mark2_rpos_sample_Y; }
            set { this.mark2_rpos_sample_Y = value; }
        }

        /// <summary>
        /// 2 Marks Distance (基板座標) (mm)
        /// </summary>
        private double marks_dist_sample = 62.7;
        /// <summary>
        /// 2 Marks Distance (基板座標) (mm)
        /// </summary>
        public double Marks_dist_sample
        {
            get { return this.marks_dist_sample; }
            set { this.marks_dist_sample = value; }
        }

        /// <summary>
        /// 是否計算 2 Marks Distance
        /// </summary>
        private bool b_compute_marks_dist = false;
        /// <summary>
        /// 是否計算 2 Marks Distance
        /// </summary>
        public bool B_compute_marks_dist
        {
            get { return this.b_compute_marks_dist; }
            set { this.b_compute_marks_dist = value; }
        }

        /// <summary>
        /// Computed 2 Marks Distance (運動座標) (mm)
        /// </summary>
        private double marks_dist_motion_compute = 62.7;
        /// <summary>
        /// Computed 2 Marks Distance (運動座標) (mm)
        /// </summary>
        public double Marks_dist_motion_compute
        {
            get { return this.marks_dist_motion_compute; }
            set { this.marks_dist_motion_compute = value; }
        }

        /// <summary>
        /// 是否卡控Marks間距誤差
        /// </summary>
        private bool b_control_marks_dist = false;
        /// <summary>
        /// 是否卡控Marks間距誤差
        /// </summary>
        public bool B_control_marks_dist
        {
            get { return this.b_control_marks_dist; }
            set { this.b_control_marks_dist = value; }
        }

        /// <summary>
        /// 卡控Marks間距誤差大小 (mm)
        /// </summary>
        private double diff_controlMarksDist = 1.0;
        /// <summary>
        /// 卡控Marks間距誤差大小 (mm)
        /// </summary>
        public double Diff_controlMarksDist
        {
            get { return this.diff_controlMarksDist; }
            set { this.diff_controlMarksDist = value; }
        }

        /// <summary>
        /// 基板座標上兩marks夾角 (degree)
        /// </summary>
        private double angle_degree_marks_sample = 90.0;
        /// <summary>
        /// 基板座標上兩marks夾角 (degree)
        /// </summary>
        public double Angle_degree_marks_sample
        {
            get { return this.angle_degree_marks_sample; }
            set { this.angle_degree_marks_sample = value; }
        }

        /// <summary>
        /// create_shape_model()中的Contrast
        /// </summary>
        public string Contrast_string = "5, 12, 65"; // (20180904) Jeff Revised!
        public HTuple Contrast_HTuple = new HTuple(); // (20180904) Jeff Revised!
        //public bool AutoParam = false; // (20190323) Jeff Revised!
        private bool autoParam = false;
        public bool AutoParam
        {
            get { return this.autoParam; }
            set { this.autoParam = value; }
        }
        /// <summary>
        /// create_shape_model()中的MinContrast
        /// </summary>
        public int MinContrast = 5; // (20180828) Jeff Revised!
        /// <summary>
        /// 匹配最低分數
        /// </summary>
        public double match_score_ = 0.1;

        /// <summary>
        /// Edge Search Param
        /// </summary>
        public clsEdgeSearchParaam EdgeSearchParam = new clsEdgeSearchParaam();



        /// <summary>
        /// 根目錄是否有Model存在
        /// </summary>
        private bool b_model_exist_ = false; // (20181119) Jeff Revised!
        public bool B_model_exist_ // (20181119) Jeff Revised!
        {
            get { return b_model_exist_; }
        }

        /// <summary>
        /// 需要扭正之角度 (degree)
        /// </summary>
        private HTuple affine_degree_ = null;
        /// <summary>
        /// 需要扭正之角度 (degree)
        /// </summary>
        public HTuple Affine_degree_ // (20190112) Jeff Revised!
        {
            get { return affine_degree_; }
            set { affine_degree_ = value; }
        }
        /// <summary>
        /// 需要旋轉之角度 (degree)的補償值
        /// </summary>
        private HTuple affine_degree_shift = 0.0; // (20190112) Jeff Revised!
        /// <summary>
        /// 需要旋轉之角度 (degree)的補償值
        /// </summary>
        public HTuple Affine_degree_shift // (20190112) Jeff Revised!
        {
            get { return affine_degree_shift; }
            set { affine_degree_shift = value; }
        }
        /// <summary>
        /// 檢測到兩影像定位點之中心位置X (pixel)
        /// </summary>
        public HTuple[] mark_x_ { get; set; } = new HTuple[2];
        /// <summary>
        /// 檢測到兩影像定位點之中心位置Y (pixel)
        /// </summary>
        public HTuple[] mark_y_ { get; set; } = new HTuple[2];
        /// <summary>
        /// 已扭正Mark1 Image之定位點之中心位置 (pixel)
        /// </summary>
        public HTuple affine_mark1_x_, affine_mark1_y_;

        /// <summary>
        /// 使用者框出ROI的region (For shape model)
        /// </summary>
        private HObject model_region_;
        public HObject Model_region_
        {
            get { return model_region_; }
        }
        private HTuple ModelID_ = null; // (20190323) Jeff Revised!
        private HObject model_contour_;

        /// <summary>
        /// 使用者框出ROI的region (For mark ROI) (20181119) Jeff Revised!
        /// </summary>
        private HObject mark_ROI = null;
        public HObject Mark_ROI // (20181119) Jeff Revised!
        {
            get { return mark_ROI; }
        }

        private HObject EdgeResRegion1 = null;
        private HObject EdgeResRegion2 = null;
        public HObject GetEdgeResRegion1
        {
            get { return EdgeResRegion1; }
        }
        public HObject GetEdgeResRegion2
        {
            get { return EdgeResRegion2; }
        }


        private bool b_mark_ROI_exist = false;
        public bool B_mark_ROI_exist // (20181119) Jeff Revised!
        {
            get { return b_mark_ROI_exist; }
        }

        /// <summary>
        /// 目前暫存之 Mark1 和 Mark2 結果影像 (20190105) Jeff Revised!
        /// </summary>
        private HObject now_Mark1_ResultImg = null, now_Mark2_ResultImg = null;
        public HObject Now_Mark1_ResultImg // (20190105) Jeff Revised!
        {
            get { return now_Mark1_ResultImg; }
        }
        public HObject Now_Mark2_ResultImg // (20190105) Jeff Revised!
        {
            get { return now_Mark2_ResultImg; }
        }


        //*****************************************************************************
        public AngleAffineMethod() // (20180904) Jeff Revised!
        {
            string_2_HTuple(Contrast_string, out Contrast_HTuple, true);
        }

        /// <summary>
        /// 將 輸入字串 轉成 HTuple
        /// </summary>
        /// <param name="str_in">輸入字串，各元素用逗號","分隔</param>
        /// <param name="HTuple_out"></param>
        /// <param name="b_int">數值是否為整數</param>
        /// <returns></returns>
        public bool string_2_HTuple(string str_in, out HTuple HTuple_out, bool b_int = false) // (20180903) Jeff Revised!
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
        public bool HTuple_2_string(HTuple HTuple_in, out string str_out) // (20180903) Jeff Revised!
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
                        str_out += ",";
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
            file_directory_ = ModuleParamDirectory + PositionParam + "\\" + board_type_;
            if (!Directory.Exists(file_directory_))
                Directory.CreateDirectory(file_directory_);

            b_model_exist_ = read_shape_model();
            b_mark_ROI_exist = read_mark_ROI(); // (20181119) Jeff Revised!
            this.EdgeSearchParam = LoadXML();
            try
            {
                //if (!System.IO.File.Exists(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\Rotate Image.xml"))
                if (!System.IO.File.Exists(ModuleParamDirectory + PositionParam + "\\Rotate Image.xml"))
                    return false;
                XmlDocument xml_doc_ = new XmlDocument();
                //xml_doc_.Load(ModuleParamDirectory + RecipeFileDirectory + ModuleName + "\\Rotate Image.xml");
                xml_doc_.Load(ModuleParamDirectory + PositionParam + "\\Rotate Image.xml");

                XmlNode xml_node_ = xml_doc_.SelectSingleNode("Board_Type/Type_" + board_type_);
                if (xml_node_ == null)
                    return false;
                else
                {
                    // 更新參數
                    XmlElement xml_ele_ = (XmlElement)xml_node_.SelectSingleNode("Mark_Rotating");
                    pixel_resolution_ = Convert.ToDouble(xml_ele_.GetAttribute("Pixel_Resolution"));
                    motion_shift_dist_y_hat = Convert.ToDouble(xml_ele_.GetAttribute("Motion_Shift_Distance"));
                    Contrast_string = xml_ele_.GetAttribute("Contrast_string"); // (20180904) Jeff Revised!
                    string_2_HTuple(Contrast_string, out Contrast_HTuple, true); // (20180904) Jeff Revised!
                    MinContrast = Convert.ToInt32(xml_ele_.GetAttribute("MinContrast")); // (20180828) Jeff Revised!
                    match_score_ = Convert.ToDouble(xml_ele_.GetAttribute("Minimum_Match_Score"));
                    affine_degree_shift = Convert.ToDouble(xml_ele_.GetAttribute("affine_degree_shift")); // (20190112) Jeff Revised!

                    // (20190611) Jeff Revised!
                    try
                    {
                        // 定位測試資訊
                        b_control_marks_dist = Convert.ToBoolean(xml_ele_.GetAttribute("b_control_marks_dist"));
                        diff_controlMarksDist = Convert.ToDouble(xml_ele_.GetAttribute("diff_controlMarksDist"));

                        // 運動座標
                        motion_shift_dist_x_hat = Convert.ToDouble(xml_ele_.GetAttribute("Motion_Shift_Distance_x_hat"));

                        // 基板座標
                        mark1_rpos_sample_X = Convert.ToDouble(xml_ele_.GetAttribute("mark1_rpos_sample_X"));
                        mark1_rpos_sample_Y = Convert.ToDouble(xml_ele_.GetAttribute("mark1_rpos_sample_Y"));
                        mark2_rpos_sample_X = Convert.ToDouble(xml_ele_.GetAttribute("mark2_rpos_sample_X"));
                        mark2_rpos_sample_Y = Convert.ToDouble(xml_ele_.GetAttribute("mark2_rpos_sample_Y"));
                        marks_dist_sample = Convert.ToDouble(xml_ele_.GetAttribute("marks_dist_sample"));
                        angle_degree_marks_sample = Convert.ToDouble(xml_ele_.GetAttribute("angle_degree_marks_sample"));
                    }
                    catch
                    {
                        #region 初始化參數
                        // 定位測試資訊
                        b_control_marks_dist = false;
                        diff_controlMarksDist = 1.0;

                        // 運動座標
                        motion_shift_dist_x_hat = 0.0;

                        // 基板座標
                        mark1_rpos_sample_X = 0.0;
                        mark1_rpos_sample_Y = 0.0;
                        mark2_rpos_sample_X = 0.0;
                        mark2_rpos_sample_Y = 62.7;
                        marks_dist_sample = 62.7;
                        angle_degree_marks_sample = 90.0;
                        #endregion
                    }

                    #region AngleAffineMethod_New

                    if (clsStaticTool.LoadXML(file_directory_ + "\\angleAffineMethod_New.xml", out this.angleAffineMethod_New) == false) // (20200429) Jeff Revised!
                    {
                        this.angleAffineMethod_New = new AngleAffineMethod_New();
                        //return false;
                    }

                    #endregion

                    b_status_ = true;
                }            
            }
            catch (Exception ex)
            {
                //Console.Write(ex.ToString());

                b_status_ = false;
            }

            return b_status_;
        }

        public bool DeleteParam()
        {
            bool b_status_ = false;
            try
            {
                XmlDocument xml_doc_ = new XmlDocument();
                if (System.IO.File.Exists(ModuleParamDirectory + PositionParam + "\\Rotate Image.xml"))
                    xml_doc_.Load(ModuleParamDirectory + PositionParam + "\\Rotate Image.xml");

                XmlNode xml_node_ = xml_doc_.SelectSingleNode("Board_Type");
                if (xml_node_ == null)
                {
                    return false;
                }
                XmlNode xml_type_ = xml_doc_.SelectSingleNode("Board_Type/Type_" + board_type_);

                if (xml_type_ != null)
                {
                    xml_node_.RemoveChild(xml_type_);
                }
                xml_doc_.Save(ModuleParamDirectory + PositionParam + "\\Rotate Image.xml");

                string Path = ModuleParamDirectory + PositionParam + "\\" + board_type_ + "\\";

                Directory.Delete(Path, true);

                b_status_ = true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                b_status_ = false;
            }

            return b_status_;
        }

        public bool SaveAs(string ParamName)
        {
            bool b_status_ = false;
            try
            {

                XmlDocument xml_doc_ = new XmlDocument();
                if (System.IO.File.Exists(ModuleParamDirectory + PositionParam + "\\Rotate Image.xml"))
                    xml_doc_.Load(ModuleParamDirectory + PositionParam + "\\Rotate Image.xml");

                XmlNode xml_node_ = xml_doc_.SelectSingleNode("Board_Type");
                if (xml_node_ == null)
                {
                    xml_node_ = xml_doc_.CreateElement("Board_Type");
                    xml_doc_.AppendChild(xml_node_);
                }

                XmlNode xml_type_ = xml_doc_.SelectSingleNode("Board_Type/Type_" + ParamName);
                if (xml_type_ == null)
                {
                    xml_type_ = xml_doc_.CreateElement("Type_" + ParamName);
                    xml_node_.AppendChild(xml_type_);

                    XmlElement xml_ele_ = xml_doc_.CreateElement("Mark_Rotating");
                    xml_ele_.SetAttribute("Pixel_Resolution", pixel_resolution_.ToString());
                    xml_ele_.SetAttribute("Motion_Shift_Distance", motion_shift_dist_y_hat.ToString());
                    HTuple_2_string(Contrast_HTuple, out Contrast_string); // (20181112) Jeff Revised!
                    xml_ele_.SetAttribute("Contrast_string", Contrast_string); // (20181112) Jeff Revised!
                    xml_ele_.SetAttribute("MinContrast", MinContrast.ToString()); // (20180904) Jeff Revised!
                    xml_ele_.SetAttribute("Minimum_Match_Score", match_score_.ToString());
                    xml_ele_.SetAttribute("affine_degree_shift", affine_degree_shift.ToString()); // (20190112) Jeff Revised!

                    // (20190516) Jeff Revised!
                    try
                    {
                        // 定位測試資訊
                        xml_ele_.SetAttribute("b_control_marks_dist", b_control_marks_dist.ToString());
                        xml_ele_.SetAttribute("diff_controlMarksDist", diff_controlMarksDist.ToString());

                        // 運動座標
                        xml_ele_.SetAttribute("Motion_Shift_Distance_x_hat", motion_shift_dist_x_hat.ToString());

                        // 基板座標
                        xml_ele_.SetAttribute("mark1_rpos_sample_X", mark1_rpos_sample_X.ToString());
                        xml_ele_.SetAttribute("mark1_rpos_sample_Y", mark1_rpos_sample_Y.ToString());
                        xml_ele_.SetAttribute("mark2_rpos_sample_X", mark2_rpos_sample_X.ToString());
                        xml_ele_.SetAttribute("mark2_rpos_sample_Y", mark2_rpos_sample_Y.ToString());
                        xml_ele_.SetAttribute("marks_dist_sample", marks_dist_sample.ToString());
                        xml_ele_.SetAttribute("angle_degree_marks_sample", angle_degree_marks_sample.ToString());
                    }
                    catch
                    {

                    }

                    xml_type_.AppendChild(xml_ele_);
                }
                xml_doc_.Save(ModuleParamDirectory + PositionParam + "\\Rotate Image.xml");
                string Path = ModuleParamDirectory + PositionParam + "\\" + ParamName + "\\";

                Directory.CreateDirectory(Path);
                
                // 儲存 mark ROI (20190509) Jeff Revised!
                if (mark_ROI != null)
                    HOperatorSet.WriteRegion(mark_ROI, Path + ParamName + "_MarkROI.hobj");

                // 儲存模板
                HOperatorSet.WriteShapeModel(ModelID_, Path + ParamName + "_ModelID");
                HOperatorSet.WriteRegion(model_region_, Path + ParamName + "_ModelRegion.hobj");
                
                SaveXML(Path, ParamName);
                b_status_ = true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
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
                SaveXML();
                XmlDocument xml_doc_ = new XmlDocument();
                if (System.IO.File.Exists(ModuleParamDirectory + PositionParam + "\\Rotate Image.xml"))
                    xml_doc_.Load(ModuleParamDirectory + PositionParam + "\\Rotate Image.xml");

                XmlNode xml_node_ = xml_doc_.SelectSingleNode("Board_Type");
                if(xml_node_==null)
                {
                    xml_node_ = xml_doc_.CreateElement("Board_Type");
                    xml_doc_.AppendChild(xml_node_);
                }

                XmlNode xml_type_ = xml_doc_.SelectSingleNode("Board_Type/Type_" + board_type_);
                if(xml_type_==null)
                {
                    xml_type_ = xml_doc_.CreateElement("Type_" + board_type_);
                    xml_node_.AppendChild(xml_type_);

                    XmlElement xml_ele_ = xml_doc_.CreateElement("Mark_Rotating");
                    xml_ele_.SetAttribute("Pixel_Resolution", pixel_resolution_.ToString());
                    xml_ele_.SetAttribute("Motion_Shift_Distance", motion_shift_dist_y_hat.ToString());
                    HTuple_2_string(Contrast_HTuple, out Contrast_string); // (20181112) Jeff Revised!
                    xml_ele_.SetAttribute("Contrast_string", Contrast_string); // (20181112) Jeff Revised!
                    xml_ele_.SetAttribute("MinContrast", MinContrast.ToString()); // (20180904) Jeff Revised!
                    xml_ele_.SetAttribute("Minimum_Match_Score", match_score_.ToString());
                    xml_ele_.SetAttribute("affine_degree_shift", affine_degree_shift.ToString()); // (20190112) Jeff Revised!

                    // (20190516) Jeff Revised!
                    try
                    {
                        // 定位測試資訊
                        xml_ele_.SetAttribute("b_control_marks_dist", b_control_marks_dist.ToString());
                        xml_ele_.SetAttribute("diff_controlMarksDist", diff_controlMarksDist.ToString());

                        // 運動座標
                        xml_ele_.SetAttribute("Motion_Shift_Distance_x_hat", motion_shift_dist_x_hat.ToString());

                        // 基板座標
                        xml_ele_.SetAttribute("mark1_rpos_sample_X", mark1_rpos_sample_X.ToString());
                        xml_ele_.SetAttribute("mark1_rpos_sample_Y", mark1_rpos_sample_Y.ToString());
                        xml_ele_.SetAttribute("mark2_rpos_sample_X", mark2_rpos_sample_X.ToString());
                        xml_ele_.SetAttribute("mark2_rpos_sample_Y", mark2_rpos_sample_Y.ToString());
                        xml_ele_.SetAttribute("marks_dist_sample", marks_dist_sample.ToString());
                        xml_ele_.SetAttribute("angle_degree_marks_sample", angle_degree_marks_sample.ToString());
                    }
                    catch
                    {

                    }

                    xml_type_.AppendChild(xml_ele_);
                }
                else
                {
                    XmlNode xml_ele_ = xml_doc_.SelectSingleNode("Board_Type/Type_" + board_type_ + "/ Mark_Rotating");
                    xml_ele_.Attributes["Pixel_Resolution"].Value = pixel_resolution_.ToString();
                    xml_ele_.Attributes["Motion_Shift_Distance"].Value = motion_shift_dist_y_hat.ToString();
                    HTuple_2_string(Contrast_HTuple, out Contrast_string); // (20181112) Jeff Revised!
                    xml_ele_.Attributes["Contrast_string"].Value = Contrast_string; // (20181112) Jeff Revised!
                    xml_ele_.Attributes["MinContrast"].Value = MinContrast.ToString(); // (20180904) Jeff Revised!
                    xml_ele_.Attributes["Minimum_Match_Score"].Value = match_score_.ToString();
                    xml_ele_.Attributes["affine_degree_shift"].Value = affine_degree_shift.ToString(); // (20190112) Jeff Revised!

                    // (20190516) Jeff Revised!
                    try
                    {
                        // 定位測試資訊
                        xml_ele_.Attributes["b_control_marks_dist"].Value = b_control_marks_dist.ToString();
                        xml_ele_.Attributes["diff_controlMarksDist"].Value = diff_controlMarksDist.ToString();

                        // 運動座標
                        xml_ele_.Attributes["Motion_Shift_Distance_x_hat"].Value = motion_shift_dist_x_hat.ToString();

                        // 基板座標
                        xml_ele_.Attributes["mark1_rpos_sample_X"].Value = mark1_rpos_sample_X.ToString();
                        xml_ele_.Attributes["mark1_rpos_sample_Y"].Value = mark1_rpos_sample_Y.ToString();
                        xml_ele_.Attributes["mark2_rpos_sample_X"].Value = mark2_rpos_sample_X.ToString();
                        xml_ele_.Attributes["mark2_rpos_sample_Y"].Value = mark2_rpos_sample_Y.ToString();
                        xml_ele_.Attributes["marks_dist_sample"].Value = marks_dist_sample.ToString();
                        xml_ele_.Attributes["angle_degree_marks_sample"].Value = angle_degree_marks_sample.ToString();
                    }
                    catch (Exception ex)
                    {

                    }
                }

                xml_doc_.Save(ModuleParamDirectory + PositionParam + "\\Rotate Image.xml");

                // 儲存 mark ROI (20190509) Jeff Revised!
                if (mark_ROI != null)
                    HOperatorSet.WriteRegion(mark_ROI, file_directory_ + "\\" + board_type_ + "_MarkROI.hobj");

                // 儲存模板
                HOperatorSet.WriteShapeModel(ModelID_, file_directory_ + "\\" + board_type_ + "_ModelID");
                HOperatorSet.WriteRegion(model_region_, file_directory_ + "\\" + board_type_ + "_ModelRegion.hobj");

                #region AngleAffineMethod_New

                b_status_ = clsStaticTool.SaveXML(this.angleAffineMethod_New, file_directory_ + "\\angleAffineMethod_New.xml"); // (20200429) Jeff Revised!

                #endregion

                //b_status_ = true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                b_status_ = false;
            }

            return b_status_;
        }

        public bool create_Edge_ROI(HTuple hWindow_, HObject input_image_)
        {
            bool b_status_ = false;
            HObject gray_image_, reduce_image_;
            HTuple row1_, column1_, phi_, length1_, length2_;
            
            try
            {
                HOperatorSet.Rgb1ToGray(input_image_, out gray_image_);
                HOperatorSet.SetColor(hWindow_, "red");
                HOperatorSet.DrawRectangle2(hWindow_, out row1_, out column1_, out phi_, out length1_, out length2_);
                HOperatorSet.GenRectangle2(out model_region_, row1_, column1_, phi_, length1_, length2_);
                HOperatorSet.ReduceDomain(gray_image_, model_region_, out reduce_image_);
                
                HOperatorSet.WriteRegion(model_region_, file_directory_ + "\\" + board_type_ + "_SearchEdgeRegion.hobj");
                
                HOperatorSet.DispObj(input_image_, hWindow_);
                HOperatorSet.SetColor(hWindow_, "red");
                HOperatorSet.SetDraw(hWindow_, "margin");
                HOperatorSet.DispRegion(model_region_, hWindow_);
                
                b_model_exist_ = true; // (20181119) Jeff Revised!
                b_status_ = true;
            }
            catch (Exception ex)
            {
                b_status_ = false;
            }

            return b_status_;
        }


        /// <summary>
        /// 建立匹配模組
        /// </summary>
        /// <param name="hWindow_">預顯示的hWindow</param>
        /// <param name="input_image_">輸入影像</param>
        /// <returns></returns>
        public bool create_shape_model(HTuple hWindow_, HObject input_image_)
        {
            bool b_status_ = false;

            try
            {
                HObject gray_image_, reduce_image_;
                HObject trans_contours_;
                HTuple row1_, column1_, phi_, length1_, length2_;
                HTuple area_, row_, column_, HomMat2D_;

                HOperatorSet.Rgb1ToGray(input_image_, out gray_image_);
                HOperatorSet.SetColor(hWindow_, "red");
                HOperatorSet.DrawRectangle2(hWindow_, out row1_, out column1_, out phi_, out length1_, out length2_);
                HOperatorSet.GenRectangle2(out model_region_, row1_, column1_, phi_, length1_, length2_);
                HOperatorSet.ReduceDomain(gray_image_, model_region_, out reduce_image_);
                clear_shape_model(); // (20190323) Jeff Revised!
                if (autoParam)
                    HOperatorSet.CreateShapeModel(reduce_image_, "auto", new HTuple(0).TupleRad(), new HTuple(360).TupleRad(), "auto", "auto", "use_polarity", "auto", "auto", out ModelID_); // For 新竹光頡
                else
                    HOperatorSet.CreateShapeModel(reduce_image_, "auto", new HTuple(0).TupleRad(), new HTuple(360).TupleRad(), "auto", "auto", "use_polarity", Contrast_HTuple, MinContrast, out ModelID_); // (20180904) Jeff Revised!

                // 儲存模板 (20190509) Jeff Revised!
                //HOperatorSet.WriteShapeModel(ModelID_, file_directory_ + "\\" + board_type_ + "_ModelID");
                //HOperatorSet.WriteRegion(model_region_, file_directory_ + "\\" + board_type_ + "_ModelRegion.hobj");

                HOperatorSet.GetShapeModelContours(out model_contour_, ModelID_, 1);
                HOperatorSet.AreaCenter(model_region_, out area_, out row_, out column_);
                HOperatorSet.VectorAngleToRigid(0, 0, 0, row_, column_, 0, out HomMat2D_);
                HOperatorSet.AffineTransContourXld(model_contour_, out trans_contours_, HomMat2D_);

                //HOperatorSet.DispColor(input_image_, hWindow_);
                HOperatorSet.SetColor(hWindow_, "red");
                HOperatorSet.SetDraw(hWindow_, "margin");
                HOperatorSet.DispRegion(model_region_, hWindow_);
                //HOperatorSet.DispObj(model_contour_, hWindow_); // For debug! (20180822) Jeff Revised!
                HOperatorSet.DispObj(trans_contours_, hWindow_);

                #region Release memories
                gray_image_.Dispose();
                reduce_image_.Dispose();
                trans_contours_.Dispose();
                #endregion

                b_model_exist_ = true; // (20181119) Jeff Revised!
                b_status_ = true;
            }
            catch (Exception ex)
            {
                b_status_ = false;
            }

            return b_status_;
        }

        public bool create_shape_model(HTuple hWindow_, HObject input_image_, HObject ROI) // (20190509) Jeff Revised!
        {
            bool b_status_ = false;

            try
            {
                HObject gray_image_, reduce_image_;
                HObject trans_contours_;
                HTuple area_, row_, column_, HomMat2D_;

                HOperatorSet.Rgb1ToGray(input_image_, out gray_image_);
                model_region_ = ROI.Clone();
                HOperatorSet.ReduceDomain(gray_image_, model_region_, out reduce_image_);
                clear_shape_model(); // (20190323) Jeff Revised!
                if (autoParam)
                    HOperatorSet.CreateShapeModel(reduce_image_, "auto", new HTuple(0).TupleRad(), new HTuple(360).TupleRad(), "auto", "auto", "use_polarity", "auto", "auto", out ModelID_); // For 新竹光頡
                else
                {
                    //HOperatorSet.CreateShapeModel(reduce_image_, "auto", new HTuple(0).TupleRad(), new HTuple(360).TupleRad(), "auto", "auto", "use_polarity", Contrast_HTuple, MinContrast, out ModelID_); // (20180904) Jeff Revised!
                    HOperatorSet.CreateShapeModel(reduce_image_, "auto", new HTuple(0).TupleRad(), new HTuple(360).TupleRad(), "auto", "auto", "ignore_global_polarity", Contrast_HTuple, MinContrast, out ModelID_); // (20190517) Jeff Revised!
                    //HOperatorSet.CreateShapeModel(reduce_image_, "auto", new HTuple(0).TupleRad(), new HTuple(360).TupleRad(), "auto", "auto", "ignore_color_polarity", Contrast_HTuple, MinContrast, out ModelID_); // (20190517) Jeff Revised!
                    //HOperatorSet.CreateShapeModel(reduce_image_, "auto", new HTuple(0).TupleRad(), new HTuple(360).TupleRad(), "auto", "auto", "ignore_local_polarity", Contrast_HTuple, MinContrast, out ModelID_); // (20190517) Jeff Revised!
                }
                // 儲存模板 (20190509) Jeff Revised!
                //HOperatorSet.WriteShapeModel(ModelID_, file_directory_ + "\\" + board_type_ + "_ModelID");
                //HOperatorSet.WriteRegion(model_region_, file_directory_ + "\\" + board_type_ + "_ModelRegion.hobj");

                HOperatorSet.GetShapeModelContours(out model_contour_, ModelID_, 1);
                HOperatorSet.AreaCenter(model_region_, out area_, out row_, out column_);
                HOperatorSet.VectorAngleToRigid(0, 0, 0, row_, column_, 0, out HomMat2D_);
                HOperatorSet.AffineTransContourXld(model_contour_, out trans_contours_, HomMat2D_);

                // For debug! (20190517) Jeff Revised!
                //clear_shape_model();
                //HOperatorSet.CreateShapeModelXld(model_contour_, "auto", new HTuple(0).TupleRad(), new HTuple(360).TupleRad(), "auto", "auto", "ignore_local_polarity", MinContrast, out ModelID_);
                //HOperatorSet.GetShapeModelContours(out model_contour_, ModelID_, 1);
                //HTuple order;
                //HOperatorSet.AreaCenterXld(model_contour_, out area_, out row_, out column_, out order);
                //HOperatorSet.VectorAngleToRigid(0, 0, 0, row_, column_, 0, out HomMat2D_);
                //HOperatorSet.AffineTransContourXld(model_contour_, out trans_contours_, HomMat2D_);

                //HOperatorSet.DispColor(input_image_, hWindow_);
                HOperatorSet.SetColor(hWindow_, "red");
                HOperatorSet.SetDraw(hWindow_, "margin");
                HOperatorSet.DispRegion(model_region_, hWindow_);
                //HOperatorSet.DispObj(model_contour_, hWindow_); // For debug! (20180822) Jeff Revised!
                HOperatorSet.DispObj(trans_contours_, hWindow_);

                #region Release memories
                gray_image_.Dispose();
                reduce_image_.Dispose();
                trans_contours_.Dispose();
                #endregion

                b_model_exist_ = true; // (20181119) Jeff Revised!
                b_status_ = true;
            }
            catch (Exception ex)
            {
                b_status_ = false;
                if (ModelID_ != null) // (20190515) Jeff Revised!
                {
                    if (ModelID_.Type == HTupleType.EMPTY)
                        ModelID_ = null;
                }
            }

            return b_status_;
        }

        /// <summary>
        /// 清除 ModelID_
        /// </summary>
        private void clear_shape_model() // (20190323) Jeff Revised!
        {
            if (ModelID_ != null) // (20190515) Jeff Revised!
            {
                if (ModelID_.Type == HTupleType.EMPTY)
                    ModelID_ = null;
            }

            if (ModelID_ != null)
            {
                HOperatorSet.ClearShapeModel(ModelID_);
                ModelID_ = null;
            }
        }

        /// <summary>
        /// 建立Mark位置ROI
        /// </summary>
        /// <param name="hWindow_"></param>
        /// <param name="input_image_"></param>
        /// <returns></returns>
        public bool create_mark_ROI(HTuple hWindow_, HObject input_image_) // (20181119) Jeff Revised!
        {
            bool b_status_ = false;

            try
            {
                HTuple row1_, column1_, phi_, length1_, length2_;
                HOperatorSet.SetColor(hWindow_, "green");
                HOperatorSet.DrawRectangle2(hWindow_, out row1_, out column1_, out phi_, out length1_, out length2_);
                HOperatorSet.GenRectangle2(out mark_ROI, row1_, column1_, phi_, length1_, length2_);

                // 儲存 mark ROI (20190509) Jeff Revised!
                //HOperatorSet.WriteRegion(mark_ROI, file_directory_ + "\\" + board_type_ + "_MarkROI.hobj");

                //HOperatorSet.DispColor(input_image_, hWindow_);
                HOperatorSet.DispObj(input_image_, hWindow_);
                HOperatorSet.SetColor(hWindow_, "green");
                HOperatorSet.SetDraw(hWindow_, "margin");
                HOperatorSet.DispRegion(mark_ROI, hWindow_);

                b_mark_ROI_exist = true;
                b_status_ = true;
            }
            catch (Exception ex)
            {
                b_mark_ROI_exist = false;
                b_status_ = false;
            }

            return b_status_;
        }

        public bool create_mark_ROI(HTuple hWindow_, HObject input_image_, HObject ROI) // (20190509) Jeff Revised!
        {
            bool b_status_ = false;

            try
            {
                mark_ROI = ROI.Clone();
                
                HOperatorSet.DispObj(input_image_, hWindow_);
                HOperatorSet.SetColor(hWindow_, "green");
                HOperatorSet.SetDraw(hWindow_, "margin");
                HOperatorSet.DispRegion(mark_ROI, hWindow_);

                b_mark_ROI_exist = true;
                b_status_ = true;
            }
            catch (Exception ex)
            {
                b_mark_ROI_exist = false;
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
            clear_shape_model(); // (20190509) Jeff Revised!
            
            if (File.Exists(file_directory_ + "\\" + board_type_ + "_ModelID") && File.Exists(file_directory_ + "\\" + board_type_ + "_ModelRegion.hobj"))
            {
                //clear_shape_model(); // (20190323) Jeff Revised!
                HOperatorSet.ReadShapeModel(file_directory_ + "\\" + board_type_ + "_ModelID", out ModelID_);
                HOperatorSet.ReadRegion(out model_region_, file_directory_ + "\\" + board_type_ + "_ModelRegion.hobj");
                HOperatorSet.GetShapeModelContours(out model_contour_, ModelID_, 1);
                //HTuple area_, row_, column_, HomMat2D_;
                //HOperatorSet.AreaCenter(model_region_, out area_, out row_, out column_);
                //HOperatorSet.VectorAngleToRigid(0, 0, 0, row_, column_, 0, out HomMat2D_);
                //HObject trans_contours_;
                //HOperatorSet.AffineTransContourXld(model_contour_, out trans_contours_, HomMat2D_);
                //trans_contours_.Dispose();

                b_status_ = true;
            }
            return b_status_;
        }

        /// <summary>
        /// 讀取Mark位置ROI
        /// </summary>
        /// <returns></returns>
        public bool read_mark_ROI()
        {
            bool b_status_ = false;

            if (mark_ROI != null) // (20190509) Jeff Revised!
            {
                mark_ROI.Dispose();
                mark_ROI = null;
            }

            if (File.Exists(file_directory_ + "\\" + board_type_ + "_MarkROI.hobj"))
            {
                HOperatorSet.ReadRegion(out mark_ROI, file_directory_ + "\\" + board_type_ + "_MarkROI.hobj");              
                b_status_ = true;
            }
            return b_status_;
        }

        public bool read_SearchEdge_ROI() // (20181119) Jeff Revised!
        {
            bool b_status_ = false;

            if (File.Exists(file_directory_ + "\\" + board_type_ + "_SearchEdgeRegion.hobj"))
            {
                HOperatorSet.ReadRegion(out mark_ROI, file_directory_ + "\\" + board_type_ + "_SearchEdgeRegion.hobj");
                b_status_ = true;
            }
            return b_status_;
        }

        /// <summary>
        /// 顯示Mark位置ROI
        /// </summary>
        /// <param name="hWindow_"></param>
        /// <returns></returns>
        public bool disp_mark_ROI(HTuple hWindow_, HObject input_image_) // (20190509) Jeff Revised!
        {
            bool b_status_ = false;
            if (b_mark_ROI_exist && mark_ROI != null)
            {
                HOperatorSet.DispObj(input_image_, hWindow_);
                HOperatorSet.SetColor(hWindow_, "green");
                HOperatorSet.SetDraw(hWindow_, "margin");
                HOperatorSet.DispRegion(mark_ROI, hWindow_);
                b_status_ = true;
            }

            return b_status_;
        }

        /// <summary>
        /// 尋找定位點並產生旋轉矩陣
        /// </summary>
        /// <param name="hWindow_1_">預顯示的hWindow 1</param>
        /// <param name="mark_image_1_">定位點影像1</param>
        /// <param name="hWindow_2_">預顯示的hWindow 2</param>
        /// <param name="mark_image_2_">定位點影像2</param>
        /// <param name="b_save_Image">定位點影像2</param>
        /// <param name="b_draw_Image">是否將檢測結果影像暫存於程式中</param>
        /// <returns></returns>
        public bool find_shpe_model(HTuple hWindow_1_, HObject mark_image_1_, HTuple hWindow_2_, HObject mark_image_2_, bool b_save_Image = false, bool b_draw_Image = false) // (20200429) Jeff Revised!
        {
            bool b_status_ = false;
            HObject all_mark_image_;
            HTuple HomMat2D_;
            double[] score = { -1, -1 }; // (20181116) Jeff Revised!

            //b_mark_ROI_exist = read_mark_ROI(); // (20190510) Jeff Revised!
            //HOperatorSet.Rgb1ToGray(mark_image_1_.Clone(), out mark_image_1_); // (20190514) Jeff Revised!
            //HOperatorSet.Rgb1ToGray(mark_image_2_.Clone(), out mark_image_2_); // (20190514) Jeff Revised!
            // Method 1 (20181119) Jeff Revised!
            HOperatorSet.ConcatObj(mark_image_1_, mark_image_2_, out all_mark_image_);
            if (b_mark_ROI_exist & mark_ROI != null) // (20200429) Jeff Revised!
            {
                //HOperatorSet.ReduceDomain(all_mark_image_.Clone(), mark_ROI, out all_mark_image_);
                HObject ExpTmpOutVar_0;
                HOperatorSet.ReduceDomain(all_mark_image_, mark_ROI, out ExpTmpOutVar_0);
                all_mark_image_.Dispose();
                all_mark_image_ = ExpTmpOutVar_0;
            }
            // Method 2 (20181119) Jeff Revised!
            //if (b_mark_ROI_exist & mark_ROI != null)
            //{
            //    HObject mark_image_1_ROI, mark_image_2_ROI;
            //    HOperatorSet.ReduceDomain(mark_image_1_, mark_ROI, out mark_image_1_ROI);
            //    HOperatorSet.ReduceDomain(mark_image_2_, mark_ROI, out mark_image_2_ROI);
            //    HOperatorSet.ConcatObj(mark_image_1_ROI, mark_image_2_ROI, out all_mark_image_);
            //    HOperatorSet.WriteImage(mark_image_1_ROI, "tiff", 0, "mark_image_1_ROI.tiff"); // For debug! (20181119) Jeff Revised!
            //    HOperatorSet.WriteImage(mark_image_2_ROI, "tiff", 0, "mark_image_2_ROI.tiff"); // For debug! (20181119) Jeff Revised!
            //    mark_image_1_ROI.Dispose();
            //    mark_image_2_ROI.Dispose();
            //}
            //else
            //    HOperatorSet.ConcatObj(mark_image_1_, mark_image_2_, out all_mark_image_);

            for (int i = 1; i <= 2; i++)
            {
                HTuple find_row_, find_col_, find_angle_, find_score_;

                if (ModelID_ == null)
                {
                    // (20190509) Jeff Revised!
                    mark_x_[0] = mark_x_[1] = mark_y_[0] = mark_y_[1] = -1;
                    affine_degree_ = null;
                    break;
                }

                HObject select_image_;
                HOperatorSet.SelectObj(all_mark_image_, out select_image_, i);
                //HOperatorSet.WriteImage(select_image_, "tiff", 0, "select_image_.tiff"); // For debug! (20181119) Jeff Revised!
                HOperatorSet.FindShapeModel(select_image_, ModelID_, new HTuple(0).TupleRad(), new HTuple(360).TupleRad(), match_score_, 1, 0, "least_squares", 0, 0.9,
                                            out find_row_, out find_col_, out find_angle_, out find_score_); // (20181029) Jeff Revised!

                if (find_score_.Length > 0)
                {
                    score[i - 1] = find_score_[0].D; // (20181116) Jeff Revised!

                    HOperatorSet.HomMat2dIdentity(out HomMat2D_);
                    HOperatorSet.HomMat2dRotate(HomMat2D_, find_angle_, 0, 0, out HomMat2D_);
                    HOperatorSet.HomMat2dTranslate(HomMat2D_, find_row_, find_col_, out HomMat2D_);
                    HObject trans_contours_;
                    HOperatorSet.AffineTransContourXld(model_contour_, out trans_contours_, HomMat2D_);

                    /* 找到真正 Mark Contour中心 */
                    HObject trans_region_;
                    HTuple trans_row_, trans_col_;
                    HOperatorSet.GenRegionContourXld(trans_contours_, out trans_region_, "filled");
                    HOperatorSet.Union1(trans_region_, out trans_region_);
                    // Method 1: 取輪廓在x,y方向最外側點的中點
                    HOperatorSet.GetRegionPoints(trans_region_, out trans_row_, out trans_col_);
                    mark_x_[i - 1] = (trans_col_.TupleMax() + trans_col_.TupleMin()) / 2.0; // (20180823) Jeff Revised!
                    mark_y_[i - 1] = (trans_row_.TupleMax() + trans_row_.TupleMin()) / 2.0; // (20180823) Jeff Revised!
                    // Method 2: 取 contours' region 之中心 (20180823) Jeff Revised!
                    //HTuple area_, row_, column_;
                    //HOperatorSet.AreaCenter(trans_region_, out area_, out row_, out column_);
                    //mark_x_[i - 1] = column_;
                    //mark_y_[i - 1] = row_;

                    if (i == 1)
                    {
                        HOperatorSet.DispObj(mark_image_1_, hWindow_1_);
                        HOperatorSet.SetColor(hWindow_1_, "red");
                        HOperatorSet.DispObj(trans_contours_, hWindow_1_);
                    }
                    else
                    {
                        HOperatorSet.DispObj(mark_image_2_, hWindow_2_);
                        HOperatorSet.SetColor(hWindow_2_, "red");
                        HOperatorSet.DispObj(trans_contours_, hWindow_2_);
                    }

                    trans_region_.Dispose();
                    trans_contours_.Dispose();
                }
                else
                {
                    mark_x_[0] = mark_x_[1] = mark_y_[0] = mark_y_[1] = -1; // (20181107) Jeff Revised!
                    affine_degree_ = null; // (20181107) Jeff Revised!
                    break;
                }

                if (i == 2)
                    b_status_ = this.create_affine_trans();

                select_image_.Dispose();
            }
            all_mark_image_.Dispose();

            // 匹配/搜尋失敗處理機制
            if ((b_status_ == false) && (this.angleAffineMethod_New.DefaultMarksPosEnable)) // (20200429) Jeff Revised!
            {
                b_status_ = true;
                this.mark_x_[0] = this.angleAffineMethod_New.defMark1_x;
                this.mark_x_[1] = this.angleAffineMethod_New.defMark2_x;
                this.mark_y_[0] = this.angleAffineMethod_New.defMark1_y;
                this.mark_y_[1] = this.angleAffineMethod_New.defMark2_y;
                this.affine_degree_ = this.angleAffineMethod_New.defAffineAngle;
            }

            #region 定位測試&驗證

            if (b_save_Image || b_draw_Image) // (20190105) Jeff Revised!
            {
                try
                {
                    if (b_save_Image)
                        count_mark_image++;
                    string path1 = "D:/Position test/", path1_fileName = null;

                    if (b_status_) // 定位成功
                    {
                        if (b_save_Image)
                        {
                            path1_fileName = path1 + count_mark_image + " (Score1 = " + String.Format("{0:0.##}", score[0]) + ", Score2 = " + String.Format("{0:0.##}", score[1]) + ")";
                            //path1_fileName = path1 + count_mark_image + ", Mark1(" + mark_x_[0].TupleInt() + ", " + mark_y_[0].TupleInt() + "), Mark2(" + mark_x_[1].TupleInt() + ", " + mark_y_[1].TupleInt() + ")"; // (20181119) Jeff Revised!
                            path1_fileName += ", Mark1(" + mark_x_[0].TupleInt() + ", " + mark_y_[0].TupleInt() + "), Mark2(" + mark_x_[1].TupleInt() + ", " + mark_y_[1].TupleInt() + ")"; // (20181119) Jeff Revised!
                            CurrentPath_SaveImage = path1_fileName; // (20190112) Jeff Revised!
                            if (!(create_2Layers_file(path1, path1_fileName))) // 資料夾不存在且創建失敗
                                return b_status_;

                            path1_fileName += "/";
                            // 儲存定位原始影像
                            HOperatorSet.WriteImage(mark_image_1_, "tiff", 0, path1_fileName + "mark_image_1.tiff");
                            HOperatorSet.WriteImage(mark_image_2_, "tiff", 0, path1_fileName + "mark_image_2.tiff");
                        }

                        // Display center points of two mark images
                        HObject mark1_center = null;
                        HObject mark2_center = null;
                        HOperatorSet.GenCircle(out mark1_center, mark_y_[0], mark_x_[0], 10);
                        HOperatorSet.GenCircle(out mark2_center, mark_y_[1], mark_x_[1], 10);

                        // 轉成RGB image (20181225) Jeff Revised!
                        HTuple Channels;
                        HOperatorSet.CountChannels(mark_image_1_, out Channels);
                        if (Channels == 1)
                        {
                            HOperatorSet.Compose3(mark_image_1_.Clone(), mark_image_1_.Clone(), mark_image_1_.Clone(), out now_Mark1_ResultImg);
                            HOperatorSet.Compose3(mark_image_2_.Clone(), mark_image_2_.Clone(), mark_image_2_.Clone(), out now_Mark2_ResultImg);
                        }
                        else
                        {
                            HOperatorSet.CopyImage(mark_image_1_.Clone(), out now_Mark1_ResultImg);
                            HOperatorSet.CopyImage(mark_image_2_.Clone(), out now_Mark2_ResultImg);
                        }

                        // 儲存定位影像 + Cross Mark + 中心點 + Mark ROI (20181225) Jeff Revised!
                        HOperatorSet.OverpaintRegion(now_Mark1_ResultImg, gen_CrossMark(now_Mark1_ResultImg), ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "fill"); // (20181225) Jeff Revised!
                        HOperatorSet.OverpaintRegion(now_Mark2_ResultImg, gen_CrossMark(now_Mark2_ResultImg), ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "fill"); // (20181225) Jeff Revised!
                        HOperatorSet.OverpaintRegion(now_Mark1_ResultImg, mark1_center, ((new HTuple(0)).TupleConcat(0)).TupleConcat(255), "fill");
                        HOperatorSet.OverpaintRegion(now_Mark2_ResultImg, mark2_center, ((new HTuple(0)).TupleConcat(0)).TupleConcat(255), "fill");
                        if (b_mark_ROI_exist & mark_ROI != null) // (20181119) Jeff Revised!
                        {
                            HOperatorSet.OverpaintRegion(now_Mark1_ResultImg, mark_ROI, ((new HTuple(0)).TupleConcat(255)).TupleConcat(0), "margin");
                            HOperatorSet.OverpaintRegion(now_Mark2_ResultImg, mark_ROI, ((new HTuple(0)).TupleConcat(255)).TupleConcat(0), "margin");
                        }

                        if (b_save_Image)
                        {
                            HOperatorSet.WriteImage(now_Mark1_ResultImg, "tiff", 0, path1_fileName + "mark_image_1_center.tiff");
                            HOperatorSet.WriteImage(now_Mark2_ResultImg, "tiff", 0, path1_fileName + "mark_image_2_center.tiff");
                        }
                    }
                    else // 定位失敗
                    {
                        if (b_save_Image)
                        {
                            //MessageBox.Show("定位失敗!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            path1_fileName = path1 + count_mark_image + " (Failure)";
                            CurrentPath_SaveImage = path1_fileName; // (20190112) Jeff Revised!
                            if (!(create_2Layers_file(path1, path1_fileName))) // 資料夾不存在且創建失敗
                                return b_status_;

                            path1_fileName += "/";
                            // 儲存定位原始影像
                            HOperatorSet.WriteImage(mark_image_1_, "tiff", 0, path1_fileName + "mark_image_1.tiff");
                            HOperatorSet.WriteImage(mark_image_2_, "tiff", 0, path1_fileName + "mark_image_2.tiff");
                        }

                        // 轉成RGB image (20181225) Jeff Revised!
                        HTuple Channels;
                        HOperatorSet.CountChannels(mark_image_1_, out Channels);
                        if (Channels == 1)
                        {
                            HOperatorSet.Compose3(mark_image_1_.Clone(), mark_image_1_.Clone(), mark_image_1_.Clone(), out now_Mark1_ResultImg);
                            HOperatorSet.Compose3(mark_image_2_.Clone(), mark_image_2_.Clone(), mark_image_2_.Clone(), out now_Mark2_ResultImg);
                        }
                        else
                        {
                            HOperatorSet.CopyImage(mark_image_1_.Clone(), out now_Mark1_ResultImg);
                            HOperatorSet.CopyImage(mark_image_2_.Clone(), out now_Mark2_ResultImg);
                        }

                        // 儲存定位原始影像 + Cross Mark + Mark ROI (20181225) Jeff Revised!
                        HOperatorSet.OverpaintRegion(now_Mark1_ResultImg, gen_CrossMark(now_Mark1_ResultImg), ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "fill"); // (20181225) Jeff Revised!
                        HOperatorSet.OverpaintRegion(now_Mark2_ResultImg, gen_CrossMark(now_Mark2_ResultImg), ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "fill"); // (20181225) Jeff Revised!
                        if (b_mark_ROI_exist & mark_ROI != null)
                        {
                            HOperatorSet.OverpaintRegion(now_Mark1_ResultImg, mark_ROI, ((new HTuple(0)).TupleConcat(255)).TupleConcat(0), "margin");
                            HOperatorSet.OverpaintRegion(now_Mark2_ResultImg, mark_ROI, ((new HTuple(0)).TupleConcat(255)).TupleConcat(0), "margin");
                        }

                        if (b_save_Image)
                        {
                            HOperatorSet.WriteImage(now_Mark1_ResultImg, "tiff", 0, path1_fileName + "mark_image_1_ROI.tiff");
                            HOperatorSet.WriteImage(now_Mark2_ResultImg, "tiff", 0, path1_fileName + "mark_image_2_ROI.tiff");
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("儲存定位影像錯誤!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            #endregion

            return b_status_;
        }

        public HObject gen_CrossMark(HObject Image) // (20181225) Jeff Revised!
        {
            // Get image size
            HObject CrossRegion = null;
            HOperatorSet.GenEmptyObj(out CrossRegion);
            HTuple hv_Width = null, hv_Height = null;
            HTuple hv_CenterX = null, hv_CenterY = null;
            HOperatorSet.GetImageSize(Image, out hv_Width, out hv_Height);
            hv_CenterX = hv_Width / 2;
            hv_CenterY = hv_Height / 2;

            int Method = 0;
            if (Method == 0)
            {
                #region Method 0
                // Local iconic variables 
                HObject ho_CrossRegion1;
                HObject ho_Rectangle_h, ho_Rectangle_v;

                // Local control variables 
                HTuple hv_Row1 = null;
                HTuple hv_Column1 = null, hv_Row2 = null, hv_Column2 = null;
                HTuple hv_Range = null;

                // Initialize local and output iconic variables 
                HOperatorSet.GenEmptyObj(out ho_CrossRegion1);
                HOperatorSet.GenEmptyObj(out ho_Rectangle_h);
                HOperatorSet.GenEmptyObj(out ho_Rectangle_v);

                // Setting range
                hv_Range = 0;

                //Set cross feature 水平
                hv_Row1 = hv_CenterY - hv_Range;
                hv_Column1 = 0;
                //Column1 := CenterX-100
                hv_Row2 = hv_CenterY + hv_Range;
                hv_Column2 = hv_Width.Clone();
                //Column2 := CenterX+100
                ho_Rectangle_h.Dispose();
                HOperatorSet.GenRectangle1(out ho_Rectangle_h, hv_Row1, hv_Column1, hv_Row2,
                    hv_Column2);

                //Set cross feature 垂直
                hv_Row1 = 0;
                //Row1 := CenterY-100
                hv_Column1 = hv_CenterX - hv_Range;
                hv_Row2 = hv_Height.Clone();
                //Row2 := CenterY+100
                hv_Column2 = hv_CenterX + hv_Range;
                ho_Rectangle_v.Dispose();
                HOperatorSet.GenRectangle1(out ho_Rectangle_v, hv_Row1, hv_Column1, hv_Row2,
                    hv_Column2);

                //合併水平與垂直
                ho_CrossRegion1.Dispose();
                HOperatorSet.Union2(ho_Rectangle_h, ho_Rectangle_v, out ho_CrossRegion1);

                ho_Rectangle_h.Dispose();
                ho_Rectangle_v.Dispose();

                #endregion

                // Set show cross region
                CrossRegion = ho_CrossRegion1;
            }
            else if (Method == 1)
            {
                #region Method 1
                HObject ho_CrossRegion2;
                HOperatorSet.GenEmptyObj(out ho_CrossRegion2);
                HOperatorSet.GenCrossContourXld(out ho_CrossRegion2, hv_CenterY, hv_CenterX, hv_Width, 0);
                #endregion

                HOperatorSet.GenRegionContourXld(ho_CrossRegion2, out CrossRegion, "filled");


                // Set show cross region
                //ho_CrossRegion = ho_CrossRegion2;
            }

            return CrossRegion;
        }

        /// <summary>
        /// 創建兩層的資料夾
        /// </summary>
        /// <param name="path1">第一層資料夾路徑名稱</param>
        /// <param name="path1_fileName">第一層資料夾內的資料夾路徑名稱</param>
        /// <returns>true (資料夾存在或創建成功), false (創建失敗)</returns>
        private bool create_2Layers_file(string path1, string path1_fileName) // (20181116) Jeff Revised!
        {
            bool b_status_ = false;
            try
            {
                HTuple file_exist_ = 0;
                HOperatorSet.FileExists(path1, out file_exist_);
                if (file_exist_ == 1) // 資料夾存在
                {

                }
                else
                {
                    HOperatorSet.MakeDir(path1); // Create file
                }

                HOperatorSet.FileExists(path1_fileName, out file_exist_);
                if (file_exist_ == 1) // 資料夾存在
                {

                }
                else
                {
                    HOperatorSet.MakeDir(path1_fileName); // Create file
                }

                b_status_ = true;
            }
            catch
            {

            }

            return b_status_;
        }

        /// <summary>
        /// 影像扭正，得到扭正影像 及 已扭正Mark1 Image之定位點之中心位置 affine_mark1_x_, affine_mark1_y_
        /// </summary>
        /// <param name="hWindow_">預顯示的hWindow</param>
        /// <param name="input_image_">待扭正影像</param>
        /// <param name="output_image_">已扭正影像</param>
        /// <param name="b_save_rotatedImage">是否儲存扭正後影像</param>
        /// <returns></returns>
        public bool rotate_image_(HTuple hWindow_, HObject input_image_, out HObject output_image_, 
                                  bool b_save_rotatedImage = false, string ImageName = "") // (20190112) Jeff Revised!
        {
            bool b_status_ = false;
            HTuple hommat2d_affine_;
            output_image_ = null;
            
            if (input_image_ != null && affine_degree_ != null)
            {
                HOperatorSet.HomMat2dIdentity(out hommat2d_affine_);
                HOperatorSet.HomMat2dRotate(hommat2d_affine_, affine_degree_.TupleRad(), 0, 0, out hommat2d_affine_);
                HOperatorSet.AffineTransImage(input_image_, out output_image_, hommat2d_affine_, "constant", "false");
                try // (20191216) MIL Jeff Revised!
                {
                    HOperatorSet.AffineTransPixel(hommat2d_affine_, mark_y_[0].D, mark_x_[0].D, out affine_mark1_y_, out affine_mark1_x_);
                }
                catch
                { }

                if (hWindow_ != null)
                    HOperatorSet.DispObj(output_image_, hWindow_);

                #region 是否儲存扭正後影像
                if (b_save_rotatedImage) // (20190112) Jeff Revised!
                {
                    try
                    {
                        // 儲存扭正後影像
                        HOperatorSet.WriteImage(output_image_, "tiff", 0, CurrentPath_SaveImage + "/" + ImageName);
                    }
                    catch
                    {
                        MessageBox.Show("儲存扭正後影像錯誤!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                #endregion

                b_status_ = true;
            }

            return b_status_;
        }

        /// <summary>
        /// 建立旋轉矩陣
        /// </summary>
        /// <returns></returns>
        public bool create_affine_trans() // (20190516) Jeff Revised!
        {
            bool b_status_ = false;
            try
            {
                int method = 2;
                if (method == 1)
                {
                    #region method 1

                    double side_a_ = motion_shift_dist_y_hat * 1000 / pixel_resolution_ + (this.mark_y_[1].D - this.mark_y_[0].D); // (20180822) Jeff Revised!
                    double side_b_ = (this.mark_x_[0].D - this.mark_x_[1].D);
                    HTuple dev_rad_, affine_rad_;

                    //side_b_ = 0; // 輸出90度 For debug! (20180822) Jeff Revised!
                    HOperatorSet.TupleAtan(side_a_ / side_b_, out dev_rad_);

                    if (dev_rad_ < 0)
                    {
                        //affine_rad_ = -(dev_rad_ + new HTuple(90).TupleRad());          
                        affine_rad_ = -(dev_rad_ + Math.PI / 2); // 順時針旋轉時，affine_rad_ < 0
                    }
                    else
                    {
                        //affine_rad_ = new HTuple(90).TupleRad() - dev_rad_;
                        affine_rad_ = Math.PI / 2 - dev_rad_; // 逆時針旋轉時，affine_rad_ > 0
                    }

                    HOperatorSet.TupleDeg(affine_rad_, out this.affine_degree_);

                    #endregion
                }
                else if (method == 2) // (20190515) Jeff Revised!
                {
                    #region method 2

                    double numerator = motion_shift_dist_y_hat + (mark_y_[1].D - mark_y_[0].D) * pixel_resolution_ / 1000.0;
                    double denominator = motion_shift_dist_x_hat + (mark_x_[1].D - mark_x_[0].D) * pixel_resolution_ / 1000.0;
                    double angle_degree_marks_motion = (((HTuple)(numerator / denominator)).TupleAtan()).TupleDeg().D;
                    if ((angle_degree_marks_motion >= 0 && angle_degree_marks_sample >= 0) || (angle_degree_marks_motion < 0 && angle_degree_marks_sample < 0))
                    {
                        affine_degree_ = angle_degree_marks_motion - angle_degree_marks_sample;
                    }
                    else if (angle_degree_marks_motion < 0 && angle_degree_marks_sample > 0)
                    {
                        affine_degree_ = angle_degree_marks_motion + angle_degree_marks_sample;
                    }

                    // Computed 2 Marks Distance (運動座標) (mm) (20190516) Jeff Revised!
                    if (b_compute_marks_dist)
                    {
                        marks_dist_motion_compute = Math.Sqrt(Math.Pow(numerator, 2) + Math.Pow(denominator, 2));
                    }

                    // Computed Resolution (μm/pixel) (20190516) Jeff Revised!
                    if (b_compute_resolution)
                    {
                        double a = (Math.Pow(mark_y_[1].D - mark_y_[0].D, 2) + Math.Pow(mark_x_[1].D - mark_x_[0].D, 2)) / 1000000.0;
                        double b = (motion_shift_dist_y_hat * (mark_y_[1].D - mark_y_[0].D) + motion_shift_dist_x_hat * (mark_x_[1].D - mark_x_[0].D)) / 500.0;
                        double c = Math.Pow(motion_shift_dist_y_hat, 2) + Math.Pow(motion_shift_dist_x_hat, 2) - Math.Pow(marks_dist_sample, 2);

                        QuadraticEq_1Unknown Resol = new QuadraticEq_1Unknown();
                        double r1, r2;
                        if (Resol.roots(a, b, c, out r1, out r2))
                            computed_resolution_ = r1;
                    }

                    #endregion
                }

                // 做角度補償 (20190112) Jeff Revised!
                this.affine_degree_ += this.affine_degree_shift;

                b_status_ = true;
            }
            catch
            { }

            return b_status_;
        }

        /// <summary>
        /// 判斷Marks間距誤差是否符合設定範圍
        /// </summary>
        /// <returns></returns>
        public bool B_marks_dist_valid() // (20190516) Jeff Revised!
        {
            bool b_status_ = true;

            if (b_compute_marks_dist && b_control_marks_dist)
            {
                if (Math.Abs(marks_dist_motion_compute - marks_dist_sample) > diff_controlMarksDist)
                    b_status_ = false;
            }

            return b_status_;
        }

        private int count_mark_image = 0; // For debug! (20181025) Jeff Revised!
        private string CurrentPath_SaveImage = ""; // (20190112) Jeff Revised!
        /// <summary>
        /// 尋找定位點並產生旋轉矩陣
        /// </summary>
        /// <param name="mark_image_1_">定位點影像1</param>
        /// <param name="mark_image_2_">定位點影像2</param>
        /// <param name="b_save_Image">是否儲存定位影像及檢測結果</param>
        /// <param name="b_draw_Image">是否將檢測結果影像暫存於程式中</param>
        /// <returns></returns>
        public bool find_shpe_model_2(HObject mark_image_1_, HObject mark_image_2_, bool b_save_Image = false, bool b_draw_Image = false) // (20200429) Jeff Revised!
        {
            bool b_status_ = false;
            HObject all_mark_image_;
            HTuple HomMat2D_;
            double[] score = { -1, -1 }; // (20181116) Jeff Revised!

            //b_mark_ROI_exist = read_mark_ROI(); // (20190510) Jeff Revised!
            //HOperatorSet.Rgb1ToGray(mark_image_1_.Clone(), out mark_image_1_); // (20190514) Jeff Revised!
            //HOperatorSet.Rgb1ToGray(mark_image_2_.Clone(), out mark_image_2_); // (20190514) Jeff Revised!
            // Method 1 (20181119) Jeff Revised!
            HOperatorSet.ConcatObj(mark_image_1_, mark_image_2_, out all_mark_image_);
            if (this.b_mark_ROI_exist & this.mark_ROI != null) // (20200429) Jeff Revised!
            {
                //HOperatorSet.ReduceDomain(all_mark_image_.Clone(), mark_ROI, out all_mark_image_);
                HObject ExpTmpOutVar_0;
                HOperatorSet.ReduceDomain(all_mark_image_, this.mark_ROI, out ExpTmpOutVar_0);
                all_mark_image_.Dispose();
                all_mark_image_ = ExpTmpOutVar_0;
            }
            // Method 2 (20181119) Jeff Revised!
            //if (b_mark_ROI_exist & mark_ROI != null)
            //{
            //    HObject mark_image_1_ROI, mark_image_2_ROI;
            //    HOperatorSet.ReduceDomain(mark_image_1_, mark_ROI, out mark_image_1_ROI);
            //    HOperatorSet.ReduceDomain(mark_image_2_, mark_ROI, out mark_image_2_ROI);
            //    HOperatorSet.ConcatObj(mark_image_1_ROI, mark_image_2_ROI, out all_mark_image_);
            //    HOperatorSet.WriteImage(mark_image_1_ROI, "tiff", 0, "mark_image_1_ROI.tiff"); // For debug! (20181119) Jeff Revised!
            //    HOperatorSet.WriteImage(mark_image_2_ROI, "tiff", 0, "mark_image_2_ROI.tiff"); // For debug! (20181119) Jeff Revised!
            //    mark_image_1_ROI.Dispose();
            //    mark_image_2_ROI.Dispose();
            //}
            //else
            //    HOperatorSet.ConcatObj(mark_image_1_, mark_image_2_, out all_mark_image_);

            for (int i = 1; i <= 2; i++)
            {
                HTuple find_row_, find_col_, find_angle_, find_score_;

                if (ModelID_ == null)
                    break;

                HObject select_image_;
                HOperatorSet.SelectObj(all_mark_image_, out select_image_, i);
                //HOperatorSet.WriteImage(select_image_, "tiff", 0, "select_image_.tiff"); // For debug! (20181119) Jeff Revised!
                HOperatorSet.FindShapeModel(select_image_, ModelID_, new HTuple(0).TupleRad(), new HTuple(360).TupleRad(), match_score_, 1, 0, "least_squares", 0, 0.9,
                                            out find_row_, out find_col_, out find_angle_, out find_score_); // (20181119) Jeff Revised!

                if (find_score_.Length > 0)
                {
                    score[i - 1] = find_score_[0].D; // (20181116) Jeff Revised!

                    HOperatorSet.HomMat2dIdentity(out HomMat2D_);
                    HOperatorSet.HomMat2dRotate(HomMat2D_, find_angle_, 0, 0, out HomMat2D_);
                    HOperatorSet.HomMat2dTranslate(HomMat2D_, find_row_, find_col_, out HomMat2D_);
                    HObject trans_contours_;
                    HOperatorSet.AffineTransContourXld(model_contour_, out trans_contours_, HomMat2D_);

                    /* 找到真正 Mark Contour中心 */
                    HObject trans_region_;
                    HTuple trans_row_, trans_col_;
                    HOperatorSet.GenRegionContourXld(trans_contours_, out trans_region_, "filled");
                    HOperatorSet.Union1(trans_region_, out trans_region_);
                    // Method 1: 取輪廓在x,y方向最外側點的中點
                    HOperatorSet.GetRegionPoints(trans_region_, out trans_row_, out trans_col_);
                    mark_x_[i - 1] = (trans_col_.TupleMax() + trans_col_.TupleMin()) / 2.0; // (20180823) Jeff Revised!
                    mark_y_[i - 1] = (trans_row_.TupleMax() + trans_row_.TupleMin()) / 2.0; // (20180823) Jeff Revised!
                    // Method 2: 取 contours' region 之中心 (20180823) Jeff Revised!
                    //HTuple area_, row_, column_;
                    //HOperatorSet.AreaCenter(trans_region_, out area_, out row_, out column_);
                    //mark_x_[i - 1] = column_;
                    //mark_y_[i - 1] = row_;

                    trans_region_.Dispose();
                    trans_contours_.Dispose();
                }
                else
                    break;

                if (i == 2)
                    b_status_ = this.create_affine_trans();

                select_image_.Dispose();
            }
            all_mark_image_.Dispose();

            // 匹配/搜尋失敗處理機制
            if ((b_status_ == false) && (this.angleAffineMethod_New.DefaultMarksPosEnable)) // (20200429) Jeff Revised!
            {
                b_status_ = true;
                this.mark_x_[0] = this.angleAffineMethod_New.defMark1_x;
                this.mark_x_[1] = this.angleAffineMethod_New.defMark2_x;
                this.mark_y_[0] = this.angleAffineMethod_New.defMark1_y;
                this.mark_y_[1] = this.angleAffineMethod_New.defMark2_y;
                this.affine_degree_ = this.angleAffineMethod_New.defAffineAngle;
            }

            if (!b_status_)
            {
                Log.WriteLog("");
                Log.WriteLog("Error: Marks定位 失敗!");
                Log.WriteLog("");
            }
            
            #region 定位測試&驗證

            if (b_save_Image || b_draw_Image) // (20190105) Jeff Revised!
            {
                try
                {
                    if (b_save_Image)
                        count_mark_image++;
                    string path1 = "D:/Position test/", path1_fileName = null;

                    if (b_status_) // 定位成功
                    {
                        if (b_save_Image)
                        {
                            path1_fileName = path1 + count_mark_image + " (Score1 = " + String.Format("{0:0.##}", score[0]) + ", Score2 = " + String.Format("{0:0.##}", score[1]) + ")";
                            //path1_fileName = path1 + count_mark_image + ", Mark1(" + mark_x_[0].TupleInt() + ", " + mark_y_[0].TupleInt() + "), Mark2(" + mark_x_[1].TupleInt() + ", " + mark_y_[1].TupleInt() + ")"; // (20181119) Jeff Revised!
                            path1_fileName += ", Mark1(" + mark_x_[0].TupleInt() + ", " + mark_y_[0].TupleInt() + "), Mark2(" + mark_x_[1].TupleInt() + ", " + mark_y_[1].TupleInt() + ")"; // (20181119) Jeff Revised!
                            CurrentPath_SaveImage = path1_fileName; // (20190112) Jeff Revised!
                            if (!(create_2Layers_file(path1, path1_fileName))) // 資料夾不存在且創建失敗
                                return b_status_;

                            path1_fileName += "/";
                            // 儲存定位原始影像
                            HOperatorSet.WriteImage(mark_image_1_, "tiff", 0, path1_fileName + "mark_image_1.tiff");
                            HOperatorSet.WriteImage(mark_image_2_, "tiff", 0, path1_fileName + "mark_image_2.tiff");
                        }

                        // Display center points of two mark images
                        HObject mark1_center = null;
                        HObject mark2_center = null;
                        HOperatorSet.GenCircle(out mark1_center, mark_y_[0], mark_x_[0], 10);
                        HOperatorSet.GenCircle(out mark2_center, mark_y_[1], mark_x_[1], 10);

                        // 轉成RGB image (20181225) Jeff Revised!
                        HTuple Channels;
                        HOperatorSet.CountChannels(mark_image_1_, out Channels);
                        if (Channels == 1)
                        {
                            HOperatorSet.Compose3(mark_image_1_.Clone(), mark_image_1_.Clone(), mark_image_1_.Clone(), out now_Mark1_ResultImg);
                            HOperatorSet.Compose3(mark_image_2_.Clone(), mark_image_2_.Clone(), mark_image_2_.Clone(), out now_Mark2_ResultImg);
                        }
                        else
                        {
                            HOperatorSet.CopyImage(mark_image_1_.Clone(), out now_Mark1_ResultImg);
                            HOperatorSet.CopyImage(mark_image_2_.Clone(), out now_Mark2_ResultImg);
                        }

                        // 儲存定位影像 + Cross Mark + 中心點 + Mark ROI (20181225) Jeff Revised!
                        HOperatorSet.OverpaintRegion(now_Mark1_ResultImg, gen_CrossMark(now_Mark1_ResultImg), ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "fill"); // (20181225) Jeff Revised!
                        HOperatorSet.OverpaintRegion(now_Mark2_ResultImg, gen_CrossMark(now_Mark2_ResultImg), ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "fill"); // (20181225) Jeff Revised!
                        HOperatorSet.OverpaintRegion(now_Mark1_ResultImg, mark1_center, ((new HTuple(0)).TupleConcat(0)).TupleConcat(255), "fill");
                        HOperatorSet.OverpaintRegion(now_Mark2_ResultImg, mark2_center, ((new HTuple(0)).TupleConcat(0)).TupleConcat(255), "fill");
                        if (b_mark_ROI_exist & mark_ROI != null) // (20181119) Jeff Revised!
                        {
                            HOperatorSet.OverpaintRegion(now_Mark1_ResultImg, mark_ROI, ((new HTuple(0)).TupleConcat(255)).TupleConcat(0), "margin");
                            HOperatorSet.OverpaintRegion(now_Mark2_ResultImg, mark_ROI, ((new HTuple(0)).TupleConcat(255)).TupleConcat(0), "margin");
                        }

                        if (b_save_Image)
                        {
                            HOperatorSet.WriteImage(now_Mark1_ResultImg, "tiff", 0, path1_fileName + "mark_image_1_center.tiff");
                            HOperatorSet.WriteImage(now_Mark2_ResultImg, "tiff", 0, path1_fileName + "mark_image_2_center.tiff");
                        }
                    }
                    else // 定位失敗
                    {
                        if (b_save_Image)
                        {
                            //MessageBox.Show("定位失敗!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            path1_fileName = path1 + count_mark_image + " (Failure)";
                            CurrentPath_SaveImage = path1_fileName; // (20190112) Jeff Revised!
                            if (!(create_2Layers_file(path1, path1_fileName))) // 資料夾不存在且創建失敗
                                return b_status_;

                            path1_fileName += "/";
                            // 儲存定位原始影像
                            HOperatorSet.WriteImage(mark_image_1_, "tiff", 0, path1_fileName + "mark_image_1.tiff");
                            HOperatorSet.WriteImage(mark_image_2_, "tiff", 0, path1_fileName + "mark_image_2.tiff");
                        }

                        // 轉成RGB image (20181225) Jeff Revised!
                        HTuple Channels;
                        HOperatorSet.CountChannels(mark_image_1_, out Channels);
                        if (Channels == 1)
                        {
                            HOperatorSet.Compose3(mark_image_1_.Clone(), mark_image_1_.Clone(), mark_image_1_.Clone(), out now_Mark1_ResultImg);
                            HOperatorSet.Compose3(mark_image_2_.Clone(), mark_image_2_.Clone(), mark_image_2_.Clone(), out now_Mark2_ResultImg);
                        }
                        else
                        {
                            HOperatorSet.CopyImage(mark_image_1_.Clone(), out now_Mark1_ResultImg);
                            HOperatorSet.CopyImage(mark_image_2_.Clone(), out now_Mark2_ResultImg);
                        }

                        // 儲存定位原始影像 + Cross Mark + Mark ROI (20181225) Jeff Revised!
                        HOperatorSet.OverpaintRegion(now_Mark1_ResultImg, gen_CrossMark(now_Mark1_ResultImg), ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "fill"); // (20181225) Jeff Revised!
                        HOperatorSet.OverpaintRegion(now_Mark2_ResultImg, gen_CrossMark(now_Mark2_ResultImg), ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "fill"); // (20181225) Jeff Revised!
                        if (b_mark_ROI_exist & mark_ROI != null)
                        {
                            HOperatorSet.OverpaintRegion(now_Mark1_ResultImg, mark_ROI, ((new HTuple(0)).TupleConcat(255)).TupleConcat(0), "margin");
                            HOperatorSet.OverpaintRegion(now_Mark2_ResultImg, mark_ROI, ((new HTuple(0)).TupleConcat(255)).TupleConcat(0), "margin");                           
                        }

                        if (b_save_Image)
                        {
                            HOperatorSet.WriteImage(now_Mark1_ResultImg, "tiff", 0, path1_fileName + "mark_image_1_ROI.tiff");
                            HOperatorSet.WriteImage(now_Mark2_ResultImg, "tiff", 0, path1_fileName + "mark_image_2_ROI.tiff");
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("儲存定位影像錯誤!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            #endregion

            return b_status_;
        }

        /// <summary>
        /// 影像扭正
        /// </summary>
        /// <param name="input_image_">待扭正影像</param>
        /// <param name="output_image_">已扭正影像</param>
        /// <returns></returns>
        public bool rotate_image_2(HObject input_image_, out HObject output_image_)
        {
            bool b_status_ = false;
            HTuple hommat2d_affine_;
            output_image_ = null;

            if (input_image_ != null && affine_degree_ != null)
            {
                HOperatorSet.HomMat2dIdentity(out hommat2d_affine_);
                HOperatorSet.HomMat2dRotate(hommat2d_affine_, affine_degree_.TupleRad(), 0, 0, out hommat2d_affine_);
                HOperatorSet.AffineTransImage(input_image_, out output_image_, hommat2d_affine_, "constant", "false");
                HOperatorSet.AffineTransPixel(hommat2d_affine_, mark_y_[0].D, mark_x_[0].D, out affine_mark1_y_, out affine_mark1_x_);
                b_status_ = true;
            }

            return b_status_;
        }

        public enum enuSearchType
        {
            TopLeft = 0,
            BottomLeft = 1,
            TopRight = 2,
            BottomRight = 3,
            Center = 4
        }

        public clsEdgeSearchParaam LoadXML()
        {
            string Path = file_directory_ + "\\" + board_type_ + "_EdgeSearchPaaram.xml";
            clsEdgeSearchParaam Res = new clsEdgeSearchParaam();
            try
            {
                XmlSerializer XmlS = new XmlSerializer(Res.GetType());
                Stream S = File.Open(Path, FileMode.Open);
                Res = (clsEdgeSearchParaam)XmlS.Deserialize(S);
                S.Close();

            }
            catch (Exception e)
            {

            }
            return Res;
        }

        public void SaveXML()
        {
            string Path = file_directory_ + "\\" + board_type_ + "_EdgeSearchPaaram.xml";
            clsEdgeSearchParaam Product = clsStaticTool.Clone<clsEdgeSearchParaam>(this.EdgeSearchParam);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(Path));

            try
            {
                XmlSerializer XmlS = new XmlSerializer(Product.GetType());
                Stream S = File.Open(Path, FileMode.Create);
                XmlS.Serialize(S, Product);
                S.Close();
            }
            catch (Exception e)
            {
            }
        }

        public void SaveXML(string FilePath,string Name)
        {
            string Path = FilePath + "\\" + Name + "_EdgeSearchPaaram.xml";
            clsEdgeSearchParaam Product = clsStaticTool.Clone<clsEdgeSearchParaam>(this.EdgeSearchParam);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(Path));

            try
            {
                XmlSerializer XmlS = new XmlSerializer(Product.GetType());
                Stream S = File.Open(Path, FileMode.Create);
                XmlS.Serialize(S, Product);
                S.Close();
            }
            catch (Exception e)
            {
            }
        }

        public bool FindEdgePoins(HObject SearchImage1,HObject SearchImage2,bool b_save_Image)
        {
            bool Res = false;

            if (SearchImage1 != null && SearchImage2 != null)
            {
                this.b_mark_ROI_exist = this.read_SearchEdge_ROI();
                if (!this.b_mark_ROI_exist)
                {
                    SearchImage1.Dispose();
                    SearchImage2.Dispose();
                    return Res;
                }
                HObject ThResRegion1, ThResRegion2;
                HObject MarkRegion1, MarkRegion2;
                if (!FindMark_EdgeFeature_Method(SearchImage1, mark_ROI, out ThResRegion1, out EdgeResRegion1, out MarkRegion1, EdgeSearchParam.Th_Low, EdgeSearchParam.Th_High, EdgeSearchParam.DeNoiseSize, EdgeSearchParam.SearchType1, EdgeSearchParam.TransType, EdgeSearchParam.OpeningSize, EdgeSearchParam.ClosingSize, out mark_x_[0], out mark_y_[0]))
                {
                    SearchImage1.Dispose();
                    SearchImage2.Dispose();
                    return Res;
                }

                if (!FindMark_EdgeFeature_Method(SearchImage2, mark_ROI, out ThResRegion2, out EdgeResRegion2, out MarkRegion2, EdgeSearchParam.Th_Low, EdgeSearchParam.Th_High, EdgeSearchParam.DeNoiseSize, EdgeSearchParam.SearchType2, EdgeSearchParam.TransType, EdgeSearchParam.OpeningSize, EdgeSearchParam.ClosingSize, out mark_x_[1], out mark_y_[1]))
                {
                    SearchImage1.Dispose();
                    SearchImage2.Dispose();
                    return Res;
                }

                this.create_affine_trans();
            }

            #region 定位測試&驗證

            if (b_save_Image)
            {
                try
                {
                    if (b_save_Image)
                        count_mark_image++;
                    string path1 = "D:/Position test/", path1_fileName = null;

                    if (b_save_Image)
                    {
                        path1_fileName = path1 + count_mark_image + " (Score1 = " + String.Format("{0:0.##}", "1.00") + ", Score2 = " + String.Format("{0:0.##}", "1.00") + ")";

                        path1_fileName += ", Mark1(" + mark_x_[0].TupleInt() + ", " + mark_y_[0].TupleInt() + "), Mark2(" + mark_x_[1].TupleInt() + ", " + mark_y_[1].TupleInt() + ")"; // (20181119) Jeff Revised!
                        CurrentPath_SaveImage = path1_fileName; // (20190112) Jeff Revised!
                        if (!(create_2Layers_file(path1, path1_fileName))) // 資料夾不存在且創建失敗
                            return Res;

                        path1_fileName += "/";
                        // 儲存定位原始影像
                        HOperatorSet.WriteImage(SearchImage1, "tiff", 0, path1_fileName + "mark_image_1.tiff");
                        HOperatorSet.WriteImage(SearchImage2, "tiff", 0, path1_fileName + "mark_image_2.tiff");
                    }

                    // Display center points of two mark images
                    HObject mark1_center = null;
                    HObject mark2_center = null;
                    HOperatorSet.GenCircle(out mark1_center, mark_y_[0], mark_x_[0], 10);
                    HOperatorSet.GenCircle(out mark2_center, mark_y_[1], mark_x_[1], 10);

                    // 轉成RGB image 
                    HTuple Channels;
                    HOperatorSet.CountChannels(SearchImage1, out Channels);
                    if (Channels == 1)
                    {
                        HOperatorSet.Compose3(SearchImage1.Clone(), SearchImage1.Clone(), SearchImage1.Clone(), out now_Mark1_ResultImg);
                        HOperatorSet.Compose3(SearchImage2.Clone(), SearchImage2.Clone(), SearchImage2.Clone(), out now_Mark2_ResultImg);
                    }
                    else
                    {
                        HOperatorSet.CopyImage(SearchImage1, out now_Mark1_ResultImg);
                        HOperatorSet.CopyImage(SearchImage2, out now_Mark2_ResultImg);
                    }

                    // 儲存定位影像 + Cross Mark + 中心點 + Mark ROI
                    HOperatorSet.OverpaintRegion(now_Mark1_ResultImg, gen_CrossMark(now_Mark1_ResultImg), ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "fill"); // (20181225) Jeff Revised!
                    HOperatorSet.OverpaintRegion(now_Mark2_ResultImg, gen_CrossMark(now_Mark2_ResultImg), ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "fill"); // (20181225) Jeff Revised!
                    HOperatorSet.OverpaintRegion(now_Mark1_ResultImg, mark1_center, ((new HTuple(0)).TupleConcat(0)).TupleConcat(255), "fill");
                    HOperatorSet.OverpaintRegion(now_Mark2_ResultImg, mark2_center, ((new HTuple(0)).TupleConcat(0)).TupleConcat(255), "fill");

                    if (EdgeResRegion1 != null)
                    {
                        HOperatorSet.OverpaintRegion(now_Mark1_ResultImg, EdgeResRegion1, ((new HTuple(0)).TupleConcat(255)).TupleConcat(0), "margin");
                    }
                    if (EdgeResRegion2 != null)
                    {
                        HOperatorSet.OverpaintRegion(now_Mark2_ResultImg, EdgeResRegion2, ((new HTuple(0)).TupleConcat(255)).TupleConcat(0), "margin");
                    }
                    if (b_save_Image)
                    {
                        HOperatorSet.WriteImage(now_Mark1_ResultImg, "tiff", 0, path1_fileName + "mark_image_1_center.tiff");
                        HOperatorSet.WriteImage(now_Mark2_ResultImg, "tiff", 0, path1_fileName + "mark_image_2_center.tiff");
                    }

                }
                catch
                { }
            }

            if (SearchImage1 != null)
                SearchImage1.Dispose();
            if (SearchImage2 != null)
                SearchImage2.Dispose();

            #endregion
            
            Res = true;
            return Res;
        }

        /// <summary>
        /// 尋找邊緣頂點
        /// </summary>
        /// <param name="ho_srcImage"></param>
        /// <param name="ho_ROI"></param>
        /// <param name="ho_ThvResultRgn"></param>
        /// <param name="ho_EdgeResultRgn"></param>
        /// <param name="ho_MarkRgn"></param>
        /// <param name="hv_Thv_Low"></param>
        /// <param name="hv_Thv_High"></param>
        /// <param name="hv_DeNoiseSize"></param>
        /// <param name="hv_PosOption"></param>
        /// <param name="hv_TransType"></param>
        /// <param name="hv_OpeningSize"></param>
        /// <param name="hv_ClosingSize"></param>
        /// <param name="hv_MarkX"></param>
        /// <param name="hv_MarkY"></param>
        /// <returns></returns>
        public bool FindMark_EdgeFeature_Method(HObject ho_srcImage, HObject ho_ROI, out HObject ho_ThvResultRgn, out HObject ho_EdgeResultRgn, out HObject ho_MarkRgn, HTuple hv_Thv_Low, HTuple hv_Thv_High, HTuple hv_DeNoiseSize, enuSearchType hv_PosOption, HTuple hv_TransType, HTuple hv_OpeningSize, HTuple hv_ClosingSize, out HTuple hv_MarkX, out HTuple hv_MarkY)
        {
            bool Res = false;

            #region 變數宣告
            // Local iconic variables 
            HObject ho_ImageReduced, ho_Region, ho_RegionClosing;
            HObject ho_RegionOpening, ho_Contours, ho_ContoursSplit;
            HObject ho_ObjectSelected = null;
            // Local control variables 
            HTuple hv_XCoordCorners = null, hv_YCoordCorners = null;
            HTuple hv_Number = null, hv_Index = null, hv_RowBegin = new HTuple();
            HTuple hv_ColBegin = new HTuple(), hv_RowEnd = new HTuple();
            HTuple hv_ColEnd = new HTuple(), hv_Nr = new HTuple();
            HTuple hv_Nc = new HTuple(), hv_Dist = new HTuple(), hv_Area = null;
            HTuple hv_Row = null, hv_Column = null, hv_Length = null;
            HTuple hv_I = null, hv_TopLeftX = new HTuple(), hv_TopLeftY = new HTuple();
            HTuple hv_BottomLeftX = new HTuple(), hv_BottomLeftY = new HTuple();
            HTuple hv_TopRightX = new HTuple(), hv_TopRightY = new HTuple();
            HTuple hv_BottomRightX = new HTuple(), hv_BottomRightY = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ThvResultRgn);
            HOperatorSet.GenEmptyObj(out ho_EdgeResultRgn);
            HOperatorSet.GenEmptyObj(out ho_MarkRgn);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_ContoursSplit);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            hv_MarkX = new HTuple();
            hv_MarkY = new HTuple();

            #endregion

            try
            {

                #region 前處理
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_srcImage, ho_ROI, out ho_ImageReduced);
                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, hv_Thv_Low, hv_Thv_High);
                ho_ThvResultRgn.Dispose();
                HOperatorSet.ErosionCircle(ho_Region, out ho_ThvResultRgn, hv_DeNoiseSize);
                ho_RegionClosing.Dispose();
                HOperatorSet.ClosingCircle(ho_ThvResultRgn, out ho_RegionClosing, hv_ClosingSize);
                ho_RegionOpening.Dispose();
                HOperatorSet.OpeningCircle(ho_RegionClosing, out ho_RegionOpening, hv_OpeningSize);
                #endregion

                #region 取得四頂點座標
                ho_EdgeResultRgn.Dispose();
                HOperatorSet.ShapeTrans(ho_RegionOpening, out ho_EdgeResultRgn, hv_TransType);
                ho_Contours.Dispose();
                HOperatorSet.GenContourRegionXld(ho_EdgeResultRgn, out ho_Contours, "border");
                ho_ContoursSplit.Dispose();
                HOperatorSet.SegmentContoursXld(ho_Contours, out ho_ContoursSplit, "lines", 11, 50, 50);
                hv_XCoordCorners = new HTuple();
                hv_YCoordCorners = new HTuple();

                HOperatorSet.CountObj(ho_ContoursSplit, out hv_Number);
                HTuple end_val15 = hv_Number;
                HTuple step_val15 = 1;
                for (hv_Index = 1; hv_Index.Continue(end_val15, step_val15); hv_Index = hv_Index.TupleAdd(step_val15))
                {
                    ho_ObjectSelected.Dispose();
                    HOperatorSet.SelectObj(ho_ContoursSplit, out ho_ObjectSelected, hv_Index);
                    HOperatorSet.FitLineContourXld(ho_ObjectSelected, "tukey", -1, 0, 5, 3, out hv_RowBegin,
                        out hv_ColBegin, out hv_RowEnd, out hv_ColEnd, out hv_Nr, out hv_Nc, out hv_Dist);
                    HOperatorSet.TupleConcat(hv_YCoordCorners, hv_RowBegin, out hv_YCoordCorners);
                    HOperatorSet.TupleConcat(hv_XCoordCorners, hv_ColBegin, out hv_XCoordCorners);
                }

                #endregion

                #region Setting Output
                HOperatorSet.AreaCenter(ho_EdgeResultRgn, out hv_Area, out hv_Row, out hv_Column);
                HOperatorSet.TupleLength(hv_XCoordCorners, out hv_Length);
                HTuple end_val22 = hv_Length - 1;
                HTuple step_val22 = 1;
                for (hv_I = 0; hv_I.Continue(end_val22, step_val22); hv_I = hv_I.TupleAdd(step_val22))
                {
                    if ((int)(new HTuple(((hv_XCoordCorners.TupleSelect(hv_I))).TupleLess(hv_Column))) != 0)
                    {
                        if ((int)(new HTuple(((hv_YCoordCorners.TupleSelect(hv_I))).TupleLess(hv_Row))) != 0)
                        {
                            hv_TopLeftX = hv_XCoordCorners.TupleSelect(hv_I);
                            hv_TopLeftY = hv_YCoordCorners.TupleSelect(hv_I);
                        }
                        else
                        {
                            hv_BottomLeftX = hv_XCoordCorners.TupleSelect(hv_I);
                            hv_BottomLeftY = hv_YCoordCorners.TupleSelect(hv_I);
                        }
                    }
                    else
                    {
                        if ((int)(new HTuple(((hv_YCoordCorners.TupleSelect(hv_I))).TupleLess(hv_Row))) != 0)
                        {
                            hv_TopRightX = hv_XCoordCorners.TupleSelect(hv_I);
                            hv_TopRightY = hv_YCoordCorners.TupleSelect(hv_I);
                        }
                        else
                        {
                            hv_BottomRightX = hv_XCoordCorners.TupleSelect(hv_I);
                            hv_BottomRightY = hv_YCoordCorners.TupleSelect(hv_I);
                        }
                    }

                }

                if (hv_PosOption == enuSearchType.TopLeft)
                {
                    hv_MarkX = hv_TopLeftX.Clone();
                    hv_MarkY = hv_TopLeftY.Clone();
                }
                else if (hv_PosOption == enuSearchType.BottomLeft)
                {
                    hv_MarkX = hv_BottomLeftX.Clone();
                    hv_MarkY = hv_BottomLeftY.Clone();
                }
                else if (hv_PosOption == enuSearchType.TopRight)
                {
                    hv_MarkX = hv_TopRightX.Clone();
                    hv_MarkY = hv_TopRightY.Clone();
                }
                else if (hv_PosOption == enuSearchType.BottomRight)
                {
                    hv_MarkX = hv_BottomRightX.Clone();
                    hv_MarkY = hv_BottomRightY.Clone();
                }
                else if (hv_PosOption == enuSearchType.Center)
                {
                    hv_MarkX = hv_Column.Clone();
                    hv_MarkY = hv_Row.Clone();
                }
                else
                {
                    ho_MarkRgn.Dispose();
                    ho_ImageReduced.Dispose();
                    ho_Region.Dispose();
                    ho_RegionClosing.Dispose();
                    ho_RegionOpening.Dispose();
                    ho_Contours.Dispose();
                    ho_ContoursSplit.Dispose();
                    ho_ObjectSelected.Dispose();
                    Res = true;
                    return Res;
                }
                HOperatorSet.GenCircle(out ho_MarkRgn, hv_MarkY, hv_MarkX, 15);

                #endregion
            }
            catch (HalconException ex)
            {
                ho_MarkRgn.Dispose();
                ho_ImageReduced.Dispose();
                ho_Region.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionOpening.Dispose();
                ho_Contours.Dispose();
                ho_ContoursSplit.Dispose();
                ho_ObjectSelected.Dispose();
                return Res;
            }

            #region Dispose
            ho_MarkRgn.Dispose();
            ho_ImageReduced.Dispose();
            ho_Region.Dispose();
            ho_RegionClosing.Dispose();
            ho_RegionOpening.Dispose();
            ho_Contours.Dispose();
            ho_ContoursSplit.Dispose();
            ho_ObjectSelected.Dispose();
            #endregion


            Res = true;
            return Res;
        }


        public bool ModelEmpty
        {
            get { return ModelID_ == null ? true : false; }
        }
    }

    [Serializable]
    public class AngleAffineMethod_New // (20200429) Jeff Revised!
    {
        public AngleAffineMethod_New() { }

        #region 匹配/搜尋失敗處理機制

        /// <summary>
        /// 強制指定預設位置
        /// </summary>
        public bool DefaultMarksPosEnable { get; set; } = false;

        /* 標記1 & 標記2 影像座標 */
        public double defMark1_x { get; set; } = 100.0;
        public double defMark1_y { get; set; } = 100.0;
        public double defMark2_x { get; set; } = 100.0;
        public double defMark2_y { get; set; } = 100.0;

        /// <summary>
        /// 扭正角度
        /// </summary>
        public double defAffineAngle { get; set; } = 0.0;

        #endregion
    }
}
