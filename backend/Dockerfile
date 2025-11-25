# --- STAGE 1: Build ---
# Use the.NET 9 SDK, which is required for.slnx file support.
# Using a specific version tag is recommended for reproducible builds.
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files first to leverage Docker layer caching.
# This ensures 'dotnet restore' only runs when dependencies change.
COPY YumaIdentity.slnx .
COPY src/Core/YumaIdentity.Domain/YumaIdentity.Domain.csproj src/Core/YumaIdentity.Domain/
COPY src/Core/YumaIdentity.Application/YumaIdentity.Application.csproj src/Core/YumaIdentity.Application/
COPY src/Infrastructure/YumaIdentity.Infrastructure/YumaIdentity.Infrastructure.csproj src/Infrastructure/YumaIdentity.Infrastructure/
COPY src/Presentation/YumaIdentity.API/YumaIdentity.API.csproj src/Presentation/YumaIdentity.API/

# Restore dependencies.
RUN dotnet restore "YumaIdentity.slnx"

# Copy the rest of the source code.
COPY . .

# Publish the application.
WORKDIR "/src/src/Presentation/YumaIdentity.API"
RUN dotnet publish "YumaIdentity.API.csproj" -c Release -o /app/publish --no-restore

# --- STAGE 2: Final ---
# Use the corresponding.NET 9 ASP.NET runtime image.
# It's smaller and more secure as it doesn't contain the SDK.
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Create a non-root user for security.
RUN adduser -u 5000 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

# Copy the published output from the build stage.
COPY --from=build /app/publish .

# Expose the port the app will run on.
# This can be overridden by docker-compose.yml.
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Set the entrypoint for the application.
ENTRYPOINT ["dotnet", "YumaIdentity.API.dll"]