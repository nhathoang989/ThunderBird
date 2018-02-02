using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CockFighting.ViewModels;
using CockFighting.Models;

namespace CockFighting.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            SWUserViewModel<DerbyRegisterViewModel> register = new SWUserViewModel<DerbyRegisterViewModel>(
                new User());
            
            return View(register);
        }
    }
}
