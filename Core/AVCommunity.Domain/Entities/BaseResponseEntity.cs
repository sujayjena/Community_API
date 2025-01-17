using Microsoft.AspNetCore.Http;

namespace AVCommunity.Domain.Entities
{
    public class BaseResponseEntity : BaseEntity
    {
        public string CreatorName { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public string ModifierName { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
 