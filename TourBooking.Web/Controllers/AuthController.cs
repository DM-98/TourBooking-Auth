using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourBooking.Core.DTOs.Inputs;
using TourBooking.Core.DTOs.Outputs;
using TourBooking.Core.Enums;
using TourBooking.Core.Interfaces;

namespace TourBooking.Web.Controllers;

[Route("api/auth")]
[ApiController]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService authService;

    public AuthController(IAuthService authService)
    {
        this.authService = authService;
    }

    [AllowAnonymous, HttpPost("Login")]
    public async Task<ActionResult<ResponseDTO<TokenDTO>>> LoginAsync(LoginInputModel loginInputModel)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }

        var loginResult = await authService.LoginAsync(loginInputModel);

        if (loginResult.IsSuccess)
        {
            return Ok(loginResult);
        }
        else
        {
            if (loginResult.ErrorType is LoginErrorType.CouldNotFindApplicationUser)
            {
                return NotFound(loginResult);
            }
            else if (loginResult.ErrorType is LoginErrorType.InvalidPassword or LoginErrorType.EmailIsNullOrWhitespace or LoginErrorType.PasswordIsNullOrWhitespace)
            {
                return BadRequest(loginResult);
            }
            else if (loginResult.ErrorType is LoginErrorType.ApplicationUserIsLockedOut or LoginErrorType.UpdateApplicationUserFailed)
            {
                return UnprocessableEntity(loginResult);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, loginResult);
            }
        }
    }

    [AllowAnonymous, HttpPost("RefreshToken")]
    public async Task<ActionResult<ResponseDTO<TokenDTO>>> RefreshTokenAsync(TokenDTO tokenModel)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }

        var refreshTokenResult = await authService.RefreshTokensAsync(tokenModel);

        if (refreshTokenResult.IsSuccess)
        {
            return Ok(refreshTokenResult);
        }
        else
        {
            if (refreshTokenResult.ErrorType is RefreshTokenErrorType.CouldNotFindApplicationUser)
            {
                return NotFound(refreshTokenResult);
            }
            else if (refreshTokenResult.ErrorType is RefreshTokenErrorType.AccessOrRefreshTokenIsNullOrWhitespace or RefreshTokenErrorType.InvalidRefreshToken)
            {
                return BadRequest(refreshTokenResult);
            }
            else if (refreshTokenResult.ErrorType is RefreshTokenErrorType.CouldNotValidateAccessToken or RefreshTokenErrorType.UpdateApplicationUserFailed)
            {
                return UnprocessableEntity(refreshTokenResult);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, refreshTokenResult);
            }
        }
    }
}