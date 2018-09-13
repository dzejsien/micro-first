FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 5010
EXPOSE 5011

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY src/Services/Policy/Policy.Api/Policy.Api.csproj src/Services/Policy/Policy.Api/
COPY src/Services/Policy/Policy.DataAccess.SqlServer/Policy.DataAccess.SqlServer.csproj src/Services/Policy/Policy.DataAccess.SqlServer/
COPY src/Services/Policy/Policy.Domain/Policy.Domain.csproj src/Services/Policy/Policy.Domain/
COPY src/Core/Messages/Messages.csproj src/Core/Messages/
COPY src/Core/Common/Common.csproj src/Core/Common/
COPY src/Core/EventBus/EventBus.csproj src/Core/EventBus/
RUN dotnet restore src/Services/Policy/Policy.Api/Policy.Api.csproj
COPY . .
WORKDIR /src/src/Services/Policy/Policy.Api
RUN dotnet build Policy.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Policy.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Policy.Api.dll"]
