using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Support.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult FinancialDocuments()
        {
            return View();
        }

        public ActionResult CustomerFiles()
        {
            return View();
        }

        public ActionResult UploadFiles()
        {
            return View();
        }

        public ActionResult FAQ()
        {
            return View();
        }
       /* public ActionResult Tiket()
        {
            return View();
        }*/

        public ActionResult ChangePassword()
        {
            return View();
        }

        public ActionResult RecoveryPassword()
        {
            return View();
        }


        public ActionResult User()
        {
            return View();
        }

        public ActionResult CustAccount()
        {
            return View();
        }

        public ActionResult MailBox()
        {
            return View();
        }

        //http://localhost:52798/Home/Tiket?LockNumber=10000&Pass=ADf5243hh2059dghQQQ

        public ActionResult Tiket(string LockNumber, string Pass)
        {
            ViewBag.LockNumber = "";
            if (Pass == "ADf5243hh2059dghQQQ")
            {
                ViewBag.LockNumber = LockNumber;
            }
            
            return View();
        }

        

    }
}