# Debugging Stage:
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Base Stage for Deployables:
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EvaluationApi.csproj", "."]
RUN dotnet restore "./EvaluationApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./EvaluationApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# For Artefacts / Publishing:
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EvaluationApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# For Deployment into Environments (Production):
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MiddlewareNz.EvaluationApi.dll"]
