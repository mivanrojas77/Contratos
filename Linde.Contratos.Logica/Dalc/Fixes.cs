using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linde.Contratos.LogicaV2.Dalc
{
    class Fixes
    {
        public void AjustarFechaFinalizacion()
        {
            string sql = @"SELECT MAS.ID_CONTRATO_MAESTRO AS MAESTRO, MAX(CON.FECHA_FINALIZACION) AS FECHA
from SCH_CONTRATO_MAESTRO MAS INNER JOIN SCH_CONTRATO CON ON MAS.ID_CONTRATO_MAESTRO = CON.ID_MAESTRO
WHERE CON.FECHA_FINALIZACION != '1/01/0001' AND CON.FECHA_FINALIZACION IS NOT NULL
GROUP BY MAS.ID_CONTRATO_MAESTRO
   HAVING COUNT(CON.FECHA_FINALIZACION) > 1 ORDER BY 1";

            OracleConnection cx = new OracleConnection();
            //cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();
            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    OracleDataReader lector = cm.ExecuteReader();

                    while (lector.Read())
                    {
                        int id = int.Parse(lector["MAESTRO"].ToString());
                        DateTime fecha = DateTime.Parse(lector["FECHA"].ToString());

                        if (!AjustarFecha(id, fecha))
                        {
                            System.Console.Out.WriteLine("Fallo-> id:" + id.ToString() + " fecha:" + fecha.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.Out.WriteLine(ex.ToString());
                return;
            }
            finally
            {
                if (cx != null)
                {
                    cx.Close();
                    cx.Dispose();
                }
            }
        }
        private bool AjustarFecha(int idMaestro, DateTime fechaFinalizacion)
        {
            string sql = @"UPDATE SCH_CONTRATO  SET FECHA_FINALIZACION  = :P0  WHERE ID_MAESTRO = :P1";

            OracleConnection cx = new OracleConnection();
            //cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();
            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", fechaFinalizacion));
                    cm.Parameters.Add(new OracleParameter("P1", idMaestro));

                    int lector = cm.ExecuteNonQuery();

                    return (lector > 0);

                }
            }
            catch (Exception ex)
            {
                System.Console.Out.WriteLine(ex.ToString());
                return false;
            }
            finally
            {
                if (cx != null)
                {
                    cx.Close();
                    cx.Dispose();
                }
            }
        }
    }
}
