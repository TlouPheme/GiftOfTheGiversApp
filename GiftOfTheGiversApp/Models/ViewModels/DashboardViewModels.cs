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

public class DonatePageViewModel
{
    public MonetaryDonation Money { get; set; } = new();
    public GoodsDonation Goods { get; set; } = new();
    public List<GoodsCategory> Categories { get; set; } = new();
    public string ActiveTab { get; set; } = "money";
}
