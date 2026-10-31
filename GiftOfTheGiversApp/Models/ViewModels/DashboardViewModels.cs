using System.ComponentModel.DataAnnotations;
using GiftOfTheGiversApp.Models;

namespace GiftOfTheGiversApp.Models.ViewModels;

public class HomeDashboardViewModel
{
    public decimal MoneyTotal { get; set; }
    public int GoodsTotal { get; set; }
    public int ActiveIncidentCount { get; set; }
    public List<Incident> ActiveIncidents { get; set; } = new();
}

public class TransparencyViewModel
{
    public decimal MoneyTotal { get; set; }
    public int GoodsTotal { get; set; }
    public List<Incident> ActiveIncidents { get; set; } = new();
    public List<CategoryTotal> GoodsByCategory { get; set; } = new();
}

public class CategoryTotal
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int ItemCount { get; set; }
}

public class TaxCertificateViewModel
{
    [Required, StringLength(20), Display(Name = "Certificate number")]
    public string CertificateNumber { get; set; } = string.Empty;

    [Required, Range(1, 100000000), Display(Name = "Amount donated (ZAR)")]
    public decimal? AmountZar { get; set; }

    // Filled in once the donation has been found.
    public MonetaryDonation? Donation { get; set; }
    public string? IssuedCertificateNumber { get; set; }
    public DateTime IssuedOn { get; set; } = DateTime.Today;
}

public class DonatePageViewModel
{
    public MonetaryDonation Money { get; set; } = new();
    public GoodsDonation Goods { get; set; } = new();
    public List<GoodsCategory> Categories { get; set; } = new();
    public string ActiveTab { get; set; } = "money";
}
