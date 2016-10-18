using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MessageLogger.UI.Controllers
{
    public class ApplicationController : Controller
    {
        public ViewResult Index()
        {
            ViewBag.APIURL = ConfigurationManager.AppSettings["MessageLogger.API.BaseURL"];

            return View();
        }
    }
}