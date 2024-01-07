#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BankProject.API/BankProject.API.csproj", "BankProject.API/"]
COPY ["BankProject.Business/BankProject.Business.csproj", "BankProject.Business/"]
COPY ["BankProject.Data/BankProject.Data.csproj", "BankProject.Data/"]
COPY ["BankProject.Core/BankProject.Core.csproj", "BankProject.Core/"]
RUN dotnet restore "./BankProject.API/./BankProject.API.csproj"
COPY . .
WORKDIR "/src/BankProject.API"
RUN dotnet build "./BankProject.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BankProject.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BankProject.API.dll"]