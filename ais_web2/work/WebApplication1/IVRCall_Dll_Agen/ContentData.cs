using System;
using System.Collections.Generic;

using System.Text;
using System.Collections;
using System.Xml;
using System.IO;

namespace IVRCall_Dll_Agen
{
   public class ContentData{

        public string tranID                 { get; set; }
        public string phoneNo             {get; set; }   
        public string aicID                   {get; set; }     
        public string agenID                {get; set; }
        public string ipTerminal           {get; set; }
        public string callStatus            {get; set; }
        public string onlineStatus        { get; set; }
        public string channelNo           { get; set; }
        public Boolean contdata           { get; set; }
        public string cmd                    { get; set; }
        public string status                 { get; set; }
        public string detail                  { get; set; }     
    
    }
}


 