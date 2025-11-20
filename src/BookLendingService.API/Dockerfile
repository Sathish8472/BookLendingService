# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY ["BookLendingService.API.csproj", "./"]
RUN dotnet restore

# Copy the rest of the source code
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Force ASP.NET Core to listen on port 80 inside the container
ENV ASPNETCORE_URLS=http://+:80

COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "BookLendingService.API.dll"]
