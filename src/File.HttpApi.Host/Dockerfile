#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src
COPY ["src/File.HttpApi.Host/File.HttpApi.Host.csproj", "src/File.HttpApi.Host/"]
COPY ["src/File.Application/File.Application.csproj", "src/File.Application/"]
COPY ["src/File.Application.Contract/File.Application.Contract.csproj", "src/File.Application.Contract/"]
COPY ["src/File.Shared/File.Shared.csproj", "src/File.Shared/"]
COPY ["src/File.Entity/File.Entity.csproj", "src/File.Entity/"]
RUN dotnet restore "src/File.HttpApi.Host/File.HttpApi.Host.csproj"
COPY . .
WORKDIR "/src/src/File.HttpApi.Host"
RUN dotnet build "File.HttpApi.Host.csproj" -c Release --os linux -o /app/build

FROM build AS publish
RUN dotnet publish "File.HttpApi.Host.csproj" -c Release --os linux -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "File.HttpApi.Host.dll"]