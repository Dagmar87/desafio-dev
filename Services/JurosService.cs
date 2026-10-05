namespace DesafioDev.Services;

public class JurosService
{
    private const decimal TaxaDiaria = 0.025m;

    public decimal CalcularJuros(
        decimal valor,
        DateTime dataVencimento,
        DateTime dataAtual)
    {
        if (valor <= 0)
        {
            throw new ArgumentException(
                "O valor deve ser maior que zero."
            );
        }

        if (dataAtual <= dataVencimento)
        {
            return 0;
        }

        int diasAtraso =
            (dataAtual.Date - dataVencimento.Date).Days;

        return valor * TaxaDiaria * diasAtraso;
    }

    public decimal CalcularValorTotal(
        decimal valor,
        DateTime dataVencimento,
        DateTime dataAtual)
    {
        decimal juros = CalcularJuros(
            valor,
            dataVencimento,
            dataAtual
        );

        return valor + juros;
    }
}