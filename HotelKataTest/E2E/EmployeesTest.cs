using System.Net;
using System.Net.Http.Json;
using HotelKata.Employees.infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace HotelKataTest.E2E;

public class EmployeesTest
{
        
    [Fact]
    public async void ShouldBeAbleToAddEmployee()
    {
        var testServer = new WebApplicationFactory<Program>();
        var client = testServer.CreateClient();
        
        var response = await client.PostAsJsonAsync(
            "employees",
            new {
                employeeId = "pepito",
                companyId = "codurance"
            });

        thenAddEmployeeRepliedWith201();
        thenEmployeeHasBeenSaved();

        void thenEmployeeHasBeenSaved()
        {
            var employeesRepository = testServer.Services.GetService<InMemoryEmployeesRepository>();
            Assert.Equal("codurance", employeesRepository.Employees["pepito"]);
        }

        void thenAddEmployeeRepliedWith201()
        {
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
    
    [Fact]
    public async void ShouldReplyWithBadRequestWhenTryingToAddAnExistingEmployee()
    {
        var testServer = new WebApplicationFactory<Program>();
        var client = testServer.CreateClient();
        await client.PostAsJsonAsync(
            "employees",
            new {
                employeeId = "pepito",
                companyId = "codurance"
            });
        
        var response = await client.PostAsJsonAsync(
            "employees",
            new {
                employeeId = "pepito",
                companyId = "coduranco"
            });

        thenAddEmployeeRepliedWith400();
        thenEmployeeShouldNotBeUpdated();

        void thenEmployeeShouldNotBeUpdated()
        {
            var employeesRepository = testServer.Services.GetService<InMemoryEmployeesRepository>();
            Assert.Equal("codurance", employeesRepository.Employees["pepito"]);
        }

        void thenAddEmployeeRepliedWith400()
        {
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}