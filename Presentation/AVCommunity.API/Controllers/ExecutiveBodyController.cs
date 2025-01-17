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
    public class ExecutiveBodyController : ControllerBase
    {
        private ResponseModel _response;
        private readonly IExecutiveBodyRepository _executiveBodyRepository;
        private IFileManager _fileManager;

        public ExecutiveBodyController(IFileManager fileManager, IExecutiveBodyRepository executiveBodyRepository)
        {
            _fileManager = fileManager;
            _executiveBodyRepository = executiveBodyRepository;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveExecutiveBody(ExecutiveBody_Request parameters)
        {
            int result = await _executiveBodyRepository.SaveExecutiveBody(parameters);
             
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
        public async Task<ResponseModel> GetExecutiveBodyList(ExecutiveBody_Search parameters)
        {
            IEnumerable<ExecutiveBody_Response> lstUsers = await _executiveBodyRepository.GetExecutiveBodyList(parameters);
            _response.Data = lstUsers.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetExecutiveBodyById(long Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _executiveBodyRepository.GetExecutiveBodyById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }
    }
}
