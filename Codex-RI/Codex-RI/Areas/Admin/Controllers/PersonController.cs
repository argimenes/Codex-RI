using Codex_RI.Areas.Admin.Models.Persons;
using Neo4jClientVector.Constants.Enums;
using Neo4jClientVector.Helpers;
using Services.Services.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Codex_RI.Areas.Admin.Controllers
{
    public class PersonController : AdminBaseController
    {
        #region constructor
        protected IPersonService service;
        public PersonController(IPersonService _service)
        {
            service = _service;
        }
        #endregion

        public async Task<ActionResult> Search(SearchPersonClusterModel model)
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
        async Task<ViewResult> ViewAsync(SearchPersonClusterModel model)
        {
            ViewBag.SortOptions = SortOptions();
            return View("Search", model);
        }

        List<SelectListItem> SortOptions()
        {
            var list = new List<SelectListItem>
            {
                new SelectListItem { Text = "By Name", Value = SearchOrder.ByName.ToString() },
            };
            return list;
        }

        static SearchPersonCluster ToQuery(SearchPersonClusterModel model)
        {
            return new SearchPersonCluster
            {
                Name1 = model.Name1,
                Name3 = model.Name3,

                Page = model.Page,
                PageRows = model.PageRows,
                Order = model.Order.ToEnum(SearchOrder.ByName).ToString(),
                Direction = model.Direction
            };
        }
        #endregion
    }
}