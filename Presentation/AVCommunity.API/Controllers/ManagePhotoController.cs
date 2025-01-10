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
    public class ManagePhotoController : ControllerBase
    {
        private ResponseModel _response;
        private readonly IManagePhotoRepository _managePhotoRepository;
        private IFileManager _fileManager;

        public ManagePhotoController(IFileManager fileManager, IManagePhotoRepository managePhotoRepository)
        {
            _fileManager = fileManager;
            _managePhotoRepository = managePhotoRepository;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        #region Photo Header

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SavePhotoHeader(PhotoHeader_Request parameters)
        {
            int result = await _managePhotoRepository.SavePhotoHeader(parameters);

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
        public async Task<ResponseModel> GetPhotoHeaderList(PhotoHeader_Search parameters)
        {
            IEnumerable<PhotoHeader_Response> lstUsers = await _managePhotoRepository.GetPhotoHeaderList(parameters);
            _response.Data = lstUsers.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetPhotoHeaderById(long Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _managePhotoRepository.GetPhotoHeaderById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Photo

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SavePhoto(Photo_Request parameters)
        {
            if (parameters! != null && !string.IsNullOrWhiteSpace(parameters.Photo_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.Photo_Base64, "\\Uploads\\Photo\\", parameters.PhotoOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.PhotoFileName = vUploadFile;
                }
            }

            int result = await _managePhotoRepository.SavePhoto(parameters);

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
        public async Task<ResponseModel> GetPhotoList(Photo_Search parameters)
        {
            IEnumerable<Photo_Response> lstUsers = await _managePhotoRepository.GetPhotoList(parameters);
            _response.Data = lstUsers.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetPhotoById(long Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _managePhotoRepository.GetPhotoById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion
    }
}