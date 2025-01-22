using AVCommunity.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVCommunity.Application.Interfaces
{
    public interface IManageMarriageRepository
    {
        Task<int> SaveMarriage(Marriage_Request parameters);

        Task<IEnumerable<Marriage_Response>> GetMarriageList(Marriage_Search parameters);

        Task<Marriage_Response?> GetMarriageById(long Id);

        Task<int> MarriageApproveNReject(Marriage_ApproveNReject parameters);

        Task<IEnumerable<MarriageRemarkLog_Response>> GetMarriageRemarkLogListById(MarriageRemarkLog_Search parameters);
    }
}
