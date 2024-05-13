#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy AS base
WORKDIR /app
RUN apt-get update && \
	apt-get install -y wget wkhtmltopdf

USER app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WkHtmlTo.Wrapper.Samples.Mvc/WkHtmlTo.Wrapper.Samples.Mvc.csproj", "WkHtmlTo.Wrapper.Samples.Mvc/"]
RUN dotnet restore "./WkHtmlTo.Wrapper.Samples.Mvc/WkHtmlTo.Wrapper.Samples.Mvc.csproj"
COPY . .
WORKDIR "/src/WkHtmlTo.Wrapper.Samples.Mvc"
RUN dotnet build "./WkHtmlTo.Wrapper.Samples.Mvc.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WkHtmlTo.Wrapper.Samples.Mvc.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WkHtmlTo.Wrapper.Samples.Mvc.dll"]