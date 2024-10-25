# Usar a imagem oficial do SDK do .NET
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Definir o diret�rio de trabalho dentro do cont�iner
WORKDIR /app

# Copiar os arquivos de solu��o e de projeto para o cont�iner
COPY . .

# Restaurar as depend�ncias do projeto de testes
RUN dotnet restore ./tests/unit-tests.csproj

# Compilar o projeto de testes
RUN dotnet build ./tests/unit-tests.csproj --configuration Debug --no-restore

# Rodar os testes
CMD ["dotnet", "test", "./tests/unit-tests.csproj", "--no-build", "--configuration", "Debug", "--verbosity", "normal"]
