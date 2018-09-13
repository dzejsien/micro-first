FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 5020
EXPOSE 5021

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY src/Services/Pricing/Pricing.Api/Pricing.Api.csproj src/Services/Pricing/Pricing.Api/
COPY src/Core/Messages/Messages.csproj src/Core/Messages/
COPY src/Core/Common/Common.csproj src/Core/Common/
COPY src/Core/EventBus/EventBus.csproj src/Core/EventBus/
COPY src/Services/Pricing/Pricing.DataAccess.PostgreSql/Pricing.DataAccess.PostgreSql.csproj src/Services/Pricing/Pricing.DataAccess.PostgreSql/
COPY src/Services/Pricing/Pricing.Domain/Pricing.Domain.csproj src/Services/Pricing/Pricing.Domain/
RUN dotnet restore src/Services/Pricing/Pricing.Api/Pricing.Api.csproj
COPY . .
WORKDIR /src/src/Services/Pricing/Pricing.Api
RUN dotnet build Pricing.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Pricing.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pricing.Api.dll"]
