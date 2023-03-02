using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linde.Contratos.LogicaV2.Dalc
{
    class Ruta
    {
        public List<Objeto.Ruta> ListarRuta() {
            List<Objeto.Ruta> salida = new List<Objeto.Ruta>();

            string sql = "";

            sql = "SELECT ID, ETAPA, TEXTO, CONDICION, PUERTA1, PUERTA2, APLICA_CORREO, TEXTO_CORREO, APLICA_COMENTARIO FROM scb_ruta_pay ruta ";

            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    OracleDataReader lector = cm.ExecuteReader();

                    while (lector.Read())
                    {
                        salida.Add(new Objeto.Ruta()
                        {
                            Id = int.Parse(lector["ID"].ToString()),
                            Etapa = lector["ETAPA"].ToString(),
                            Texto = lector["TEXTO"].ToString(),
                            Condicion = lector["CONDICION"].ToString(),
                            Puerta1 = lector["PUERTA1"].ToString(),
                            Puerta2 = lector["PUERTA2"].ToString(),
                            AplicaCorreo = int.Parse(lector["APLICA_CORREO"].ToString()),
                            TextoCorreo = lector["TEXTO_CORREO"].ToString(),
                            AplicaComentario = int.Parse(lector["APLICA_COMENTARIO"].ToString())
                        });
                    }
                }
                return salida;
            }
            catch(Exception ex)
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
