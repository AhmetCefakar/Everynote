using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everynote.Entities.Messages
{
	public class SuccessViewModel : NotifyMessageBase<string>
	{
		public SuccessViewModel()
		{
			Title = "İşlem Başarılı";
		}
	}
}
