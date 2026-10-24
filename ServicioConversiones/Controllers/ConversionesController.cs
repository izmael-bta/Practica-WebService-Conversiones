
using Microsoft.AspNetCore.Mvc;

namespace ServicioConversiones.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConversionesController : ControllerBase
    {
        // TEMPERATURA

        [HttpGet("celsius-fahrenheit")]
        public IActionResult CelsiusAFahrenheit(double valor)
        {
            double resultado = (valor * 9 / 5) + 32;

            return Ok(new
            {
                Conversion = "Celsius a Fahrenheit",
                ValorOriginal = valor,
                Resultado = resultado
            });
        }

        [HttpGet("fahrenheit-celsius")]
        public IActionResult FahrenheitACelsius(double valor)
        {
            double resultado = (valor - 32) * 5 / 9;

            return Ok(new
            {
                Conversion = "Fahrenheit a Celsius",
                ValorOriginal = valor,
                Resultado = resultado
            });
        }

        [HttpGet("celsius-kelvin")]
        public IActionResult CelsiusAKelvin(double valor)
        {
            double resultado = valor + 273.15;

            return Ok(new
            {
                Conversion = "Celsius a Kelvin",
                ValorOriginal = valor,
                Resultado = resultado
            });
        }

        [HttpGet("kelvin-celsius")]
        public IActionResult KelvinACelsius(double valor)
        {
            if (valor < 0)
                return BadRequest("Kelvin no puede ser negativo.");

            double resultado = valor - 273.15;

            return Ok(new
            {
                Conversion = "Kelvin a Celsius",
                ValorOriginal = valor,
                Resultado = resultado
            });
        }

        // LONGITUD

        [HttpGet("metros-kilometros")]
        public IActionResult MetrosAKilometros(double valor)
        {
            double resultado = valor / 1000;

            return Ok(new
            {
                Conversion = "Metros a Kilómetros",
                ValorOriginal = valor,
                Resultado = resultado
            });
        }

        [HttpGet("kilometros-metros")]
        public IActionResult KilometrosAMetros(double valor)
        {
            double resultado = valor * 1000;

            return Ok(new
            {
                Conversion = "Kilómetros a Metros",
                ValorOriginal = valor,
                Resultado = resultado
            });
        }

        [HttpGet("metros-pies")]
        public IActionResult MetrosAPies(double valor)
        {
            double resultado = valor / 0.3048;

            return Ok(new
            {
                Conversion = "Metros a Pies",
                ValorOriginal = valor,
                Resultado = resultado
            });
        }

        [HttpGet("pies-metros")]
        public IActionResult PiesAMetros(double valor)
        {
            double resultado = valor * 0.3048;

            return Ok(new
            {
                Conversion = "Pies a Metros",
                ValorOriginal = valor,
                Resultado = resultado
            });
        }
    }
}
