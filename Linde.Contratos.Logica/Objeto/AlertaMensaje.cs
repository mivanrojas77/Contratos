using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linde.Contratos.LogicaV2.Objeto
{
    public class AlertaMensaje
    {
        public int Id { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int IdTipoAlerta { get; set; }
        public string  DesTipoAlerta { get; set; }
        public int Leido { get; set; }
        public string Mensaje { get; set; }
        public int IdMaestro { get; set; }
        public int IdContrato { get; set; }
        public string FechaRegistroTexto {
            get{
                return this.FechaRegistro.ToShortDateString();
            } 
        }

    }
}
