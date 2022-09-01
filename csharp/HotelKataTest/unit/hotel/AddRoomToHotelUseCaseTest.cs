using HotelKata.Hotel.api;
using HotelKata.Hotel.application;
using HotelKata.Hotel.domain;
using Moq;

namespace HotelKataTest.unit.hotel;

public class AddRoomToHotelUseCaseTest
{
    [Fact]
    public void ShouldAddOrUpdateRoomsOfExistingHotel()
    {
        const string hotelId = "1";
        const RoomType roomType = RoomType.Standard;
        const int numberOfRooms = 5;
        var hotelRepository = new Mock<HotelRepository>();
        hotelRepository.Setup(repository => repository.findHotelWith(hotelId))
            .Returns(new FindHotelUseCase.HotelDetails("Palace", new Dictionary<RoomType, int>()));
        var addRoomToHotelUseCase = new AddRoomToHotelUseCase(hotelRepository.Object);
        
        addRoomToHotelUseCase.execute(hotelId, new CorporateHotel.RoomData(roomType, numberOfRooms));
        
        hotelRepository.Verify(hotelRepository => hotelRepository.addRoomsToHotel(hotelId, roomType, numberOfRooms), Times.Once);
    }
    
    [Fact]
    public void ShouldThrowWhenAddingRoomsToHotelThatDoesNotExist()
    {
        const string hotelId = "1";
        var hotelRepository = new Mock<HotelRepository>();
        hotelRepository.Setup(repository => repository.findHotelWith(hotelId))
            .Returns((FindHotelUseCase.HotelDetails?) null);
        var addRoomToHotelUseCase = new AddRoomToHotelUseCase(hotelRepository.Object);

        Assert.Throws<NotExistingHotelException>(() => addRoomToHotelUseCase.execute(hotelId, new CorporateHotel.RoomData(RoomType.Standard, 5)));
    }
}