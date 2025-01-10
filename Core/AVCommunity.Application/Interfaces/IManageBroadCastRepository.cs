using AVCommunity.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVCommunity.Application.Interfaces
{
    public interface IManageBroadCastRepository
    {
        Task<int> SaveBroadCast(BroadCast_Request parameters);

        Task<IEnumerable<BroadCast_Response>> GetBroadCastList(BroadCast_Search parameters);

        Task<BroadCast_Response?> GetBroadCastById(long Id);
    }
}
