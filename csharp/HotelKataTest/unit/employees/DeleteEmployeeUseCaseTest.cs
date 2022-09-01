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
        const string employeeId = "pepito";
        var employeesRepository = new Mock<EmployeesRepository>();
        employeesRepository.Setup(repository => repository.retrieveEmployeeInformation(employeeId)).Returns("codurance");
        var policiesRepository = new Mock<PoliciesRepository>();
        var bookingsRepository = new Mock<BookingRepository>();
        var deleteEmployeeUseCase = new DeleteEmployeeUseCase(employeesRepository.Object, policiesRepository.Object, bookingsRepository.Object);
        
        deleteEmployeeUseCase.execute(employeeId);

        employeesRepository.Verify(repository => repository.delete(employeeId), Times.Once);
        policiesRepository.Verify(repository => repository.deleteEmployeePoliciesOf(employeeId), Times.Once);
        bookingsRepository.Verify(repository => repository.deleteBookingsOf(employeeId), Times.Once);
    }
    
    [Fact]
    public void ShouldThrowNotExisting()
    {
        const string employeeId = "pepito";
        var employeesRepository = new Mock<EmployeesRepository>();
        employeesRepository.Setup(repository => repository.retrieveEmployeeInformation(employeeId)).Returns((string?) null);
        var deleteEmployeeUseCase = new DeleteEmployeeUseCase(employeesRepository.Object, new Mock<PoliciesRepository>().Object, new Mock<BookingRepository>().Object);
        
        Assert.Throws<NotExistingEmployeeException>(() => deleteEmployeeUseCase.execute(employeeId));
    }
}