using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
namespace ContratosLinde.App_Start
{
    public class InitBasicas
    {

        public static List<Linde.Contratos.LogicaV2.Objeto.Cliente> ListaGeneralCliente; 
        public static void Init()
        {
            Thread _ejecutorListaCliente = new Thread(ProcesosP.CargarListaClientes);
            if (Linde.Contratos.LogicaV2.Servicio.Ping())
                _ejecutorListaCliente.Start();
        }
    }
    class ProcesosP
    {
        public static void CargarListaClientes()
        {
            Linde.Contratos.LogicaV2.Servicio.ListarCliente(ref InitBasicas.ListaGeneralCliente);
        }
    }
}