FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Web/GraphQL/GraphQL.AspNet/GraphQL.AspNet.csproj", "Web/GraphQL/GraphQL.AspNet/"]
RUN dotnet restore "Web/GraphQL/GraphQL.AspNet/GraphQL.AspNet.csproj"
COPY . .
WORKDIR "/src/Web/GraphQL/GraphQL.AspNet"
RUN dotnet build "GraphQL.AspNet.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "GraphQL.AspNet.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GraphQL.AspNet.dll"]
