using AVCommunity.API.CustomAttributes;
using AVCommunity.Application.Constants;
using AVCommunity.Application.Helpers;
using AVCommunity.Application.Interfaces;
using AVCommunity.Application.Models;
using AVCommunity.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AVCommunity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private ResponseModel _response;
        private ILoginRepository _loginRepository;
        private IJwtUtilsRepository _jwt;
        private readonly IEmployeePermissionRepository _rolePermissionRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationRepository _notificationRepository;

        public LoginController(ILoginRepository loginRepository, IJwtUtilsRepository jwt, IEmployeePermissionRepository rolePermissionRepository, IUserRepository userRepository, INotificationRepository notificationRepository)
        {
            _loginRepository = loginRepository;
            _jwt = jwt;
            _userRepository = userRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _notificationRepository = notificationRepository;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ResponseModel> MobileAppLogin(MobileAppLoginRequestModel parameters)
        {
            LoginByMobileNumberRequestModel loginParameters = new LoginByMobileNumberRequestModel();
            loginParameters.MobileNumber = parameters.MobileNumber;
            loginParameters.Password = parameters.Password;
            loginParameters.MobileUniqueId = parameters.MobileUniqueId;
            loginParameters.Remember = parameters.Remember;
            loginParameters.IsWebOrMobileUser = parameters.IsWebOrMobileUser;
            loginParameters.UserType = "User";

            //_response.Data = await Login(loginParameters);

            var vLoginObj = await Login(loginParameters);

            return vLoginObj;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ResponseModel> Login(LoginByMobileNumberRequestModel parameters)
        {
            (string, DateTime) tokenResponse;
            SessionDataEmployee employeeSessionData;
            UsersLoginSessionData? loginResponse;
            UserLoginHistorySaveParameters loginHistoryParameters;

            if(parameters.IsWebOrMobileUser == "W")
            {
                parameters.UserType = "Admin";
            }

            parameters.Password = EncryptDecryptHelper.EncryptString(parameters.Password);

            loginResponse = await _loginRepository.ValidateUserLoginByEmail(parameters);

            if (loginResponse != null)
            {
                if (loginResponse.IsActive == true && (loginResponse.IsWebUser == true && parameters.IsWebOrMobileUser == "W" || loginResponse.IsMobileUser == true && parameters.IsWebOrMobileUser == "M"))
                {
                    tokenResponse = _jwt.GenerateJwtToken(loginResponse);

                    if (loginResponse.UserId != null)
                    {
                        int districtId = 0;
                        string strVillageIdList = string.Empty;

                        var vRoleList = await _rolePermissionRepository.GetEmployeePermissionById(Convert.ToInt64(loginResponse.UserId));

                        // Notification List
                        var vNotification_SearchObj = new Notification_Search()
                        {
                            NotifyDate = null,
                            UserId = Convert.ToInt32(loginResponse.UserId)
                        };
                        //var vUserNotificationList = await _notificationRepository.GetNotificationList(vNotification_SearchObj);

                        var vUserDetail = await _userRepository.GetAdminById(Convert.ToInt32(loginResponse.UserId));
                        if(vUserDetail != null)
                        {
                            districtId = Convert.ToInt32(vUserDetail.AdminDistrictId);
                        }

                        var vUserVillageMappingDetail = await _userRepository.GetAdminVillageByEmployeeId(EmployeeId: Convert.ToInt32(loginResponse.UserId), VillageId: 0);
                        if (vUserVillageMappingDetail.ToList().Count > 0)
                        {
                            strVillageIdList = string.Join(",", vUserVillageMappingDetail.ToList().OrderBy(x => x.VillageId).Select(x => x.VillageId));
                        }

                        employeeSessionData = new SessionDataEmployee
                        {
                            UserId = loginResponse.UserId,
                            UserCode = loginResponse.UserCode,
                            UserName = loginResponse.UserName,
                            MobileNumber = loginResponse.MobileNumber,
                            EmailId = loginResponse.EmailId,
                            UserType = loginResponse.UserType,
                            DistrictId = districtId,
                            VillageId = strVillageIdList,
                            IsMobileUser = loginResponse.IsMobileUser,
                            IsWebUser = loginResponse.IsWebUser,
                            IsActive = loginResponse.IsActive,
                            Token = tokenResponse.Item1,

                            UserRoleList = vRoleList.ToList(),
                            //UserNotificationList = vUserNotificationList.ToList()
                        };

                        _response.Data = employeeSessionData;
                    }

                    //Login History
                    loginHistoryParameters = new UserLoginHistorySaveParameters
                    {
                        UserId = loginResponse.UserId,
                        UserToken = tokenResponse.Item1,
                        IsLoggedIn = true,
                        IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        DeviceName = HttpContext.Request.Headers["User-Agent"],
                        TokenExpireOn = tokenResponse.Item2,
                        RememberMe = parameters.Remember
                    };

                    await _loginRepository.SaveUserLoginHistory(loginHistoryParameters);

                    _response.Message = MessageConstants.LoginSuccessful;
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = ErrorConstants.InactiveProfileError;
                }
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "Invalid credential, please try again with correct credential";
            }

            return _response;
        }

        [HttpPost]
        [Route("[action]")]
        [CustomAuthorize]
        public async Task<ResponseModel> Logout()
        {
            string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last().SanitizeValue()!;
            //UsersLoginSessionData? sessionData = (UsersLoginSessionData?)HttpContext.Items["SessionData"]!;

            UserLoginHistorySaveParameters logoutParameters = new UserLoginHistorySaveParameters
            {
                UserId = SessionManager.LoggedInUserId,
                UserToken = token,
                IsLoggedIn = false, //To Logout set IsLoggedIn = false
                IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                DeviceName = HttpContext.Request.Headers["User-Agent"],
                TokenExpireOn = DateTime.Now,
                RememberMe = false
            };

            await _loginRepository.SaveUserLoginHistory(logoutParameters);

            _response.Message = MessageConstants.LogoutSuccessful;

            return _response;
        }
    }
}
