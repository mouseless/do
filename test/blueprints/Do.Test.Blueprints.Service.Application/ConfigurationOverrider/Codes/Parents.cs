namespace Do.Test.ConfigurationOverrider.Codes;

public static class Parents
{
    public static readonly string Code = """
        using Do.Orm;
        using Microsoft.AspNetCore.Mvc;

        namespace Do.Test.RestApi.Analyzer;

        [ApiController]
        public class ParentsController
        {
            public record AllRequest(
                bool Asc = false,
                bool Desc = false,
                int? Take = default,
                int? Skip = default
            );

            [HttpGet]
            [Route("parents")]
            public List<Parent> All([FromServices] Parents target, [FromQuery] AllRequest request)
            {
                return target.All(
                    asc: request.Asc,
                    desc: request.Desc,
                    take: request.Take,
                    skip: request.Skip
                );
            }

            public record ByNameRequest(string Contains,
                bool Asc = false,
                bool Desc = false,
                int? Take = default,
                int? Skip = default
            );

            [HttpGet]
            [Route("parents/by-name")]
            public List<Parent> ByName([FromServices] Parents target, [FromQuery] ByNameRequest request)
            {
                return target.ByName(request.Contains,
                    asc: request.Asc,
                    desc: request.Desc,
                    take: request.Take,
                    skip: request.Skip
                );
            }

            [HttpGet]
            [Route("parents/{id}")]
            public Parent Get([FromServices] IQueryContext<Parent> parentQuery, Guid id)
            {
                return parentQuery.SingleById(id);
            }

            [HttpGet]
            [Route("parents/{id}/children")]
            public List<Child> GetChildren([FromServices] IQueryContext<Parent> parentQuery, [FromRoute] Guid id)
            {
                var target = parentQuery.SingleById(id);

                return target.GetChildren();
            }

            public record AddChildRequest();

            [HttpPost]
            [Route("parents/{id}/children")]
            public void AddChild([FromServices] IQueryContext<Parent> parentQuery, [FromRoute] Guid id, [FromBody] AddChildRequest request)
            {
                var target = parentQuery.SingleById(id);

                target.AddChild();
            }
        }
    """;
}
