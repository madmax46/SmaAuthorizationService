FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build

WORKDIR /src
COPY ["CommonProjects/CommonProjects/AuthCommonLib/AuthCommonLib.csproj", "CommonProjects/AuthCommonLib"]

WORKDIR /src
COPY ["SmaAuthorizationService/SmaAuthorizationService.csproj", "SmaAuthorizationService/"]
RUN dotnet restore "SmaAuthorizationService/SmaAuthorizationService.csproj"
COPY . .
WORKDIR "/src/SmaAuthorizationService"
RUN dotnet build "SmaAuthorizationService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmaAuthorizationService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmaAuthorizationService.dll"]