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
    public class ManageBroadCastController : ControllerBase
    {
        private ResponseModel _response;
        private readonly IManageBroadCastRepository _manageBroadCastRepository;
        private IFileManager _fileManager;

        public ManageBroadCastController(IFileManager fileManager, IManageBroadCastRepository manageBroadCastRepository)
        {
            _fileManager = fileManager;
            _manageBroadCastRepository = manageBroadCastRepository;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveBroadCast(BroadCast_Request parameters)
        {
            int result = await _manageBroadCastRepository.SaveBroadCast(parameters);

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
                _response.Message = "Record details saved successfully";
            }

            _response.Id = result;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetBroadCastList(BroadCast_Search parameters)
        {
            IEnumerable<BroadCast_Response> lstUsers = await _manageBroadCastRepository.GetBroadCastList(parameters);
            _response.Data = lstUsers.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetBroadCastById(long Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _manageBroadCastRepository.GetBroadCastById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }
        
    }
}
