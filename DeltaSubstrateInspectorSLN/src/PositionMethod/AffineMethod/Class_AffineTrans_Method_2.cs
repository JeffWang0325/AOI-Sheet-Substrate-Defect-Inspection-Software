using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Xml;
using HalconDotNet;

namespace DeltaSubstrateInspector.src.PositionMethod.AffineMethod
{
    class Class_AffineTrans_Method_2
    {
        /// <summary>
        /// Model、Region儲存路徑
        /// 完整路徑: D:\RisistAOI\System\Position\affine_trans\temp
        /// </summary>
        public static string save_path_=@"temp\";

        /// <summary>
        /// 影像單位轉換成實際距離 (um/pixel)
        /// </summary>
        public static double pixel_resolution_ = 3.45;

        /// <summary>
        /// 兩定位影像機台(y方向)移動距離 (mm)
        /// </summary>
        public static double motion_shift_dist_ = 51.83;

        /// <summary>
        /// 匹配最低分數
        /// </summary>
        public static double match_score_ = 0.7;





        static HTuple affine_degree_ = null; // 需要旋轉之角度 (degree)
        static HTuple[] mark_x_ = new HTuple[2]; // 兩影像定位點之中心位置X
        static HTuple[] mark_y_ = new HTuple[2]; // 兩影像定位點之中心位置Y
        static HObject model_region_, model_contour_;
        static HTuple ModelID_;

        //*****************************************************************************

        /// <summary>
        /// 從.xml檔讀取變數
        /// </summary>
        /// <returns></returns>
        public static bool load()
        {
            bool b_status_ = false;

            try
            {
                XmlDocument xml_doc_ = new XmlDocument();
                xml_doc_.Load("Rotate Image.xml");

                XmlNode xml_node_ = xml_doc_.SelectSingleNode("Mark_Rotating");
                if(xml_node_==null)
                    b_status_ = false;
                else
                {
                    XmlElement xml_ele_ = (XmlElement)xml_node_;
                    save_path_ = xml_ele_.GetAttribute("Save_File");
                    pixel_resolution_ = Convert.ToDouble(xml_ele_.GetAttribute("Pixel_Resolution"));
                    motion_shift_dist_ = Convert.ToDouble(xml_ele_.GetAttribute("Motion_Shift_Distance"));
                    match_score_ = Convert.ToDouble(xml_ele_.GetAttribute("Minimum_Match_Score"));
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
        public static bool save()
        {
            bool b_status_ = false;

            try
            {
                XmlDocument xml_doc_ = new XmlDocument();
                XmlElement xml_ele_ = xml_doc_.CreateElement("Mark_Rotating");
                xml_ele_.SetAttribute("Save_File", save_path_);
                xml_ele_.SetAttribute("Pixel_Resolution", pixel_resolution_.ToString());
                xml_ele_.SetAttribute("Motion_Shift_Distance", motion_shift_dist_.ToString());
                xml_ele_.SetAttribute("Minimum_Match_Score", match_score_.ToString());
                xml_doc_.AppendChild(xml_ele_);
                xml_doc_.Save("Rotate Image.xml");
                b_status_ = true;
            }
            catch(Exception ex)
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
        public static bool create_shape_model(HTuple hWindow_, HObject input_image_)
        {
            bool b_status_ = false;
            HObject gray_image_, reduce_image_;
            HObject trans_contours_;
            HTuple row1_, column1_, phi_, length1_, length2_;
            HTuple area_, row_, column_, HomMat2D_;

            try
            {
                HOperatorSet.Rgb1ToGray(input_image_, out gray_image_);
                HOperatorSet.SetColor(hWindow_, "red");
                HOperatorSet.DrawRectangle2(hWindow_, out row1_, out column1_, out phi_, out length1_, out length2_);
                HOperatorSet.GenRectangle2(out model_region_, row1_, column1_, phi_, length1_, length2_);
                HOperatorSet.ReduceDomain(gray_image_, model_region_, out reduce_image_);

                HOperatorSet.CreateShapeModel(reduce_image_, "auto", new HTuple(0).TupleRad(), new HTuple(360).TupleRad(), "auto", "auto", "use_polarity", "auto", "auto", out ModelID_);

                if (!File.Exists(@"affine_trans\" + save_path_))
                    Directory.CreateDirectory(@"affine_trans\" + save_path_);

                HOperatorSet.WriteShapeModel(ModelID_, @"affine_trans\" + save_path_ + "ModelID");
                HOperatorSet.WriteRegion(model_region_, @"affine_trans\" + save_path_ + "ModelRegion.hobj");

                HOperatorSet.GetShapeModelContours(out model_contour_, ModelID_, 1);
                HOperatorSet.AreaCenter(model_region_, out area_, out row_, out column_);
                HOperatorSet.VectorAngleToRigid(0, 0, 0, row_, column_, 0, out HomMat2D_);
                HOperatorSet.AffineTransContourXld(model_contour_, out trans_contours_, HomMat2D_);

                HOperatorSet.DispColor(input_image_, hWindow_);
                HOperatorSet.SetColor(hWindow_, "red");
                HOperatorSet.SetDraw(hWindow_, "margin");
                HOperatorSet.DispRegion(model_region_, hWindow_);
                HOperatorSet.DispObj(trans_contours_, hWindow_);
                b_status_=true;
            }
            catch(Exception ex)
            {
                b_status_ = false;
            }

            return b_status_;
        }

        /// <summary>
        /// 讀取匹配模塊、模塊區域
        /// </summary>
        /// <returns></returns>
        public static bool read_shape_model()
        {
            bool b_status_ = false;
            HObject trans_contours_;
            HTuple area_, row_, column_, HomMat2D_;

            if (File.Exists(@"affine_trans\" + save_path_ + "ModelID") && File.Exists(@"affine_trans\" + save_path_ + "ModelRegion.hobj"))
            {
                HOperatorSet.ReadShapeModel(@"affine_trans\" + save_path_ + "ModelID",out ModelID_);
                HOperatorSet.ReadRegion(out model_region_, @"affine_trans\" + save_path_ + "ModelRegion.hobj");
                HOperatorSet.GetShapeModelContours(out model_contour_, ModelID_, 1);
                HOperatorSet.AreaCenter(model_region_, out area_, out row_, out column_);
                HOperatorSet.VectorAngleToRigid(0, 0, 0, row_, column_, 0, out HomMat2D_);
                HOperatorSet.AffineTransContourXld(model_contour_, out trans_contours_, HomMat2D_);

                b_status_ = true;
            }
            return b_status_;
        }

        /// <summary>
        /// 尋找定位點並產生旋轉矩陣
        /// </summary>
        /// <param name="hWindow_">預顯示的hWindow</param>
        /// <param name="mark_image_1_">定位點影像1</param>
        /// <param name="mark_image_2_">定位點影像2</param>
        /// <returns></returns>
        public static bool find_shpe_model(HTuple hWindow_1_, HObject mark_image_1_, HTuple hWindow_2_, HObject mark_image_2_)
        {
            bool b_status_ = false;
            HObject all_mark_image_, select_image_, trans_contours_;
            HTuple HomMat2D_;

            HOperatorSet.ConcatObj(mark_image_1_, mark_image_2_, out all_mark_image_);
            for(int i=1;i<=2;i++)
            {
                HTuple find_row_, find_col_, find_angle_, find_score_;

                HOperatorSet.SelectObj(all_mark_image_, out select_image_, i);
                HOperatorSet.FindShapeModel(select_image_, ModelID_, new HTuple(0).TupleRad(), new HTuple(360).TupleRad(), match_score_, 1, 0.5, "least_squares", 0, 0.9, 
                    out find_row_, out find_col_, out find_angle_, out find_score_);

                if(find_score_.Length==1)
                {
                    mark_x_[i - 1] = find_col_.Clone();
                    mark_y_[i - 1] = find_row_.Clone();

                    HOperatorSet.HomMat2dIdentity(out HomMat2D_);
                    HOperatorSet.HomMat2dRotate(HomMat2D_, find_angle_, 0, 0, out HomMat2D_);
                    HOperatorSet.HomMat2dTranslate(HomMat2D_, find_row_, find_col_, out HomMat2D_);
                    HOperatorSet.AffineTransContourXld(model_contour_, out trans_contours_, HomMat2D_);

                    if (i == 1)
                    {
                        HOperatorSet.DispColor(select_image_, hWindow_1_);
                        HOperatorSet.SetColor(hWindow_1_, "red");
                        HOperatorSet.DispObj(trans_contours_, hWindow_1_);
                    }
                    else
                    {
                        HOperatorSet.DispColor(select_image_, hWindow_2_);
                        HOperatorSet.SetColor(hWindow_2_, "red");
                        HOperatorSet.DispObj(trans_contours_, hWindow_2_);
                    }
                }
                else
                    break;

                if (i == 2)
                {
                    create_affine_trans();
                    b_status_ = true;
                }
            }

            return b_status_;
        }

        /// <summary>
        /// 影像扭正
        /// </summary>
        /// <param name="hWindow_">預顯示的hWindow</param>
        /// <param name="input_image_">待扭正影像</param>
        /// <param name="output_image_">已扭正影像</param>
        /// <returns></returns>
        public static bool rotate_image_(HTuple hWindow_, HObject input_image_, out HObject output_image_)
        {
            bool b_status_ = false;
            output_image_ = null;

            if (input_image_ != null && affine_degree_ != null)
            {
                HOperatorSet.RotateImage(input_image_, out output_image_, affine_degree_, "constant");
                HOperatorSet.DispColor(output_image_, hWindow_);
                b_status_ = true;
            }

            return b_status_;
        }

        /// <summary>
        /// 建立旋轉矩陣
        /// </summary>
        /// <returns></returns>
        private static bool create_affine_trans()
        {
            bool b_status_ = false;
            double side_a_ = motion_shift_dist_ * 1000 / pixel_resolution_ + Math.Abs(mark_y_[0].D - mark_y_[1].D);
            double side_b_ = Math.Abs(mark_x_[0].D - mark_x_[1].D);
            HTuple dev_rad_, dev_degree_, affine_rad_;

            HOperatorSet.TupleAtan(side_a_ / side_b_, out dev_rad_);
            HOperatorSet.TupleDeg(dev_rad_, out dev_degree_);

            if(dev_degree_<0)            
                affine_rad_ =  dev_rad_ + 3.14 / 2;            
             else            
                affine_rad_ = -(3.14 / 2- dev_rad_);

            HOperatorSet.TupleDeg(affine_rad_, out affine_degree_);
            return b_status_;
        }
    }
}
