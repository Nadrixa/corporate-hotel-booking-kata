using System.Net;
using System.Net.Http.Json;
using HotelKata.Bookings.domain;
using HotelKata.Bookings.infrastructure;
using HotelKata.Hotel.domain;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace HotelKataTest.E2E;

public class BookingTest
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _testServer;

    public BookingTest()
    {
        _testServer = new WebApplicationFactory<Program>();
        _client = _testServer.CreateClient();
    }

    [Fact]
    public async void ShouldBeAbleToBookARoom()
    {
        var (employeeId, hotelId, roomType, checkIn, checkOut) = GivenBookingInformation();
        await GivenHotelCreatedWithRooms(); 
        
        var response = await _client.PostAsJsonAsync(
            "bookings",
            new {
                employeeId,
                hotelId,
                roomType,
                checkIn,
                checkOut
            });
        
        NetworkAssertions.ThenRepliedWithExpectedStatus(HttpStatusCode.Created, response.StatusCode);
        ThenBookingShouldBeAdded();

        async Task GivenHotelCreatedWithRooms()
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
                    type = roomType,
                    number = 5
                });
        }

        void ThenBookingShouldBeAdded()
        {
            var inMemoryBookingsRepository = _testServer.Services.GetService<InMemoryBookingsRepository>();
            Assert.True(inMemoryBookingsRepository?.Bookings.Count == 1);
            Assert.Equal(new Booking(employeeId, hotelId, roomType, checkIn, checkOut), inMemoryBookingsRepository?.Bookings[0]);
        }
    }
    
    [Fact]
    public async void ShouldReplyWithBadRequestAndNotAddBookingWhenThereIsNotRoomAvailable()
    {
        var (employeeId, hotelId, roomType, checkIn, checkOut) = GivenBookingInformation();
        await GivenHotelCreatedWithRooms();
        
        await _client.PostAsJsonAsync(
            "bookings",
            new
            {
                employeeId,
                hotelId,
                roomType,
                checkIn,
                checkOut
            });
        
        var response = await _client.PostAsJsonAsync(
            "bookings",
            new
            {
                employeeId,
                hotelId,
                roomType,
                checkIn,
                checkOut
            });
        
        NetworkAssertions.ThenRepliedWithExpectedStatus(HttpStatusCode.BadRequest, response.StatusCode);
        ThenBookingShouldNotBeAdded();

        void ThenBookingShouldNotBeAdded()
        {
            var inMemoryBookingsRepository = _testServer.Services.GetService<InMemoryBookingsRepository>();
            Assert.True(inMemoryBookingsRepository?.Bookings.Count == 1);
        }

        async Task GivenHotelCreatedWithRooms()
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
                    type = roomType,
                    number = 1
                });
        }
    }
    
    [Fact]
    public async void ShouldReplyWithBadRequestAndNotAddBookingWhenAnythingGoesWrong()
    {
        var (employeeId, hotelId, roomType, checkIn, checkOut) = GivenBookingInformation();
        
        var response = await _client.PostAsJsonAsync(
            "bookings",
            new
            {
                employeeId,
                hotelId,
                roomType,
                checkIn,
                checkOut
            });
        
        NetworkAssertions.ThenRepliedWithExpectedStatus(HttpStatusCode.BadRequest, response.StatusCode);
        ThenBookingShouldNotBeAdded();

        void ThenBookingShouldNotBeAdded()
        {
            var inMemoryBookingsRepository = _testServer.Services.GetService<InMemoryBookingsRepository>();
            Assert.Empty(inMemoryBookingsRepository?.Bookings);
        }
    }

    private (string, string, RoomType, long, long) GivenBookingInformation()
    {
        return (
            "pepito",
            "palace",
            RoomType.Standard,
            new DateTimeOffset(new DateTime(2022, 9, 1)).ToUnixTimeMilliseconds(),
            new DateTimeOffset(new DateTime(2022, 9, 2)).ToUnixTimeMilliseconds());
    }
}