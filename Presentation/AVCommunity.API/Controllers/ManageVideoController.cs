using AVCommunity.Application.Enums;
using AVCommunity.Application.Helpers;
using AVCommunity.Application.Interfaces;
using AVCommunity.Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AVCommunity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageVideoController : ControllerBase
    {
        private ResponseModel _response;
        private readonly IManageVideoRepository _manageVideoRepository;
        private IFileManager _fileManager;

        public ManageVideoController(IFileManager fileManager, IManageVideoRepository manageVideoRepository)
        {
            _fileManager = fileManager;
            _manageVideoRepository = manageVideoRepository;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        #region Video Header

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveVideoHeader(VideoHeader_Request parameters)
        {
            int result = await _manageVideoRepository.SaveVideoHeader(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
                _response.IsSuccess = false;
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record is already exists";
                _response.IsSuccess = false;
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
                _response.IsSuccess = false;
            }
            else
            {
                _response.Message = "Record details saved sucessfully";
            }

            _response.Id = result;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetVideoHeaderList(VideoHeader_Search parameters)
        {
            IEnumerable<VideoHeader_Response> lstUsers = await _manageVideoRepository.GetVideoHeaderList(parameters);
            _response.Data = lstUsers.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetVideoHeaderById(long Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _manageVideoRepository.GetVideoHeaderById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion
    }
}
