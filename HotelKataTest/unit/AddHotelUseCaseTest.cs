using HotelKata.Hotel.api;
using HotelKata.Hotel.application;
using HotelKata.Hotel.domain;
using Moq;

namespace HotelKataTest.unit;

public class AddHotelUseCaseTest
{

    [Fact]
    public void ShouldAddHotelToRepository()
    {
        var hotelsRepository = new Mock<HotelRepository>();
        var addHotelUseCase = new AddHotelUseCase(hotelsRepository.Object);
        
        addHotelUseCase.execute(new CorporateHotel.HotelCreationBody("1", "Palace"));
        
        hotelsRepository.Verify(hotelsRepository => hotelsRepository.add("1", "Palace"), Times.Once);
    }
    
    [Fact]
    public void ShouldThrowIfHotelPreviouslyExistsWhenAdding()
    {
        var hotelsRepository = new Mock<HotelRepository>();
        hotelsRepository.Setup(hotelsRepository => hotelsRepository.add("1", "Palace"))
            .Throws(new ExistingHotelException());
        var addHotelUseCase = new AddHotelUseCase(hotelsRepository.Object);
        
        Assert.Throws<ExistingHotelException>(() => addHotelUseCase.execute(new CorporateHotel.HotelCreationBody("1", "Palace")));
    }
}