using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using IdeaEngineAuth.Models;

namespace IdeaEngineAuth.Data
{
    [MetadataType(typeof(SchoolEditViewModel))]
    public partial class CurrentSchoolData : SchoolEditViewModel { }

    public partial class SchoolRevision : RevisionOf<School, SchoolRevision>
    {
        public SchoolRevision build(Revision revision, School school)
        {
            Revision = revision;
            School = school;
            return this;
        }
    }
}