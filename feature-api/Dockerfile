FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY Customers.Api/Customers.Api.csproj Customers.Api/
COPY Customers.Application/Customers.Application.csproj Customers.Application/
COPY Customers.Infrastructure/Customers.Infrastructure.csproj Customers.Infrastructure/
COPY Customers.Domain/Customers.Domain.csproj Customers.Domain/

RUN dotnet restore Customers.Api/Customers.Api.csproj

COPY . .
WORKDIR /src/Customers.Api
RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 80

ENTRYPOINT ["dotnet", "Customers.Api.dll"]
