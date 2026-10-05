using DesafioDev.Models;

namespace DesafioDev.Services;

public class ComissaoService
{
    public decimal CalcularComissao(decimal valorVenda)
    {
        if (valorVenda < 100)
        {
            return 0;
        }

        if (valorVenda < 500)
        {
            return valorVenda * 0.01m;
        }

        return valorVenda * 0.05m;
    }

    public Dictionary<string, decimal> CalcularComissaoPorVendedor(
        List<Venda> vendas)
    {
        return vendas
            .GroupBy(v => v.Vendedor)
            .ToDictionary(
                grupo => grupo.Key,
                grupo => grupo.Sum(v => CalcularComissao(v.Valor))
            );
    }
}