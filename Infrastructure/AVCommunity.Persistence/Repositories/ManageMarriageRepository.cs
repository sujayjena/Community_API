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
    public class ManageMarriageRepository : GenericRepository, IManageMarriageRepository
    {
        private IConfiguration _configuration;

        public ManageMarriageRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> SaveMarriage(Marriage_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@RegisterUserId", parameters.RegisterUserId);
            queryParameters.Add("@EmployeeId", parameters.EmployeeId);
            queryParameters.Add("@MarriageDesc", parameters.MarriageDesc);
            queryParameters.Add("@AttachmentOriginalFileName", parameters.AttachmentOriginalFileName);
            queryParameters.Add("@AttachmentFileName", parameters.AttachmentFileName);
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveMarriage", queryParameters);
        }

        public async Task<IEnumerable<Marriage_Response>> GetMarriageList(Marriage_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@FromDate", parameters.FromDate);
            queryParameters.Add("@ToDate", parameters.ToDate);
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@RegisterUserId", parameters.RegisterUserId);
            queryParameters.Add("@DistrictId", parameters.DistrictId);
            queryParameters.Add("@VillageId", parameters.VillageId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<Marriage_Response>("GetMarriageList", queryParameters);

            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<Marriage_Response?> GetMarriageById(long Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<Marriage_Response>("GetMarriageById", queryParameters)).FirstOrDefault();
        }

        public async Task<int> MarriageApproveNReject(Marriage_ApproveNReject parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();

            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@Remarks", parameters.Remarks);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("MarriageApproveNReject", queryParameters);
        }

        public async Task<IEnumerable<MarriageRemarkLog_Response>> GetMarriageRemarkLogListById(MarriageRemarkLog_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@MarriageId", parameters.MarriageId);

            var result = await ListByStoredProcedure<MarriageRemarkLog_Response>("GetMarriageRemarkLogListById", queryParameters);

            return result;
        }
    }
}
