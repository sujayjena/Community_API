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
    public class ManageBirthRepository : GenericRepository, IManageBirthRepository
    {
        private IConfiguration _configuration;

        public ManageBirthRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> SaveBirth(Birth_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@EmployeeId", parameters.EmployeeId);
            queryParameters.Add("@BirthDesc", parameters.BirthDesc);
            queryParameters.Add("@AttachmentOriginalFileName", parameters.AttachmentOriginalFileName);
            queryParameters.Add("@AttachmentFileName", parameters.AttachmentFileName); 
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveBirth", queryParameters);
        }

        public async Task<IEnumerable<Birth_Response>> GetBirthList(Birth_Search parameters)
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

            var result = await ListByStoredProcedure<Birth_Response>("GetBirthList", queryParameters);

            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<Birth_Response?> GetBirthById(long Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<Birth_Response>("GetBirthById", queryParameters)).FirstOrDefault();
        }

        public async Task<int> BirthApproveNReject(Birth_ApproveNReject parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();

            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@Remarks", parameters.Remarks);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("BirthApproveNReject", queryParameters);
        }
    }
}
