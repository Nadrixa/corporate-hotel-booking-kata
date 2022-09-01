using HotelKata.Hotel.domain;

namespace HotelKata.Bookings.domain;

public interface BookingRepository
{
    public List<Booking> retrieveBookingsFor(string hotelId, RoomType roomType, long checkIn, long checkOut);
    void add(Booking booking);
    void deleteBookingsOf(string employeeId);
}