using System.ComponentModel.DataAnnotations;

namespace TourBooking.Core.DTOs.Inputs;

public sealed class RegisterInputModel
{
    [Required(ErrorMessage = "This field is required.")]
    [StringLength(50, ErrorMessage = "{0} can only consist of {1} characters.")]
    [Display(Name = "First name")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "This field is required.")]
    [StringLength(50, ErrorMessage = "{0} can only consist of {1} characters.")]
    [Display(Name = "Last name")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "This field is required.")]
    [StringLength(75, ErrorMessage = "{0} can only consist of {1} characters.")]
    [EmailAddress(ErrorMessage = "{0} must be in a correct format.")]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Display(Name = "Phone number")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "This field is required.")]
    [StringLength(100, ErrorMessage = "{0} can only consist of {1} characters.")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [Required(ErrorMessage = "This field is required.")]
    [StringLength(100, ErrorMessage = "{0} can only consist of {1} characters.")]
    [Compare(nameof(Password), ErrorMessage = "Password and confirm password must match.")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    public string ConfirmPassword { get; set; }

    [Required]
    [Range(typeof(bool), "true", "true", ErrorMessage = "To register, you must accept the terms and conditions.")]
    [Display(Name = "I accept the terms and conditions")]
    public bool IsTermsAccepted { get; set; }
}