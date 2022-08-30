using HotelKata.Hotel.domain;
using HotelKata.Policies.domain;

namespace HotelKata.Policies.infrastructure;

public class InMemoryPoliciesRepository : PoliciesRepository
{
    public Dictionary<string, RoomType[]> PoliciesByCompany { get; }
    public Dictionary<string, RoomType[]> PoliciesByEmployee { get; }

    public InMemoryPoliciesRepository()
    {
        PoliciesByEmployee = new Dictionary<string, RoomType[]>();
        PoliciesByCompany = new Dictionary<string, RoomType[]>();
    }

    public void setCompanyPolicy(string companyId, RoomType[] roomTypes)
    {
        PoliciesByCompany[companyId] = roomTypes;
    }

    public void setEmployeePolicy(string employeeId, RoomType[] roomTypes)
    {
        PoliciesByEmployee[employeeId] = roomTypes;
    }
}