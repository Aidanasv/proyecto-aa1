FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish "proyecto-aa1.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
RUN apt-get update && apt-get install -y
COPY --from=build /app ./
VOLUME ["/app/data"]
ENV LOG_PATH=/app/data/logs
ENV DATA_PATH=/app/data/data
EXPOSE 7818
ENTRYPOINT ["dotnet", "proyecto-aa1.dll"]


