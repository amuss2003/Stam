#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime-deps:5.0-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /src
COPY ["TestApp.UnitTests/TestApp.UnitTests.csproj", "TestApp.UnitTests/"]
COPY ["TestApp/TestApp.csproj", "TestApp/"]
COPY . .
RUN dotnet build

FROM build AS unit-testing
RUN dotnet test "TestApp.UnitTests/TestApp.UnitTests.csproj"

FROM build AS publish
WORKDIR "/src/TestApp"
RUN dotnet publish "TestApp.csproj" -c Release -o /app/publish --runtime alpine-x64 --self-contained true /p:PublishTrimmed=true /p:PublishSingleFile=true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["./TestApp", "--urls", "http://0.0.0.0:80"]