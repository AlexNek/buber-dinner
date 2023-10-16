using BuberDinner.Api.Controllers;
using BuberDinner.Api.Http;

using ErrorOr;

using FluentAssertions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;

namespace BuberDinner.Api.UnitTests.Controllers
{
    public class ApiControllerTests
    {
        private readonly TestApiController _testClass;

        public ApiControllerTests()
        {
            _testClass = new TestApiController();
            HttpContext httpContext = new DefaultHttpContext();
            ActionContext context = new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor());
            _testClass.ControllerContext = new ControllerContext(context);
        }

        [Fact]
        public void CheckProblemFunc_WhenConflict_Return409()
        {
            // Arrange
            var errors = new List<Error> { Error.Conflict() };

            // Act
            ObjectResult? result = _testClass.PublicProblem(errors) as ObjectResult;

            // Assert
            var items = _testClass.HttpContext.Items[HttpContextItemKeys.Errors] as List<Error>;
            items.Should().NotBeNull();
            items.Count.Should().Be(errors.Count);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(409);
            var details = result.Value as ProblemDetails;
            details.Should().NotBeNull();
            details.Title.Should().Be("A conflict error has occurred.");
        }

        [Fact]
        public void CheckProblemFunc_WhenFailure_Return500()
        {
            // Arrange
            var errors = new List<Error> { Error.Failure() };

            // Act
            ObjectResult? result = _testClass.PublicProblem(errors) as ObjectResult;

            // Assert
            var items = _testClass.HttpContext.Items[HttpContextItemKeys.Errors] as List<Error>;
            items.Should().NotBeNull();
            items.Count.Should().Be(errors.Count);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(500);
            var details = result.Value as ProblemDetails;
            details.Should().NotBeNull();
            details.Title.Should().Be("A failure has occurred.");
        }

        [Fact]
        public void CheckProblemFunc_WhenLotOfErrors_UseHttpContext()
        {
            // Arrange
            var errors = new List<Error>
                             {
                                 Error.Failure(),
                                 Error.Conflict(),
                                 Error.NotFound(),
                                 Error.Unexpected(),
                                 Error.Validation()
                             };

            // Act
            ObjectResult? result = _testClass.PublicProblem(errors) as ObjectResult;

            // Assert
            var items = _testClass.HttpContext.Items[HttpContextItemKeys.Errors] as List<Error>;
            items.Should().NotBeNull();
            items.Count.Should().Be(errors.Count);
            result.Should().NotBeNull();
            result.StatusCode.Should().NotBe(200);
        }

        [Fact]
        public void CheckProblemFunc_WhenNotFound_Return404()
        {
            // Arrange
            var errors = new List<Error> { Error.NotFound() };

            // Act
            ObjectResult? result = _testClass.PublicProblem(errors) as ObjectResult;

            // Assert
            var items = _testClass.HttpContext.Items[HttpContextItemKeys.Errors] as List<Error>;
            items.Should().NotBeNull();
            items.Count.Should().Be(errors.Count);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404);
            var details = result.Value as ProblemDetails;
            details.Should().NotBeNull();
            details.Title.Should().Be("A 'Not Found' error has occurred.");
        }

        [Fact]
        public void CheckProblemFunc_WhenUnexpected_Return500()
        {
            // Arrange
            var errors = new List<Error> { Error.Unexpected() };

            // Act
            ObjectResult? result = _testClass.PublicProblem(errors) as ObjectResult;

            // Assert
            var items = _testClass.HttpContext.Items[HttpContextItemKeys.Errors] as List<Error>;
            items.Should().NotBeNull();
            items.Count.Should().Be(errors.Count);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(500);
            var details = result.Value as ProblemDetails;
            details.Should().NotBeNull();
            details.Title.Should().Be("An unexpected error has occurred.");
        }

        [Fact]
        public void CheckProblemFunc_WhenValidation_ReturnValidationProblem()
        {
            // Arrange
            var errors = new List<Error> { Error.Validation() };

            // Act
            ObjectResult? result = _testClass.PublicProblem(errors) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().BeNull();
            var details = result.Value as ValidationProblemDetails;
            details.Should().NotBeNull();
            details.Errors.Count.Should().Be(1);
        }

        [Fact]
        public void CheckProblemFunc_WithNullErrors_ThrowException()
        {
            FluentActions.Invoking(() => _testClass.PublicProblem(default(List<Error>))).Should().Throw<ArgumentNullException>()
                .WithParameterName("errors");
        }

        private class TestApiController : ApiController
        {
            public IActionResult PublicProblem(List<Error> errors)
            {
                return Problem(errors);
            }
        }
    }
}
