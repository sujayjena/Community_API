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
    public class DashboardController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IDashboardRepository _dashboardRepository;
        private IFileManager _fileManager;

        public DashboardController(IFileManager fileManager, IDashboardRepository dashboardRepository)
        {
            _fileManager = fileManager;
            _dashboardRepository = dashboardRepository;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetDashboard_Summary(GetDashboard_Summary_Search parameters)
        {
            IEnumerable<GetDashboard_Summary_Response> lst = await _dashboardRepository.GetDashboard_Summary(parameters);
            _response.Data = lst.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetDashboard_AgeWiseCategoryDistribution(GetDashboard_AgeWiseCategoryDistribution_Search parameters)
        {
            IEnumerable<GetDashboard_AgeWiseCategoryDistribution_Response> lst = await _dashboardRepository.GetDashboard_AgeWiseCategoryDistribution(parameters);
            _response.Data = lst.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetDashboard_BirthSummary(GetDashboard_BirthSummary_Search parameters)
        {
            IEnumerable<GetDashboard_BirthSummary_Response> lst = await _dashboardRepository.GetDashboard_BirthSummary(parameters);
            _response.Data = lst.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetDashboard_DeathSummary(GetDashboard_DeathSummary_Search parameters)
        {
            IEnumerable<GetDashboard_DeathSummary_Response> lst = await _dashboardRepository.GetDashboard_DeathSummary(parameters);
            _response.Data = lst.ToList();
            return _response;
        }
    }
}
