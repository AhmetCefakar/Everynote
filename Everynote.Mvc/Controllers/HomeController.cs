using Everynote.BusinessLayer;
using Everynote.Entities;
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
            return View(noteManager.GetAllNotes());
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

            return View("Index", category.Notes);
        }

    }
}