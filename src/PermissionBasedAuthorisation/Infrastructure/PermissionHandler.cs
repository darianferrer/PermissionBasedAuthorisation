using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace PermissionBasedAuthorisation.Infrastructure
{
    internal class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IPermissionVerificationService _permissionVerificationService;

        public PermissionHandler(IPermissionVerificationService permissionVerificationService)
        {
            _permissionVerificationService = permissionVerificationService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (await _permissionVerificationService.ChallengePermissionAsync(context.User, requirement))
            {
                context.Succeed(requirement);
            }
        }
    }
}
