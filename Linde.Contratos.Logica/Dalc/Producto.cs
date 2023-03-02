using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linde.Contratos.LogicaV2.Dalc
{
    class Producto
    {
        public void ListarProducto(ref List<Objeto.Producto> lista) {
            var salida = new List<Objeto.Producto>();
            salida.Add(new Objeto.Producto() {IdProducto = "-1", Descripcion = "Seleccione" });
            string sql = @"SELECT ARTCODE, ARTCODE || ' - ' || ARTDSCCORTA AS ARTICULO FROM CS2000.stkarticulo WHERE EMPID = 21";

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
                        salida.Add(new Objeto.Producto() { IdProducto= lector["ARTCODE"].ToString(), Descripcion = lector["ARTICULO"].ToString() });
                    }
                }
                lista = salida;
            }
            catch
            {
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
    }
}
