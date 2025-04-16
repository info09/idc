FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["IDC.Api/IDC.Api.csproj", "IDC.Api/"]
COPY ["IDC.Domain/IDC.Domain.csproj", "src/IDC.Domain/"]
COPY ["IDC.Infrastructure/IDC.Infrastructure.csproj", "IDC.Infrastructure/"]
COPY ["IDC.Shared/IDC.Shared.csproj", "IDC.Shared/"]
COPY ["IDC.Application/IDC.Application.csproj", "IDC.Application/"]

RUN dotnet restore "IDC.Api/IDC.Api.csproj"
COPY . .
WORKDIR "/src/IDC.Api"
RUN dotnet build "IDC.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IDC.Api.csproj" -c Release -o /app/publish

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IDC.Api.dll"]