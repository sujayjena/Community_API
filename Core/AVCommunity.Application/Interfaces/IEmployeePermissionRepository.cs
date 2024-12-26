using AVCommunity.Application.Models;
using AVCommunity.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVCommunity.Application.Interfaces
{
    public interface IEmployeePermissionRepository
    {
        #region Module Master 

        Task<IEnumerable<ModuleMaster_Response>> GetModuleMasterList(BaseSearchEntity parameters);

        #endregion

        #region Employee Permission 

        Task<int> SaveEmployeePermission(Employee_Permission_Request parameters);

        Task<IEnumerable<Employee_Permission_Response>> GetEmployeePermissionList(Employee_Search_Request parameters);

        Task<IEnumerable<Employee_Permission_Response>> GetEmployeePermissionById(long EmployeeId);

        #endregion
    }
}
