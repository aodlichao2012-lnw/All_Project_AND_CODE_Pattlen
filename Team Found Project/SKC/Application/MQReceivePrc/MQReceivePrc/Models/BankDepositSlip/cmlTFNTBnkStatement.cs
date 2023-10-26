using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Models.BankDepositSlip
{
    public class cmlTFNTBnkStatement
    {
        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///รหัสสมุดบัญชี
        /// </summary>
        public string FTBbkCode { get; set; }

        /// <summary>
        ///ประเภท Transaction  1:Deposit, 2:Withdrawal, 3:Transfer, 4:Income, 5:Expense, 6:Withdrawal with cheque
        /// </summary>
        public string FTBktType { get; set; }

        /// <summary>
        ///วันที่ทำ Transaction
        /// </summary>
        public Nullable<DateTime> FDBktDate { get; set; }

        /// <summary>
        ///จากบัญชี
        /// </summary>
        public string FTBktAccFrom { get; set; }

        /// <summary>
        ///เข้าบัญชี
        /// </summary>
        public string FTBktAccTo { get; set; }

        /// <summary>
        ///อ้างอิง/หมายเลขเช็ค
        /// </summary>
        public string FTBktRefChq { get; set; }

        /// <summary>
        ///จำนวนเงิน
        /// </summary>
        public Nullable<decimal> FCBktAmt { get; set; }

        /// <summary>
        ///ค่าธรรมเนียม
        /// </summary>
        public Nullable<decimal> FCBktFree { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// </summary>
        public string FTBktRmk { get; set; }

        /// <summary>
        ///สถานะเอกสาร 1: อนุมัติแล้ว
        /// </summary>
        public string FTBktStaPrcDoc { get; set; }

        /// <summary>
        ///ผู้อนุมัติเอกสาร
        /// </summary>
        public string FTBktApvCode { get; set; }

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
    }
}
