using Do.Authentication;

namespace Do.Test.Authentication;

public class ValidatingFormPost : TestServiceSpec
{
    [Test]
    public void Concats_given_data_with_token_and_compares_it_to_provided_hash()
    {
        var middleware = GiveMe.AFixedTokenMiddleware(tokenNames: ["Test"]);
        MockMe.ASetting(key: "Authentication:FixedToken:Test", value: "token");
        var request = GiveMe.AnHttpRequest(
            metadata: [new UseAttribute<Do.Authentication.FixedToken.Middleware>()],
            form: GiveMe.ADictionary(
                ("hash", "Fp4hWyHBe50ZkackNZdDMIO7My70naTsQUZDoS0v9aw="), // 12token -sha256-> 169E215B21C17B9D1991A7243597433083BB332EF49DA4EC414643A12D2FF5AC
                ("value-1", "1"),
                ("value-2", "2")
            )
        );

        var action = () => middleware.Invoke(request.HttpContext);

        action.ShouldNotThrow();
    }

    [Test]
    public void Throws_unauthorized_access_when_hash_and_parameters_does_not_match()
    {
        var middleware = GiveMe.AFixedTokenMiddleware(tokenNames: ["Test"]);
        MockMe.ASetting(key: "Authentication:FixedToken:Test", value: "other-token");
        var request = GiveMe.AnHttpRequest(
            metadata: [new UseAttribute<Do.Authentication.FixedToken.Middleware>()],
            form: GiveMe.ADictionary(
              ("hash", "Fp4hWyHBe50ZkackNZdDMIO7My70naTsQUZDoS0v9aw="), // 12token -sha256-> 169E215B21C17B9D1991A7243597433083BB332EF49DA4EC414643A12D2FF5AC
              ("value-1", "1"),
              ("value-2", "2")
            )
        );

        var action = () => middleware.Invoke(request.HttpContext);

        action.ShouldThrow<UnauthorizedAccessException>();
    }

    [Test]
    public void Throws_unauthorized_access_when_hash_is_not_provided()
    {
        var middleware = GiveMe.AFixedTokenMiddleware(tokenNames: ["Test"]);
        MockMe.ASetting(key: "Authentication:FixedToken:Test", value: GiveMe.AString());
        var request = GiveMe.AnHttpRequest(
            metadata: [new UseAttribute<Do.Authentication.FixedToken.Middleware>()],
            form: GiveMe.ADictionary()
        );

        var action = () => middleware.Invoke(request.HttpContext);

        action.ShouldThrow<UnauthorizedAccessException>();
    }
}
