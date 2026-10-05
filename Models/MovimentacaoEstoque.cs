namespace DesafioDev.Models;

public class MovimentacaoEstoque
{
    public Guid Id { get; set; }

    public int CodigoProduto { get; set; }

    public string Tipo { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public int Quantidade { get; set; }

    public DateTime Data { get; set; }
}