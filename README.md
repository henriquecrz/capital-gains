# capital-gains

## Decisões técnicas e arquiteturais

- Input


## Frameworks e bibliotecas

- C# como linguagem de programação
- .NET 8 como framework de desenvolvimento
- xUnit como framework de testes
- Newtonsoft como biblioteca para manipulação de JSON

## Instruções sobre como compilar e executar o projeto

Caso tenha o ambiente configurado para .NET, basta navegar até a pasta ./capital-gains e executar os comandos, respectivamente:

- ```dotnet restore```
- ```dotnet build```
- ```dotnet run```

E, para rodar os testes:

- ```dotnet test```

Caso contrário, é possível rodar o projeto utilizando Docker. Para isso, basta executar os comandos abaixo:

- ```docker build -t capital-gains-app .```
- ```docker run --rm -it --name capital-gains-app capital-gains-app```

Para executar os testes, navegue até a raiz do projeto e execute os comandos abaixo:

- ```docker build -t capital-gains-tests .```
- ```docker run --rm --name capital-gains-tests capital-gains-tests```

## Notas adicionais que você considere importantes para a avaliação.

