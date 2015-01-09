using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaEngineAuth.Models
{
    public interface BaseDomainModel
    {
        CurrentUser getEmployeePermissions(String domain_id);
        void Dispose();
    }
}