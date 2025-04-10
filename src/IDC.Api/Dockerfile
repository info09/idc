FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["IDC.Api/IDC.Api.csproj", "IDC.Api/"]
COPY ["IDC.Domain/IDC.Domain.csproj", "src/IDC.Domain/"]
COPY ["IDC.Infrastructure/IDC.Infrastructure.csproj", "IDC.Infrastructure/"]
COPY ["IDC.Shared/IDC.Shared.csproj", "IDC.Shared/"]
COPY ["IDC.Service/IDC.Application.csproj", "IDC.Service/"]

RUN dotnet restore "./src/IDC.Api/IDC.Api.csproj"
COPY . .
WORKDIR "/src/src/IDC.Api"
RUN dotnet build "./IDC.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./IDC.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IDC.Api.dll"]