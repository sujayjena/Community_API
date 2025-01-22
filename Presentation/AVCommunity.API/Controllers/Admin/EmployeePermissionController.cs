using AVCommunity.Application.Enums;
using AVCommunity.Application.Interfaces;
using AVCommunity.Application.Models;
using AVCommunity.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace AVCommunity.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeePermissionController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IEmployeePermissionRepository _rolePermissionRepository;

        public EmployeePermissionController(IEmployeePermissionRepository rolePermissionRepository)
        {
            _rolePermissionRepository = rolePermissionRepository;
            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        #region Module Master 

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetModuleMasterList(BaseSearchEntity parameters)
        {
            IEnumerable<ModuleMaster_Response> lstModuleMaster = await _rolePermissionRepository.GetModuleMasterList(parameters);
            _response.Data = lstModuleMaster.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        #endregion

        #region Employee Permission 

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveEmployeePermission(Employee_Permission_Request parameters)
        {
            if (parameters.EmployeeId <= 0)
            {
                _response.Message = "EmployeeId is required";
            }
            else
            {
                int result = await _rolePermissionRepository.SaveEmployeePermission(parameters);

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
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetEmployeePermissionList(Employee_Search_Request parameters)
        {
            if (parameters.EmployeeId <= 0)
            {
                _response.Message = "EmployeeId is required";
            }
            else
            {
                IEnumerable<Employee_Permission_Response> lstEmployeePermissions = await _rolePermissionRepository.GetEmployeePermissionList(parameters);
                _response.Data = lstEmployeePermissions.ToList();
                _response.Total = parameters.Total;
            }
            return _response;
        }

        //[Route("[action]")]
        //[HttpPost]
        //public async Task<ResponseModel> GetEmployeePermissionById(long employeeid)
        //{
        //    if (employeeid <= 0)
        //    {
        //        _response.Message = "EmployeeId is required";
        //    }
        //    else
        //    {
        //        var vResultObj = await _rolePermissionRepository.GetEmployeePermissionById(employeeid);
        //        _response.Data = vResultObj;
        //    }
        //    return _response;
        //}

        #endregion
    }
}
