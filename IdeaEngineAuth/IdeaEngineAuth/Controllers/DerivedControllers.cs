using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdeaEngineAuth.Models;
using System.Web.Mvc;

namespace IdeaEngineAuth.Controllers
{
    public class ProjectController : BaseEntityController<ProjectViewModelBuilder, ProjectOverviewViewModel, ProjectOverviewList, ProjectDetailsViewModel, ProjectEditViewModel> { }

    public class EmissaryController : BaseEntityController<EmissaryViewModelBuilder, EmissaryOverviewViewModel, EmissaryOverviewList, EmissaryDetailsViewModel, EmissaryEditViewModel> { }

    public class SchoolController : BaseEntityController<SchoolViewModelBuilder, SchoolOverviewViewModel, SchoolOverviewList, SchoolDetailsViewModel, SchoolEditViewModel> { }
}