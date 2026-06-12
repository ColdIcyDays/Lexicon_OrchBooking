FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5173

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Lexicon_OrchBookingBackend/Lexicon_OrchBookingBackend.csproj", "Lexicon_OrchBookingBackend/"]
RUN dotnet restore "Lexicon_OrchBookingBackend/Lexicon_OrchBookingBackend.csproj"
COPY . .
WORKDIR "/src/Lexicon_OrchBookingBackend"
RUN dotnet build "./Lexicon_OrchBookingBackend.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Lexicon_OrchBookingBackend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lexicon_OrchBookingBackend.dll"]
