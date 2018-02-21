using Codex_RI.Areas.Admin.Models.IndexPlaces;
using Services.Services.Places;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<ActionResult> Search(SearchIndexPlaceClusterModel model)
        {
            if (ModelState.IsValid)
            {
                var search = await service.SearchAsync(ToQuery(model));
                model.Page = search.Page;
                model.MaxPage = search.MaxPage;
                model.Results = search.Results;
            }
            return await ViewAsync(model);
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var data = await service.FindGraphAsync(id);
            return View(data);
        }

        #region private methods
        static SearchIndexPlaceCluster ToQuery(SearchIndexPlaceClusterModel model)
        {
            return new SearchIndexPlaceCluster
            {
                Name = model.Name,

                Page = model.Page,
                PageRows = model.PageRows,
                Order = model.Order ?? "ByName",
                Direction = model.Direction
            };
        }

        async Task<ViewResult> ViewAsync(SearchIndexPlaceClusterModel model)
        {
            ViewBag.SortOptions = SortOptions();
            return View("Search", model);
        }

        List<SelectListItem> SortOptions()
        {
            var list = new List<SelectListItem>
            {
                new SelectListItem { Text = "By Name", Value = "ByName" },
                new SelectListItem { Text = "By Regestae Count", Value = "ByRegestaeCount" },
            };
            return list;
        }
        #endregion
    }
}