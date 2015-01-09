using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdeaEngineAuth.Data;

namespace IdeaEngineAuth.Models
{
    public abstract class BaseViewModelBuilder : BaseDomainModel
    {
        protected Entities db = new Entities();

        public CurrentUser getEmployeePermissions(String domain_id)
        {
            if (domain_id != null)
            {
                try
                {
                    return db.CurrentEmissaryDatas.First(x => x.Domain_ID.Equals(domain_id));
                }
                catch (InvalidOperationException) { }
            }

            return new CurrentUser()
            {
                ID = 0,
                Domain_ID = String.IsNullOrWhiteSpace(domain_id) ? ( domain_id = "Unknown\\Guest" ) : domain_id,
                Name = domain_id.Split(new char[] { '/', '\\' }, 2)[1],
                Admin = false,
                Ambassador = false
            }; ;
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}