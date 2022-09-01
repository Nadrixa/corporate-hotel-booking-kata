using HotelKata.Employees.domain;

namespace HotelKata.Employees.infrastructure;

public class InMemoryEmployeesRepository : EmployeesRepository
{
    public Dictionary<string,string> Employees { get; }

    public InMemoryEmployeesRepository()
    {
        Employees = new Dictionary<string, string>();
    }

    public void Add(string companyId, string employeeId)
    {
        Employees.Add(employeeId, companyId);
    }

    public string? retrieveEmployeeInformation(string employeeId)
    {
        return Employees.ContainsKey(employeeId) ? Employees[employeeId] : null;
    }

    public void delete(string employeeId)
    {
        Employees.Remove(employeeId);
    }
}