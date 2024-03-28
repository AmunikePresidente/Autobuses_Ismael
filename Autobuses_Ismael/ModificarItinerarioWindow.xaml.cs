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
    /// Lógica de interacción para ModificarItinerarioWindow.xaml
    /// </summary>
    public partial class ModificarItinerarioWindow : Window
    {
        private Itinerario _itinerarioSeleccionado;

        public ModificarItinerarioWindow(Itinerario itinerarioSeleccionado)
        {
            InitializeComponent();
            _itinerarioSeleccionado = itinerarioSeleccionado;

            // Configura el UserControlItinerario con el itinerario para editar
            userControlItinerario.ItinerarioParaEditar = _itinerarioSeleccionado;
            userControlItinerario.EsModoEdicion = true;
        }
    }
}
