using HotelKata.Hotel.domain;

namespace HotelKata.Policies.domain;

public interface PoliciesRepository
{
    void setCompanyPolicy(string companyId, RoomType[] roomTypes);
    void setEmployeePolicy(string employeeId, RoomType[] roomTypes);
    RoomType[] retrieveEmployeePolicies(string employeeId);
    RoomType[] retrieveCompanyPolicies(string companyId);
    void deleteEmployeePoliciesOf(string employeeId);
}