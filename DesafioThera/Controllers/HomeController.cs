﻿using System.Web.Mvc;

namespace DesafioThera.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (TempData["password"] != null)
            {
                ViewBag.Password = "Aqui está a senha gerada: " + TempData["password"].ToString();
                TempData.Remove("password");
            }
            return View();
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}