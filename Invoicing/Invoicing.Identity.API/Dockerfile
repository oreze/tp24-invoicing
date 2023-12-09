FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY ["./Invoicing.Identity.API/Invoicing.Identity.API.csproj", "Invoicing.Identity.API/"]
COPY ["./Invoicing.Identity.Domain/Invoicing.Identity.Domain.csproj", "Invoicing.Identity.Domain/"]
COPY ["./Invoicing.Identity.Infrastructure/Invoicing.Identity.Infrastructure.csproj", "Invoicing.Identity.Infrastructure/"]

RUN dotnet restore "Invoicing.Identity.API/Invoicing.Identity.API.csproj"
WORKDIR /src
COPY . .
RUN dotnet build "./Invoicing.Identity.API/Invoicing.Identity.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Invoicing.Identity.API/Invoicing.Identity.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Invoicing.Identity.API.dll"]
