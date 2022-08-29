using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace HotelKataTest;

public class HelloWorldTest
{
    private readonly HttpClient _client;
    public HelloWorldTest()
    {
        var testServer = new WebApplicationFactory<Program>();
        _client = testServer.CreateClient();
    }

    [Fact]
    public async void HelloWorldAPIWorks()
    {

        // when
        var response = await _client.GetAsync("hello-world");
        
        // then
        await assertThatIsTheExpected(response);

        async Task assertThatIsTheExpected(HttpResponseMessage httpResponseMessage)
        {
            Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
            Assert.Equal("HelloWorld", await httpResponseMessage.Content.ReadAsStringAsync());
        }
    }
}