# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia todo el proyecto
COPY . .

# Restaura dependencias
RUN dotnet restore

# Compila y publica
RUN dotnet publish -c Release -o out

# Etapa de ejecución
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app

# Copia los archivos compilados desde la etapa anterior
COPY --from=build /app/out .

# Comando de inicio del contenedor
ENTRYPOINT ["dotnet", "LogViewer.dll"]