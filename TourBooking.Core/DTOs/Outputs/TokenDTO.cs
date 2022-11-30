namespace TourBooking.Core.DTOs.Outputs;

public sealed record TokenDTO
{
    public string AccessToken { get; init; }

    public string RefreshToken { get; init; }

    public TokenDTO(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}