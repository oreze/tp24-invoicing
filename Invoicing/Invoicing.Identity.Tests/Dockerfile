FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY ["./Invoicing.Identity.Tests/Invoicing.Identity.Tests.csproj", "Invoicing.Identity.Tests/"]
RUN dotnet restore "Invoicing.Identity.Tests/Invoicing.Identity.Tests.csproj"
WORKDIR /src
COPY . .
RUN dotnet build "./Invoicing.Identity.Tests/Invoicing.Identity.Tests.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Invoicing.Identity.Tests/Invoicing.Identity.Tests.csproj" -c Release -o /app/publish

FROM publish AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "test", "Invoicing.Identity.Tests.dll", "--logger:trx"]
