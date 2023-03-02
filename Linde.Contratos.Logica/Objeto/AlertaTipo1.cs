using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linde.Contratos.LogicaV2.Objeto
{
    class AlertaTipo1
    {
        public int IdMaestro { get; set; }
        public DateTime FechaFinaliacion { get; set; }
        public String NombreCliente { get; set; }
        public String IdCliente { get; set; }
        public String Nit { get; set; }
        public int TipoCliente { get; set; }
        public String NombreTipoCliente { get; set; }
        public String NumeroRubroPresupuestal { get; set; }

    }
}
