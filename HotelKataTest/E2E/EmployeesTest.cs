using System.Net;
using System.Net.Http.Json;
using HotelKata.Employees.infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace HotelKataTest.E2E;

public class EmployeesTest
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _testServer;

    public EmployeesTest()
    {
        _testServer = new WebApplicationFactory<Program>();
        _client = _testServer.CreateClient();
    }

    [Fact]
    public async void ShouldBeAbleToAddEmployee()
    {
        const string employeeId = "pepito";
        const string companyId = "codurance";
        var response = await _client.PostAsJsonAsync(
            "employees",
            new {
                employeeId, companyId
            });

        NetworkAssertions.ThenRepliedWithExpectedStatus(HttpStatusCode.Created, response.StatusCode);
        thenEmployeeHasBeenSaved();

        void thenEmployeeHasBeenSaved()
        {
            var employeesRepository = _testServer.Services.GetService<InMemoryEmployeesRepository>();
            Assert.Equal(companyId, employeesRepository.Employees[employeeId]);
        }
    }

    [Fact]
    public async void ShouldReplyWithBadRequestWhenTryingToAddAnExistingEmployee()
    {
        const string employeeId = "pepito";
        const string companyId = "codurance";
        await _client.PostAsJsonAsync(
            "employees",
            new {
                employeeId, companyId
            });
        
        var response = await _client.PostAsJsonAsync(
            "employees",
            new {
                employeeId,
                companyId = "coduranco"
            });

        NetworkAssertions.ThenRepliedWithExpectedStatus(HttpStatusCode.BadRequest, response.StatusCode);
        thenEmployeeShouldNotBeUpdated();

        void thenEmployeeShouldNotBeUpdated()
        {
            var employeesRepository = _testServer.Services.GetService<InMemoryEmployeesRepository>();
            Assert.Equal(companyId, employeesRepository.Employees[employeeId]);
        }
    }
}