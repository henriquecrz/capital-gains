# Usar a imagem base do SDK do .NET 8
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Definir o diret�rio de trabalho dentro do cont�iner
WORKDIR /app

# Copiar o c�digo do projeto
COPY . .

# Restaurar as depend�ncias e compilar a aplica��o
RUN dotnet restore
RUN dotnet build -c Release --no-restore

# Publicar a aplica��o
RUN dotnet publish -c Release -o /app/publish --no-build

# Usando o SDK para poder rodar a aplica��o com intera��o de console
FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Definindo o comando de entrada
ENTRYPOINT ["dotnet", "capital-gains.dll"]
