# ---------- build ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# ---------- runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish ./

# Render provides PORT env var. App MUST listen on it.
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}

# (Optional) Expose (not required by Render, but okay)
EXPOSE 10000

CMD ["sh", "-c", "dotnet Codeikoo.TodoApi.dll"]
