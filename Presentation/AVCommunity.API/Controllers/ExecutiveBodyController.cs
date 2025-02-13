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
    public class ExecutiveBodyController : ControllerBase
    {
        private ResponseModel _response;
        private readonly IExecutiveBodyRepository _executiveBodyRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationRepository _notificationRepository;
        private IFileManager _fileManager;

        public ExecutiveBodyController(IFileManager fileManager, IExecutiveBodyRepository executiveBodyRepository, IUserRepository userRepository, INotificationRepository notificationRepository)
        {
            _fileManager = fileManager;
            _executiveBodyRepository = executiveBodyRepository;
            _userRepository = userRepository;
            _notificationRepository = notificationRepository;

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

                #region Notification
                if (result > 0 && parameters.Id == 0)
                {
                    int districtId = 0;
                    int villageId = 0;

                    var vUserDetails = await _executiveBodyRepository.GetExecutiveBodyById(Convert.ToInt32(result));
                    if (vUserDetails != null)
                    {
                        districtId = Convert.ToInt32(vUserDetails.DistrictId == null ? 0 : vUserDetails.DistrictId);
                        villageId = Convert.ToInt32(vUserDetails.VillageId == null ? 0 : vUserDetails.VillageId);

                        if (districtId > 0 || villageId > 0)
                        {
                            string notifyMessage = String.Format(
                                @"
                                પ્રિય,

                                કાસુન્દ્રા પરિવાર તરફથી શુભેચ્છાઓ! 

                                નવી કાર્યકારી સમિતિની સૂચિ નીચે મુજબ છે:

                                નામ: - {0}
                                પદ: - {1}
                                મોબાઇલ નંબર: - {2}
                                ", vUserDetails.ExecutiveBodyName, vUserDetails.PositionName, vUserDetails.MobileNumber);

                            var vAdminVillageUser = await _userRepository.GetAdminVillageByEmployeeId(0, villageId); // Village Wise Admin User 
                            var vAdminVillageUserList = vAdminVillageUser.ToList().Select(x => x.EmployeeId);

                            var vAdminSearch = new Admin_Search();
                            vAdminSearch.AdminDistrictId = districtId;

                            var vAdminUserByDistrict = await _userRepository.GetAdminList(vAdminSearch); // Admin District Wise Admin User 
                            var vAdminUserFinalList = vAdminUserByDistrict.Where(x => vAdminVillageUserList.Contains(x.Id)).ToList();
                            if (vAdminUserFinalList.Count > 0)
                            {
                                foreach (var usr in vAdminUserFinalList)
                                {
                                    var vNotifyObj = new Notification_Request()
                                    {
                                        Subject = "Executive Body",
                                        SendTo = "Admin",
                                        //CustomerId = vWorkOrderObj.CustomerId,
                                        //CustomerMessage = NotifyMessage,
                                        EmployeeId = usr.Id,
                                        EmployeeMessage = notifyMessage,
                                        RefValue1 = "",
                                        ReadUnread = false
                                    };

                                    int resultNotification = await _notificationRepository.SaveNotification(vNotifyObj);
                                }
                            }
                        }
                    }
                }
                #endregion
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
