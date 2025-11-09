FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY NoActivityFiler/NoActivityFiler.csproj NoActivityFiler/
COPY NoActivityFiler.Tests/NoActivityFiler.Tests.csproj NoActivityFiler.Tests/
RUN dotnet restore NoActivityFiler/NoActivityFiler.csproj

COPY . .
RUN dotnet publish NoActivityFiler/NoActivityFiler.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "NoActivityFiler.dll"]

