# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution file and restore dependencies
COPY *.sln .
COPY QuizApp.API/*.csproj ./QuizApp.API/
COPY QuizApp.Models/*.csproj ./QuizApp.Models/
COPY QuizApp.Wasm/*.csproj ./QuizApp.Wasm/    # ✅ This fixes the restore error!

RUN dotnet restore

# Copy the rest of the application code
COPY . .

# Build the application (just API in this case)
WORKDIR /src/QuizApp.API
RUN dotnet publish -c Release -o /app

# Use the official .NET runtime image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the build output from the SDK image
COPY --from=build /app .

# Expose the port your API runs on
EXPOSE 80

# Set the entry point for the container
ENTRYPOINT ["dotnet", "QuizApp.API.dll"]
