using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Everynote.BusinessLayer;
using Everynote.Entities;
using Everynote.Mvc.Filter;
using Everynote.Mvc.Models;

namespace Everynote.Mvc.Controllers
{
	[UserAuthentication]
	[AdminAuthentication]
	public class CategoryController : Controller
	{
		private readonly CategoryManager categoryManager = new CategoryManager();

		// GET: Categories
		public ActionResult Index()
		{
			return View(categoryManager.List());
		}

		// GET: Categories/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Category category = categoryManager.Find(q => q.Id == id.Value);

			if (category == null)
			{
				return HttpNotFound();
			}
			return View(category);
		}

		// GET: Categories/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Categories/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "Id,Title,Description,CreatedUserName,CreatedOn,ModifiedUserName,ModifiedOn")] Category category)
		{
			ModelState.Remove("CreatedUserName");
			ModelState.Remove("CreatedOn");
			if (ModelState.IsValid)
			{
				categoryManager.Insert(category);
				CacheHelper.RemoveCategoryCache();
				
				return RedirectToAction("Index");
			}

			return View(category);
		}

		// GET: Categories/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Category category = categoryManager.Find(q => q.Id == id.Value);
			if (category == null)
			{
				return HttpNotFound();
			}
			return View(category);
		}

		// POST: Categories/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "Id,Title,Description,CreatedUserName,CreatedOn,ModifiedUserName,ModifiedOn")] Category category)
		{
			ModelState.Remove("CreatedUserName");
			ModelState.Remove("CreatedOn");

			if (ModelState.IsValid)
			{
				// Veritabanından ilgili kayıt çekilir, güncellenir ve update işlemi tamamlanır
				Category categoryEdited = categoryManager.Find(q => q.Id == category.Id);
				categoryEdited.Title = category.Title;
				categoryEdited.Description = category.Description;

				categoryManager.Update(categoryEdited);
				CacheHelper.RemoveCategoryCache();

				return RedirectToAction("Index");
			}
			return View(category);
		}

		// GET: Categories/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Category category = categoryManager.Find(q => q.Id == id.Value);
			if (category == null)
			{
				return HttpNotFound();
			}
			return View(category);
		}

		// POST: Categories/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Category category = categoryManager.Find(q => q.Id == id);
			CacheHelper.RemoveCategoryCache();

			return RedirectToAction("Index");
		}

	}
}
