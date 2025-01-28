using AVCommunity.Domain.Entities;
using AVCommunity.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AVCommunity.Application.Models
{
    public class ExecutiveBody_Search : BaseSearchEntity
    {
        public int? DistrictId { get; set; }

        [DefaultValue("")]
        public string? VillageId { get; set; }
    }

    public class ExecutiveBody_Request : BaseEntity
    {
        [DefaultValue("")]
        public string? ExecutiveBodyName { get; set; }

        [DefaultValue(0)]
        public int? PositionId { get; set; }
        public string? MobileNumber { get; set; }

        [DefaultValue(0)]
        public int? StateId { get; set; }

        [DefaultValue(0)]
        public int? DistrictId { get; set; }

        [DefaultValue(0)]
        public int? VillageId { get; set; }
        public bool? IsActive { get; set; }
    }

    public class ExecutiveBody_Response : BaseResponseEntity
    {
        public string? ExecutiveBodyName { get; set; }
        public int? PositionId { get; set; }
        public string? PositionName { get; set; }
        public string? MobileNumber { get; set; }
        public int? StateId { get; set; }
        public string? StateName { get; set; }
        public int? DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public int? VillageId { get; set; }
        public string? VillageName { get; set; }
        public bool? IsActive { get; set; }
    }
}
