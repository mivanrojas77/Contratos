using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linde.Contratos.LogicaV2.Dalc
{
    class Rol
    {
        public List<Objeto.Rol> ObteneRoles() {
            List<Objeto.Rol> salida = new List<Objeto.Rol>();

            string sql = "";

            sql = "SELECT ID, USUARIO, PERMISO FROM SCH_ROL";

            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    OracleDataReader lector = cm.ExecuteReader();

                    while (lector.Read())
                    {
                        salida.Add(new Objeto.Rol() { Id= int.Parse(lector["ID"].ToString()), Usuario = lector["USUARIO"].ToString(), Permiso = int.Parse(lector["PERMISO"].ToString())});
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
