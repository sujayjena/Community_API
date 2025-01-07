using AVCommunity.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVCommunity.Application.Interfaces
{
    public interface IManageBirthRepository
    {
        Task<int> SaveBirth(Birth_Request parameters);

        Task<IEnumerable<Birth_Response>> GetBirthList(Birth_Search parameters);

        Task<Birth_Response?> GetBirthById(long Id);

        Task<int> BirthApproveNReject(Birth_ApproveNReject parameters);
    }
}
