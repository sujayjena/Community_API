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
    public class Death_Search : BaseSearchEntity
    {
        [DefaultValue(null)]
        public DateTime? FromDate { get; set; }

        [DefaultValue(null)]
        public DateTime? ToDate { get; set; }
        public int? StatusId { get; set; }
        public int? RegisterUserId { get; set; }
    }

    public class Death_Request : BaseEntity
    {
        public int? RegisterUserId { get; set; }
        public int? EmployeeId { get; set; }
        public string? DeathDesc { get; set; }
        public DateTime? DateOfDeath { get; set; }

        [DefaultValue("")]
        public string? AttachmentOriginalFileName { get; set; }

        [JsonIgnore]
        public string? AttachmentFileName { get; set; }

        [DefaultValue("")]
        public string? Attachment_Base64 { get; set; }
        public int? StatusId { get; set; }
        public bool? IsActive { get; set; }
    }

    public class Death_Response : BaseResponseEntity
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
        public string? DeathDesc { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public string? AttachmentOriginalFileName { get; set; }
        public string? AttachmentFileName { get; set; }
        public string? AttachmentURL { get; set; }
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public bool? IsActive { get; set; }
    }

    public class Death_ApproveNReject
    {
        public int? Id { get; set; }
        public int? StatusId { get; set; }

        [DefaultValue("")]
        public string? Remarks { get; set; }
    }

    public class DeathRemarkLog_Search
    {
        [DefaultValue(0)]
        public int? DeathId { get; set; }
    }

    public class DeathRemarkLog_Response : BaseEntity
    {
        public int? DeathId { get; set; }
        public string? Remarks { get; set; }
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public string? CreatorName { get; set; }
    }
}
