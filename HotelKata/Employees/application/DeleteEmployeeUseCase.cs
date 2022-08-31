using HotelKata.Bookings.domain;
using HotelKata.Employees.domain;
using HotelKata.Policies.domain;

namespace HotelKata.Employees.application;

public class DeleteEmployeeUseCase
{
    private readonly EmployeesRepository _employeesRepository;
    private readonly PoliciesRepository _policiesRepository;
    private readonly BookingRepository _bookingsRepository;

    public DeleteEmployeeUseCase(EmployeesRepository employeesRepository, PoliciesRepository policiesRepository, BookingRepository bookingsRepository)
    {
        _employeesRepository = employeesRepository;
        _policiesRepository = policiesRepository;
        _bookingsRepository = bookingsRepository;
    }
    public void execute(string employeeId)
    {
        _employeesRepository.delete(employeeId);
        _policiesRepository.deleteEmployeePoliciesOf(employeeId);
        _bookingsRepository.deleteBookingsOf(employeeId);
    }
}