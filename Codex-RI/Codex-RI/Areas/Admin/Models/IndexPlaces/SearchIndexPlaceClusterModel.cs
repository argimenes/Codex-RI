using Services.Services.Places;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Codex_RI.Areas.Admin.Models.IndexPlaces
{
    public class SearchIndexPlaceClusterModel : SearchModel<IndexPlaceCluster>
    {
        public string Name { get; set; }
    }
}