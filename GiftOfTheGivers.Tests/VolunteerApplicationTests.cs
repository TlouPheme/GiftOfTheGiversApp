using GiftOfTheGiversApp.Models;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace GiftOfTheGivers.Tests
{
    public class VolunteerApplicationTests
    {
        [Fact]
        public void CompleteApplication_IsValid()
        {
            // Arrange
            var application = new VolunteerApplication
            {
                FullName = "John Doe",
                Phone = "0821234567",
                City = "Cape Town",
                Email = "john.doe@example.com"
            };
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(
                application,
                new ValidationContext(application),
                results,
                true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(results);
        }

        [Fact]
        public void MissingRequiredFields_AreRejected()
        {
            // Arrange
            var application = new VolunteerApplication();
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(
                application,
                new ValidationContext(application),
                results,
                true);

            // Assert
            Assert.False(isValid);
            Assert.Equal(3, results.Count);
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(VolunteerApplication.FullName)));
        }

        [Fact]
        public void BadEmail_IsRejected()
        {
            // Arrange
            var application = new VolunteerApplication
            {
                FullName = "John Doe",
                Phone = "0821234567",
                City = "Cape Town",
                Email = "not-an-email"
            };
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(
                application,
                new ValidationContext(application),
                results,
                true);

            // Assert
            Assert.False(isValid);
        }
    }
}