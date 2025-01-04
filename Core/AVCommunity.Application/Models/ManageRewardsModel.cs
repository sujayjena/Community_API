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
    }

    public class Rewards_Request : BaseEntity
    {
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
        public string? RewardDesc { get; set; }
        public string? AttachmentOriginalFileName { get; set; }
        public string? AttachmentFileName { get; set; }
        public string? AttachmentURL { get; set; }
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public bool? IsActive { get; set; }
    }
}
