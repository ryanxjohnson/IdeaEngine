using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdeaEngineAuth.Data;
using System.Data.Entity;

namespace IdeaEngineAuth.Models
{
    public class ProjectViewModelBuilder : BaseEntityViewModelBuilder<ProjectOverviewViewModel, ProjectOverviewList, ProjectDetailsViewModel, ProjectEditViewModel, CurrentProjectData>, ProjectDomainModel
    {
        public override ProjectOverviewList buildOverviewViewModelList(CurrentUser currentUser)
        {
            ProjectOverviewList list = new ProjectOverviewList();
            list.CurrentUser = currentUser;
            foreach (CurrentProjectData model in db.CurrentProjectDatas)
            {
                list.Add(buildOverviewInternal(currentUser, model));
            }
            return list;
        }

        protected override CurrentProjectData buildOverviewInternal(CurrentUser currentUser, CurrentProjectData model)
        {
            model.CurrentUser = currentUser;
            model.School = db.CurrentSchoolDatas.Find(model.School_ID);
            if ( model.School == null )
            {
                model.School = nullSchool;
            }
            model.Contributor = getEmployeePermissions(model.Contributor_ID);
            model.Emissaries = buildList(db.CurrentProjectEmissaries.Where(x => x.Project_ID.Equals(model.ID)), x => db.CurrentEmissaryDatas.Find(x.Emissary_ID));
            return model;
        }

        private static readonly CurrentSchoolData nullSchool = new CurrentSchoolData()
        {
            ID = 0,
            Name = "None Assigned"
        };

        protected override CurrentProjectData buildDetailsInternal(CurrentUser currentUser, long id)
        {
            CurrentProjectData model = buildOverviewInternal(currentUser, db.CurrentProjectDatas.Find(id));
            model.Files = buildList(db.CurrentProjectFiles.Where(x => x.Project_ID.Equals(id)), x => db.CurrentFileDatas.Find(x.File_ID));
            model.Notes = buildList(db.CurrentProjectNotes.Where(x => x.Project_ID.Equals(id)), x =>
            {
                x.Author = getEmployeePermissions(x.Employee_ID);
                return x;
            });
            return model;
        }

        public override ProjectEditViewModel buildEditViewModel(CurrentUser currentUser, long id)
        {
            ProjectEditViewModel model = buildDetailsInternal(currentUser, id);
            if ( !model.School.ID.Equals(0L) )
            {
                List<CurrentSchoolData> list = db.CurrentSchoolDatas.ToList();
                list.Add(nullSchool);
                model.OtherSchools = list.Except(makeListFromItem(model.School));
            }
            else
            {
                model.OtherSchools = db.CurrentSchoolDatas.ToList();
            }
            model.OtherEmissaries = db.CurrentEmissaryDatas.ToList().Except(model.Emissaries);
            return model;
        }

        public override ProjectEditViewModel buildCreateViewModel(CurrentUser currentUser)
        {
            return new ProjectEditViewModel()
            {
                CurrentUser = currentUser,
                School = nullSchool,
                OtherSchools = db.CurrentSchoolDatas.ToList(),
                OtherEmissaries = db.CurrentEmissaryDatas.ToList(),
                Notes = new List<NoteData>()
            };
        }

        public override bool saveModelData(ProjectEditViewModel model)
        {
            CurrentProjectData old = model.ID.Equals(0L) ? null : buildDetailsInternal(model.CurrentUser, model.ID);

            if ( old == null &&
            (
            String.IsNullOrWhiteSpace(model.Title = validateLength(model.Title, 100)) ||
            String.IsNullOrWhiteSpace(model.Summary = validateLength(model.Summary, 500)) ||
            String.IsNullOrWhiteSpace(model.Description) ||
            String.IsNullOrWhiteSpace(model.Status = validateStatus(model.Status, model, null))
            ))
            {
                throw new InvalidOperationException("Create does not include all required fields");
            }

            if ( ( old == null && !model.CurrentUser.CanCreateProject() ) || ( old != null && !old.CanEdit() ) )
            {
                throw new UnauthorizedAccessException("User is not Authorized to edit this Project");
            }

            Revision revision = getRevision(model);

            ProjectRevision projectRevision = getRevisionOf(model, revision, db.Projects, db.ProjectRevisions);

            if ( old == null || model.CurrentUser.CanEditProjectIdea(old) )
            {
                saveUpdate(model.Title, old == null ? null : old.Title, y => validateLength(y, 100),
                    z => db.ProjectTitles.Add(new ProjectTitle()
                    {
                        ProjectRevision = projectRevision,
                        Title = z
                    }));

                saveUpdate(model.Summary, old == null ? null : old.Summary, y => validateLength(y, 500),
                    z => db.ProjectSummaries.Add(new ProjectSummary()
                    {
                        ProjectRevision = projectRevision,
                        Summary = z
                    }));

                saveUpdate(model.Description, old == null ? null : old.Description, y => y,
                    z => db.ProjectDescriptions.Add(new ProjectDescription()
                    {
                        ProjectRevision = projectRevision,
                        Description = z
                    }));

                processIncludes(model.Files = addFiles(model), old == null ? null : old.Files,
                    (item, included) => db.ProjectFiles.Add(new ProjectFile()
                    {
                        ProjectRevision = projectRevision,
                        File = item as File,
                        File_ID = item.ID,
                        Included = included
                    }));
            }

            if ( old == null || model.CurrentUser.CanEditProjectStatus(old) )
            {
                saveUpdate(model.Status, old == null ? null : old.Status, y => validateStatus(y, model, old),
                   z => db.ProjectStatus.Add(new ProjectStatu()
                   {
                       ProjectRevision = projectRevision,
                       Status = z
                   }));
            }
            
            if ( model.CurrentUser.CanEditProjectStatus(old) )
            {
                if (!String.IsNullOrWhiteSpace(model.NewNote))
                {
                    db.RevisionNotes.Add(new RevisionNote()
                    {
                        Revision = revision,
                        Note = model.NewNote
                    });
                }
            }

            if ( model.CurrentUser.CanEditProjectAssignment() )
            {
                processIncludes(makeListFromItem(model.School), old == null ? null : makeListFromItem(old.School),
                (item, included) => item.ID.Equals(0L) ? null : db.SchoolProjects.Add(new SchoolProject()
                {
                    School_ID = item.ID,
                    Project_ID = model.ID,
                    Revision = revision,
                    Included = included
                }));

                processIncludes(model.Emissaries, old == null ? null : old.Emissaries,
                    (item, included) => item.ID.Equals(0L) ? null : db.EmissaryProjects.Add(new EmissaryProject()
                    {
                        Emissary_ID = item.ID,
                        Project_ID = model.ID,
                        Revision = revision,
                        Included = included
                    }));
            }

            int changes = db.SaveChanges();

            if ( model.CurrentUser.CanEditProjectIdea(old) )
            {
                foreach (FileLink file in model.Files)
                {
                    file.saveFile();
                }
            }

            return changes > 0;
        }

        private IEnumerable<FileLink> addFiles(ProjectEditViewModel model)
        {
            List<FileLink> fileList = model.Files != null ? model.Files.ToList() : new List<FileLink>();

            if (model.UploadedFiles != null)
            {

                long file_id = 1;
                try
                {
                    file_id = db.Files.Max(x => x.File_ID) + 1;
                }
                catch (InvalidOperationException) { }

                foreach (HttpPostedFileBase file in model.UploadedFiles)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        String fileName = System.IO.Path.GetFileName(file.FileName);
                        String path = file_id + "_" + fileName;
                        file_id++;

                        try
                        {
                            fileList.Add(db.Files.Single(x => x.FileName.Equals(fileName) && x.FilePath.Equals(path)));
                        }
                        catch (InvalidOperationException)
                        {
                            fileList.Add(db.Files.Add(new File()
                            {
                                RawFile = file,
                                FileName = fileName,
                                FilePath = path
                            }));
                        }
                    }
                }
            }

            return fileList;
        }

        private String validateStatus( String status, ProjectEditViewModel model, ProjectEditViewModel old )
        {
            String trimmedStatus = (status ?? "").Replace("-", "").Replace(" ", "").Trim().ToLowerInvariant();

            if ( model.CurrentUser.IsAdmin() )
            {
                return model.getStatusList().Any(x => trimmedStatus.Equals(x.Trimmed)) ? status : null;
            }
            else if ( old == null )
            {
                return trimmedStatus.Equals("submitted") ? status : null;
            }
            else
            {
                return old.getStatusList().Any(x => trimmedStatus.Equals(x.Trimmed)) ? status : null;
            }
        }

        private String validateLength( String str, int length )
        {
            return str.Length <= length ? str : null;
        }

        public override bool deleteModelData(CurrentUser currentUser, long id)
        {
            if ( !buildDetailsInternal(currentUser, id).CanEdit() )
            {
                throw new UnauthorizedAccessException("User is not Authorized to delete this Project");
            }

            db.Projects.Remove(db.Projects.Find(id));
            return db.SaveChanges() > 0;
        }
    }
}