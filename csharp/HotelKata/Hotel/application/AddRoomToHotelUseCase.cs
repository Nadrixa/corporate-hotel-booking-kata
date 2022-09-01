using HotelKata.Hotel.api;
using HotelKata.Hotel.domain;

namespace HotelKata.Hotel.application;

public class AddRoomToHotelUseCase
{
    private readonly HotelRepository _hotelRepository;

    public AddRoomToHotelUseCase(HotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    public void execute(string hotelId, CorporateHotel.RoomData roomData)
    {
        if (_hotelRepository.findHotelWith(hotelId) is null) throw new NotExistingHotelException();
        
        _hotelRepository.addRoomsToHotel(hotelId, roomData.type, roomData.number);
    }
}