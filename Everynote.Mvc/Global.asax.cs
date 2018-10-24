using Everynote.Core.Authentication;
using Everynote.Mvc.Init;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Everynote.Mvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

			//Common katmanına kullanıcının gerekli bilgilerini atamak için kullanılıyor
			App.Common = new WebCommon();

        }
		
    }
}
