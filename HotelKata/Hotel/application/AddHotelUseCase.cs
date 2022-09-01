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
        if (_hotelsRepository.findHotelWith(hotelId) is not null) throw new ExistingHotelException();
        
        _hotelsRepository.addHotelWith(hotelId, hotelName);
    }
}