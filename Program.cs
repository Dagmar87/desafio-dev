using System.Text.Json;
using DesafioDev.Models;
using DesafioDev.Services;

while (true)
{
    Console.Clear();

    Console.WriteLine("========================================");
    Console.WriteLine("          DESAFIO DEV - C#");
    Console.WriteLine("========================================");
    Console.WriteLine();
    Console.WriteLine("1 - Calcular comissões");
    Console.WriteLine("2 - Movimentar estoque");
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

            case "0":
                Console.WriteLine("Encerrando programa...");
                return;

            default:
                Console.WriteLine("Opção inválida.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine($"ERRO: {ex.Message}");
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
    Console.WriteLine("          QUESTÃO 1 - COMISSÕES");
    Console.WriteLine("========================================");
    Console.WriteLine();

    string caminhoArquivo = Path.Combine(
        AppContext.BaseDirectory,
        "Data",
        "vendas.json"
    );

    Console.WriteLine($"Arquivo: {caminhoArquivo}");

    if (!File.Exists(caminhoArquivo))
    {
        Console.WriteLine("Arquivo vendas.json não encontrado.");
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

    var service = new ComissaoService();

    var resultados =
        service.CalcularComissaoPorVendedor(
            arquivo.Vendas
        );

    Console.WriteLine();
    Console.WriteLine("===== COMISSÕES POR VENDEDOR =====");
    Console.WriteLine();

    foreach (var resultado in resultados)
    {
        Console.WriteLine(
            $"{resultado.Key}: R$ {resultado.Value:N2}"
        );
    }
}


// ======================================================
// QUESTÃO 2 - ESTOQUE
// ======================================================

static void ExecutarEstoque()
{
    Console.Clear();

    Console.WriteLine("========================================");
    Console.WriteLine("          QUESTÃO 2 - ESTOQUE");
    Console.WriteLine("========================================");
    Console.WriteLine();

    string caminhoArquivo = Path.Combine(
        AppContext.BaseDirectory,
        "Data",
        "estoque.json"
    );

    Console.WriteLine($"Arquivo: {caminhoArquivo}");

    if (!File.Exists(caminhoArquivo))
    {
        Console.WriteLine("Arquivo estoque.json não encontrado.");
        return;
    }

    string json = File.ReadAllText(caminhoArquivo);

    using JsonDocument documento =
        JsonDocument.Parse(json);

    JsonElement estoqueJson =
        documento.RootElement.GetProperty("estoque");

    List<Produto> produtos =
        JsonSerializer.Deserialize<List<Produto>>(
            estoqueJson.GetRawText()
        ) ?? new List<Produto>();

    if (produtos.Count == 0)
    {
        Console.WriteLine(
            "Nenhum produto foi encontrado no estoque."
        );

        return;
    }

    var estoqueService = new EstoqueService(produtos);

    Console.WriteLine("Produtos disponíveis:");
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
        Console.WriteLine(
            "Código do produto inválido."
        );

        return;
    }

    Produto? produtoSelecionado =
        estoqueService.BuscarProduto(codigoProduto);

    if (produtoSelecionado is null)
    {
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

    Console.WriteLine("Tipo de movimentação:");
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
        Console.WriteLine(
            "Quantidade inválida."
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
        estoqueService.BuscarProduto(codigoProduto)!;

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
        $"Quantidade movimentada: {movimentacao.Quantidade}"
    );

    Console.WriteLine(
        $"Data: {movimentacao.Data:dd/MM/yyyy HH:mm:ss}"
    );

    Console.WriteLine();
    Console.WriteLine(
        $"ESTOQUE FINAL: {produtoAtualizado.Estoque}"
    );
}