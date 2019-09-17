FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["User.API/User.API.csproj", "User.API/"]
RUN dotnet restore "User.API/User.API.csproj"
COPY . .
WORKDIR "/src/User.API"
RUN dotnet build "User.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "User.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "User.API.dll"]