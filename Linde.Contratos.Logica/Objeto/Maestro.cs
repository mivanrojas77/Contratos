using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linde.Contratos.LogicaV2.Objeto
{
    public class Maestro
    {
        public int Id { get; set; }
        public int IdCompania { get; set; }
        public int TipoCliente { get; set; }
        public string Cliente {get;set;}
        public string Identificacion { get; set; }
        public string Correo { get; set; }
    }
}
