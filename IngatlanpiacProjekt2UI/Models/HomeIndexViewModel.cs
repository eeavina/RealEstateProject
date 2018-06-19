using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace RealEstateProjectUI.Models
{
    public class HomeIndexViewModel
    {
        public HomeIndexViewModel()
        {
            AdModelsList = new List<AdModels>();
        }
        //list property for enabling post action on home index view
        public List<AdModels> AdModelsList { get; set; }
    }
}