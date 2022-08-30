namespace HotelKata.Employees.domain;

public interface EmployeesRepository
{
    public void Add(string companyId, string employeeId);
    string retrieveEmployeeInformation(string employeeId);
}

public class ExistingEmployeeException : Exception 
{

}