using System.Web.Mvc;
using Core.Repositories;

namespace Presentation.Controllers
{
    public class AjaxController : Controller
    {
        // GET: Ajax
        public ActionResult GetAnalysedByBroadcaster(string name)
        {
            var repo = new FaceRepository();

            var content = repo.GetForStartPage(name);
           
            return Json(content, JsonRequestBehavior.AllowGet);
        }
    }
}