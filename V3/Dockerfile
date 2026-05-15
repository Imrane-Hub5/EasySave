FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY LogServer/ ./LogServer/
RUN dotnet publish LogServer/LogServer.csproj -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .
EXPOSE 5000
ENTRYPOINT ["dotnet", "LogServer.dll"] 
