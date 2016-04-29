using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Core.DataContext;
using Core.Repositories;
using Newtonsoft.Json;

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