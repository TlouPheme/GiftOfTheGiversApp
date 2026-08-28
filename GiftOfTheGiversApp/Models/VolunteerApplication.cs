using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGiversApp.Models;

public class VolunteerApplication
{
    public int Id { get; set; }

    [Required, Display(Name = "Full name"), StringLength(120)]
    public string FullName { get; set; } = string.Empty;

    [Required, Display(Name = "Contact number"), StringLength(40)]
    public string Phone { get; set; } = string.Empty;

    [EmailAddress, StringLength(120)]
    public string? Email { get; set; }

    [Required, Display(Name = "City / town"), StringLength(80)]
    public string City { get; set; } = string.Empty;

    [StringLength(400)]
    public string? Skills { get; set; }

    [StringLength(200)]
    public string? Availability { get; set; }

    [Display(Name = "Submitted")]
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
}
