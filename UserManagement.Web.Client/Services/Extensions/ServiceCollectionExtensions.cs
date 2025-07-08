using UserManagement.Web.Client.Services.Implementations;
using UserManagement.Web.Client.Services.Interfaces;

namespace UserManagement.Web.Client.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClientServices(this IServiceCollection services)
        => services.AddScoped<IUserApiService, UserApiService>();
}
