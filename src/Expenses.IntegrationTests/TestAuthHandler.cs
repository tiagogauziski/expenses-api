using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Expenses.IntegrationTests
{
    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            return base.HandleForbiddenAsync(properties);
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            return base.HandleChallengeAsync(properties);
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var identity = new ClaimsIdentity(new List<Claim>
            {
                new Claim("permissions", "read:statements", "", "https://myexpenses.au.auth0.com/"),
                new Claim("permissions", "create:statements", "", "https://myexpenses.au.auth0.com/"),
                new Claim("permissions", "update:statements", "", "https://myexpenses.au.auth0.com/"),
                new Claim("permissions", "delete:statements", "", "https://myexpenses.au.auth0.com/"),
                new Claim("permissions", "read:invoices", "", "https://myexpenses.au.auth0.com/"),
                new Claim("permissions", "create:invoices", "", "https://myexpenses.au.auth0.com/"),
                new Claim("permissions", "update:invoices", "", "https://myexpenses.au.auth0.com/"),
                new Claim("permissions", "delete:invoices", "", "https://myexpenses.au.auth0.com/"),
                new Claim(ClaimTypes.Name, "Test")
            }, "AuthenticationType");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Test");
            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}
