﻿using AVCommunity.Domain.Entities;
using AVCommunity.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AVCommunity.Application.Models
{
    #region MeritalStatus

    public class MeritalStatus_Search : BaseSearchEntity
    {
    }

    public class MeritalStatus_Request : BaseEntity
    {
        public string? MeritalStatusName { get; set; }

        public bool? IsActive { get; set; }
    }

    public class MeritalStatus_Response : BaseResponseEntity
    {
        public string? MeritalStatusName { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Gender

    public class Gender_Search : BaseSearchEntity
    {
    }

    public class Gender_Request : BaseEntity
    {
        public string? GenderName { get; set; }

        public bool? IsActive { get; set; }
    }

    public class Gender_Response : BaseResponseEntity
    {
        public string? GenderName { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Blood Group

    public class BloodGroup_Search : BaseSearchEntity
    {
    }

    public class BloodGroup_Request : BaseEntity
    {
        public string? BloodGroup { get; set; }
        public bool? IsActive { get; set; }
    }

    public class BloodGroup_Response : BaseResponseEntity
    {
        public string? BloodGroup { get; set; }
        public bool? IsActive { get; set; }
    }

    #endregion
}