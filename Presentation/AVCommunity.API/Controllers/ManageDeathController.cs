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
    public class ManageDeathController : ControllerBase
    {
        private ResponseModel _response;
        private readonly IManageDeathRepository _manageDeathRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationRepository _notificationRepository;
        private IFileManager _fileManager;

        public ManageDeathController(IFileManager fileManager, IManageDeathRepository manageDeathRepository, IUserRepository userRepository, INotificationRepository notificationRepository)
        {
            _fileManager = fileManager;
            _manageDeathRepository = manageDeathRepository;
            _userRepository = userRepository;
            _notificationRepository = notificationRepository;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveDeath(Death_Request parameters)
        {
            if (parameters! != null && !string.IsNullOrWhiteSpace(parameters.Attachment_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.Attachment_Base64, "\\Uploads\\Death\\", parameters.AttachmentOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.AttachmentFileName = vUploadFile;
                }
            }

            int result = await _manageDeathRepository.SaveDeath(parameters);

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

                    var vUserDetails = await _userRepository.GetUserById(Convert.ToInt32(parameters.RegisterUserId));
                    if (vUserDetails != null)
                    {
                        districtId = Convert.ToInt32(vUserDetails.DistrictId == null ? 0 : vUserDetails.DistrictId);
                        villageId = Convert.ToInt32(vUserDetails.VillageId == null ? 0 : vUserDetails.VillageId);

                        if (districtId > 0 || villageId > 0)
                        {
                            string userName = string.Concat(vUserDetails.FirstName, " ", vUserDetails.MiddleName, " ", vUserDetails.Surname);
                            string notifyMessage = String.Format(@"પ્રિય,કાસુન્દ્રા પરિવાર તરફથી શુભેચ્છાઓ! વપરાશકર્તાએ મૃત્યુ થયા ની જાણકારી આપી છે। {0} અને {1}।", userName.Trim(), vUserDetails.MobileNumber);

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
                                        Subject = "Manage Death",
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
        public async Task<ResponseModel> GetDeathList(Death_Search parameters)
        {
            IEnumerable<Death_Response> lstUsers = await _manageDeathRepository.GetDeathList(parameters);
            _response.Data = lstUsers.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetDeathById(long Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _manageDeathRepository.GetDeathById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> DeathApproveNReject(Death_ApproveNReject parameters)
        {
            if (parameters.Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                int resultExpenseDetails = await _manageDeathRepository.DeathApproveNReject(parameters);

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

                    #region Notification
                    if (parameters.StatusId == 2)
                    {
                        var vMarriegeDetails = await _manageDeathRepository.GetDeathById(Convert.ToInt32(parameters.Id));
                        if (vMarriegeDetails != null)
                        {
                            var vUserDetails = await _userRepository.GetUserById(Convert.ToInt32(vMarriegeDetails.ModifiedBy));
                            if (vUserDetails != null)
                            {
                                string userName = string.Concat(vUserDetails.FirstName, " ", vUserDetails.MiddleName, " ", vUserDetails.Surname);
                                string notifyMessage = String.Format(@"પ્રિય,કાસુન્દ્રા પરિવાર તરફથી શુભેચ્છાઓ! તમારી મૃત્યુની વિનંતી સફળતાપૂર્વક {0} દ્વારા સ્વીકારવામાં આવી છે.", userName.Trim());

                                var vNotifyObj = new Notification_Request()
                                {
                                    Subject = "Death Accept",
                                    SendTo = "Registered User",
                                    //CustomerId = vWorkOrderObj.CustomerId,
                                    //CustomerMessage = NotifyMessage,
                                    EmployeeId = vMarriegeDetails.RegisterUserId,
                                    EmployeeMessage = notifyMessage,
                                    RefValue1 = "",
                                    ReadUnread = false
                                };

                                int resultNotification = await _notificationRepository.SaveNotification(vNotifyObj);
                            }
                        }
                    }
                    #endregion
                }
            }

            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetDeathRemarkLogListById(DeathRemarkLog_Search parameters)
        {
            if (parameters.DeathId <= 0)
            {
                _response.Message = "Death Id is required";
            }
            else
            {
                var vResultObj = await _manageDeathRepository.GetDeathRemarkLogListById(parameters);
                _response.Data = vResultObj;
            }
            return _response;
        }
    }
}
