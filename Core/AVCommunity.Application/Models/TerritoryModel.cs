using AVCommunity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVCommunity.Application.Models
{
    public class TerritoryModel
    {
    }

    #region State

    public class State_Request : BaseEntity
    {
        public string? StateName { get; set; }

        public bool? IsActive { get; set; }
    }

    public class State_Response : BaseResponseEntity
    {
        public string? StateName { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region District

    public class District_Request : BaseEntity
    {
        public string? DistrictName { get; set; }

        public bool? IsActive { get; set; }
    }

    public class District_Response : BaseResponseEntity
    {
        public string? DistrictName { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Village

    public class Village_Request : BaseEntity
    {
        public string? VillageName { get; set; }

        public bool? IsActive { get; set; }
    }

    public class Village_Response : BaseResponseEntity
    {
        public string? VillageName { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Territories

    public class Territories_Request : BaseEntity
    {
        public int? StateId { get; set; }

        public int? DistrictId { get; set; }

        public int? VillageId { get; set; }

        public bool? IsActive { get; set; }
    }

    public class Territories_Response : BaseResponseEntity
    {
        public int? StateId { get; set; }

        public string? StateName { get; set; }

        public int? DistrictId { get; set; }

        public string? DistrictName { get; set; }

        public int? VillageId { get; set; }

        public string? VillageName { get; set; }

        public bool? IsActive { get; set; }
    }

    public class Territories_State_Dist_Village_Search
    {
        public int? StateId { get; set; }

        public int? DistrictId { get; set; }

        public int? VillageId { get; set; }
    }

    public class Territories_State_Dist_Village_Response
    {
        public int? Id { get; set; }

        public string? Value { get; set; }

        public string? Text { get; set; }
    }

    #endregion
}
 