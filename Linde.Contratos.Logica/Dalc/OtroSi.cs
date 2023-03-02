using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linde.Contratos.LogicaV2.Dalc
{
    class OtroSi
    {
        public bool CrearOtroSi(int idContrato, DateTime fechaActa, double totalAdiciones, int totalProrrogaMes)
        {
            bool salida = false;
            
            string sql = @"INSERT INTO SCH_CONTRATO_OTROSI(ID, ID_CONTRATO_PRODUCTO, FECHA_ACTA, TOTAL_ADICIONES, TOTAL_PRORROGA_MES)
                           VALUES(SEQ_SECH_CONTRATO_OTROSI.NEXTVAL, :P0, :P1, :P2, :P3)";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", idContrato));
                    cm.Parameters.Add(new OracleParameter("P1", fechaActa));
                    cm.Parameters.Add(new OracleParameter("P2", totalAdiciones));
                    cm.Parameters.Add(new OracleParameter("P3", totalProrrogaMes));

                    int escritor = cm.ExecuteNonQuery();

                    salida = (escritor > 0);

                    IncrementarDetalle(idContrato, totalAdiciones, totalProrrogaMes);

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

        public List<Objeto.OtroSi> ObtenerOtroSi(int idContrato)
        {
            List<Objeto.OtroSi> salida = new List<Objeto.OtroSi>();
            string sql = @"select otrosi.ID, otrosi.ID_CONTRATO_PRODUCTO, otrosi.FECHA_ACTA, otrosi.TOTAL_ADICIONES, otrosi.TOTAL_PRORROGA_MES
from sch_contrato_otrosi otrosi inner join sch_contrato con on (con.id_CONTRATO = OTROSI.ID_CONTRATO_PRODUCTO)
where con.id_maestro = :P0 and producto is not null";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", idContrato));

                    var lector = cm.ExecuteReader();

                    while (lector.Read())
                    {
                        Objeto.OtroSi anx = new Objeto.OtroSi();
                        anx.Id = lector.GetInt32(0);
                        anx.IdContrato = lector.GetInt32(1);
                        anx.FechaActa = lector.GetDateTime(2);
                        anx.FechaActaS = anx.FechaActa.ToShortDateString(); 
                        anx.TotalAdiciones = lector.GetDouble(3);
                        anx.TotalProrrogaMes = lector.GetInt32(4);
                        salida.Add(anx);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.Out.WriteLine(ex.ToString());
                salida = new List<Objeto.OtroSi>();
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

        public Objeto.OtroSi ObtenerOtroSiPorId(int id)
        {
            Objeto.OtroSi salida = new Objeto.OtroSi();
            string sql = @"select otrosi.ID, otrosi.ID_CONTRATO_PRODUCTO, otrosi.FECHA_ACTA, otrosi.TOTAL_ADICIONES, otrosi.TOTAL_PRORROGA_MES
from sch_contrato_otrosi otrosi where otrosi.ID = :P0";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", id));

                    var lector = cm.ExecuteReader();

                    while (lector.Read())
                    {
                        Objeto.OtroSi anx = new Objeto.OtroSi();
                        anx.Id = lector.GetInt32(0);
                        anx.IdContrato = lector.GetInt32(1);
                        anx.FechaActa = lector.GetDateTime(2);
                        anx.FechaActaS = anx.FechaActa.ToShortDateString();
                        anx.TotalAdiciones = lector.GetDouble(3);
                        anx.TotalProrrogaMes = lector.GetInt32(4);
                        salida = anx;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.Out.WriteLine(ex.ToString());
                salida = null;
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

        public Objeto.Acumulado ObtenerOtroSiAcumulado(int idContrato)
        {
            Objeto.Acumulado salida = new Objeto.Acumulado();
            string sql = @"SELECT MAX(otrosi.FECHA_ACTA - con.FECHA_INICIO) AS DIAS,
                                CON.PRECIO_ESCALADO AS MONTO 
                           FROM SCH_CONTRATO_OTROSI  otrosi 
                                inner join SCH_CONTRATO con on (con.id_contrato = otrosi.ID_CONTRATO_PRODUCTO)
                                inner join SCH_CONTRATO_maestro maestro on (con.id_maestro = maestro.id_contrato_maestro)
                           WHERE ID_CONTRATO_PRODUCTO = :P0
                           GROUP BY CON.PRECIO_ESCALADO";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", idContrato));

                    var lector = cm.ExecuteReader();

                    while (lector.Read())
                    {
                        salida.Dias = int.Parse(lector["DIAS"].ToString());
                        salida.Monto = double.Parse(lector["MONTO"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.Out.WriteLine(ex.ToString());
                salida.Dias = 0;
                salida.Monto = 0;
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

        private void IncrementarDetalle(int IdProducto, double valor, int dias) {
            string sql = @"UPDATE SCH_CONTRATO SET FECHA_FINALIZACION = FECHA_FINALIZACION + :P0, ASIGNACION_PRESUPUESTAL = ASIGNACION_PRESUPUESTAL + :P1   WHERE ID_CONTRATO = :P2";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", dias));
                    cm.Parameters.Add(new OracleParameter("P1", valor));
                    cm.Parameters.Add(new OracleParameter("P2", IdProducto));

                    var lector = cm.ExecuteNonQuery();
                    return;
                }
            }
            catch (Exception ex)
            {
                System.Console.Out.WriteLine(ex.ToString());
            }
            finally
            {
                if (cx != null)
                {
                    cx.Close();
                    cx.Dispose();
                }
            }
            return;
        }

        private void DecrementarDetalle(int IdProducto, double valor, int dias)
        {
            //string sql = @"UPDATE SCH_CONTRATO SET FECHA_FINALIZACION = FECHA_FINALIZACION + :P0, PRECIO_ESCALADO = PRECIO_ESCALADO + :P1   WHERE ID_CONTRATO = :P2";
            string sql = @"UPDATE SCH_CONTRATO SET FECHA_FINALIZACION = FECHA_FINALIZACION - :P0, ASIGNACION_PRESUPUESTAL = ASIGNACION_PRESUPUESTAL - :P1   WHERE ID_CONTRATO = :P2";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", dias));
                    cm.Parameters.Add(new OracleParameter("P1", valor));
                    cm.Parameters.Add(new OracleParameter("P2", IdProducto));

                    var lector = cm.ExecuteNonQuery();
                    return;
                }
            }
            catch (Exception ex)
            {
                System.Console.Out.WriteLine(ex.ToString());
            }
            finally
            {
                if (cx != null)
                {
                    cx.Close();
                    cx.Dispose();
                }
            }
            return;
        }

        public bool EliminarOtroSi(int id) {
            bool salida = false;
            var objeto = ObtenerOtroSiPorId(id);
            string sql = @"DELETE SCH_CONTRATO_OTROSI WHERE ID = :P0";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", id));

                    var lector = cm.ExecuteNonQuery();
                    salida = (lector > 0);
                    if (salida) {
                        DecrementarDetalle(objeto.IdContrato, objeto.TotalAdiciones, objeto.TotalProrrogaMes);
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
    }
}
