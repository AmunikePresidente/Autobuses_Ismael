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
            //PREPARACI�N
            var nuevaLinea = new Linea
            {
                NumeroLinea = 1,
                MunicipioOrigen = "OrigenTest",
                MunicipioDestino = "DestinoTest",
                HoraInicialSalida = System.TimeSpan.FromHours(8),
                IntervaloEntreBuses = System.TimeSpan.FromMinutes(30)
            };

            //ACCI�N
            LogicaNegocio.AltaLinea(nuevaLinea);


            //VERIFICACI�N
            var resultado = LogicaNegocio.ConsultarLineas(1).FirstOrDefault();

            Assert.That(resultado, Is.Not.Null);
            Assert.That(resultado.MunicipioOrigen, Is.EqualTo("OrigenTest"), "El MunicipioOrigen no coincide.");
            Assert.That(resultado.MunicipioDestino, Is.EqualTo("DestinoTest"), "El MunicipioDestino no coincide.");
        }
    
    //Este caso de prueba unitaria consiste en comprobar que la consulta no este  vacia y que si se ha escogido una linea esta exista.
    [Test]
    public void ConsultarLineasRetornaListaCorrecta()
    {
            //PREPARACI�N
            var nuevaLinea = new Linea
            {
                NumeroLinea = 1,
                MunicipioOrigen = "OrigenTest",
                MunicipioDestino = "DestinoTest",
                HoraInicialSalida = System.TimeSpan.FromHours(8),
                IntervaloEntreBuses = System.TimeSpan.FromMinutes(30)
            };

            //VERIFICACI�N
            var resultado = LogicaNegocio.ConsultarLineas(1);

            Assert.That(resultado, Is.Not.Empty, "La consulta no deber�a estar vac�a.");
            Assert.That(resultado.Any(l => l.NumeroLinea == 1), Is.True, "Deber�a encontrar la l�nea agregada en la prueba anterior.");
        }
    }
}