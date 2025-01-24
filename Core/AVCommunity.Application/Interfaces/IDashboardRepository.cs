using AVCommunity.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVCommunity.Application.Interfaces
{
    public interface IDashboardRepository
    {
        Task<IEnumerable<GetDashboard_Summary_Response>> GetDashboard_Summary(GetDashboard_Summary_Search parameters);
        Task<IEnumerable<GetDashboard_AgeWiseCategoryDistribution_Response>> GetDashboard_AgeWiseCategoryDistribution(GetDashboard_AgeWiseCategoryDistribution_Search parameters);
        Task<IEnumerable<GetDashboard_DeathSummary_Response>> GetDashboard_DeathSummary(GetDashboard_DeathSummary_Search parameters);
    }
}
