using System.Security.Claims;
using System.Threading.Tasks;

namespace PermissionBasedAuthorisation
{
    /// <summary>
    ///     Responsible for verifying that a user has a permission
    /// </summary>
    public interface IPermissionVerificationService
    {
        /// <summary>
        ///     Checks that the current user has the permission specified
        /// </summary>
        /// <param name="claimsPrincipal">User</param>
        /// <param name="requirement">Permission</param>
        Task<bool> ChallengePermissionAsync(ClaimsPrincipal claimsPrincipal, PermissionRequirement requirement);
    }
}
