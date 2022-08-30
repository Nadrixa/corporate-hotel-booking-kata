using HotelKata.Hotel.domain;
using HotelKata.Policies.application;
using HotelKata.Policies.domain;
using Moq;

namespace HotelKataTest.unit.policies;

public class AddCompanyPolicyUseCaseTest
{
    [Fact]
    public void ShouldAddReceivedCompanyPolicy()
    {
        var policiesRepository = new Mock<PoliciesRepository>();
        var addCompanyPolicyUseCase = new AddCompanyPolicyUseCase(policiesRepository.Object);
        
        addCompanyPolicyUseCase.execute("codurance", new [] { RoomType.Standard , RoomType.Premium });
        
        policiesRepository.Verify(repository => repository.setCompanyPolicy(It.IsAny<string>(), It.IsAny<RoomType[]>()), Times.Once);
    }
}