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
        private IFileManager _fileManager;

        public UserController(IUserRepository userRepository, IFileManager fileManager, ITerritoryRepository territoryRepository, IAdminMasterRepository adminMasterRepository)
        {
            _userRepository = userRepository;
            _fileManager = fileManager;
            _territoryRepository = territoryRepository;
            _adminMasterRepository = adminMasterRepository;

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
                _response.Message = "Record details saved sucessfully";

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
        public async Task<ResponseModel> GetAdminList(BaseSearchEntity parameters)
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
                _response.Message = "Record details saved sucessfully";

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
                    WorkSheet1.Cells[1, 8].Value = "Date Of Birth";
                    WorkSheet1.Cells[1, 9].Value = "Age";
                    WorkSheet1.Cells[1, 10].Value = "Higher Study";
                    WorkSheet1.Cells[1, 11].Value = "Business";
                    WorkSheet1.Cells[1, 12].Value = "Current Address";
                    WorkSheet1.Cells[1, 13].Value = "State";
                    WorkSheet1.Cells[1, 14].Value = "District";
                    WorkSheet1.Cells[1, 15].Value = "Village";
                    WorkSheet1.Cells[1, 16].Value = "Pincode";
                    WorkSheet1.Cells[1, 17].Value = "Industry";
                    WorkSheet1.Cells[1, 18].Value = "Business Address";
                    //WorkSheet1.Cells[1, 17].Value = "State";
                    //WorkSheet1.Cells[1, 18].Value = "District";
                    //WorkSheet1.Cells[1, 19].Value = "Village";
                    //WorkSheet1.Cells[1, 20].Value = "Pincode";
                    WorkSheet1.Cells[1, 19].Value = "IsActive";

                    recordIndex = 2;

                    int i = 1;
                    foreach (var items in lstSizeObj)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = i.ToString();
                        WorkSheet1.Cells[recordIndex, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        WorkSheet1.Cells[recordIndex, 2].Value = items.FirstName + " " + items.MiddleName;
                        WorkSheet1.Cells[recordIndex, 3].Value = items.Surname;
                        WorkSheet1.Cells[recordIndex, 4].Value = items.MobileNumber;
                        WorkSheet1.Cells[recordIndex, 5].Value = items.RelationName;
                        WorkSheet1.Cells[recordIndex, 6].Value = items.GenderName;
                        WorkSheet1.Cells[recordIndex, 7].Value = items.MeritalStatusName;
                        WorkSheet1.Cells[recordIndex, 8].Value = items.DateOfBirth.HasValue ? items.DateOfBirth.Value.ToString("dd/MM/yyyy") : string.Empty;
                        WorkSheet1.Cells[recordIndex, 9].Value = items.Age;
                        WorkSheet1.Cells[recordIndex, 10].Value = items.HigherStudyName;
                        WorkSheet1.Cells[recordIndex, 11].Value = items.OccupationName;
                        WorkSheet1.Cells[recordIndex, 12].Value = items.CurrentAddress;
                        WorkSheet1.Cells[recordIndex, 13].Value = items.StateName;
                        WorkSheet1.Cells[recordIndex, 14].Value = items.DistrictName;
                        WorkSheet1.Cells[recordIndex, 15].Value = items.VillageName;
                        WorkSheet1.Cells[recordIndex, 16].Value = items.Pincode;
                        WorkSheet1.Cells[recordIndex, 17].Value = items.IndustryName;
                        WorkSheet1.Cells[recordIndex, 18].Value = items.BusinessAddress;
                        //WorkSheet1.Cells[recordIndex, 17].Value = items.StateName;
                        //WorkSheet1.Cells[recordIndex, 18].Value = items.DistrictName;
                        //WorkSheet1.Cells[recordIndex, 19].Value = items.VillageName;
                        //WorkSheet1.Cells[recordIndex, 20].Value = items.Pincode;
                        WorkSheet1.Cells[recordIndex, 19].Value = items.IsActive == true ? "Active" : "Inactive";

                        recordIndex += 1;

                        //member of family list
                        var vUser_Search = new User_Search();
                        vUser_Search.RegisterUserId = items.Id;

                        int j = 1;
                        IEnumerable<User_Response> lstMUserObj = await _userRepository.GetUserList(vUser_Search);
                        foreach (var mitems in lstMUserObj)
                        {
                            WorkSheet1.Cells[recordIndex, 1].Value = i + "." + j;
                            WorkSheet1.Cells[recordIndex, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            WorkSheet1.Cells[recordIndex, 2].Value = mitems.FirstName + " " + mitems.MiddleName;
                            WorkSheet1.Cells[recordIndex, 3].Value = mitems.Surname;
                            WorkSheet1.Cells[recordIndex, 4].Value = mitems.MobileNumber;
                            WorkSheet1.Cells[recordIndex, 5].Value = mitems.RelationName;
                            WorkSheet1.Cells[recordIndex, 6].Value = mitems.GenderName;
                            WorkSheet1.Cells[recordIndex, 7].Value = mitems.MeritalStatusName;
                            WorkSheet1.Cells[recordIndex, 8].Value = mitems.DateOfBirth.HasValue ? mitems.DateOfBirth.Value.ToString("dd/MM/yyyy") : string.Empty;
                            WorkSheet1.Cells[recordIndex, 9].Value = mitems.Age;
                            WorkSheet1.Cells[recordIndex, 10].Value = mitems.HigherStudyName;
                            WorkSheet1.Cells[recordIndex, 11].Value = mitems.OccupationName;
                            WorkSheet1.Cells[recordIndex, 12].Value = mitems.CurrentAddress;
                            WorkSheet1.Cells[recordIndex, 13].Value = mitems.StateName;
                            WorkSheet1.Cells[recordIndex, 14].Value = mitems.DistrictName;
                            WorkSheet1.Cells[recordIndex, 15].Value = mitems.VillageName;
                            WorkSheet1.Cells[recordIndex, 16].Value = mitems.Pincode;
                            WorkSheet1.Cells[recordIndex, 17].Value = items.IndustryName;
                            WorkSheet1.Cells[recordIndex, 18].Value = mitems.CurrentAddress;
                            //WorkSheet1.Cells[recordIndex, 17].Value = mitems.StateName;
                            //WorkSheet1.Cells[recordIndex, 18].Value = mitems.DistrictName;
                            //WorkSheet1.Cells[recordIndex, 19].Value = mitems.VillageName;
                            //WorkSheet1.Cells[recordIndex, 20].Value = mitems.Pincode;
                            WorkSheet1.Cells[recordIndex, 19].Value = mitems.IsActive == true ? "Active" : "Inactive";

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
                    //WorkSheet1.Column(19).AutoFit();
                    //WorkSheet1.Column(20).AutoFit();
                    //WorkSheet1.Column(21).AutoFit();

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
                _response.Message = "Record details saved sucessfully";
            }

            _response.Id = result;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetGlobalUserList(GlobalUser_Search parameters)
        {
            IEnumerable<User_Response> lstUsers = await _userRepository.GetGlobalUserList(parameters);
            _response.Data = lstUsers.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        #endregion
    }
}
