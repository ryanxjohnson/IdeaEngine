using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaEngineAuth.Models
{
    public interface FileDomainModel : BaseDomainModel
    {
        FileLink getFileLink(CurrentUser currentUser, long fileID);
    }
}