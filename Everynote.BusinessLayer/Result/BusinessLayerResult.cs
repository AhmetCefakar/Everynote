using Everynote.Entities.Enums;
using Everynote.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everynote.BusinessLayer.Result
{
	/// <summary>
	/// Hata listesini ve generic Entity sınıfını taşımak için kullanılan sınıf
	/// </summary>
	/// <typeparam name="T">Hata </typeparam>
	public class BusinessLayerResult<T> where T : class, new()
	{
		public BusinessLayerResult()
		{
			Errors = new List<ErrorMessage>();
		}

		public List<ErrorMessage> Errors { get; set; }
		public T Result { get; set; }

		/// <summary>
		/// Hata eklemeye sağlayan metod
		/// </summary>
		/// <param name="code">Hata Kodu</param>
		/// <param name="message">Hata Açıklaması</param>
		public void AddError(ErrorMessageCode code, string message)
		{
			Errors.Add(new ErrorMessage { Code = code, Message = message });
		}
	}
}
