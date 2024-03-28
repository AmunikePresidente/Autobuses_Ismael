using Autobuses_Ismael.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Autobuses_Ismael
{
    /// <summary>
    /// Lógica de interacción para Itinerarios.xaml
    /// </summary>
    public partial class Itinerarios : Window
    {
        private LogicaNegocio logicaNegocio;
        private static string rutaArchivoItinerariosCSV = "Datos/itinerarios.csv";
        private static string rutaRelativaArchivoCSV = "Datos/lineas.csv";

        public Itinerarios()
        {
            InitializeComponent();
            logicaNegocio = new LogicaNegocio();
            RefrescarListaItinerarios();
        }

        private void Alta_Click(object sender, RoutedEventArgs e)
        {
            var altaItinerarioWindow = new AltaItinerarioWindow();
            altaItinerarioWindow.ShowDialog();

            // Refrescar la lista de itinerarios después de cerrar la ventana de alta
            RefrescarListaItinerarios();
        }

        private void Modificar_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxItinerarios.SelectedItem == null)
            {
                MessageBox.Show("Por favor, seleccione un itinerario para modificar.");
                return;
            }

            Itinerario itinerarioSeleccionado = listBoxItinerarios.SelectedItem as Itinerario;

            if (itinerarioSeleccionado != null)
            {
                // Obtiene los municipios para el itinerario seleccionado
                var municipios = logicaNegocio.ObtenerMunicipiosDeItinerario(itinerarioSeleccionado.NumeroLinea);

                // Crea la ventana de modificación
                var modificarItinerarioWindow = new ModificarItinerarioWindow(itinerarioSeleccionado);

                // Muestra la ventana de modificación como un diálogo modal
                modificarItinerarioWindow.ShowDialog();

                // Refrescar la lista después de cerrar la ventana de modificación
                RefrescarListaItinerarios();
            }
        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxItinerarios.SelectedItem == null)
            {
                MessageBox.Show("Por favor, seleccione un itinerario para eliminar.");
                return;
            }

            Itinerario itinerarioSeleccionado = listBoxItinerarios.SelectedItem as Itinerario;

            if (itinerarioSeleccionado != null)
            {
                LogicaNegocio.BajaItinerario(itinerarioSeleccionado.NumeroLinea);

                RefrescarListaItinerarios();
            }
        }

        private void RefrescarListaItinerarios()
        {
            // Leer tanto las líneas como los itinerarios desde el archivo CSV
            var lineas = LogicaNegocio.LeerCSVLineas(rutaRelativaArchivoCSV);
            var itinerarios = LogicaNegocio.LeerCSVItinerarios(rutaArchivoItinerariosCSV);


            // Asignar manualmente el municipio de destino de la línea correspondiente a cada itinerario
            foreach (var itinerario in itinerarios)
            {
                itinerario.LineaAsociada = lineas.FirstOrDefault(l => l.NumeroLinea == itinerario.NumeroLinea);
            }

            // Asignar los itinerarios a la lista de la interfaz de usuario
            listBoxItinerarios.ItemsSource = itinerarios;
        }

        private void Refrescar_Click(object sender, RoutedEventArgs e)
        {
            txtNumeroLinea.Text = "";
            listBoxItinerarios.ItemsSource = LogicaNegocio.LeerCSVItinerarios(rutaArchivoItinerariosCSV);
        }

        private void listBoxItinerarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtNumeroLinea.Text, out int numeroLinea))
            {
                // Leer tanto las líneas como los itinerarios desde el archivo CSV
                var lineas = LogicaNegocio.LeerCSVLineas(rutaRelativaArchivoCSV);
                var itinerarios = LogicaNegocio.LeerCSVItinerarios(rutaArchivoItinerariosCSV);

                // Encuentra el itinerario específico para el número de línea.
                var itinerarioEspecifico = itinerarios.FirstOrDefault(i => i.NumeroLinea == numeroLinea);

                if (itinerarioEspecifico != null)
                {
                    // Asignar manualmente la línea correspondiente al itinerario encontrado.
                    itinerarioEspecifico.LineaAsociada = lineas.FirstOrDefault(l => l.NumeroLinea == numeroLinea);

                    // Si se encontró la línea asociada y el itinerario, actualiza la lista de la interfaz de usuario.
                    listBoxItinerarios.ItemsSource = new List<Itinerario> { itinerarioEspecifico };
                }
                else
                {
                    // Si no se encuentra un itinerario para ese número de línea, notifica al usuario.
                    MessageBox.Show("No se encontró un itinerario para el número de línea proporcionado.");
                    listBoxItinerarios.ItemsSource = null; // Limpiar la lista si no se encontró nada.
                }
            }
            else
            {
                MessageBox.Show("Por favor, introduzca un número de línea válido.");
            }

            txtNumeroLinea.Clear(); // Limpiar el campo de texto después de la consulta.
        }
    }
}
