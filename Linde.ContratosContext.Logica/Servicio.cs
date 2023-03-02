using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linde.ContratosContext.Logica
{
    public class Servicio
    {
        public static void ListarCliente(ref List<Objeto.Cliente> lista)
        {
            new Dalc.Cliente().ListarClientes(true, ref lista);
            return;
        }
    }
}
