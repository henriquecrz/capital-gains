# Usar a imagem base do SDK do .NET 8
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Definir o diretório de trabalho dentro do contêiner
WORKDIR /app

# Copiar o código do projeto
COPY . .

# Restaurar as dependências e compilar a aplicação
RUN dotnet restore
RUN dotnet build -c Release --no-restore

# Publicar a aplicação
RUN dotnet publish -c Release -o /app/publish --no-build

# Usando o SDK para poder rodar a aplicação com interação de console
FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Definindo o comando de entrada
ENTRYPOINT ["dotnet", "capital-gains.dll"]
