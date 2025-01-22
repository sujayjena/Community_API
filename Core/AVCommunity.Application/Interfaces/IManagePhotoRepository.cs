using AVCommunity.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVCommunity.Application.Interfaces
{
    public interface IManagePhotoRepository
    {
        #region Photo Header

        Task<int> SavePhotoHeader(PhotoHeader_Request parameters);

        Task<IEnumerable<PhotoHeader_Response>> GetPhotoHeaderList(PhotoHeader_Search parameters);

        Task<PhotoHeader_Response?> GetPhotoHeaderById(long Id);

        #endregion

        #region Photo

        Task<int> SavePhoto(Photo_Request parameters);

        Task<IEnumerable<Photo_Response>> GetPhotoList(Photo_Search parameters);

        Task<Photo_Response?> GetPhotoById(long Id);

        #endregion
    }
}
 