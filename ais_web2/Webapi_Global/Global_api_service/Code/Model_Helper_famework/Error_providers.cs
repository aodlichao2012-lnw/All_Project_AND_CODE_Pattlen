using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model_Helper_famework
{
    public class Error_providers
    {
        private Error_providers()
        {

        }

        private static Error_providers Instance = null;
        public static Error_providers Instances
        {
            get
            {
                if (Instance == null)
                    Instance = new Error_providers();
                return Instance;
            }
        }

        public string CustomsExceptions(Exception ex)
        {

            if (ex.GetType() == typeof(NotImplementedException))
            {
                return "ยังไม่เอา interface นี้มาใช้";
            }
            else if (ex.GetType() == typeof(NullReferenceException))
            {
                return "ยังไม่ได้กำหนดค่า";

            }
            else if (ex.GetType() == typeof(OutOfMemoryException))
            {
                return "หน่วยความจำ Ram เต็ม";
            }
            else if (ex.GetType() == typeof(OverflowException))
            {
                return "หน่วยความจำล้น";

            }
            else if (ex.GetType() == typeof(StackOverflowException))
            {
                return "หน่วยความจำล้น";

            }
            else if (ex.GetType() == typeof(TypeInitializationException))
            {
                return "ออปเจ็ค หรือประเภท ตัวแปรผิดประเภท";

            }
            else if (ex.GetType() == typeof(FormatException))
            {
                return "ไม่สามารถเปลี่ยน format ข้อมูล ได้";
            }
            else if (ex.GetType() == typeof(TimeoutException))
            {
                return "หมดเวลา และ ไม่สามารถทำงานต่อได้ เนื่องจาก ทรัพยากรได้คืนหมดแล้ว";
            }
            else if (ex.GetType() == typeof(ArgumentException))
            {
                return "พารามิเตอร์ ไม่ถูกต้อง";
            }
            else
            {
                return "ไม่สามารถระบุ error ได้ในขณะนี้";
            }

        }


    }
}

