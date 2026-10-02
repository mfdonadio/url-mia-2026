Laboratorio No. 2 - MIA Azure Blob Storage

OBJETIVO
    Desarrollar una aplicación de consola en C# para gestionar archivos
    en Azure Blob Storage: subir, listar, descargar y eliminar.

TECNOLOGÍAS UTILIZADAS
    - C# / .NET
    - Azure.Storage.Blobs
    - DotNetEnv
    - Azure Portal
    - GitHub Secrets

CONFIGURACIÓN DE AZURE
    1. Se creó un Storage Account en Azure Portal.
    2. Se configuró un contenedor privado llamado "marchivos".
    3. Se obtuvo la Connection String desde:
       Security + networking -> Access keys.

ARQUITECTURA DE LA SOLUCIÓN
    BlobServiceClient:
        Conecta la aplicación con la cuenta de almacenamiento.

    BlobContainerClient:
        Administra las operaciones sobre el contenedor "marchivos".

    BlobClient:
        Gestiona las operaciones sobre cada archivo o blob.

OPERACIONES
    1. Subir archivo:
        Valida la ruta local y sube el archivo al contenedor.

    2. Listar archivos:
        Muestra el nombre y el tamaño en bytes de los blobs.

    3. Descargar archivo:
        Verifica que el blob exista y lo guarda en la carpeta indicada.

    4. Eliminar archivo:
        Solicita confirmación antes de eliminar el blob.

MANEJO DE ERRORES
    Se utilizan bloques try-catch para capturar errores de conexión
    y de las operaciones.

    También se valida la existencia de archivos, carpetas y blobs
    antes de realizar las acciones correspondientes.

PROTECCIÓN DE LA CONNECTION STRING
    La Connection String no está escrita directamente en Program.cs.
    La aplicación obtiene su valor desde la variable de entorno:

        AZURE_STORAGE_CONNECTION_STRING

    Para ejecutar localmente, DotNetEnv carga esa variable desde .env.

CONFIGURACIÓN LOCAL
    1. Crear el archivo .env en la carpeta del proyecto.

    2. Agregar la Connection String completa en una sola línea:

        AZURE_STORAGE_CONNECTION_STRING=TU_CONNECTION_STRING_COMPLETA

    3. En Program.cs, cargar el archivo antes de leer la variable:

        DotNetEnv.Env.NoClobber().Load(".env");

        string? connectionString =
            Environment.GetEnvironmentVariable(
                "AZURE_STORAGE_CONNECTION_STRING");

    NoClobber() conserva el valor si la variable ya existe en el entorno.

    El archivo .env contiene la credencial real y no se sube a GitHub.
    Para excluirlo, se agregó esta regla en .gitignore:

        .env
        
INSTRUCCIONES DE EJECUCIÓN
    1. Abrir una terminal en la carpeta del proyecto.
    2. Configurar .env con la Connection String completa.
    3. Ejecutar:

        dotnet run --project MIA_AzureBlob_MD.csproj

    4. Seleccionar una operación en el menú de la aplicación.

SECRETO EN GITHUB
    Se creó un secreto desde:

        Settings -> Secrets and variables -> Actions
        -> New repository secret

    Nombre del secreto:

        AZURE_STORAGE_CONNECTION_STRING_GITHUB

    Valor del secreto:
        La Connection String completa de Azure Storage.

    Guardar el secreto en GitHub no lo hace disponible automáticamente
    al ejecutar el programa en la computadora.

ARCHIVO .env.production
    Se publica como plantilla sin credenciales reales:

        # Secreto de GitHub: AZURE_STORAGE_CONNECTION_STRING_GITHUB
        AZURE_STORAGE_CONNECTION_STRING=

    El programa actual carga .env para la ejecución local.
    .env.production no obtiene el secreto de GitHub por sí solo.

CÓMO FUNCIONARÍA EN GITHUB ACTIONS
    Un workflow podría entregar el secreto al programa mediante:

        env:
          AZURE_STORAGE_CONNECTION_STRING: ${{ secrets.AZURE_STORAGE_CONNECTION_STRING_GITHUB }}

    Este fragmento debe formar parte de un workflow completo ubicado
    en .github/workflows/ desde la raíz del repositorio.

    La aplicación leería la variable de entorno sin tener que guardar
    la credencial en un archivo publicado.

    Actualmente no se incluye un workflow y la ejecución es local.
    Para automatizar las operaciones en GitHub Actions, habría que
    adaptar el menú para funcionar sin intervención del usuario.