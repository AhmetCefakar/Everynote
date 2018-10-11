using Everynote.DataAccessLayer.EntityFramework;
using Everynote.Entities;
using Everynote.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everynote.BusinessLayer
{
	/// <summary>
	/// Kullanıcı işlemlerinin yönetildiği kodları barındıran sınıf
	/// </summary>
	public class EverynoteUserManager
	{
		private Repository<User> repoUser = new Repository<User>();

		/// <summary>
		/// Yeni kullanıcı keydetme işlemini yapan metod
		/// Yapılanlar;
		/// - Kullanıcı userName, e-posta kontrolleri
		/// - Kayıt işlemi
		/// - Aktivasyon email'i yollanması yapılır
		/// </summary>
		/// <param name="registerDTO"></param>
		/// <returns>BLL katmanında Entity kısıtları dışında özel hatalar(aynı isimli kullanıcı kaydetmek istemek gibi) oluşursa bu hataları tutan generic model</returns>
		public BusinessLayerResult<User> RegisterUser(RegisterDTO registerDTO)
		{
			User user = repoUser.Find(q => q.UserName == registerDTO.UserName || q.Email == registerDTO.Email);
			BusinessLayerResult<User> layerResult = new BusinessLayerResult<User>(); // Geriye döndürülecek olan generic hata listesini tutan model

			if (user != null)
			{ // İlgili name-password değerlerinde kullanıcı varsa bu bloğa giriliyor
				if (user.UserName == registerDTO.UserName)
				{
					layerResult.Errors.Add("Kullanıcı adı kayıtlı!");
				}

				if (user.Email == registerDTO.Email)
				{
					layerResult.Errors.Add("Email adı kayıtlı!");
				}
			}
			else
			{ // Yeni kullaıncıyı kayıt işlemi
				int dbResult = repoUser.Insert(new User
				{
					UserName = registerDTO.UserName,
					Email = registerDTO.Email,
					Password = registerDTO.Password,
					IsActive = false,
					IsAdmin = false,
					ActivateGuid = Guid.NewGuid()
				});

				if (dbResult > 1)
				{
					layerResult.Result = repoUser.Find(q => q.UserName == registerDTO.UserName && q.Email == registerDTO.Email && q.Password == registerDTO.Password);

					// TODO: Hesap doğrulama mail'i atılacak

				}
			}

			return layerResult;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public BusinessLayerResult<User> LoginUser(LoginDTO loginDTO)
		{
			BusinessLayerResult<User> layerResult = new BusinessLayerResult<User>(); // Geriye döndürülecek olan generic hata listesini tutan model
			layerResult.Result = repoUser.Find(q => q.UserName == loginDTO.UserName && q.Password == loginDTO.Password);
			

			if (layerResult.Result != null)
			{
				if (!layerResult.Result.IsActive)
				{
					layerResult.Errors.Add("Kullanıcı Aktif Değil, Email'inizi kontrol ediniz");
				}
			}
			else
			{
				layerResult.Errors.Add("Kullanıcı Aktifya da şifre uyuşmuyor");
			}

			return layerResult;
		}

	}
}
