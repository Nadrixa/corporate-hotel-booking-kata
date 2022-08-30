using HotelKata.Hotel.domain;
using HotelKata.Policies.application;
using HotelKata.Policies.domain;
using Moq;

namespace HotelKataTest.unit.policies;

public class AddEmployeePolicyUseCaseTest
{

    [Fact]
    public void ShouldAddGivenEmployeePolicy()
    {
        var policiesRepository = new Mock<PoliciesRepository>();
        var addEmployeePolicyUseCase = new AddEmployeePolicyUseCase(policiesRepository.Object);
        
        addEmployeePolicyUseCase.execute("pepito", new [] { RoomType.Standard , RoomType.Junior });
        
        policiesRepository.Verify(repository => repository.setEmployeePolicy(It.IsAny<string>(), It.IsAny<RoomType[]>()), Times.Once);
    }
}