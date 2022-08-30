using HotelKata.Hotel.domain;
using HotelKata.Policies.domain;

namespace HotelKata.Policies.application;

public class AddEmployeePolicyUseCase
{
    private readonly PoliciesRepository _policiesRepository;

    public AddEmployeePolicyUseCase(PoliciesRepository policiesRepository)
    {
        _policiesRepository = policiesRepository;
    }

    public void execute(string employeeId, RoomType[] roomTypes)
    {
        _policiesRepository.setEmployeePolicy(employeeId, roomTypes);
    }
}