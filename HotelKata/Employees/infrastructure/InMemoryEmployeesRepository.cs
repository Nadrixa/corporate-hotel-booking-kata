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
        if (Employees.ContainsKey(employeeId))
        {
            throw new ExistingEmployeeException();
        }
        Employees.Add(employeeId, companyId);
    }
}