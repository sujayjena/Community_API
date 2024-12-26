using AVCommunity.Application.Enums;
using AVCommunity.Application.Interfaces;
using AVCommunity.Application.Models;
using AVCommunity.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AVCommunity.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminMasterController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IAdminMasterRepository _adminMasterRepository;

        public AdminMasterController(IAdminMasterRepository AdminMasterRepository)
        {
            _adminMasterRepository = AdminMasterRepository;
            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        #region MeritalStatus

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveMeritalStatus(MeritalStatus_Request parameters)
        {
            int result = await _adminMasterRepository.SaveMeritalStatus(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record is already exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
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
        public async Task<ResponseModel> GetMeritalStatusList(MeritalStatus_Search parameters)
        {
            IEnumerable<MeritalStatus_Response> lstRoles = await _adminMasterRepository.GetMeritalStatusList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetMeritalStatusById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetMeritalStatusById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        /*
        #region Gender

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveGender(Gender_Request parameters)
        {
            int result = await _adminMasterRepository.SaveGender(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record is already exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
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
        public async Task<ResponseModel> GetGenderList(Gender_Search parameters)
        {
            IEnumerable<Gender_Response> lstRoles = await _adminMasterRepository.GetGenderList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetGenderById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetGenderById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Blood Group

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveBloodGroup(BloodGroup_Request parameters)
        {
            int result = await _adminMasterRepository.SaveBloodGroup(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record is already exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
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
        public async Task<ResponseModel> GetBloodGroupList(BloodGroup_Search parameters)
        {
            IEnumerable<BloodGroup_Response> lstRoles = await _adminMasterRepository.GetBloodGroupList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetBloodGroupById(long Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetBloodGroupById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        */
    }
}
