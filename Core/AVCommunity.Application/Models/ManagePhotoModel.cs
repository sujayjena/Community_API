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
    #region Photo Header

    public class PhotoHeader_Search : BaseSearchEntity
    {
        public int? DistrictId { get; set; }
    }

    public class PhotoHeader_Request : BaseEntity
    {
        public int? DistrictId { get; set; }
        public string? PhotoHeaderName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
    }

    public class PhotoHeader_Response : BaseResponseEntity
    {
        public int? DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public string? PhotoHeaderName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
    }

    #endregion

    #region Photo

    public class Photo_Search : BaseSearchEntity
    {
        public int? PhotoHeaderId { get; set; }
    }

    public class Photo_Request : BaseEntity
    {
        public int? PhotoHeaderId { get; set; }

        [DefaultValue("")]
        public string? Comments { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [DefaultValue("")]
        public string? PhotoOriginalFileName { get; set; }

        [JsonIgnore]
        public string? PhotoFileName { get; set; }

        [DefaultValue("")]
        public string? Photo_Base64 { get; set; }
        public bool? IsActive { get; set; }
    }

    public class Photo_Response : BaseResponseEntity
    {
        public int? PhotoHeaderId { get; set; }
        public string? PhotoHeaderName { get; set; }
        public string? Comments { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? PhotoOriginalFileName { get; set; }
        public string? PhotoFileName { get; set; }
        public string? PhotoURL { get; set; }
        public bool? IsActive { get; set; }
    }

    #endregion
}
