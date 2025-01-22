using AVCommunity.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVCommunity.Application.Interfaces
{
    public interface IManageVideoRepository
    {
        #region Video Header

        Task<int> SaveVideoHeader(VideoHeader_Request parameters);

        Task<IEnumerable<VideoHeader_Response>> GetVideoHeaderList(VideoHeader_Search parameters);

        Task<VideoHeader_Response?> GetVideoHeaderById(long Id);

        #endregion

        #region Video

        Task<int> SaveVideo(Video_Request parameters);

        Task<IEnumerable<Video_Response>> GetVideoList(Video_Search parameters);

        Task<Video_Response?> GetVideoById(long Id);

        #endregion
    }
}
 