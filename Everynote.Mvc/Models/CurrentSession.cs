using Everynote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Everynote.Mvc.Models
{
	/// <summary>
	/// Oturum açan kullanıcının Session verisi üzerinde işlemler yapmak için kullanılan sınıf
	/// </summary>
	public class CurrentSession
	{
		/// <summary>
		/// Geçerli bir session varsa geriye döndüren metod
		/// </summary>
		public static User User
		{
			get
			{
				return Get<User>("login");
			}
		}

		/// <summary>
		/// Sessina parametre atan metod
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="obj"></param>
		public static void Set<T>(string key, T obj)
		{
			//if (HttpContext.Current.Session["login"] != null)
			//{
				HttpContext.Current.Session[key] = obj;
			//}
		}

		/// <summary>
		/// Sessindan parametre okuyan metod, sessionda değer bulunamazsa ilgili T tipinin varsayılan değerini döner
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		public static T Get<T>(string key)
		{
			if (HttpContext.Current.Session[key] != null)
			{
				return (T)HttpContext.Current.Session[key];
			}
			return default(T); // Burada null yazılmadı cünkü T tipi int, string, class, struck gibi farklı türden olabilir ve int gibi türler null değer alamaz
		}

		/// <summary>
		/// İlgili Session'daki key değerini silen metod
		/// </summary>
		/// <param name="key"></param>
		public static void Remove(string key)
		{
			if (HttpContext.Current.Session[key] != null)
			{
				HttpContext.Current.Session.Remove(key);
			}
		}

		/// <summary>
		/// İlgili kullanıcının session verilerini silen metod
		/// </summary>
		public static void Clear()
		{
			HttpContext.Current.Session.Clear(); // İlgili Session için bütün verileri temizler
		}
	}
}
