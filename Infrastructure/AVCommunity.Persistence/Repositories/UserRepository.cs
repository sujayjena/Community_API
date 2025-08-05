using AVCommunity.Application.Enums;
using AVCommunity.Application.Helpers;
using AVCommunity.Application.Interfaces;
using AVCommunity.Application.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

        public async Task<IEnumerable<Admin_Response>> GetAdminList(Admin_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@AdminDistrictId", parameters.AdminDistrictId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<Admin_Response>("GetAdminList", queryParameters);

            if (SessionManager.LoggedInUserId > 1)
            {
                if (SessionManager.LoggedInUserId > 2)
                {
                    result = result.Where(x => x.Id > 2).ToList();
                }
                else
                {
                    result = result.Where(x => x.Id > 1).ToList();
                }
            }

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
            queryParameters.Add("@Age", parameters.Age);
            queryParameters.Add("@HigherStudyId", parameters.HigherStudyId);
            queryParameters.Add("@OccupationId", parameters.OccupationId);
            queryParameters.Add("@QuestionId", parameters.QuestionId);
            queryParameters.Add("@QuestionAnswer", parameters.QuestionAnswer);
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@UserType", "User");
            queryParameters.Add("@MobileUniqueId", parameters.MobileUniqueId);
            queryParameters.Add("@RegisterUserId", parameters.RegisterUserId);
            queryParameters.Add("@IndustryName", parameters.IndustryName);
            queryParameters.Add("@NativeName", parameters.NativeName);
            queryParameters.Add("@BloodGroupId", parameters.BloodGroupId);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveUser", queryParameters);
        }

        public async Task<IEnumerable<User_Response>> GetUserList(User_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@RegisterUserId", parameters.RegisterUserId);
            queryParameters.Add("@DistrictId", parameters.DistrictId);
            queryParameters.Add("@VillageId", parameters.VillageId);
            queryParameters.Add("@IsSplit", parameters.IsSplit);
            queryParameters.Add("@StatusId", parameters.StatusId);
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

        public async Task<IEnumerable<Champion_Response>> GetChampionList(Champion_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@FromAge", parameters.FromAge);
            queryParameters.Add("@ToAge", parameters.ToAge);
            queryParameters.Add("@DistrictId", parameters.DistrictId);
            queryParameters.Add("@VillageId", parameters.VillageId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<Champion_Response>("GetChampionList", queryParameters);

            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<int> SaveSplit(Split_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@EmployeeId", parameters.EmployeeId);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveSplit", queryParameters);
        }

        public async Task<IEnumerable<GloberUser_Response>> GetGlobalUserList(GlobalUser_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@EmployeeId", parameters.EmployeeId);
            queryParameters.Add("@BusinessId", parameters.BusinessId);
            queryParameters.Add("@DistrictId", parameters.DistrictId);
            queryParameters.Add("@VillageId", parameters.VillageId);
            queryParameters.Add("@AppType", parameters.AppType);
            queryParameters.Add("@RegisterUserId", parameters.RegisterUserId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<GloberUser_Response>("GetGlobalUserList", queryParameters);

            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<int> SaveUserIndustry(UserIndustry_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Action", parameters.Action);
            queryParameters.Add("@EmployeeId", parameters.UserId);
            queryParameters.Add("@IndustryId", parameters.IndustryId);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveUserIndustry", queryParameters);
        }

        public async Task<IEnumerable<UserIndustry_Response>> GetUserIndustryByEmployeeId(int EmployeeId, int IndustryId)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@EmployeeId", EmployeeId);
            queryParameters.Add("@IndustryId", IndustryId);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<UserIndustry_Response>("GetUserIndustryByEmployeeId", queryParameters);

            return result;
        }

        public async Task<int> ForgotPassword(ForgotPassword_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@MiddleName", parameters.MiddleName);
            queryParameters.Add("@MobileNumber", parameters.MobileNumber);
            queryParameters.Add("@NewPassword", parameters.NewPassword);
            queryParameters.Add("@ConfirmPassword", !string.IsNullOrWhiteSpace(parameters.ConfirmPassword) ? EncryptDecryptHelper.EncryptString(parameters.ConfirmPassword) : string.Empty);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("ForgotPassword", queryParameters);
        }

        public async Task<int> UserApproveNReject(User_ApproveNReject parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();

            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@Remarks", parameters.Remarks);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("UserApproveNReject", queryParameters);
        }

        public async Task<IEnumerable<UserRemarkLog_Response>> GetUserRemarkLogListById(UserRemarkLog_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@EmployeeId", parameters.EmployeeId);

            var result = await ListByStoredProcedure<UserRemarkLog_Response>("GetUserRemarkLogListById", queryParameters);

            return result;
        }

        public async Task<IEnumerable<User_Response>> GetBirthday_DeathListByDate(Birthday_DeathListByDate_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Filter_Date", parameters.Filter_Date);
            queryParameters.Add("@IsBirthday_Death", parameters.IsBirthday_Death);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<User_Response>("GetBirthday_DeathListByDate", queryParameters);

            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        #endregion
    }
}
