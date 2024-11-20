# Use a imagem base do SDK do .NET 8.0 para compilar o projeto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copie os arquivos do projeto para o contêiner
COPY . .

# Restaure as dependências
RUN dotnet restore HumbertoMVC.csproj

# Compile o projeto
RUN dotnet publish HumbertoMVC.csproj -c Release -o out

# Use a imagem base do runtime do .NET 8.0 para rodar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copie os artefatos da compilação para o contêiner
COPY --from=build /app/out .

# Exponha a porta que o aplicativo usa
EXPOSE 80

# Defina o comando para iniciar o aplicativo
ENTRYPOINT ["dotnet", "HumbertoMVC.dll"]
