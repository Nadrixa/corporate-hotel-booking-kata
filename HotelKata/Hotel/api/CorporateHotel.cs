using HotelKata.Hotel.application;
using HotelKata.Hotel.domain;
using HotelKata.Hotel.infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace HotelKata.Hotel.api;

[ApiController]
[Route("hotels")]
public class CorporateHotel : ControllerBase
{
    private readonly FindHotelUseCase _findHotelUseCase;
    private readonly AddHotelUseCase _addHotelUseCase;
    private readonly AddRoomToHotelUseCase _addRoomToHotelUseCase;

    public CorporateHotel(InMemoryHotelRepository inMemoryHotelRepository)
    {
        _addRoomToHotelUseCase = new AddRoomToHotelUseCase(inMemoryHotelRepository);
        _addHotelUseCase = new AddHotelUseCase(inMemoryHotelRepository);
        _findHotelUseCase = new FindHotelUseCase(inMemoryHotelRepository);
    }

    [HttpPost]
    public IActionResult AddHotel(HotelCreationBody hotelData)
    {
        _addHotelUseCase.execute(hotelData.hotelId, hotelData.hotelName);
        return StatusCode(201);
    }
    
    [HttpPost("{hotelId}/rooms")]
    public IActionResult AddRoomsToHotel(String hotelId, RoomData roomData)
    {
        _addRoomToHotelUseCase.execute(hotelId, roomData);
        return StatusCode(200);
    }
    
    [HttpGet("{hotelId}")]
    public FindHotelUseCase.HotelDetails GetHotelWith(String hotelId)
    {
        return _findHotelUseCase.execute(hotelId);
    }

    public record HotelCreationBody(string hotelId, string hotelName);
    public record RoomData(RoomType type, int number);
}