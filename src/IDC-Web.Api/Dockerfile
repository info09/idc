FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["IDC-Web.Api/IDC-Web.Api.csproj", "IDC-Web.Api/"]
COPY ["IDC-Web.Domain/IDC-Web.Domain.csproj", "IDC-Web.Domain/"]
COPY ["IDC-Web.Infrastructure/IDC-Web.Infrastructure.csproj", "IDC-Web.Infrastructure/"]
COPY ["IDC-Web.Shared/IDC-Web.Shared.csproj", "IDC-Web.Shared/"]
COPY ["IDC-Web.Application/IDC-Web.Application.csproj", "IDC-Web.Application/"]

RUN dotnet restore "IDC-Web.Api/IDC-Web.Api.csproj"
COPY . .
WORKDIR "/src/IDC-Web.Api"
RUN dotnet build "IDC-Web.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IDC-Web.Api.csproj" -c Release -o /app/publish

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IDC-Web.Api.dll"]