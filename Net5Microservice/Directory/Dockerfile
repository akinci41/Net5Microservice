#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Directory/Directory.csproj", "Directory/"]
RUN dotnet restore "Directory/Directory.csproj"
COPY . .
WORKDIR "/src/Directory"
RUN dotnet build "Directory.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Directory.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Directory.dll"]
