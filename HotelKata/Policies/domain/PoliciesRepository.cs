using HotelKata.Hotel.domain;

namespace HotelKata.Policies.domain;

public interface PoliciesRepository
{
    void setCompanyPolicy(string companyId, RoomType[] roomTypes);
}