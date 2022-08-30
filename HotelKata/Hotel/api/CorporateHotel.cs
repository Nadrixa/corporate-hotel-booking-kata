using HotelKata.Hotel.application;
using HotelKata.Hotel.infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace HotelKata.Hotel.api;

[ApiController]
[Route("hotels")]
public class CorporateHotel : ControllerBase
{
    private readonly FindHotelUseCase _findHotelUseCase;
    private readonly AddHotelUseCase _addHotelUseCase;

    public CorporateHotel(InMemoryHotelRepository inMemoryHotelRepository)
    {
        _addHotelUseCase = new AddHotelUseCase(inMemoryHotelRepository);
        _findHotelUseCase = new FindHotelUseCase(inMemoryHotelRepository);
    }

    [HttpPost]
    public IActionResult addHotel(HotelCreationBody hotelData)
    {
        _addHotelUseCase.execute(hotelData);
        return StatusCode(201);
    }
    
    [HttpGet("{id}")]
    public FindHotelUseCase.HotelDetails getHotel(String id)
    {
        return _findHotelUseCase.execute(id);
    }

    public record HotelCreationBody(string hotelId, string hotelName);
}