using HotelKata.Bookings.domain;
using HotelKata.Hotel.domain;

namespace HotelKata.Bookings.infrastructure;

public class InMemoryBookingsRepository : BookingRepository
{
    
    public List<Booking> Bookings { get; }
    
    public InMemoryBookingsRepository()
    {
        Bookings = new List<Booking>();
    }

    public List<Booking> retrieveBookingsFor(string hotelId, RoomType roomType, long checkIn, long checkOut)
    {
        return Bookings.FindAll(BookingOfSameHotelAndRoomBetweenBookingDates());

        Predicate<Booking> BookingOfSameHotelAndRoomBetweenBookingDates()
        {
            return booking => booking.hotelId == hotelId && booking.roomType == roomType && IsBetweenNewBookingDates(booking);
            
            bool IsBetweenNewBookingDates(Booking existingBooking)
            {
                return IsDateBetweenCheckInAndCheckOut(existingBooking.checkIn, checkIn, checkOut) || IsDateBetweenCheckInAndCheckOut(existingBooking.checkOut, checkIn, checkOut);

                bool IsDateBetweenCheckInAndCheckOut(long date, long checkIn, long checkOut)
                {
                    return checkIn <= date && date <= checkOut;
                }
            }
        }
    }

    public void add(Booking booking)
    {
        Bookings.Add(booking);
    }

    public void deleteBookingsOf(string employeeId)
    {
        Bookings.RemoveAll(booking => booking.employeeId == employeeId);
    }
}