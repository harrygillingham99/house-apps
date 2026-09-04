FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["API/House.API.csproj", "API/"]
COPY ["HLL/House.HLL.csproj", "HLL/"]
COPY ["DAL/House.DAL.csproj", "DAL/"]
COPY ["House.SignalR/House.SignalR.csproj", "House.SignalR/"]
COPY ["House.Objects/House.Objects.csproj", "House.Objects/"]

RUN dotnet restore "API/House.API.csproj"

COPY . .
WORKDIR "/src/API"
RUN dotnet publish "House.API.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "House.API.dll"]