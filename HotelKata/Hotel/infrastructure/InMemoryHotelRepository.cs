using HotelKata.Hotel.application;
using HotelKata.Hotel.domain;

namespace HotelKata.Hotel.infrastructure;

public class InMemoryHotelRepository : HotelRepository
{
    private readonly Dictionary<string, FindHotelUseCase.HotelDetails> _hotels;

    public InMemoryHotelRepository()
    {
        _hotels = new Dictionary<string, FindHotelUseCase.HotelDetails>();
    }

    public FindHotelUseCase.HotelDetails find(string id)
    {
        return _hotels[id];
    }

    public void add(string id, string name)
    {
        _hotels.Add(id, new FindHotelUseCase.HotelDetails(name, new Dictionary<RoomType, int>()));
    }
}