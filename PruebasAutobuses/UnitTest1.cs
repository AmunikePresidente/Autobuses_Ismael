using Autobuses_Ismael.Modelos;

namespace PruebasAutobuses
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }


        //Este caso de prueba unitaria consiste en comprobar que a la hora de dar de alta una linea de autobuses esta se crea y contiene los municipios  correctos.
        [Test]
        public void AltaLineaAgregaCorrectamente()
        {
            //PREPARACIÓN
            var nuevaLinea = new Linea
            {
                NumeroLinea = 1,
                MunicipioOrigen = "OrigenTest",
                MunicipioDestino = "DestinoTest",
                HoraInicialSalida = System.TimeSpan.FromHours(8),
                IntervaloEntreBuses = System.TimeSpan.FromMinutes(30)
            };

            //ACCIÓN
            LogicaNegocio.AltaLinea(nuevaLinea);


            //VERIFICACIÓN
            var resultado = LogicaNegocio.ConsultarLineas(1).FirstOrDefault();

            Assert.That(resultado, Is.Not.Null);
            Assert.That(resultado.MunicipioOrigen, Is.EqualTo("OrigenTest"), "El MunicipioOrigen no coincide.");
            Assert.That(resultado.MunicipioDestino, Is.EqualTo("DestinoTest"), "El MunicipioDestino no coincide.");
        }
    
    //Este caso de prueba unitaria consiste en comprobar que la consulta no este  vacia y que si se ha escogido una linea esta exista.
    [Test]
    public void ConsultarLineasRetornaListaCorrecta()
    {
            //PREPARACIÓN
            var nuevaLinea = new Linea
            {
                NumeroLinea = 1,
                MunicipioOrigen = "OrigenTest",
                MunicipioDestino = "DestinoTest",
                HoraInicialSalida = System.TimeSpan.FromHours(8),
                IntervaloEntreBuses = System.TimeSpan.FromMinutes(30)
            };

            //VERIFICACIÓN
            var resultado = LogicaNegocio.ConsultarLineas(1);

            Assert.That(resultado, Is.Not.Empty, "La consulta no debería estar vacía.");
            Assert.That(resultado.Any(l => l.NumeroLinea == 1), Is.True, "Debería encontrar la línea agregada en la prueba anterior.");
        }
    }
}