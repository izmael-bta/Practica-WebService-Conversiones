
using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

const string urlServicio = "https://localhost:7171";

using HttpClient cliente = new HttpClient
{
    BaseAddress = new Uri(urlServicio),
    Timeout = TimeSpan.FromSeconds(15)
};

string[] conversiones =
{
    "Celsius a Fahrenheit",
    "Fahrenheit a Celsius",
    "Celsius a Kelvin",
    "Kelvin a Celsius",
    "Metros a Kilómetros",
    "Kilómetros a Metros",
    "Metros a Pies",
    "Pies a Metros"
};

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

while (true)
{
    Console.Clear();
    Console.WriteLine("==================================");
    Console.WriteLine("      CONVERSOR DE UNIDADES");
    Console.WriteLine("==================================");
    Console.WriteLine();

    for (int i = 0; i < conversiones.Length; i++)
    {
        Console.WriteLine($"{i + 1}. {conversiones[i]}");
    }

    Console.WriteLine("0. Salir");
    Console.WriteLine();

    Console.Write("Seleccione una opción: ");
    string? entrada = Console.ReadLine();

    if (!int.TryParse(entrada, out int opcion) ||
        opcion < 0 || opcion > conversiones.Length)
    {
        Console.WriteLine("Opción no válida.");
        Pausar();
        continue;
    }

    if (opcion == 0)
    {
        Console.WriteLine("Programa finalizado.");
        break;
    }

    Console.Write("Ingrese el valor a convertir: ");
    string textoValor = Console.ReadLine() ?? "";

    bool valorValido =
        double.TryParse(textoValor, NumberStyles.Float,
            CultureInfo.CurrentCulture, out double valor)
        || double.TryParse(textoValor, NumberStyles.Float,
            CultureInfo.InvariantCulture, out valor);

    if (!valorValido || !double.IsFinite(valor))
    {
        Console.WriteLine("Ingrese un número válido.");
        Pausar();
        continue;
    }

    string ruta = rutas[opcion - 1];
    string numero = valor.ToString(CultureInfo.InvariantCulture);
    string endpoint = $"/api/conversiones/{ruta}?valor={numero}";

    try
    {
        using HttpResponseMessage respuesta =
            await cliente.GetAsync(endpoint);

        if (!respuesta.IsSuccessStatusCode)
        {
            string error = await respuesta.Content.ReadAsStringAsync();
            Console.WriteLine($"Error del servicio ({(int)respuesta.StatusCode}):");
            Console.WriteLine(error);
        }
        else
        {
            ResultadoConversion? datos =
                await respuesta.Content.ReadFromJsonAsync<ResultadoConversion>();

            if (datos is not null)
            {
                Console.WriteLine();
                Console.WriteLine("------ RESULTADO ------");
                Console.WriteLine($"Conversión: {datos.Conversion}");
                Console.WriteLine($"Valor original: {datos.ValorOriginal}");
                Console.WriteLine($"Resultado: {datos.Resultado:F4}");
                Console.WriteLine("-----------------------");
            }
        }
    }

    catch (HttpRequestException ex)
    {
        Console.WriteLine("Error al comunicarse con el Web Service.");
        Console.WriteLine($"Detalle: {ex.Message}");

        if (ex.InnerException != null)
        {
            Console.WriteLine($"Causa interna: {ex.InnerException.Message}");
        }
    }


    Pausar();
}

static void Pausar()
{
    Console.WriteLine();
    Console.WriteLine("Presione ENTER para continuar...");
    Console.ReadLine();
}

public class ResultadoConversion
{
    [JsonPropertyName("conversion")]
    public string Conversion { get; set; } = "";

    [JsonPropertyName("valorOriginal")]
    public double ValorOriginal { get; set; }

    [JsonPropertyName("resultado")]
    public double Resultado { get; set; }
}
