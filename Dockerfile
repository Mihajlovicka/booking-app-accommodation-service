FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source
COPY AccommodationService/*.csproj AccommodationService/
RUN dotnet restore AccommodationService/AccommodationService.csproj 


COPY AccommodationService/ AccommodationService/
WORKDIR /source/AccommodationService
RUN dotnet build -c release --no-restore

# test
FROM build AS test
COPY AccommodationService.Tests/*.csproj AccommodationService.Tests/
RUN dotnet restore AccommodationService.Tests/AccommodationService.Tests.csproj

WORKDIR /source/AccommodationService.Tests
COPY AccommodationService.Tests/ .
ENTRYPOINT ["dotnet", "test", "--logger:console;verbosity=detailed"]


FROM build AS publish
WORKDIR /source/AccommodationService
RUN dotnet publish -c release --no-build -o /app

# final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app .

EXPOSE 8080

ENTRYPOINT ["dotnet", "AccommodationService.dll"]