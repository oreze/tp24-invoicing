FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Invoicing.Identity.API.csproj", "Invoicing.Identity.API/"]
RUN dotnet restore "Invoicing.Identity.API/Invoicing.Identity.API.csproj"
WORKDIR "/src/Invoicing.Identity.API"
COPY . .
RUN dotnet build "Invoicing.Identity.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Invoicing.Identity.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Invoicing.Identity.API.dll"]
