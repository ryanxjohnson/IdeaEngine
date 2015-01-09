using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using IdeaEngineAuth.Models;

namespace IdeaEngineAuth.Data
{
    [MetadataType(typeof(ProjectEditViewModel))]
    public partial class CurrentProjectData : ProjectEditViewModel { }

    public partial class ProjectRevision : RevisionOf<Project, ProjectRevision>
    {
        public ProjectRevision build(Revision revision, Project project)
        {
            Revision = revision;
            Project = project;
            return this;
        }
    }
}