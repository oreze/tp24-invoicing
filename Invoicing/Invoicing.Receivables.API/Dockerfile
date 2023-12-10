FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY ["./Invoicing.Receivables.API/Invoicing.Receivables.API.csproj", "Invoicing.Receivables.API/"]
COPY ["./Invoicing.Receivables.Domain/Invoicing.Receivables.Domain.csproj", "Invoicing.Receivables.Domain/"]
COPY ["./Invoicing.Receivables.Infrastructure/Invoicing.Receivables.Infrastructure.csproj", "Invoicing.Receivables.Infrastructure/"]
COPY ["./Invoicing.Receivables.Application/Invoicing.Receivables.Application.csproj", "Invoicing.Receivables.Application/"]
COPY ["./Invoicing.Receivables.ApplicationContracts/Invoicing.Receivables.ApplicationContracts.csproj", "Invoicing.Receivables.ApplicationContracts/"]

RUN dotnet restore "Invoicing.Receivables.API/Invoicing.Receivables.API.csproj"
WORKDIR /src
COPY . .
RUN dotnet build "./Invoicing.Receivables.API/Invoicing.Receivables.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Invoicing.Receivables.API/Invoicing.Receivables.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Invoicing.Receivables.API.dll"]
