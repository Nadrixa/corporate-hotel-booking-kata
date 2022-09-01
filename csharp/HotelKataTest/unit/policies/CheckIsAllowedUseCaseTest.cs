using HotelKata.Employees.domain;
using HotelKata.Hotel.domain;
using HotelKata.Policies.application;
using HotelKata.Policies.domain;
using Moq;

namespace HotelKataTest.unit.policies;

public class CheckIsAllowedUseCaseTest
{

    [Fact]
    public void ShouldReturnIsAllowedIfThereAreNotPoliciesDefined()
    {
        var policiesRepository = new Mock<PoliciesRepository>();
        var employeesRepository = new Mock<EmployeesRepository>();
        var checkIsAllowedUseCase = new CheckIsAllowedUseCase(policiesRepository.Object, employeesRepository.Object);
        
        Assert.True(checkIsAllowedUseCase.execute("pepito", RoomType.Standard));
    }
    
    [Fact]
    public void ShouldReturnIsAllowedIfThereAreEmployeePolicyThatGrantsIt()
    {
        var policiesRepository = new Mock<PoliciesRepository>();
        var employeesRepository = new Mock<EmployeesRepository>();
        policiesRepository.Setup(repository => repository.retrieveEmployeePolicies(It.IsAny<string>())).Returns(new[]
        {
            RoomType.Standard
        });
        var checkIsAllowedUseCase = new CheckIsAllowedUseCase(policiesRepository.Object, employeesRepository.Object);
        
        Assert.True(checkIsAllowedUseCase.execute("pepito", RoomType.Standard));
    }
    
    [Fact]
    public void ShouldReturnIsAllowedIfThereAreCompanyPolicyThatGrantsIt()
    {
        var policiesRepository = new Mock<PoliciesRepository>();
        policiesRepository.Setup(repository => repository.retrieveCompanyPolicies("codurance")).Returns(new[]
        {
            RoomType.Standard
        });
        var employeesRepository = new Mock<EmployeesRepository>();
        employeesRepository.Setup(repository => repository.retrieveEmployeeInformation("pepito")).Returns("codurance");
        var checkIsAllowedUseCase = new CheckIsAllowedUseCase(policiesRepository.Object, employeesRepository.Object);
        
        Assert.True(checkIsAllowedUseCase.execute("pepito", RoomType.Standard));
    }
    
    [Fact]
    public void ShouldReturnIsNotAllowedIfEmployeePolicyExistAndDoesNotGrantIt()
    {
        var policiesRepository = new Mock<PoliciesRepository>();
        policiesRepository.Setup(repository => repository.retrieveEmployeePolicies(It.IsAny<string>())).Returns(new[]
        {
            RoomType.Standard
        });
        var employeesRepository = new Mock<EmployeesRepository>();
        var checkIsAllowedUseCase = new CheckIsAllowedUseCase(policiesRepository.Object, employeesRepository.Object);
        
        Assert.False(checkIsAllowedUseCase.execute("pepito", RoomType.Premium));
    }
    
    [Fact]
    public void ShouldReturnIsNotAllowedIfCompanyPolicyExistAndDoesNotGrantIt()
    {
        var policiesRepository = new Mock<PoliciesRepository>();
        policiesRepository.Setup(repository => repository.retrieveCompanyPolicies("codurance")).Returns(new[]
        {
            RoomType.Standard
        });
        var employeesRepository = new Mock<EmployeesRepository>();
        employeesRepository.Setup(repository => repository.retrieveEmployeeInformation("pepito")).Returns("codurance");
        var checkIsAllowedUseCase = new CheckIsAllowedUseCase(policiesRepository.Object, employeesRepository.Object);
        
        Assert.False(checkIsAllowedUseCase.execute("pepito", RoomType.Premium));
    }
    
    [Fact]
    public void ShouldReturnIsNotAllowedIfBothCompanyAndEmployeePoliciesExistButEmployeePolicyDoesNotGrantItAndCompanyGrantsIt()
    {
        var policiesRepository = new Mock<PoliciesRepository>();
        policiesRepository.Setup(repository => repository.retrieveCompanyPolicies("codurance")).Returns(new[]
        {
            RoomType.Premium
        });
        policiesRepository.Setup(repository => repository.retrieveEmployeePolicies("pepito")).Returns(new[]
        {
            RoomType.Standard
        });
        var employeesRepository = new Mock<EmployeesRepository>();
        employeesRepository.Setup(repository => repository.retrieveEmployeeInformation("pepito")).Returns("codurance");
        var checkIsAllowedUseCase = new CheckIsAllowedUseCase(policiesRepository.Object, employeesRepository.Object);
        
        Assert.False(checkIsAllowedUseCase.execute("pepito", RoomType.Premium));
    }

}