using GiftOfTheGivers.Helpers;
using Xunit;

namespace GiftOfTheGivers.Tests
{
    public class DonationHelperTests
    {
        [Theory]
        [InlineData(1, 2024, 6, 1, "GOTG-2024-000001")]
        [InlineData(123, 2024, 6, 1, "GOTG-2024-000123")]
        [InlineData(1234567, 2026, 11, 2, "GOTG-2026-1234567")]
        public void FormatCertificateNumber_ReturnsCorrectFormat(int donationId, int year, int month, int day, string expected)
        {
            // Arrange
            var donationDate = new DateTime(year, month, day);

            // Act
            var result = DonationHelper.FormatCertificateNumber(donationId, donationDate);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
