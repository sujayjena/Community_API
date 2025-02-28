﻿using AVCommunity.Application.Models;
using AVCommunity.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVCommunity.Application.Interfaces
{
    public interface IAdminMasterRepository
    {
        #region MeritalStatus

        Task<int> SaveMeritalStatus(MeritalStatus_Request parameters);

        Task<IEnumerable<MeritalStatus_Response>> GetMeritalStatusList(MeritalStatus_Search parameters);

        Task<MeritalStatus_Response?> GetMeritalStatusById(int Id);

        #endregion

        #region Gender

        Task<int> SaveGender(Gender_Request parameters);

        Task<IEnumerable<Gender_Response>> GetGenderList(Gender_Search parameters);

        Task<Gender_Response?> GetGenderById(int Id);

        #endregion

        #region Blood Group

        Task<int> SaveBloodGroup(BloodGroup_Request parameters);
        Task<IEnumerable<BloodGroup_Response>> GetBloodGroupList(BaseSearchEntity parameters);
        Task<BloodGroup_Response?> GetBloodGroupById(long Id);

        #endregion 

        #region Occupation

        Task<int> SaveOccupation(Occupation_Request parameters);
        Task<IEnumerable<Occupation_Response>> GetOccupationList(BaseSearchEntity parameters);
        Task<Occupation_Response?> GetOccupationById(long Id);

        #endregion 

        #region HigherStudy

        Task<int> SaveHigherStudy(HigherStudy_Request parameters);
        Task<IEnumerable<HigherStudy_Response>> GetHigherStudyList(BaseSearchEntity parameters);
        Task<HigherStudy_Response?> GetHigherStudyById(long Id);

        #endregion

        #region Relation

        Task<int> SaveRelation(Relation_Request parameters);
        Task<IEnumerable<Relation_Response>> GetRelationList(BaseSearchEntity parameters);
        Task<Relation_Response?> GetRelationById(long Id);

        #endregion

        #region Position

        Task<int> SavePosition(Position_Request parameters);
        Task<IEnumerable<Position_Response>> GetPositionList(BaseSearchEntity parameters);
        Task<Position_Response?> GetPositionById(long Id);

        #endregion

        #region Industry

        Task<int> SaveIndustry(Industry_Request parameters);

        Task<IEnumerable<Industry_Response>> GetIndustryList(Industry_Search parameters);

        Task<Industry_Response?> GetIndustryById(int Id);

        #endregion

        #region Version Details
        Task<int> SaveVersionDetails(VersionDetails_Request parameters);

        Task<IEnumerable<VersionDetails_Response>> GetVersionDetailsList(VersionDetails_Search parameters);

        Task<VersionDetails_Response?> GetVersionDetailsById(int Id);
        #endregion
    }
}
