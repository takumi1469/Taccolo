#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Taccolo/nuget.config", "Taccolo/"]
COPY ["LibreTranslate.TakumiCustom.Net/LibreTranslate.TakumiCustom.Net/nuget.config", "LibreTranslate.TakumiCustom.Net/LibreTranslate.TakumiCustom.Net/"]
COPY ["Taccolo/Taccolo.csproj", "Taccolo/"]
COPY ["LibreTranslate.TakumiCustom.Net/LibreTranslate.TakumiCustom.Net/LibreTranslate.TakumiCustom.Net.csproj", "LibreTranslate.TakumiCustom.Net/LibreTranslate.TakumiCustom.Net/"]
RUN dotnet restore "./Taccolo/Taccolo.csproj"
COPY . .
WORKDIR "/src/Taccolo"
RUN dotnet build "./Taccolo.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Taccolo.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Docker
ENTRYPOINT ["dotnet", "Taccolo.dll"]