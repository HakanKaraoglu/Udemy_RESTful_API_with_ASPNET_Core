using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ParkyWeb.Models.ViewModel
{
    public class IndexVM
    {
        public IEnumerable<NationalPark> NationalParkList { get; set; }
        public IEnumerable<Trail> TrailList { get; set; }
    }
}
