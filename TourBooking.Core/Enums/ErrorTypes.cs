namespace TourBooking.Core.Enums;

public enum LoginErrorType
{
    EmailIsNullOrWhitespace = 1,
    PasswordIsNullOrWhitespace = 2,
    CouldNotFindApplicationUser = 3,
    ApplicationUserIsLockedOut = 4,
    InvalidPassword = 5,
    UpdateApplicationUserFailed = 6,
}

public enum RefreshTokenErrorType
{
    AccessOrRefreshTokenIsNullOrWhitespace = 7,
    CouldNotValidateAccessToken = 8,
    InvalidRefreshToken = 9,
    CouldNotFindApplicationUser = 10,
    UpdateApplicationUserFailed = 11,
}

public enum RegisterErrorType
{
    EmailAlreadyExists = 12,
    CouldNotCreateApplicationUser = 13,
    CouldNotFindApplicationUser = 14,
}