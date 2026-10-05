# desafio-dev

Aplicação console em C# para resolver 3 desafios de negócio em um único menu interativo:

- cálculo de comissão por vendedor a partir de um arquivo JSON
- movimentação de estoque com entrada e saída de produtos
- cálculo de juros por atraso de pagamento

## Visão geral

O projeto foi desenvolvido como um exercício prático de programação em .NET, com foco em leitura de dados JSON, processamento de regras de negócio e interação via console.

Ao executar a aplicação, o usuário pode escolher uma das opções do menu:

1. Calcular comissões
2. Movimentar estoque
3. Calcular juros
0. Sair

## Requisitos

- .NET SDK 10.0
- Sistema operacional compatível com .NET

## Estrutura do projeto

- `Program.cs` — menu principal e fluxo das operações
- `Models/` — modelos das entidades (`Produto`, `Venda`, `MovimentacaoEstoque`, etc.)
- `Services/` — regras de negócio para comissão, estoque e juros
- `Data/` — arquivos JSON de entrada (`vendas.json` e `estoque.json`)
- `desafio-dev.csproj` — configuração do projeto

## Como executar

No diretório raiz do projeto, execute:

```bash
dotnet restore
dotnet run
```

## Funcionalidades

### 1) Cálculo de comissões

A opção 1 lê o arquivo `Data/vendas.json` e agrupa os valores por vendedor.

Regra aplicada:

- vendas abaixo de R$ 100,00: comissão = 0
- vendas entre R$ 100,00 e R$ 499,99: comissão = 1%
- vendas a partir de R$ 500,00: comissão = 5%

O resultado é exibido por vendedor com o total calculado.

### 2) Movimentação de estoque

A opção 2 lê o arquivo `Data/estoque.json` e permite:

- listar os produtos disponíveis
- selecionar um código de produto
- informar o tipo da movimentação: entrada ou saída
- informar a descrição da operação
- informar a quantidade movimentada

A aplicação valida:

- produto existente
- quantidade maior que zero
- estoque suficiente para saída

### 3) Cálculo de juros por atraso

A opção 3 calcula juros diários sobre o valor da dívida, com base na data de vencimento e na data atual.

Regra aplicada:

- taxa diária: 2,5%
- se a data atual for anterior ou igual à data de vencimento, não há juros
- caso haja atraso, o valor total é calculado como: valor original + juros

## Estruturas dos arquivos JSON

### `Data/vendas.json`

```json
{
  "vendas": [
    {
      "vendedor": "João Silva",
      "valor": 1200.5
    }
  ]
}
```

### `Data/estoque.json`

```json
{
  "estoque": [
    {
      "codigoProduto": 101,
      "descricaoProduto": "Caneta Azul",
      "estoque": 150
    }
  ]
}
```

## Observações

- Os arquivos JSON em `Data/` são copiados para a pasta de saída pela configuração do projeto.
- A aplicação é executada em modo console e permanece em loop até que o usuário escolha sair.
