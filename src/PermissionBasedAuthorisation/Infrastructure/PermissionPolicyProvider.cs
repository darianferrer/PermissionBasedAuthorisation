using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace PermissionBasedAuthorisation.Infrastructure
{
    internal class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions _options;
        private readonly IAuthenticationSchemeProvider _authenticationSchemeProvider;
        private readonly object _policyCreationLock = new();

        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options, IAuthenticationSchemeProvider authenticationSchemeProvider) : base(options)
        {
            _options = options.Value;
            _authenticationSchemeProvider = authenticationSchemeProvider;
        }

        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            var policy = await base.GetPolicyAsync(policyName);

            if (policy == null)
            {
                var schemes = await _authenticationSchemeProvider.GetAllSchemesAsync();
                policy = new AuthorizationPolicyBuilder(schemes.Select(e => e.Name).ToArray())
                    .AddRequirements(new PermissionRequirement(policyName))
                    .Build();

                lock (_policyCreationLock)
                {
                    _options.AddPolicy(policyName, policy);
                }
            }

            return policy;
        }
    }
}
