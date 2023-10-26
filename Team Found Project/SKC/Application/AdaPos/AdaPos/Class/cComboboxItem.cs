using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Class
{
    public class cComboboxItem
    {
        private string tC_DisplayValue;
        private string tC_HiddenValue;

        //Constructor
        public cComboboxItem(string ptDisplay, string ptHidden)
        {
            tC_DisplayValue = ptDisplay;
            tC_HiddenValue = ptHidden;
        }

        //Accessor
        public string HiddenValue
        {
            get
            {
                return tC_HiddenValue;
            }
        }

        //Override ToString method
        public override string ToString()
        {
            return tC_DisplayValue;
        }
    }
}
