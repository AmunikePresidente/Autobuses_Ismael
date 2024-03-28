using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autobuses_Ismael.Modelos
{
    public class Linea
    {
        public int NumeroLinea { get; set; }
        public string MunicipioOrigen { get; set; }
        public string MunicipioDestino { get; set; }
        public TimeSpan HoraInicialSalida { get; set; }
        public TimeSpan IntervaloEntreBuses { get; set; }

        public override string ToString()
        {
            return $"Línea {NumeroLinea}: De {MunicipioOrigen} a {MunicipioDestino} - Salida: {HoraInicialSalida}, Intervalo: {IntervaloEntreBuses}";
        }
    }
}
