using System.Net;

namespace HotelKataTest.E2E;

public static class NetworkAssertions
{
    public static void ThenRepliedWithExpectedStatus(HttpStatusCode expectedStatusCode, HttpStatusCode receivedStatusCode)
    {
        Assert.Equal(expectedStatusCode, receivedStatusCode);
    }
}