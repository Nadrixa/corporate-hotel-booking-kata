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

    public Policies(InMemoryPoliciesRepository inMemoryPoliciesRepository)
    {
        _addCompanyPolicyUseCase = new AddCompanyPolicyUseCase(inMemoryPoliciesRepository);
    }

    [HttpPost("company")]
    public IActionResult AddCompanyPolicy(CompanyPolicyBody companyPolicyBody)
    {
        _addCompanyPolicyUseCase.execute(companyPolicyBody.companyId, companyPolicyBody.roomTypes);
        return StatusCode(201);
    }
}

public record CompanyPolicyBody(string companyId, RoomType[] roomTypes);