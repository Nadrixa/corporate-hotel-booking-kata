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
        var hotelRepository = new Mock<HotelRepository>();
        var addRoomToHotelUseCase = new AddRoomToHotelUseCase(hotelRepository.Object);
        
        addRoomToHotelUseCase.execute("1", new CorporateHotel.RoomData(RoomType.Standard, 5));
        
        hotelRepository.Verify(hotelRepository => hotelRepository.addRoomsToHotel("1", RoomType.Standard, 5), Times.Once);
    }
    
    [Fact]
    public void ShouldThrowWhenAddingRoomsToHotelThatDoesNotExist()
    {
        var hotelRepository = new Mock<HotelRepository>();
        hotelRepository
            .Setup(repository => repository.addRoomsToHotel(It.IsAny<string>(), It.IsAny<RoomType>(), It.IsAny<int>()))
            .Throws(new NotExistingHotelException());
        var addRoomToHotelUseCase = new AddRoomToHotelUseCase(hotelRepository.Object);

        Assert.Throws<NotExistingHotelException>(() => addRoomToHotelUseCase.execute("1", new CorporateHotel.RoomData(RoomType.Standard, 5)));
    }
}