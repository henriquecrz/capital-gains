# Usar a imagem oficial do SDK do .NET
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Definir o diretório de trabalho dentro do contêiner
WORKDIR /app

# Copiar os arquivos de solução e de projeto para o contêiner
COPY . .

# Restaurar as dependências do projeto de testes
RUN dotnet restore ./unit-tests/unit-tests.csproj

# Compilar o projeto de testes
RUN dotnet build ./unit-tests/unit-tests.csproj --configuration Debug --no-restore

# Rodar os testes
CMD ["dotnet", "test", "./unit-tests/unit-tests.csproj", "--no-build", "--configuration", "Debug", "--verbosity", "normal"]
