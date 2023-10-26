using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Models.Other
{
    public class cmlRedeemPointDetail
    {
        //public string tRefCode{ get; set; }
        //public string tBarcode { get; set; }
        //public string tPdtCode { get; set; }
        //public string tPdtName { get; set; }
        //public string tUnitName { get; set; }
        //public decimal cQty { get; set; }
        //public decimal cUsePoint { get; set; }
        //public decimal cUseMny { get; set; }
        //public decimal cMintoBill { get; set; }
        //public string tCalType { get; set; }
        //public decimal tSetPrice { get; set; }
        //public int nSeqNo { get; set; }
        //public int nLimitPerBill { get; set; }

        public string tRefCode { get; set; }
        public int nLimitPerBill { get; set; }
        public decimal cUsePoint { get; set; }
        public decimal cUseMny { get; set; }
        public string tCalType { get; set; }
        public string tBarcode { get; set; }
        public string tPdtName { get; set; }
        public string tUnitName { get; set; }
        public int nSeqNo { get; set; }
        public decimal cQtyDT { get; set; }
        public decimal cSetPrice { get; set; }
        public decimal cQtyAval { get; set; }
        public decimal cRdQtyUsed { get; set; }
        public decimal cMinTotBill { get; set; }

        /// <summary>
        /// จำนวนครั้งที่ใช้ไปแล้ว ของ Redeem แต้มแลกส่วนลด
        /// </summary>
        public int nCountUsed { get; set; }
    }

    /// <summary>
    /// *Arm 63-03-11
    /// TPSTSalDTDis
    /// </summary>
    public class cmlRdSalDTDis
    {
        /// <summary>
        ///สาขาสร้าง
        /// </summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///เลขที่เอกสาร
        /// </summary>
        public string FTXshDocNo { get; set; }

        /// <summary>
        ///ลำดับ
        /// </summary>
        public Nullable<int> FNXsdSeqNo { get; set; }

        /// <summary>
        ///วัน/เวลาทำรายการ [dd/mm/yyyy H:mm:ss]
        /// </summary>
        public Nullable<DateTime> FDXddDateIns { get; set; }

        /// <summary>
        ///สถานะส่วนลด 1: ลดรายการ ,2: ลดท้ายบิล
        /// </summary>
        public Nullable<int> FNXddStaDis { get; set; }

        /// <summary>
        ///ข้อความมูลค่าลดชาร์จ เช่น 5 หรือ 5%
        /// </summary>
        public string FTXddDisChgTxt { get; set; }

        /// <summary>
        ///ประเภทลดชาร์จ 1:ลดบาท 2: ลด % 3: ชาร์จบาท 4: ชาร์จ %
        /// </summary>
        public string FTXddDisChgType { get; set; }

        /// <summary>
        ///มูลค่าสุทธิก่อนลดชาร์จ
        /// </summary>
        public Nullable<decimal> FCXddNet { get; set; }

        /// <summary>
        ///ยอดลด/ชาร์จ
        /// </summary>
        public Nullable<decimal> FCXddValue { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string FTXddRefCode { get; set; }

        //************** for RD ***************
        /// <summary>
        ///ประเภทเอกสาร 1: Redeem สินค้า แต้ม+เงิน 2: Redeem ส่วนลด
        /// </summary>
        public string FTRdhDocType { get; set; }

        /// <summary>
        ///จำนวนสินค้าที่ Redeem
        /// </summary>
        public Nullable<decimal> FCXrdPdtQty { get; set; }

        /// <summary>
        ///จำนวนแต้มที่ใช้แลก
        /// </summary>
        public Nullable<Int64> FNXrdPntUse { get; set; }

    }

    /// <summary>
    /// TPSTSalHDDis
    /// </summary>
    public class cmlRdSalHDDis
    {
        /// <summary>
        ///สาขาสร้าง
        /// </summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///เลขที่เอกสาร
        /// </summary>
        public string FTXshDocNo { get; set; }

        /// <summary>
        ///วัน/เวลาทำรายการ [dd/mm/yyyy H:mm:ss]
        /// </summary>
        public Nullable<DateTime> FDXhdDateIns { get; set; }

        /// <summary>
        ///ข้อความมูลค่าลดชาร์จ เช่น 5 หรือ 5%
        /// </summary>
        public string FTXhdDisChgTxt { get; set; }

        /// <summary>
        ///ประเภทลดชาร์จ 1:ลดบาท 2: ลด % 3: ชาร์จบาท 4: ชาร์จ %
        /// </summary>
        public string FTXhdDisChgType { get; set; }

        /// <summary>
        ///ยอดรวมหลังลด (FCXshTotalAfDisChgV+FCXshTotalAfDisChgNV)
        /// </summary>
        public Nullable<decimal> FCXhdTotalAfDisChg { get; set; }

        /// <summary>
        ///ยอดลด/ชาร์จ
        /// </summary>
        public Nullable<decimal> FCXhdDisChg { get; set; }

        /// <summary>
        ///มูลค่าลด/ชาร์จ
        /// </summary>
        public Nullable<decimal> FCXhdAmt { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string FTXhdRefCode { get; set; }
    }


    /// <summary>
    /// TPSTSalRD
    /// </summary>
    public class cmlRdSalRD
    {
        /// <summary>
        ///สาขาสร้าง
        /// </summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///เลขที่เอกสาร
        /// </summary>
        public string FTXshDocNo { get; set; }

        /// <summary>
        ///ลำดับการ Redeem
        /// </summary>
        public Nullable<int> FNXrdSeqNo { get; set; }

        /// <summary>
        ///ประเภทเอกสาร 1: Redeem สินค้า แต้ม+เงิน 2: Redeem ส่วนลด
        /// </summary>
        public string FTRdhDocType { get; set; }

        /// <summary>
        ///RD Type 1 อ้างอิงลำดับใน DT ,RD Type 2 อ้างอิงลำดับใน RC
        /// </summary>
        public Nullable<int> FNXrdRefSeq { get; set; }

        /// <summary>
        ///เลขที่อ้างอิง
        /// </summary>
        public string FTXrdRefCode { get; set; }

        /// <summary>
        ///จำนวนสินค้าที่ Redeem
        /// </summary>
        public Nullable<decimal> FCXrdPdtQty { get; set; }

        /// <summary>
        ///จำนวนแต้มที่ใช้แลก
        /// </summary>
        public Nullable<Int64> FNXrdPntUse { get; set; }
    }

    /// <summary>
    /// Main
    /// </summary>
    public class cmlTransaction
    {
        public string ptFunction { get; set; }
        public string ptSource { get; set; }
        public string ptDest { get; set; }
        public string ptData { get; set; }
    }

    public class cmlTxnRedeem
    {
        public List<cmlTCNTMemTxnRedeem> aoTCNTMemTxnRedeem { get; set; }
    }

    public class cmlTxnSale
    {
        public List<cmlTCNTMemTxnSale> aoTCNTMemTxnSale { get; set; }
    }

    /// <summary>
    /// Detail Tanssaction TCNTMemTxnSale
    /// </summary>
    public class cmlTCNTMemTxnSale
    {
        /// <summary>
        ///รหัสกลุ่มบริษัท
        /// </summary>
        public string FTCgpCode { get; set; }

        /// <summary>
        ///รหัสสมาชิก / เลขที่บัตรประจำตัวประชาชน/Passport
        /// </summary>
        public string FTMemCode { get; set; }

        /// <summary>
        ///เอกสารอ้างอิง
        /// </summary>
        public string FTTxnRefDoc { get; set; }

        /// <summary>
        ///อ้างอิงเอกสาร เช่น กรณีคืนอ้างอิงบิลขาย , บิลขายเก็บค่าเป็นว่าง
        /// </summary>
        public string FTTxnRefInt { get; set; }

        /// <summary>
        ///รหัสผู้จำหน่าย
        /// </summary>
        public string FTTxnRefSpl { get; set; }

        /// <summary>
        ///วันที่เอกสาร
        /// </summary>
        public Nullable<DateTime> FDTxnRefDate { get; set; }

        /// <summary>
        ///ยอดซื้อสุทธิ
        /// </summary>
        public Nullable<decimal> FCTxnRefGrand { get; set; }

        /// <summary>
        ///เงื่อนไข อัตราส่วนมูลค่าซื้อ
        /// </summary>
        public Nullable<decimal> FCTxnPntOptBuyAmt { get; set; }

        /// <summary>
        ///เงื่อนไข อัตราแต้มที่จะได้
        /// </summary>
        public Nullable<decimal> FCTxnPntOptGetQty { get; set; }

        /// <summary>
        ///จำนวนแต้มสะสมขณะสร้างเอกสาร
        /// </summary>
        public Nullable<decimal> FCTxnPntB4Bill { get; set; }

        /// <summary>
        ///วันที่เริ่มใช้งานแต้มได้
        /// </summary>
        public Nullable<DateTime> FDTxnPntStart { get; set; }

        /// <summary>
        ///วันที่เริ่มใช้งานแต้มได้
        /// </summary>
        public Nullable<DateTime> FDTxnPntExpired { get; set; }

        /// <summary>
        ///จำนวนแต้มที่ได้
        /// </summary>
        public Nullable<decimal> FCTxnPntBillQty { get; set; }

        /// <summary>
        ///แต้มที่ใช้ไปแล้ว
        /// </summary>
        public Nullable<decimal> FCTxnPntUsed { get; set; }

        /// <summary>
        ///แต้มที่หมดอายุ
        /// </summary>
        public Nullable<decimal> FCTxnPntExpired { get; set; }

        /// <summary>
        ///สถานะ 1:คำนวนได้ 2:ไม่นำไปคำนวนแล้ว priority สูงกว่าช่วง start-expired
        /// </summary>
        public string FTTxnPntStaClosed { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// </summary>
        public Nullable<DateTime> FDLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// </summary>
        public string FTLastUpdBy { get; set; }

        /// <summary>
        ///วันที่สร้างรายการ
        /// </summary>
        public Nullable<DateTime> FDCreateOn { get; set; }

        /// <summary>
        ///ผู้สร้างรายการ
        /// </summary>
        public string FTCreateBy { get; set; }

        /// <summary>
        ///ประเภทบิล   1 = บิลขาย , 2:บิล Void   Def: 1
        /// </summary>
        public string FTTxnPntDocType { get; set; }
    }


    /// <summary>
    /// TCNTMemTxnRedeem
    /// </summary>
    public class cmlTCNTMemTxnRedeem
    {
        /// <summary>
        ///รหัสกลุ่มบริษัท
        /// <summary>
        public string FTCgpCode { get; set; }

        /// <summary>
        ///รหัสสมาชิก / เลขที่บัตรประจำตัวประชาชน/Passport
        /// <summary>
        public string FTMemCode { get; set; }

        /// <summary>
        ///เอกสารอ้างอิง
        /// <summary>
        public string FTRedRefDoc { get; set; }

        /// <summary>
        ///รหัสผู้จำหน่าย
        /// <summary>
        public string FTRedRefSpl { get; set; }

        /// <summary>
        ///อ้างอิงเอกสาร เช่น กรณีคืนอ้างอิงบิลขาย , บิลขายเก็บค่าเป็นว่าง
        /// </summary>
        public string FTRedRefInt { get; set; }     //*Arm 63-03-21

        /// <summary>
        ///วันที่เอกสาร
        /// <summary>
        public Nullable<DateTime> FDRedRefDate { get; set; }

        /// <summary>
        ///จำนวนแต้มสะสม
        /// <summary>
        public Nullable<decimal> FCRedPntB4Bill { get; set; }

        /// <summary>
        ///จำนวนแต้มที่ใช้
        /// <summary>
        public Nullable<decimal> FCRedPntBillQty { get; set; }

        /// <summary>
        ///สถานะ 1:คำนวนได้ 2:ไม่นำไปคำนวนแล้ว priority สูงกว่าช่วง start-expired
        /// <summary>
        public string FTRedPntStaClosed { get; set; }

        /// <summary>
        ///วันที่เริ่มใช้งานแต้มได้
        /// <summary>
        public Nullable<DateTime> FDRedPntStart { get; set; }

        /// <summary>
        /// วันที่แต้มหมดอายุ
        /// </summary>
        public Nullable<DateTime> FDRedPntExpired { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
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

        /// <summary>
        ///ประเภทบิล   1 = บิลขาย , 2:บิล Void   Def: 1
        /// </summary>
        public string FTRedPntDocType { get; set; } //*Arm 63-03-31
    }


    public class cmlSalReturn
    {
        public int nRCount { get; set; }
        public decimal cGetGrandRtn { get; set; }
        public decimal cGetPoint { get; set; }
    }


}
