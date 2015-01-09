using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using IdeaEngineAuth.Models;

namespace IdeaEngineAuth.Data
{
    [MetadataType(typeof(EmissaryEditViewModel))]
    public partial class CurrentEmissaryData : EmissaryEditViewModel { }

    public partial class EmissaryRevision : RevisionOf<Emissary, EmissaryRevision>
    {
        public EmissaryRevision build(Revision revision, Emissary emmisary)
        {
            Revision = revision;
            Emissary = emmisary;
            return this;
        }
    }
}