using TourBooking.Core.Domain;
using TourBooking.Core.DTOs.Inputs;
using TourBooking.Core.DTOs.Outputs;
using TourBooking.Core.Enums;

namespace TourBooking.Core.Interfaces;

public interface IUserService
{
    Task<ResponseDTO<ApplicationUser>> RegisterAsync(RegisterInputModel registerInputModel, RoleType roleType);
}