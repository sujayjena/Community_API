using AVCommunity.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVCommunity.Application.Interfaces
{
    public interface IManageDeathRepository
    {
        Task<int> SaveDeath(Death_Request parameters);

        Task<IEnumerable<Death_Response>> GetDeathList(Death_Search parameters);

        Task<Death_Response?> GetDeathById(long Id);

        Task<int> DeathApproveNReject(Death_ApproveNReject parameters);
    }
}
 