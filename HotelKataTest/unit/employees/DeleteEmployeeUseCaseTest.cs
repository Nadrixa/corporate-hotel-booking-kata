using HotelKata.Bookings.domain;
using HotelKata.Employees.application;
using HotelKata.Employees.domain;
using HotelKata.Policies.domain;
using Moq;

namespace HotelKataTest.unit.employees;

public class DeleteEmployeeUseCaseTest
{
    [Fact]
    public void ShouldDeleteGivenEmployeeAndItsBookingsAndPolicies()
    {
        var employeesRepository = new Mock<EmployeesRepository>();
        var policiesRepository = new Mock<PoliciesRepository>();
        var bookingsRepository = new Mock<BookingRepository>();
        var deleteEmployeeUseCase = new DeleteEmployeeUseCase(employeesRepository.Object, policiesRepository.Object, bookingsRepository.Object);
        
        deleteEmployeeUseCase.execute("pepito");

        employeesRepository.Verify(repository => repository.delete("pepito"), Times.Once);
        policiesRepository.Verify(repository => repository.deleteEmployeePoliciesOf("pepito"), Times.Once);
        bookingsRepository.Verify(repository => repository.deleteBookingsOf("pepito"), Times.Once);
    }
    
    [Fact]
    public void ShouldThrowNotExisting()
    {
        var employeesRepository = new Mock<EmployeesRepository>();
        employeesRepository.Setup(repository => repository.delete("pepito")).Throws<NotExistingEmployeeException>();
        var deleteEmployeeUseCase = new DeleteEmployeeUseCase(employeesRepository.Object, new Mock<PoliciesRepository>().Object, new Mock<BookingRepository>().Object);
        
        Assert.Throws<NotExistingEmployeeException>(() => deleteEmployeeUseCase.execute("pepito"));
    }
}