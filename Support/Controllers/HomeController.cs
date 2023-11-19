using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Support.Controllers.Unit;
using Support.Models;

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

        public ActionResult ProgVersions()
        {
            return View();
        }

        public ActionResult Videos()
        {
            return View();
        }
        /* public ActionResult Tiket()
         {
             return View();
         }*/

        public ActionResult Download()
        {
            return View();
        }

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


        //http://localhost:52798/Home/MailBox?LockNumber=10000&Pass=ADf5243hh2059dghQpAdq42114
        public ActionResult MailBox(string LockNumber, string Pass)
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

        //http://localhost:52798/Home/Tiket?LockNumber=10000&Pass=ADf5243hh2059dghQpAdq42114

        public ActionResult Tiket(string LockNumber, string Pass)
        {
            SupportModel db = new SupportModel();
            if (LockNumber != null)
            {
                ViewBag.LockNumber = "";
                if (Pass == "ADf5243hh2059dghQpAdq42114")
                {
                    string sql = string.Format(@"select * from Users where (LockNumber = {0})", LockNumber);
                    var list = db.Database.SqlQuery<Users>(sql).First(); // اگر 1 یا 2 بود به تیکت دسترسی دارد
                    if (list.UserType == 1 || list.UserType == 2)
                    {
                        ViewBag.LockNumber = LockNumber;
                    }
                    else
                    {
                        ViewBag.LockNumber = "NotAccess";
                    }
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
                            string sql = string.Format(@"select * from Users where (LockNumber = {0})", lockNumber);
                            var list = db.Database.SqlQuery<Users>(sql).First();
                            if (list.UserType == 1 || list.UserType == 2)
                            {
                                ViewBag.LockNumber = lockNumber;
                            }
                            else
                            {
                                ViewBag.LockNumber = "NotAccess";
                            }
                        }
                    }
                }
            }

            return View();
        }

        //http://localhost:52798/api/Data/CheckVideoFormId/1/4OClgAD-oIzeawIDNx86MvzfUjUlCURKy-4gjG1r3pI=
        //http://localhost:52798/Home/VideoFormId?HashLink=RnRjZ6pO2xlDkNOb641a8saAXdSngF1qycRKhFU0vZEn84JBwWWaP9n6IW4oj5VqwHapxFS5zB;RtEHnqKnaJA==&&Token=4OClgAD-oIzeawIDNx86MvzfUjUlCURKy-4gjG1r3pI=
        public ActionResult VideoFormId(string HashLink , string Token)
        {
            long currentDate = DateTime.Now.Ticks;
            var inputToken = UnitPublic.Decrypt(Token);
            var data = inputToken.Split('-');
            if (data.Length == 3)
            {
                string lockNumber = data[0];
                Int64 tik = Int64.Parse(data[2]);
                long elapsedTicks = currentDate - tik;
                TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
                if (elapsedSpan.TotalMinutes <= 1)
                {
                    ViewBag.Link = UnitPublic.Decrypt(HashLink);
                    ViewBag.LockNumber = lockNumber;
                }
            }
            return View();
        }




    }
}