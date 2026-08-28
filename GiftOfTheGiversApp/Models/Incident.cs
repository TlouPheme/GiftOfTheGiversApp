using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGiversApp.Models;

public class Incident
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(160)]
    public string Location { get; set; } = string.Empty;

    [Required, DataType(DataType.Date), Display(Name = "Start date")]
    public DateTime StartDate { get; set; } = DateTime.Today;

    [DataType(DataType.Date), Display(Name = "End date")]
    public DateTime? EndDate { get; set; }

    [Required, StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Aid required")]
    public string RequiredAid { get; set; } = "Food parcels";

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;
}
