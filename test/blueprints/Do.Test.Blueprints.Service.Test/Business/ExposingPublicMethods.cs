using System.Net;
using System.Net.Http.Json;

namespace Do.Test.Business;

public class ExposingPublicMethods : TestServiceNfr
{
    [Test]
    public async Task Void()
    {
        var response = await Client.PostAsync("/generated/Methods/Void", new StringContent(string.Empty));

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Test]
    public async Task VoidAsync()
    {
        var response = await Client.PostAsync("/generated/Methods/VoidAsync", new StringContent(string.Empty));

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Test]
    public async Task PrimitiveParameters()
    {
        var response = await Client.PostAsync("/generated/Methods/PrimitiveParameters", JsonContent.Create(
            new
            {
                @string = "string",
                @int = 42,
                dateTime = DateTime.Now
            }
        ));

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Test]
    public async Task PrimitiveListParameters()
    {
        var response = await Client.PostAsync("/generated/Methods/PrimitiveListParameters", JsonContent.Create(
            new
            {
                stringList = new[] { "a", "b" }
            }
        ));

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Test]
    public async Task EntityParameters()
    {
        var response = await Client.PostAsync("/generated/Methods/EntityParameters", JsonContent.Create(
            new
            {
                entityId = $"{Guid.NewGuid()}"
            }
        ));

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
