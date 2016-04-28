using System;
using System.Web.Mvc;
using Core.Repositories;

namespace Presentation.Controllers
{
    public class StatisticsController : Controller
    {
        // GET: Statistics
        public ActionResult Index(string name)
        {
            if(string.IsNullOrEmpty(name))
            {
                name = "SverigesRadio";
            }
            var repo = new FaceRepository();
            var content = repo.GetForStartPage(name);
            return View(content);
        }
    }
}