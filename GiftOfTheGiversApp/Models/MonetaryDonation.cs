using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftOfTheGiversApp.Models;

public class MonetaryDonation
{
    public int Id { get; set; }

    [Required, DataType(DataType.Date), Display(Name = "Date of donation")]
    public DateTime DonationDate { get; set; } = DateTime.Today;

    [Required, Range(1, 100000000), Display(Name = "Amount (ZAR)")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal AmountZar { get; set; }

    [Display(Name = "Remain anonymous")]
    public bool IsAnonymous { get; set; }

    [Display(Name = "Donor name"), StringLength(120)]
    public string? DonorName { get; set; }

    [StringLength(400)]
    public string? Message { get; set; }

    [Display(Name = "Display name")]
    public string DisplayName => IsAnonymous || string.IsNullOrWhiteSpace(DonorName)
        ? "Anonymous"
        : DonorName;
}
