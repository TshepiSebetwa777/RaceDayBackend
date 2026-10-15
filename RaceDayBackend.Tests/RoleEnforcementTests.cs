using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

using Moq;

using RaceDayBackend.Controllers;

using RaceDayBackend.Data;

using RaceDayBackend.DTOs;

using RaceDayBackend.Models;

using System.Text;

using Xunit;

namespace RaceDayBackend.Tests

{

    public class RoleEnforcementTests

    {

        private DbContextOptions<RaceDayDBContext> GetInMemoryOptions()

        {

            return new DbContextOptionsBuilder<RaceDayDBContext>()

                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())

                .Options;

        }

        // Helper to fake the Session Cookie for specific roles

        private ControllerContext GetMockedContext(int userId, string role)

        {

            var mockSession = new Mock<ISession>();

            var userIdBytes = BitConverter.GetBytes(userId);

            mockSession.Setup(s => s.TryGetValue("UserId", out userIdBytes)).Returns(true);

            var roleBytes = Encoding.UTF8.GetBytes(role);

            mockSession.Setup(s => s.TryGetValue("Role", out roleBytes)).Returns(true);

            return new ControllerContext

            {

                HttpContext = new DefaultHttpContext { Session = mockSession.Object }

            };

        }

        [Fact]

        public async Task CreateEvent_AsOrganiser_ReturnsCreated()

        {

            // Arrange

            var options = GetInMemoryOptions();

            using var context = new RaceDayDBContext(options);

            var controller = new EventsController(context)

            {

                ControllerContext = GetMockedContext(userId: 1, role: "Organiser")

            };

            var dto = new EventDTO { Title = "Organiser Event", ScheduledDate = DateTime.Now, Description = "Valid" };

            // Act

            var result = await controller.CreateEvent(dto);

            // Assert

            Assert.IsType<CreatedResult>(result);

        }

        [Fact]

        public async Task CreateEvent_AsParticipant_ReturnsForbidden()

        {

            // Arrange

            var options = GetInMemoryOptions();

            using var context = new RaceDayDBContext(options);

            var controller = new EventsController(context)

            {

                ControllerContext = GetMockedContext(userId: 2, role: "Participant")

            };

            var dto = new EventDTO { Title = "Hacked Event", ScheduledDate = DateTime.Now, Description = "Invalid" };

            // Act

            var result = await controller.CreateEvent(dto);

            // Assert

            var objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal(403, objectResult.StatusCode); // 403 Forbidden proves role rejection works

        }

        [Fact]

        public async Task Enrol_AsParticipant_ReturnsCreated()

        {

            // Arrange

            var options = GetInMemoryOptions();

            using var context = new RaceDayDBContext(options);

            var controller = new EnrolmentsController(context)

            {

                ControllerContext = GetMockedContext(userId: 2, role: "Participant")

            };

            var dto = new EnrolmentDTO { CategoryId = 1 };

            // Act

            var result = await controller.Enrol(dto);

            // Assert

            Assert.IsType<CreatedResult>(result);

        }

    }

}
