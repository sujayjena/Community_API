using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVCommunity.Application.Models
{
    public class Champion_ResponseModel
    {
        [DefaultValue(0)]
        public long Id { get; set; }

        public bool? IsSuccess { get; set; }
        public string? Message { get; set; }

        [DefaultValue(0)]
        public long Total { get; set; }
        public long TotalMemberCount { get; set; }
        public object Data { get; set; }
    }
}
