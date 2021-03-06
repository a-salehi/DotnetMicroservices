#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8006

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebApps/AspnetBasics/AspnetBasics.csproj", "WebApps/AspnetBasics/"]
COPY ["BuildingBlocks/Common.Logging/Common.Logging.csproj", "BuildingBlocks/Common.Logging/"]
RUN dotnet restore "WebApps/AspnetBasics/AspnetBasics.csproj"
COPY . .
WORKDIR "/src/WebApps/AspnetBasics"
RUN dotnet build "AspnetBasics.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AspnetBasics.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS http://*:8006
ENTRYPOINT ["dotnet", "AspnetBasics.dll"]
