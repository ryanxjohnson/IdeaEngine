//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IdeaEngineAuth.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Emissary
    {
        public Emissary()
        {
            this.EmissaryRevisions = new HashSet<EmissaryRevision>();
            this.EmissarySchools = new HashSet<EmissarySchool>();
            this.EmissaryProjects = new HashSet<EmissaryProject>();
        }
    
        public long Emissary_ID { get; set; }
    
        public virtual ICollection<EmissaryRevision> EmissaryRevisions { get; set; }
        public virtual ICollection<EmissarySchool> EmissarySchools { get; set; }
        public virtual ICollection<EmissaryProject> EmissaryProjects { get; set; }
    }
}