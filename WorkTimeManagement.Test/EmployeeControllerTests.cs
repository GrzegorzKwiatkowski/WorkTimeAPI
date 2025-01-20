using WorkTimeManagementAPI.Controllers;
using WorkTimeManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Moq;


public class EmployeesControllerTests
{
    [Fact]
    public async Task GetEmployee_ShouldReturnEmployee_WhenIdIsValid()
    {
        // Arrange
        var employeeId = 1;
        var mockService = new Mock<IEmployeeRepository>();
        mockService.Setup(service => service.GetEmployeeByIdAsync(employeeId))
                   .ReturnsAsync(new Employee
                   {
                       Id = 1,
                       FirstName = "Jan",
                       LastName = "Kowalski",
                       Email = "jan.kowalski@example.com"
                   });

        var controller = new EmployeeController(mockService.Object);

        // Act
        var result = await controller.GetEmployeeById(employeeId);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        var employee = okResult.Value as Employee;
        employee.Should().NotBeNull();
        employee.FirstName.Should().Be("Jan");
    }

    [Fact]
    public async Task GetEmployee_ShouldReturnNotFound_WhenIdIsInvalid()
    {
        // Arrange
        var employeeId = 99;
        var mockService = new Mock<IEmployeeRepository>();
        mockService.Setup(service => service.GetEmployeeByIdAsync(employeeId))
               .ReturnsAsync((Employee?)null); // Brak pracownika

        var controller = new EmployeeController(mockService.Object);

        // Act
        var result = await controller.GetEmployeeById(employeeId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}
