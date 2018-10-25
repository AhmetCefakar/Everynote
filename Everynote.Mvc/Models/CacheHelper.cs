using Everynote.BusinessLayer;
using Everynote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace Everynote.Mvc.Models
{
	/// <summary>
	/// Cache için kullanılan sınıf(Cache yapısı genişletilebilir, bir klasör altına alınıp her bir tablo için ayrı sınıflardan çalışılabilir)
	/// 'Cache' işlemleri back-end tarafında yapılmaktadır
	/// </summary>
	public class CacheHelper
	{
		public static List<Category> GetCategoriesFromCache()
		{
			// Cache boş ise veritabanından çek ve cache'i Category listesi ile doldur
			if (WebCache.Get("categoryCacheList") == null)
			{
				CategoryManager categoryManager = new CategoryManager();
				WebCache.Set("categoryCacheList", categoryManager.List(), 30, true);
			}
			return  WebCache.Get("categoryCacheList") as List<Category>;
		}

		// 'categoryCacheList' cache değerini temizleyen metod
		public static void RemoveCategoryCache()
		{
			RemoveCache("categoryCacheList");
		}

		// Verilen anahtardaki Cache virisini temizleyen metod
		public static void RemoveCache(string key)
		{
			WebCache.Remove(key);
		}
	}
}