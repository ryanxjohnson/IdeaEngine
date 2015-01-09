﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Emissary> Emissaries { get; set; }
        public virtual DbSet<EmissaryDomainID> EmissaryDomainIDs { get; set; }
        public virtual DbSet<EmissaryName> EmissaryNames { get; set; }
        public virtual DbSet<EmissaryRevision> EmissaryRevisions { get; set; }
        public virtual DbSet<EmissarySchool> EmissarySchools { get; set; }
        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectDescription> ProjectDescriptions { get; set; }
        public virtual DbSet<ProjectFile> ProjectFiles { get; set; }
        public virtual DbSet<ProjectRevision> ProjectRevisions { get; set; }
        public virtual DbSet<ProjectStatu> ProjectStatus { get; set; }
        public virtual DbSet<ProjectSummary> ProjectSummaries { get; set; }
        public virtual DbSet<ProjectTitle> ProjectTitles { get; set; }
        public virtual DbSet<Revision> Revisions { get; set; }
        public virtual DbSet<RevisionNote> RevisionNotes { get; set; }
        public virtual DbSet<School> Schools { get; set; }
        public virtual DbSet<SchoolContactEmail> SchoolContactEmails { get; set; }
        public virtual DbSet<SchoolContactName> SchoolContactNames { get; set; }
        public virtual DbSet<SchoolContactPhoneNumber> SchoolContactPhoneNumbers { get; set; }
        public virtual DbSet<SchoolName> SchoolNames { get; set; }
        public virtual DbSet<SchoolRevision> SchoolRevisions { get; set; }
        public virtual DbSet<CurrentEmissarySchool> CurrentEmissarySchools { get; set; }
        public virtual DbSet<CurrentProjectEmissary> CurrentProjectEmissaries { get; set; }
        public virtual DbSet<CurrentProjectNote> CurrentProjectNotes { get; set; }
        public virtual DbSet<CurrentSchoolProject> CurrentSchoolProjects { get; set; }
        public virtual DbSet<CurrentProjectFile> CurrentProjectFiles { get; set; }
        public virtual DbSet<EmissaryAdmin> EmissaryAdmins { get; set; }
        public virtual DbSet<EmissaryAmbassador> EmissaryAmbassadors { get; set; }
        public virtual DbSet<CurrentEmissaryData> CurrentEmissaryDatas { get; set; }
        public virtual DbSet<CurrentFileData> CurrentFileDatas { get; set; }
        public virtual DbSet<CurrentProjectData> CurrentProjectDatas { get; set; }
        public virtual DbSet<CurrentSchoolData> CurrentSchoolDatas { get; set; }
        public virtual DbSet<EmissaryProject> EmissaryProjects { get; set; }
        public virtual DbSet<SchoolProject> SchoolProjects { get; set; }
        public virtual DbSet<CurrentFileStatu> CurrentFileStatus { get; set; }
    }
}
