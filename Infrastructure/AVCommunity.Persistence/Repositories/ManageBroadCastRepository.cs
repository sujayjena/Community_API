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
    public class ManageBroadCastRepository : GenericRepository, IManageBroadCastRepository
    {
        private IConfiguration _configuration;

        public ManageBroadCastRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> SaveBroadCast(BroadCast_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@MessageName", parameters.MessageName);
            queryParameters.Add("@SequenceNo", parameters.SequenceNo);
            queryParameters.Add("@StartDate", parameters.StartDate);
            queryParameters.Add("@EndDate", parameters.EndDate); 
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveBroadCast", queryParameters);
        }

        public async Task<IEnumerable<BroadCast_Response>> GetBroadCastList(BroadCast_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@BroadCastDate", parameters.BroadCastDate);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<BroadCast_Response>("GetBroadCastList", queryParameters);

            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<BroadCast_Response?> GetBroadCastById(long Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<BroadCast_Response>("GetBroadCastById", queryParameters)).FirstOrDefault();
        }
    }
}
