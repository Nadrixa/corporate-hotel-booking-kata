using HotelKata.Hotel.domain;
using HotelKata.Policies.application;
using HotelKata.Policies.domain;
using HotelKata.Policies.infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace HotelKata.Policies.api;

[ApiController]
[Route("policies")]
public class Policies : ControllerBase
{
    private readonly AddCompanyPolicyUseCase _addCompanyPolicyUseCase;
    private readonly AddEmployeePolicyUseCase _addEmployeePolicyUseCase;

    public Policies(InMemoryPoliciesRepository inMemoryPoliciesRepository)
    {
        _addEmployeePolicyUseCase = new AddEmployeePolicyUseCase(inMemoryPoliciesRepository);
        _addCompanyPolicyUseCase = new AddCompanyPolicyUseCase(inMemoryPoliciesRepository);
    }

    [HttpPost("company")]
    public IActionResult AddCompanyPolicy(CompanyPolicyBody companyPolicyBody)
    {
        _addCompanyPolicyUseCase.execute(companyPolicyBody.companyId, companyPolicyBody.roomTypes);
        return StatusCode(201);
    }
    
    [HttpPost("employee")]
    public IActionResult AddCompanyPolicy(EmployeePolicyBody companyPolicyBody)
    {
        _addEmployeePolicyUseCase.execute(companyPolicyBody.employeeId, companyPolicyBody.roomTypes);
        return StatusCode(201);
    }
}

public record EmployeePolicyBody(string employeeId, RoomType[] roomTypes);
public record CompanyPolicyBody(string companyId, RoomType[] roomTypes);