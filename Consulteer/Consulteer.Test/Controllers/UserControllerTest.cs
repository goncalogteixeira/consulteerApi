using AspNetCoreHero.Results;
using Consulteer.API.Controllers;
using Consulteer.Application.DTO;
using Consulteer.Application.Features.Users.Commands.LoginUser;
using Consulteer.Application.Features.Users.Commands.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consulteer.Test.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mockMediator = new Mock<IMediator>();
            _controller = new UsersController(null, null, _mockMediator.Object);
        }

        [Fact]
        public async Task GetUserToken_ReturnsOkResult_WhenRequestSucceeds()
        {
            // Arrange
            var email = "user@example.com";
            var password = "password";
            var result = new Result<string> { Succeeded = true, Message = "token" };
            _mockMediator.Setup(x => x.Send(It.IsAny<LoginUserCommand>(), default)).ReturnsAsync(result);

            // Act
            var actionResult = await _controller.GetUserToken(email, password);
            var okResult = actionResult as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(result, okResult.Value);
        }

        [Fact]
        public async Task GetUserToken_ReturnsBadRequestResult_WhenRequestFails()
        {
            // Arrange
            var email = "user@example.com";
            var password = "password";
            var result = new Result<string> { Succeeded = false, Message = "Login for this user failed" };
            _mockMediator.Setup(x => x.Send(It.IsAny<LoginUserCommand>(), default)).ReturnsAsync(result);

            // Act
            var actionResult = await _controller.GetUserToken(email, password);
            var badRequestResult = actionResult as BadRequestObjectResult;

            // Assert
            Assert.NotNull(badRequestResult);
            Assert.Equal(result, badRequestResult.Value);
        }

        [Fact]
        public async Task Register_ReturnsOkResult_WhenRequestSucceeds()
        {
            // Arrange
            var email = "user@example.com";
            var name = "user";
            var password = "password";
            var result = new Result<UserDTO> { Succeeded = true };
            _mockMediator.Setup(x => x.Send(It.IsAny<RegisterUserCommand>(), default)).ReturnsAsync(result);

            // Act
            var actionResult = await _controller.Register(email, name, password);
            var okResult = actionResult as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(result, okResult.Value);
        }

        [Fact]
        public async Task Register_ReturnsBadRequestResult_WhenRequestFails()
        {
            // Arrange
            var email = "user@example.com";
            var name = "user";
            var password = "password";
            var result = new Result<UserDTO> { Succeeded = false, Message = "Email already exists" };
            _mockMediator.Setup(x => x.Send(It.IsAny<RegisterUserCommand>(), default)).ReturnsAsync(result);

            // Act
            var actionResult = await _controller.Register(email, name, password);
            var badRequestResult = actionResult as BadRequestObjectResult;

            // Assert
            Assert.NotNull(badRequestResult);
            Assert.Equal(result, badRequestResult.Value);
        }
    }

}
