using Autobuses_Ismael.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Autobuses_Ismael
{
    /// <summary>
    /// Lógica de interacción para UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {

        public UserControl1()
        {
            InitializeComponent();
            CargarMunicipiosDesdeCSV();
        }

        private Linea _lineaOriginal;

        public Linea LineaParaEditar
        {
            set
            {
                if (value != null)
                {
                    _lineaOriginal = value;
                    CargarDatosLinea(value);
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int numeroLinea = int.Parse(Lineatxt.Text);

                var lineaActualizada = new Linea
                {
                    NumeroLinea = numeroLinea,
                    MunicipioOrigen = MSalidacb.SelectedItem.ToString(),
                    MunicipioDestino = MDestinocb.SelectedItem.ToString(),
                    HoraInicialSalida = TimeSpan.Parse(HorarioStxt.Text),
                    IntervaloEntreBuses = TimeSpan.Parse(Intervalotxt.Text)
                };

                if (_lineaOriginal != null)
                {
                    // Si el número de línea ha cambiado y el nuevo número ya existe, mostrar mensaje de error.
                    if (_lineaOriginal.NumeroLinea != numeroLinea && LogicaNegocio.ExisteNumeroLinea(numeroLinea))
                    {
                        MessageBox.Show("El número de línea ya existe. Por favor, elija otro número.");
                        return;
                    }
                    else if (_lineaOriginal.NumeroLinea != numeroLinea)
                    {
                        // Eliminar la línea original y crear una nueva si el número ha cambiado.
                        LogicaNegocio.BajaLinea(_lineaOriginal.NumeroLinea);
                        LogicaNegocio.AltaLinea(lineaActualizada);
                        var window = Window.GetWindow(this);
                        window?.Close();
                        MessageBox.Show("Línea actualizada con nuevo número.");
                    }
                    else
                    {
                        // Si el número de línea no ha cambiado, simplemente actualizar los datos.
                        LogicaNegocio.ModificarLinea(_lineaOriginal.NumeroLinea, lineaActualizada);
                        var window = Window.GetWindow(this);
                        window?.Close();
                        MessageBox.Show("Línea actualizada correctamente.");
                    }
                }
                else
                {
                    // Creando una nueva línea si no estamos en modo de edición.
                    LogicaNegocio.AltaLinea(lineaActualizada);
                    var window = Window.GetWindow(this);
                    window?.Close();
                    MessageBox.Show("Nueva línea creada correctamente.");
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Por favor, asegúrese de que todos los campos están correctamente llenados y el formato de hora es correcto.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar la línea: " + ex.Message);
            }          
        }

        private void CargarMunicipiosDesdeCSV()
        {
            var rutaArchivo = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Municipios.csv");
            List<string> municipios = new List<string>();

            try
            {
                using (StreamReader reader = new StreamReader(rutaArchivo))
                {
                    string linea;
                    while ((linea = reader.ReadLine()) != null)
                    {
                        var partes = linea.Split(';');
                        municipios.Add(partes[1]);
                    }
                }

                MSalidacb.ItemsSource = municipios;
                MDestinocb.ItemsSource = municipios;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar municipios: {ex.Message}");
            }
        }

        private void CargarDatosLinea(Linea linea)
        {
            if (linea != null)
            {
                Lineatxt.Text = linea.NumeroLinea.ToString();
                HorarioStxt.Text = linea.HoraInicialSalida.ToString(@"hh\:mm");
                Intervalotxt.Text = linea.IntervaloEntreBuses.ToString(@"hh\:mm");
             
                MSalidacb.SelectedValue = linea.MunicipioOrigen;
                MDestinocb.SelectedValue = linea.MunicipioDestino;
            }
        }
    }
}
