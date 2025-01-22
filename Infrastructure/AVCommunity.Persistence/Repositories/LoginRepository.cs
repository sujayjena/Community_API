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
    public class LoginRepository : GenericRepository, ILoginRepository
    {
        private IConfiguration _configuration;

        public LoginRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<UsersLoginSessionData?> ValidateUserLoginByEmail(LoginByMobileNumberRequestModel parameters)
        {
            IEnumerable<UsersLoginSessionData> lstResponse;
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Username", parameters.MobileNumber.SanitizeValue());
            queryParameters.Add("@Password", parameters.Password.SanitizeValue()); 
            queryParameters.Add("@MobileUniqueId", parameters.MobileUniqueId.SanitizeValue());

            lstResponse = await ListByStoredProcedure<UsersLoginSessionData>("ValidateUserLoginByUsername", queryParameters);
            return lstResponse.FirstOrDefault();
        }

        public async Task SaveUserLoginHistory(UserLoginHistorySaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@UserId", parameters.UserId);
            queryParameters.Add("@UserToken", parameters.UserToken.SanitizeValue());
            queryParameters.Add("@TokenExpireOn", parameters.TokenExpireOn);
            queryParameters.Add("@DeviceName", parameters.DeviceName.SanitizeValue());
            queryParameters.Add("@IPAddress", parameters.IPAddress.SanitizeValue());
            queryParameters.Add("@RememberMe", parameters.RememberMe);
            queryParameters.Add("@IsLoggedIn", parameters.IsLoggedIn);

            await ExecuteNonQuery("SaveUserLoginHistory", queryParameters);
        }

        public async Task<UsersLoginSessionData?> GetProfileDetailsByToken(string token)
        {
            IEnumerable<UsersLoginSessionData> lstResponse;
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Token", token);
            lstResponse = await ListByStoredProcedure<UsersLoginSessionData>("GetProfileDetailsByToken", queryParameters);

            return lstResponse.FirstOrDefault();
        }
    }
}
