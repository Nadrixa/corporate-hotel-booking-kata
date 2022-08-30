using HotelKata.Hotel.domain;
using HotelKata.Policies.domain;

namespace HotelKata.Policies.infrastructure;

public class InMemoryPoliciesRepository : PoliciesRepository
{
    public Dictionary<string, RoomType[]> PoliciesByCompany { get; }
    
    public InMemoryPoliciesRepository()
    {
        PoliciesByCompany = new Dictionary<string, RoomType[]>();
    }

    public void setCompanyPolicy(string companyId, RoomType[] roomTypes)
    {
        PoliciesByCompany[companyId] = roomTypes;
    }
}