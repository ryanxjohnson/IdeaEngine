using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaEngineAuth.Models
{
    public class FileViewModelBuilder : BaseViewModelBuilder, FileDomainModel
    {
        public FileLink getFileLink(CurrentUser currentUser, long fileID)
        {
            return currentUser.IsAdmin() || 0.Equals(db.CurrentFileStatus.Find(fileID).Archived ?? 1)
                ? db.CurrentFileDatas.Find(fileID) : null;
        }
    }
}