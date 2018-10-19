using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everynote.Entities.Messages
{
	/// <summary>
	/// Mesaj göstermek için kullanılan sınıfların türediği base sınıf
	/// </summary>
	/// <typeparam name="T">Mesaj sınıflarından bir tür</typeparam>
	public class NotifyMessageBase<T>
	{
		public NotifyMessageBase()
		{
			Header = "Yönlendiriliyorsunuz...";
			Title = "Geçersiz İşlem";
			IsRedirecting = true;
			RedirectingUrl = "/Home/Index";
			RedirectingTimeout = 3000;
			Items = new List<T>();
		}

		public List<T> Items { get; set; }
		public string Header { get; set; }
		public string Title { get; set; }
		public bool IsRedirecting { get; set; }
		public string RedirectingUrl { get; set; }
		public int RedirectingTimeout { get; set; }
	}
}
