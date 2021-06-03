FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["KekpointCool/KekpointCool.csproj", "KekpointCool/"]
RUN dotnet restore "KekpointCool/KekpointCool.csproj"
COPY ["TimeControlService/TimeControlService.csproj", "TimeControlService/"]
RUN dotnet restore "TimeControlService/TimeControlService.csproj"
COPY ["UserService/UserService.csproj", "UserService/"]
RUN dotnet restore "UserService/UserService.csproj"
COPY . .
WORKDIR "/src/KekpointCool"
RUN dotnet build "KekpointCool.csproj" -c Release -o /app/build
WORKDIR "/src/TimeControlService"
RUN dotnet build "TimeControlService.csproj" -c Release -o /app/build
WORKDIR "/src/UserService"
RUN dotnet build "UserService.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "KekpointCool.csproj" -c Release -o /app/publish
RUN dotnet publish "TimeControlService.csproj" -c Release -o /app/publish
RUN dotnet publish "UserService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KekpointCool.dll"]
ENTRYPOINT ["dotnet", "TimeControlService.dll"]
ENTRYPOINT ["dotnet", "UserService.dll"]
