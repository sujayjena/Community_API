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
    public class Rewards_Search : BaseSearchEntity
    {
        [DefaultValue(null)]
        public DateTime? FromDate { get; set; }

        [DefaultValue(null)]
        public DateTime? ToDate { get; set; }
        public int? StatusId { get; set; }
        public int? RegisterUserId { get; set; }
        public int? DistrictId { get; set; }

        [DefaultValue("")]
        public string? VillageId { get; set; }
    }

    public class Rewards_Request : BaseEntity
    {
        public int? RegisterUserId { get; set; }
        public int? EmployeeId { get; set; }
        public string? RewardDesc { get; set; }

        [DefaultValue("")]
        public string? AttachmentOriginalFileName { get; set; }

        [JsonIgnore]
        public string? AttachmentFileName { get; set; }

        [DefaultValue("")]
        public string? Attachment_Base64 { get; set; }
        public int? StatusId { get; set; }
        public bool? IsActive { get; set; }
    }

    public class Rewards_Response : BaseResponseEntity
    {
        public int? RegisterUserId { get; set; }
        public string? RegisterUser { get; set; }
        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public int? Age { get; set; }
        public int? GenderId { get; set; }
        public string? GenderName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? MobileNumber { get; set; }
        public string? RewardDesc { get; set; }
        public string? AttachmentOriginalFileName { get; set; }
        public string? AttachmentFileName { get; set; }
        public string? AttachmentURL { get; set; }
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public bool? IsActive { get; set; }
    }

    public class Rewards_ApproveNReject
    {
        public int? Id { get; set; }
        public int? StatusId { get; set; }

        [DefaultValue("")]
        public string? Remarks { get; set; }
    }

    public class RewardRemarkLog_Search
    {
        [DefaultValue(0)]
        public int? RewardId { get; set; }
    }

    public class RewardRemarkLog_Response : BaseEntity
    {
        public int? RewardId { get; set; }
        public string? Remarks { get; set; }
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public string? CreatorName { get; set; }
    }
}
