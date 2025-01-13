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
    public class ManageVideoRepository : GenericRepository, IManageVideoRepository
    {
        private IConfiguration _configuration;

        public ManageVideoRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        #region Video Header
        public async Task<int> SaveVideoHeader(VideoHeader_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@DistrictId", parameters.DistrictId);
            queryParameters.Add("@VideoHeaderName", parameters.VideoHeaderName);
            queryParameters.Add("@StartDate", parameters.StartDate);
            queryParameters.Add("@EndDate", parameters.EndDate);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveVideoHeader", queryParameters);
        }

        public async Task<IEnumerable<VideoHeader_Response>> GetVideoHeaderList(VideoHeader_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@DistrictId", parameters.DistrictId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<VideoHeader_Response>("GetVideoHeaderList", queryParameters);

            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<VideoHeader_Response?> GetVideoHeaderById(long Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<VideoHeader_Response>("GetVideoHeaderById", queryParameters)).FirstOrDefault();
        }
        #endregion
    }
}
