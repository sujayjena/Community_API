using AVCommunity.Application.Models;
using AVCommunity.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVCommunity.Application.Interfaces
{
    public interface IUserRepository  
    {
        #region Admin 

        Task<int> SaveAdmin(Admin_Request parameters);

        Task<IEnumerable<Admin_Response>> GetAdminList(BaseSearchEntity parameters);

        Task<Admin_Response?> GetAdminById(long Id);

        Task<int> SaveAdminVillage(AdminVillage_Request parameters);

        Task<IEnumerable<AdminVillage_Response>> GetAdminVillageByEmployeeId(int EmployeeId, int BranchId);

        #endregion

        #region User 

        Task<int> SaveUser(User_Request parameters);

        Task<IEnumerable<User_Response>> GetUserList(User_Search parameters);

        Task<User_Response?> GetUserById(long Id);

        Task<IEnumerable<Champion_Response>> GetChampionList(Champion_Search parameters);

        Task<int> SaveSplit(Split_Request parameters);

        Task<IEnumerable<User_Response>> GetGlobalUserList(GlobalUser_Search parameters);

        #endregion
    }
}
