FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 8080
EXPOSE 443


FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WCMAPI/WCMAPI.csproj", "WCMAPI/"]
RUN dotnet restore "WCMAPI/WCMAPI.csproj"
COPY . .
WORKDIR "/src/WCMAPI"
RUN dotnet build "WCMAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WCMAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WCMAPI.dll"]
