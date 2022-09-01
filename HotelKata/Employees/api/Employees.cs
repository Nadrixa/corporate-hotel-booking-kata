using HotelKata.Bookings.infrastructure;
using HotelKata.Employees.application;
using HotelKata.Employees.domain;
using HotelKata.Employees.infrastructure;
using HotelKata.Policies.infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace HotelKata.Employees.api;

[ApiController]
[Route("employees")]
public class Employees : ControllerBase
{
    private readonly AddEmployeeUseCase _addEmployeeUseCase;
    private readonly DeleteEmployeeUseCase _deleteEmployeeUseCase;

    public Employees(InMemoryEmployeesRepository inMemoryEmployeesRepository,
        InMemoryPoliciesRepository inMemoryPoliciesRepository, InMemoryBookingsRepository inMemoryBookingsRepository)
    {
        _deleteEmployeeUseCase = new DeleteEmployeeUseCase(inMemoryEmployeesRepository, inMemoryPoliciesRepository, inMemoryBookingsRepository);
        _addEmployeeUseCase = new AddEmployeeUseCase(inMemoryEmployeesRepository);
    }

    [HttpPost]
    public IActionResult AddEmployee(AddEmployeeBody addEmployeeBody)
    {
        try
        {
            _addEmployeeUseCase.execute(addEmployeeBody.companyId, addEmployeeBody.employeeId);
            return StatusCode(201);
        }
        catch (ExistingEmployeeException)
        {
            return StatusCode(400);
        }
    }
    
    [HttpDelete("{employeeId}")]
    public IActionResult DeleteEmployee(string employeeId)
    {
        try
        {
            _deleteEmployeeUseCase.execute(employeeId);
            return StatusCode(200);
        }
        catch (NotExistingEmployeeException)
        {
            return StatusCode(404);
        }

    }

    public record AddEmployeeBody(string employeeId, string companyId);
}