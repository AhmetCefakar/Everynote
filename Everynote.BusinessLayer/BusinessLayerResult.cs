using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everynote.BusinessLayer
{
	/// <summary>
	/// Hata listesini ve generic Entity sınıfını taşımak için kullanılan sınıf
	/// </summary>
	/// <typeparam name="T">Hata </typeparam>
	public class BusinessLayerResult<T> where T : class, new()
	{
		public BusinessLayerResult()
		{
			Errors = new List<string>();
		}

		public List<string> Errors { get; set; }
		public T Result { get; set; }
	}
}
