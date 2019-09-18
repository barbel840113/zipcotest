FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Account.API/Account.API.csproj", "Account.API/"]
RUN dotnet restore "Account.API/Account.API.csproj"
COPY . .
WORKDIR "/src/Account.API"
RUN dotnet build "Account.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Account.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Account.API.dll"]
