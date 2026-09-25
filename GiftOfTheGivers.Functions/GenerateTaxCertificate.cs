using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using GiftOfTheGivers.Helpers;

namespace GiftOfTheGivers.Functions;

public class GenerateTaxCertificate
{
    private readonly ILogger<GenerateTaxCertificate> _logger;

    public GenerateTaxCertificate(ILogger<GenerateTaxCertificate> logger)
    {
        _logger = logger;
    }

    [Function("GenerateTaxCertificate")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        if (!int.TryParse(req.Query["donationId"], out var donationId) || donationId <= 0)
        {
            return new BadRequestObjectResult("donationId must be a positive number.");
        }

        if (!decimal.TryParse(req.Query["amount"], out var amount) || amount <= 0)
        {
            return new BadRequestObjectResult("amount must be greater than zero.");
        }

        var donorName = req.Query["donorName"].ToString();
        var isAnonymous = string.Equals(req.Query["anonymous"], "true", StringComparison.OrdinalIgnoreCase);
        var displayName = isAnonymous || string.IsNullOrWhiteSpace(donorName) ? "Anonymous" : donorName;
        var certificateNumber = DonationHelper.FormatCertificateNumber(donationId, DateTime.Today);

        _logger.LogInformation("Tax certificate {CertificateNumber} created for donation {DonationId}.", certificateNumber, donationId);

        return new OkObjectResult(new
        {
            certificateNumber,
            donorName = displayName,
            amount,
            message = "Dummy tax certificate generated."
        });
    }
}