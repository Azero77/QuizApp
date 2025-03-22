# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution file and restore dependencies
COPY *.sln .                        
COPY QuizApp.API/*.csproj ./QuizApp.API/
COPY QuizApp.Models/*.csproj ./QuizApp.Models/
COPY QuizApp.WASM/*.csproj ./QuizApp.WASM/

RUN dotnet restore

# Copy the rest of the application code
COPY . .

# Build the application (QuizApp.API in this case)
WORKDIR /src/QuizApp.API
RUN dotnet publish -c Release -o /app

# --------------------------------------------------------------------------------------------------

# Use the official .NET runtime image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the build output from the build stage
COPY --from=build /app . 

# Copy your certificate (this file should be at the Docker build context root!)
COPY aspnetapp.pfx /https/aspnetapp.pfx

# Expose the ports
EXPOSE 80
EXPOSE 443

# # Environment variables for the cert (runtime stage!)
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=bios7777

# Start the app
ENTRYPOINT ["dotnet", "QuizApp.API.dll"]
