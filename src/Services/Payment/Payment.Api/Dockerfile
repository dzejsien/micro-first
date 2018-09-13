FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY src/Services/Payment/Payment.Api/Payment.Api.csproj src/Services/Payment/Payment.Api/
COPY src/Core/Messages/Messages.csproj src/Core/Messages/
COPY src/Core/Common/Common.csproj src/Core/Common/
COPY src/Core/EventBus/EventBus.csproj src/Core/EventBus/
COPY src/Services/Payment/Payment.DataAccess.MongoDb/Payment.DataAccess.MongoDb.csproj src/Services/Payment/Payment.DataAccess.MongoDb/
COPY src/Services/Payment/Payment.Domain/Payment.Domain.csproj src/Services/Payment/Payment.Domain/
RUN dotnet restore src/Services/Payment/Payment.Api/Payment.Api.csproj
COPY . .
WORKDIR /src/src/Services/Payment/Payment.Api
RUN dotnet build Payment.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Payment.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Payment.Api.dll"]
