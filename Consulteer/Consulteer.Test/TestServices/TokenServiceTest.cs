using Consulteer.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Consulteer.Test.TestServices
{

    public class TokenGeneratorTests
    {
        private readonly IConfiguration _configuration;

        public TokenGeneratorTests()
        {
            // Set up the configuration for the tests
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"Jwt:Key", "SecretKey10125779374235322"},
                    {"Jwt:Issuer", "https://localhost:7120"},
                    {"Jwt:Audience", "https://localhost:7120"}
                })
                .Build();
        }

        [Fact]
        public void GenerateToken_Returns_Valid_Token()
        {
            // Arrange
            var userName = "john.doe";
            var role = "admin";
            var claims = new List<Claim>();

            // Act
            var tokenGenerator = new TokenService(_configuration);
            var token = tokenGenerator.GenerateToken(userName, role, claims);

            // Assert
            Assert.NotNull(token);
            Assert.NotEmpty(token);

            var tokenHandler = new JwtSecurityTokenHandler();
            var decodedToken = tokenHandler.ReadJwtToken(token);

            Assert.Equal(userName, decodedToken.Claims.First(c => c.Type == ClaimTypes.Name).Value);
            Assert.Equal(role, decodedToken.Claims.First(c => c.Type == ClaimTypes.Role).Value);
            Assert.Equal(_configuration["Jwt:Issuer"], decodedToken.Issuer);
        }

        [Fact]
        public void GenerateToken_Throws_Exception_When_Configuration_Is_Invalid()
        {
            // Arrange
            var userName = "john.doe";
            var role = "admin";
            var claims = new List<Claim>();

            // Set up an invalid configuration
            var invalidConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"Jwt:Key", ""},
                    {"Jwt:Issuer", ""},
                    {"Jwt:Audience", ""}
                })
                .Build();

            // Act & Assert
            var tokenGenerator = new TokenService(invalidConfiguration);
            Assert.Throws<ArgumentException>(() => tokenGenerator.GenerateToken(userName, role, claims));
        }
    }
}

