using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linde.Contratos.LogicaV2.Objeto
{
    public class Ruta
    {
        public int Id { get; set; }
        public string Etapa { get; set; }
        public string Texto { get; set; }
        public string Condicion { get; set; }
        public string Puerta1 { get; set; }
        public string Puerta2 { get; set; }
        public int AplicaCorreo { get; set; }
        public string TextoCorreo { get; set; }
        public int AplicaComentario { get; set; }
    }
}
