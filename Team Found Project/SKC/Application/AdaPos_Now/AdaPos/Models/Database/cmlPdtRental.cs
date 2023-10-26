using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Database
{
    public class cmlPdtRental
    {
        public string FTPdtCode { get; set; }//--รหัสสินค้า
        public string FTBarCode { get; set; }//--บาร์โค้ดสินค้า
        public string FTPdtName { get; set; }//--ชื่อสินค้า
        public string FTImgObj { get; set; } //--รูปภาพสินค้า
        public string FCPdtDeposit { get; set; }//--ค่ามัดจำ
        public string FTPdtStaPay { get; set; } //--สถานะการชำระ 1:จ่ายขั้นต่ำตอนเช่า/Pre-Paid 2: จ่ายตอนคืน/PostPaid
        public string FTPdtSetOrSN { get; set; }//-- Serial Number ถ้า FTPdtSetOrSN = 3 เมื่อกดเลือกสินค้าจะต้องแสดง pop-up ให้กรอก Serail number
        public string FTPghDocNo { get; set; }  //--เลขที่เอกสารใบปรับราคา
        public string FTRthCode { get; set; }   //--รหัสราคาค่าเช่า
        public string FTRthCalType { get; set; }//--1:ปัดขึ้น(Df);2ปัดลง;3:เฉลี่ย(ใช้ราคาจาก Rate Seq ก่อนหน้า)
        public string FNRtdSeqNo { get; set; }  //--ลำดับอัตราค่าเช่า
        public string FNRtdMinQty { get; set; } //--เวลาตามอัตรา 
        public string FTRtdTmeType { get; set; }//--หน่วย 1:นาที 2:ชั่วโมง 3:วัน 4:เดือน 5: ปี 
        public string FCRtdPrice { get; set; }  //--ราคา
    }
}