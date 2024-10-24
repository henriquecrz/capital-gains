# capital-gains

## Decis�es t�cnicas e arquiteturais

- Input


## Frameworks e bibliotecas

- C# como linguagem de programa��o
- .NET 8 como framework de desenvolvimento
- xUnit como framework de testes
- Newtonsoft como biblioteca para manipula��o de JSON

## Instru��es sobre como compilar e executar o projeto

Caso tenha o ambiente configurado para .NET, basta navegar at� a pasta ./capital-gains e executar os comandos, respectivamente:

- ```dotnet restore```
- ```dotnet build```
- ```dotnet run```

E, para rodar os testes:

- ```dotnet test```

Caso contr�rio, � poss�vel rodar o projeto utilizando Docker. Para isso, basta executar os comandos abaixo:

- ```docker build -t capital-gains-app .```
- ```docker run --rm -it --name capital-gains-app capital-gains-app```

Para executar os testes, navegue at� a raiz do projeto e execute os comandos abaixo:

- ```docker build -t capital-gains-tests .```
- ```docker run --rm --name capital-gains-tests capital-gains-tests```

## Notas adicionais que voc� considere importantes para a avalia��o.

