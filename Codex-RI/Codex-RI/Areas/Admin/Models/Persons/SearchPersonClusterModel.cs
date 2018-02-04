using Services.Services.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Codex_RI.Areas.Admin.Models.Persons
{
    public class SearchPersonClusterModel : SearchModel<PersonCluster>
    {
        public string Name1 { get; set; }

        public string Name3 { get; set; }
    }
}