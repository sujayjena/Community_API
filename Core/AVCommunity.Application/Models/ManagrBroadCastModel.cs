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
    public class BroadCast_Search : BaseSearchEntity
    {
        [DefaultValue(null)]
        public DateTime? BroadCastDate { get; set; }
    }

    public class BroadCast_Request : BaseEntity
    {
        public string? MessageName { get; set; }
        public int? SequenceNo { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
    }

    public class BroadCast_Response : BaseResponseEntity
    {
        public string? MessageName { get; set; }
        public int? SequenceNo { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; } 
        public bool? IsActive { get; set; }
    }
}
