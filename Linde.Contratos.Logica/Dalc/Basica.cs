using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linde.Contratos.LogicaV2.Dalc
{
    class Basica
    {
        public List<Objeto.Basica> ListarTipoCliente(int compania) {
            var salida = new List<Objeto.Basica>();
            salida.Add(new Objeto.Basica() { Codigo = "-1", Descripcion = "Seleccione" });
            string sql = "";
            
            if (compania == 21)
                sql = "SELECT codde01, deta101 FROM M_tabdes WHERE CODTA01 = 11 order by deta101";
            else if (compania == 22)
                sql = "SELECT codde01, deta101 FROM M_tabdes@COPG WHERE CODTA01 = 11 order by deta101";
            else
                sql = "SELECT codde01, deta101 FROM M_tabdes@COLC WHERE CODTA01 = 11 order by deta101";


            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    OracleDataReader lector = cm.ExecuteReader();

                    while (lector.Read())
                    {
                        salida.Add(new Objeto.Basica() { Codigo = lector["codde01"].ToString(), Descripcion = lector["deta101"].ToString() });
                    }
                }
                return salida;
            }
            catch
            {
                return salida;
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
