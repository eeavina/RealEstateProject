using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RealEstateProjectUI;
using RealEstateProjectUI.Models;

//currently not in use
namespace RealEstateProjectUI.Controllers
{
    public class PictureController : Controller
    {
        private RealEstateDBEntities db = new RealEstateDBEntities();

        // GET: Picture
        public ActionResult Index()
        {
            List<AdModels> adModels = new List<AdModels>();
            db.Picture.ForEachAsync(p => adModels.Add(new AdModels()
            {
                Id = p.Id,
                SourceString = ConvertImageDataToSourceString(p.Pic)
            }
            ));

            return View(adModels);
            //return View(db.Picture.ToList());
        }

        // GET: Picture/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.Picture.Find(id);

            AdModels adModel = new AdModels();

            if (picture == null)
            {
                return HttpNotFound();
            }
            else
            {
                adModel.Id = picture.Id;
                adModel.SourceString = ConvertImageDataToSourceString(picture.Pic);
            }
            return View(adModel);
            //return View(picture);
        }

        // GET: Picture/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Picture/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DataInHttpPostedFileBase")] AdModels adModels)
        {
            if (ModelState.IsValid)
            {
                Picture picture = new Picture()
                {
                    Pic = ConvertHttpPostedFileBaseToByteArray(adModels.DataInHttpPostedFileBase)
                };

                db.Picture.Add(picture);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(adModels);
            //return View(picture);
        }

        // GET: Picture/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.Picture.Find(id);

            AdModels adModels = new AdModels();

            if (picture == null)
            {
                return HttpNotFound();
            }
            else
            {
                adModels.Id = picture.Id;
            }
            return View(adModels);
            //return View(picture);
        }

        // POST: Picture/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DataHttpPostedFileBase")] AdModels adModels)
        {

            if (ModelState.IsValid)
            {
                Picture picture = db.Picture.Find(adModels.Id);

                if (adModels.DataInHttpPostedFileBase != null)
                {
                    picture.Pic = ConvertHttpPostedFileBaseToByteArray(adModels.DataInHttpPostedFileBase);
                }

                db.Entry(picture).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(adModels);
            //return View(picture);
        }

        // GET: Picture/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.Picture.Find(id);

            AdModels adModels = new AdModels();
            if (picture == null)
            {
                return HttpNotFound();
            }
            else
            {
                adModels.Id = picture.Id;
                adModels.SourceString = ConvertImageDataToSourceString(picture.Pic);
            }
            return View(adModels);
            //return View(picture);
        }

        // POST: Picture/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Picture picture = db.Picture.Find(id);
            db.Picture.Remove(picture);
            db.SaveChanges();
            return RedirectToAction("Index");
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
