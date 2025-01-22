using AVCommunity.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVCommunity.Application.Interfaces
{
    public interface IManageRewardsRepository
    {
        Task<int> SaveRewards(Rewards_Request parameters);

        Task<IEnumerable<Rewards_Response>> GetRewardsList(Rewards_Search parameters);

        Task<Rewards_Response?> GetRewardsById(long Id);

        Task<int> RewardsApproveNReject(Rewards_ApproveNReject parameters);
    }
}
