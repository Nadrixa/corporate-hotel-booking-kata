using System.Net;
using System.Net.Http.Json;
using HotelKata.Hotel.application;
using HotelKata.Hotel.domain;
using Microsoft.AspNetCore.Mvc.Testing;

namespace HotelKataTest.E2E;

public class CorporateHotelTest
{
    private readonly HttpClient _client;

    public CorporateHotelTest()
    {
        var testServer = new WebApplicationFactory<Program>();
        _client = testServer.CreateClient();
    }

    [Fact]
    public async Task ShouldBeAbleToAddAndRetrieveHotelInformation()
    {
        var hotelInformation = new
        {
            hotelId = "1",
            hotelName = "Palace"
        };
        var expectedHotelInformation = new FindHotelUseCase.HotelDetails("Palace", new Dictionary<RoomType, int>());
        
        var response = await _client.PostAsJsonAsync("hotels", hotelInformation);
        var (name, rooms) = (await _client.GetFromJsonAsync<FindHotelUseCase.HotelDetails>("hotels/1"))!;
        
        thenHotelCreationRepliedWith201();
        thenHotelInformationIsTheExpected();

        void thenHotelCreationRepliedWith201()
        {
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        void thenHotelInformationIsTheExpected()
        {
            Assert.Equal(
                expectedHotelInformation.Name,
                name
            );
            Assert.Equal(
                expectedHotelInformation.Rooms,
                rooms);
        }
    }
}