namespace HotelKata.Employees.domain;

public interface EmployeesRepository
{
    public void Add(string companyId, string employeeId);
    string? retrieveEmployeeInformation(string employeeId);
    void delete(string pepito);
}

public class ExistingEmployeeException : Exception 
{

}