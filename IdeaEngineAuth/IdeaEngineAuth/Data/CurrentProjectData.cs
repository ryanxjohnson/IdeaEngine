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
    
    public partial class CurrentProjectData
    {
        public override long ID { get; set; }
        public override string Title { get; set; }
        public override string Summary { get; set; }
        public override string Description { get; set; }
        public override string Status { get; set; }
        public override Nullable<System.DateTime> DateRevised { get; set; }
        public string Revisor_ID { get; set; }
        public string Contributor_ID { get; set; }
        public override Nullable<System.DateTime> DateSubmitted { get; set; }
        public override Nullable<System.DateTime> DateAssigned { get; set; }
        public Nullable<long> School_ID { get; set; }
    }
}
