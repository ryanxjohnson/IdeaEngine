using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdeaEngineAuth.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IdeaEngineAuth.Models
{
    public class SchoolOverviewViewModel : HasPermissions
    {
        [DisplayName("School")]
        public override long ID { get; set; }

        [Required]
        [DisplayName("School Name")]
        public virtual String Name { get; set; }

        [DisplayName("School Contact Name")]
        public virtual String ContactName { get; set; }

        [DisplayName("Contact Phone Number")]
        [RegularExpression(@".*\d{3}.*\d{3}.*\d{4}.*", ErrorMessage="Must be a valid phone nuumber, such as (123) 456-7890")]
        public String ContactPhone { get; set; }

        [DisplayName("Contact Email Address")]
        [EmailAddress(ErrorMessage="Must be a vaid Email Address, such as user@example.com")]
        public virtual String ContactEmail { get; set; }

        [DisplayName("Emissaries")]
        public IEnumerable<EmissaryOverviewViewModel> Emissaries { get; set; }

        public override MvcHtmlString getName()
        {
            return MvcHtmlString.Create(Name);
        }

        [DisplayName("Current User")]
        public CurrentUser CurrentUser { get; set; }

        public override CurrentUser getCurrentUser()
        {
            return CurrentUser;
        }

        public override bool CanViewList()
        {
            return CurrentUser.CanViewSchoolList();
        }

        public override bool CanCreate()
        {
            return CurrentUser.CanCreateSchool();
        }

        public override bool CanViewDetails()
        {
            return CurrentUser.CanViewSchoolDetails();
        }

        public override bool CanEdit()
        {
            return CurrentUser.CanEditSchool(this);
        }

        public override bool CanDelete()
        {
            return CurrentUser.CanDeleteSchool();
        }
    }

    public class SchoolTuple : M_N_Tuple
    {
        public SchoolTuple(IEnumerable<SchoolOverviewViewModel> things, IEnumerable<SchoolOverviewViewModel> otherThings)
            : base(things, otherThings) { }

        public SchoolTuple(IEnumerable<SchoolOverviewViewModel> things) : base(things, new List<HasID>()) { }

        public override string getLable()
        {
            return "Associated Schools";
        }

        public override string getTitle()
        {
            return "School Name";
        }

        public override string getSetName()
        {
            return "Schools";
        }
    }

    public class SchoolOverviewList : UserViewModelList<SchoolOverviewViewModel>
    {
        public override bool CanViewList()
        {
            return CurrentUser.CanViewSchoolList();
        }

        public override bool CanCreate()
        {
            return CurrentUser.CanCreateSchool();
        }
    }

    public class SchoolDetailsViewModel : SchoolOverviewViewModel, UserViewModel
    {
        [DisplayName("Projects")]
        public IEnumerable<ProjectOverviewViewModel> Projects { get; set; }
    }

    public class SchoolEditViewModel : SchoolDetailsViewModel
    {
        [DisplayName("Other Emissaries")]
        public IEnumerable<EmissaryOverviewViewModel> OtherEmissaries { get; set; }

        [DisplayName("Other Projects")]
        public IEnumerable<ProjectOverviewViewModel> OtherProjects { get; set; }
    }
}