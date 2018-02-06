using Codex_RI.Areas.Admin.Models.Places;
using Services.Services.Persons;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Codex_RI.Areas.Admin.Controllers
{
    public class PlaceController : AdminBaseController
    {
        #region constructor
        protected IPlaceService service;
        public PlaceController(IPlaceService _service)
        {
            service = _service;
        }
        #endregion

        public async Task<ActionResult> Search(SearchPlaceClusterModel model)
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
        async Task<ViewResult> ViewAsync(SearchPlaceClusterModel model)
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

        static SearchPlaceCluster ToQuery(SearchPlaceClusterModel model)
        {
            return new SearchPlaceCluster
            {
                Name = model.Name,

                Page = model.Page,
                PageRows = model.PageRows,
                Order = model.Order ?? "ByName",
                Direction = model.Direction
            };
        }
        #endregion
    }
}