using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IdeaEngineAuth.Models
{
    public static class ViewUtils
    {
        public static MvcHtmlString ToParagraphs(this HtmlHelper html, string value)
        {
            return MvcHtmlString.Create("<p>" + String.Join("</p><p>", html.Encode(value).Replace("\r", String.Empty).Split('\n').Where(a => a.Trim() != string.Empty)) + "</p>");
        }

        public static MvcHtmlString ToYesNoString(this HtmlHelper html, bool? value)
        {
            return MvcHtmlString.Create(value??false ? "Yes" : "No");
        }
    }

    public abstract class HasCurrentUser
    {
        public abstract CurrentUser getCurrentUser();
    }

    public abstract class HasID : HasCurrentUser
    {
        public virtual long ID { get; set; }

        public abstract MvcHtmlString getName();
    }

    public abstract class HasPermissions : HasID
    {
        public abstract bool CanViewList();

        public abstract bool CanViewDetails();

        public abstract bool CanCreate();

        public abstract bool CanEdit();

        public abstract bool CanDelete();
    }

    public abstract class M_N_Tuple : Tuple<IEnumerable<HasID>, IEnumerable<HasID>>
    {
        public M_N_Tuple(IEnumerable<HasID> things, IEnumerable<HasID> otherThings)
            : base(things, otherThings) { }

        public abstract String getLable();

        public abstract String getTitle();

        public abstract String getSetName();
    }

    public interface UserViewModel
    {
        CurrentUser CurrentUser { get; set; }

        CurrentUser getCurrentUser();
    }

    public abstract class UserViewModelList<O> : HasCurrentUser, IEnumerable<O>, UserViewModel
    {
        public abstract bool CanViewList();

        public abstract bool CanCreate();

        [DisplayName("Current User")]
        public CurrentUser CurrentUser { get; set; }

        public override CurrentUser getCurrentUser()
        {
            return CurrentUser;
        }

        private readonly List<O> internalList = new List<O>();

        public void Add(O item)
        {
            internalList.Add(item);
        }

        public IEnumerator<O> GetEnumerator()
        {
            return internalList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}