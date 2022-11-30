using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourBooking.Core.Domain;
using TourBooking.Core.DTOs.Inputs;
using TourBooking.Core.DTOs.Outputs;
using TourBooking.Core.Enums;
using TourBooking.Core.Interfaces;

namespace TourBooking.Web.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService userService;

    public UserController(IUserService userService)
    {
        this.userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("RegisterBooker")]
    public async Task<ActionResult<ResponseDTO<ApplicationUser>>> RegisterBookerAsync(RegisterInputModel registerInputModel)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }

        var registerResult = await userService.RegisterAsync(registerInputModel, RoleType.Booker);

        if (registerResult.IsSuccess)
        {
            return Ok(registerResult);
        }
        else
        {
            if (registerResult.ErrorType is RegisterErrorType.CouldNotFindApplicationUser)
            {
                return NotFound(registerResult);
            }
            else if (registerResult.ErrorType is RegisterErrorType.CouldNotCreateApplicationUser or RegisterErrorType.EmailAlreadyExists)
            {
                return UnprocessableEntity(registerResult);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, registerResult);
            }
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("RegisterEmployee")]
    public async Task<ActionResult<ResponseDTO<ApplicationUser>>> RegisterEmployeeAsync(RegisterInputModel registerInputModel)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }

        var registerResult = await userService.RegisterAsync(registerInputModel, RoleType.Employee);

        if (registerResult.IsSuccess)
        {
            return Ok(registerResult);
        }
        else
        {
            if (registerResult.ErrorType is RegisterErrorType.CouldNotFindApplicationUser)
            {
                return NotFound(registerResult);
            }
            else if (registerResult.ErrorType is RegisterErrorType.CouldNotCreateApplicationUser or RegisterErrorType.EmailAlreadyExists)
            {
                return UnprocessableEntity(registerResult);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, registerResult);
            }
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("RegisterAdmin")]
    public async Task<ActionResult<ResponseDTO<ApplicationUser>>> RegisterAsync(RegisterInputModel registerInputModel)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }

        var registerResult = await userService.RegisterAsync(registerInputModel, RoleType.Admin);

        if (registerResult.IsSuccess)
        {
            return Ok(registerResult);
        }
        else
        {
            if (registerResult.ErrorType is RegisterErrorType.CouldNotFindApplicationUser)
            {
                return NotFound(registerResult);
            }
            else if (registerResult.ErrorType is RegisterErrorType.CouldNotCreateApplicationUser or RegisterErrorType.EmailAlreadyExists)
            {
                return UnprocessableEntity(registerResult);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, registerResult);
            }
        }
    }
}