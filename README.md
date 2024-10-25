# capital-gains

## Contexto

O objetivo deste exerc�cio � implementar um programa de linha de comando (CLI) que calcula o imposto a ser pago sobre lucros ou preju�zos de opera��es no mercado financeiro de a��es.

## Decis�es t�cnicas e arquiteturais

### Input

Na documenta��o do desafio consta que a aplica��o deveria ser de linha de comando e ser capaz de receber N linhas de simula��es (considere simula��o como uma sequ�ncia de opera��es). Contudo, esse tipo de aplica��o s� l� um input no console por vez. Portanto, optei por assumir que todas as poss�veis entradas seriam um JSON inline, mais especificamente um array. Ou seja, caso a entrada seja uma simula��o �nica o esperado � receber uma entrada parecida com esta:

```[{"operation":"buy", "unit-cost":10.00, "quantity": 100}, {"operation":"sell", "unit-cost":15.00, "quantity": 50}, {"operation":"sell", "unit-cost":15.00, "quantity": 50}]```

Para o caso de m�ltiplas simula��es, o esperado � receber uma entrada parecida com esta:

```[[{"operation":"buy", "unit-cost":10.00, "quantity": 100}, {"operation":"sell", "unit-cost":15.00, "quantity": 50}, {"operation":"sell", "unit-cost":15.00, "quantity": 50}],[{"operation":"buy", "unit-cost":10.00, "quantity": 10000}, {"operation":"sell", "unit-cost":20.00, "quantity": 5000}, {"operation":"sell", "unit-cost":5.00, "quantity": 5000}]]```

Note que � um array de arrays, onde cada array interno representa uma simula��o.

### Arquitetura

Como a proposta era de um projeto de linha de comando, procurei manter a solu��o o mais simples poss�vel, bem objetiva e leg�vel.

Al�m disso, segui o princ�pio da responsabilidade �nica, em que cada componente tem sua responsabilidade bem definida. Por exemplo, a classe Program � respons�vel receber o input, manipular os dados em JSON e exibir o output; a classe CapitalGains � respons�vel por calcular e retornar o imposto a ser pago em cada opera��o, levando em considera��o todas as regras previstas; j� a classe Operation � respons�vel por representar o objeto de uma opera��o, permitindo que o sistema interprete os dados do input para realizar as devidas manipula��es.

Essa divis�o de responsabilidades foi pensada para facilitar a escrita dos testes, manuten��o e extens�o do c�digo.

## Frameworks e bibliotecas

- ```C#``` como linguagem de programa��o
- ```.NET 8``` como framework de desenvolvimento
- ```xUnit``` como framework de testes
- ```Newtonsoft``` como biblioteca para manipula��o de JSON

## Instru��es sobre como compilar e executar o projeto

Caso tenha o ambiente configurado para .NET, basta navegar at� a pasta ./capital-gains e executar os comandos, respectivamente:

- ```dotnet restore```
- ```dotnet build```
- ```dotnet run```

E, para rodar os testes:

- ```dotnet test```

Caso contr�rio, � poss�vel rodar o projeto utilizando Docker. Para isso, tamb�m na pasta ./capital-gains, basta executar os comandos abaixo:

- ```docker build -t capital-gains-app .```
- ```docker run --rm -it --name capital-gains-app capital-gains-app```

Para executar os testes, navegue at� a raiz do projeto e execute os comandos abaixo:

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
