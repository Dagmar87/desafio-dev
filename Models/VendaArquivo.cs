using System.Text.Json.Serialization;

namespace DesafioDev.Models;

public class VendaArquivo
{
    [JsonPropertyName("vendas")]
    public List<Venda> Vendas { get; set; } = new();
}