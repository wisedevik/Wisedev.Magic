FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

COPY . .

RUN dotnet restore Wisedev.Magic.sln
RUN dotnet publish Wisedev.Magic.Server/Wisedev.Magic.Server.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 9339

ENTRYPOINT ["dotnet", "Wisedev.Magic.Server.dll"]