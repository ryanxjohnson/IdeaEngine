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
    
    public partial class File
    {
        public File()
        {
            this.ProjectFiles = new HashSet<ProjectFile>();
        }
    
        public long File_ID { get; set; }
        public override string FileName { get; set; }
        public override string FilePath { get; set; }
    
        public virtual ICollection<ProjectFile> ProjectFiles { get; set; }
    }
}