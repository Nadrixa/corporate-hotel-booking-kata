using HotelKata.Hotel.application;

namespace HotelKata.Hotel.domain;

public interface HotelRepository
{
    FindHotelUseCase.HotelDetails? findHotelWith(string id);
    void addHotelWith(string id, string name);
    void addRoomsToHotel(string hotelId, RoomType type, int number);
}