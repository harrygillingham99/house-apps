# syntax=docker/dockerfile:1

# ---- Build stage ----
FROM node:18-bullseye-slim AS node-build
WORKDIR /src

COPY package.json package-lock.json* ./
RUN npm install --force

# ---- .NET SDK stage ----
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy node binaries and modules from node-build stage
COPY --from=node-build /usr/local/bin /usr/local/bin
COPY --from=node-build /usr/local/lib/node_modules /usr/local/lib/node_modules
COPY --from=node-build /src/node_modules ./node_modules
COPY package.json package-lock.json* ./

# Restore .NET dependencies
COPY front-end.csproj ./
RUN dotnet restore front-end.csproj

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
