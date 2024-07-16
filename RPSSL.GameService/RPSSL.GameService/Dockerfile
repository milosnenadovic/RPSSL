#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
EXPOSE 5432

FROM postgres:16 AS postgres
ENV POSTGRES_PASSWORD=SuperSecret2024!
ENV POSTGRES_USER=admin
ENV PATH=/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin:/usr/lib/postgresql/16/bin
ENV GOSU_VERSION=1.16
ENV LANG=en_US.utf8
ENV PG_MAJOR=16
ENV PG_VERSION=16.1-1.pgdg120+1
ENV PGDATA=/var/lib/postgresql/data


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RPSSL.GameService/RPSSL.GameService.csproj", "RPSSL.GameService/"]
RUN dotnet restore "./RPSSL.GameService/RPSSL.GameService.csproj"
COPY . .
WORKDIR "/src/RPSSL.GameService"
RUN dotnet build "./RPSSL.GameService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RPSSL.GameService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=postgres /usr/local/bin/docker-entrypoint.sh /usr/local/bin/
COPY --from=postgres /usr/local/lib/postgresql/ /usr/local/lib/postgresql/
COPY --from=postgres /usr/local/share/postgresql/ /usr/local/share/postgresql/
COPY --from=postgres /usr/lib/postgresql/ /usr/lib/postgresql/
COPY --from=postgres /var/lib/postgresql/ /var/lib/postgresql/

ENTRYPOINT ["sh", "-c", "docker-entrypoint.sh postgres & dotnet RPSSL.GameService.dll"]