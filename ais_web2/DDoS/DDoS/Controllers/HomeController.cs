using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DDoS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            for(int i =0; i < 1000000; i++)
            {
                return Redirect("https://localhost:44389/FrmDetail/Index");
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}