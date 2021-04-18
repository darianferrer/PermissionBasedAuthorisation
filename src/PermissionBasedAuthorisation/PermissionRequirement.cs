using Microsoft.AspNetCore.Authorization;

namespace PermissionBasedAuthorisation
{
    /// <summary>
    ///     Permission to be challenged
    /// </summary>
    public class PermissionRequirement : IAuthorizationRequirement
    {
        internal PermissionRequirement(string permission)
        {
            Permission = permission;
        }

        /// <summary>
        ///     Permission code
        /// </summary>
        public string Permission { get; }
    }
}
