using System.Text.Json;
using DesafioDev.Models;
using DesafioDev.Services;

Console.WriteLine("Iniciando programa...");

string caminhoArquivo = Path.Combine(
    AppContext.BaseDirectory,
    "Data",
    "vendas.json"
);

Console.WriteLine($"Arquivo: {caminhoArquivo}");

if (!File.Exists(caminhoArquivo))
{
    Console.WriteLine("Arquivo não encontrado.");
    return;
}

Console.WriteLine("Arquivo encontrado.");

string json = File.ReadAllText(caminhoArquivo);

Console.WriteLine("JSON carregado.");

VendaArquivo? arquivo = JsonSerializer.Deserialize<VendaArquivo>(json);

if (arquivo is null)
{
    Console.WriteLine("Não foi possível desserializar o JSON.");
    return;
}

Console.WriteLine($"Quantidade de vendas: {arquivo.Vendas.Count}");

var service = new ComissaoService();

var resultados = service.CalcularComissaoPorVendedor(arquivo.Vendas);

Console.WriteLine();
Console.WriteLine("===== COMISSÕES =====");

foreach (var resultado in resultados)
{
    Console.WriteLine(
        $"{resultado.Key}: R$ {resultado.Value:N2}"
    );
}