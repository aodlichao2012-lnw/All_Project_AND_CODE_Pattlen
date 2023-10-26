using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using DataTable = System.Data.DataTable;

namespace Model_Helper_famework
{
    public class Valid_provider
    {

        private Valid_provider()
        {

        }

        private static Valid_provider Instance = null;
        public static Valid_provider Instances
        {
            get
            {
                if (Instance == null)
                    Instance = new Valid_provider();
                return Instance;
            }
        }

        public string isvaild(object model, object values, ref string message)
        {
            try
            {
                List<DataRow> objectList1 = null;
                List<object> objectList2 = null;
                if (values.GetType() == typeof(DataTable))
                {
                    DataTable objectArray = (DataTable)values;
                    objectList1 = objectArray.AsEnumerable().ToList();
                    foreach (var item in objectList1)
                    {
                        if (item == null || item.ToString() == "")
                        {
                            message = "มีค่า type" + item.GetType().DeclaringType + "ที่ว่างเปล่า";
                            return message;
                        }
                    }
                }
                else
                {
                    object[] objectArray = (object[])values;
                    objectList2 = new List<object>(objectArray);
                    foreach (var item in objectList2)
                    {
                        if (item == null || item.ToString() == "")
                        {
                            message = "มีค่า type" + item.GetType().DeclaringType + "ที่ว่างเปล่า";
                            return message;
                        }
                    }
                }
             

                foreach (var values_item in values.GetType().GetProperties())
                {

                    if (!model.GetType().GetProperties().Any( z => z.PropertyType == values_item.PropertyType))
                    {
                        message = "type " + values_item.GetType().DeclaringType + " ไม่ตรงกัน ไม่สามารถทำงานต่อได้";
                        return message;
                    }
                    else
                    {
                        message = "ok";
                    }
                }
           

            

                return message;
            }
            catch(Exception ex)
            {
                return Error_providers.Instances.CustomsExceptions(ex);
            }
           
        }
    }
}
