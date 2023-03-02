using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linde.Contratos.LogicaV2.Dalc
{
    class Anexo
    {
        [Obsolete("El método a partir de su versión 2, almacena binarios en la base de datos")]
        public bool CrearAnexo(int idContrato, string infoAnexo) {
            bool salida = false;
            string sql = @"INSERT INTO SCH_CONTRATO_ANEXO(ID, ID_CONTRATO, ANEXO, ACTIVO)
                           VALUES(SEQ_SEH_CONTRATO_ANEXO.NEXTVAL, :P0, :P1, 0)";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", idContrato));
                    cm.Parameters.Add(new OracleParameter("P1", infoAnexo));

                    int escritor = cm.ExecuteNonQuery();

                    salida = (escritor > 0);

                }
            }
            catch (Exception ex)
            {
                System.Console.Out.WriteLine(ex.ToString());
                salida = false;
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

        public bool DescargarAnexo(int idContrato) {
            bool salida = false;
            UInt32 FileSize;
            byte[] rawData;
            FileStream fs;
            String ubicacionLocal = System.Configuration.ConfigurationManager.AppSettings["UbicacionArchivo"];
            string ubicacionWeb = System.Configuration.ConfigurationManager.AppSettings["UrlArchivo"];

            string sql = @"SELECT BINARIO, ANEXO FROM SCH_CONTRATO_ANEXO WHERE ID = :P0";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", idContrato));

                    OracleDataReader lector = cm.ExecuteReader(CommandBehavior.SequentialAccess);

                    while (lector.Read()) {
                        string nombre = lector["ANEXO"].ToString();
                        nombre = nombre.Replace(ubicacionWeb, ubicacionLocal);

                        if (File.Exists(nombre))
                            File.Delete(nombre);

                        var tamaño  = lector.GetInt32(lector["BINARIO"].ToString().Length);
                        rawData = new byte[tamaño];

                        lector.GetBytes(lector.GetOrdinal("BINARIO"), 0, rawData, 0, (int)tamaño);

                        fs = new FileStream(nombre, FileMode.OpenOrCreate, FileAccess.Write);
                        fs.Write(rawData, 0, (int)tamaño);
                        fs.Close();
                        salida = true;
                    }

                }
            }
            catch (Exception ex)
            {
                System.Console.Out.WriteLine(ex.ToString());
                salida = false;
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

        public bool CrearAnexo(int idContrato, string infoAnexo, byte[] archivo)
        {
            bool salida = false;
            string sql = @"INSERT INTO SCH_CONTRATO_ANEXO(ID, ID_CONTRATO, ANEXO, ACTIVO, BINARIO)
                           VALUES(SEQ_SEH_CONTRATO_ANEXO.NEXTVAL, :P0, :P1, 0, :P2)";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", idContrato));
                    cm.Parameters.Add(new OracleParameter("P1", infoAnexo));
                    cm.Parameters.Add("P2", OracleDbType.Blob, archivo, ParameterDirection.Input);

                    int escritor = cm.ExecuteNonQuery();

                    salida = (escritor > 0);

                }
            }
            catch (Exception ex)
            {
                System.Console.Out.WriteLine(ex.ToString());
                salida = false;
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

        public List<Objeto.Anexo> ObtenerAnexos(int idContrato) {
            List<Objeto.Anexo> salida = new List<Objeto.Anexo>();
            string sql = @"SELECT ID, ID_CONTRATO, ANEXO FROM SCH_CONTRATO_ANEXO WHERE ID_CONTRATO = :P0 AND ACTIVO = 0";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", idContrato));

                    var lector = cm.ExecuteReader(CommandBehavior.SequentialAccess);

                    while (lector.Read()) {
                        Objeto.Anexo anx = new Objeto.Anexo();
                        anx.Id = lector.GetInt32(0);
                        anx.IdContrato = lector.GetInt32(1);
                        anx.InfoAnexo = lector.GetString(2);
                        salida.Add(anx);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.Out.WriteLine(ex.ToString());
                salida = new List<Objeto.Anexo>();
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

        public bool EliminarAnexo(int id) {
            bool salida = false;
            string sql = @"UPDATE SCH_CONTRATO_ANEXO SET ACTIVO = 1 WHERE ID = :P0";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", id));
                    int escritor = cm.ExecuteNonQuery();
                    salida = (escritor > 0);
                }
            }
            catch (Exception ex)
            {
                System.Console.Out.WriteLine(ex.ToString());
                salida = false;
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
