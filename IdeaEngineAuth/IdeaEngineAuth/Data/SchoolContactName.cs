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
    
    public partial class SchoolContactName
    {
        public long SchoolRevision_ID { get; set; }
        public string ContactName { get; set; }
    
        public virtual SchoolRevision SchoolRevision { get; set; }
    }
}
