using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Codex_RI.Areas.Admin.Controllers
{
    public class PersonController : AdminBaseController
    {
        [HttpGet]
        public ActionResult Index(Guid guid)
        {
            return View();
        }
    }
}