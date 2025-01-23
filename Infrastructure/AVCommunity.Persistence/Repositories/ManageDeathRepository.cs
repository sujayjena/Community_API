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
    public class ManageDeathRepository : GenericRepository, IManageDeathRepository
    {
        private IConfiguration _configuration;

        public ManageDeathRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> SaveDeath(Death_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@RegisterUserId", parameters.RegisterUserId);
            queryParameters.Add("@EmployeeId", parameters.EmployeeId);
            queryParameters.Add("@DeathDesc", parameters.DeathDesc);
            queryParameters.Add("@DateOfDeath", parameters.DateOfDeath);
            queryParameters.Add("@AttachmentOriginalFileName", parameters.AttachmentOriginalFileName);
            queryParameters.Add("@AttachmentFileName", parameters.AttachmentFileName);
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveDeath", queryParameters);
        }

        public async Task<IEnumerable<Death_Response>> GetDeathList(Death_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@FromDate", parameters.FromDate);
            queryParameters.Add("@ToDate", parameters.ToDate);
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@RegisterUserId", parameters.RegisterUserId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<Death_Response>("GetDeathList", queryParameters);

            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<Death_Response?> GetDeathById(long Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<Death_Response>("GetDeathById", queryParameters)).FirstOrDefault();
        }

        public async Task<int> DeathApproveNReject(Death_ApproveNReject parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();

            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@Remarks", parameters.Remarks);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("DeathApproveNReject", queryParameters);
        }

        public async Task<IEnumerable<DeathRemarkLog_Response>> GetDeathRemarkLogListById(DeathRemarkLog_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@DeathId", parameters.DeathId);

            var result = await ListByStoredProcedure<DeathRemarkLog_Response>("GetDeathRemarkLogListById", queryParameters);

            return result;
        }
    }
}
