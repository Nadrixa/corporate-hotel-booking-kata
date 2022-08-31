using System.Net;
using System.Net.Http.Json;
using HotelKata.Bookings.infrastructure;
using HotelKata.Employees.infrastructure;
using HotelKata.Hotel.domain;
using HotelKata.Policies.infrastructure;
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
        await givenEmployeeAdded();
        
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

        async Task givenEmployeeAdded()
        {
            await _client.PostAsJsonAsync(
                "employees",
                new
                {
                    employeeId, companyId
                });
        }
    }
    
    [Fact]
    public async void ShouldDeleteEmployeeAndAssociatedPoliciesAndBookings()
    {
        const string employeeId = "pepito";
        await givenSystemInitialized();

        var response = await _client.DeleteAsync($"employees/{employeeId}");

        NetworkAssertions.ThenRepliedWithExpectedStatus(HttpStatusCode.OK, response.StatusCode);
        thenEmployeeHasBeenDeleted();

        void thenEmployeeHasBeenDeleted()
        {
            var employeesRepository = _testServer.Services.GetService<InMemoryEmployeesRepository>();
            Assert.True(employeesRepository.Employees.Count == 0);

            var policiesRepository = _testServer.Services.GetService<InMemoryPoliciesRepository>();
            Assert.True(policiesRepository.PoliciesByEmployee.Count == 0);
            
            var bookingsRepository = _testServer.Services.GetService<InMemoryBookingsRepository>();
            Assert.True(bookingsRepository.Bookings.Count == 0);
        }

        async Task givenSystemInitialized()
        {
            const string hotelId = "palace";
            
            await givenEmployeeAdded();
            await givenHotelWithRoomsAdded();
            await givenEmployeePolicyAdded();
            await givenBookingOfEmployeeAdded();

            async Task givenEmployeeAdded()
            {
                await _client.PostAsJsonAsync(
                    "employees",
                    new
                    {
                        employeeId,
                        companyId = "codurance"
                    });
            }

            async Task givenHotelWithRoomsAdded()
            {
                await _client.PostAsJsonAsync(
                    "hotels",
                    new
                    {
                        hotelId,
                        hotelName = "Palace"
                    });
                await _client.PostAsJsonAsync(
                    "hotels/palace/rooms",
                    new
                    {
                        type = RoomType.Standard,
                        number = 5
                    });
            }

            async Task givenEmployeePolicyAdded()
            {
                await _client.PostAsJsonAsync(
                    "policies/employee",
                    new
                    {
                        employeeId,
                        roomTypes = new[] { RoomType.Standard, RoomType.Junior }
                    });
            }

            async Task givenBookingOfEmployeeAdded()
            {
                await _client.PostAsJsonAsync(
                    "bookings",
                    new
                    {
                        employeeId,
                        hotelId,
                        roomType = RoomType.Standard,
                        checkIn = new DateTimeOffset(new DateTime(2022, 9, 1)).ToUnixTimeMilliseconds(),
                        checkOut = new DateTimeOffset(new DateTime(2022, 9, 2)).ToUnixTimeMilliseconds()
                    });
            }
        }
    }
    
    [Fact]
    public async void ShouldReplyWithNotFoundWhenTryingToDeleteAnEmployeeThatDoesNotExist()
    {
        const string employeeId = "pepito";

        var response = await _client.DeleteAsync($"employees/{employeeId}");

        NetworkAssertions.ThenRepliedWithExpectedStatus(HttpStatusCode.NotFound, response.StatusCode);
    }
}