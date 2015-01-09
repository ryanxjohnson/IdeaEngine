using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using IdeaEngineAuth.Models;

namespace IdeaEngineAuth.Controllers
{
    public class DownloadController : BaseController<FileViewModelBuilder>
    {
        // GET: Download
        public ActionResult FAQ()
        {
            return File(HttpContext.Server.MapPath("~/App_Data/Downloadable/idea_engine_user_guide.pdf"),
                MediaTypeNames.Application.Octet, "Idea_Engine_User_Guide.pdf");
        }

        // GET: Download/Details/5
        public ActionResult File(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FileLink file = db.getFileLink(getCurrentUser(), id ?? 0L);
            if (file == null)
            {
                return HttpNotFound();
            }
            return File(file.buildPath(), MediaTypeNames.Application.Octet, file.FileName);
        }
    }
}
