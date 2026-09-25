using Azure.Storage.Blobs;

// Obtener Connection String desde la variable de entorno
string? connectionString =
    Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING"); //----> aqui va el connection string

if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.WriteLine("No se encontró la Connection String.");
    return;
}

// Nombre real de tu container
string nombreContainer = "marchivos";

try
{
    // Conexión con Azure Blob Storage
    BlobServiceClient servicio =
        new BlobServiceClient(connectionString);

    BlobContainerClient container =
        servicio.GetBlobContainerClient(nombreContainer);

    // Verificar que el container exista
    if (!await container.ExistsAsync())
    {
        Console.WriteLine("El container no existe.");
        return;
    }

    int opcion;

    do
    {
        Console.Clear();

        Console.WriteLine("=================================");
        Console.WriteLine("     MIA - AZURE BLOB STORAGE");
        Console.WriteLine("=================================");
        Console.WriteLine("1. Subir archivo");
        Console.WriteLine("2. Listar archivos");
        Console.WriteLine("3. Descargar archivo");
        Console.WriteLine("4. Eliminar archivo");
        Console.WriteLine("5. Salir");
        Console.WriteLine("=================================");
        Console.Write("Seleccione una opción: ");

        if (!int.TryParse(Console.ReadLine(), out opcion))
        {
            opcion = 0;
        }

        Console.WriteLine();

        switch (opcion)
        {
            case 1:
                await SubirArchivo(container);
                break;

            case 2:
                await ListarArchivos(container);
                break;

            case 3:
                await DescargarArchivo(container);
                break;

            case 4:
                await EliminarArchivo(container);
                break;

            case 5:
                Console.WriteLine("Saliendo del programa...");
                break;

            default:
                Console.WriteLine("Opción inválida.");
                break;
        }

        if (opcion != 5)
        {
            Console.WriteLine();
            Console.WriteLine("Presione una tecla para continuar...");
            Console.ReadKey();
        }

    } while (opcion != 5);
}
catch (Exception ex)
{
    Console.WriteLine("Error de conexión con Azure:");
    Console.WriteLine(ex.Message);
}


// ==============================
// SUBIR ARCHIVO
// ==============================
static async Task SubirArchivo(BlobContainerClient container)
{
    Console.WriteLine("========== SUBIR ARCHIVO ==========");
    Console.WriteLine();

    Console.Write("Ingrese la ruta del archivo: ");
    string? ruta = Console.ReadLine();

    // Validar ruta
    if (string.IsNullOrWhiteSpace(ruta))
    {
        Console.WriteLine("La ruta no puede estar vacía.");
        return;
    }

    // Validar que exista el archivo
    if (!File.Exists(ruta))
    {
        Console.WriteLine("El archivo no existe.");
        return;
    }

    try
    {
        // Obtener únicamente el nombre
        string nombreArchivo = Path.GetFileName(ruta);

        // Crear referencia al blob
        BlobClient blob =
            container.GetBlobClient(nombreArchivo);

        // Subir archivo
        await blob.UploadAsync(ruta, overwrite: true);

        Console.WriteLine();
        Console.WriteLine("Archivo subido correctamente.");
        Console.WriteLine($"Nombre: {nombreArchivo}");
    }
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine("Error al subir el archivo:");
        Console.WriteLine(ex.Message);
    }
}

// ==============================
// LISTAR ARCHIVOS SUBIDOS
// ==============================
static async Task ListarArchivos(BlobContainerClient container)
{
    Console.WriteLine("========== LISTAR ARCHIVOS ==========");
    Console.WriteLine();

    Console.WriteLine("{0,-35} {1,15}", "Nombre", "Tamaño");
    Console.WriteLine(new string('-', 52));

    bool hayArchivos = false;

    await foreach (var blob in container.GetBlobsAsync())
    {
        hayArchivos = true;

        long tamanio = blob.Properties.ContentLength ?? 0;

        Console.WriteLine(
            "{0,-35} {1,15} bytes",
            blob.Name,
            tamanio
        );
    }

    if (!hayArchivos)
    {
        Console.WriteLine("No hay archivos en el container.");
    }
}

// =====================================
// DESCARGAR ARCHIVO
// =====================================
static async Task DescargarArchivo(BlobContainerClient container)
{
    Console.WriteLine("========== DESCARGAR ARCHIVO ==========");
    Console.WriteLine();

    Console.Write("Ingrese el nombre del archivo: ");
    string? nombreArchivo = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(nombreArchivo))
    {
        Console.WriteLine("El nombre no puede estar vacío.");
        return;
    }

    nombreArchivo = nombreArchivo.Trim();

    try
    {
        BlobClient blob =
            container.GetBlobClient(nombreArchivo);

        // Validar que el blob exista
        if (!(await blob.ExistsAsync()).Value)
        {
            Console.WriteLine("El archivo no existe en Azure.");
            return;
        }

        Console.Write("Ingrese la carpeta de destino: ");
        string? carpetaDestino = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(carpetaDestino))
        {
            Console.WriteLine("La carpeta no puede estar vacía.");
            return;
        }

        carpetaDestino =
            carpetaDestino.Trim().Trim('"');

        // Validar que la carpeta exista
        if (!Directory.Exists(carpetaDestino))
        {
            Console.WriteLine("La carpeta de destino no existe.");
            return;
        }

        // Construir ruta final
        string rutaDestino =
            Path.Combine(carpetaDestino, nombreArchivo);

        // Descargar
        await blob.DownloadToAsync(rutaDestino);

        Console.WriteLine();
        Console.WriteLine("Archivo descargado correctamente.");
        Console.WriteLine($"Ubicación: {rutaDestino}");
    }
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine("Error al descargar el archivo:");
        Console.WriteLine(ex.Message);
    }
}


// =====================================
// ELIMINAR ARCHIVO
// =====================================
static async Task EliminarArchivo(BlobContainerClient container)
{
    Console.WriteLine("========== ELIMINAR ARCHIVO ==========");
    Console.WriteLine();

    Console.Write("Ingrese el nombre del archivo: ");
    string? nombreArchivo = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(nombreArchivo))
    {
        Console.WriteLine("El nombre no puede estar vacío.");
        return;
    }

    nombreArchivo = nombreArchivo.Trim();

    try
    {
        BlobClient blob =
            container.GetBlobClient(nombreArchivo);

        // Validar que exista
        if (!(await blob.ExistsAsync()).Value)
        {
            Console.WriteLine("El archivo no existe en Azure.");
            return;
        }

        Console.Write(
            $"¿Está seguro de eliminar '{nombreArchivo}'? (S/N): "
        );

        string? confirmacion = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(confirmacion) ||
            confirmacion.Trim().ToUpper() != "S")
        {
            Console.WriteLine("Eliminación cancelada.");
            return;
        }

        // Eliminar blob
        bool eliminado =
            (await blob.DeleteIfExistsAsync()).Value;

        if (eliminado)
        {
            Console.WriteLine();
            Console.WriteLine("Archivo eliminado correctamente.");
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("No fue posible eliminar el archivo.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine("Error al eliminar el archivo:");
        Console.WriteLine(ex.Message);
    }
}
