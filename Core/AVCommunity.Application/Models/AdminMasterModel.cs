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

    #region Occupation

    public class Occupation_Search : BaseSearchEntity
    {
    }

    public class Occupation_Request : BaseEntity
    {
        public string? OccupationName { get; set; }
        public bool? IsActive { get; set; }
    }

    public class Occupation_Response : BaseResponseEntity
    {
        public string? OccupationName { get; set; }
        public bool? IsActive { get; set; }
    }

    #endregion

    #region HigherStudy

    public class HigherStudy_Search : BaseSearchEntity
    {
    }

    public class HigherStudy_Request : BaseEntity
    {
        public string? HigherStudyName { get; set; }
        public bool? IsActive { get; set; }
    }

    public class HigherStudy_Response : BaseResponseEntity
    {
        public string? HigherStudyName { get; set; }
        public bool? IsActive { get; set; }
    }

    #endregion

    #region Relation

    public class Relation_Search : BaseSearchEntity
    {
    }

    public class Relation_Request : BaseEntity
    {
        public string? RelationName { get; set; }
        public bool? IsActive { get; set; }
    }

    public class Relation_Response : BaseResponseEntity
    {
        public string? RelationName { get; set; }
        public bool? IsActive { get; set; }
    }

    #endregion

    #region Position

    public class Position_Search : BaseSearchEntity
    {
    }

    public class Position_Request : BaseEntity
    {
        public string? PositionName { get; set; }
        public bool? IsActive { get; set; }
    }

    public class Position_Response : BaseResponseEntity
    {
        public string? PositionName { get; set; }
        public bool? IsActive { get; set; }
    }

    #endregion

    #region Industry

    public class Industry_Search : BaseSearchEntity
    {
    }

    public class Industry_Request : BaseEntity
    {
        public string? IndustryName { get; set; }

        public bool? IsActive { get; set; }
    }

    public class Industry_Response : BaseResponseEntity
    {
        public string? IndustryName { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Version Details

    public class VersionDetails_Search : BaseSearchEntity
    {
        [DefaultValue("")]
        public string? PackageName { get; set; }

        [DefaultValue("")]
        public string? UpdateType { get; set; }
    }

    public class VersionDetails_Request : BaseEntity
    {
        public int? AppVersionNo { get; set; }

        [DefaultValue("")]
        public string? AppVersionName { get; set; }

        [DefaultValue("")]
        public string? UpdateMsg { get; set; }

        [DefaultValue("")]
        public string? PackageName { get; set; }

        [DefaultValue("")]
        public string? UpdateType { get; set; }
        public bool? IsActive { get; set; }
    }

    public class VersionDetails_Response : BaseResponseEntity
    {
        public long? AppVersionNo { get; set; }
        public string? AppVersionName { get; set; }
        public string? UpdateMsg { get; set; }
        public string? PackageName { get; set; }
        public string? UpdateType { get; set; }
        public bool IsActive { get; set; }

    }

    #endregion
}
