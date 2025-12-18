# Arandata API

API REST desarrollada con **ASP.NET Core (.NET 8)** y **MySQL**.

## Requisitos

- .NET 8 SDK
- MySQL/MariaDB en ejecución

## Configuración

1. **Configurar archivo `.env`** en la carpeta `src/Arandata.API/`:

   ```
   BD_HOST=localhost
   DB_PORT=3306
   DB_NAME=arandata
   DB_USER=root
   DB_PASSWORD=tu_contraseña
   ```

2. **Ejecutar el proyecto**:

   ```powershell
   cd src\Arandata.API
   dotnet run
   ```

   **Nota**: Al ejecutar por primera vez, la aplicación creará automáticamente:

   - La base de datos `arandata`

## Acceso

- **API**: http://localhost:5185
- **Swagger**: http://localhost:5185/swagger
