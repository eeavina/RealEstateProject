using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RealEstateProjectUI;
using System.Data.Entity.Validation;
using RealEstateProjectUI.Models;
using System.IO;

namespace RealEstateProjectUI.Controllers
{
    public class AdController : Controller
    {
        //instanciating the db entity class
        private RealEstateDBEntities db = new RealEstateDBEntities();

        // GET: Ad
        public ActionResult Index()
        {
            //creating a new admodels list 
            List<AdModels> adModels = new List<AdModels>();

            //filling up the list while iterating thru the database
            foreach (var a in db.Ad)
            {
                //mapping the properties
                adModels.Add(new Models.AdModels()
                {
                    Id = a.Id,
                    Title = a.Title,
                    District = a.District,
                    Street = a.Street,
                    Description = a.Description,
                    Price = a.Price,
                    Size = a.Size,
                    PictureId = a.Picture.Id,
                    //calling a method for data conversion
                    SourceString = ConvertImageDataToSourceString(a.Picture.Pic)
                });
            }
            return View(adModels);
        }

        // GET: Ad/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //initializing an ad and picture, using the ad id parameter
            Ad ad = db.Ad.Find(id);
            Picture picture = db.Picture.Find(id);
            User user = db.User.Find(ad.UserId);

            //instanciating the admodel class
            AdModels adModels = new AdModels();

            //mapping
            if (ad == null)
            {
                return HttpNotFound();
            }
            else
            {
                adModels.Id = ad.Id;
                adModels.Title = ad.Title;
                adModels.District = ad.District;
                adModels.Street = ad.Street;
                adModels.Description = ad.Description;
                adModels.Price = ad.Price;
                adModels.Size = ad.Size;
                adModels.UserId = ad.UserId;
                //checking if a picture was also added
                if (ad.PictureId != null)
                {
                    adModels.PictureId = ad.Picture.Id;
                    adModels.SourceString = ConvertImageDataToSourceString(ad.Picture.Pic);

                }
                adModels.UserName = user.UserName;
                adModels.UserEmail = user.Email;
            }

            return View(adModels);
        }

        // GET: Ad/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ad/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AdModels adModels)
        {
            if (ModelState.IsValid)
            {
                //instanciating the ad class and mapping the properties
                Ad ad = new Ad();
                ad.Title = adModels.Title;
                ad.District = adModels.District;
                ad.Street = adModels.Street;
                if (adModels.Description.Length>1000) /*correctionNeeded*/
                {
                    ModelState.AddModelError(nameof(adModels.Description), "A mező nem fogad többet 1000 karakternél");
                    return View(adModels);
                }
                ad.Description = adModels.Description;
                ad.Price = adModels.Price;
                ad.Size = adModels.Size;
                ad.UserId = int.Parse(Session["UserId"]?.ToString());

                if (adModels.DataInHttpPostedFileBase==null)
                {
                    ModelState.AddModelError(nameof(adModels.DataInHttpPostedFileBase), "Kép feltöltése kötelező");
                    return View();
                }
                //instanciating the picture class and mapping the properties
                Picture picture = new Picture()
                {
                    Pic = ConvertHttpPostedFileBaseToByteArray(adModels.DataInHttpPostedFileBase)
                };
                ad.PictureId = picture.Id;
                

                //adding both the picture and the ad to the database
                db.Picture.Add(picture);
                db.Ad.Add(ad);
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            return View(adModels);
        }

        // GET: Ad/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ad ad = db.Ad.Find(id);

            Picture picture = db.Picture.Find(id);
            AdModels adModels = new AdModels();

            //mapping
            if (ad == null)
            {
                return HttpNotFound();
            }
            else
            {
                adModels.Id = ad.Id;
                adModels.Title = ad.Title;
                adModels.District = ad.District;
                adModels.Street = ad.Street;
                adModels.Description = ad.Description;
                adModels.Price = ad.Price;
                adModels.Size = ad.Size;
                adModels.UserId = ad.UserId;
                if (ad.PictureId != null)
                {
                    adModels.PictureId = ad.Picture.Id;
                    adModels.SourceString = ConvertImageDataToSourceString(ad.Picture.Pic);

                }
            }

            return View(adModels);
        }

        // POST: Ad/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,District,Street,Description,Size,Price,PictureId,DataHttpPostedFileBase")] AdModels adModels)
        {
            if (ModelState.IsValid)
            {
                Ad ad = db.Ad.Find(adModels.Id);
                Picture picture = db.Picture.Find(ad.PictureId);

                //mapping
                if (adModels.DataInHttpPostedFileBase != null)
                {
                    picture.Pic = ConvertHttpPostedFileBaseToByteArray(adModels.DataInHttpPostedFileBase);
                    db.Entry(picture).State = EntityState.Modified;
                }
                
                ad.UserId = int.Parse(Session["UserId"]?.ToString());
                ad.Id = adModels.Id;
                ad.Title = adModels.Title;
                ad.District = adModels.District;
                ad.Street = adModels.Street;
                ad.Description = adModels.Description;
                ad.Price = adModels.Price;
                ad.Size = adModels.Size;

                db.Entry(ad).State = EntityState.Modified;
                
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(adModels);
        }

        // GET: Ad/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ad ad = db.Ad.Find(id);

            Picture picture = db.Picture.Find(ad.PictureId);
            AdModels adModels = new AdModels();

            if (ad == null)
            {
                return HttpNotFound();
            }
            else
            {
                adModels.Id = ad.Id;
                adModels.Title = ad.Title;
                adModels.District = ad.District;
                adModels.Street = ad.Street;
                adModels.Description = ad.Description;
                adModels.Price = ad.Price;
                adModels.Size = ad.Size;
                adModels.PictureId = picture.Id;
                adModels.SourceString = ConvertImageDataToSourceString(picture.Pic);
                adModels.UserId = int.Parse(Session["UserId"]?.ToString());
            }
            return View(adModels);
        }

        // POST: Ad/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ad ad = db.Ad.Find(id);
            Picture picture = db.Picture.Find(ad.PictureId);
            db.Ad.Remove(ad);
            if (ad.PictureId != null)
            {
                db.Picture.Remove(picture);
            }
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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
