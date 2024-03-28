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
    /// Lógica de interacción para ModificarLineaWindow.xaml
    /// </summary>
    public partial class ModificarLineaWindow : Window
    {
        private Linea _lineaSeleccionada;
        public ModificarLineaWindow(Linea lineaSeleccionada)
        {
            InitializeComponent();
            _lineaSeleccionada = lineaSeleccionada;
        }
    }
}
