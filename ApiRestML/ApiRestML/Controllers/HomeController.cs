using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ApiRestML.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Ejercicio Mercado Libre";

            return View();
        }
        //http://apirestml-dev.us-west-2.elasticbeanstalk.com/

        public string prueba(int id)
        {
            return id.ToString();
        }
    }
}
