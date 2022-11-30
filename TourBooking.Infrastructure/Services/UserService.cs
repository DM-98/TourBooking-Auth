using Microsoft.AspNetCore.Identity;
using TourBooking.Core.Domain;
using TourBooking.Core.DTOs.Inputs;
using TourBooking.Core.DTOs.Outputs;
using TourBooking.Core.Enums;
using TourBooking.Core.Interfaces;

namespace TourBooking.Infrastructure.Services;

public sealed class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly RoleManager<IdentityRole<Guid>> roleManager;

    public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
    }

    public async Task<ResponseDTO<ApplicationUser>> RegisterAsync(RegisterInputModel registerInputModel, RoleType roleType)
    {
        var applicationUserExists = await userManager.FindByEmailAsync(registerInputModel.Email) is not null;

        if (applicationUserExists)
        {
            return new ResponseDTO<ApplicationUser>(false, $"User with the email ({registerInputModel.Email}) already exists.", RegisterErrorType.EmailAlreadyExists);
        }

        var applicationUserToCreate = new ApplicationUser
        {
            UserName = registerInputModel.FirstName + " " + registerInputModel.LastName,
            Email = registerInputModel.Email,
            PhoneNumber = registerInputModel.PhoneNumber,
            IsEmailNotificationsEnabled = true,
            LockoutEnabled = true,
            SecurityStamp = Guid.NewGuid().ToString(),
        };

        var createApplicationUserResult = await userManager.CreateAsync(applicationUserToCreate, registerInputModel.Password);

        if (!createApplicationUserResult.Succeeded)
        {
            return new ResponseDTO<ApplicationUser>(false, "Server error. Unable to register - contact us for more information.", RegisterErrorType.CouldNotCreateApplicationUser);
        }

        var applicationUser = await userManager.FindByEmailAsync(registerInputModel.Email);

        if (applicationUser is null)
        {
            return new ResponseDTO<ApplicationUser>(false, "Server error. Contact us for more information.", RegisterErrorType.CouldNotFindApplicationUser);
        }

        var roleExists = await roleManager.RoleExistsAsync(roleType.ToString());

        if (!roleExists)
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>(roleType.ToString()));
        }

        await userManager.AddToRoleAsync(applicationUser, roleType.ToString());

        return new ResponseDTO<ApplicationUser>(true, content: applicationUser);
    }
}