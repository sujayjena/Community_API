using AVCommunity.Application.Models;
using AVCommunity.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVCommunity.Application.Interfaces
{
    public interface IAdminMasterRepository
    {
        #region MeritalStatus

        Task<int> SaveMeritalStatus(MeritalStatus_Request parameters);

        Task<IEnumerable<MeritalStatus_Response>> GetMeritalStatusList(MeritalStatus_Search parameters);

        Task<MeritalStatus_Response?> GetMeritalStatusById(int Id);

        #endregion

        #region Gender

        Task<int> SaveGender(Gender_Request parameters);

        Task<IEnumerable<Gender_Response>> GetGenderList(Gender_Search parameters);

        Task<Gender_Response?> GetGenderById(int Id);

        #endregion

        #region Blood Group

        Task<int> SaveBloodGroup(BloodGroup_Request parameters);
        Task<IEnumerable<BloodGroup_Response>> GetBloodGroupList(BaseSearchEntity parameters);
        Task<BloodGroup_Response?> GetBloodGroupById(long Id);

        #endregion 
    }
}
