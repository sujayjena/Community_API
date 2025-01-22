using AVCommunity.Application.Models;
using AVCommunity.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVCommunity.Application.Interfaces
{
    public interface ITerritoryRepository
    {
        #region State 

        Task<int> SaveState(State_Request parameters);

        Task<IEnumerable<State_Response>> GetStateList(BaseSearchEntity parameters);

        Task<State_Response?> GetStateById(long Id);

        #endregion

        #region District 

        Task<int> SaveDistrict(District_Request parameters);

        Task<IEnumerable<District_Response>> GetDistrictList(BaseSearchEntity parameters);

        Task<District_Response?> GetDistrictById(long Id);

        #endregion

        #region Village 

        Task<int> SaveVillage(Village_Request parameters);

        Task<IEnumerable<Village_Response>> GetVillageList(BaseSearchEntity parameters);

        Task<Village_Response?> GetVillageById(long Id);

        #endregion

        #region Territories 

        Task<int> SaveTerritories(Territories_Request parameters);

        Task<IEnumerable<Territories_Response>> GetTerritoriesList(BaseSearchEntity parameters);

        Task<Territories_Response?> GetTerritoriesById(long Id);

        Task<IEnumerable<Territories_State_Dist_Village_Response>> GetTerritories_State_Dist_Village_List_ById(Territories_State_Dist_Village_Search parameters);

        #endregion
    }
}
 