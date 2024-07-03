# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com / dotnet / aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FitnessFormulaMVP/FitnessFormulaMVP.csproj", "FitnessFormulaMVP/"]
RUN dotnet restore "FitnessFormulaMVP/FitnessFormulaMVP.csproj"
COPY . .
WORKDIR "/src/FitnessFormulaMVP"
RUN dotnet build "FitnessFormulaMVP.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FitnessFormulaMVP.csproj" -c Release -o /app/publish

# Copy the build output to the base image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FitnessFormulaMVP.dll"]
