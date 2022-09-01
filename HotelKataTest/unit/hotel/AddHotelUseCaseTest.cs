using HotelKata.Hotel.application;
using HotelKata.Hotel.domain;
using Moq;

namespace HotelKataTest.unit.hotel;

public class AddHotelUseCaseTest
{

    [Fact]
    public void ShouldAddHotelToRepository()
    {
        const string hotelId = "1";
        const string hotelName = "Palace";
        var hotelsRepository = new Mock<HotelRepository>();
        hotelsRepository.Setup(repository => repository.findHotelWith(hotelId))
            .Returns((FindHotelUseCase.HotelDetails?) null);
        var addHotelUseCase = new AddHotelUseCase(hotelsRepository.Object);
        
        addHotelUseCase.execute(hotelId, hotelName);
        
        hotelsRepository.Verify(hotelsRepository => hotelsRepository.addHotelWith(hotelId, hotelName), Times.Once);
    }
    
    [Fact]
    public void ShouldThrowIfHotelPreviouslyExistsWhenAdding()
    {
        const string hotelId = "1";
        const string hotelName = "Palace";
        var hotelsRepository = new Mock<HotelRepository>();
        hotelsRepository.Setup(repository => repository.findHotelWith(hotelId))
            .Returns(new FindHotelUseCase.HotelDetails(hotelName, new Dictionary<RoomType, int>()));

        var addHotelUseCase = new AddHotelUseCase(hotelsRepository.Object);
        
        Assert.Throws<ExistingHotelException>(() => addHotelUseCase.execute(hotelId, hotelName));
    }
}