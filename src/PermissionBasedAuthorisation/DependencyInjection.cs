using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using PermissionBasedAuthorisation.Infrastructure;

namespace PermissionBasedAuthorisation
{
    /// <summary>
    ///     Extension methods for adding services to an <see cref="IServiceCollection"/>.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        ///     Adds the core services to create authorisation policies.
        /// </summary>
        /// <typeparam name="TPermissionVerificationService">Service that will verify if a user has a permission.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddPbacCore<TPermissionVerificationService>(this IServiceCollection services)
            where TPermissionVerificationService : class, IPermissionVerificationService
        {
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionHandler>();
            services.AddScoped<IPermissionVerificationService, TPermissionVerificationService>();

            return services;
        }
    }
}
