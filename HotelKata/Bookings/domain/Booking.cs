using HotelKata.Hotel.domain;

namespace HotelKata.Bookings.domain;

public record Booking(string employeeId, string hotelId, RoomType roomType, long checkIn, long checkOut)
{
    public void validateDates()
    {
        const int dayInMilliseconds = 24 * 3600 * 1000;
        if (checkOut - checkIn < dayInMilliseconds)
        {
            throw new InvalidBookingException();
        }
    }
}

public class InvalidBookingException : Exception
{
}

public class NotAvailableRoomBookingException : Exception
{
}