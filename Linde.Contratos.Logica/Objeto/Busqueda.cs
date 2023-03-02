using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linde.Contratos.LogicaV2.Objeto
{
    public class Busqueda
    {
        public int IdMaestro { get; set; }
        public string Registro { get; set; }

        public int IdContratoProducto { get; set; }

        public string Producto { get; set; } 

        public string Alerta { get; set; }
    }
}
