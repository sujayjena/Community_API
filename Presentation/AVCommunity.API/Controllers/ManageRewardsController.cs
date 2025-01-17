using AVCommunity.Application.Enums;
using AVCommunity.Application.Helpers;
using AVCommunity.Application.Interfaces;
using AVCommunity.Application.Models;
using AVCommunity.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AVCommunity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageRewardsController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IManageRewardsRepository _manageRewardsRepository;
        private IFileManager _fileManager;

        public ManageRewardsController(IFileManager fileManager, IManageRewardsRepository manageRewardsRepository)
        {
            _fileManager = fileManager;
            _manageRewardsRepository = manageRewardsRepository;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        } 

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveRewards(Rewards_Request parameters)
        {
            if (parameters! != null && !string.IsNullOrWhiteSpace(parameters.Attachment_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.Attachment_Base64, "\\Uploads\\Rewards\\", parameters.AttachmentOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.AttachmentFileName = vUploadFile;
                }
            }

            int result = await _manageRewardsRepository.SaveRewards(parameters);

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
        public async Task<ResponseModel> GetRewardsList(Rewards_Search parameters)
        {
            IEnumerable<Rewards_Response> lstUsers = await _manageRewardsRepository.GetRewardsList(parameters);
            _response.Data = lstUsers.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetRewardsById(long Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _manageRewardsRepository.GetRewardsById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> RewardsApproveNReject(Rewards_ApproveNReject parameters)
        {
            if (parameters.Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                int resultExpenseDetails = await _manageRewardsRepository.RewardsApproveNReject(parameters);

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

