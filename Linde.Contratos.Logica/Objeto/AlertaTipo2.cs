using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linde.Contratos.LogicaV2.Objeto
{
    class AlertaTipo2
    {
        public int IdContratoMaestro { get; set; }
        public string VestorProductos { get; set; }
        public string NumeroContrato { get; set; }
        public string TipoCliente { get; set; }
        public string IdCliente { get; set; }
        public string Nit { get; set; }
        public int Mes { get; set; }
        public int Año { get; set; }
        public double? AsignacionPresupuestal { get; set; }
        public double? Calculo { get; set; }
        public double? Ventas { get; set; }
    }
}
