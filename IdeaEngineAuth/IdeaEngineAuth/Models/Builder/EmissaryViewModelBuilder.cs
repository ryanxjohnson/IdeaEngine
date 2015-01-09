using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdeaEngineAuth.Data;

namespace IdeaEngineAuth.Models
{
    public class EmissaryViewModelBuilder : BaseEntityViewModelBuilder<EmissaryOverviewViewModel, EmissaryOverviewList, EmissaryDetailsViewModel, EmissaryEditViewModel, CurrentEmissaryData>, EmissaryDomainModel
    {
        public override EmissaryOverviewList buildOverviewViewModelList(CurrentUser currentUser)
        {
            EmissaryOverviewList list = new EmissaryOverviewList();
            list.CurrentUser = currentUser;
            foreach ( CurrentEmissaryData model in db.CurrentEmissaryDatas )
            {
                list.Add(buildOverviewInternal(currentUser, model));
            }
            return list;
        }

        protected override CurrentEmissaryData buildOverviewInternal(CurrentUser currentUser, CurrentEmissaryData model)
        {
            model.CurrentUser = currentUser;
            return model;
        }

        protected override CurrentEmissaryData buildDetailsInternal(CurrentUser currentUser, long id)
        {
            CurrentEmissaryData model = buildOverviewInternal(currentUser, db.CurrentEmissaryDatas.Find(id));
            model.Projects = buildList(db.CurrentProjectEmissaries.Where(x => x.Emissary_ID.Equals(id)), x => db.CurrentProjectDatas.Find(x.Project_ID));
            model.Schools = buildList(db.CurrentEmissarySchools.Where(x => x.Emissary_ID.Equals(id)), x => db.CurrentSchoolDatas.Find(x.School_ID));
            return model;
        }

        public override EmissaryEditViewModel buildEditViewModel(CurrentUser currentUser, long id)
        {
            EmissaryEditViewModel model = buildDetailsInternal(currentUser, id);
            model.OtherProjects = db.CurrentProjectDatas.ToList().Except(model.Projects);
            model.OtherSchools = db.CurrentSchoolDatas.ToList().Except(model.Schools);
            return model;
        }

        public override EmissaryEditViewModel buildCreateViewModel(CurrentUser currentUser)
        {
            return new EmissaryEditViewModel()
            {
                CurrentUser = currentUser,
                OtherProjects = db.CurrentProjectDatas.ToList(),
                OtherSchools = db.CurrentSchoolDatas.ToList()
            };
        }

        public override bool saveModelData(EmissaryEditViewModel model)
        {
            if (model.ID.Equals(0) &&
            (
            String.IsNullOrWhiteSpace(model.Domain_ID) ||
            String.IsNullOrWhiteSpace(model.Name)
            ))
            {
                throw new InvalidOperationException("Create does not include all required fields");
            }

            CurrentEmissaryData old = model.ID == 0 ? null : buildDetailsInternal(model.CurrentUser, model.ID);

            if ( ( old == null && !model.CurrentUser.CanCreateEmissary() ) || ( old != null && !old.CanEdit() ) )
            {
                throw new UnauthorizedAccessException("User is not Authorized to edit this Emissary");
            }

            Revision revision = getRevision(model);

            EmissaryRevision emissaryRevision = getRevisionOf(model, revision, db.Emissaries, db.EmissaryRevisions);

            saveUpdate(model.Name, old == null ? null : old.Name, y => y,
                z => db.EmissaryNames.Add(new EmissaryName()
                {
                    EmissaryRevision = emissaryRevision,
                    Name = z
                }));

            saveUpdate(model.Admin??false, old == null ? false : old.Admin??false, y => y,
                z => db.EmissaryAdmins.Add(new EmissaryAdmin()
                {
                    EmissaryRevision = emissaryRevision,
                    Admin = z
                }));

            saveUpdate(model.Ambassador??false, old == null ? false : old.Ambassador??false, y => y,
                z => db.EmissaryAmbassadors.Add(new EmissaryAmbassador()
                {
                    EmissaryRevision = emissaryRevision,
                    Ambassador = z
                }));

            saveUpdate(model.Domain_ID, old == null ? null : old.Domain_ID, y => y,
                z => db.EmissaryDomainIDs.Add(new EmissaryDomainID()
                {
                    EmissaryRevision = emissaryRevision,
                    Domain_ID = z
                }));

            processIncludes(model.Schools, old == null ? null : old.Schools,
                (item, included) => item.ID.Equals(0L) ? null : db.EmissarySchools.Add(new EmissarySchool()
                {
                    School_ID = item.ID,
                    Emissary_ID = model.ID,
                    Revision = revision,
                    Included = included
                }));

            processIncludes(model.Projects, old == null ? null : old.Projects,
                (item, included) => item.ID.Equals(0L) ? null : db.EmissaryProjects.Add(new EmissaryProject()
                {
                    Project_ID = item.ID,
                    Emissary_ID = model.ID,
                    Revision = revision,
                    Included = included
                }));

            return db.SaveChanges() > 0;
        }

        public override bool deleteModelData(CurrentUser currentUser, long id)
        {
            if (!buildDetailsInternal(currentUser, id).CanEdit())
            {
                throw new UnauthorizedAccessException("User is not Authorized to delete this Emissary");
            }

            db.Emissaries.Remove(db.Emissaries.Find(id));
            return db.SaveChanges() > 0;
        }
    }
}