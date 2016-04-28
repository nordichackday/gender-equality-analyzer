using System;
using System.Web.Mvc;

namespace Presentation.Controllers
{
    public class StatisticsController : Controller
    {
        // GET: Statistics
        public ActionResult Index()
        {

            double totalProcessedImages = 2153;
            double totalMale = 1600;
            double totalFemale = 553;

            var percentMale = Math.Round((totalMale / totalProcessedImages) * 100);
            var percentFemale = Math.Round((totalFemale / totalProcessedImages) * 100);




            return View();
        }
    }
}