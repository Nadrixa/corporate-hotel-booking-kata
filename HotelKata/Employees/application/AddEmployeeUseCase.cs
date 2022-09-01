using HotelKata.Employees.domain;

namespace HotelKata.Employees.application;

public class AddEmployeeUseCase
{
    private readonly EmployeesRepository _employeesRepository;

    public AddEmployeeUseCase(EmployeesRepository employeesRepository)
    {
        _employeesRepository = employeesRepository;
    }

    public void execute(string companyId, string employeeId)
    {
        if (_employeesRepository.retrieveEmployeeInformation(employeeId) is not null) throw new ExistingEmployeeException();
        
        _employeesRepository.Add(companyId, employeeId);
    }
}