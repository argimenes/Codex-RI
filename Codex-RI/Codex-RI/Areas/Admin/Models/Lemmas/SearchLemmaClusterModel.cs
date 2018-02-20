using Services.Services.Persons;
using Services.Services.Regestae;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Codex_RI.Areas.Admin.Models.Regestae
{
    public class SearchLemmaClusterModel : SearchModel<LemmaCluster>
    {
        public string Lemma { get; set; }
    }
}