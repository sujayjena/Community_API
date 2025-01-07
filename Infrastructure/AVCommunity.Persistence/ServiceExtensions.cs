using AVCommunity.Application.Helpers;
using AVCommunity.Application.Interfaces;
using AVCommunity.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AVCommunity.Persistence
{
    public static class ServiceExtensions
    {
        public static void ConfigurePersistence(this IServiceCollection services, IConfiguration configuration)
        {
            //var connectionString = configuration.GetConnectionString("DefaultConnection");
            //services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(connectionString));

            services.AddScoped<IGenericRepository, GenericRepository>();
            services.AddScoped<IJwtUtilsRepository, JwtUtilsRepository>();
            services.AddScoped<IFileManager, FileManager>();

            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITerritoryRepository, TerritoryRepository>();
            services.AddScoped<IEmployeePermissionRepository, EmployeePermissionRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IAdminMasterRepository, AdminMasterRepository>();
            services.AddScoped<IManageRewardsRepository, ManageRewardsRepository>();
            services.AddScoped<IManageMarriageRepository, ManageMarriageRepository>();
            services.AddScoped<IManageBirthRepository, ManageBirthRepository>();
            services.AddScoped<IManageDeathRepository, ManageDeathRepository>();
        }
    }
}
