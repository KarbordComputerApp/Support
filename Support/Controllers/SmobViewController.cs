using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Support.Controllers
{
    public class SmobViewController : Controller
    {
        // GET: SmobView
        public ActionResult Fish()
        {
            return View();
        }
    }
}