using Microsoft.AspNetCore.Mvc;
using NumberMultiplier.Controllers;
using Xunit;
using FluentAssertions;

namespace NumberMultiplier.Tests
{
    public class NumberControllerTests
    {
        [Fact]
        public void MultiplyNumber_ShouldReturnOk_WithDoubledValue()
        {
            var controller = new NumberController();
            var req = new NumberRequest { Number = 2.5 };

            var actionResult = controller.MultiplyNumber(req);

            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var ok = actionResult.Result as OkObjectResult;
            ok!.Value.Should().BeOfType<NumberResponse>();
            var resp = (NumberResponse)ok.Value!;
            resp.OriginalNumber.Should().Be(2.5);
            resp.MultipliedNumber.Should().Be(5.0);
        }

        [Fact]
        public void MultiplyNumber_ShouldReturnBadRequest_WhenBodyIsNull()
        {
            var controller = new NumberController();

            var actionResult = controller.MultiplyNumber(null!);

            actionResult.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Theory]
        [InlineData(double.NaN)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(double.NegativeInfinity)]
        public void MultiplyNumber_ShouldReturnBadRequest_WhenNotFinite(double value)
        {
            var controller = new NumberController();
            var req = new NumberRequest { Number = value };

            var actionResult = controller.MultiplyNumber(req);

            actionResult.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Theory]
        [InlineData(-1_000_000_001d)]
        [InlineData( 1_000_000_001d)]
        public void MultiplyNumber_ShouldReturnBadRequest_WhenOutOfRange(double value)
        {
            var controller = new NumberController();
            var req = new NumberRequest { Number = value };

            var actionResult = controller.MultiplyNumber(req);

            actionResult.Result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}


