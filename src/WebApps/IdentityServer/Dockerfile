#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 7000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebApps/IdentityServer/IdentityServer.csproj", "WebApps/IdentityServer/"]
RUN dotnet restore "WebApps/IdentityServer/IdentityServer.csproj"
COPY . .
WORKDIR "/src/WebApps/IdentityServer"
RUN dotnet build "IdentityServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdentityServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS http://*:7000
ENTRYPOINT ["dotnet", "IdentityServer.dll"]
