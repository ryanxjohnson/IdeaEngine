using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using IdeaEngineAuth.Models;
using System.Net;
using System.Net.Mime;

namespace IdeaEngineAuth.Controllers
{
    public abstract class BaseEntityController<M, O, L, D, E> : BaseController<M>
        where L : UserViewModelList<O>, UserViewModel
        where O : HasPermissions
        where D : O, UserViewModel
        where E : D, UserViewModel
        where M : BaseEntityDomainModel<O, L, D, E>, new()
    {
        // GET: Projects
        public ActionResult Index()
        {
            L model = db.buildOverviewViewModelList(getCurrentUser());
            if ( !model.CanViewList() )
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            return View(model);
        }

        // GET: Projects/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            D model = db.buildDetailsViewModel(getCurrentUser(), id??0L);
            if (model == null)
            {
                return HttpNotFound();
            }
            if ( !model.CanViewDetails() )
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            return View(model);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            E model = db.buildCreateViewModel(getCurrentUser());
            if ( !model.CanCreate() )
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            return View(model);
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(E model)
        {
            model.CurrentUser = getCurrentUser();
            try
            {
                db.saveModelData(model);

                return RedirectToAction("Index");
                //return RedirectToAction("Details", new { id = model.ID });
            }
            catch (UnauthorizedAccessException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            //catch
            {
                //return View(model);
            }
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            E model = db.buildEditViewModel(getCurrentUser(), id??0L);
            if (model == null)
            {
                return HttpNotFound();
            }
            if ( !model.CanEdit() )
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            return View(model);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(long? id, E model)
        {
            model.CurrentUser = getCurrentUser();
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                db.saveModelData(model);

                return RedirectToAction("Details", new { id = id });
            }
            catch (UnauthorizedAccessException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            //catch
            {
                //return View(model);
            }
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            D model = db.buildDetailsViewModel(getCurrentUser(), id??0L);
            if (model == null)
            {
                return HttpNotFound();
            }
            if ( !model.CanDelete() )
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            return View(model);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(long? id, FormCollection collection)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                db.deleteModelData(getCurrentUser(), id??0L);

                return RedirectToAction("Index");
            }
            catch (UnauthorizedAccessException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
    }
}