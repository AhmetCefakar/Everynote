using Everynote.DataAccessLayer.EntityFramework;
using Everynote.Entities;
using Everynote.Entities.DTO;
using Everynote.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SendFastEmail.Models;
using SendFastEmail.Enums;
using System.Net.Mail;

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
					layerResult.AddError(code: ErrorMessageCode.UserNameAlreadyExist, message: "Kullanıcı Adı Kayıtlı!");
				}

				if (user.Email == registerDTO.Email)
				{
					layerResult.AddError(code: ErrorMessageCode.EmailAlreadyExist, message: "Email adı kayıtlı!");
				}
			}
			else
			{ // Yeni kullaıncıyı kayıt işlemi
				User newUser = new User
				{
					UserName = registerDTO.UserName,
					Email = registerDTO.Email,
					Password = registerDTO.Password,
					IsActive = false,
					IsAdmin = false,
					ActivateGuid = Guid.NewGuid()
				};
				int dbResult = repoUser.Insert(newUser);

				if (dbResult > 0)
				{
					layerResult.Result = repoUser.Find(q => q.UserName == registerDTO.UserName && q.Email == registerDTO.Email && q.Password == registerDTO.Password);

					// TODO: Hesap doğrulama mail'i atılacak
					// Yollanacak mail'in ayarlanması
					string activateUrl = $"yourServerDomainAddress/Home/UserActivate/{layerResult.Result.ActivateGuid}";
					MailContent mailContent = new MailContent
					{
						From = new MailAddress("yourMailAddress"),
						ToList = new List<MailAddress> { new MailAddress(layerResult.Result.Email.Trim()) },
						Subject = "EveryNote Account Activation",
						Body = $"Merhaba, {layerResult.Result.UserName}; <br/><br/> Hesabınızı aktifleştirmek için <a href='http://{activateUrl}' target = '_blank'>tıklayıyınız.</a>",
						IsBodyHtml = true,
						SmtpConfiguration = new SmtpConfiguration
						{
							Host = "smtp.gmail.com",
							Port = 587,
							EnableSsl = true,
							UseDefaultCredentials = false,
							DeliveryMethod = SmtpDeliveryMethod.Network
						},
						Email = "yourMailAddress",
						Password = "yourMailPassword"
					};

					// Mail gönderme metodunun çağırılması ve sonucun 'MailSendResult' modeline atanması
					MailSendResult sendResult = SendFastEmail.EMail.Send(mailContent);
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
					layerResult.AddError(ErrorMessageCode.UserIsNotActive, "Kullanıcı Aktif Değil");
					layerResult.AddError(ErrorMessageCode.CheckYourEmail, "Email'inizi kontrol ediniz");
				}
			}
			else
			{
				layerResult.AddError(ErrorMessageCode.UserNameOrPasswordWrong, "Kullanıcı adı ya da şifre uyuşmuyor");
			}

			return layerResult;
		}


		public BusinessLayerResult<User> ActivateUser(Guid activateId)
		{
			BusinessLayerResult<User> layerResult = new BusinessLayerResult<User>(); // Geriye döndürülecek olan generic hata listesini tutan model
			layerResult.Result = repoUser.Find(q => q.ActivateGuid == activateId);

			if (layerResult.Result != null)
			{
				if (layerResult.Result.IsActive)
				{
					layerResult.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı Zaten Aktif!");
					return layerResult;
				}

				layerResult.Result.IsActive = true;
				repoUser.Update(layerResult.Result);
			}
			else
			{
				layerResult.AddError(ErrorMessageCode.ActivateIdDoesNotExist, "Aktifleştirilecek Kullanıcı Bulunamadı!");
			}

			return layerResult;
		}

	}
}
