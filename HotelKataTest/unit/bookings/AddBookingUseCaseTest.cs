using HotelKata.Bookings.application;
using HotelKata.Bookings.domain;
using HotelKata.Employees.domain;
using HotelKata.Hotel.application;
using HotelKata.Hotel.domain;
using HotelKata.Policies.application;
using HotelKata.Policies.domain;
using Moq;

namespace HotelKataTest.unit.bookings;

public class AddBookingUseCaseTest
{

    [Fact]
    public void ShouldThrowWhenCheckOutIsNotOneDayAfterCheckIn()
    {
        var addBookingUseCase = new AddBookingUseCase(
            new Mock<HotelRepository>().Object,
            new Mock<CheckIsAllowedUseCase>(new Mock<PoliciesRepository>().Object, new Mock<EmployeesRepository>().Object).Object,
            new Mock<BookingRepository>().Object
            );
        
        Assert.Throws<InvalidBookingException>(() => addBookingUseCase.execute(new Booking(
            "pepito",
            "palace",
            RoomType.Standard,
            1661942150000,
            1661942150000
            )));
    }
    
    [Fact]
    public void ShouldThrowWhenHotelDoesNotExist()
    {
        var addBookingUseCase = GivenAddBookingUseCaseInitializedWithoutDesiredHotel();

        Assert.Throws<InvalidBookingException>(() => addBookingUseCase.execute(new Booking(
            "pepito",
            "notExistingHotel",
            RoomType.Standard,
            1661942150000,
            1662028550000
        )));

        AddBookingUseCase GivenAddBookingUseCaseInitializedWithoutDesiredHotel()
        {
            var hotelRepository = new Mock<HotelRepository>();
            hotelRepository.Setup(repository => repository.findHotelWith("notExistingHotel"))
                .Returns((FindHotelUseCase.HotelDetails?) null);

            return new AddBookingUseCase(
                hotelRepository.Object,
                new Mock<CheckIsAllowedUseCase>(new Mock<PoliciesRepository>().Object, new Mock<EmployeesRepository>().Object)
                    .Object,
                new Mock<BookingRepository>().Object);
        }
    }
    
    [Fact]
    public void ShouldThrowWhenHotelDoesNotHaveGivenRoomType()
    {
        var addBookingUseCase = GivenAddBookingUseCaseInitializedWithHotelWithoutDesiredRoomType();

        Assert.Throws<InvalidBookingException>(() => addBookingUseCase.execute(new Booking(
            "pepito",
            "palace",
            RoomType.Standard,
            1661942150000,
            1662028550000
        )));

        AddBookingUseCase GivenAddBookingUseCaseInitializedWithHotelWithoutDesiredRoomType()
        {
            var hotelRepository = new Mock<HotelRepository>();
            hotelRepository.Setup(repository => repository.findHotelWith("palace"))
                .Returns(new FindHotelUseCase.HotelDetails("Palace", new Dictionary<RoomType, int>()));
            
            return new AddBookingUseCase(
                hotelRepository.Object,
                new Mock<CheckIsAllowedUseCase>(new Mock<PoliciesRepository>().Object, new Mock<EmployeesRepository>().Object).Object,
                new Mock<BookingRepository>().Object);
        }
    }
    
    [Fact]
    public void ShouldThrowWhenEmployeeIsNotAllowedToBookThisTypeOfRoom()
    {
        var addBookingUseCase = GivenAddBookingUseCaseInitializedWithoutGrantedBooking();
        
        Assert.Throws<InvalidBookingException>(() => addBookingUseCase.execute(new Booking(
            "pepito",
            "palace",
            RoomType.Standard,
            1661942150000,
            1662028550000
        )));
        
        AddBookingUseCase GivenAddBookingUseCaseInitializedWithoutGrantedBooking()
        {
            var hotelRepository = new Mock<HotelRepository>();
            hotelRepository.Setup(repository => repository.findHotelWith("palace"))
                .Returns(new FindHotelUseCase.HotelDetails("Palace",
                    new Dictionary<RoomType, int>() { { RoomType.Standard, 5 } }));
            var checkIsAllowedUseCase =
                new Mock<CheckIsAllowedUseCase>(new Mock<PoliciesRepository>().Object, new Mock<EmployeesRepository>().Object);
            checkIsAllowedUseCase.Setup(useCase => useCase.execute("pepito", RoomType.Standard)).Returns(false);
            
            return new AddBookingUseCase(hotelRepository.Object, checkIsAllowedUseCase.Object,
                new Mock<BookingRepository>().Object);
        }
    }

    [Fact]
    public void ShouldThrowWhenThereIsNoAvailableRoomWithinGivenBookingPeriod()
    {
        var addBookingUseCase = GivenAddBookingUseCaseInitializedWithoutAvailableRooms();

        Assert.Throws<NotAvailableRoomBookingException>(() => addBookingUseCase.execute(new Booking(
            "pepito",
            "palace",
            RoomType.Standard,
            1661942150000,
            1662028550000
        )));
        
        AddBookingUseCase GivenAddBookingUseCaseInitializedWithoutAvailableRooms()
        {
            var hotelRepository = new Mock<HotelRepository>();
            hotelRepository.Setup(repository => repository.findHotelWith("palace"))
                .Returns(new FindHotelUseCase.HotelDetails("Palace", new Dictionary<RoomType, int>() { { RoomType.Standard , 1 }}));
            var checkIsAllowedUseCase = new Mock<CheckIsAllowedUseCase>(new Mock<PoliciesRepository>().Object, new Mock<EmployeesRepository>().Object);
            checkIsAllowedUseCase.Setup(useCase => useCase.execute("pepito", RoomType.Standard)).Returns(true);
            var bookingRepository = new Mock<BookingRepository>();
            bookingRepository.Setup(repository =>
                repository.retrieveBookingsFor("palace", RoomType.Standard, 1661942150000, 1662028550000)).Returns(new List<Booking>()
            {
                new(
                    "paco",
                    "palace",
                    RoomType.Standard,
                    16619421501000,
                    1662028551000
                )
            });
            
            return new AddBookingUseCase(hotelRepository.Object, checkIsAllowedUseCase.Object, bookingRepository.Object);
        }
    }
    
    [Fact]
    public void ShouldAddBookingAndReturnBookingConfirmationWhenBookingIsAvailable()
    {
        var (addBookingUseCase, bookingRepository, expectedBooking) = GivenAddBookingUseCaseBookingRepositoryAndExpectedBooking();

        var bookingConfirmation = addBookingUseCase.execute(expectedBooking);

        Assert.Equal(expectedBooking, bookingConfirmation);
        bookingRepository.Verify(repository => repository.add(expectedBooking), Times.Once);

        (AddBookingUseCase, Mock<BookingRepository>, Booking) GivenAddBookingUseCaseBookingRepositoryAndExpectedBooking()
        {
            var hotelRepository = new Mock<HotelRepository>();
            hotelRepository.Setup(repository => repository.findHotelWith("palace"))
                .Returns(new FindHotelUseCase.HotelDetails("Palace",
                    new Dictionary<RoomType, int>() { { RoomType.Standard, 5 } }));
            var checkIsAllowedUseCase =
                new Mock<CheckIsAllowedUseCase>(new Mock<PoliciesRepository>().Object, new Mock<EmployeesRepository>().Object);
            checkIsAllowedUseCase.Setup(useCase => useCase.execute("pepito", RoomType.Standard)).Returns(true);
            var bookingRepository = new Mock<BookingRepository>();
            bookingRepository.Setup(repository =>
                    repository.retrieveBookingsFor("palace", RoomType.Standard, 1661942150000, 1662028550000))
                .Returns(new List<Booking>());
            
            return (
                new AddBookingUseCase(hotelRepository.Object, checkIsAllowedUseCase.Object, bookingRepository.Object),
                bookingRepository,
                new Booking("pepito",
                    "palace",
                    RoomType.Standard,
                    1661942150000,
                    1662028550000)
                );
        }
    }
}