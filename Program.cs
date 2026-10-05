using System.Text.Json;
using DesafioDev.Models;
using DesafioDev.Services;

string caminhoArquivo = Path.Combine(
    AppContext.BaseDirectory,
    "Data",
    "vendas.json"
);

string json = File.ReadAllText(caminhoArquivo);

VendaArquivo? arquivo = JsonSerializer.Deserialize<VendaArquivo>(json);

if (arquivo is null)
{
    Console.WriteLine("Não foi possível carregar o arquivo.");
    return;
}

var service = new ComissaoService();

var resultados = service.CalcularComissaoPorVendedor(arquivo.Vendas);

foreach (var resultado in resultados)
{
    Console.WriteLine(
        $"{resultado.Key}: R$ {resultado.Value:N2}"
    );
}