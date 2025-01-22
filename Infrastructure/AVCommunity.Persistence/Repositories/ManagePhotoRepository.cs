using AVCommunity.Application.Helpers;
using AVCommunity.Application.Interfaces;
using AVCommunity.Application.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVCommunity.Persistence.Repositories
{
    public class ManagePhotoRepository : GenericRepository, IManagePhotoRepository
    {
        private IConfiguration _configuration;

        public ManagePhotoRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        #region Photo Header
        public async Task<int> SavePhotoHeader(PhotoHeader_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@DistrictId", parameters.DistrictId);
            queryParameters.Add("@PhotoHeaderName", parameters.PhotoHeaderName);
            queryParameters.Add("@StartDate", parameters.StartDate);
            queryParameters.Add("@EndDate", parameters.EndDate);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SavePhotoHeader", queryParameters);
        }

        public async Task<IEnumerable<PhotoHeader_Response>> GetPhotoHeaderList(PhotoHeader_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@DistrictId", parameters.DistrictId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<PhotoHeader_Response>("GetPhotoHeaderList", queryParameters);

            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<PhotoHeader_Response?> GetPhotoHeaderById(long Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<PhotoHeader_Response>("GetPhotoHeaderById", queryParameters)).FirstOrDefault();
        }
        #endregion

        #region Photo
        public async Task<int> SavePhoto(Photo_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@PhotoHeaderId", parameters.PhotoHeaderId);
            queryParameters.Add("@Comments", parameters.Comments);
            queryParameters.Add("@StartDate", parameters.StartDate);
            queryParameters.Add("@EndDate", parameters.EndDate);
            queryParameters.Add("@PhotoOriginalFileName", parameters.PhotoOriginalFileName);
            queryParameters.Add("@PhotoFileName", parameters.PhotoFileName);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SavePhoto", queryParameters);
        }

        public async Task<IEnumerable<Photo_Response>> GetPhotoList(Photo_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PhotoHeaderId", parameters.PhotoHeaderId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<Photo_Response>("GetPhotoList", queryParameters);

            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<Photo_Response?> GetPhotoById(long Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<Photo_Response>("GetPhotoById", queryParameters)).FirstOrDefault();
        }
        #endregion
    }
}
