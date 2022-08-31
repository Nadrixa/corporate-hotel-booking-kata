using HotelKata.Bookings.application;
using HotelKata.Bookings.domain;
using HotelKata.Bookings.infrastructure;
using HotelKata.Employees.infrastructure;
using HotelKata.Hotel.domain;
using HotelKata.Hotel.infrastructure;
using HotelKata.Policies.application;
using HotelKata.Policies.infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace HotelKata.Bookings.api;

[ApiController]
[Route("bookings")]
public class Bookings : ControllerBase
{
    private readonly AddBookingUseCase _addBookingUseCase;

    public Bookings(InMemoryHotelRepository hotelRepository, InMemoryPoliciesRepository inMemoryPoliciesRepository, InMemoryEmployeesRepository inMemoryEmployeesRepository, InMemoryBookingsRepository inMemoryBookingsRepository)
    {
        _addBookingUseCase = new AddBookingUseCase(hotelRepository, new CheckIsAllowedUseCase(inMemoryPoliciesRepository, inMemoryEmployeesRepository), inMemoryBookingsRepository);
    }

    [HttpPost]
    public IActionResult addBooking(BookingBody bookingBody)
    {
        try
        {
            _addBookingUseCase.execute(
                new Booking(
                    bookingBody.employeeId,
                    bookingBody.hotelId,
                    bookingBody.roomType,
                    bookingBody.checkIn,
                    bookingBody.checkOut
                ));
            return StatusCode(201);
        }
        catch (Exception ex) when (ex is InvalidBookingException or NotAvailableRoomBookingException)
        {
            return StatusCode(400);
        }

    }

    public record BookingBody(string employeeId, string hotelId, RoomType roomType, long checkIn, long checkOut);
}