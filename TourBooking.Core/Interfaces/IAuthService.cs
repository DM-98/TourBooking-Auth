using TourBooking.Core.DTOs.Inputs;
using TourBooking.Core.DTOs.Outputs;

namespace TourBooking.Core.Interfaces;

public interface IAuthService
{
    Task<ResponseDTO<TokenDTO>> LoginAsync(LoginInputModel loginInputModel);

    Task<ResponseDTO<TokenDTO>> RefreshTokensAsync(TokenDTO tokenDTO);
}