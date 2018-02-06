using Services.Services.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Codex_RI.Areas.Admin.Models.Regestae
{
    public class SearchRegestClusterModel : SearchModel<RegestCluster>
    {
        public string Ident { get; set; }

        public string Year { get; set; }

        public string Month { get; set; }

        public string Day { get; set; }

        public string PlaceOfIssue { get; set; }

        public Guid? MentionedPlace { get; set; }
    }
}