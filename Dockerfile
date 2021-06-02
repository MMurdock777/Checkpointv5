FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["KekpointCool/KekpointCool.csproj", "KekpointCool/"]
RUN dotnet restore "KekpointCool/KekpointCool.csproj"
COPY . .
WORKDIR "/src/KekpointCool"
RUN dotnet build "KekpointCool.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KekpointCool.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KekpointCool.dll"]
