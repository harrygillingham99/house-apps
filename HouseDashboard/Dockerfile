# syntax=docker/dockerfile:1

# ---- Build stage ----
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Install Node.js (required by the csproj build target that runs webpack)
RUN curl -fsSL https://deb.nodesource.com/setup_18.x | bash - \
    && apt-get install -y --no-install-recommends nodejs \
    && rm -rf /var/lib/apt/lists/*

# Restore .NET dependencies first for better layer caching
COPY front-end.csproj ./
RUN dotnet restore front-end.csproj

# Restore npm dependencies for better layer caching
COPY package.json package-lock.json* ./
RUN npm install --force

# Copy the rest of the source and publish (Release build runs "npm run prod" via csproj target)
COPY . .
RUN dotnet publish front-end.csproj -c Release -o /app/publish --no-restore

# ---- Runtime stage ----
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "front-end.dll"]
