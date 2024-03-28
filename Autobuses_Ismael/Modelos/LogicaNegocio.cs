using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Autobuses_Ismael.Modelos
{
    public class LogicaNegocio
    {
        private static List<Linea> lineas;
        private static List<Itinerario> itinerarios;
        private static string rutaArchivoItinerariosCSV = "Datos/itinerarios.csv";
        private static string rutaRelativaArchivoCSV = "Datos/lineas.csv";
        private static string rutaArchivoLineasJson = "Datos/lineas.json";
        private static string rutaArchivoItinerariosJson = "Datos/itinerarios.json";

        static LogicaNegocio()
        {
            lineas = LeerCSVLineas(rutaRelativaArchivoCSV);
            itinerarios = LeerCSVItinerarios(rutaArchivoItinerariosCSV);
        }


        //Logica de linea

        public static void AltaLinea(Linea nuevaLinea)
        {
            if (!lineas.Any(l => l.NumeroLinea == nuevaLinea.NumeroLinea))
            {
                lineas.Add(nuevaLinea);
                GenerarCSVLineas(lineas);
                GuardarEnJson(lineas, rutaArchivoLineasJson);
                Console.WriteLine("Línea agregada correctamente.");
            }
            else
            {
                Console.WriteLine("Error: Ya existe una línea con este número.");
            }
        }

        public static void BajaLinea(int numeroLinea)
        {
            var linea = lineas.FirstOrDefault(l => l.NumeroLinea == numeroLinea);
            if (linea != null)
            {
                lineas.Remove(linea);
                GenerarCSVLineas(lineas);
                GuardarEnJson(lineas, rutaArchivoLineasJson);
                Console.WriteLine("Línea eliminada correctamente.");
            }
            else
            {
                Console.WriteLine("Error: La línea no existe.");
            }
        }

        public static void ModificarLinea(int numeroLinea, Linea datosActualizados)
        {
            var linea = lineas.FirstOrDefault(l => l.NumeroLinea == numeroLinea);
            if (linea != null)
            {
                linea.MunicipioOrigen = datosActualizados.MunicipioOrigen;
                linea.MunicipioDestino = datosActualizados.MunicipioDestino;
                linea.HoraInicialSalida = datosActualizados.HoraInicialSalida;
                linea.IntervaloEntreBuses = datosActualizados.IntervaloEntreBuses;
                GenerarCSVLineas(lineas);
                GuardarEnJson(lineas, rutaArchivoLineasJson);
                Console.WriteLine("Línea modificada correctamente.");
            }
            else
            {
                Console.WriteLine("Error: La línea no existe.");
            }
        }

        public static List<Linea> ConsultarLineas(int? numeroLinea = null)
        {
            List<Linea> lineasFiltradas;
            if (numeroLinea.HasValue)
            {
                lineasFiltradas = lineas.Where(l => l.NumeroLinea == numeroLinea.Value).ToList();
            }
            else
            {
                lineasFiltradas = lineas;
            }

            return lineasFiltradas;
        }

        public static void GenerarCSVLineas(List<Linea> lineas)
        {
            var rutaBase = AppDomain.CurrentDomain.BaseDirectory;

            var rutaArchivoCSV = Path.Combine(rutaBase, "Datos", "lineas.csv");

            try
            {
                // Asegura que el directorio exista.
                var directorio = Path.GetDirectoryName(rutaArchivoCSV);
                if (!Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }

                using (StreamWriter writer = new StreamWriter(rutaArchivoCSV, false))
                {
                    // Escribir la cabecera del CSV
                    writer.WriteLine("Número Linea,Municipio Origen,Municipio Destino,Hora Inicial Salida,Intervalo Entre Buses");

                    // Escribir los datos de cada línea
                    foreach (var linea in lineas)
                    {
                        var horaInicialSalida = linea.HoraInicialSalida.ToString(@"hh\:mm");
                        var intervaloEntreBuses = linea.IntervaloEntreBuses.ToString(@"hh\:mm");

                        writer.WriteLine($"{linea.NumeroLinea},{linea.MunicipioOrigen},{linea.MunicipioDestino},{horaInicialSalida},{intervaloEntreBuses}");
                    }
                }
                Console.WriteLine($"Archivo CSV generado exitosamente en: {rutaArchivoCSV}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ocurrió un error al escribir el archivo CSV: {e.Message}");
            }
        }

        public static List<Linea> LeerCSVLineas(string rutaRelativaArchivoCSV)
        {
            List<Linea> lineas = new List<Linea>();

            // Construye la ruta completa al archivo CSV
            var rutaBase = AppDomain.CurrentDomain.BaseDirectory;
            var rutaArchivoCSV = Path.Combine(rutaBase, rutaRelativaArchivoCSV);

            try
            {
                using (StreamReader reader = new StreamReader(rutaArchivoCSV))
                {
                    reader.ReadLine();

                    string lineaCsv;
                    while ((lineaCsv = reader.ReadLine()) != null)
                    {
                        var partes = lineaCsv.Split(',');

                        if (partes.Length == 5)
                        {
                            var nuevaLinea = new Linea
                            {
                                NumeroLinea = int.Parse(partes[0]),
                                MunicipioOrigen = partes[1],
                                MunicipioDestino = partes[2],
                                HoraInicialSalida = TimeSpan.ParseExact(partes[3], @"hh\:mm", CultureInfo.InvariantCulture),
                                IntervaloEntreBuses = TimeSpan.ParseExact(partes[4], @"hh\:mm", CultureInfo.InvariantCulture)
                            };

                            lineas.Add(nuevaLinea);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ocurrió un error al leer el archivo CSV: {e.Message}");
            }

            return lineas;
        }

        public static bool ExisteNumeroLinea(int numeroLinea)
        {
            return lineas.Any(l => l.NumeroLinea == numeroLinea);
        }

        //Logica de Itinerario

        public static void AltaItinerario(Itinerario nuevoItinerario)
        {
            var itinerarioExistente = itinerarios.FirstOrDefault(i => i.NumeroLinea == nuevoItinerario.NumeroLinea);
            if (itinerarioExistente != null)
            {
                itinerarioExistente.Paradas.AddRange(nuevoItinerario.Paradas);
            }
            else
            {
                itinerarios.Add(nuevoItinerario);
            }
            GenerarCSVItinerarios(rutaArchivoItinerariosCSV);
            GuardarEnJson(itinerarios, rutaArchivoItinerariosJson);
            Console.WriteLine("Itinerario actualizado correctamente.");
        }

        public static void BajaItinerario(int numeroLinea)
        {
            var itinerarioIndex = itinerarios.FindIndex(i => i.NumeroLinea == numeroLinea);
            if (itinerarioIndex != -1)
            {
                itinerarios.RemoveAt(itinerarioIndex);
                // Aquí llamas a los métodos para actualizar los archivos CSV y JSON
                GenerarCSVItinerarios(rutaArchivoItinerariosCSV);
                GuardarEnJson(itinerarios, rutaArchivoItinerariosJson);
                Console.WriteLine("Itinerario eliminado correctamente.");
            }
            else
            {
                Console.WriteLine("Error: El itinerario no existe.");
            }
        }

        public static void ModificarItinerario(int numeroLinea, List<ParadaItinerario> paradasActualizadas)
        {
            try
            {
                var itinerario = itinerarios.FirstOrDefault(l => l.NumeroLinea == numeroLinea);

                if (itinerario != null)
                {
                    foreach (var nuevaParada in paradasActualizadas)
                    {
                        var paradaExistente = itinerario.Paradas.FirstOrDefault(p => p.Municipio == nuevaParada.Municipio);

                        if (paradaExistente != null)
                        {
                            paradaExistente.IntervaloDesdeSalida = nuevaParada.IntervaloDesdeSalida;
                            paradaExistente.Municipio = nuevaParada.Municipio;
                        }
                        else
                        {
                            itinerario.Paradas.Add(nuevaParada);
                        }
                    }

                    GenerarCSVItinerarios(rutaArchivoItinerariosCSV);
                    GuardarEnJson(itinerarios, rutaArchivoItinerariosJson);

                    MessageBox.Show("Itinerario modificado correctamente.");
                }
                else
                {
                    MessageBox.Show("El itinerario no existe para la línea especificada.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar el itinerario: {ex.Message}");
            }
        }

        public static List<Itinerario> ConsultarItinerarios(int? numeroLinea = null)
        {
            var resultados = numeroLinea.HasValue ? itinerarios.Where(i => i.NumeroLinea == numeroLinea.Value).ToList() : itinerarios;
            return resultados;
        }

        public static void GenerarCSVItinerarios(string rutaArchivo)
        {
            try
            {
                using (var writer = new StreamWriter(rutaArchivo, false))
                {
                    // Escribir la cabecera del CSV
                    writer.WriteLine("NumeroLinea,Municipio,IntervaloDesdeSalida");

                    // Iterar sobre cada itinerario
                    foreach (var itinerario in itinerarios)
                    {
                        // Iterar sobre cada parada del itinerario
                        foreach (var parada in itinerario.Paradas)
                        {
                            writer.WriteLine($"{itinerario.NumeroLinea},{parada.Municipio},{parada.IntervaloDesdeSalida:hh\\:mm}");
                        }
                    }
                }

                Console.WriteLine($"Archivo CSV guardado en: {rutaArchivo}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error al guardar el archivo CSV: {e.Message}");
            }
        }

        public static void AgregarParadaAItinerario(int numeroLinea, ParadaItinerario nuevaParada)
        {
            var itinerario = itinerarios.FirstOrDefault(i => i.NumeroLinea == numeroLinea);
            if (itinerario == null)
            {
                itinerario = new Itinerario
                {
                    NumeroLinea = numeroLinea,
                    Paradas = new List<ParadaItinerario>()
                };
                itinerarios.Add(itinerario);
            }
            itinerario.Paradas.Add(nuevaParada);

            GenerarCSVItinerarios(rutaArchivoItinerariosCSV);
            GuardarEnJson(itinerarios, rutaArchivoItinerariosJson);
        }

        public static List<Itinerario> LeerCSVItinerarios(string rutaRelativaArchivoCSV)
        {
            var itinerariosAgrupados = new Dictionary<int, Itinerario>();
            var rutaArchivoCSV = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rutaRelativaArchivoCSV);

            try
            {
                using (var reader = new StreamReader(rutaArchivoCSV))
                {
                    reader.ReadLine(); 
                    string linea;
                    while ((linea = reader.ReadLine()) != null)
                    {
                        var partes = linea.Split(',');
                        if (partes.Length >= 3)
                        {
                            int numeroLinea = int.Parse(partes[0]);
                            string municipio = partes[1];
                            TimeSpan intervaloDesdeSalida = TimeSpan.ParseExact(partes[2], @"hh\:mm", CultureInfo.InvariantCulture);

                            if (!itinerariosAgrupados.ContainsKey(numeroLinea))
                            {
                                itinerariosAgrupados[numeroLinea] = new Itinerario
                                {
                                    NumeroLinea = numeroLinea,
                                    Paradas = new List<ParadaItinerario>()
                                };
                            }

                            itinerariosAgrupados[numeroLinea].Paradas.Add(new ParadaItinerario
                            {
                                Municipio = municipio,
                                IntervaloDesdeSalida = intervaloDesdeSalida
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ocurrió un error al leer el archivo CSV de itinerarios: {e.Message}");
            }

            return itinerariosAgrupados.Values.ToList();
        }

        private static T CargarDesdeJson<T>(string rutaArchivo)
        {
            if (File.Exists(rutaArchivo))
            {
                string json = File.ReadAllText(rutaArchivo);
                return JsonSerializer.Deserialize<T>(json);
            }
            return default;
        }

        private static void GuardarEnJson<T>(T objeto, string rutaArchivo)
        {
            string json = JsonSerializer.Serialize(objeto, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(rutaArchivo, json);
        }

        public List<string> LeerNombresMunicipios(string rutaArchivoCSV)
        {
            List<string> nombresMunicipios = new List<string>();

            using (StreamReader reader = new StreamReader(rutaArchivoCSV))
            {
                string linea;
                while ((linea = reader.ReadLine()) != null)
                {
                   
                    string[] campos = linea.Split(';');

                    nombresMunicipios.Add(campos[1]);
                }
            }

            return nombresMunicipios;
        }

        public List<string> ObtenerMunicipiosDeItinerario(int numeroLinea)
        {
            var itinerario = itinerarios.FirstOrDefault(i => i.NumeroLinea == numeroLinea);
            if (itinerario != null)
            {
                return itinerario.Paradas.Select(parada => parada.Municipio).ToList();
            }
            else
            {
                Console.WriteLine("Error: El itinerario no existe.");
                return new List<string>(); // Devolver una lista vacía en caso de no encontrar el itinerario
            }
        }
    }
}

