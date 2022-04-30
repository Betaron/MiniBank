using System.Text;
using System.Text.Json;
using Minibank.Core.Exceptions;

namespace Minibank.Web.Middlewares
{
    public class CustomAuthenticationMiddleware
    {
        public readonly RequestDelegate next;

        public CustomAuthenticationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string authToken = httpContext.Request.Headers["Authorization"];

            if (authToken is not null)
            {
                var base64Payload = authToken.Split(".")[1];
                var jsonPayload = Encoding.UTF8
                    .GetString(Convert.FromBase64String(base64Payload))
                    .Replace("\\", "");

                var payload = JsonSerializer.Deserialize<JsonElement>(jsonPayload);

                var exp = payload.GetProperty("exp").GetInt32();

                var expireDate = new DateTime(
                        1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                    .AddSeconds(exp);

                if (DateTime.UtcNow >= expireDate)
                {
                    throw new ExpiredTokenException();
                }
            }

            await next(httpContext);
        }
    }
}
