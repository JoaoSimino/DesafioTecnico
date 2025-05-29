# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia csproj e restaura dependências
COPY *.csproj ./
RUN dotnet restore

# Copia todo o código
COPY . ./

# Build em modo Release
RUN dotnet publish -c Release -o out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copia app buildado do stage anterior
COPY --from=build /app/out ./

# Exponha a porta que sua API usa
EXPOSE 80

# Comando de entrada
ENTRYPOINT ["dotnet", "DesafioTecnico1.dll"]