    using System.Text.Json;

namespace Laboratorio_2_CSV_JSON;
class Program
{
    static void Main(string[] args)
    {
        //Seteamos la direccion del archvo CSV
        //Como el archivo csv esta en el mismo archivo que el programa, solo se coloca el nombre del archivo, una ruta relativa.
        string csvFilePath = "estudiantes.csv";


        //Validamos la existencia del archivo CSV
        if(!File.Exists(csvFilePath))
        {
            Console.WriteLine($"El archivo CSV no existe.");
            return;
        }
        //Leemos todas las lineas del archivo CSV y las guardamos en un arreglo de strings
        string[] csvLines = File.ReadAllLines(csvFilePath);

        //Hay que saltarse el encabezado
        //Guardamos los datos en una lista de objetos Estudiante
        List<Estudiante> estudiantes = new List<Estudiante>();
        for(int i = 1; i < csvLines.Length; i++)
        {
            String line = csvLines[i];

            //En caso de que existiese una linea que esta vacia, la saltamos
            if(string.IsNullOrWhiteSpace(line)) continue;
            

            //Dividimos las lineas en columnas, usando la coma como separador
            String[] columns = line.Split(',');

            //Creamos un objeto estudiante y le asignamos los valores de las columnas
            Estudiante estudiante = new Estudiante();
            estudiante.Id = int.Parse(columns[0].Trim());
            estudiante.Nombre = columns[1].Trim();
            estudiante.Carrera = columns[2].Trim();

            //Mostramos los datos del estudiante en consola
            Console.WriteLine($"Id: {estudiante.Id}, Nombre: {estudiante.Nombre}, Carrera: {estudiante.Carrera}");
            estudiantes.Add(estudiante);
        }

        //Guardamos los datos en un archivo JSON
        string jsonFilePath = "estudiantes.json";

        //Para que el Json quede bien formateado
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = System.Text.Json.JsonSerializer.Serialize(estudiantes, options);

        File.WriteAllText(jsonFilePath, json);
        Console.WriteLine("\nArchivo JSON generado con exito!!!");
    }   
}

public class Estudiante
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Carrera { get; set; }

    public Estudiante()
    {
        Id = 0;
        Nombre = "";
        Carrera = "";
    }
}

