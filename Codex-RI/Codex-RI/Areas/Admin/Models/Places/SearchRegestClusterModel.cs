using Services.Services.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Codex_RI.Areas.Admin.Models.Places
{
    public class SearchPlaceClusterModel : SearchModel<PlaceCluster>
    {
    public string Name { get; set; }
    }
}