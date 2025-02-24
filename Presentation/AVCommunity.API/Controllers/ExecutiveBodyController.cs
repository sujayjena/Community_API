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
                _response.Message = "Record details saved successfully";

                #region Notification
                if (result > 0 && parameters.Id == 0)
                {
                    int districtId = 0;
                    int villageId = 0;

                    var vUserDetails = await _executiveBodyRepository.GetExecutiveBodyById(Convert.ToInt32(result));
                    if (vUserDetails != null)
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

                        //admin notification
                        var vAdminSearch = new Admin_Search();
                        vAdminSearch.AdminDistrictId = 0;

                        var vAdminUserByDistrict = await _userRepository.GetAdminList(vAdminSearch); // Admin District Wise Admin User 
                        foreach (var usr in vAdminUserByDistrict)
                        {
                            if (usr.Id > 2)
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

                        //user notification
                        var vUserSearch = new User_Search();
                        vUserSearch.DistrictId = districtId;
                        vUserSearch.RegisterUserId = null;
                        vUserSearch.VillageId = "";
                        vUserSearch.StatusId = 0;
                        vUserSearch.IsSplit = false;

                        var vUserList = await _userRepository.GetUserList(vUserSearch);
                        if (vUserList.ToList().Count > 0)
                        {
                            foreach (var usr in vUserList)
                            {
                                var vNotifyObj = new Notification_Request()
                                {
                                    Subject = "Executive Body",
                                    SendTo = "User",
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
