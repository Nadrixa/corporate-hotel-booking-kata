using HotelKata.Hotel.domain;
using HotelKata.Policies.domain;

namespace HotelKata.Policies.application;

public class AddCompanyPolicyUseCase
{
    private readonly PoliciesRepository _policiesRepository;

    public AddCompanyPolicyUseCase(PoliciesRepository policiesRepository)
    {
        _policiesRepository = policiesRepository;
    }

    public void execute(string companyId, RoomType[] roomTypes)
    {
        _policiesRepository.setCompanyPolicy(companyId, roomTypes);
    }
}