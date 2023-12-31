﻿using ais_web3.Models;
using Jose;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace ais_web3.Controllers
{
    public class FrmStatusController : Controller
    {
        [Obsolete]
        private readonly Module2 module;

        [Obsolete]
        public FrmStatusController()
        {
            module = new Module2();
        }
        string Agenids = string.Empty;
        [HttpGet]
        public string Index()
        {

            return "";
        }
        [HttpGet]
        [Obsolete]
        public string FrmStatus_Load()
        {
            try
            {
                if(HttpContext.Request.Cookies["Agen"] != null)
                if (HttpContext.Request.Cookies["Agen"].Value != null)
                {
                    Agenids = JWT.Decode(HttpContext.Request.Cookies["Agen"].Value).Split(':')[1].Split('}')[0].Replace(@"""", "");
                    Module2.Agent_Id = Agenids;
                }
            }
            catch(Exception ex)
            {
                WriteLog.instance.Log("Error ที่ FrmStatus_Load : " + ex.Message.ToString());
                Module2.Agent_Id = "";
            }
            string json = string.Empty;
          json = Get_Project();
            return json;
        }

        [Obsolete]
        public string Get_Project()
        {
            try
            {
                string SQL = "";
                SQL = "select CNFG_STATUS_CODE.DESCRIPTION  as DESCRIPTION  from CNFG_AGENT_INFO,CNFG_STATUS_CODE  where AGENT_ID = :AGENT_ID AND CNFG_AGENT_INFO.LOGON_EXT= CNFG_STATUS_CODE.STATUS_ID";
                // Conn.Open(SQL, Conn)
                DataTable dt2 = null;
                  module.Comman_Static2(SQL, new string[] { Agenids }, new string[] { ":AGENT_ID" } , ref dt2);
                if (dt2 == null)
                {
                    return "Unknow";
                }
                if (dt2.Rows.Count > 0)
                {
                    return dt2.Rows[0]["DESCRIPTION"].ToString();
                }
                return "Unknow";
            }
            catch (Exception ex)
            {
                WriteLog.instance.Log("Error ที่ Get_Project : " + ex.Message.ToString());
                return "Unknow";
            }
        }
    }
}