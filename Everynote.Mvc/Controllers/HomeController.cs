using Everynote.BusinessLayer;
using Everynote.Entities;
using Everynote.Mvc.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Everynote.Mvc.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            // Veritabanını yoksa oluşturan metod çağırılıyor
            BusinessLayer.Test test = new BusinessLayer.Test();
            test.CommetInsertTest();
            
            NoteManager noteManager = new NoteManager();
            return View(noteManager.GetAllNotesQueryable().OrderByDescending(q => q.CreatedOn).ToList());
        }

        // GET: Bu action kendine gelen id değerine göre bu kategori değerine sahip olan notları listelemek için BLL katmanından aldığı modeli
        // 'Home/Index' action'ına yönlendiriyor
        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            CategoryManager categoryManager = new CategoryManager();
            Category category = categoryManager.GetCategoryById(id.Value);

            if (category == null)
            {
                return HttpNotFound();
            }

            return View("Index", category.Notes.OrderByDescending(q => q.CreatedOn).ToList());
        }

        // GET: En çok beğenilenler notların listesini döndüren action
        public ActionResult MostLiked()
        {
            NoteManager noteManager = new NoteManager();
            
            return View("Index", noteManager.GetAllNotes().OrderByDescending(q => q.LikeCount).ToList());
        }

        // GET: About
        public ActionResult About()
        {
            return View();
        }






        // GET: Login, Login sayfası çağırıldığında çalışan action
        public ActionResult Login()
        {
            return View();
        }

        // POST: Login, Login sayfası post olduğunda çalışan action
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

            }

            return View(model);
        }

        // GET: Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Register, Register sayfası post olduğunda çalışan action
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            // Kullanıcı userName, e-posta kontrolleri
            // Kayıt işlemi
            // Aktivasyon email'i yollanması yapılır

            if (ModelState.IsValid)
            {

            }

            return View(model);
        }

        // Kullanıcının kendini aktif etmesini sağlayan action
        public ActionResult UserActivate(Guid guid)
        {
            // Kullanıcı aktivasyonu sağlanacak 

            return View();
        }

    }
}