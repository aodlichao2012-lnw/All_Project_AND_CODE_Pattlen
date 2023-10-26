using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Webservice.Respond.User
{
    public class cmlResInfoUserGrp
    {
        public string rtUsrCode { get; set; }
        public string rtBchCode { get; set; }
        //public string rtUsrStaShop { get; set; }              //*Arm 63-06-16 ยกเลิกฟิลด์ ปรับตามโครงสร้าง SKC
        public string rtShpCode { get; set; }
        //public Nullable<DateTime> rdUsrStart { get; set; }    //*Arm 63-06-16 ยกเลิกฟิลด์ ปรับตามโครงสร้าง SKC
        //public Nullable<DateTime> rdUsrStop { get; set; }     //*Arm 63-06-16 ยกเลิกฟิลด์ ปรับตามโครงสร้าง SKC

        /// <summary>
        ///รหัสตัวแทน/เจ้าของกำเนินการ
        /// </summary>
        public string rtMerCode { get; set; }                   //*Arm 63-06-16 เพิ่มฟิลด์ ปรับตามโครงสร้าง SKC

        /// <summary>
        ///รหัสคู้ค้า
        /// </summary>
        public string rtAgnCode { get; set; }                   //*Arm 63-06-16 เพิ่มฟิลด์ ปรับตามโครงสร้าง SKC

        /// <summary>
        ///วันที่สร้าง
        /// </summary>
        public Nullable<DateTime> rdCreateOn { get; set; }      //*Arm 63-06-16 เพิ่มฟิลด์ ปรับตามโครงสร้าง SKC

        /// <summary>
        ///ผู้สร้าง
        /// </summary>
        public string rtCreateBy { get; set; }                  //*Arm 63-06-16 เพิ่มฟิลด์ ปรับตามโครงสร้าง SKC

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// </summary>
        public Nullable<DateTime> rdLastUpdOn { get; set; }     //*Arm 63-06-16 เพิ่มฟิลด์ ปรับตามโครงสร้าง SKC

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// </summary>
        public string rtLastUpdBy { get; set; }                 //*Arm 63-06-16 เพิ่มฟิลด์ ปรับตามโครงสร้าง SKC
    }
}
