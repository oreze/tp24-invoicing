FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY ./*.sln ./
COPY ["./Invoicing.Identity.IntegrationTests/Invoicing.Identity.IntegrationTests.csproj", "Invoicing.Identity.IntegrationTests/"]
COPY ["./Invoicing.Identity.API/Invoicing.Identity.API.csproj", "Invoicing.Identity.API/"]
COPY ["./Invoicing.Identity.Domain/Invoicing.Identity.Domain.csproj", "Invoicing.Identity.Domain/"]
COPY ["./Invoicing.Identity.Infrastructure/Invoicing.Identity.Infrastructure.csproj", "Invoicing.Identity.Infrastructure/"]

RUN dotnet restore "Invoicing.Identity.IntegrationTests/Invoicing.Identity.IntegrationTests.csproj"
COPY . ./
WORKDIR /src

COPY . ./

RUN dotnet build "./Invoicing.Identity.IntegrationTests/Invoicing.Identity.IntegrationTests.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Invoicing.Identity.IntegrationTests/Invoicing.Identity.IntegrationTests.csproj" -c Release -o /app/publish

FROM publish AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "test", "Invoicing.Identity.IntegrationTests.dll"]

