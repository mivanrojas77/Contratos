using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linde.Contratos.LogicaV2.Objeto
{
    public class ContratoRuta
    {
        public int Id { get; set; }
        public int idContrato { get; set; }
        public int IdRuta { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Accion { get; set; }
        public int Actual { get; set; }
        public string FechaRegistroTexto { get; set; }
        public string NombreRuta { get; set; }
        public string Cliente { get; set; }
    }
}
