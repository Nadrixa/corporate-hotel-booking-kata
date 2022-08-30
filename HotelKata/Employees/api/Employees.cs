using HotelKata.Employees.application;
using HotelKata.Employees.domain;
using HotelKata.Employees.infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace HotelKata.Employees.api;

[ApiController]
[Route("employees")]
public class Employees : ControllerBase
{
    private readonly AddEmployeeUseCase _addEmployeeUseCase;

    public Employees(InMemoryEmployeesRepository inMemoryEmployeesRepository)
    {
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

    public record AddEmployeeBody(string employeeId, string companyId);
}