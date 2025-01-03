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
    public class UserModel
    {
    }

    #region Admin

    public class Admin_Request : BaseEntity
    {
        public Admin_Request()
        {
            AdminVillageList = new List<AdminVillage_Request>();
        }

        public string? AdminName { get; set; }

        public string? MobileNumber { get; set; }

        public string? EmailId { get; set; }

        public string? Password { get; set; }

        public int? StateId { get; set; }

        public int? DistrictId { get; set; }

        public int? VillageId { get; set; }

        public int? Pincode { get; set; }

        public int? AdminDistrictId { get; set; }

        [JsonIgnore]
        [DefaultValue("Admin")]
        public string? UserType { get; set; }

        public string? MobileUniqueId { get; set; }

        public bool? IsMobileUser { get; set; }

        public bool? IsWebUser { get; set; }

        public bool? IsActive { get; set; }

        public List<AdminVillage_Request>? AdminVillageList { get; set; }
    }

    public class Admin_Response : BaseResponseEntity
    {
        public Admin_Response()
        {
            AdminVillageList = new List<AdminVillage_Response>();
        }

        public string? AdminName { get; set; }

        public string? MobileNumber { get; set; }

        public string? EmailId { get; set; }

        public string? Password { get; set; }

        public int? StateId { get; set; }
        public string? StateName { get; set; }

        public int? DistrictId { get; set; }
        public string? DistrictName { get; set; }

        public int? VillageId { get; set; }
        public string? VillageName { get; set; }

        public int? Pincode { get; set; }

        public int? AdminDistrictId { get; set; }
        public string? AdminDistrictName { get; set; }

        public string? UserType { get; set; }

        public bool? IsMobileUser { get; set; }

        public bool? IsWebUser { get; set; }

        public bool? IsActive { get; set; }

        public List<AdminVillage_Response>? AdminVillageList { get; set; }
    }

    public class AdminVillage_Request : BaseEntity
    {
        [JsonIgnore]
        public string? Action { get; set; }
        [JsonIgnore]
        public int? UserId { get; set; }
        public int? VillageId { get; set; }
    }
    public class AdminVillage_Response : BaseEntity
    {
        public int? UserId { get; set; }
        public int? VillageId { get; set; }
        public string? VillageName { get; set; }
    }

    #endregion

    #region User
    public class User_Request : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }

        public string? Surname { get; set; }

        public string? MobileNumber { get; set; }

        public string? EmailId { get; set; }

        public string? Password { get; set; }

        public int? RelationId { get; set; }

        public int? GenderId { get; set; }

        public int? MeritalStatusId { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public int? HigherStudyId { get; set; }

        public string? CurrentAddress { get; set; }
        public int? StateId { get; set; }

        public int? DistrictId { get; set; }

        public int? VillageId { get; set; }

        public int? Pincode { get; set; }

        public int? OccupationId { get; set; }

        public string? BusinessAddress { get; set; }

        public int? BusinessStateId { get; set; }

        public int? BusinessDistrictId { get; set; }

        public int? BusinessVillageId { get; set; }

        public int? BusinessPincode { get; set; }
        public int? QuestionId { get; set; }
        public string? QuestionAnswer { get; set; }
        public int? StatusId { get; set; }

        public string? MobileUniqueId { get; set; }

        [DefaultValue(0)]
        public int? RegisterUserId { get; set; }

        [JsonIgnore]
        [DefaultValue("User")]
        public string? UserType { get; set; }

        public bool? IsActive { get; set; }
    }

    public class User_Search : BaseSearchEntity
    {
        public int? RegisterUserId { get; set; }
    }

    public class User_Response : BaseResponseEntity
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }

        public string? Surname { get; set; }

        public string? MobileNumber { get; set; }

        public string? EmailId { get; set; }

        public string? Password { get; set; }

        public int? RelationId { get; set; }
        public int? RelationName { get; set; }

        public int? GenderId { get; set; }
        public string? GenderName { get; set; }

        public int? MeritalStatusId { get; set; }
        public string? MeritalStatusName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public int? HigherStudyId { get; set; }
        public string? HigherStudyName { get; set; }

        public string? CurrentAddress { get; set; }
        public int? StateId { get; set; }
        public string? StateName { get; set; }

        public int? DistrictId { get; set; }
        public string? DistrictName { get; set; }

        public int? VillageId { get; set; }
        public string? VillageName { get; set; }

        public int? Pincode { get; set; }

        public int? OccupationId { get; set; }
        public string? OccupationName { get; set; }

        public string? BusinessAddress { get; set; }

        public int? BusinessStateId { get; set; }
        public string? BusinessStateName { get; set; }

        public int? BusinessDistrictId { get; set; }
        public string? BusinessDistrictName { get; set; }
        public int? BusinessVillageId { get; set; }
        public string? BusinessVillageName { get; set; }
        public int? BusinessPincode { get; set; }
        public int? QuestionId { get; set; }
        public string? QuestionName { get; set; }
        public string? QuestionAnswer { get; set; }
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public string? UserType { get; set; }
        public string? MobileUniqueId { get; set; }
        public int? RegisterUserId { get; set; }
        public string? RegisterUser { get; set; }
        public bool? IsActive { get; set; }
    }

    public class SelectList_Response
    {
        public long Value { get; set; }
        public string? Text { get; set; }
    }

    #endregion
}
