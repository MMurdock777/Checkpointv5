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

WORKDIR /app
EXPOSE 5000
EXPOSE 5001

WORKDIR /src
COPY ["TimeControlService/TimeControlService.csproj", "TimeControlService/"]
RUN dotnet restore "TimeControlService/TimeControlService.csproj"
COPY . .
WORKDIR "/src/TimeControlService"
RUN dotnet build "TimeControlService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TimeControlService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TimeControlService.dll"]

WORKDIR /app
EXPOSE 5002
EXPOSE 5003

WORKDIR /src
COPY ["UserService/UserService.csproj", "UserService/"]
RUN dotnet restore "UserService/UserService.csproj"
COPY . .
WORKDIR "/src/UserService"
RUN dotnet build "UserService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserService.dll"]
