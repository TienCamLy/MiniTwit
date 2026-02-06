FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
RUN mkdir -p /app/keys && chown -R $APP_UID:$APP_UID /app/keys && chmod 700 /app/keys

USER $APP_UID
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["razor-pages/razor-pages.csproj", "razor-pages/"]
RUN dotnet restore "razor-pages/razor-pages.csproj"
COPY . .
WORKDIR "/src/razor-pages"
RUN dotnet build "./razor-pages.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./razor-pages.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER root
RUN chown $APP_UID:$APP_UID /app
USER $APP_UID
ENTRYPOINT ["dotnet", "razor-pages.dll"]
