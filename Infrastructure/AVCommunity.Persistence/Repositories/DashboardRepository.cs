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
    public class DashboardRepository : GenericRepository, IDashboardRepository
    {
        private IConfiguration _configuration;

        public DashboardRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<GetDashboard_Summary_Response>> GetDashboard_Summary(GetDashboard_Summary_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();

            queryParameters.Add("@FromDate", parameters.FromDate);
            queryParameters.Add("@ToDate", parameters.ToDate);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<GetDashboard_Summary_Response>("GetDashboard_Summary", queryParameters);

            return result;
        }

        public async Task<IEnumerable<GetDashboard_AgeWiseCategoryDistribution_Response>> GetDashboard_AgeWiseCategoryDistribution(GetDashboard_AgeWiseCategoryDistribution_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();

            queryParameters.Add("@FromDate", parameters.FromDate);
            queryParameters.Add("@ToDate", parameters.ToDate);
            queryParameters.Add("@DistrictId", parameters.DistrictId);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<GetDashboard_AgeWiseCategoryDistribution_Response>("GetDashboard_AgeWiseCategoryDistribution", queryParameters);

            return result;
        }

        public async Task<IEnumerable<GetDashboard_BirthSummary_Response>> GetDashboard_BirthSummary(GetDashboard_BirthSummary_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();

            queryParameters.Add("@FromDate", parameters.FromDate);
            queryParameters.Add("@ToDate", parameters.ToDate);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<GetDashboard_BirthSummary_Response>("GetDashboard_BirthSummary", queryParameters);

            return result;
        }

        public async Task<IEnumerable<GetDashboard_DeathSummary_Response>> GetDashboard_DeathSummary(GetDashboard_DeathSummary_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();

            queryParameters.Add("@FromDate", parameters.FromDate);
            queryParameters.Add("@ToDate", parameters.ToDate);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<GetDashboard_DeathSummary_Response>("GetDashboard_DeathSummary", queryParameters);

            return result;
        }

    }
}
