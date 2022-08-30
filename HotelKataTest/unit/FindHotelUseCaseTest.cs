using HotelKata.Hotel.application;
using HotelKata.Hotel.domain;
using Moq;

namespace HotelKataTest.unit;

public class FindHotelUseCaseTest
{

    [Fact]
    public void ShouldRetrieveDesiredHotelInformation()
    {
        var expectedHotelInformation = new FindHotelUseCase.HotelDetails("Palace", new Dictionary<RoomType, int>() { { RoomType.Standard, 4 } });
        var hotelRepository = new Mock<HotelRepository>();
        hotelRepository.Setup(hotelRepository => hotelRepository.find(It.IsAny<string>())).Returns(expectedHotelInformation);
        var findHotelUseCase = new FindHotelUseCase(hotelRepository.Object);

        var hotelInformation = findHotelUseCase.execute("1");
        
        Assert.Equal(expectedHotelInformation,hotelInformation);
    }
    
    [Fact]
    public void ShouldThrowIfDesiredHotelDoesNotExist()
    {
        var hotelRepository = new Mock<HotelRepository>();
        hotelRepository.Setup(hotelRepository => hotelRepository.find(It.IsAny<string>())).Throws(new NotExistingHotelException());
        var findHotelUseCase = new FindHotelUseCase(hotelRepository.Object);

        Assert.Throws<NotExistingHotelException>(() => findHotelUseCase.execute("1"));
    }
}