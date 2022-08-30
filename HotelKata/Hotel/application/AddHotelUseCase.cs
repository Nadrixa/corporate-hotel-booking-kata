using HotelKata.Hotel.api;
using HotelKata.Hotel.domain;

namespace HotelKata.Hotel.application;

public class AddHotelUseCase
{
    private readonly HotelRepository _hotelsRepository;

    public AddHotelUseCase(HotelRepository hotelsRepository)
    {
        _hotelsRepository = hotelsRepository;
    }

    public void execute(CorporateHotel.HotelCreationBody hotelData)
    {
        _hotelsRepository.add(hotelData.hotelId, hotelData.hotelName);
    }
}