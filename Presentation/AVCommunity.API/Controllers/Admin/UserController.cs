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

namespace AVCommunity.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IUserRepository _userRepository;
        private readonly ITerritoryRepository _territoryRepository;
        private IFileManager _fileManager;

        public UserController(IUserRepository userRepository, IFileManager fileManager, ITerritoryRepository territoryRepository)
        {
            _userRepository = userRepository;
            _fileManager = fileManager;
            _territoryRepository = territoryRepository;

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
        public async Task<ResponseModel> SaveUser(User_Request parameters)
        {
            int result = await _userRepository.SaveUser(parameters);

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
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetUserList(BaseSearchEntity parameters)
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
                _response.Data = vResultObj;
            }
            return _response;
        }

        /*
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ExportUserData(bool IsActive = true)
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var request = new BaseSearchEntity();
            request.IsActive = IsActive;

            IEnumerable<User_Response> lstSizeObj = await _userRepository.GetUserList(request);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("Employee");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "User Code";
                    WorkSheet1.Cells[1, 2].Value = "User Name";
                    WorkSheet1.Cells[1, 3].Value = "Mobile";
                    WorkSheet1.Cells[1, 4].Value = "EmailId";
                    WorkSheet1.Cells[1, 5].Value = "Role";
                    WorkSheet1.Cells[1, 6].Value = "ReportingTo";
                    WorkSheet1.Cells[1, 7].Value = "Department";
                    WorkSheet1.Cells[1, 8].Value = "Company";
                    WorkSheet1.Cells[1, 9].Value = "Address";
                    WorkSheet1.Cells[1, 10].Value = "Region";
                    WorkSheet1.Cells[1, 11].Value = "State";
                    WorkSheet1.Cells[1, 12].Value = "District";
                    WorkSheet1.Cells[1, 13].Value = "City";
                    WorkSheet1.Cells[1, 14].Value = "Pincode";
                    WorkSheet1.Cells[1, 15].Value = "DateOfBirth";
                    WorkSheet1.Cells[1, 16].Value = "Date Of Joining";
                    WorkSheet1.Cells[1, 17].Value = "Emergency Contact Number";
                    WorkSheet1.Cells[1, 18].Value = "Blood Group";
                    WorkSheet1.Cells[1, 19].Value = "Aadhar Number";
                    WorkSheet1.Cells[1, 20].Value = "Pan Number";
                    WorkSheet1.Cells[1, 21].Value = "Mobile UniqueId";
                    WorkSheet1.Cells[1, 22].Value = "IsMobileUser";
                    WorkSheet1.Cells[1, 23].Value = "IsWebUser";
                    WorkSheet1.Cells[1, 24].Value = "IsActive";


                    recordIndex = 2;

                    foreach (var items in lstSizeObj)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = items.UserCode;
                        WorkSheet1.Cells[recordIndex, 2].Value = items.UserName;
                        WorkSheet1.Cells[recordIndex, 3].Value = items.MobileNumber;
                        WorkSheet1.Cells[recordIndex, 4].Value = items.EmailId;
                        WorkSheet1.Cells[recordIndex, 11].Value = items.StateName;
                        WorkSheet1.Cells[recordIndex, 12].Value = items.DistrictName;
                        WorkSheet1.Cells[recordIndex, 13].Value = items.VillageName;
                        WorkSheet1.Cells[recordIndex, 14].Value = items.Pincode;
                        WorkSheet1.Cells[recordIndex, 15].Value = items.DateOfBirth.HasValue ? items.DateOfBirth.Value.ToString("dd/MM/yyyy") : string.Empty;
                        WorkSheet1.Cells[recordIndex, 21].Value = items.MobileUniqueId;
                        WorkSheet1.Cells[recordIndex, 24].Value = items.IsActive == true ? "Active" : "Inactive";

                        recordIndex += 1;
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
                    WorkSheet1.Column(21).AutoFit();
                    WorkSheet1.Column(22).AutoFit();
                    WorkSheet1.Column(23).AutoFit();
                    WorkSheet1.Column(24).AutoFit();

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
        */
        #endregion
    }
}
