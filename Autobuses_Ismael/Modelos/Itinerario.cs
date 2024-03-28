using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autobuses_Ismael.Modelos
{
    public class ParadaItinerario
    {
        public string Municipio { get; set; }
        public TimeSpan IntervaloDesdeSalida { get; set; }

        public override string ToString()
        {
            return $"{Municipio} - {IntervaloDesdeSalida.ToString(@"hh\:mm")}";
        }
    }

    public class Itinerario
    {
        public int NumeroLinea { get; set; }
        public Linea LineaAsociada { get; set; }
        public List<ParadaItinerario> Paradas { get; set; } = new List<ParadaItinerario>();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Línea {NumeroLinea}: ");

            // Calcular la suma del intervalo entre buses de la línea
            TimeSpan intervaloEntreBuses = LineaAsociada.IntervaloEntreBuses;

            // Calcular la suma del intervalo desde la última parada hasta el destino
            TimeSpan intervaloUltimaParadaHastaDestino = Paradas.LastOrDefault()?.IntervaloDesdeSalida ?? new TimeSpan(0);

            // Calcular el intervalo total hasta el destino
            TimeSpan intervaloHastaDestino = intervaloEntreBuses + intervaloUltimaParadaHastaDestino;

            for (int i = 0; i < Paradas.Count; i++)
            {
                if (i > 0) sb.Append(" hacia ");
                sb.Append($"{Paradas[i].Municipio} - En {Paradas[i].IntervaloDesdeSalida.ToString(@"hh\:mm")} horas ");
            }

            // Agregar el municipio de destino de la línea y el intervalo hasta el destino
            sb.Append($" - Destino: {LineaAsociada?.MunicipioDestino ?? "Desconocido"} - En {intervaloHastaDestino.ToString(@"hh\:mm")} horas");

            return sb.ToString();
        }
    }
}
