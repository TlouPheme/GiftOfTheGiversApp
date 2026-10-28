using GiftOfTheGivers.Helpers;
using Xunit;

namespace GiftOfTheGivers.Tests
{
    public class DonationHelperTests
    {
        [Theory]
        [InlineData(1, 2024, 6, 1, "GOTG-2024-000001")]
        [InlineData(2, 2024, 6, 1, "GOTG-2024-000002")]
        [InlineData(123467, 2024, 6, 1, "GOTG-2024-123467")]

        public void FormatCertificateNumber_ReturnsCorrectFormat(int certificateNumber, int year, int month, int day, string expected)
        {
            var result = DonationHelper.FormatCertificateNumber(certificateNumber, new DateTime(year, month, day));
            Assert.Equal(expected, result);
        }
    }
}
