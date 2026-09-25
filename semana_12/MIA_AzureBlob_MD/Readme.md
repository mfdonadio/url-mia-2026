#Laboratorio No. 2 - MIA Azure Blob Storage

Objetivo
Desarrollar una aplicación de consola en C# para interactuar con Azure Blob Storage, permitiendo gestionar archivos (subir, listar, descargar y eliminar) a través de una Connection String.

Tecnologías Utilizadas
    C# / .NET
    Azure.Storage.Blobs (SDK oficial de Azure)
    Azure Portal (Blob Storage Account)

#Configuración de Azure
    1. Se creó un Storage Account en el portal de Azure.
    2. Se configuró un contenedor privado llamado `marchivos`.
    3. Se obtuvo la Connection String desde `Security + networking -> Access keys`.

#Arquitectura de la Solución
    **BlobServiceClient:** Punto de entrada para conectar con la cuenta de almacenamiento.
    **BlobContainerClient:** Administra las operaciones sobre el contenedor `marchivos`.
    **BlobClient:** Gestiona las acciones directas sobre los archivos individuales (blobs).

#Descripción de Operaciones
    1. **Subir Archivo:** Valida la ruta del archivo local y lo sube al contenedor de Azure Blob.
    2. **Listar Archivos:** Obtiene todos los blobs existentes mostrando su nombre y tamaño en bytes.
    3. **Descargar Archivo:** Verifica la presencia del blob en la nube y lo guarda en la ruta local especificada.
    4. **Eliminar Archivo:** Solicita confirmación previa y remueve el archivo del contenedor.

#Manejo de Errores
Se implementaron bloques `try-catch` para capturar errores de red o credenciales, y validaciones con `File.Exists()`, `Directory.Exists()` y `blobClient.ExistsAsync()` para prevenir excepciones por rutas inexistentes.

#Protección de la Connection String
Para prevenir la filtración de credenciales sensibles en repositorios públicos como GitHub, la Connection String no fue codificada en duro (*hardcoded*). En su lugar, se configuró como una variable de entorno del sistema (`AZURE_STORAGE_CONNECTION_STRING`), leída en tiempo de ejecución por la aplicación.

#Instrucciones de Ejecución
    1. Configurar la variable de entorno:
   ```bash
   setx AZURE_STORAGE_CONNECTION_STRING "Tu_Connection_String"