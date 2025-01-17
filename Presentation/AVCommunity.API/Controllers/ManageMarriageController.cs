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
    public class ManageMarriageController : ControllerBase
    {
        private ResponseModel _response;
        private readonly IManageMarriageRepository _manageMarriageRepository;
        private IFileManager _fileManager;

        public ManageMarriageController(IFileManager fileManager, IManageMarriageRepository manageMarriageRepository)
        {
            _fileManager = fileManager;
            _manageMarriageRepository = manageMarriageRepository;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveMarriage(Marriage_Request parameters)
        {
            if (parameters! != null && !string.IsNullOrWhiteSpace(parameters.Attachment_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.Attachment_Base64, "\\Uploads\\Marriage\\", parameters.AttachmentOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.AttachmentFileName = vUploadFile;
                }
            }
             
            int result = await _manageMarriageRepository.SaveMarriage(parameters);

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
        public async Task<ResponseModel> GetMarriageList(Marriage_Search parameters)
        {
            IEnumerable<Marriage_Response> lstUsers = await _manageMarriageRepository.GetMarriageList(parameters);
            _response.Data = lstUsers.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetMarriageById(long Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _manageMarriageRepository.GetMarriageById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> MarriageApproveNReject(Marriage_ApproveNReject parameters)
        {
            if (parameters.Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                int resultExpenseDetails = await _manageMarriageRepository.MarriageApproveNReject(parameters);

                if (resultExpenseDetails == (int)SaveOperationEnums.NoRecordExists)
                {
                    _response.Message = "No record exists";
                }
                else if (resultExpenseDetails == (int)SaveOperationEnums.ReocrdExists)
                {
                    _response.Message = "Record already exists";
                }
                else if (resultExpenseDetails == (int)SaveOperationEnums.NoResult)
                {
                    _response.Message = "Something went wrong, please try again";
                }
                else
                {
                    _response.Message = "Record details saved sucessfully";
                }
            }

            return _response;
        }
    }
}
