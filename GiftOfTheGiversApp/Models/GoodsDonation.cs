using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGiversApp.Models;

public class GoodsDonation
{
    public int Id { get; set; }

    [Required, DataType(DataType.Date), Display(Name = "Date of donation")]
    public DateTime DonationDate { get; set; } = DateTime.Today;

    [Required, Range(1, 1000000), Display(Name = "Number of items")]
    public int ItemCount { get; set; }

    [Required, Display(Name = "Category")]
    public int CategoryId { get; set; }

    public GoodsCategory? Category { get; set; }

    [Required, StringLength(400), Display(Name = "Description of items")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Remain anonymous")]
    public bool IsAnonymous { get; set; }

    [Display(Name = "Donor name"), StringLength(120)]
    public string? DonorName { get; set; }

    public string DisplayName => IsAnonymous || string.IsNullOrWhiteSpace(DonorName)
        ? "Anonymous"
        : DonorName;
}
