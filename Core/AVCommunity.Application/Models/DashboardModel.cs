using AVCommunity.Domain.Entities;
using AVCommunity.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVCommunity.Application.Models
{
    public class GetDashboard_Summary_Search
    {
        [DefaultValue(null)]
        public DateTime? FromDate { get; set; }

        [DefaultValue(null)]
        public DateTime? ToDate { get; set; }
    }

    public class GetDashboard_Summary_Response
    {
        public int? TotalAdminCount { get; set; }
        public int? TotalUserCount { get; set; }
        public int? TotalStateCount { get; set; }
        public int? TotalDistrictCount { get; set; }
        public int? TotalVillageCount { get; set; }
    }

    public class GetDashboard_AgeWiseCategoryDistribution_Search
    {
        [DefaultValue(null)]
        public DateTime? FromDate { get; set; }

        [DefaultValue(null)]
        public DateTime? ToDate { get; set; }

        public int? DistrictId { get; set; }
    }

    public class GetDashboard_AgeWiseCategoryDistribution_Response
    {
        public int? TotalMaleCount { get; set; }
        public int? TotalFemaleCount { get; set; }
        public int? TotalMaleAge_0_15 { get; set; }
        public int? TotalFemaleAge_0_15 { get; set; }
        public int? TotalMaleAge_16_30 { get; set; }
        public int? TotalFemaleAge_16_30 { get; set; }
        public int? TotalMaleAge_31_60 { get; set; }
        public int? TotalFemaleAge_31_60 { get; set; }
        public int? TotalMaleAge_61 { get; set; }
        public int? TotalFemaleAge_61 { get; set; }
    }
    public class GetDashboard_BirthSummary_Search
    {
        [DefaultValue(null)]
        public DateTime? FromDate { get; set; }

        [DefaultValue(null)]
        public DateTime? ToDate { get; set; }
    }

    public class GetDashboard_BirthSummary_Response
    {
        public int? TotalMaleCount { get; set; }
        public int? TotalFemaleCount { get; set; }
    }
    public class GetDashboard_DeathSummary_Search
    {
        [DefaultValue(null)]
        public DateTime? FromDate { get; set; }

        [DefaultValue(null)]
        public DateTime? ToDate { get; set; }
    }

    public class GetDashboard_DeathSummary_Response
    {
        public int? TotalMaleCount { get; set; }
        public int? TotalFemaleCount { get; set; }
    }
}
