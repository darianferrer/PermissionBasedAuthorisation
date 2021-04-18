using System.Security.Claims;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Moq;
using PermissionBasedAuthorisation;
using PermissionBasedAuthorisation.Infrastructure;
using Xunit;

namespace PermissionBasedAuthorisationTests.Infrastructure
{
    public class PermissionHandlerTests
    {
        private readonly PermissionHandler _sut;
        private readonly IPermissionVerificationService _permissionVerificationService = Mock.Of<IPermissionVerificationService>();
        private readonly Fixture _fixture = new();

        public PermissionHandlerTests()
        {
            _sut = new(_permissionVerificationService);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task HandleRequirementAsync_GivenValidatorReturns_ShouldReturn(bool validatorResult)
        {
            // Arrange
            var requirement = new PermissionRequirement(_fixture.Create<string>());
            var user = new ClaimsPrincipal(new ClaimsIdentity(_fixture.Create<string>()));
            var context = new AuthorizationHandlerContext(new[] { requirement }, user, string.Empty);

            Mock.Get(_permissionVerificationService)
                .Setup(e => e.ChallengePermissionAsync(user, requirement))
                .ReturnsAsync(validatorResult);

            // Act
            await _sut.HandleAsync(context);

            // Assert
            context.HasSucceeded.Should().Be(validatorResult);
        }
    }
}
