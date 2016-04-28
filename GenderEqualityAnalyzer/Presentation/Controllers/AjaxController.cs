using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Core.Repositories;
using Newtonsoft.Json;

namespace Presentation.Controllers
{
    public class AjaxController : Controller
    {
        // GET: Ajax
        public ActionResult GetAnalysedByBroadcaster(string name)
        {
            var repo = new ArticleRepository();
            var content = repo.Get(x => x.ContainsPerson && x.Site.Name == name);
            var result = JsonConvert.SerializeObject(content, Formatting.None, new JsonSerializerSettings() {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore, MaxDepth = 1
                });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}