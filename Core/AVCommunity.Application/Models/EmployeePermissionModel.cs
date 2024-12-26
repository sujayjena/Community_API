using AVCommunity.Domain.Entities;
using AVCommunity.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVCommunity.Application.Models
{
    public class EmployeePermissionModel
    {
    }

    #region ModuleMaster

    public class ModuleMaster_Request //: BaseEntity
    {
        public long ModuleId { get; set; }
        public long ModuleName { get; set; }
        public long AppType { get; set; }
        public long IsActive { get; set; }
    }

    public class ModuleMaster_Response //: BaseResponseEntity
    {
        public long ModuleId { get; set; }
        public string? ModuleName { get; set; }
        public string? AppType { get; set; }
        public bool? IsActive { get; set; }
    }

    public class ModuleList
    {
        public long ModuleId { get; set; }
        public bool? View { get; set; }
        public bool? Add { get; set; }
        public bool? Edit { get; set; }
    }

    #endregion


    #region Employee_Permission

    public class Employee_Search_Request : BaseSearchEntity
    {
        public long EmployeeId { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }

    public class Employee_Permission_Request // : BaseEntity
    {
        public long EmployeePermissionId { get; set; }
        public string? AppType { get; set; }
        public long EmployeeId { get; set; }
        public bool? IsActive { get; set; }

        public List<ModuleList>? ModuleList { get; set; }
    }

    public class Employee_Permission_Response 
    {
        public string? AppType { get; set; }
        //public long EmployeePermissionId { get; set; }
        //public long EmployeeId { get; set; }
        //public long RoleId { get; set; }
        //public string? RoleName { get; set; }
        public long ModuleId { get; set; }
        public string? ModuleName { get; set; }
        public bool? View { get; set; }
        public bool? Add { get; set; }
        public bool? Edit { get; set; }
    }

    #endregion
}
