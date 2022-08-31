using HotelKata.Bookings.domain;
using HotelKata.Hotel.application;
using HotelKata.Hotel.domain;
using HotelKata.Policies.application;

namespace HotelKata.Bookings.application;

public class AddBookingUseCase
{
    private readonly HotelRepository _hotelRepository;
    private readonly CheckIsAllowedUseCase _checkIsAllowedUseCase;
    private readonly BookingRepository _bookingRepository;

    public AddBookingUseCase(HotelRepository hotelRepository, CheckIsAllowedUseCase checkIsAllowedUseCase,
        BookingRepository bookingRepository)
    {
        _checkIsAllowedUseCase = checkIsAllowedUseCase;
        _hotelRepository = hotelRepository;
        _bookingRepository = bookingRepository;
    }

    public Booking execute(Booking booking)
    {
        ValidateBooking();
        _bookingRepository.add(booking);

        return booking;
        
        void ValidateBooking()
        {
            booking.validateDates();
            ValidateHotelInformation();
            ValidateIfBookingIsAllowed();
            ValidateIfThereIsRoomAvailableInBookingPeriod();
            
            void ValidateIfThereIsRoomAvailableInBookingPeriod()
            {
                var bookingsThatClashes = _bookingRepository.retrieveBookingsFor(booking.hotelId, booking.roomType,
                    booking.checkIn,
                    booking.checkOut);
                var hotelDetails = RetrieveHotelDetails(); // TODO: Maybe do it only once?
                if (hotelDetails.GetNumberOfRoomsOf(booking.roomType) > bookingsThatClashes.Count) return;
                throw new NotAvailableRoomBookingException();
            }

            void ValidateIfBookingIsAllowed()
            {
                if (_checkIsAllowedUseCase.execute(booking.employeeId, booking.roomType)) return;
                throw new InvalidBookingException();
            }

            void ValidateHotelInformation()
            {
                var hotelDetails = RetrieveHotelDetails();
                if (!hotelDetails.hasRoom(booking.roomType))
                {
                    throw new InvalidBookingException();
                }
            }
            
            FindHotelUseCase.HotelDetails RetrieveHotelDetails()
            {
                try
                {
                    return _hotelRepository.findHotelWith(booking.hotelId);
                }
                catch (Exception)
                {
                    throw new InvalidBookingException();
                }
            }
        }
    }
}