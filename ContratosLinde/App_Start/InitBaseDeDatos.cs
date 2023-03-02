using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContratosLinde.App_Start
{
    public class InitBaseDeDatos
    {
        public bool InitBase() {
            return  Linde.Contratos.LogicaV2.Servicio.InitBase();
        }

        public bool PingBase() {
            return Linde.Contratos.LogicaV2.Servicio.Ping();
        }
    }
}