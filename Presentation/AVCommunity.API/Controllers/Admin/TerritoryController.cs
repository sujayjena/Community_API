using AVCommunity.Application.Enums;
using AVCommunity.Application.Helpers;
using AVCommunity.Application.Interfaces;
using AVCommunity.Application.Models;
using AVCommunity.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Globalization;

namespace AVCommunity.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class TerritoryController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly ITerritoryRepository _territoryRepository;

        public TerritoryController(ITerritoryRepository territoryRepository)
        {
            _territoryRepository = territoryRepository;
            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        #region State 

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveState(State_Request parameters)
        {
            int result = await _territoryRepository.SaveState(parameters);

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
                _response.Message = "Record details saved successfully";
            }

            _response.Id = result;
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetStateList(BaseSearchEntity parameters)
        {
            IEnumerable<State_Response> lstStates = await _territoryRepository.GetStateList(parameters);
            _response.Data = lstStates.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetStateById(long Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _territoryRepository.GetStateById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ExportStateData()
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var request = new BaseSearchEntity();

            IEnumerable<State_Response> lstObj = await _territoryRepository.GetStateList(request);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("State");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "State Name";
                    WorkSheet1.Cells[1, 2].Value = "Status";

                    WorkSheet1.Cells[1, 3].Value = "CreatedDate";
                    WorkSheet1.Cells[1, 4].Value = "CreatedBy";


                    recordIndex = 2;

                    foreach (var items in lstObj)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = items.StateName;
                        WorkSheet1.Cells[recordIndex, 2].Value = items.IsActive == true ? "Active" : "Inactive";

                        WorkSheet1.Cells[recordIndex, 3].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                        WorkSheet1.Cells[recordIndex, 3].Value = items.CreatedDate;
                        WorkSheet1.Cells[recordIndex, 4].Value = items.CreatorName;

                        recordIndex += 1;
                    }

                    WorkSheet1.Column(1).AutoFit();
                    WorkSheet1.Column(2).AutoFit();
                    WorkSheet1.Column(3).AutoFit();
                    WorkSheet1.Column(4).AutoFit();

                    excelExportData.SaveAs(msExportDataFile);
                    msExportDataFile.Position = 0;
                    result = msExportDataFile.ToArray();
                }
            }

            if (result != null)
            {
                _response.Data = result;
                _response.IsSuccess = true;
                _response.Message = "Record Exported successfully";
            }

            return _response;
        }

        #endregion

        #region District 

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveDistrict(District_Request parameters)
        {
            int result = await _territoryRepository.SaveDistrict(parameters);

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
                _response.Message = "Record details saved successfully";
            }

            _response.Id = result;
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetDistrictList(BaseSearchEntity parameters)
        {
            IEnumerable<District_Response> lstDistricts = await _territoryRepository.GetDistrictList(parameters);
            _response.Data = lstDistricts.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetDistrictById(long Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _territoryRepository.GetDistrictById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ExportDistrictData()
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var request = new BaseSearchEntity();

            IEnumerable<District_Response> lstObj = await _territoryRepository.GetDistrictList(request);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("District");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "District Name";
                    WorkSheet1.Cells[1, 2].Value = "Status";

                    WorkSheet1.Cells[1, 3].Value = "CreatedDate";
                    WorkSheet1.Cells[1, 4].Value = "CreatedBy";


                    recordIndex = 2;

                    foreach (var items in lstObj)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = items.DistrictName;
                        WorkSheet1.Cells[recordIndex, 2].Value = items.IsActive == true ? "Active" : "Inactive";

                        WorkSheet1.Cells[recordIndex, 3].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                        WorkSheet1.Cells[recordIndex, 3].Value = items.CreatedDate;
                        WorkSheet1.Cells[recordIndex, 4].Value = items.CreatorName;

                        recordIndex += 1;
                    }

                    WorkSheet1.Column(1).AutoFit();
                    WorkSheet1.Column(2).AutoFit();
                    WorkSheet1.Column(3).AutoFit();
                    WorkSheet1.Column(4).AutoFit();

                    excelExportData.SaveAs(msExportDataFile);
                    msExportDataFile.Position = 0;
                    result = msExportDataFile.ToArray();
                }
            }

            if (result != null)
            {
                _response.Data = result;
                _response.IsSuccess = true;
                _response.Message = "Record Exported successfully";
            }

            return _response;
        }

        #endregion

        #region Village 

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveVillage(Village_Request parameters)
        {
            int result = await _territoryRepository.SaveVillage(parameters);

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
                _response.Message = "Record details saved successfully";
            }

            _response.Id = result;
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetVillageList(BaseSearchEntity parameters)
        {
            IEnumerable<Village_Response> lstVillages = await _territoryRepository.GetVillageList(parameters);
            _response.Data = lstVillages.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetVillageById(long Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _territoryRepository.GetVillageById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ExportVillageData()
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var request = new BaseSearchEntity();

            IEnumerable<Village_Response> lstObj = await _territoryRepository.GetVillageList(request);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("Village");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "Village Name";
                    WorkSheet1.Cells[1, 2].Value = "Status";

                    WorkSheet1.Cells[1, 3].Value = "CreatedDate";
                    WorkSheet1.Cells[1, 4].Value = "CreatedBy";


                    recordIndex = 2;

                    foreach (var items in lstObj)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = items.VillageName;
                        WorkSheet1.Cells[recordIndex, 2].Value = items.IsActive == true ? "Active" : "Inactive";

                        WorkSheet1.Cells[recordIndex, 3].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                        WorkSheet1.Cells[recordIndex, 3].Value = items.CreatedDate;
                        WorkSheet1.Cells[recordIndex, 4].Value = items.CreatorName;

                        recordIndex += 1;
                    }

                    WorkSheet1.Column(1).AutoFit();
                    WorkSheet1.Column(2).AutoFit();
                    WorkSheet1.Column(3).AutoFit();
                    WorkSheet1.Column(4).AutoFit();

                    excelExportData.SaveAs(msExportDataFile);
                    msExportDataFile.Position = 0;
                    result = msExportDataFile.ToArray();
                }
            }

            if (result != null)
            {
                _response.Data = result;
                _response.IsSuccess = true;
                _response.Message = "Record Exported successfully";
            }

            return _response;
        }

        #endregion

        #region Territories 

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveTerritories(Territories_Request parameters)
        {
            int result = await _territoryRepository.SaveTerritories(parameters);

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
                _response.Message = "Record details saved successfully";
            }

            _response.Id = result;
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetTerritoriesList(BaseSearchEntity parameters)
        {
            IEnumerable<Territories_Response> lstTerritoriess = await _territoryRepository.GetTerritoriesList(parameters);
            _response.Data = lstTerritoriess.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetTerritoriesById(long Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _territoryRepository.GetTerritoriesById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetTerritories_State_Dist_Village_List_ById(Territories_State_Dist_Village_Search parameters)
        {
            var vResultObj = await _territoryRepository.GetTerritories_State_Dist_Village_List_ById(parameters);
            _response.Data = vResultObj;

            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ExportTerritoriesData()
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var request = new BaseSearchEntity();

            IEnumerable<Territories_Response> lstSizeObj = await _territoryRepository.GetTerritoriesList(request);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("Territories");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "State";
                    WorkSheet1.Cells[1, 2].Value = "District";
                    WorkSheet1.Cells[1, 3].Value = "Village";
                    WorkSheet1.Cells[1, 4].Value = "Status";

                    WorkSheet1.Cells[1, 5].Value = "CreatedDate";
                    WorkSheet1.Cells[1, 6].Value = "CreatedBy";


                    recordIndex = 2;

                    foreach (var items in lstSizeObj)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = items.StateName;
                        WorkSheet1.Cells[recordIndex, 2].Value = items.DistrictName;
                        WorkSheet1.Cells[recordIndex, 3].Value = items.VillageName;
                        WorkSheet1.Cells[recordIndex, 4].Value = items.IsActive == true ? "Active" : "Inactive";

                        WorkSheet1.Cells[recordIndex, 5].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                        WorkSheet1.Cells[recordIndex, 5].Value = items.CreatedDate;
                        WorkSheet1.Cells[recordIndex, 6].Value = items.CreatorName;

                        recordIndex += 1;
                    }

                    WorkSheet1.Column(1).AutoFit();
                    WorkSheet1.Column(2).AutoFit();
                    WorkSheet1.Column(3).AutoFit();
                    WorkSheet1.Column(4).AutoFit();
                    WorkSheet1.Column(5).AutoFit();
                    WorkSheet1.Column(6).AutoFit();

                    excelExportData.SaveAs(msExportDataFile);
                    msExportDataFile.Position = 0;
                    result = msExportDataFile.ToArray();
                }
            }

            if (result != null)
            {
                _response.Data = result;
                _response.IsSuccess = true;
                _response.Message = "Territories list Exported successfully";
            }

            return _response;
        }

        #endregion
    }
}