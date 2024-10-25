# capital-gains

## Contexto

O objetivo deste exercício é implementar um programa de linha de comando (CLI) que calcula o imposto a ser pago sobre lucros ou prejuízos de operações no mercado financeiro de ações.

## Decisões técnicas e arquiteturais

### Input

Na documentação do desafio consta que a aplicação deveria ser de linha de comando e ser capaz de receber N linhas de simulações (considere simulação como uma sequência de operações). Contudo, esse tipo de aplicação só lê um input no console por vez. Portanto, optei por assumir que todas as possíveis entradas seriam um JSON inline, mais especificamente um array. Ou seja, caso a entrada seja uma simulação única o esperado é receber uma entrada parecida com esta:

```[{"operation":"buy", "unit-cost":10.00, "quantity": 100}, {"operation":"sell", "unit-cost":15.00, "quantity": 50}, {"operation":"sell", "unit-cost":15.00, "quantity": 50}]```

Para o caso de múltiplas simulações, o esperado é receber uma entrada parecida com esta:

```[[{"operation":"buy", "unit-cost":10.00, "quantity": 100}, {"operation":"sell", "unit-cost":15.00, "quantity": 50}, {"operation":"sell", "unit-cost":15.00, "quantity": 50}],[{"operation":"buy", "unit-cost":10.00, "quantity": 10000}, {"operation":"sell", "unit-cost":20.00, "quantity": 5000}, {"operation":"sell", "unit-cost":5.00, "quantity": 5000}]]```

Note que é um array de arrays, onde cada array interno representa uma simulação.

### Arquitetura

Como a proposta era de um projeto de linha de comando, procurei manter a solução o mais simples possível, bem objetiva e legível.

Além disso, segui o princípio da responsabilidade única, em que cada componente tem sua responsabilidade bem definida. Por exemplo, a classe Program é responsável receber o input, manipular os dados em JSON e exibir o output; a classe CapitalGains é responsável por calcular e retornar o imposto a ser pago em cada operação, levando em consideração todas as regras previstas; já a classe Operation é responsável por representar o objeto de uma operação, permitindo que o sistema interprete os dados do input para realizar as devidas manipulações.

Essa divisão de responsabilidades foi pensada para facilitar a escrita dos testes, manutenção e extensão do código.

## Frameworks e bibliotecas

- ```C#``` como linguagem de programação
- ```.NET 8``` como framework de desenvolvimento
- ```xUnit``` como framework de testes
- ```Newtonsoft``` como biblioteca para manipulação de JSON

## Instruções sobre como compilar e executar o projeto

Caso tenha o ambiente configurado para .NET, basta navegar até a pasta ./capital-gains e executar os comandos, respectivamente:

- ```dotnet restore```
- ```dotnet build```
- ```dotnet run```

E, para rodar os testes:

- ```dotnet test```

Caso contrário, é possível rodar o projeto utilizando Docker. Para isso, também na pasta ./capital-gains, basta executar os comandos abaixo:

- ```docker build -t capital-gains-app .```
- ```docker run --rm -it --name capital-gains-app capital-gains-app```

Para executar os testes, navegue até a raiz do projeto e execute os comandos abaixo:

- ```docker build -t capital-gains-tests .```
- ```docker run --rm --name capital-gains-tests capital-gains-tests```

## Cases de teste

### Case 1

#### Input

```[{"operation":"buy", "unit-cost":10.00, "quantity": 100}, {"operation":"sell", "unit-cost":15.00, "quantity": 50}, {"operation":"sell", "unit-cost":15.00, "quantity": 50}]```

#### Output

```
[{"tax": 0.00},{"tax": 0.00},{"tax": 0.00}]
```

### Case 2

#### Input

```[{"operation":"buy", "unit-cost":10.00, "quantity": 10000}, {"operation":"sell", "unit-cost":20.00, "quantity": 5000}, {"operation":"sell", "unit-cost":5.00, "quantity": 5000}]```

#### Output

```
[{"tax": 0.00},{"tax": 10000.00},{"tax": 0.00}]
```

### Case 1 + Case 2

#### Input

```[[{"operation":"buy", "unit-cost":10.00, "quantity": 100}, {"operation":"sell", "unit-cost":15.00, "quantity": 50}, {"operation":"sell", "unit-cost":15.00, "quantity": 50}],[{"operation":"buy", "unit-cost":10.00, "quantity": 10000}, {"operation":"sell", "unit-cost":20.00, "quantity": 5000}, {"operation":"sell", "unit-cost":5.00, "quantity": 5000}]]```

#### Output

```
[{"tax": 0.00},{"tax": 0.00},{"tax": 0.00}]
[{"tax": 0.00},{"tax": 10000.00},{"tax": 0.00}]
```

### Case 3

#### Input

```[{"operation":"buy", "unit-cost":10.00, "quantity": 10000}, {"operation":"sell", "unit-cost":5.00, "quantity": 5000}, {"operation":"sell", "unit-cost":20.00, "quantity": 3000}]```

#### Output

```
[{"tax": 0.00},{"tax": 0.00},{"tax": 1000.00}]
```

### Case 4

#### Input

```[{"operation":"buy", "unit-cost":10.00, "quantity": 10000}, {"operation":"buy", "unit-cost":25.00, "quantity": 5000}, {"operation":"sell", "unit-cost":15.00, "quantity": 10000}]```

#### Output

```
[{"tax": 0.00},{"tax": 0.00},{"tax": 0.00}]
```

### Case 5

#### Input

```[{"operation":"buy", "unit-cost":10.00, "quantity": 10000}, {"operation":"buy", "unit-cost":25.00, "quantity": 5000}, {"operation":"sell", "unit-cost":15.00, "quantity": 10000}, {"operation":"sell", "unit-cost":25.00, "quantity": 5000}]```

#### Output

```
[{"tax": 0.00},{"tax": 0.00},{"tax": 0.00},{"tax": 10000.00}]
```

### Case 6

#### Input

```[{"operation":"buy", "unit-cost":10.00, "quantity": 10000}, {"operation":"sell", "unit-cost":2.00, "quantity": 5000}, {"operation":"sell", "unit-cost":20.00, "quantity": 2000}, {"operation":"sell", "unit-cost":20.00, "quantity": 2000}, {"operation":"sell", "unit-cost":25.00, "quantity": 1000}]```

#### Output

```
[{"tax": 0.00},{"tax": 0.00},{"tax": 0.00},{"tax": 0.00},{"tax": 3000.00}]
```

### Case 7

#### Input

```[{"operation":"buy", "unit-cost":10.00, "quantity": 10000}, {"operation":"sell", "unit-cost":2.00, "quantity": 5000}, {"operation":"sell", "unit-cost":20.00, "quantity": 2000}, {"operation":"sell", "unit-cost":20.00, "quantity": 2000}, {"operation":"sell", "unit-cost":25.00, "quantity": 1000}, {"operation":"buy", "unit-cost":20.00, "quantity": 10000}, {"operation":"sell", "unit-cost":15.00, "quantity": 5000}, {"operation":"sell", "unit-cost":30.00, "quantity": 4350}, {"operation":"sell", "unit-cost":30.00, "quantity": 650}]```

#### Output

```
[{"tax":0.00}, {"tax":0.00}, {"tax":0.00}, {"tax":0.00}, {"tax":3000.00}, {"tax":0.00}, {"tax":0.00}, {"tax":3700.00}, {"tax":0.00}]
```

### Case 8

#### Input

```[{"operation":"buy", "unit-cost":10.00, "quantity": 10000}, {"operation":"sell", "unit-cost":50.00, "quantity": 10000}, {"operation":"buy", "unit-cost":20.00, "quantity": 10000}, {"operation":"sell", "unit-cost":50.00, "quantity": 10000}]```

#### Output

```
[{"tax":0.00},{"tax":80000.00},{"tax":0.00},{"tax":60000.00}]
```
