using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linde.Contratos.LogicaV2.Dalc
{
    class TipoAlerta
    {
        public List<Objeto.TipoAlerta> ObtenerTipoAlerta()
        {
            List<Objeto.TipoAlerta> salida = new List<Objeto.TipoAlerta>();
            string sql = @"select id, descripcion_alerta, observacion, mensaje from SCB_TIPO_ALERTA where activo = 0";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    var lector = cm.ExecuteReader();

                    while (lector.Read())
                    {
                        Objeto.TipoAlerta tAl = new Objeto.TipoAlerta();
                        tAl.Id = lector.GetInt32(0);
                        tAl.Observacion = lector.GetString(2);
                        tAl.Descripcion = lector.GetString(1);
                        tAl.Mensaje = lector.GetString(3);
                        salida.Add(tAl);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.Out.WriteLine(ex.ToString());
                salida = new List<Objeto.TipoAlerta>();
            }
            finally
            {
                if (cx != null)
                {
                    cx.Close();
                    cx.Dispose();
                }
            }
            return salida;
        }
    }
}
