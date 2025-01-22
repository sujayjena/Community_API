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
    public class ExecutiveBodyRepository : GenericRepository, IExecutiveBodyRepository
    {
        private IConfiguration _configuration;

        public ExecutiveBodyRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> SaveExecutiveBody(ExecutiveBody_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id); 
            queryParameters.Add("@ExecutiveBodyName", parameters.ExecutiveBodyName);
            queryParameters.Add("@PositionId", parameters.PositionId);
            queryParameters.Add("@MobileNumber", parameters.MobileNumber);
            queryParameters.Add("@StateId", parameters.StateId);
            queryParameters.Add("@DistrictId", parameters.DistrictId);
            queryParameters.Add("@VillageId", parameters.VillageId);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveExecutiveBody", queryParameters);
        }

        public async Task<IEnumerable<ExecutiveBody_Response>> GetExecutiveBodyList(ExecutiveBody_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<ExecutiveBody_Response>("GetExecutiveBodyList", queryParameters);

            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<ExecutiveBody_Response?> GetExecutiveBodyById(long Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<ExecutiveBody_Response>("GetExecutiveBodyById", queryParameters)).FirstOrDefault();
        }
    }
}
