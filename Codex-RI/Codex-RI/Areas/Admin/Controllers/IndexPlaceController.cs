﻿using Services.Services.Places;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Codex_RI.Areas.Admin.Controllers
{
    public class IndexPlaceController : AdminBaseController
    {
        #region constructor
        protected readonly IIndexPlaceService service;
        public IndexPlaceController(IIndexPlaceService _service)
        {
            service = _service;
        }
        #endregion

        [HttpGet]
        public ActionResult Index(Guid id)
        {
            var data = service.FindGraphAsync(id);
            return View(data);
        }
    }
}