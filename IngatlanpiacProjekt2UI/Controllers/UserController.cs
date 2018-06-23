using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using RealEstateProjectUI;
using RealEstateProjectUI.Models;
using Microsoft.AspNet.Identity;
using System.Web.Helpers;

namespace RealEstateProjectUI.Controllers
{
    public class UserController : Controller
    {
        private RealEstateDBEntities db = new RealEstateDBEntities();

        
        /// <summary>
        /// GET: User
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var user = db.User.Include(u => u.UserRole);
            return View(user.ToList());
        }

        // GET: User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // GET: /User/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            //ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /User/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            //check if model binding is correct
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //get hashed password and salt from the database
            var hashedPassword = db.User.Where(u => u.Email == model.Email).Select(u => u.Password).FirstOrDefault();
            var salt = db.User.Where(u => u.Email == model.Email).Select(u => u.Salt).FirstOrDefault();
            var emailList = db.User.Select(e => e.Email).ToList();

            //validate the email address
            if (!emailList.Contains(model.Email))
            {
                ModelState.AddModelError(nameof(model.Email), "Ez az email cím nem szerepel az adatbázisunkban.");
                return View(model);
            }

            //validate the password
            if (model.Password == null || Crypto.SHA256(model.Password + salt) != hashedPassword)
            {
                ModelState.AddModelError(nameof(model.Password), "A jelszó nem megfelelő.");
                return View(model);
            }

            //instantiate a user
            User user = db.User.Where(u => u.Email == model.Email && u.Password == hashedPassword).FirstOrDefault();

            //update the session objects for further authorization
            if (user != null)
            {
                Session["UserRole"] = user.UserRole.Name;
                Session["UserName"] = user.UserName;
                Session["UserId"] = user.Id;
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        public ActionResult LogOff()
        {
            Session["UserRole"] = "";
            Session["UserName"] = "";
            Session["UserId"] = "";
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: User/Create
        [AllowAnonymous]
        public ActionResult Create()
        {
            //ViewBag.UserRoleId = db.UserRole.ToList();
            return View();
        }

        //
        // POST: User/Create
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserModels model)
        {
            if (ModelState.IsValid)
            {
                User user = new RealEstateProjectUI.User();
                var salt = Crypto.GenerateSalt();
                var hashedPassword = Crypto.SHA256(model.Password + salt);
                var userList = db.User.Select(u => u.UserName).ToList() ;
                var emailList = db.User.Select(e => e.Email).ToList();

                user.UserName = model.UserName;
                user.Email = model.Email;
                user.Salt = salt;

                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError(nameof(model.ConfirmPassword), "A jelszavak nem egyeznek.");
                    return View(model);
                }

                if (userList.Contains(model.UserName))
                {
                    ModelState.AddModelError(nameof(model.UserName), "Ez a felhasználónév már foglalt.");
                    return View(model);
                }

                if (emailList.Contains(model.Email))
                {
                    ModelState.AddModelError(nameof(model.Email), "Ez az email cím már foglalt.");
                    return View(model);
                }

                if (model.ConfirmPassword == model.Password)
                {
                    user.Password = hashedPassword;
                }
                
                //user.UserRoleId = model.UserRoleId;
                if (model.Email == "admin@email.com")
                {
                    user.UserRoleId = 1;
                }
                else
                {
                    user.UserRoleId = 2;
                }

                db.User.Add(user);
                db.SaveChanges();

                //update the session objects for further authorization
                if (user != null)
                {
                    if (model.UserRoleId==1)
                    {
                        Session["UserRole"] = "admin";

                    }
                    else
                    {
                        Session["UserRole"] = "advertiser";

                    }
                    Session["UserName"] = user.UserName;
                    Session["UserId"] = user.Id;

                    return RedirectToAction("Index", "Home");
                }

                return RedirectToAction("Index", "Home");
            }

            ViewBag.UserRoleId = db.UserRole.ToList();
            // If we got this far, something failed, redisplay form
            return View(model);
        }


        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserRoleId = new SelectList(db.UserRole, "Id", "Name", user.UserRoleId);
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserName,Email,Password,UserRoleId")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserRoleId = new SelectList(db.UserRole, "Id", "Name", user.UserRoleId);
            return View(user);
        }

        // GET: User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.User.Find(id);
            db.User.Remove(user);
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
    }
}
