# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY *.sln ./
COPY BSC.Domain/*.csproj ./BSC.Domain/
COPY BSC.Application/*.csproj ./BSC.Application/
COPY BSC.Infrastructure/*.csproj ./BSC.Infrastructure/
COPY BSC.Utilities/*.csproj ./BSC.Utilities/
COPY BSC.Api/*.csproj ./BSC.Api/

RUN dotnet restore

# Copiar todo el código
COPY . .

# Publicar solo el proyecto API
RUN dotnet publish BSC.Api/BSC.Api.csproj -c Release -o /app/publish

# Imagen final
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5000
ENTRYPOINT ["dotnet", "BSC.Api.dll"]
