using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdeaEngineAuth.Data;
using System.Text.RegularExpressions;

namespace IdeaEngineAuth.Models
{
    public class SchoolViewModelBuilder : BaseEntityViewModelBuilder<SchoolOverviewViewModel, SchoolOverviewList, SchoolDetailsViewModel, SchoolEditViewModel, CurrentSchoolData>, SchoolDomainModel
    {
        public override SchoolOverviewList buildOverviewViewModelList(CurrentUser currentUser)
        {
            SchoolOverviewList list = new SchoolOverviewList();
            list.CurrentUser = currentUser;
            foreach (CurrentSchoolData model in db.CurrentSchoolDatas)
            {
                list.Add(buildOverviewInternal(currentUser, model));
            }
            return list;
        }

        protected override CurrentSchoolData buildOverviewInternal(CurrentUser currentUser, CurrentSchoolData model)
        {
            model.CurrentUser = currentUser;
            if (model.ContactPhoneNumber != null)
            {
                long number = (long)model.ContactPhoneNumber;
                model.ContactPhone = "(" + (number / 10000000L).ToString() + ") "
                    + (number / 10000L % 1000L).ToString() + "-"
                    + (number % 10000L).ToString();
            }
            else
            {
                model.ContactPhone = "";
            }
            model.Emissaries = buildList(db.CurrentEmissarySchools.Where(x => x.School_ID.Equals(model.ID)), x => db.CurrentEmissaryDatas.Find(x.Emissary_ID));
            return model;
        }

        protected override CurrentSchoolData buildDetailsInternal(CurrentUser currentUser, long id)
        {
            CurrentSchoolData model = buildOverviewInternal(currentUser, db.CurrentSchoolDatas.Find(id));
            model.Projects = buildList(db.CurrentSchoolProjects.Where(x => x.School_ID.Equals(id)), x => db.CurrentProjectDatas.Find(x.Project_ID));
            return model;
        }

        public override SchoolEditViewModel buildEditViewModel(CurrentUser currentUser, long id)
        {
            SchoolEditViewModel model = buildDetailsInternal(currentUser, id);
            model.OtherProjects = db.CurrentProjectDatas.ToList().Except(model.Projects);
            model.OtherEmissaries = db.CurrentEmissaryDatas.ToList().Except(model.Emissaries);
            return model;
        }

        public override SchoolEditViewModel buildCreateViewModel(CurrentUser currentUser)
        {
            return new SchoolEditViewModel()
            {
                CurrentUser = currentUser,
                OtherEmissaries = db.CurrentEmissaryDatas.ToList(),
                OtherProjects = db.CurrentProjectDatas.ToList()
            };
        }

        public override bool saveModelData(SchoolEditViewModel model)
        {
            if ( model.ID.Equals(0) &&
            (
            String.IsNullOrWhiteSpace(model.Name)
            ))
            {
                throw new InvalidOperationException("Create does not include all required fields");
            }

            CurrentSchoolData old = model.ID == 0 ? null : buildDetailsInternal(model.CurrentUser, model.ID);

            if ( ( old == null && !model.CurrentUser.CanCreateSchool() ) || ( old != null && !old.CanEdit() ) )
            {
                throw new UnauthorizedAccessException("User is not Authorized to edit this School");
            }

            Revision revision = getRevision(model);

            SchoolRevision schoolRevision = getRevisionOf(model, revision, db.Schools, db.SchoolRevisions);

            if ( model.CurrentUser.CanEditSchoolContactInfo(old) )
            {
                saveUpdate(model.Name, old == null ? null : old.Name, y => y,
                    z => db.SchoolNames.Add(new SchoolName()
                    {
                        SchoolRevision = schoolRevision,
                        Name = z
                    }));

                saveUpdate(model.ContactName, old == null ? null : old.ContactName, y => y,
                    z => db.SchoolContactNames.Add(new SchoolContactName()
                    {
                        SchoolRevision = schoolRevision,
                        ContactName = z
                    }));

                saveUpdate(model.ContactEmail, old == null ? null : old.ContactEmail, y => validateEmail(y),
                    z => db.SchoolContactEmails.Add(new SchoolContactEmail()
                    {
                        SchoolRevision = schoolRevision,
                        ContactEmail = z
                    }));

                saveUpdate(model.ContactPhone, old == null ? null : old.ContactPhoneNumber, y => DigitUtils.parseOrNull(y),
                    z => db.SchoolContactPhoneNumbers.Add(new SchoolContactPhoneNumber()
                    {
                        SchoolRevision = schoolRevision,
                        ContactPhoneNumber = (long)z
                    }));
            }

            if ( model.CurrentUser.CanEditSchoolAssignment() )
            {
                processIncludes(model.Emissaries, old == null ? null : old.Emissaries,
                    (item, included) => item.ID.Equals(0L) ? null : db.EmissarySchools.Add(new EmissarySchool()
                    {
                        Emissary_ID = item.ID,
                        School_ID = model.ID,
                        Revision = revision,
                        Included = included
                    }));

                processIncludes(model.Projects, old == null ? null : old.Projects,
                    (item, included) =>
                    {
                        if (!item.ID.Equals(0L))
                        {
                            removeOldSchool(item, revision, included);
                            db.SchoolProjects.Add(new SchoolProject()
                            {
                                Project_ID = item.ID,
                                School_ID = model.ID,
                                Revision = revision,
                                Included = included
                            });
                        }
                        return true;
                    });
            }

            return db.SaveChanges() > 0;
        }

        private void removeOldSchool(ProjectOverviewViewModel project, Revision revision, bool included)
        {
            if (included)
            {
                try
                {
                    db.SchoolProjects.Add(new SchoolProject()
                    {
                        Project_ID = project.ID,
                        School_ID = db.CurrentSchoolProjects.Single(x => x.Project_ID.Equals(project.ID)).School_ID,
                        Revision = revision,
                        Included = false
                    });
                }
                catch (InvalidOperationException) { }
            }
        }

        private static readonly Regex regex = new Regex(@"\A[\w\.]+@\w+(\.\w+)+\Z", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private String validateEmail(String email)
        {
            return ( regex.IsMatch(email) ) ? email : null;
        }

        public override bool deleteModelData(CurrentUser currentUser, long id)
        {
            if (!buildDetailsInternal(currentUser, id).CanEdit())
            {
                throw new UnauthorizedAccessException("User is not Authorized to delete this School");
            }

            db.Schools.Remove(db.Schools.Find(id));
            return db.SaveChanges() > 0;
        }
    }
}