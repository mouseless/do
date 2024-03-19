namespace Do.Test.ConfigurationOverrider.Codes;

public static class Entities
{
    public static readonly string Code = """
        using Do.Orm;
        using Microsoft.AspNetCore.Mvc;

        namespace Do.Test;

        [ApiController]
        public class EntitiesController
        {
            public record ByRequest(
                Guid? Guid = default,
                string String = default,
                string StringData = default,
                int? Int32 = default,
                Uri Uri = default,
                Status? Status = default,
                DateTime? DateTime = default,
                int? Take = null,
                int? Skip = null
            );

            [HttpGet]
            [Route("entities")]
            public List<Entity> By([FromServices] Entities target, [FromQuery] ByRequest request)
            {
                return target.By(
                    guid: request.Guid,
                    @string: request.String,
                    stringData: request.StringData,
                    int32: request.Int32,
                    uri: request.Uri,
                    status: request.Status,
                    dateTime: request.DateTime,
                    take: request.Take,
                    skip: request.Skip
                );
            }

            public record SingleByStringRequest(string String);

            [HttpGet]
            [Route("entities/single-by-string")]
            public Entity SingleByString([FromServices] Entities target, [FromQuery] SingleByStringRequest request)
            {
                return target.SingleByString(request.String);
            }

            public record FirstByStringRequest(string StartsWith,
                bool Asc = false,
                bool Desc = false
            );

            [HttpGet]
            [Route("entities/first-by-string")]
            public Entity FirstByString([FromServices] Entities target, [FromQuery] FirstByStringRequest request)
            {
                return target.FirstByString(request.StartsWith,
                    asc: request.Asc,
                    desc: request.Desc
                );
            }

            [HttpGet]
            [Route("entities/{id}")]
            public Entity Get([FromServices] IQueryContext<Entity> entityQuery, Guid id)
            {
                return entityQuery.SingleById(id);
            }

            public record NewRequest(
                Guid Guid = default,
                string String = default,
                string StringData = default,
                int Int32 = default,
                Uri Uri = default,
                object Dynamic = default,
                Status Status = default,
                DateTime DateTime = default
            );

            [HttpPost]
            [Route("entities")]
            public Entity New([FromServices] Func<Entity> newTarget, [FromBody] NewRequest request)
            {
                var target = newTarget();

                return target.With(request.Guid, request.String, request.StringData, request.Int32, request.Uri, request.Dynamic, request.Status, request.DateTime);
            }

            [HttpDelete]
            [Route("entities/{id}")]
            public void Delete([FromServices] IQueryContext<Entity> entityQuery, [FromRoute] Guid id)
            {
                var target = entityQuery.SingleById(id);

                target.Delete();
            }

            public record UpdateRequest(
                Guid Guid = default,
                string String = default,
                string StringData = default,
                int Int32 = default,
                Uri Uri = default,
                object Dynamic = default,
                Status Status = default,
                DateTime DateTime = default,
                bool useTransaction = false,
                bool throwError = false
            );

            [HttpPut]
            [Route("entities/{id}")]
            public async Task Update([FromServices] IQueryContext<Entity> entityQuery, [FromRoute] Guid id, [FromBody] UpdateRequest request)
            {
                var target = entityQuery.SingleById(id);

                await target.Update(request.Guid, request.String, request.StringData, request.Int32, request.Uri, request.Dynamic, request.Status, request.DateTime,
                    useTransaction: request.useTransaction,
                    throwError: request.throwError
                );
            }
        }
    """;
}
