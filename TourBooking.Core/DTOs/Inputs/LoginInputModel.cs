using System.ComponentModel.DataAnnotations;

namespace TourBooking.Core.DTOs.Inputs;

public sealed class LoginInputModel
{
    [Required(ErrorMessage = "This field is required.")]
    [StringLength(75, ErrorMessage = "{0} can only consist of {1} characters.")]
    [EmailAddress(ErrorMessage = "{0} must be in a correct format.")]
    [Display(Name = "Email")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "This field is required.")]
    [StringLength(50, ErrorMessage = "{0} can only consist of {1} characters.")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = default!;

    [Display(Name = "Stay logged in")]
    public bool RememberMe { get; set; }
}