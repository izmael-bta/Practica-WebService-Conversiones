
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace WebConversiones.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public string TipoConversion { get; set; } = "celsius-fahrenheit";

        [BindProperty]
        public string Valor { get; set; } = "";

        public double? Resultado { get; set; }

        public string? Error { get; set; }

        public string? NombreConversion { get; set; }

        public async Task OnPostAsync()
        {
            string[] rutas =
            {
                "celsius-fahrenheit",
                "fahrenheit-celsius",
                "celsius-kelvin",
                "kelvin-celsius",
                "metros-kilometros",
                "kilometros-metros",
                "metros-pies",
                "pies-metros"
            };

            if (!rutas.Contains(TipoConversion))
            {
                Error = "Seleccione una conversión válida.";
                return;
            }

            bool valido = double.TryParse(
                Valor,
                NumberStyles.Float,
                CultureInfo.CurrentCulture,
                out double numero);

            if (!valido)
            {
                valido = double.TryParse(
                    Valor,
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out numero);
            }

            if (!valido || !double.IsFinite(numero))
            {
                Error = "Ingrese un número válido.";
                return;
            }

            try
            {
                HttpClient cliente =
                    _httpClientFactory.CreateClient("ConversionesAPI");

                string valorFormato =
                    numero.ToString(CultureInfo.InvariantCulture);

                string url =
                    $"api/conversiones/{TipoConversion}?valor={valorFormato}";

                using HttpResponseMessage respuesta =
                    await cliente.GetAsync(url);

                if (!respuesta.IsSuccessStatusCode)
                {
                    Error = $"El servicio devolvió el error {(int)respuesta.StatusCode}.";
                    return;
                }

                ResultadoAPI? datos =
                    await respuesta.Content.ReadFromJsonAsync<ResultadoAPI>();

                if (datos is null)
                {
                    Error = "No se recibió una respuesta válida.";
                    return;
                }

                NombreConversion = datos.Conversion;
                Resultado = datos.Resultado;
            }
            catch (HttpRequestException ex)
            {
                Error = $"No se pudo conectar al Web Service: {ex.Message}";
            }
            catch (TaskCanceledException)
            {
                Error = "El servicio tardó demasiado en responder.";
            }
        }

        public class ResultadoAPI
        {
            [JsonPropertyName("conversion")]
            public string Conversion { get; set; } = "";

            [JsonPropertyName("resultado")]
            public double Resultado { get; set; }
        }
    }
}
