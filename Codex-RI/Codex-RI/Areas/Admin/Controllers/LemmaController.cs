using Codex_RI.Areas.Admin.Models.Regestae;
using Neo4jClientVector.Constants.Enums;
using Services.Services.Regestae;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Codex_RI.Areas.Admin.Controllers
{
    public class LemmaController : AdminBaseController
    {
        #region constructor
        protected readonly ILemmaService service;
        public LemmaController(ILemmaService _service)
        {
            service = _service;
        }
        #endregion

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var data = await service.FindGraphAsync(id);
            return View(data);
        }

        public async Task<ActionResult> Search(SearchLemmaClusterModel model)
        {
            if (ModelState.IsValid)
            {
                var search = await service.SearchAsync(ToQuery(model));
                model.Page = search.Page;
                model.MaxPage = search.MaxPage;
                model.Results = search.Results;
            }
            return View(model);
        }

        private ViewResult View(SearchLemmaClusterModel model)
        {
            ViewBag.SortOptions = new List<SelectListItem> {
                new SelectListItem { Text = "By Name", Value = "ByName" },
                new SelectListItem { Text = "By Regest Count", Value = "ByRegestCount" },
            };
            return View("Search", model);
        }

        #region private methods
        static SearchLemmaCluster ToQuery(SearchLemmaClusterModel model)
        {
            return new SearchLemmaCluster
            {
                Lemma = model.Lemma,
                Order = model.Order ?? "ByName",
                Direction = model.Direction,
                Page = model.Page,
                PageRows = model.PageRows
            };
        }
        #endregion
    }
}