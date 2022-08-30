using System.Net;
using System.Net.Http.Json;
using HotelKata.Hotel.domain;
using HotelKata.Policies.infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace HotelKataTest.E2E;

public class BookingPolicyTest
{

    [Fact]
    public async void ShouldBeAbleToAddCompanyPolicy()
    {
        var testServer = new WebApplicationFactory<Program>();
        var client = testServer.CreateClient();
        
        var response = await client.PostAsJsonAsync(
            "policies/company",
            new {
                companyId = "codurance",
                roomTypes = new[] { RoomType.Standard, RoomType.Junior }
            });

        thenRepliedWith201();
        thenCompanyPolicyHasBeenCreated();

        void thenCompanyPolicyHasBeenCreated()
        {
            var policiesRepository = testServer.Services.GetService<InMemoryPoliciesRepository>();
            Assert.Equal(new[] { RoomType.Standard, RoomType.Junior}, policiesRepository.PoliciesByCompany["codurance"]);
        }

        void thenRepliedWith201()
        {
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
    
    [Fact]
    public async void ShouldBeAbleToUpdateCompanyPolicy()
    {
        var testServer = new WebApplicationFactory<Program>();
        var client = testServer.CreateClient(); 
        await client.PostAsJsonAsync(
            "policies/company",
            new {
                companyId = "codurance",
                roomTypes = new[] { RoomType.Standard, RoomType.Junior }
            });

        var response = await client.PostAsJsonAsync(
            "policies/company",
            new {
                companyId = "codurance",
                roomTypes = new[] { RoomType.Premium }
            });
        
        thenRepliedWith201();
        thenCompanyPolicyHasBeenCreated();

        void thenCompanyPolicyHasBeenCreated()
        {
            var policiesRepository = testServer.Services.GetService<InMemoryPoliciesRepository>();
            Assert.Equal(new[] { RoomType.Premium }, policiesRepository.PoliciesByCompany["codurance"]);
        }

        void thenRepliedWith201()
        {
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}