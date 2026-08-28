using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGiversApp.Models;

public class GoodsCategory
{
    public int Id { get; set; }

    [Required, StringLength(80)]
    public string Name { get; set; } = string.Empty;

    [StringLength(250)]
    public string? Description { get; set; }

    [Display(Name = "Pre-configured")]
    public bool IsPredefined { get; set; }
}
