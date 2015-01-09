using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IdeaEngineAuth.Models;

namespace IdeaEngineAuth.Controllers
{
    public abstract class BaseController<M> : Controller
        where M : BaseDomainModel, new()
    {
        protected M db = new M();

        protected CurrentUser getCurrentUser()
        {
            return db.getEmployeePermissions(
                (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                ? System.Web.HttpContext.Current.User.Identity.Name
                : null
            );
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}