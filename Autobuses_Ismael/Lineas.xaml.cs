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
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class Lineas : Window
    {
        private LogicaNegocio logicaNegocio;
        private static string rutaRelativaArchivoCSV = "Datos/lineas.csv";
        public Lineas()
        {
            InitializeComponent();
            logicaNegocio = new LogicaNegocio();
            RefrescarListaLineas();
        }

        private void Alta_Click(object sender, RoutedEventArgs e)
        {
            var altaLineaWindow = new AltaLineaWindow();
            altaLineaWindow.ShowDialog();
        }

        private void Modificar_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxLineas.SelectedItem == null)
            {
                MessageBox.Show("Por favor, seleccione una línea para modificar.");
                return;
            }

            Linea lineaSeleccionada = listBoxLineas.SelectedItem as Linea;

            if (lineaSeleccionada != null)
            {
                var modificarLineaWindow = new ModificarLineaWindow(lineaSeleccionada);
                modificarLineaWindow.ShowDialog();

                // Refrescar la lista después de cerrar la ventana de modificación
                // Esto asume que la ventana de modificación guarda los cambios directamente
                RefrescarListaLineas();

            }
        }

        private void Refrescar_Click(object sender, RoutedEventArgs e)
        {
            txtNumeroLinea.Text = "";
            listBoxLineas.ItemsSource = LogicaNegocio.LeerCSVLineas(rutaRelativaArchivoCSV);
        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxLineas.SelectedItem == null)
            {
                MessageBox.Show("Por favor, seleccione una línea para eliminar.");
                return;
            }

            // Obtiene la línea seleccionada
            Linea lineaSeleccionada = listBoxLineas.SelectedItem as Linea;

            if (lineaSeleccionada != null)
            {
                LogicaNegocio.BajaLinea(lineaSeleccionada.NumeroLinea);

                RefrescarListaLineas();
            }
        }

        private void RefrescarListaLineas()
        {         
            listBoxLineas.ItemsSource = LogicaNegocio.ConsultarLineas();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            if (!string.IsNullOrWhiteSpace(txtNumeroLinea.Text) && int.TryParse(txtNumeroLinea.Text, out int numeroLinea))
            {
                var lineasFiltradas = LogicaNegocio.ConsultarLineas(numeroLinea);
                listBoxLineas.ItemsSource = lineasFiltradas;
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un número de línea válido.");
            }
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
}
