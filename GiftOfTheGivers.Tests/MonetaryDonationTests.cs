using GiftOfTheGiversApp.Models;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace GiftOfTheGivers.Tests
{
    public class MonetaryDonationTests
    {
        [Theory]
        [InlineData(false, "John Doe", "John Doe")]
        [InlineData(false, null, "Anonymous")]          
        [InlineData(true, "Jane Smith", "Anonymous")]
        public void DisplayName_ReturnsExpectedName(bool isAnonymous, string? donorName, string expectedDisplayName) 
        {
            var donation = new MonetaryDonation { IsAnonymous = isAnonymous, DonorName = donorName };  

            var result = donation.DisplayName;          

            Assert.Equal(expectedDisplayName, result); 
        }

        [Fact]
        public void AmountZar_Zero_IsInvalid()
        {
            // Arrange
            var donation = new MonetaryDonation { AmountZar = 0 };
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(
                donation,
                new ValidationContext(donation),
                results,
                true);

            // Assert
            Assert.False(isValid);
            Assert.NotEmpty(results);
        }

        [Fact]
        public void AmountZar_Positive_IsValid()
        {
            // Arrange
            var donation = new MonetaryDonation { AmountZar = 500 };
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(
                donation,
                new ValidationContext(donation),
                results,
                true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(results);
        }
    }
}