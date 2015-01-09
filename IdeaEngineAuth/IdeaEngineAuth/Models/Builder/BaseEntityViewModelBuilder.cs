using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdeaEngineAuth.Data;
using System.Data.Entity;
using System.Web.Mvc;

namespace IdeaEngineAuth.Models
{
    public abstract class BaseEntityViewModelBuilder<O, L, D, E, V> : BaseViewModelBuilder, BaseEntityDomainModel<O, L, D, E>
        where L : UserViewModelList<O>, UserViewModel
        where D : O, UserViewModel
        where E : D, UserViewModel
        where V : E, UserViewModel
    {
        public abstract L buildOverviewViewModelList(CurrentUser currentUser);

        protected abstract V buildOverviewInternal(CurrentUser currentUser, V model);

        protected abstract V buildDetailsInternal(CurrentUser currentUser, long id);

        public abstract E buildEditViewModel(CurrentUser currentUser, long id);

        public abstract E buildCreateViewModel(CurrentUser currentUser);

        public abstract bool saveModelData(E model);

        public abstract bool deleteModelData(CurrentUser currentUser, long id);

        public D buildDetailsViewModel(CurrentUser currentUser, long id)
        {
            return buildDetailsInternal(currentUser, id);
        }

        protected IEnumerable<R> buildList<S, R>(IEnumerable<S> source, Func<S, R> getItem)
        {
            List<R> list = new List<R>();
            foreach (S item in source)
            {
                list.Add(getItem(item));
            }
            return list;
        }

        protected IEnumerable<T> makeListFromItem<T>(T item) where T : HasID
        {
            return item != null && item.ID != 0 ? new List<T>() { item } : new List<T>();
        }

        protected Revision getRevision(UserViewModel model)
        {
            return db.Revisions.Add(new Revision()
            {
                Employee_ID = model.CurrentUser.Domain_ID,
                Date = DateTime.Now,
            });
        }

        protected R getRevisionOf<T, R>(HasID model, Revision revision, DbSet<T> tTable, DbSet<R> rTable)
            where T : class, new()
            where R : class,  RevisionOf<T, R>, new()
        {
            return rTable.Add(new R().build(revision, model.ID != 0 ? tTable.Find(model.ID) : tTable.Add(new T())));
        }

        protected void saveUpdate<ModelItem, OtherItem, Something>(ModelItem modelItem, OtherItem oldItem, Func<ModelItem, OtherItem> transform, Func<OtherItem, Something> saveItem)
        {
            if (modelItem != null)
            {
                OtherItem newItem = transform(modelItem);
                if ( newItem != null && (oldItem == null || !newItem.Equals(oldItem)) )
                {
                    saveItem(newItem);
                }
            }
        }

        protected void processIncludes<ModelItem, Something>(IEnumerable<ModelItem> modelList, IEnumerable<ModelItem> oldList, Func<ModelItem, bool, Something> addInclude)
            where ModelItem : HasID
        {
            if (modelList == null)
            {
                modelList = new List<ModelItem>();
            }

            if (oldList == null)
            {
                foreach (ModelItem item in modelList)
                {
                    addInclude(item, true);
                }
            }
            else
            {
                foreach (ModelItem item in modelList.Except(oldList, idCompare))
                {
                    addInclude(item, true);
                }

                foreach (ModelItem item in oldList.Except(modelList, idCompare))
                {
                    addInclude(item, false);
                }
            }
        }

        private readonly IDCompare idCompare = new IDCompare();

        private class IDCompare : IEqualityComparer<HasID>
        {
            public bool Equals(HasID has1, HasID has2)
            {
                // !has1.ID.Equals(0L) is to prevent de-duping of new items that don't have an id yet.
                // This assumes that oldList will never have any items with id==0, or double modificaion
                // will occur and throw an exception when the unique constraint of the db is violated.
                return has1 != null && has2 != null && !has1.ID.Equals(0L) && has1.ID.Equals(has2.ID);
            }

            public int GetHashCode(HasID hasID)
            {
                return hasID == null ? 0 : hasID.ID.GetHashCode();
            }
        }

        public FileLink getFileLink(CurrentUser currentUser, long fileID)
        {
            return currentUser.IsAdmin() || 0.Equals(db.CurrentFileStatus.Find(fileID).Archived??1)
                ? db.CurrentFileDatas.Find(fileID) : null;
        }
    }
}