using DesafioDev.Models;

namespace DesafioDev.Services;

public class EstoqueService
{
    private readonly List<Produto> _produtos;

    public EstoqueService(List<Produto> produtos)
    {
        _produtos = produtos;
    }

    public MovimentacaoEstoque Movimentar(
        int codigoProduto,
        string tipo,
        string descricao,
        int quantidade)
    {
        if (quantidade <= 0)
        {
            throw new ArgumentException(
                "A quantidade deve ser maior que zero."
            );
        }

        Produto? produto = _produtos
            .FirstOrDefault(p => p.CodigoProduto == codigoProduto);

        if (produto is null)
        {
            throw new ArgumentException(
                "Produto não encontrado."
            );
        }

        if (tipo.Equals("SAIDA", StringComparison.OrdinalIgnoreCase))
        {
            if (produto.Estoque < quantidade)
            {
                throw new InvalidOperationException(
                    "Estoque insuficiente."
                );
            }

            produto.Estoque -= quantidade;
        }
        else if (tipo.Equals("ENTRADA", StringComparison.OrdinalIgnoreCase))
        {
            produto.Estoque += quantidade;
        }
        else
        {
            throw new ArgumentException(
                "Tipo deve ser ENTRADA ou SAIDA."
            );
        }

        return new MovimentacaoEstoque
        {
            Id = Guid.NewGuid(),
            CodigoProduto = codigoProduto,
            Tipo = tipo.ToUpper(),
            Descricao = descricao,
            Quantidade = quantidade,
            Data = DateTime.Now
        };
    }

    public Produto? BuscarProduto(int codigoProduto)
    {
        return _produtos.FirstOrDefault(
            p => p.CodigoProduto == codigoProduto
        );
    }
}