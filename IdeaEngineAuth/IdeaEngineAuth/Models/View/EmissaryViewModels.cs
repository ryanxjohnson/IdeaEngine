using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdeaEngineAuth.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace IdeaEngineAuth.Models
{
    public class Employee : HasPermissions
    {
        [DisplayName("Employee")]
        public override long ID { get; set; }

        [Required]
        [DisplayName("Domain Username")]
        public virtual String Domain_ID { get; set; }

        [DisplayName("Employee Name")]
        public virtual String Name { get; set; }

        [Required]
        [DisplayName("Admin Permissions")]
        public virtual bool? Admin { get; set; }

        [Required]
        [DisplayName("Ambassador Permissions")]
        public virtual bool? Ambassador { get; set; }

        public override MvcHtmlString getName()
        {
            String username;
            if ( Domain_ID.Contains("\\") || Domain_ID.Contains("/") )
            {
                username = Domain_ID.Split(new char[] { '\\', '/' }, 2)[1];
            }
            else
            {
                username = Domain_ID;
            }
            return MvcHtmlString.Create("<a href='mailto:" + username + "@commercebank.com?subject=Idea Engine - '>" + Name + "</a>");
        }

        [DisplayName("Current User")]
        public CurrentUser CurrentUser { get; set; }

        public override CurrentUser getCurrentUser()
        {
            return CurrentUser;
        }

        public override bool CanViewList()
        {
            return CurrentUser.CanViewEmissaryList();
        }

        public override bool CanCreate()
        {
            return CurrentUser.CanCreateEmissary();
        }

        public override bool CanViewDetails()
        {
            return CurrentUser.CanViewEmissaryDetails(this);
        }

        public override bool CanEdit()
        {
            return CurrentUser.CanEditEmissary(this);
        }

        public override bool CanDelete()
        {
            return CurrentUser.CanDeleteEmissary();
        }
    }

    public class EmissaryTuple : M_N_Tuple
    {
        public EmissaryTuple(IEnumerable<EmissaryOverviewViewModel> things, IEnumerable<EmissaryOverviewViewModel> otherThings)
            : base(things, otherThings) { }

        public EmissaryTuple(IEnumerable<EmissaryOverviewViewModel> things) : base(things, new List<HasID>()) { }

        public override string getLable()
        {
            return "Emissaries";
        }

        public override string getTitle()
        {
            return "Emissary Name";
        }

        public override string getSetName()
        {
            return "Emissaries";
        }
    }

    public class Contributor : Employee
    {
        [DisplayName("Contributor")]
        public override long ID { get; set; }

        [DisplayName("Contributor Name")]
        public override String Name { get; set; }
    }

    public class CurrentUser : Contributor
    {
        [DisplayName("Current User")]
        public override long ID { get; set; }

        [DisplayName("Current User Name")]
        public override String Name { get; set; }

        public bool IsAdmin()
        {
            return Admin??false;
        }

        public bool IsAmbassador()
        {
            return (Ambassador??false) || (Admin??false);
        }

        public String premissionString()
        {
            return Admin??false ? "Admin" : Ambassador??false ? "Ambassador" : "Contributor";
        }

        //Project Permissions
        public bool CanViewProjectList()
        {
            return true;
        }

        public bool CanCreateProject()
        {
            return true;
        }

        public bool CanViewProjectDetails(ProjectOverviewViewModel project)
        {
            return IsAdmin() || (project != null && !project.IsArchived());
        }

        public bool CanEditProject(ProjectOverviewViewModel project)
        {
            return CanEditProjectIdeaInternal(project) || CanEditProjectStatusInternal(project) || CanEditProjectAssignment();
        }

        public bool CanEditProjectIdea(ProjectOverviewViewModel project)
        {
            return IsAdmin() || CanEditProjectIdeaInternal(project);
        }

        private bool CanEditProjectIdeaInternal(ProjectOverviewViewModel project)
        {
            return project != null && ( project.ID.Equals(0L) || (project.IsSubmitted() && project.Contributor.Domain_ID.Equals(Domain_ID)) );
        }

        public bool CanEditProjectStatus(ProjectOverviewViewModel project)
        {
            return IsAdmin() || CanEditProjectStatusInternal(project);
        }

        private bool CanEditProjectStatusInternal(ProjectOverviewViewModel project)
        {
            return project != null && (project.IsSubmitted() || project.IsAssigned() || project.IsInProgress()) && project.Emissaries != null && project.Emissaries.Any(e => e.Domain_ID.Equals(Domain_ID));
        }

        public bool CanEditProjectAssignment()
        {
            return IsAdmin();
        }

        public bool CanDeleteProject()
        {
            return IsAdmin();
        }

        //Emissary Permissions
        public bool CanViewEmissaryList()
        {
            return IsAmbassador();
        }

        public bool CanCreateEmissary()
        {
            // Contributors should not be able to create Emissaries, for testing only
            return IsAdmin() || !IsAmbassador();
        }

        public bool CanViewEmissaryDetails(Employee emissary)
        {
            return IsAdmin() || (emissary != null && emissary.Domain_ID.Equals(Domain_ID));
        }

        public bool CanEditEmissary(Employee emissary)
        {
            // Emissaries should not be able to edit themselves, for testing only
            return IsAdmin() || (emissary != null && emissary.Domain_ID.Equals(Domain_ID));
        }

        public bool CanDeleteEmissary()
        {
            return IsAdmin();
        }

        //School Permissions
        public bool CanViewSchoolList()
        {
            return IsAmbassador();
        }

        public bool CanCreateSchool()
        {
            return IsAdmin();
        }

        public bool CanViewSchoolDetails()
        {
            return IsAmbassador();
        }

        public bool CanEditSchool(SchoolOverviewViewModel school)
        {
            return CanEditSchoolContactInfo(school);
        }

        public bool CanEditSchoolContactInfo(SchoolOverviewViewModel school)
        {
            return IsAdmin() || (school != null && IsAmbassador() && school.Emissaries.Any(e => e.Domain_ID.Equals(Domain_ID)));
        }

        public bool CanEditSchoolAssignment()
        {
            return IsAdmin();
        }

        public bool CanDeleteSchool()
        {
            return IsAdmin();
        }
    }

    public class EmissaryOverviewViewModel : CurrentUser
    {
        [DisplayName("Emissary")]
        public override long ID { get; set; }

        [Required]
        [DisplayName("Emissary Name")]
        public override String Name { get; set; }
    }

    public class EmissaryOverviewList : UserViewModelList<EmissaryOverviewViewModel>
    {
        public override bool CanViewList()
        {
            return CurrentUser.CanViewEmissaryList();
        }

        public override bool CanCreate()
        {
            return CurrentUser.CanCreateEmissary();
        }
    }

    public class EmissaryDetailsViewModel : EmissaryOverviewViewModel, UserViewModel
    {
        [DisplayName("Schools")]
        public IEnumerable<SchoolOverviewViewModel> Schools { get; set; }

        [DisplayName("Projects")]
        public IEnumerable<ProjectOverviewViewModel> Projects { get; set; }
    }

    public class EmissaryEditViewModel : EmissaryDetailsViewModel
    {
        [DisplayName("Other Schools")]
        public IEnumerable<SchoolOverviewViewModel> OtherSchools { get; set; }

        [DisplayName("Other Projects")]
        public IEnumerable<ProjectOverviewViewModel> OtherProjects { get; set; }
    }
}