using HotelKata.Employees.application;
using HotelKata.Employees.domain;
using Moq;

namespace HotelKataTest.unit.employees;

public class AddEmployeeUseCaseTest
{

    [Fact]
    public void ShouldAddNewEmployee()
    {
        var employeesRepository = new Mock<EmployeesRepository>();
        var addEmployeeUseCase = new AddEmployeeUseCase(employeesRepository.Object);
        
        addEmployeeUseCase.execute("codurance", "pepito");
        
        employeesRepository.Verify(
            repository => repository.Add("codurance", "pepito"), Times.Once
            );
    }
    
    [Fact]
    public void ShouldThrowIfEmployeeExistWhenTryingToAddHerAgain()
    {
        var employeeId = "pepito";
        var employeesRepository = new Mock<EmployeesRepository>();
        employeesRepository.Setup(repository => repository.retrieveEmployeeInformation(employeeId))
            .Returns("codurance");
        var addEmployeeUseCase = new AddEmployeeUseCase(employeesRepository.Object);
        
        Assert.Throws<ExistingEmployeeException>(() => addEmployeeUseCase.execute("coduranco", employeeId));
    }
}