using AVCommunity.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVCommunity.Application.Interfaces
{
    public interface IExecutiveBodyRepository
    {
        Task<int> SaveExecutiveBody(ExecutiveBody_Request parameters);

        Task<IEnumerable<ExecutiveBody_Response>> GetExecutiveBodyList(ExecutiveBody_Search parameters);

        Task<ExecutiveBody_Response?> GetExecutiveBodyById(long Id);
    }
}
