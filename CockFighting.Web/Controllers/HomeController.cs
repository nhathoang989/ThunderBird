using CockFighting.ViewModels;
using CockFighting.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CockFighting.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            SWUserViewModel<DerbyRegisterViewModel> register = new SWUserViewModel<DerbyRegisterViewModel>(new User());
            return View(register.View);
        }

        [HttpPost]
        public ActionResult Register(DerbyRegisterViewModel view)
        {
            SWUserViewModel<DerbyRegisterViewModel> register = new SWUserViewModel<DerbyRegisterViewModel>(view);
            register.SaveModel(true);
            return Redirect("/");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}