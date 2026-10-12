# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files for restoration
COPY ["Directory.Build.props", "./"]
COPY ["MskCore/MskCore.csproj", "MskCore/"]
COPY ["MskConsole/MskConsole.csproj", "MskConsole/"]
COPY ["MskBenchmarks/MskBenchmarks.csproj", "MskBenchmarks/"]

# Restore dependencies for MskConsole
RUN dotnet restore "MskConsole/MskConsole.csproj"

# Copy the rest of the source code
COPY . .

# Publish MskConsole
WORKDIR "/src/MskConsole"
RUN dotnet publish "MskConsole.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

# Set default environment variables matching configuration options (Msk:ApiKey, Msk:ModelId, Msk:Endpoint)
# In GitHub Actions or Docker run, pass -e Msk__ApiKey=your_api_key
ENV Msk__ModelId="gemini-3.5-flash-lite"
ENV Msk__Endpoint="https://generativelanguage.googleapis.com/v1beta/openai/"

ENTRYPOINT ["dotnet", "MskConsole.dll"]
