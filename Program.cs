using System.Text.Json;
using DesafioDev.Models;
using DesafioDev.Services;

while (true)
{
    Console.Clear();

    Console.WriteLine("========================================");
    Console.WriteLine("           DESAFIO DEV - C#");
    Console.WriteLine("========================================");
    Console.WriteLine();
    Console.WriteLine("1 - Calcular comissões");
    Console.WriteLine("2 - Movimentar estoque");
    Console.WriteLine("3 - Calcular juros");
    Console.WriteLine("0 - Sair");
    Console.WriteLine();

    Console.Write("Escolha uma opção: ");

    string? opcao = Console.ReadLine();

    try
    {
        switch (opcao)
        {
            case "1":
                ExecutarComissoes();
                break;

            case "2":
                ExecutarEstoque();
                break;

            case "3":
                ExecutarJuros();
                break;

            case "0":
                Console.WriteLine();
                Console.WriteLine("Encerrando programa...");
                return;

            default:
                Console.WriteLine();
                Console.WriteLine("Opção inválida.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine("========================================");
        Console.WriteLine("ERRO");
        Console.WriteLine("========================================");
        Console.WriteLine(ex.Message);
    }

    Console.WriteLine();
    Console.WriteLine("Pressione ENTER para continuar...");
    Console.ReadLine();
}


// ======================================================
// QUESTÃO 1 - COMISSÕES
// ======================================================

static void ExecutarComissoes()
{
    Console.Clear();

    Console.WriteLine("========================================");
    Console.WriteLine("        QUESTÃO 1 - COMISSÕES");
    Console.WriteLine("========================================");
    Console.WriteLine();

    string caminhoArquivo = Path.Combine(
        AppContext.BaseDirectory,
        "Data",
        "vendas.json"
    );

    Console.WriteLine($"Arquivo: {caminhoArquivo}");
    Console.WriteLine();

    if (!File.Exists(caminhoArquivo))
    {
        Console.WriteLine(
            "Arquivo vendas.json não encontrado."
        );

        return;
    }

    string json = File.ReadAllText(caminhoArquivo);

    VendaArquivo? arquivo =
        JsonSerializer.Deserialize<VendaArquivo>(json);

    if (arquivo is null)
    {
        Console.WriteLine(
            "Não foi possível carregar o arquivo de vendas."
        );

        return;
    }

    if (arquivo.Vendas.Count == 0)
    {
        Console.WriteLine(
            "Nenhuma venda foi encontrada."
        );

        return;
    }

    var service = new ComissaoService();

    Dictionary<string, decimal> resultados =
        service.CalcularComissaoPorVendedor(
            arquivo.Vendas
        );

    Console.WriteLine("===== COMISSÕES POR VENDEDOR =====");
    Console.WriteLine();

    foreach (var resultado in resultados)
    {
        Console.WriteLine(
            $"Vendedor: {resultado.Key}"
        );

        Console.WriteLine(
            $"Comissão: R$ {resultado.Value:N2}"
        );

        Console.WriteLine();
    }
}


// ======================================================
// QUESTÃO 2 - ESTOQUE
// ======================================================

static void ExecutarEstoque()
{
    Console.Clear();

    Console.WriteLine("========================================");
    Console.WriteLine("        QUESTÃO 2 - ESTOQUE");
    Console.WriteLine("========================================");
    Console.WriteLine();

    string caminhoArquivo = Path.Combine(
        AppContext.BaseDirectory,
        "Data",
        "estoque.json"
    );

    Console.WriteLine($"Arquivo: {caminhoArquivo}");
    Console.WriteLine();

    if (!File.Exists(caminhoArquivo))
    {
        Console.WriteLine(
            "Arquivo estoque.json não encontrado."
        );

        return;
    }

    string json = File.ReadAllText(caminhoArquivo);

    /*
     * O arquivo possui a estrutura:
     *
     * {
     *   "estoque": [
     *      ...
     *   ]
     * }
     *
     * O JsonDocument acessa o array "estoque".
     */

    using JsonDocument documento =
        JsonDocument.Parse(json);

    if (!documento.RootElement.TryGetProperty(
        "estoque",
        out JsonElement estoqueJson))
    {
        Console.WriteLine(
            "A propriedade 'estoque' não foi encontrada no JSON."
        );

        return;
    }

    /*
     * O JSON utiliza:
     *
     * codigoProduto
     * descricaoProduto
     * estoque
     *
     * Enquanto o modelo utiliza:
     *
     * CodigoProduto
     * DescricaoProduto
     * Estoque
     *
     * PropertyNameCaseInsensitive = true
     * permite a desserialização sem precisar
     * alterar o modelo Produto.
     */

    var opcoesJson = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    List<Produto> produtos =
        JsonSerializer.Deserialize<List<Produto>>(
            estoqueJson.GetRawText(),
            opcoesJson
        ) ?? new List<Produto>();

    if (produtos.Count == 0)
    {
        Console.WriteLine(
            "Nenhum produto foi encontrado no estoque."
        );

        return;
    }

    var estoqueService =
        new EstoqueService(produtos);

    Console.WriteLine("PRODUTOS DISPONÍVEIS");
    Console.WriteLine();

    foreach (var produto in produtos)
    {
        Console.WriteLine(
            $"Código: {produto.CodigoProduto} | " +
            $"Produto: {produto.DescricaoProduto} | " +
            $"Estoque: {produto.Estoque}"
        );
    }

    Console.WriteLine();

    // ------------------------------------------
    // Código do produto
    // ------------------------------------------

    Console.Write("Digite o código do produto: ");

    if (!int.TryParse(
        Console.ReadLine(),
        out int codigoProduto))
    {
        Console.WriteLine();
        Console.WriteLine(
            "Código do produto inválido."
        );

        return;
    }

    Produto? produtoSelecionado =
        estoqueService.BuscarProduto(codigoProduto);

    if (produtoSelecionado is null)
    {
        Console.WriteLine();
        Console.WriteLine(
            "Produto não encontrado."
        );

        return;
    }

    Console.WriteLine();
    Console.WriteLine(
        $"Produto selecionado: " +
        $"{produtoSelecionado.DescricaoProduto}"
    );

    Console.WriteLine(
        $"Estoque atual: {produtoSelecionado.Estoque}"
    );

    Console.WriteLine();

    // ------------------------------------------
    // Tipo da movimentação
    // ------------------------------------------

    Console.WriteLine("TIPO DE MOVIMENTAÇÃO");
    Console.WriteLine();
    Console.WriteLine("1 - ENTRADA");
    Console.WriteLine("2 - SAIDA");
    Console.WriteLine();

    Console.Write("Escolha: ");

    string? opcaoTipo = Console.ReadLine();

    string tipo;

    if (opcaoTipo == "1")
    {
        tipo = "ENTRADA";
    }
    else if (opcaoTipo == "2")
    {
        tipo = "SAIDA";
    }
    else
    {
        Console.WriteLine();
        Console.WriteLine(
            "Tipo de movimentação inválido."
        );

        return;
    }

    // ------------------------------------------
    // Descrição
    // ------------------------------------------

    Console.WriteLine();

    Console.Write(
        "Digite a descrição da movimentação: "
    );

    string? descricao = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(descricao))
    {
        Console.WriteLine();
        Console.WriteLine(
            "A descrição é obrigatória."
        );

        return;
    }

    // ------------------------------------------
    // Quantidade
    // ------------------------------------------

    Console.WriteLine();

    Console.Write("Digite a quantidade: ");

    if (!int.TryParse(
        Console.ReadLine(),
        out int quantidade))
    {
        Console.WriteLine();
        Console.WriteLine(
            "Quantidade inválida."
        );

        return;
    }

    if (quantidade <= 0)
    {
        Console.WriteLine();
        Console.WriteLine(
            "A quantidade deve ser maior que zero."
        );

        return;
    }

    // ------------------------------------------
    // Executar movimentação
    // ------------------------------------------

    MovimentacaoEstoque movimentacao =
        estoqueService.Movimentar(
            codigoProduto,
            tipo,
            descricao,
            quantidade
        );

    Produto produtoAtualizado =
        estoqueService.BuscarProduto(
            codigoProduto
        )!;

    Console.WriteLine();
    Console.WriteLine("========================================");
    Console.WriteLine("       MOVIMENTAÇÃO REALIZADA");
    Console.WriteLine("========================================");
    Console.WriteLine();

    Console.WriteLine(
        $"Identificador: {movimentacao.Id}"
    );

    Console.WriteLine(
        $"Produto: {produtoAtualizado.DescricaoProduto}"
    );

    Console.WriteLine(
        $"Código: {movimentacao.CodigoProduto}"
    );

    Console.WriteLine(
        $"Tipo: {movimentacao.Tipo}"
    );

    Console.WriteLine(
        $"Descrição: {movimentacao.Descricao}"
    );

    Console.WriteLine(
        $"Quantidade movimentada: " +
        $"{movimentacao.Quantidade}"
    );

    Console.WriteLine(
        $"Data: {movimentacao.Data:dd/MM/yyyy HH:mm:ss}"
    );

    Console.WriteLine();

    Console.WriteLine(
        $"ESTOQUE FINAL: {produtoAtualizado.Estoque}"
    );
}


// ======================================================
// QUESTÃO 3 - JUROS
// ======================================================

static void ExecutarJuros()
{
    Console.Clear();

    Console.WriteLine("========================================");
    Console.WriteLine("          QUESTÃO 3 - JUROS");
    Console.WriteLine("========================================");
    Console.WriteLine();

    const decimal taxaDiaria = 0.025m;

    Console.WriteLine(
        "Taxa de juros: 2,5% ao dia"
    );

    Console.WriteLine();

    // ------------------------------------------
    // Valor
    // ------------------------------------------

    Console.Write(
        "Digite o valor da dívida: R$ "
    );

    string? valorTexto = Console.ReadLine();

    if (!decimal.TryParse(
        valorTexto,
        out decimal valor))
    {
        Console.WriteLine();
        Console.WriteLine(
            "Valor inválido."
        );

        return;
    }

    if (valor <= 0)
    {
        Console.WriteLine();
        Console.WriteLine(
            "O valor deve ser maior que zero."
        );

        return;
    }

    // ------------------------------------------
    // Data de vencimento
    // ------------------------------------------

    Console.WriteLine();

    Console.Write(
        "Digite a data de vencimento " +
        "(dd/MM/yyyy): "
    );

    string? dataTexto = Console.ReadLine();

    if (!DateTime.TryParseExact(
        dataTexto,
        "dd/MM/yyyy",
        System.Globalization.CultureInfo
            .InvariantCulture,
        System.Globalization.DateTimeStyles.None,
        out DateTime dataVencimento))
    {
        Console.WriteLine();
        Console.WriteLine(
            "Data de vencimento inválida."
        );

        return;
    }

    DateTime hoje = DateTime.Today;

    // ------------------------------------------
    // Verificar atraso
    // ------------------------------------------

    if (dataVencimento >= hoje)
    {
        Console.WriteLine();
        Console.WriteLine(
            "O pagamento ainda não está atrasado."
        );

        Console.WriteLine(
            $"Data de vencimento: " +
            $"{dataVencimento:dd/MM/yyyy}"
        );

        Console.WriteLine(
            $"Data atual: {hoje:dd/MM/yyyy}"
        );

        Console.WriteLine(
            $"Juros: R$ 0,00"
        );

        Console.WriteLine(
            $"Total: R$ {valor:N2}"
        );

        return;
    }

    // ------------------------------------------
    // Calcular dias em atraso
    // ------------------------------------------

    int diasAtraso =
        (hoje - dataVencimento).Days;

    // ------------------------------------------
    // Calcular juros
    // ------------------------------------------

    decimal juros =
        valor * taxaDiaria * diasAtraso;

    decimal total =
        valor + juros;

    // ------------------------------------------
    // Resultado
    // ------------------------------------------

    Console.WriteLine();
    Console.WriteLine("========================================");
    Console.WriteLine("          RESULTADO DOS JUROS");
    Console.WriteLine("========================================");
    Console.WriteLine();

    Console.WriteLine(
        $"Valor original: R$ {valor:N2}"
    );

    Console.WriteLine(
        $"Data de vencimento: " +
        $"{dataVencimento:dd/MM/yyyy}"
    );

    Console.WriteLine(
        $"Data atual: {hoje:dd/MM/yyyy}"
    );

    Console.WriteLine(
        $"Dias em atraso: {diasAtraso}"
    );

    Console.WriteLine(
        $"Taxa diária: 2,5%"
    );

    Console.WriteLine(
        $"Juros: R$ {juros:N2}"
    );

    Console.WriteLine();

    Console.WriteLine(
        $"VALOR TOTAL: R$ {total:N2}"
    );
}