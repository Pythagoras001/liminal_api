# Liminal API

Backend de la aplicación Liminal desarrollado en ASP.NET Core con arquitectura modular. La API gestiona autenticación de usuarios, gestión de clases de nivel y publicación de reportes con evidencias multimedia.

## Características

- Autenticación JWT para usuarios.
- Registro e inicio de sesión con hash seguro mediante BCrypt.
- Gestión de perfil de usuario y subida de avatar.
- Administración de clases de nivel (`class`).
- Creación, edición, listado y eliminación de reportes (`report`).
- Soporte para evidencias principales y galerías de imágenes.
- Likes/ratings en reportes.
- Persistencia con SQLite.
- Integración con Cloudinary para almacenamiento de imágenes.
- CORS habilitado para frontend en `http://localhost:5173`.

## Stack

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- JWT Bearer Authentication
- CloudinaryDotNet
- BCrypt.Net-Next

## Estructura del proyecto

```text
liminal_api/
├── Controllers/                     # Controladores base residual
├── Modules/
│   ├── Image/                      # Integración Cloudinary
│   ├── LevelClass/                # Casos de uso de clases de nivel
│   ├── Report/                    # Casos de uso de reportes
│   └── User/                      # Autenticación y usuarios
├── Shared/
│   └── Infrastructure/Persistence/
├── appsettings.json
├── appsettings.Development.json
├── liminal.db
├── liminal_api.csproj
├── Program.cs
└── Properties/launchSettings.json
```

## Requisitos

- .NET SDK 10.0+
- SQLite
- Cuenta de Cloudinary para subir imágenes

## Instalación y ejecución

1. Clona el proyecto y entra a la carpeta raíz.
2. Restaura paquetes:

```bash
dotnet restore
```

3. Configura las credenciales de Cloudinary y el secreto JWT.
4. Ejecuta la API:

```bash
dotnet run --project liminal_api/liminal_api.csproj
```

La API se ejecuta normalmente en:

- HTTP: `http://localhost:5276`
- HTTPS: `https://localhost:7117`

## Configuración de entorno

El proyecto usa `appsettings.json` para la conexión a SQLite y ajustes JWT. Además, la integración con Cloudinary se valida al iniciar la aplicación mediante `CloudinaryOptions`.

Puedes configurar Cloudinary usando User Secrets o variables de entorno, por ejemplo:

```bash
dotnet user-secrets set "Cloudinary:CloudName" "tu_cloud_name"
dotnet user-secrets set "Cloudinary:ApiKey" "tu_api_key"
dotnet user-secrets set "Cloudinary:ApiSecret" "tu_api_secret"
```

También puedes ajustar el JWT en `appsettings.json`:

```json
"Jwt": {
  "Key": "tu_clave_secreta_muy_larga",
  "Issuer": "liminal_api",
  "Audience": "liminal_api",
  "ExpireMinutes": 60
}
```

## Endpoints principales

### Usuarios

```http
POST /User/register
POST /User/login
GET /User/me
PATCH /User/update
PUT /User/me
```

### Clases de nivel

```http
GET /class
POST /class
PATCH /class/{id}
```

### Reportes

```http
GET /report
GET /report/{id}
GET /report/me
POST /report
PATCH /report
PATCH /report/{id}
DELETE /report/{id}
```

> Los endpoints de creación y actualización de reportes requieren autenticación y aceptan `multipart/form-data` para archivos de evidencia.

## Ejemplo de autenticación

Después de hacer login, usa el token devuelto en el header:

```http
Authorization: Bearer <token>
```

## Base de datos

La API usa SQLite con la base de datos local:

```text
liminal_api/liminal.db
```

Se crea y migra automáticamente en entorno de desarrollo con `EnsureCreated()` y `Migrate()`.

## Nota

Este proyecto está pensado para integrarse con un frontend en Vite/React u otro cliente que se ejecute en el puerto 5173, por lo que la API incluye CORS para ese origen por defecto.