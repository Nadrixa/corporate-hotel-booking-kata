namespace HotelKata.Employees.domain;

public interface EmployeesRepository
{
    public void Add(string companyId, string employeeId);
}

public class ExistingEmployeeException : Exception 
{

}