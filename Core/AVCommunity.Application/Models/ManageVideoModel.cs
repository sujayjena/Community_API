using AVCommunity.Domain.Entities;
using AVCommunity.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
}
