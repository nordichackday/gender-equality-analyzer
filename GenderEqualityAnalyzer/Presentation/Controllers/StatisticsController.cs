using System;
using System.Web.Mvc;
using Core.Repositories;

namespace Presentation.Controllers
{
    public class StatisticsController : Controller
    {
        // GET: Statistics
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetGenderRepresentation(string broadcaster)
        {
            if (string.IsNullOrEmpty(broadcaster))
            {
                broadcaster = "SverigesRadio";
            }
            var repo = new FaceRepository();
            var content = repo.GetForStartPage(broadcaster);
            return View(content);
        }

        public ActionResult Details(string broadcaster)
        {
            var repo = new FaceRepository();
            var content = repo.GetForDetailsPage(broadcaster);
            return View(content);
        }

        public ActionResult Charts(string broadcaster)
        {
            
        }
    }
}