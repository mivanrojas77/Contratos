using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;


namespace ContratosLinde.App_Start
{
    public class InitProducto
    {
        public static List<Linde.Contratos.LogicaV2.Objeto.Producto> ListaGeneralProducto;
        public static void Init()
        {
            Thread _ejecutorListaCliente = new Thread(Procesos.CargarListaProductos);
            _ejecutorListaCliente.Start();
        }
    }
    class Procesos
    {
        public static void CargarListaProductos()
        {
            Linde.Contratos.LogicaV2.Servicio.ListarProducto(ref InitProducto.ListaGeneralProducto);
        }
    }
}