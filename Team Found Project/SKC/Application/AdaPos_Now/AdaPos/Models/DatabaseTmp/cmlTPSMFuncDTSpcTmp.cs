using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.DatabaseTmp
{
    public class cmlTPSMFuncDTSpcTmp
    {
        /// <summary>
        ///รหัส Function Keyboard
        /// </summary>
        public string FTGhdCode { get; set; }

        /// <summary>
        ///รหัสคีย์บอร์ด : KB999
        /// </summary>
        public string FTSysCode { get; set; }

        /// <summary>
        ///รหัสแอปพลิเคชั่น
        /// </summary>
        public string FTAppCode { get; set; }

        /// <summary>
        ///แสดงใน หน้า กรณีเกิน 1 หน้า
        /// </summary>
        public Nullable<int> FNGdtPage { get; set; }

        /// <summary>
        ///ลำดับ ตามกลุ่ม-Default
        /// </summary>
        public Nullable<Int64> FNGdtDefSeq { get; set; }

        /// <summary>
        ///ลำดับ ตามกลุ่ม-User setup
        /// </summary>
        public Nullable<Int64> FNGdtUsrSeq { get; set; }

        /// <summary>
        ///ขนาดปุ่ม แกน X
        /// </summary>
        public Nullable<int> FNGdtBtnSizeX { get; set; }

        /// <summary>
        ///ขนาดปุ่ม แกน Y
        /// </summary>
        public Nullable<int> FNGdtBtnSizeY { get; set; }

        /// <summary>
        ///ฟังก์ชันที่อ้างอิงถึง
        /// </summary>
        public string FTGdtCallByName { get; set; }

        /// <summary>
        ///สถานะการใช้งาน ว่าง Null:ไม่ใช้งาน 1:ใช้งาน
        /// </summary>
        public string FTGdtStaUse { get; set; }

        /// <summary>
        ///ระดับฟังก์ชั่น
        /// </summary>
        public Nullable<int> FNGdtFuncLevel { get; set; }

        /// <summary>
        ///สถานะใช้งาน 1:เปิดให้แก้ไข 2:ยังไม่รองรับ 3:ไม่เปิดให้แก้ไข
        /// </summary>
        public string FTGdtSysUse { get; set; }
    }
}
