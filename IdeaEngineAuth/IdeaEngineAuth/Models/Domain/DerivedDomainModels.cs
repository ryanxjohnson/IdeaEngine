using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaEngineAuth.Models
{
    public interface ProjectDomainModel : BaseEntityDomainModel<ProjectOverviewViewModel, ProjectOverviewList, ProjectDetailsViewModel, ProjectEditViewModel> { }

    public interface EmissaryDomainModel : BaseEntityDomainModel<EmissaryOverviewViewModel, EmissaryOverviewList, EmissaryDetailsViewModel, EmissaryEditViewModel> { }

    public interface SchoolDomainModel : BaseEntityDomainModel<SchoolOverviewViewModel, SchoolOverviewList, SchoolDetailsViewModel, SchoolEditViewModel> { }
}