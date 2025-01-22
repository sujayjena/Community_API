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
    #region Video Header

    public class VideoHeader_Search : BaseSearchEntity
    {
        public int? DistrictId { get; set; }
    }

    public class VideoHeader_Request : BaseEntity
    {
        public int? DistrictId { get; set; }
        public string? VideoHeaderName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
    }

    public class VideoHeader_Response : BaseResponseEntity
    {
        public int? DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public string? VideoHeaderName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
    }

    #endregion

    #region Video

    public class Video_Search : BaseSearchEntity
    {
        public int? VideoHeaderId { get; set; }
    }

    public class Video_Request : BaseEntity
    {
        public int? VideoHeaderId { get; set; }

        [DefaultValue("")]
        public string? Comments { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [DefaultValue("")]
        public string? VideoOriginalFileName { get; set; }

        [JsonIgnore]
        public string? VideoFileName { get; set; }

        [DefaultValue("")]
        public string? Video_Base64 { get; set; }
        public bool? IsActive { get; set; }
    }

    public class Video_Response : BaseResponseEntity
    {
        public int? VideoHeaderId { get; set; }
        public string? VideoHeaderName { get; set; }
        public string? Comments { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? VideoOriginalFileName { get; set; }
        public string? VideoFileName { get; set; }
        public string? VideoURL { get; set; }
        public bool? IsActive { get; set; }
    }

    #endregion
}
