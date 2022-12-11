using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Support.Controllers.Unit;

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

        //http://localhost:52798/Home/Tiket?LockNumber=10000&Pass=ADf5243hh2059dghQpAdq42114

        public ActionResult Tiket(string LockNumber, string Pass)
        {
            if (LockNumber != null)
            {
                ViewBag.LockNumber = "";
                if (Pass == "ADf5243hh2059dghQpAdq42114")
                {
                    ViewBag.LockNumber = LockNumber;
                }
                else
                {
                    long currentDate = DateTime.Now.Ticks;
                    var inputToken = UnitPublic.Decrypt(LockNumber);
                    var data = inputToken.Split('-');
                    if (data.Length == 3)
                    {
                        string lockNumber = data[0];
                        Int64 tik = Int64.Parse(data[2]);
                        long elapsedTicks = currentDate - tik;
                        TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);

                        if (elapsedSpan.TotalMinutes <= 1)
                        {
                            ViewBag.LockNumber = lockNumber;
                        }
                    }
                }
            }

            return View();
        }



    }
}