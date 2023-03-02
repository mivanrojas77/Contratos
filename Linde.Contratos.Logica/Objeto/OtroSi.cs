using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linde.Contratos.LogicaV2.Objeto
{
    public class OtroSi
    {
        public int Id { get; set; }
        public int IdContrato { get; set; }
        public DateTime FechaActa { get; set; }
        public string FechaActaS { get; set; }
        public double TotalAdiciones { get; set; }
        public int TotalProrrogaMes { get; set; }

    }
}
