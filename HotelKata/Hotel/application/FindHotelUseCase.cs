using HotelKata.Hotel.domain;

namespace HotelKata.Hotel.application;

public class FindHotelUseCase
{
    private readonly HotelRepository _hotelRepository;

    public FindHotelUseCase(HotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    public HotelDetails execute(string id)
    {
        return _hotelRepository.find(id);
    }
    
    public record HotelDetails(string Name, Dictionary<RoomType, int> Rooms);
}