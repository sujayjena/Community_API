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
    public class NotificationController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly INotificationRepository _notificationRepository;
        private IFileManager _fileManager;

        public NotificationController(INotificationRepository NotificationRepository, IFileManager fileManager)
        {
            _notificationRepository = NotificationRepository;
            _fileManager = fileManager;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveNotification(Notification_Request parameters)
        {
            int result = await _notificationRepository.SaveNotification(parameters);

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
        public async Task<ResponseModel> GetNotificationList(Notification_Search parameters)
        {
            var objList = await _notificationRepository.GetNotificationList(parameters);
            _response.Data = objList.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetNotificationPopupList(Notification_Search parameters)
        {
            var vNotificationPopup_ResponseObj = new NotificationPopup_Response();

            var objList = await _notificationRepository.GetNotificationList(parameters);

            vNotificationPopup_ResponseObj.UnReadCount = objList.ToList().Where(x => x.ReadUnread == false).ToList().Count();
            foreach (var notification in objList)
            {
                var vNotification_ResponseObj = new Notification_Response()
                {
                    Id = notification.Id,
                    CustomerEmployeeId = notification.CustomerEmployeeId,
                    Subject = notification.Subject,
                    SendTo = notification.SendTo,
                    Message = notification.Message,
                    RefValue1 = notification.RefValue1,
                    RefValue2 = notification.RefValue2,
                    ReadUnread = notification.ReadUnread,
                    CreatedDate = notification.CreatedDate,
                };

                vNotificationPopup_ResponseObj.NotificationList.Add(notification);
            }

            _response.Data = vNotificationPopup_ResponseObj;
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetNotificationById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _notificationRepository.GetNotificationById(Id);

                _response.Data = vResultObj;
            }
            return _response;
        }
    }
}
