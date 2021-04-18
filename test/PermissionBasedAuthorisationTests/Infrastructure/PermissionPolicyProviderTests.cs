using System;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Kernel;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Moq;
using PermissionBasedAuthorisation;
using PermissionBasedAuthorisation.Infrastructure;
using Xunit;

namespace PermissionBasedAuthorisationTests.Infrastructure
{
    public class PermissionPolicyProviderTests
    {
        private readonly PermissionPolicyProvider _sut;
        private readonly AuthorizationOptions _options = new();
        private readonly IAuthenticationSchemeProvider _authenticationSchemeProvider = Mock.Of<IAuthenticationSchemeProvider>();
        private readonly Fixture _fixture = new();

        public PermissionPolicyProviderTests()
        {
            var options = Mock.Of<IOptions<AuthorizationOptions>>();
            Mock.Get(options)
                .Setup(e => e.Value)
                .Returns(_options);
            _sut = new(options, _authenticationSchemeProvider);

            _fixture.Customizations.Add(new AuthenticationSchemeSpecimenBuilder());
        }

        [Fact]
        public async Task GetPolicyAsync_GivenPolicyExists_ShouldReturnPolicyFromOptions()
        {
            // Arrange
            var permission = _fixture.Create<string>();
            var expected = new PermissionRequirement(permission);
            _options.AddPolicy(permission, builder => builder.Requirements.Add(expected));

            // Act
            var result = await _sut.GetPolicyAsync(permission);

            // Assert
            result!.Requirements.Should()
                .AllBeOfType<PermissionRequirement>()
                .And.HaveCount(1)
                .And.AllBeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetPolicyAsync_GivenPolicyDoesNotExists_ShouldReturnNewPolicy()
        {
            // Arrange
            var permission = _fixture.Create<string>();
            var expected = new PermissionRequirement(permission);
            var expectedCount = _fixture.Create<int>();
            Mock.Get(_authenticationSchemeProvider)
                .Setup(e => e.GetAllSchemesAsync())
                .ReturnsAsync(_fixture.CreateMany<AuthenticationScheme>(expectedCount));

            // Act
            var result = await _sut.GetPolicyAsync(permission);

            // Assert
            using var scope = new AssertionScope();
            result!.AuthenticationSchemes.Should()
                .HaveCount(expectedCount);
            result.Requirements.Should()
                .AllBeOfType<PermissionRequirement>()
                .And.HaveCount(1)
                .And.AllBeEquivalentTo(expected);
        }
    }

    internal class AuthenticationSchemeSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(AuthenticationScheme))
            {
                return new AuthenticationScheme(context.Create<string>(), 
                    context.Create<string>(), 
                    typeof(AuthenticationHandler<AuthenticationSchemeOptions>));
            }

            return new NoSpecimen();
        }
    }
}
