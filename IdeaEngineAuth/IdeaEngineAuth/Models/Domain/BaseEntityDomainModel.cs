using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaEngineAuth.Models
{
    public interface BaseEntityDomainModel<O, L, D, E> : BaseDomainModel
        where L : UserViewModelList<O>, UserViewModel
        where D : O, UserViewModel
        where E : D, UserViewModel
    {
        L buildOverviewViewModelList(CurrentUser currentUser);
        D buildDetailsViewModel(CurrentUser currentUser, long id);
        E buildEditViewModel(CurrentUser currentUser, long id);
        E buildCreateViewModel(CurrentUser currentUser);
        bool saveModelData(E model);
        bool deleteModelData(CurrentUser currentUser, long id);
    }
}