using System.Net;
using System.Net.Http.Json;
using HotelKata.Hotel.domain;
using HotelKata.Policies.infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace HotelKataTest.E2E;

public class BookingPolicyTest
{
    private readonly WebApplicationFactory<Program> _testServer;
    private readonly HttpClient _client;
    
    public BookingPolicyTest()
    {
        _testServer = new WebApplicationFactory<Program>();
        _client = _testServer.CreateClient();
    }

    [Fact]
    public async void ShouldBeAbleToAddCompanyPolicy()
    {
        var companyId = "codurance";
        var roomTypes = new[] { RoomType.Standard, RoomType.Junior };
        var response = await _client.PostAsJsonAsync(
            "policies/company",
            new { companyId, roomTypes });

        NetworkAssertions.ThenRepliedWithExpectedStatus(HttpStatusCode.Created, response.StatusCode);
        ThenCompanyPolicyHasExpectedValues(companyId, roomTypes);
    }
    
    [Fact]
    public async void ShouldBeAbleToUpdateCompanyPolicy()
    {
        const string companyId = "codurance";
        await _client.PostAsJsonAsync(
            "policies/company",
            new {
                companyId,
                roomTypes = new[] { RoomType.Standard, RoomType.Junior }
            });

        var roomTypes = new[] { RoomType.Premium };
        var response = await _client.PostAsJsonAsync(
            "policies/company",
            new {
                companyId, roomTypes
            });

        NetworkAssertions.ThenRepliedWithExpectedStatus(HttpStatusCode.Created, response.StatusCode);
        ThenCompanyPolicyHasExpectedValues(companyId, roomTypes);
    }
    
    
    [Fact]
    public async void ShouldBeAbleToAddEmployeePolicy()
    {
        const string employeeId = "pepito";
        var roomTypes = new[] { RoomType.Standard, RoomType.Junior };
        var response = await _client.PostAsJsonAsync(
            "policies/employee",
            new {
                employeeId, roomTypes
            });

        NetworkAssertions.ThenRepliedWithExpectedStatus(HttpStatusCode.Created, response.StatusCode);
        ThenEmployeePolicyHasExpectedValues(employeeId, roomTypes);
    }
    
    [Fact]
    public async void ShouldBeAbleToUpdateEmployeePolicy()
    {
        const string employeeId = "pepito";
        await _client.PostAsJsonAsync(
            "policies/employee",
            new {
                employeeId,
                roomTypes = new[] { RoomType.Standard, RoomType.Junior }
            });

        var roomTypes = new[] { RoomType.Premium };
        var response = await _client.PostAsJsonAsync(
            "policies/employee",
            new {
                employeeId, roomTypes
            });

        NetworkAssertions.ThenRepliedWithExpectedStatus(HttpStatusCode.Created, response.StatusCode);
        ThenEmployeePolicyHasExpectedValues(employeeId, roomTypes);
    }
    
    [Fact]
    public async void ShouldReturnThatBookingIsAllowedForGivenEmployeeAndRoomType()
    {
        var response = await _client.GetFromJsonAsync<BookingPolicyTest.IsAllowedResponseBody>("policies/isAllowed/pepito/rooms/standard");
        
        Assert.True(response.isAllowed);
    }

    private void ThenCompanyPolicyHasExpectedValues(string companyId, RoomType[] expectedRoomTypes)
    {
        var policiesRepository = _testServer.Services.GetService<InMemoryPoliciesRepository>();
        Assert.Equal(expectedRoomTypes, policiesRepository.PoliciesByCompany[companyId]);
    }

    private void ThenEmployeePolicyHasExpectedValues(string employeeId, RoomType[] expectedRoomTypes)
    {
        var policiesRepository = _testServer.Services.GetService<InMemoryPoliciesRepository>();
        Assert.Equal(expectedRoomTypes, policiesRepository.PoliciesByEmployee[employeeId]);
    }

    public record IsAllowedResponseBody(bool isAllowed);
}