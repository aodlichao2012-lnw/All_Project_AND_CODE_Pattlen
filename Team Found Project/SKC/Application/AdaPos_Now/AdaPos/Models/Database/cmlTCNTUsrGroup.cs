using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Database
{
    public class cmlTCNTUsrGroup
    {
        public string FTUsrCode { get; set; }
        public string FTBchCode { get; set; }
        //public string FTUsrStaShop { get; set; }      //*Arm 63-06-16 ยกเลิกฟิลด์ ปรับตามโครงสร้าง SKC
        public string FTShpCode { get; set; }

        //public Nullable<DateTime> FDUsrStart { get; set; }    //*Arm 63-06-16 ยกเลิกฟิลด์ ปรับตามโครงสร้าง SKC
        //public Nullable<DateTime> FDUsrStop { get; set; }     //*Arm 63-06-16 ยกเลิกฟิลด์ ปรับตามโครงสร้าง SKC

        /// <summary>
        ///รหัสตัวแทน/เจ้าของกำเนินการ
        /// </summary>
        public string FTMerCode { get; set; }       //*Arm 63-06-16 เพิ่มฟิลด์ ปรับตามโครงสร้าง SKC

        /// <summary>
        ///รหัสคู้ค้า
        /// </summary>
        public string FTAgnCode { get; set; }       //*Arm 63-06-16 เพิ่มฟิลด์ ปรับตามโครงสร้าง SKC

        /// <summary>
        ///วันที่สร้าง
        /// </summary>
        public Nullable<DateTime> FDCreateOn { get; set; }      //*Arm 63-06-16 เพิ่มฟิลด์ ปรับตามโครงสร้าง SKC

        /// <summary>
        ///ผู้สร้าง
        /// </summary>
        public string FTCreateBy { get; set; }      //*Arm 63-06-16 เพิ่มฟิลด์ ปรับตามโครงสร้าง SKC

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// </summary>
        public Nullable<DateTime> FDLastUpdOn { get; set; }     //*Arm 63-06-16 เพิ่มฟิลด์ ปรับตามโครงสร้าง SKC

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// </summary>
        public string FTLastUpdBy { get; set; }     //*Arm 63-06-16 เพิ่มฟิลด์ ปรับตามโครงสร้าง SKC
    }
}
