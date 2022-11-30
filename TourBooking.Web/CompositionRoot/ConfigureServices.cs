using TourBooking.Core.Interfaces;
using TourBooking.Infrastructure.Services;

namespace TourBooking.Web.CompositionRoot;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureTourBookingServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}