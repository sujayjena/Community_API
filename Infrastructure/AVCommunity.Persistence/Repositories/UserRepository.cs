using AVCommunity.Application.Helpers;
using AVCommunity.Application.Interfaces;
using AVCommunity.Application.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVCommunity.Persistence.Repositories
{
    public class UserRepository : GenericRepository, IUserRepository
    {
        private IConfiguration _configuration;

        public UserRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        #region Admin 

        public async Task<int> SaveAdmin(Admin_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@AdminName", parameters.AdminName);
            queryParameters.Add("@MobileNumber", parameters.MobileNumber);
            queryParameters.Add("@EmailId", parameters.EmailId);
            queryParameters.Add("@Password", !string.IsNullOrWhiteSpace(parameters.Password) ? EncryptDecryptHelper.EncryptString(parameters.Password) : string.Empty);
            queryParameters.Add("@UserType", parameters.UserType);
            queryParameters.Add("@StateId", parameters.StateId);
            queryParameters.Add("@DistrictId", parameters.DistrictId);
            queryParameters.Add("@VillageId", parameters.VillageId);
            queryParameters.Add("@Pincode", parameters.Pincode);
            queryParameters.Add("@AdminDistrictId", parameters.AdminDistrictId);
            queryParameters.Add("@UserType", "Admin");
            queryParameters.Add("@MobileUniqueId", parameters.MobileUniqueId);
            queryParameters.Add("@IsMobileUser", parameters.IsMobileUser);
            queryParameters.Add("@IsWebUser", parameters.IsWebUser);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveAdmin", queryParameters);
        }

        public async Task<IEnumerable<Admin_Response>> GetAdminList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<Admin_Response>("GetAdminList", queryParameters);

            //if (SessionManager.LoggedInUserId > 1)
            //{
            //    if (SessionManager.LoggedInUserId > 2)
            //    {
            //        result = result.Where(x => x.Id > 2).ToList();
            //    }
            //    else
            //    {
            //        result = result.Where(x => x.Id > 1).ToList();
            //    }
            //}

            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<Admin_Response?> GetAdminById(long Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<Admin_Response>("GetAdminById", queryParameters)).FirstOrDefault();
        }

        public async Task<int> SaveAdminVillage(AdminVillage_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Action", parameters.Action);
            queryParameters.Add("@EmployeeId", parameters.UserId);
            queryParameters.Add("@VillageId", parameters.VillageId);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveAdminVillage", queryParameters);
        }

        public async Task<IEnumerable<AdminVillage_Response>> GetAdminVillageByEmployeeId(int EmployeeId, int VillageId)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@EmployeeId", EmployeeId);
            queryParameters.Add("@VillageId", VillageId);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<AdminVillage_Response>("GetAdminVillageByEmployeeId", queryParameters);

            return result;
        }

        #endregion

        #region User 

        public async Task<int> SaveUser(User_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@FirstName", parameters.FirstName);
            queryParameters.Add("@MiddleName", parameters.MiddleName);
            queryParameters.Add("@LastName", parameters.LastName);
            queryParameters.Add("@Surname", parameters.Surname);
            queryParameters.Add("@MobileNumber", parameters.MobileNumber);
            queryParameters.Add("@EmailId", parameters.EmailId);
            queryParameters.Add("@Password", !string.IsNullOrWhiteSpace(parameters.Password) ? EncryptDecryptHelper.EncryptString(parameters.Password) : string.Empty);
            queryParameters.Add("@RelationId", parameters.RelationId);
            queryParameters.Add("@GenderId", parameters.GenderId);
            queryParameters.Add("@MeritalStatusId", parameters.MeritalStatusId);
            queryParameters.Add("@CurrentAddress", parameters.CurrentAddress);
            queryParameters.Add("@StateId", parameters.StateId);
            queryParameters.Add("@DistrictId", parameters.DistrictId);
            queryParameters.Add("@VillageId", parameters.VillageId);
            queryParameters.Add("@Pincode", parameters.Pincode);
            queryParameters.Add("@BusinessAddress", parameters.BusinessAddress);
            queryParameters.Add("@BusinessStateId", parameters.BusinessStateId);
            queryParameters.Add("@BusinessDistrictId", parameters.BusinessDistrictId);
            queryParameters.Add("@BusinessVillageId", parameters.BusinessVillageId);
            queryParameters.Add("@BusinessPincode", parameters.BusinessPincode);
            queryParameters.Add("@DateOfBirth", parameters.DateOfBirth);
            queryParameters.Add("@HigherStudyId", parameters.HigherStudyId);
            queryParameters.Add("@OccupationId", parameters.OccupationId);
            queryParameters.Add("@QuestionId", parameters.QuestionId);
            queryParameters.Add("@QuestionAnswer", parameters.QuestionAnswer);
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@UserType", "User");
            queryParameters.Add("@MobileUniqueId", parameters.MobileUniqueId);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveUser", queryParameters);
        }

        public async Task<IEnumerable<User_Response>> GetUserList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<User_Response>("GetUserList", queryParameters);

            //if (SessionManager.LoggedInUserId > 1)
            //{
            //    if (SessionManager.LoggedInUserId > 2)
            //    {
            //        result = result.Where(x => x.Id > 2).ToList();
            //    }
            //    else
            //    {
            //        result = result.Where(x => x.Id > 1).ToList();
            //    }
            //}

            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<User_Response?> GetUserById(long Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<User_Response>("GetUserById", queryParameters)).FirstOrDefault();
        }

        #endregion
    }
}
