using Xunit;
using Moq;
using HelloWorldWebFordjour.Controllers;
using HelloWorldWebFordjour.Interfaces;
using HelloWorldWebFordjour.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // For DefaultHttpContext
using Microsoft.AspNetCore.Mvc.ViewFeatures; // For TempDataDictionary
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorldWebFordjour.Tests.Controllers
{
    public class TicketsControllerTests
    {
        // Helper method to create some consistent test data
        private List<Ticket> GetTestTickets()
        {
            return new List<Ticket>
            {
                new Ticket { Id = 1, Name = "Implement Favorites", Description = "Desc 1", SprintNumber = 1, PointValue = 8, Status = TicketStatus.Done },
                new Ticket { Id = 2, Name = "Create ToDo Module", Description = "Desc 2", SprintNumber = 2, PointValue = 10, Status = TicketStatus.InProgress },
                new Ticket { Id = 3, Name = "Design Landing Page", Description = "Desc 3", SprintNumber = 2, PointValue = 5, Status = TicketStatus.ToDo },
                new Ticket { Id = 4, Name = "Fix Footer", Description = "Desc 4", SprintNumber = 1, PointValue = 2, Status = TicketStatus.QA }
            };
        }

        // Add a setup method or inline setup for TempData
        private TicketsController CreateControllerWithMockRepo(Mock<ITicketRepository> mockRepo)
        {
            var controller = new TicketsController(mockRepo.Object);

            // Initialize HttpContext and TempData for the controller
            // Needed because TempData relies on HttpContext and ITempDataProvider
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            // ITempDataProvider implementation for TempData to work.
                var tempDataDictionary = new TempDataDictionary(
                controller.ControllerContext.HttpContext,
                Mock.Of<ITempDataProvider>());

            controller.TempData = tempDataDictionary;

            return controller;
        }


        // Test 1: Index action returns a view with a list of tickets
        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfTickets()
        {
            // Arrange
            var mockRepo = new Mock<ITicketRepository>();
            mockRepo.Setup(repo => repo.GetAllTicketsAsync())
                    .ReturnsAsync(GetTestTickets());

            // Use the new setup method
            var controller = CreateControllerWithMockRepo(mockRepo);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Ticket>>(viewResult.Model);
            Assert.Equal(4, model.Count());
            mockRepo.Verify(repo => repo.GetAllTicketsAsync(), Times.Once);
        }

        // Test 2: Create (POST) with valid model adds a ticket and redirects to Index
        [Fact]
        public async Task Create_Post_ValidModel_AddsTicketAndRedirectsToIndex()
        {
            // Arrange
            var mockRepo = new Mock<ITicketRepository>();
            var newTicket = new Ticket { Name = "New Ticket", Description = "Test Desc", SprintNumber = 3, PointValue = 5, Status = TicketStatus.ToDo };

            mockRepo.Setup(repo => repo.AddTicketAsync(It.IsAny<Ticket>()))
                    .Returns(Task.CompletedTask);

            // Use the new setup method
            var controller = CreateControllerWithMockRepo(mockRepo);

            // Act
            var result = await controller.Create(newTicket);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mockRepo.Verify(repo => repo.AddTicketAsync(It.Is<Ticket>(t => t.Name == newTicket.Name)), Times.Once);
        }

        // Test 3: Create (POST) with invalid model returns view with same model and doesn't add ticket
        [Fact]
        public async Task Create_Post_InvalidModel_ReturnsViewWithTicketAndDoesNotAdd()
        {
            // Arrange
            var mockRepo = new Mock<ITicketRepository>();
            var invalidTicket = new Ticket { Name = null, Description = "Test Desc", SprintNumber = 3, PointValue = 5, Status = TicketStatus.ToDo };

            // Use the new setup method
            var controller = CreateControllerWithMockRepo(mockRepo);
            controller.ModelState.AddModelError("Name", "Ticket Name is required."); // Manually add a model error

            // Act
            var result = await controller.Create(invalidTicket);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Ticket>(viewResult.Model);
            Assert.Null(model.Name);
            mockRepo.Verify(repo => repo.AddTicketAsync(It.IsAny<Ticket>()), Times.Never);
        }

        // Test 4: Edit (GET) returns NotFound for null ID
        [Fact]
        public async Task Edit_Get_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            var mockRepo = new Mock<ITicketRepository>();
            var controller = CreateControllerWithMockRepo(mockRepo); 

            // Act
            var result = await controller.Edit(null); // Pass null ID

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // Test 5: Edit (GET) returns NotFound for valid but non-existent ID
        [Fact]
        public async Task Edit_Get_ReturnsNotFound_WhenTicketNotFound()
        {
            // Arrange
            var mockRepo = new Mock<ITicketRepository>();
            mockRepo.Setup(repo => repo.GetTicketByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync((Ticket?)null); // Simulate ticket not found

            var controller = CreateControllerWithMockRepo(mockRepo); 

            // Act
            var result = await controller.Edit(99); // Request a non-existent ID

            // Assert
            Assert.IsType<NotFoundResult>(result);
            mockRepo.Verify(repo => repo.GetTicketByIdAsync(99), Times.Once);
        }

        // Test 6: Edit (GET) returns View with Ticket for existent ID
        [Fact]
        public async Task Edit_Get_ReturnsViewResult_WithTicket()
        {
            // Arrange
            var mockRepo = new Mock<ITicketRepository>();
            var existingTicket = GetTestTickets().First();
            mockRepo.Setup(repo => repo.GetTicketByIdAsync(existingTicket.Id))
                    .ReturnsAsync(existingTicket);

            var controller = CreateControllerWithMockRepo(mockRepo); 

            // Act
            var result = await controller.Edit(existingTicket.Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Ticket>(viewResult.Model);
            Assert.Equal(existingTicket.Id, model.Id);
            mockRepo.Verify(repo => repo.GetTicketByIdAsync(existingTicket.Id), Times.Once);
        }

        // Test 7: Edit (POST) with valid model updates ticket and redirects to Index
        [Fact]
        public async Task Edit_Post_ValidModel_UpdatesTicketAndRedirectsToIndex()
        {
            // Arrange
            var mockRepo = new Mock<ITicketRepository>();
            var existingTicket = GetTestTickets().First();
            existingTicket.Name = "Updated Name for Test"; // Modify a property

            // Setup TicketExistsAsync to return true (important for the catch block logic in the controller)
            mockRepo.Setup(repo => repo.TicketExistsAsync(existingTicket.Id))
                    .ReturnsAsync(true);
            // Setup UpdateTicketAsync to complete successfully
            mockRepo.Setup(repo => repo.UpdateTicketAsync(It.IsAny<Ticket>()))
                    .Returns(Task.CompletedTask);

            var controller = CreateControllerWithMockRepo(mockRepo);

            // Act
            var result = await controller.Edit(existingTicket.Id, existingTicket);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mockRepo.Verify(repo => repo.UpdateTicketAsync(It.Is<Ticket>(t => t.Name == "Updated Name for Test")), Times.Once);
        }

        // Test 8: Edit (POST) returns NotFound if ID in route does not match model ID
        [Fact]
        public async Task Edit_Post_IdMismatch_ReturnsNotFound()
        {
            // Arrange
            var mockRepo = new Mock<ITicketRepository>();
            var ticket = GetTestTickets().First();
            var controller = CreateControllerWithMockRepo(mockRepo); 

            // Act
            var result = await controller.Edit(999, ticket); // Mismatched IDs

            // Assert
            Assert.IsType<NotFoundResult>(result);
            mockRepo.Verify(repo => repo.UpdateTicketAsync(It.IsAny<Ticket>()), Times.Never);
        }

        // Test 9: DeleteConfirmed (POST) removes ticket and redirects to Index
        [Fact]
        public async Task DeleteConfirmed_RemovesTicketAndRedirectsToIndex()
        {
            // Arrange
            var mockRepo = new Mock<ITicketRepository>();
            var ticketToDeleteId = 1;

            mockRepo.Setup(repo => repo.DeleteTicketAsync(ticketToDeleteId))
                    .Returns(Task.CompletedTask);

            var controller = CreateControllerWithMockRepo(mockRepo);

            // Act
            var result = await controller.DeleteConfirmed(ticketToDeleteId);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mockRepo.Verify(repo => repo.DeleteTicketAsync(ticketToDeleteId), Times.Once);
            // Verify TempData 
            Assert.Equal("Ticket deleted successfully!", controller.TempData["Message"]);
        }
    }
}