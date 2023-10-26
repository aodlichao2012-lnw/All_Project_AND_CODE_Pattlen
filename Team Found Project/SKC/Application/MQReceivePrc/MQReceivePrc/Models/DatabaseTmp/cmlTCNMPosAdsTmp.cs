using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.DatabaseTmp
{
    public class cmlTCNMPosAdsTmp
    {
        /// <summary>
        ///รหัสสาขา
        /// <summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///รหัส Shop
        /// <summary>
        public string FTShpCode { get; set; }

        /// <summary>
        ///รหัสเครื่อง Pos / รหัสตู้
        /// <summary>
        public string FTPosCode { get; set; }

        /// <summary>
        ///ลำดับ
        /// <summary>
        public Nullable<int> FNPsdSeq { get; set; }

        /// <summary>
        ///ตำแหน่งโฆษณา  TL,TM,TR,ML,MM,MR,BL,BM,BR,AL
        /// <summary>
        public string FTPsdPosition { get; set; }

        /// <summary>
        ///รหัสโฆษณา
        /// <summary>
        public string FTAdvCode { get; set; }

        /// <summary>
        ///ความกว้าง
        /// <summary>
        public Nullable<int> FNPsdWide { get; set; }

        /// <summary>
        ///ความสูง
        /// <summary>
        public Nullable<int> FNPsdHigh { get; set; }

        /// <summary>
        ///วันที่/เวลาเริ่ม
        /// <summary>
        public Nullable<DateTime> FDPsdStart { get; set; }

        /// <summary>
        ///วันที่/เวลาหมดอายุ
        /// <summary>
        public Nullable<DateTime> FDPsdStop { get; set; }

        /// <summary>
        ///วันที่ Update ล่าสุด
        /// <summary>
        public Nullable<DateTime> FDLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// <summary>
        public string FTLastUpdBy { get; set; }

        /// <summary>
        ///วันที่สร้างรายการ
        /// <summary>
        public Nullable<DateTime> FDCreateOn { get; set; }

        /// <summary>
        ///ผู้สร้างรายการ
        /// <summary>
        public string FTCreateBy { get; set; }
    }
}
