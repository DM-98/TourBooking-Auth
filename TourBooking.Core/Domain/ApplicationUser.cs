using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TourBooking.Core.Domain;

public sealed class ApplicationUser : IdentityUser<Guid>
{
    public bool IsEmailNotificationsEnabled { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    [Timestamp]
    public byte[]? RowVersion { get; set; }

    public DateTime? DeletionRequestedDate { get; set; }
}