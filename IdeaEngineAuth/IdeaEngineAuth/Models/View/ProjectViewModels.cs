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
    public class ProjectOverviewViewModel : HasPermissions
    {
        [DisplayName("Project")]
        public override long ID { get; set; }

        [Required]
        [DisplayName("Project Title")]
        [StringLength(100, ErrorMessage="The title cannot be more than 100 characters")]
        public virtual String Title { get; set; }

        [Required]
        [DisplayName("Summary")]
        [StringLength(500, ErrorMessage = "The summary cannot be more than 500 characters")]
        public virtual String Summary { get; set; }

        [Required]
        [DisplayName("Description")]
        public virtual String Description { get; set; }

        [Required]
        [DisplayName("Status")]
        public virtual String Status { get; set; }

        [DisplayName("School")]
        public SchoolOverviewViewModel School { get; set; }

        [DisplayName("Date of last revision")]
        public virtual Nullable<DateTime> DateRevised { get; set; }

        [DisplayName("Contributor")]
        public Contributor Contributor { get; set; }

        [DisplayName("Emissaries")]
        public IEnumerable<EmissaryOverviewViewModel> Emissaries { get; set; }

        public override MvcHtmlString getName()
        {
            return MvcHtmlString.Create(Title);
        }

        public bool IsSubmitted()
        {
            return checkStatus("submitted");
        }

        public bool IsAssigned()
        {
            return checkStatus("assigned");
        }

        public bool IsInProgress()
        {
            return checkStatus("inprogress");
        }

        public bool IsFinalizing()
        {
            return checkStatus("finalizing");
        }

        public bool IsDeployed()
        {
            return checkStatus("deployed");
        }

        public bool IsArchived()
        {
            return checkStatus("archived");
        }

        public class SelectListItemWithTrimmed : SelectListItem
        {
            public String Trimmed { get; set; }
        }

        public IEnumerable<SelectListItemWithTrimmed> getStatusList()
        {
            if ( CurrentUser.IsAdmin() )
            {
                return new List<SelectListItemWithTrimmed>
                {
                    new SelectListItemWithTrimmed() {Text = "Submitted", Value = "Submitted", Selected = IsSubmitted(), Trimmed = "submitted"},
                    new SelectListItemWithTrimmed() {Text = "Assigned", Value = "Assigned", Selected = IsAssigned(), Trimmed = "assigned"},
                    new SelectListItemWithTrimmed() {Text = "In Progress", Value = "In Progress", Selected = IsInProgress(), Trimmed = "inprogress"},
                    new SelectListItemWithTrimmed() {Text = "Finalizing", Value = "Finalizing", Selected = IsFinalizing(), Trimmed = "finalizing"},
                    new SelectListItemWithTrimmed() {Text = "Deployed", Value = "Deployed", Selected = IsDeployed(), Trimmed = "deployed"},
                    new SelectListItemWithTrimmed() {Text = "Archived", Value = "Archived", Selected = IsArchived(), Trimmed = "archived"},
                };
            }
            else if ( Status == null )
            {
                return new List<SelectListItemWithTrimmed>
                {
                    new SelectListItemWithTrimmed() {Text = "Submitted", Value = "Submitted", Selected = true, Trimmed = "submitted"},
                };
            }
            else if( CurrentUser.CanEditProjectStatus(this) )
            {
                if ( IsAssigned() )
                {
                    return new List<SelectListItemWithTrimmed>
                    {
                        new SelectListItemWithTrimmed() {Text = "Assigned", Value = "Assigned", Selected = true, Trimmed = "assigned"},
                        new SelectListItemWithTrimmed() {Text = "In Progress", Value = "In Progress", Selected = false, Trimmed = "inprogress"},
                    };
                }
                else if ( IsInProgress() )
                {
                    return new List<SelectListItemWithTrimmed>
                    {
                        new SelectListItemWithTrimmed() {Text = "In Progress", Value = "In Progress", Selected = true, Trimmed = "inprogress"},
                        new SelectListItemWithTrimmed() {Text = "Finalizing", Value = "Finalizing", Selected = false, Trimmed = "finalizing"},
                    };
                }
            }

            return new List<SelectListItemWithTrimmed>
            {
                new SelectListItemWithTrimmed() {Text = Status, Value = Status, Selected = true, Trimmed = (Status ?? "").Replace("-", "").Replace(" ", "").Trim().ToLowerInvariant()},
            };
        }

        private bool checkStatus(String status)
        {
            return Status != null && Status.Replace("-", "").Replace(" ", "").ToLowerInvariant().Equals(status);
        }

        [DisplayName("Current User")]
        public CurrentUser CurrentUser { get; set; }

        public override CurrentUser getCurrentUser()
        {
            return CurrentUser;
        }

        public override bool CanViewList()
        {
            return CurrentUser.CanViewProjectList();
        }

        public override bool CanCreate()
        {
            return CurrentUser.CanCreateProject();
        }

        public override bool CanViewDetails()
        {
            return CurrentUser.CanViewProjectDetails(this);
        }

        public override bool CanEdit()
        {
            return CurrentUser.CanEditProject(this);
        }

        public override bool CanDelete()
        {
            return CurrentUser.CanDeleteProject();
        }
    }

    public class ProjectTuple : M_N_Tuple
    {
        public ProjectTuple(IEnumerable<ProjectOverviewViewModel> things, IEnumerable<ProjectOverviewViewModel> otherThings)
            : base(things, otherThings) { }

        public ProjectTuple(IEnumerable<ProjectOverviewViewModel> things) : base(things, new List<HasID>()) { }

        public override string getLable()
        {
            return "Projects Assigned";
        }

        public override string getTitle()
        {
            return "Project Title";
        }

        public override string getSetName()
        {
            return "Projects";
        }
    }

    public class ProjectOverviewList : UserViewModelList<ProjectOverviewViewModel>
    {
        public override bool CanViewList()
        {
            return CurrentUser.CanViewProjectList();
        }

        public override bool CanCreate()
        {
            return CurrentUser.CanCreateProject();
        }
    }

    public class ProjectDetailsViewModel : ProjectOverviewViewModel, UserViewModel
    {
        [DisplayName("Project Files")]
        public IEnumerable<FileLink> Files { get; set; }

        [DisplayName("Date Project was submitted")]
        public virtual Nullable<DateTime> DateSubmitted { get; set; }

        [DisplayName("Date Project was assigned")]
        public virtual Nullable<DateTime> DateAssigned { get; set; }

        [DisplayName("Notes")]
        public IEnumerable<NoteData> Notes { get; set; }
    }

    public class ProjectEditViewModel : ProjectDetailsViewModel
    {
        [DisplayName("Other Emissaries")]
        public IEnumerable<EmissaryOverviewViewModel> OtherEmissaries { get; set; }

        [DisplayName("Other Schools")]
        public IEnumerable<SchoolOverviewViewModel> OtherSchools { get; set; }

        [DisplayName("Uploaded Files")]
        public IEnumerable<HttpPostedFileBase> UploadedFiles { get; set; }

        [DisplayName("New Note")]
        public String  NewNote { get; set; }
    }

    public class NoteData
    {
        public virtual String Note { get; set; }
        public CurrentUser Author { get; set; }
        public virtual DateTime Date { get; set; }
    }
}