using HotelKata.Hotel.domain;

namespace HotelKata.Hotel.application;

public class AddHotelUseCase
{
    private readonly HotelRepository _hotelsRepository;

    public AddHotelUseCase(HotelRepository hotelsRepository)
    {
        _hotelsRepository = hotelsRepository;
    }

    public void execute(string hotelId, string hotelName)
    {
        _hotelsRepository.addHotelWith(hotelId, hotelName);
    }
}