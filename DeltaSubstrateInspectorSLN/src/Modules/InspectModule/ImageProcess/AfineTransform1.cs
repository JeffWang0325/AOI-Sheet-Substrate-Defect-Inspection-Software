using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extension;

namespace DeltaSubstrateInspector.src.Modules.InspectModule.ImageProcess
{
    public class AfineTransform1
    {
        /// <summary>
        /// 篩選十字定位
        /// </summary>
        public static int[] thred_ = new int[2] { 0, 160 };

        /// <summary>
        /// 找出十字定位區域
        /// </summary>
        public static int[] select_area_ = new int[2] { 18000, 25000 };

        /// <summary>
        /// 定位點數量
        /// </summary>
        public static int count_locate_ = 2;

        /// <summary>
        /// 旋轉矩陣
        /// </summary>
        static HTuple hommat2d_rotate_ = null;

        /// <summary>
        /// 建立十字定位矩陣
        /// </summary>
        /// <param name="input_image_">十字定位原始影像</param>
        /// <param name="dev_angle_">原始影像偏差角度</param>
        /// <param name="affine_image_">扭正影像</param>
        /// <returns></returns>
        public bool create_afffine_trans(HObject input_image_, out double dev_angle_, out HObject affine_image_)
        {
            bool status_ = false;
            dev_angle_ = 0;
            affine_image_ = null;

            HTuple width_, height_, count_sele_;
            HObject gray_image_, thred_region_, con_region_;
            HObject sele_region_;

            HOperatorSet.GetImageSize(input_image_, out width_, out height_);
            HOperatorSet.Rgb1ToGray(input_image_, out gray_image_);
            HOperatorSet.Threshold(gray_image_, out thred_region_, thred_[0], thred_[1]);
            HOperatorSet.Connection(thred_region_, out con_region_);
            HOperatorSet.SelectShape(con_region_, out sele_region_, "area", "and", select_area_[0], select_area_[1]);
            HOperatorSet.CountObj(sele_region_, out count_sele_);

            if (count_sele_ == count_locate_)
            {
                HTuple area_, row_, col_;
                HTuple row_center_, col_center_, length_, phi_;
                HOperatorSet.AreaCenter(sele_region_, out area_, out row_, out col_);
                HOperatorSet.LinePosition(row_[0], col_[0], row_[1], col_[1], out row_center_, out col_center_, out length_, out phi_);
                dev_angle_ = phi_.TupleDeg();

                HTuple rotate_angle_ = 0;
                if (dev_angle_ < 0)
                    rotate_angle_ = -(phi_ + 3.14 / 2);
                else
                    rotate_angle_ = 3.14 / 2 - phi_;

                HTuple hommat2d_;
                HOperatorSet.HomMat2dIdentity(out hommat2d_);
                HOperatorSet.HomMat2dRotate(hommat2d_, rotate_angle_, width_ / 2, height_ / 2, out hommat2d_rotate_);
                HOperatorSet.AffineTransImage(input_image_, out affine_image_, hommat2d_rotate_, "constant", "false");

                status_ = true;
            }

            return status_;
        }

        /// <summary>
        /// 影像扭正
        /// </summary>
        /// <param name="input_image_">待扭正影像</param>
        /// <param name="affine_image_">已扭正影像</param>
        /// <returns></returns>
        public bool affine_trans(HObject input_image_, out HObject affine_image_)
        {
            bool status_ = false;
            affine_image_ = null;

            if (hommat2d_rotate_ != null)
            {
                HOperatorSet.AffineTransImage(input_image_, out affine_image_, hommat2d_rotate_, "constant", "false");
                status_ = true;
            }
            else
                status_ = false;

            return status_;
        }



    }
}
