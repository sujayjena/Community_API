using AVCommunity.Domain.Entities;
using AVCommunity.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVCommunity.Application.Models
{
    public class BroadCast_Search : BaseSearchEntity
    {
    }

    public class BroadCast_Request : BaseEntity
    {
        public string? MessageName { get; set; }
        public int? SequenceNo { get; set; }
        public bool? IsActive { get; set; }
    }

    public class BroadCast_Response : BaseResponseEntity
    {
        public string? MessageName { get; set; }
        public int? SequenceNo { get; set; }
        public bool? IsActive { get; set; }
    }
}
