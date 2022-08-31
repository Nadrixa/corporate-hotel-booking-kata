using HotelKata.Employees.domain;
using HotelKata.Hotel.domain;
using HotelKata.Policies.domain;

namespace HotelKata.Policies.application;

public class CheckIsAllowedUseCase
{
    private readonly PoliciesRepository _policiesRepository;
    private readonly EmployeesRepository _employeesRepository;

    public CheckIsAllowedUseCase(PoliciesRepository policiesRepository, EmployeesRepository employeesRepository)
    {
        _policiesRepository = policiesRepository;
        _employeesRepository = employeesRepository;
    }

    public virtual bool execute(string employeeId, RoomType roomType)
    {
        var employeePolicies = _policiesRepository.retrieveEmployeePolicies(employeeId);
        if (employeePolicies.Length != 0)
        {
            return employeePolicies.Contains(roomType);
        }
        
        var companyPolicies = _policiesRepository.retrieveCompanyPolicies(_employeesRepository.retrieveEmployeeInformation(employeeId));
        if (companyPolicies.Length != 0)
        {
            return companyPolicies.Contains(roomType);
        }

        return true;
    }
}