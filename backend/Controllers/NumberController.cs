using Microsoft.AspNetCore.Mvc;

namespace NumberMultiplier.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NumberController : ControllerBase
    {
        // POST: /api/number/multiply
        // Принимает число и возвращает результат умножения на 2
        [HttpPost("multiply")]
        public ActionResult<NumberResponse> MultiplyNumber([FromBody] NumberRequest request)
        {
            if (request is null)
            {
                return BadRequest("Тело запроса обязательно");
            }

            // Простая валидация: число должно быть конечным и в разумных пределах
            if (double.IsNaN(request.Number) || double.IsInfinity(request.Number))
            {
                return BadRequest("Число должно быть конечным значением");
            }

            const double minAllowed = -1e9;
            const double maxAllowed =  1e9;
            if (request.Number < minAllowed || request.Number > maxAllowed)
            {
                return BadRequest($"Число должно быть в диапазоне от {minAllowed} до {maxAllowed}");
            }

            var result = request.Number * 2;

            return Ok(new NumberResponse
            {
                OriginalNumber = request.Number,
                MultipliedNumber = result
            });
        }
    }

    // Модель входящего запроса
    public class NumberRequest
    {
        public double Number { get; set; }
    }

    // Модель ответа API
    public class NumberResponse
    {
        public double OriginalNumber { get; set; }
        public double MultipliedNumber { get; set; }
    }
}