using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Everynote.Mvc.ViewModel
{
    public class LoginViewModel
    {
        // '{0}' ile DisplayName, '{1}' ile property name belirtiliyor.
        [DisplayName("Kullanıcı Adı"), Required(ErrorMessage ="{0} alanı boş geçilemez"), StringLength(25)]
        public string UserName { get; set; }

        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı boş geçilemez"), DataType(DataType.Password), StringLength(25)]
        public string Password { get; set; }
    }
}