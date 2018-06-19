using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealEstateProjectUI.Models;

namespace RealEstateProjectUI.Controllers
{
    public class HomeController : Controller
    {
        RealEstateDBEntities db = new RealEstateDBEntities();

        public ActionResult Index()
        {
            //instantiating the HomeIndexViewModel class
            HomeIndexViewModel model = new HomeIndexViewModel();

            //mapping our model
            foreach (var ad in db.Ad)
            {
                model.AdModelsList.Add(new AdModels()
                {
                    Id = ad.Id,
                    Title = ad.Title,
                    District = ad.District,
                    Street = ad.Street,
                    Description = ad.Description,
                    Price = ad.Price,
                    Size = ad.Size,
                    PictureId = ad.Picture.Id,
                    SourceString = ConvertImageDataToSourceString(ad.Picture.Pic)
                });
            }
            return View(model);
        }

        //this action is invoked by a button click on the Home/Index view;
        //it searches for the string parameter in the database, 
        //then creates a list out of the results
        //and converts the items into a list of a different model that includes images as strings
        public ActionResult SearchAds(string searchText, int? searchNumberMin, int? searchNumberMax)
        {
            try
            {
                searchText = searchText.ToLower();

                //fill the result variable with the entire Ad table from the database
                var result = db.Ad.AsQueryable();

                //filter with the below conditions
                if (searchNumberMin!= null)
                {
                    result = result.Where(a=>a.Price >= searchNumberMin);
                }

                if (searchNumberMax!=null)
                {
                    result = result.Where(a=>a.Price <=searchNumberMax);
                }

                if (searchText!=null)
                {
                    result = result.Where(a=>a.Street.ToLower().Contains(searchText));
                }

                //if no search conditions were entered, just return to view, since it already contains the entire database once
                if (searchNumberMin == null && searchNumberMax == null && searchText == "")
                {
                    return View();
                }

                //instantiating the HomeIndexiewModel
                HomeIndexViewModel model = new HomeIndexViewModel();

                //map our model with the search results
                foreach (var ad in result)
                {
                    model.AdModelsList.Add(new AdModels()
                    {
                        Id = ad.Id,
                        Title = ad.Title,
                        District = ad.District,
                        Street = ad.Street,
                        Description = ad.Description,
                        Price = ad.Price,
                        Size = ad.Size,
                        PictureId = ad.Picture.Id,
                        //this is why we need a HomeIndexViewModel instead of a List<Ad>
                        SourceString = ConvertImageDataToSourceString(ad.Picture.Pic)
                    });
                }

                //redirect to the second partial view with the model
                return PartialView("_SearchAds", model);
            }
            catch (Exception ex) 
            {
                return View("Error", new HandleErrorInfo(ex, "ConvertImageDataToString", "Create"));
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private byte[] ConvertHttpPostedFileBaseToByteArray(HttpPostedFileBase fileBase)
        {
            byte[] returnData;

            //Using a memory stream, copying the fileBase-s input stream to it, 
            //then converting it to a byteArray and putting it to the returnData
            using (MemoryStream memoryStreamThatContainsTheImage = new MemoryStream())
            {
                fileBase.InputStream.CopyTo(memoryStreamThatContainsTheImage);
                returnData = memoryStreamThatContainsTheImage.GetBuffer();
            }

            return returnData;
        }

        private string ConvertImageDataToSourceString(byte[] data)
        {
            return String.Format($"data:image/jpg;base64,{Convert.ToBase64String(data)}");
        }
    }
}