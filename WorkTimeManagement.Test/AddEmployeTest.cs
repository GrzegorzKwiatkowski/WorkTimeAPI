using Microsoft.AspNetCore.Mvc.Testing;
using WorkTimeManagementAPI.Models;
using System.Net.Http.Json;
using FluentAssertions;
using System.Net;


public class AddEmployeeTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AddEmployeeTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
public async Task AddDuplicateTimeEntry_ShouldReturnConflict()
{
    // Arrange
    var client = _factory.CreateClient();
    var employeeId = 1;
    var timeEntry = new
    {
        Date = "2024-12-01",
        HoursWorked = 8
    };

    await client.PostAsJsonAsync($"/api/employees/{employeeId}/time-entries", timeEntry);

    // Act
    var response = await client.PostAsJsonAsync($"/api/employees/{employeeId}/time-entries", timeEntry);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Conflict);
}
    [Fact]
    public async Task AddAndGetEmployee_ShouldReturnEmployee()
    {
        // Arrange
        var client = _factory.CreateClient();
        var newEmployee = new
        {
            FirstName = "Michał",
            LastName = "Kiełbasa",
            Email = "michal.kielbasa@ketchup.com"
        };

        // Act
        var postResponse = await client.PostAsJsonAsync("/api/employees", newEmployee);
        var getResponse = await client.GetAsync("/api/employees");

        // Assert
        postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var employees = await getResponse.Content.ReadFromJsonAsync<List<Employee>>();
        employees.Should().ContainSingle(e =>
            e.FirstName == newEmployee.FirstName &&
            e.LastName == newEmployee.LastName &&
            e.Email == newEmployee.Email);
    }


}
