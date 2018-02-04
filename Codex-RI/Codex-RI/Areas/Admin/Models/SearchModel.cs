using Neo4jClientVector.Constants.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Codex_RI.Areas.Admin.Models
{
    public abstract class SearchModel
    {
        public int Count { get; set; }

        [JsonProperty]
        public int Page { get; set; }

        public int MaxPage { get; set; }

        public int PageRows { get; set; }

        public bool Infinite { get; set; }

        public string OutputMode { get; set; }

        public SearchDirection Direction { get; set; }

        protected readonly int MaxPageRows = 20;// int.Parse(ConfigurationManager.AppSettings["MaxPageRowsSearch"]);

        public string Order { get; set; }

        public SearchModel()
        {
            Infinite = false;
            Page = 1;
            MaxPage = 1;
            PageRows = MaxPageRows;
        }
    }
    public abstract class SearchModel<T> : SearchModel where T : class
    {
        public List<T> Results { get; set; }
        public string[] Groups { get; set; }
        public List<SelectListItem> Directions
        {
            get
            {
                var list = new List<SelectListItem>();
                list.Add(new SelectListItem
                {
                    Text = "ASC",
                    Value = SearchDirection.Ascending.ToString(),
                    Selected = Direction == SearchDirection.Ascending
                });
                list.Add(new SelectListItem
                {
                    Text = "DESC",
                    Value = SearchDirection.Descending.ToString(),
                    Selected = Direction == SearchDirection.Descending
                });
                return list;
            }
        }
        public List<SelectListItem> Pages
        {
            get
            {
                var pages = Enumerable
                    .Range(1, MaxPage)
                    .Select(x => new SelectListItem
                    {
                        Text = x.ToString(),
                        Value = x.ToString(),
                        Selected = x == Page
                    })
                    .ToList();
                return pages;
            }
        }
        public List<SelectListItem> Records
        {
            get
            {
                var denominator = 20;
                var records = Enumerable
                    .Range(1, (MaxPageRows / denominator))
                    .Select(x => x * denominator)
                    .Select(x => new SelectListItem
                    {
                        Text = x.ToString(),
                        Value = x.ToString(),
                        Selected = x == PageRows
                    })
                    .ToList();
                records.Insert(0, new SelectListItem { Value = "0" });
                return records;
            }
        }
    }
}