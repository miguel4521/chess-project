# FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
# WORKDIR /app
# EXPOSE 80
# EXPOSE 443

# FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
# WORKDIR /src
# COPY ["chess-project/chess-project.csproj", "chess-project/"]
# RUN dotnet restore "chess-project/chess-project.csproj"
# COPY . .
# WORKDIR "/src/chess-project"
# RUN dotnet build "chess-project.csproj" -c Release -o /app/build

# FROM build AS publish
# RUN dotnet publish "chess-project.csproj" -c Release -o /app/publish

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "chess-project.dll"]
