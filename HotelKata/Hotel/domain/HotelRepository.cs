using HotelKata.Hotel.application;

namespace HotelKata.Hotel.domain;

public interface HotelRepository
{
    FindHotelUseCase.HotelDetails find(string id);
    void add(string id, string name);
}