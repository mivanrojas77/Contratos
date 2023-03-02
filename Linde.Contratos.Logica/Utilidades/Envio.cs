using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linde.Contratos.LogicaV2.Utilidades
{
    public class Envio
    {
        /// <summary>
        /// Método de envío de correo
        /// </summary>
        /// <param name="usuario">usar PAGOELECTRONICO</param>
        /// <param name="tipo">usar 99</param>
        /// <param name="destino">Correo de destino</param>
        /// <param name="mensaje">Mensaje que se envía en el correo</param>
        /// <param name="adicional">Object del correo</param>
        /// <returns>True si se envió correo de lo contrario false</returns>
        public bool AdicionarRegistro(string usuario, int tipo, string destino, string mensaje, string adicional)
        {
            bool salida = false;
            string sql = "INSERT INTO DAT_LOG_ENVIO_EMAIL(ID,USUARIO,TIPO,DESTINATARIO,MENSAJE,ADICIONAL,DERROR,DATE_AUD) VALUES(22, :P0, :P1, :P2, :P3, :P4, NULL, :P5)";

            //Utilidades.Conexion c = new Utilidades.Conexion();
            OracleConnection c = new OracleConnection();
            //c = BaseDeDatos.Conexion.ObtieneConexion();
            c = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand com = new OracleCommand())
                {
                    com.Connection = c;
                    com.CommandType = System.Data.CommandType.Text;
                    com.CommandText = sql;
                    com.Parameters.Add("P0", usuario);
                    com.Parameters.Add("P1", tipo);
                    com.Parameters.Add("P2", destino);
                    com.Parameters.Add("P3", mensaje);
                    com.Parameters.Add("P4", adicional);
                    com.Parameters.Add("P5", DateTime.Now);

                    int d = com.ExecuteNonQuery();
                    salida = (d > 0);
                    com.Dispose();
                    c.Close();
                }
                return salida;

            }
            catch (Exception ex)
            {
                return false;
            }



        }
    }
}
