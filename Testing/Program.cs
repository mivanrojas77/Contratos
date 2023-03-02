using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Testing
{
    class Program
    {
        //public static List<Linde.ContratosContext.Logica.Objeto.Cliente> ListaGeneralCliente = new List<Linde.ContratosContext.Logica.Objeto.Cliente>();
        //static void Main(string[] args)GuardarDetalle
        //{
        //    Init();
        //    System.Threading.Thread.Sleep(5000);

        //    foreach (var l in ListaGeneralCliente) {
        //        Console.WriteLine("Cliente:" + l.Descripcion);
        //    }
        //    Console.Read();

        //}0-
        static void Main(string[] args) {
            //var ejec1 = Linde.Contratos.LogicaV2.Servicio.EjecutarAlertasTipo1(21);
            var ejec2 = Linde.Contratos.LogicaV2.Servicio.EjecutarAlertasTipo2(21);
        }

        private static void FijarFechas() {
            
        }

        


    }
}
