using Autobuses_Ismael.Modelos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Autobuses_Ismael
{
    public partial class UserControlItinerario : UserControl
    {
        private Itinerario _itinerarioParaEditar;
        private bool _esModoEdicion = false;
        private static string rutaArchivoItinerariosCSV = "Datos/itinerarios.csv";
        private static string rutaRelativaArchivoCSV = "Datos/lineas.csv";

        public UserControlItinerario()
        {
            InitializeComponent();
            CargarMunicipiosDesdeCSV();
        }

        public Itinerario ItinerarioParaEditar
        {
            set
            {
                _itinerarioParaEditar = value;
                if (_itinerarioParaEditar != null)
                {
                    EsModoEdicion = true;
                    CargarDatosItinerario(_itinerarioParaEditar);
                }
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int numeroLinea = int.Parse(txtNumeroLinea.Text);
                string municipio = cbMunicipio.SelectedItem?.ToString();
                if (!TimeSpan.TryParseExact(txtIntervaloDesdeSalida.Text, "hh\\:mm", CultureInfo.InvariantCulture, out TimeSpan intervaloDesdeSalida))
                {
                    MessageBox.Show("El intervalo desde salida no tiene un formato válido. Por favor, ingrese el tiempo en formato hh:mm.");
                    return;
                }

                if (!LogicaNegocio.ExisteNumeroLinea(numeroLinea))
                {
                    MessageBox.Show("El número de línea no existe. Por favor, ingrese un número de línea válido.");
                    return;
                }

                var nuevaParada = new ParadaItinerario
                {
                    Municipio = municipio,
                    IntervaloDesdeSalida = intervaloDesdeSalida
                };

                if (_esModoEdicion)
                {
                    LogicaNegocio.ModificarItinerario(numeroLinea, new List<ParadaItinerario> { nuevaParada });
                }
                else
                {
                    LogicaNegocio.AgregarParadaAItinerario(numeroLinea, nuevaParada);
                }

                MessageBox.Show("Itinerario guardado correctamente.");
                var window = Window.GetWindow(this);
                window?.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el itinerario: " + ex.Message);
            }
        }

        private void CargarMunicipiosDesdeCSV()
        {
            var rutaArchivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Municipios.csv");
            List<string> municipios = new List<string>();

            using (StreamReader reader = new StreamReader(rutaArchivo))
            {
                string linea;
                while ((linea = reader.ReadLine()) != null)
                {
                    municipios.Add(linea.Split(';')[1]);
                }
            }

            cbMunicipio.ItemsSource = municipios;
        }

        private void CargarDatosItinerario(Itinerario itinerario)
        {
            if (itinerario != null && itinerario.Paradas.Any())
            {
                var destino = itinerario.Paradas.LastOrDefault();
                txtNumeroLinea.Text = itinerario.NumeroLinea.ToString();
                cbMunicipio.SelectedItem = destino?.Municipio;
                txtIntervaloDesdeSalida.Text = destino?.IntervaloDesdeSalida.ToString(@"hh\:mm");

                // Deshabilita el txtNumeroLinea si estás en modo edición
                txtNumeroLinea.IsEnabled = !_esModoEdicion;
            }
        }

        public bool EsModoEdicion
        {
            get { return _esModoEdicion; }
            set
            {
                _esModoEdicion = value;
                ActualizarInterfazModoEdicion();
            }
        }

        private void ActualizarInterfazModoEdicion()
        {
            if (_esModoEdicion && _itinerarioParaEditar != null)
            {
                CargarMunicipiosDelItinerarioSeleccionado(_itinerarioParaEditar);
                btnGuardar.Content = "Actualizar Itinerario";
            }
            else
            {
                CargarMunicipiosDesdeCSV(); // Solo carga todos los municipios si está creando un nuevo itinerario
                btnGuardar.Content = "Crear Itinerario";
            }
        }

        private void CargarMunicipiosDelItinerarioSeleccionado(Itinerario itinerario)
        {
            // Asume que cada parada tiene un municipio único, ajusta según sea necesario
            var municipios = itinerario.Paradas.Select(parada => parada.Municipio).Distinct().ToList();
            cbMunicipio.ItemsSource = municipios;

            // Opcionalmente, selecciona el primer municipio si deseas preseleccionar un valor
            if (municipios.Any())
            {
                cbMunicipio.SelectedIndex = 0;
            }
        }
    }
}
