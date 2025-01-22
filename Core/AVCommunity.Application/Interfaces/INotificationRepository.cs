using AVCommunity.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVCommunity.Application.Interfaces
{
    public interface INotificationRepository
    {
        Task<int> SaveNotification(Notification_Request parameters);

        Task<IEnumerable<Notification_Response>> GetNotificationList(Notification_Search parameters);

        Task<Notification_Response?> GetNotificationById(int Id);
    }
}
