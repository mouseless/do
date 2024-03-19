namespace Do.Test.ConfigurationOverrider.Codes;

public static class Singleton
{
    public static readonly string Code = """
        using Do.Authentication;
        using Do.Orm;
        using Microsoft.AspNetCore.Mvc;

        namespace Do.Test;

        [ApiController]
        public class SingletonController
        {
            [HttpGet]
            [Produces("application/json")]
            [Route("singleton/time")]
            [Use<Authentication.FixedToken.Middleware>]
            public DateTime GetNow([FromServices] Singleton target)
            {
                var result = target.GetNow();

                return result;
            }

            [HttpPost]
            [Produces("application/json")]
            [Route("singleton/test-client")]
            public async Task<object> TestClient([FromServices] Singleton target)
            {
                return await target.TestClient();
            }

            [HttpPost]
            [Produces("application/json")]
            [Route("singleton/test-form-post-authentication")]
            [Use<Authentication.FixedToken.Middleware>]
            public object TestFormPostAuthentication([FromServices] Singleton target,
                [FromForm] string value,
                [FromForm] string hash
            )
            {
                return target.TestFormPostAuthentication(value);
            }

            public record TestTransactionRollbackRequest(string String);

            [HttpPost]
            [Produces("application/json")]
            [Route("singleton/test-transaction-rollback")]
            public void TestTransactionRollback([FromServices] Singleton target, [FromBody] TestTransactionRollbackRequest request)
            {
                target.TestTransactionRollback(@string: request.String);
            }

            [HttpPost]
            [Produces("application/json")]
            [Route("singleton/test-transaction-action")]
            public async Task TestTransactionAction([FromServices] Singleton target)
            {
                await target.TestTransactionAction();
            }

            [HttpPost]
            [Produces("application/json")]
            [Route("singleton/test-transaction-func")]
            public async Task TestTransactionFunc([FromServices] Singleton target)
            {
                await target.TestTransactionFunc();
            }

            [HttpPost]
            [Produces("application/json")]
            [Route("singleton/test-exception")]
            public void TestException([FromServices] Singleton target, bool handled)
            {
                target.TestException(handled);
            }

            public record TestTransactionNullableRequest(Guid? EntityId = default);

            [HttpPost]
            [Produces("application/json")]
            [Route("singleton/test-transaction-nullable")]
            public async Task TestTransactionNullable([FromServices] Singleton target, [FromServices] IQueryContext<Entity> entityQuery, [FromBody] TestTransactionNullableRequest request)
            {
                await target.TestTransactionNullable(
                    entity: request.EntityId.HasValue
                        ? entityQuery.SingleById(request.EntityId.Value)
                        : null
                );
            }

            [HttpPut]
            [Produces("application/json")]
            [Route("singleton/test-async-object")]
            public async Task<object> TestAsyncObject([FromServices] Singleton target, [FromBody] object request)
            {
                var result = await target.TestAsyncObject(request);

                return result;
            }

            [HttpPut]
            [Produces("application/json")]
            [Route("singleton/test-object")]
            public object TestObject([FromServices] Singleton target, [FromBody] object request)
            {
                var result = target.TestObject(request);

                return result;
            }
        }
    """;
}
