using Codex_RI.Areas.Admin.Models.Regestae;
using Services.Services.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Codex_RI.Areas.Admin.Controllers
{
    public class RegestController : AdminBaseController
    {
        #region constructor
        protected IRegestService service;
        public RegestController(IRegestService _service)
        {
            service = _service;
        }
        #endregion

        public async Task<ActionResult> Search(SearchRegestClusterModel model)
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
        async Task<ViewResult> ViewAsync(SearchRegestClusterModel model)
        {
            ViewBag.PlacesOfIssue = await PlacesOfIssueAsync();
            ViewBag.Places = await PlacesAsync();
            ViewBag.Months = Months();
            ViewBag.SortOptions = SortOptions();
            return View("Search", model);
        }

        private async Task<List<SelectListItem>> PlacesAsync()
        {
            var list = (await service.PlacesAsync()).OrderBy(x => x.name1).Select(x => new SelectListItem { Text = x.name1, Value = x.Guid.ToString() }).ToList();
            list.Insert(0, new SelectListItem { });
            return list;
        }

        async Task<List<SelectListItem>> PlacesOfIssueAsync()
        {
            var list = (await service.PlacesOfIssueAsync()).OrderBy(x => x).Select(x => new SelectListItem { Text = x, Value = x }).ToList();
            list.Insert(0, new SelectListItem { });
            return list;
        }

        static List<SelectListItem> Months()
        {
            return new string[] { "", "Januar", "Februar", "März", "April", "Mai", "Juni", "July", "August", "September", "Oktober", "November", "Dezember", "------------------", "Herbst", "Spätherbst", "Winter", "Frühjahr", "Frühherbst", "Spätsommer" }
                     .Select(x => new SelectListItem { Text = x, Value = x })
                     .ToList();
        }

        List<SelectListItem> SortOptions()
        {
            var list = new List<SelectListItem>
            {
                new SelectListItem { Text = "By Date", Value = "ByDate" },
                new SelectListItem { Text = "By Regesta Id", Value = "ByRegId" },
                new SelectListItem { Text = "By Regesta Number", Value = "ByRegNum" },
                new SelectListItem { Text = "By Place of Issue", Value = "ByPlaceOfIssue" },
                new SelectListItem { Text = "By Person Count", Value = "ByPersonCount" },
            };
            return list;
        }

        static SearchRegestCluster ToQuery(SearchRegestClusterModel model)
        {
            return new SearchRegestCluster
            {
                PlaceOfIssue = model.PlaceOfIssue,
                MentionedPlace = model.MentionedPlace,

                Year = model.Year,
                Month = model.Month,
                Day = model.Day,

                Page = model.Page,
                PageRows = model.PageRows,
                Order = model.Order ?? "ByDate",
                Direction = model.Direction
            };
        }
        #endregion
    }
}