using NaturalUruguayGateway.AuthorizationDataAccess.Repositories;
using NaturalUruguayGateway.AuthorizationDataAccessInterface.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NaturalUruguayGateway.AuthorizationEngine.Services;
using NaturalUruguayGateway.AuthorizationEngineInterface.Services;
using Microsoft.EntityFrameworkCore;
using NaturalUruguayGateway.DataAccess.Context;
using NaturalUruguayGateway.Helper.Configuration;
using NaturalUruguayGateway.Helper.Services;
using NaturalUruguayGateway.HelperInterface.Configuration;
using NaturalUruguayGateway.HelperInterface.Services;
using NaturalUruguayGateway.NaturalUruguayDataAccess.Repositories;
using NaturalUruguayGateway.NaturalUruguayDataAccessInterface.Repositories;
using NaturalUruguayGateway.NaturalUruguayEngine.Services;
using NaturalUruguayGateway.NaturalUruguayEngineInterface.Services;
using NaturalUruguayGateway.ThirdPartyImport.Services;
using NaturalUruguayGateway.ThridPartyImportInterface.Services;

namespace Factory.Factories
{
    public static class ServiceFactory
    {   
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddSingleton<IConfigurationManager, ConfigManager>();
            services.AddSingleton<IEncryptor, MD5Encryptor>();
            services.AddSingleton<ILodgmentCalculator, LodgmentCalculator>();
            services.AddScoped<ISessionsService, SessionsService>();
            services.AddScoped<ISessionsRepository, SessionsRepository>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IRegionsService, RegionsService>();
            services.AddScoped<IRegionsRepository, RegionsRepository>();
            services.AddScoped<ISpotsService, SpotsService>();
            services.AddScoped<ISpotsRepository, SpotsRepository>();
            services.AddScoped<ILodgmentsService, LodgmentsService>();
            services.AddScoped<ILodgmentsRepository, LodgmentsRepository>();
            services.AddScoped<IBookingsService, BookingsService>();
            services.AddScoped<IBookingsRepository, BookingsRepository>();
            services.AddScoped<IStorageService, StorageService>();
            services.AddScoped<IReviewsService, ReviewsService>();
            services.AddScoped<IReviewsRepository, ReviewsRepository>();
            services.AddScoped<IReportsService, ReportsService>();
            services.AddScoped<IReportsRepository, ReportsRepository>();
            services.AddScoped<IImportsService, ImportsService>();
        }
        
        public static void AddDbContextService(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DbContext, NaturalUruguayContext>(options => options.UseSqlServer(connectionString));
        }
    }
}