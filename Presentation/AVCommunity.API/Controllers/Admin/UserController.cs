using AVCommunity.Application.Enums;
using AVCommunity.Application.Helpers;
using AVCommunity.Application.Interfaces;
using AVCommunity.Application.Models;
using AVCommunity.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Text;
using AVCommunity.API.CustomAttributes;

namespace AVCommunity.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IUserRepository _userRepository;
        private readonly ITerritoryRepository _territoryRepository;
        private readonly IAdminMasterRepository _adminMasterRepository;
        private readonly INotificationRepository _notificationRepository;
        private IFileManager _fileManager;

        public UserController(IUserRepository userRepository, IFileManager fileManager, ITerritoryRepository territoryRepository, IAdminMasterRepository adminMasterRepository, INotificationRepository notificationRepository)
        {
            _userRepository = userRepository;
            _fileManager = fileManager;
            _territoryRepository = territoryRepository;
            _adminMasterRepository = adminMasterRepository;
            _notificationRepository = notificationRepository;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        #region Admin 

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveAdmin(Admin_Request parameters)
        {
            int result = await _userRepository.SaveAdmin(parameters);

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

                #region // Add/Update Admin Village

                // Delete Old Admin Village

                var vAdminVillageDELETEObj = new AdminVillage_Request()
                {
                    Action = "DELETE",
                    UserId = result,
                    VillageId = 0
                };
                int resultBranchMappingDELETE = await _userRepository.SaveAdminVillage(vAdminVillageDELETEObj);


                // Add new mapping of employee
                foreach (var vVillageitem in parameters.AdminVillageList)
                {
                    var vAdminVillageObj = new AdminVillage_Request()
                    {
                        Action = "INSERT",
                        UserId = result,
                        VillageId = vVillageitem.VillageId
                    };

                    int resultAdminVillage = await _userRepository.SaveAdminVillage(vAdminVillageObj);
                }

                #endregion
            }

            _response.Id = result;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetAdminList(Admin_Search parameters)
        {
            IEnumerable<Admin_Response> lstUsers = await _userRepository.GetAdminList(parameters);
            _response.Data = lstUsers.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetAdminById(long Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _userRepository.GetAdminById(Id);
                if (vResultObj != null)
                {
                    var vAdminVillageObj = await _userRepository.GetAdminVillageByEmployeeId(vResultObj.Id, 0);

                    foreach (var item in vAdminVillageObj)
                    {
                        var vVillageObj = await _territoryRepository.GetVillageById(Convert.ToInt32(item.VillageId));
                        var vBrMapResOnj = new AdminVillage_Response()
                        {
                            Id = item.Id,
                            UserId = vResultObj.Id,
                            VillageId = item.VillageId,
                            VillageName = vVillageObj != null ? vVillageObj.VillageName : string.Empty,
                        };

                        vResultObj.AdminVillageList.Add(vBrMapResOnj);
                    }
                }
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region User 

        [Route("[action]")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ResponseModel> SaveUser(User_Request parameters)
        {
            int result = await _userRepository.SaveUser(parameters);

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

                #region // Add/Update Industry

                // Delete Old Industry

                var vUserIndustryDELETEObj = new UserIndustry_Request()
                {
                    Action = "DELETE",
                    UserId = result,
                    IndustryId = 0
                };
                int resultUserIndustryDELETE = await _userRepository.SaveUserIndustry(vUserIndustryDELETEObj);


                // Add new mapping of employee
                foreach (var vIndustryitem in parameters.UserIndustryList)
                {
                    var vUserIndustryObj = new UserIndustry_Request()
                    {
                        Action = "INSERT",
                        UserId = result,
                        IndustryId = vIndustryitem.IndustryId
                    };

                    int resultUserIndustry = await _userRepository.SaveUserIndustry(vUserIndustryObj);
                }

                #endregion

                #region Notification
                if (result > 0 && parameters.Id == 0)
                {
                    int districtId = 0;
                    int villageId = 0;
                    string userName = "";
                    string userMobileNo = "";

                    //User
                    var vUserDetails = await _userRepository.GetUserById(result);
                    if (vUserDetails != null)
                    {
                        districtId = Convert.ToInt32(vUserDetails.DistrictId == null ? 0 : vUserDetails.DistrictId);
                        villageId = Convert.ToInt32(vUserDetails.VillageId == null ? 0 : vUserDetails.VillageId);
                        userName = string.Concat(vUserDetails.FirstName, " ", vUserDetails.MiddleName, " ", vUserDetails.LastName);
                        userMobileNo = vUserDetails.MobileNumber;

                        string notifyMessage = String.Format(@"પ્રિય, કાસુન્દ્રા પરિવાર તરફથી શુભેચ્છાઓ! અમારી સાથે જોડાવા માટે આભાર! તમારું ખાતું સફળતાપૂર્વક આ મોબાઇલ નંબર - {0} સાથે બનાવવામાં આવ્યું છે. જો તમને કોઈ પ્રશ્નો હોય, તો કૃપા કરીને સંબંધિત સંચાલક સાથે સંપર્ક કરો।", vUserDetails.MobileNumber);

                        var vNotifyObj = new Notification_Request()
                        {
                            Subject = "Manage User",
                            SendTo = "User",
                            //CustomerId = vWorkOrderObj.CustomerId,
                            //CustomerMessage = NotifyMessage,
                            EmployeeId = vUserDetails.Id,
                            EmployeeMessage = notifyMessage,
                            RefValue1 = "",
                            ReadUnread = false
                        };

                        int resultNotification = await _notificationRepository.SaveNotification(vNotifyObj);
                    }

                    //Admin
                    if (districtId > 0 || villageId > 0)
                    {
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
                                string notifyMessage = String.Format(@"પ્રિય,કાસુન્દ્રા પરિવાર તરફથી શુભેચ્છાઓ! નવો વપરાશકર્તા {0} મોબાઇલ નંબર {1} સાથે બનાવવામાં આવ્યો છે।", userName.Trim(), userMobileNo);

                                var vNotifyObj = new Notification_Request()
                                {
                                    Subject = "Manage User",
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
                #endregion
            }

            _response.Id = result;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetUserList(User_Search parameters)
        {
            IEnumerable<User_Response> lstUsers = await _userRepository.GetUserList(parameters);
            _response.Data = lstUsers.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetUserById(long Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _userRepository.GetUserById(Id);
                if (vResultObj != null)
                {
                    var vUserIndustryObj = await _userRepository.GetUserIndustryByEmployeeId(vResultObj.Id, 0);

                    foreach (var item in vUserIndustryObj)
                    {
                        var vIndustryObj = await _adminMasterRepository.GetIndustryById(Convert.ToInt32(item.IndustryId));
                        var vBrMapResOnj = new UserIndustry_Response()
                        {
                            Id = item.Id,
                            UserId = vResultObj.Id,
                            IndustryId = item.IndustryId,
                            IndustryName = vIndustryObj != null ? vIndustryObj.IndustryName : string.Empty,
                        };

                        vResultObj.UserIndustryList.Add(vBrMapResOnj);
                    }
                }
                _response.Data = vResultObj;
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ExportUserData(User_Search parameters)
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            IEnumerable<User_Response> lstSizeObj = await _userRepository.GetUserList(parameters);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("User");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "Sr.No";
                    WorkSheet1.Cells[1, 2].Value = "Full Name";
                    WorkSheet1.Cells[1, 3].Value = "Surname Of Mosal";
                    WorkSheet1.Cells[1, 4].Value = "Mobile Number";
                    WorkSheet1.Cells[1, 5].Value = "Relation";
                    WorkSheet1.Cells[1, 6].Value = "Gender";
                    WorkSheet1.Cells[1, 7].Value = "Is Married";
                    WorkSheet1.Cells[1, 8].Value = "Native";
                    WorkSheet1.Cells[1, 9].Value = "Date Of Birth";
                    WorkSheet1.Cells[1, 10].Value = "Age";
                    WorkSheet1.Cells[1, 11].Value = "Higher Study";
                    WorkSheet1.Cells[1, 12].Value = "Occupation";
                    WorkSheet1.Cells[1, 13].Value = "Current Address";
                    WorkSheet1.Cells[1, 14].Value = "State";
                    WorkSheet1.Cells[1, 15].Value = "District";
                    WorkSheet1.Cells[1, 16].Value = "Village";
                    WorkSheet1.Cells[1, 17].Value = "Pincode";
                    WorkSheet1.Cells[1, 18].Value = "Industry";
                    WorkSheet1.Cells[1, 19].Value = "Business Address";
                    //WorkSheet1.Cells[1, 17].Value = "State";
                    //WorkSheet1.Cells[1, 18].Value = "District";
                    //WorkSheet1.Cells[1, 19].Value = "Village";
                    //WorkSheet1.Cells[1, 20].Value = "Pincode";
                    WorkSheet1.Cells[1, 20].Value = "IsActive";

                    recordIndex = 2;

                    int i = 1;
                    foreach (var items in lstSizeObj)
                    {
                        string fullName = items.FirstName + " " + items.MiddleName + " " + items.LastName;
                        WorkSheet1.Cells[recordIndex, 1].Value = i.ToString();
                        WorkSheet1.Cells[recordIndex, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        WorkSheet1.Cells[recordIndex, 2].Value = fullName.Trim();
                        WorkSheet1.Cells[recordIndex, 3].Value = items.Surname;
                        WorkSheet1.Cells[recordIndex, 4].Value = items.MobileNumber;
                        WorkSheet1.Cells[recordIndex, 5].Value = items.RelationName;
                        WorkSheet1.Cells[recordIndex, 6].Value = items.GenderName;
                        WorkSheet1.Cells[recordIndex, 7].Value = items.MeritalStatusName;
                        WorkSheet1.Cells[recordIndex, 8].Value = items.NativeName;
                        WorkSheet1.Cells[recordIndex, 9].Value = items.DateOfBirth.HasValue ? items.DateOfBirth.Value.ToString("dd/MM/yyyy") : string.Empty;
                        WorkSheet1.Cells[recordIndex, 10].Value = items.Age;
                        WorkSheet1.Cells[recordIndex, 11].Value = items.HigherStudyName;
                        WorkSheet1.Cells[recordIndex, 12].Value = items.OccupationName;
                        WorkSheet1.Cells[recordIndex, 13].Value = items.CurrentAddress;
                        WorkSheet1.Cells[recordIndex, 14].Value = items.StateName;
                        WorkSheet1.Cells[recordIndex, 15].Value = items.DistrictName;
                        WorkSheet1.Cells[recordIndex, 16].Value = items.VillageName;
                        WorkSheet1.Cells[recordIndex, 17].Value = items.Pincode;
                        WorkSheet1.Cells[recordIndex, 18].Value = items.IndustryName;
                        WorkSheet1.Cells[recordIndex, 19].Value = items.BusinessAddress;
                        //WorkSheet1.Cells[recordIndex, 17].Value = items.StateName;
                        //WorkSheet1.Cells[recordIndex, 18].Value = items.DistrictName;
                        //WorkSheet1.Cells[recordIndex, 19].Value = items.VillageName;
                        //WorkSheet1.Cells[recordIndex, 20].Value = items.Pincode;
                        WorkSheet1.Cells[recordIndex, 20].Value = items.IsActive == true ? "Active" : "Inactive";

                        recordIndex += 1;

                        //member of family list
                        var vUser_Search = new User_Search();
                        vUser_Search.RegisterUserId = items.Id;
                        vUser_Search.StatusId = items.StatusId;
                        vUser_Search.DistrictId = items.DistrictId;
                        vUser_Search.VillageId = items.VillageId.ToString();
                        vUser_Search.IsSplit = false;

                        int j = 1;
                        IEnumerable<User_Response> lstMUserObj = await _userRepository.GetUserList(vUser_Search);
                        foreach (var mitems in lstMUserObj)
                        {
                            string mfullName = mitems.FirstName + " " + mitems.MiddleName + " " + mitems.LastName;
                            WorkSheet1.Cells[recordIndex, 1].Value = i + "." + j;
                            WorkSheet1.Cells[recordIndex, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            WorkSheet1.Cells[recordIndex, 2].Value = mfullName.Trim();
                            WorkSheet1.Cells[recordIndex, 3].Value = mitems.Surname;
                            WorkSheet1.Cells[recordIndex, 4].Value = mitems.MobileNumber;
                            WorkSheet1.Cells[recordIndex, 5].Value = mitems.RelationName;
                            WorkSheet1.Cells[recordIndex, 6].Value = mitems.GenderName;
                            WorkSheet1.Cells[recordIndex, 7].Value = mitems.MeritalStatusName;
                            WorkSheet1.Cells[recordIndex, 8].Value = mitems.NativeName;
                            WorkSheet1.Cells[recordIndex, 9].Value = mitems.DateOfBirth.HasValue ? mitems.DateOfBirth.Value.ToString("dd/MM/yyyy") : string.Empty;
                            WorkSheet1.Cells[recordIndex, 10].Value = mitems.Age;
                            WorkSheet1.Cells[recordIndex, 11].Value = mitems.HigherStudyName;
                            WorkSheet1.Cells[recordIndex, 12].Value = mitems.OccupationName;
                            WorkSheet1.Cells[recordIndex, 13].Value = mitems.CurrentAddress;
                            WorkSheet1.Cells[recordIndex, 14].Value = mitems.StateName;
                            WorkSheet1.Cells[recordIndex, 15].Value = mitems.DistrictName;
                            WorkSheet1.Cells[recordIndex, 16].Value = mitems.VillageName;
                            WorkSheet1.Cells[recordIndex, 17].Value = mitems.Pincode;
                            WorkSheet1.Cells[recordIndex, 18].Value = items.IndustryName;
                            WorkSheet1.Cells[recordIndex, 19].Value = mitems.CurrentAddress;
                            //WorkSheet1.Cells[recordIndex, 17].Value = mitems.StateName;
                            //WorkSheet1.Cells[recordIndex, 18].Value = mitems.DistrictName;
                            //WorkSheet1.Cells[recordIndex, 19].Value = mitems.VillageName;
                            //WorkSheet1.Cells[recordIndex, 20].Value = mitems.Pincode;
                            WorkSheet1.Cells[recordIndex, 20].Value = mitems.IsActive == true ? "Active" : "Inactive";

                            recordIndex += 1;

                            j++;
                        }

                        i++;
                    }

                    WorkSheet1.Column(1).AutoFit();
                    WorkSheet1.Column(2).AutoFit();
                    WorkSheet1.Column(3).AutoFit();
                    WorkSheet1.Column(4).AutoFit();
                    WorkSheet1.Column(5).AutoFit();
                    WorkSheet1.Column(6).AutoFit();
                    WorkSheet1.Column(7).AutoFit();
                    WorkSheet1.Column(8).AutoFit();
                    WorkSheet1.Column(9).AutoFit();
                    WorkSheet1.Column(10).AutoFit();
                    WorkSheet1.Column(11).AutoFit();
                    WorkSheet1.Column(12).AutoFit();
                    WorkSheet1.Column(13).AutoFit();
                    WorkSheet1.Column(14).AutoFit();
                    WorkSheet1.Column(15).AutoFit();
                    WorkSheet1.Column(16).AutoFit();
                    WorkSheet1.Column(17).AutoFit();
                    WorkSheet1.Column(18).AutoFit();
                    WorkSheet1.Column(19).AutoFit();
                    WorkSheet1.Column(20).AutoFit();

                    excelExportData.SaveAs(msExportDataFile);
                    msExportDataFile.Position = 0;
                    result = msExportDataFile.ToArray();
                }
            }

            if (result != null)
            {
                _response.Data = result;
                _response.IsSuccess = true;
                _response.Message = "Exported successfully";
            }

            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetChampionList(Champion_Search parameters)
        {
            IEnumerable<Champion_Response> lstUsers = await _userRepository.GetChampionList(parameters);
            _response.Data = lstUsers.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveSplit(Split_Request parameters)
        {
            int result = await _userRepository.SaveSplit(parameters);

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
                if (result > 0)
                {
                    var vSplitUser = parameters.EmployeeId.Split(',').ToList();
                    foreach (var usrEmployeeId in vSplitUser)
                    {
                        int districtId = 0;
                        int villageId = 0;

                        var vUserDetails = await _userRepository.GetUserById(Convert.ToInt32(usrEmployeeId));
                        if (vUserDetails != null)
                        {
                            districtId = Convert.ToInt32(vUserDetails.DistrictId == null ? 0 : vUserDetails.DistrictId);
                            villageId = Convert.ToInt32(vUserDetails.VillageId == null ? 0 : vUserDetails.VillageId);

                            if (districtId > 0 || villageId > 0)
                            {
                                string userName = string.Concat(vUserDetails.FirstName, " ", vUserDetails.MiddleName, " ", vUserDetails.LastName);
                                string notifyMessage = String.Format(@"પ્રિય,કાસુન્દ્રા પરિવાર તરફથી શુભેચ્છાઓ! વપરાશકર્તાએ {0} અને {1} ના વિભાજન માટે વિનંતી કરી છે।", userName.Trim(), vUserDetails.MobileNumber);

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
                                            Subject = "Manage Split",
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
                }
                #endregion
            }

            _response.Id = result;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetGlobalUserList(GlobalUser_Search parameters)
        {
            IEnumerable<GloberUser_Response> lstUsers = await _userRepository.GetGlobalUserList(parameters);
            foreach(var mUsers in lstUsers)
            {
                var searchUser = new User_Search();
                searchUser.RegisterUserId = mUsers.Id;
                searchUser.DistrictId = 0;
                searchUser.VillageId = "";
                searchUser.IsSplit = false;
                searchUser.StatusId = 0;

                var memberUserList = await _userRepository.GetUserList(searchUser);
                mUsers.MemberUserList = memberUserList.ToList();
            }

            _response.Data = lstUsers.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ExportDirectoryData(GlobalUser_Search parameters)
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            IEnumerable<GloberUser_Response> lstSizeObj = await _userRepository.GetGlobalUserList(parameters);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("DirectoryUser");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "Sr.No";
                    WorkSheet1.Cells[1, 2].Value = "Full Name";
                    WorkSheet1.Cells[1, 3].Value = "Surname Of Mosal";
                    WorkSheet1.Cells[1, 4].Value = "Mobile Number";
                    WorkSheet1.Cells[1, 5].Value = "Relation";
                    WorkSheet1.Cells[1, 6].Value = "Gender";
                    WorkSheet1.Cells[1, 7].Value = "Is Married";
                    WorkSheet1.Cells[1, 8].Value = "Native";
                    WorkSheet1.Cells[1, 9].Value = "Date Of Birth";
                    WorkSheet1.Cells[1, 10].Value = "Age";
                    WorkSheet1.Cells[1, 11].Value = "Higher Study";
                    WorkSheet1.Cells[1, 12].Value = "Occupation";
                    WorkSheet1.Cells[1, 13].Value = "Current Address";
                    WorkSheet1.Cells[1, 14].Value = "State";
                    WorkSheet1.Cells[1, 15].Value = "District";
                    WorkSheet1.Cells[1, 16].Value = "Village";
                    WorkSheet1.Cells[1, 17].Value = "Pincode";
                    WorkSheet1.Cells[1, 18].Value = "Industry";
                    WorkSheet1.Cells[1, 19].Value = "Business Address";
                    //WorkSheet1.Cells[1, 17].Value = "State";
                    //WorkSheet1.Cells[1, 18].Value = "District";
                    //WorkSheet1.Cells[1, 19].Value = "Village";
                    //WorkSheet1.Cells[1, 20].Value = "Pincode";
                    WorkSheet1.Cells[1, 20].Value = "IsActive";

                    recordIndex = 2;

                    int i = 1;
                    foreach (var items in lstSizeObj)
                    {
                        string fullName = items.FirstName + " " + items.MiddleName + " " + items.LastName;
                        WorkSheet1.Cells[recordIndex, 1].Value = i.ToString();
                        WorkSheet1.Cells[recordIndex, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        WorkSheet1.Cells[recordIndex, 2].Value = fullName.Trim();
                        WorkSheet1.Cells[recordIndex, 3].Value = items.Surname;
                        WorkSheet1.Cells[recordIndex, 4].Value = items.MobileNumber;
                        WorkSheet1.Cells[recordIndex, 5].Value = items.RelationName;
                        WorkSheet1.Cells[recordIndex, 6].Value = items.GenderName;
                        WorkSheet1.Cells[recordIndex, 7].Value = items.MeritalStatusName;
                        WorkSheet1.Cells[recordIndex, 8].Value = items.NativeName;
                        WorkSheet1.Cells[recordIndex, 9].Value = items.DateOfBirth.HasValue ? items.DateOfBirth.Value.ToString("dd/MM/yyyy") : string.Empty;
                        WorkSheet1.Cells[recordIndex, 10].Value = items.Age;
                        WorkSheet1.Cells[recordIndex, 11].Value = items.HigherStudyName;
                        WorkSheet1.Cells[recordIndex, 12].Value = items.OccupationName;
                        WorkSheet1.Cells[recordIndex, 13].Value = items.CurrentAddress;
                        WorkSheet1.Cells[recordIndex, 14].Value = items.StateName;
                        WorkSheet1.Cells[recordIndex, 15].Value = items.DistrictName;
                        WorkSheet1.Cells[recordIndex, 16].Value = items.VillageName;
                        WorkSheet1.Cells[recordIndex, 17].Value = items.Pincode;
                        WorkSheet1.Cells[recordIndex, 18].Value = items.IndustryName;
                        WorkSheet1.Cells[recordIndex, 19].Value = items.BusinessAddress;
                        //WorkSheet1.Cells[recordIndex, 17].Value = items.StateName;
                        //WorkSheet1.Cells[recordIndex, 18].Value = items.DistrictName;
                        //WorkSheet1.Cells[recordIndex, 19].Value = items.VillageName;
                        //WorkSheet1.Cells[recordIndex, 20].Value = items.Pincode;
                        WorkSheet1.Cells[recordIndex, 20].Value = items.IsActive == true ? "Active" : "Inactive";

                        recordIndex += 1;

                        //member of family list
                        var vUser_Search = new GlobalUser_Search();
                        vUser_Search.EmployeeId = 0;
                        vUser_Search.BusinessId = items.OccupationId;
                        vUser_Search.DistrictId = items.DistrictId;
                        vUser_Search.VillageId = items.VillageId;
                        vUser_Search.AppType = "W";
                        vUser_Search.RegisterUserId = items.Id;
                        vUser_Search.SearchText = "";
                        vUser_Search.IsActive = true;

                        int j = 1;
                        IEnumerable<GloberUser_Response> lstMUserObj = await _userRepository.GetGlobalUserList(vUser_Search);
                        foreach (var mitems in lstMUserObj)
                        {
                            string mfullName = mitems.FirstName + " " + mitems.MiddleName + " " + mitems.LastName;
                            WorkSheet1.Cells[recordIndex, 1].Value = i + "." + j;
                            WorkSheet1.Cells[recordIndex, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            WorkSheet1.Cells[recordIndex, 2].Value = mfullName.Trim();
                            WorkSheet1.Cells[recordIndex, 3].Value = mitems.Surname;
                            WorkSheet1.Cells[recordIndex, 4].Value = mitems.MobileNumber;
                            WorkSheet1.Cells[recordIndex, 5].Value = mitems.RelationName;
                            WorkSheet1.Cells[recordIndex, 6].Value = mitems.GenderName;
                            WorkSheet1.Cells[recordIndex, 7].Value = mitems.MeritalStatusName;
                            WorkSheet1.Cells[recordIndex, 8].Value = mitems.NativeName;
                            WorkSheet1.Cells[recordIndex, 9].Value = mitems.DateOfBirth.HasValue ? mitems.DateOfBirth.Value.ToString("dd/MM/yyyy") : string.Empty;
                            WorkSheet1.Cells[recordIndex, 10].Value = mitems.Age;
                            WorkSheet1.Cells[recordIndex, 11].Value = mitems.HigherStudyName;
                            WorkSheet1.Cells[recordIndex, 12].Value = mitems.OccupationName;
                            WorkSheet1.Cells[recordIndex, 13].Value = mitems.CurrentAddress;
                            WorkSheet1.Cells[recordIndex, 14].Value = mitems.StateName;
                            WorkSheet1.Cells[recordIndex, 15].Value = mitems.DistrictName;
                            WorkSheet1.Cells[recordIndex, 16].Value = mitems.VillageName;
                            WorkSheet1.Cells[recordIndex, 17].Value = mitems.Pincode;
                            WorkSheet1.Cells[recordIndex, 18].Value = items.IndustryName;
                            WorkSheet1.Cells[recordIndex, 19].Value = mitems.CurrentAddress;
                            //WorkSheet1.Cells[recordIndex, 17].Value = mitems.StateName;
                            //WorkSheet1.Cells[recordIndex, 18].Value = mitems.DistrictName;
                            //WorkSheet1.Cells[recordIndex, 19].Value = mitems.VillageName;
                            //WorkSheet1.Cells[recordIndex, 20].Value = mitems.Pincode;
                            WorkSheet1.Cells[recordIndex, 20].Value = mitems.IsActive == true ? "Active" : "Inactive";

                            recordIndex += 1;

                            j++;
                        }

                        i++;
                    }

                    WorkSheet1.Column(1).AutoFit();
                    WorkSheet1.Column(2).AutoFit();
                    WorkSheet1.Column(3).AutoFit();
                    WorkSheet1.Column(4).AutoFit();
                    WorkSheet1.Column(5).AutoFit();
                    WorkSheet1.Column(6).AutoFit();
                    WorkSheet1.Column(7).AutoFit();
                    WorkSheet1.Column(8).AutoFit();
                    WorkSheet1.Column(9).AutoFit();
                    WorkSheet1.Column(10).AutoFit();
                    WorkSheet1.Column(11).AutoFit();
                    WorkSheet1.Column(12).AutoFit();
                    WorkSheet1.Column(13).AutoFit();
                    WorkSheet1.Column(14).AutoFit();
                    WorkSheet1.Column(15).AutoFit();
                    WorkSheet1.Column(16).AutoFit();
                    WorkSheet1.Column(17).AutoFit();
                    WorkSheet1.Column(18).AutoFit();
                    WorkSheet1.Column(19).AutoFit();
                    WorkSheet1.Column(20).AutoFit();

                    excelExportData.SaveAs(msExportDataFile);
                    msExportDataFile.Position = 0;
                    result = msExportDataFile.ToArray();
                }
            }

            if (result != null)
            {
                _response.Data = result;
                _response.IsSuccess = true;
                _response.Message = "Exported successfully";
            }

            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ResponseModel> ForgotPassword(ForgotPassword_Request parameters)
        {
            int result = await _userRepository.ForgotPassword(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "User data does not exist please enter correct details";
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
        public async Task<ResponseModel> UserApproveNReject(User_ApproveNReject parameters)
        {
            if (parameters.Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                int resultExpenseDetails = await _userRepository.UserApproveNReject(parameters);

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
                    _response.Message = "Record details saved successfully";
                }
            }

            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetUserRemarkLogListById(UserRemarkLog_Search parameters)
        {
            if (parameters.EmployeeId <= 0)
            {
                _response.Message = "User Id is required";
            }
            else
            {
                var vResultObj = await _userRepository.GetUserRemarkLogListById(parameters);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion
    }
}
