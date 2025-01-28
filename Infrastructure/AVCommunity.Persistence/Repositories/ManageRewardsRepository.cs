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
    public class ManageRewardsRepository : GenericRepository, IManageRewardsRepository
    {
        private IConfiguration _configuration;

        public ManageRewardsRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> SaveRewards(Rewards_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@RegisterUserId", parameters.RegisterUserId);
            queryParameters.Add("@EmployeeId", parameters.EmployeeId);
            queryParameters.Add("@RewardDesc", parameters.RewardDesc);
            queryParameters.Add("@AttachmentOriginalFileName", parameters.AttachmentOriginalFileName);
            queryParameters.Add("@AttachmentFileName", parameters.AttachmentFileName);
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveRewards", queryParameters);
        }

        public async Task<IEnumerable<Rewards_Response>> GetRewardsList(Rewards_Search parameters)
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

            var result = await ListByStoredProcedure<Rewards_Response>("GetRewardsList", queryParameters);

            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<Rewards_Response?> GetRewardsById(long Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<Rewards_Response>("GetRewardsById", queryParameters)).FirstOrDefault();
        }

        public async Task<int> RewardsApproveNReject(Rewards_ApproveNReject parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();

            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@Remarks", parameters.Remarks);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("RewardsApproveNReject", queryParameters);
        }

        public async Task<IEnumerable<RewardRemarkLog_Response>> GetRewardRemarkLogListById(RewardRemarkLog_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@RewardId", parameters.RewardId);

            var result = await ListByStoredProcedure<RewardRemarkLog_Response>("GetRewardRemarkLogListById", queryParameters);

            return result;
        }
    }
}
