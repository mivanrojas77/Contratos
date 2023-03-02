using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linde.Contratos.LogicaV2.Dalc
{
    class ContratoRuta
    {
        public List<Objeto.ContratoRuta> ListarContratoRuta() {
            List<Objeto.ContratoRuta> salida = new List<Objeto.ContratoRuta>();

            string sql = "";

            sql = "SELECT CON.ID, CON.ID_CONTRATO, CON.ID_RUTA, CON.FECHA_REGISTRO, CON.ACCION, CON.ACTUAL, RUTA.ETAPA, CLI.RAZON__CLI AS CLIENTE FROM SCH_CONTRATO_RUTA CON inner join scb_ruta_pay ruta on (CON.ID_RUTA = ruta.ID) inner join sch_contrato_maestro mas on (CON.ID_CONTRATO = MAS.ID_CONTRATO_MAESTRO) inner join m_df01 cli on(CLI.CLIENT_CLI = MAS.ID_CLIENTE) WHERE CON.ACTUAL = 1 ";

            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    OracleDataReader lector = cm.ExecuteReader();

                    while (lector.Read())
                    {
                        salida.Add(new Objeto.ContratoRuta()
                        {
                            Id = int.Parse(lector["ID"].ToString()),
                            idContrato = int.Parse(lector["ID_CONTRATO"].ToString()),
                            IdRuta = int.Parse(lector["ID_RUTA"].ToString()),
                            FechaRegistro = lector.GetDateTime(3), //FECHA_REGISTRO
                            Accion = lector["ACCION"].ToString(),
                            Actual = int.Parse(lector["ACTUAL"].ToString()),
                            NombreRuta = lector["ETAPA"].ToString(),
                            Cliente = lector["CLIENTE"].ToString()
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

        public List<Objeto.ContratoRuta> ListarContratoRuta(string id_contrato)
        {
            List<Objeto.ContratoRuta> salida = new List<Objeto.ContratoRuta>();

            string sql = "";

            sql = "SELECT ID, ID_CONTRATO, ID_RUTA, FECHA_REGISTRO, ACCION, ACTUAL FROM SCH_CONTRATO_RUTA WHERE ID_CONTRATO = :P0 ";

            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", id_contrato));
                    OracleDataReader lector = cm.ExecuteReader();

                    while (lector.Read())
                    {
                        salida.Add(new Objeto.ContratoRuta()
                        {
                            Id = int.Parse(lector["ID"].ToString()),
                            idContrato = int.Parse(lector["ID_CONTRATO"].ToString()),
                            IdRuta = int.Parse(lector["ID_RUTA"].ToString()),
                            FechaRegistro =  lector.GetDateTime(3), //FECHA_REGISTRO
                            Accion = lector["ACCION"].ToString(),
                            Actual = int.Parse(lector["ACTUAL"].ToString())
                        });
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

        public List<Objeto.ContratoRuta> ListarContratoRuta(string id_contrato, int ruta) {
            List<Objeto.ContratoRuta> salida = new List<Objeto.ContratoRuta>();
            salida = ListarContratoRuta(id_contrato).Where(x => x.IdRuta == ruta).ToList();
            return salida;
        }

        public List<Objeto.ContratoRuta> ListarContratoRuta(DateTime fechaI, DateTime fechaF)
        {
            List<Objeto.ContratoRuta> salida = new List<Objeto.ContratoRuta>();

            string sql = "";

            sql = "SELECT CON.ID, CON.ID_CONTRATO, CON.ID_RUTA, CON.FECHA_REGISTRO, CON.ACCION, CON.ACTUAL, RUTA.ETAPA, CLI.RAZON__CLI AS CLIENTE FROM SCH_CONTRATO_RUTA CON inner join scb_ruta_pay ruta on (CON.ID_RUTA = ruta.ID) inner join sch_contrato_maestro mas on (CON.ID_CONTRATO = MAS.ID_CONTRATO_MAESTRO) inner join m_df01 cli on(CLI.CLIENT_CLI = MAS.ID_CLIENTE) WHERE CON.ACTUAL = 1 AND CON.FECHA_REGISTRO BETWEEN :P0 AND :P1 ";

            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", fechaI));
                    cm.Parameters.Add(new OracleParameter("P1", fechaF));

                    OracleDataReader lector = cm.ExecuteReader();

                    while (lector.Read())
                    {
                        salida.Add(new Objeto.ContratoRuta()
                        {
                            Id = int.Parse(lector["ID"].ToString()),
                            idContrato = int.Parse(lector["ID_CONTRATO"].ToString()),
                            IdRuta = int.Parse(lector["ID_RUTA"].ToString()),
                            FechaRegistro = lector.GetDateTime(3), //FECHA_REGISTRO
                            Accion = lector["ACCION"].ToString(),
                            Actual = int.Parse(lector["ACTUAL"].ToString()),
                            NombreRuta = lector["ETAPA"].ToString(),
                            Cliente = lector["CLIENTE"].ToString()
                        });
                    }
                }
                return salida;
            }
            catch (Exception ex)
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

        public bool AvanzarTarea(string id_contrato, int ruta, string mensaje)
        {
            string condicion = "";
            bool salida = false;
            int nuevaEtapa = 0;
            //1. Actualizar La tarea Actual
            bool actualizaAccion = ActualizarAccionContratoRuta(new Objeto.ContratoRuta()
            {
                Accion = mensaje,
                IdRuta = ruta,
                idContrato = int.Parse( id_contrato)
            });

            var rutaReal = new Dalc.ContratoRuta().ListarContratoRuta().Where(x => x.Id == ruta).FirstOrDefault().IdRuta;




            if (!actualizaAccion)
                return actualizaAccion;
            else
                condicion = Condicion(id_contrato, rutaReal, mensaje);

            nuevaEtapa = CumpleCondicion(rutaReal, condicion);

            Objeto.ContratoRuta objeto = new Objeto.ContratoRuta();

            objeto.idContrato = int.Parse(id_contrato);
            objeto.IdRuta = nuevaEtapa;
            objeto.FechaRegistro = DateTime.Now;

            bool inserta = InsertarContratoRuta(objeto);

            bool actual =  ActualizarEstadoContratoRuta(objeto);

            return (inserta == actual == true);
        }

        private string Condicion(string id_contrato, int ruta, string mensaje) {
            string salida = "";
            //1. Traer la condicion de la ruta
            var con = new Dalc.Ruta().ListarRuta().Where(x => x.Id == ruta).FirstOrDefault().Condicion.ToString();
            con = con.Replace("<ID_CONTRATO>", id_contrato.ToString());
            con = con.Replace("<ID_RUTA>", ruta.ToString());
            con = con.Replace("<ACCION>", "'" + mensaje + "'");
            con = con.Replace("&", "AND");
            salida = con;
            return salida;
        }

        private int CumpleCondicion(int ruta, string condicion) {
            int salida = 0;
            string sql = "";
            var lista = new Dalc.Ruta().ListarRuta();

            sql = "SELECT count(*) FROM SCH_CONTRATO_RUTA CON WHERE " + condicion + " AND ACTUAL = 1";

            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    OracleDataReader lector = cm.ExecuteReader();

                    while (lector.Read())
                    {
                        if (lector.GetInt32(0) > 0){
                            salida = int.Parse(lista.Where(x => x.Id == ruta).FirstOrDefault().Puerta1);
                        }
                        else {
                            salida = int.Parse(lista.Where(x => x.Id == ruta).FirstOrDefault().Puerta2);
                        }
                    }
                }
                return salida;
            }
            catch (Exception ex)
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

        public bool InsertarContratoRuta(Objeto.ContratoRuta objeto) {
            bool salida = false;
            int escritor = 0;
            string sql = "";

            salida = ActualizarEstadoContratoRutaCero(objeto);

            if (!salida)
                return salida;

            sql = "INSERT INTO SCH_CONTRATO_RUTA(ID, ID_CONTRATO, ID_RUTA, FECHA_REGISTRO, ACCION, ACTUAL) VALUES(SEQ_SCH_CONTRATO_RUTA.NEXTVAL, :P0, :P1, SYSDATE, :P2, :P3) ";

            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", objeto.idContrato));
                    cm.Parameters.Add(new OracleParameter("P1", objeto.IdRuta));
                    cm.Parameters.Add(new OracleParameter("P2", objeto.Accion));
                    cm.Parameters.Add(new OracleParameter("P3", 1));

                    escritor = cm.ExecuteNonQuery();
                }
                return escritor > 0;
            }
            catch
            {
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

        private bool ActualizarEstadoContratoRuta(Objeto.ContratoRuta objeto)
        {
            int escritor = 0;
            string sql = "";

            sql = "UPDATE SCH_CONTRATO_RUTA SET ACTUAL = 1 WHERE ID_CONTRATO = :P0 and ID_RUTA = :P1";

            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", objeto.idContrato));
                    cm.Parameters.Add(new OracleParameter("P1", objeto.IdRuta));

                    escritor = cm.ExecuteNonQuery();
                }
                return escritor > 0;
            }
            catch
            {
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

        private bool ActualizarEstadoContratoRutaCero(Objeto.ContratoRuta objeto)
        {
            int escritor = 0;
            string sql = "";

            sql = "UPDATE SCH_CONTRATO_RUTA SET ACTUAL = 0 WHERE ID_CONTRATO = :P0 ";

            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", objeto.idContrato));

                    escritor = cm.ExecuteNonQuery();
                }
                return escritor > 0;
            }
            catch
            {
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

        private bool ActualizarAccionContratoRuta(Objeto.ContratoRuta objeto)
        {
            int escritor = 0;
            string sql = "";

            sql = "UPDATE SCH_CONTRATO_RUTA SET ACCION = '" + objeto.Accion  + "', FECHA_REGISTRO = SYSDATE  WHERE ID_CONTRATO = " + objeto.idContrato  + " AND ID = " + objeto.IdRuta;

            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    /*cm.Parameters.Add(new OracleParameter("P0", objeto.Accion));
                    cm.Parameters.Add(new OracleParameter("P1", objeto.idContrato));
                    cm.Parameters.Add(new OracleParameter("P2", objeto.IdRuta));*/

                    escritor = cm.ExecuteNonQuery();
                }
                return escritor > 0;
            }
            catch
            {
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
