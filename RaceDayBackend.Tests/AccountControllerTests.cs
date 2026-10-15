using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using RaceDayBackend.Controllers;
using RaceDayBackend.Data;
using RaceDayBackend.DTOs;
using RaceDayBackend.Models;
using Xunit;

namespace RaceDayBackend.Tests
{
    public class AccountControllerTests
    {
        private DbContextOptions<RaceDayDBContext> GetInMemoryOptions()
        {
            return new DbContextOptionsBuilder<RaceDayDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private AccountController GetControllerWithMockedSession(RaceDayDBContext context, int? mockUserId = null)
        {
            var controller = new AccountController(context);
            var mockSession = new Mock<ISession>();
            // Mock session behavior if a user is "logged in"
            if (mockUserId.HasValue)
            {
                var bytes = BitConverter.GetBytes(mockUserId.Value);
                mockSession.Setup(s => s.TryGetValue("UserId", out bytes)).Returns(true);
            }

            var httpContext = new DefaultHttpContext { Session = mockSession.Object };
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
            return controller;
        }

        [Fact]
        public async Task Register_ValidUser_ReturnsCreated()
        {
            // Arrange
            var options = GetInMemoryOptions();
            using var context = new RaceDayDBContext(options);
            // Seed a role
            context.Roles.Add(new Role { RoleID = 1, RoleName = "Organiser" });
            await context.SaveChangesAsync();

            var controller = GetControllerWithMockedSession(context);
            var dto = new RegisterDTO
            {
                FullName = "Test User",
                EmailAddress = "test@new.com",
                Password = "Password123",
                RoleId = 1
            };

            // Act
            var result = await controller.Register(dto);

            // Assert
            Assert.IsType<CreatedResult>(result);
            Assert.Equal(1, context.Users.Count());
        }

        [Fact]
        public async Task GetMe_UnauthenticatedRequest_ReturnsUnauthorized()
        {
            // Arrange
            var options = GetInMemoryOptions();
            using var context = new RaceDayDBContext(options);
            // Do not pass a mockUserId, simulating no active session
            var controller = GetControllerWithMockedSession(context);

            // Act
            var result = await controller.GetMe();

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("No valid session provided.", unauthorizedResult.Value);
        }
    }
}
