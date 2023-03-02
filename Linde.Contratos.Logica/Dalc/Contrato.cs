using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linde.Contratos.LogicaV2.Dalc
{
    class Contrato
    {

        public int ObtenerUltimoContrato(int maestro)
        {
            int salida = -1;

            var sql = @"select max(ID_CONTRATO) as CONTRATO FROM SCH_CONTRATO WHERE ID_MAESTRO = :P0 ";

            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", maestro));
                    OracleDataReader lector = cm.ExecuteReader();

                    while (lector.Read())
                    {

                        salida = int.Parse(lector["CONTRATO"].ToString());
                    }
                }
            }
            catch
            {
                return -1;
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

        public int GuardarContrato(int maestro)
        {
            int num = -1;
            string str = "INSERT INTO SCH_CONTRATO(ID_CONTRATO, ID_MAESTRO) VALUES(SEQ_SCH_CONTRATO.NEXTVAL, :P0)";
            OracleConnection connection = new OracleConnection();
            //connection = BaseDeDatos.Conexion.ObtieneConexion();
            connection = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();
            try
            {
                using (OracleCommand command = new OracleCommand(str, connection))
                {
                    command.Parameters.Add(new OracleParameter("P0", maestro));
                    if (command.ExecuteNonQuery() > 0)
                    {
                        num = this.ObtenerUltimoContrato(maestro);
                    }
                }
            }
            catch
            {
                num = -1;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            return num;
        }

        public bool CerrarContrato(int maestro, string producto, bool inactivar = false) {
            bool salida = false;
            string sql = "";
            if (!inactivar)
                sql = "UPDATE SCH_CONTRATO SET CERRADO = 1 WHERE ID_MAESTRO =:P0 AND ID_CONTRATO = :P1";
            else
                sql = "UPDATE SCH_CONTRATO SET CERRADO = -1 WHERE ID_MAESTRO =:P0 AND ID_CONTRATO = :P1";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", maestro));
                    cm.Parameters.Add(new OracleParameter("P1", producto));
                    int escritor = cm.ExecuteNonQuery();

                    salida = (escritor > 0);
                }
            }
            catch
            {
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

        private int CodigoMaestroPrimero(int idContratoProducto, bool retornarMaestro = false) {
            int salida = 0;
            Objeto.Contrato contrato = new Objeto.Contrato();
            string sql = @"SELECT ID_MAESTRO
                        FROM SCH_CONTRATO                           
                        WHERE ID_CONTRATO = :P100";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P100", idContratoProducto));
                    var lector = cm.ExecuteReader();
                    while (lector.Read()) {
                        salida = int.Parse(lector["ID_MAESTRO"].ToString());
                        if (!retornarMaestro)
                            salida = CodigoProductoPrimero(salida);
                    }
                }
            }
            catch {
                salida = idContratoProducto;
            }
            return salida;
        }

        private int CodigoProductoPrimero(int idContratoProducto)
        {
            int salida = 0;
            List<int> s = new List<int>();
            Objeto.Contrato contrato = new Objeto.Contrato();
            string sql = @"SELECT ID_CONTRATO
                        FROM SCH_CONTRATO                           
                        WHERE ID_MAESTRO = :P100 order by ID_CONTRATO";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P100", idContratoProducto));
                    var lector = cm.ExecuteReader();
                    while (lector.Read())
                    {
                        salida = int.Parse(lector["ID_CONTRATO"].ToString());
                        s.Add(salida);
                    }
                }
            }
            catch
            {
                salida = idContratoProducto;
            }
            return s.First();
        }

        public Objeto.Contrato ObtenerPrecio(int idContratoProducto)
        {
            var nuevoValor = CodigoMaestroPrimero(idContratoProducto);
            Objeto.Contrato contrato = new Objeto.Contrato();
            string sql = @"SELECT  
                            VALOR_ANO1, VALOR_ANO1, VALOR_ANO3, VALOR_ANO4, VALOR_ANO5,
                            OTROSI, VIGENCIA, ESTATAL, ASIGNACION_PRESUPUESTAL, NO_RUBRO_PRESUPUESTAL,
                            CERTIFICADO_DISPONIBLE, ACTA_FECHA_INICIO, NUMERO_FACTURA_INICIAL,
                            TOTAL_ADICIONES, TOTAL_PRORROGA_MES, ID, SERIAL
                        FROM SCH_CONTRATO                           
                        WHERE ID_CONTRATO = :P100";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P100", nuevoValor));
                    var lector = cm.ExecuteReader();
                    if (lector.Read())
                    {
                        if (lector[0] == null)
                            contrato.Valor_ano1 = 0;
                        else
                        {
                            if (lector[0].ToString().Length > 0) 
                                contrato.Valor_ano1 = lector.GetInt32(0);
                            
                            else
                                contrato.Valor_ano1 = 0;
                        }
                            

                        if (lector[1] == null)
                            contrato.Valor_ano2 = 0;
                        else
                        {
                            if (lector[1].ToString().Length > 0)
                                contrato.Valor_ano2 = lector.GetInt32(1);

                            else
                                contrato.Valor_ano2 = 0;
                        }

                        if (lector[2] == null)
                            contrato.Valor_ano3 = 0;
                        else
                        {
                            if (lector[2].ToString().Length > 0)
                                contrato.Valor_ano3 = lector.GetInt32(2);

                            else
                                contrato.Valor_ano3 = 0;
                        }

                        if (lector[3] == null)
                            contrato.Valor_ano4 = 0;
                        else
                        {
                            if (lector[3].ToString().Length > 0)
                                contrato.Valor_ano4 = lector.GetInt32(3);

                            else
                                contrato.Valor_ano4 = 0;
                        }

                        if (lector[4] == null)
                            contrato.Valor_ano5 = 0;
                        else
                        {
                            if (lector[4].ToString().Length > 0)
                                contrato.Valor_ano5 = lector.GetInt32(4);

                            else
                                contrato.Valor_ano5 = 0;
                        }

                        if (lector[5] == null)
                            contrato.Otrosi = false;
                        else
                            contrato.Otrosi = RevalidarValidarBool(lector.GetInt32(5));

                        if (lector[6] == null)
                            contrato.Vigencia = false;
                        else
                            contrato.Vigencia = RevalidarValidarBool(lector.GetInt32(6));

                        if (lector[7] == null)
                            contrato.Estatal = false;
                        else
                            contrato.Estatal = RevalidarValidarBool(lector.GetInt32(7));

                        if (lector[8] == null)
                            contrato.Asignacion_presupuestal = 0;
                        else
                            contrato.Asignacion_presupuestal = lector.GetInt32(8);

                        if (lector[9] == null)
                            contrato.No_rubro_presupuestal = "";
                        else
                            contrato.No_rubro_presupuestal = lector[9].ToString();

                        if (lector[10] == null)
                            contrato.Certificado_disponible = "";
                        else
                            contrato.Certificado_disponible = lector[10].ToString();

                        if (lector[11] == null)
                            contrato.Acta_fecha_inicio = DateTime.MinValue;
                        else
                            contrato.Acta_fecha_inicio = lector.GetDateTime(11);

                        if (lector[12] == null)
                            contrato.Numero_factura_final = "";
                        else
                        {
                            if (lector[12].ToString().Length > 0)
                                contrato.Numero_factura_final = lector.GetString(12);

                            else
                                contrato.Numero_factura_final = "";
                        }

                        

                        if (lector[13] == null)
                            contrato.Total_adiciones = 0;
                        else
                            contrato.Total_adiciones = lector.GetInt32(13);

                        if (lector[14] == null)
                            contrato.Total_prorroga_mes = 0;
                        else
                            contrato.Total_prorroga_mes = lector.GetInt32(14);

                        if (lector[15] == null)
                            contrato.Id = "";
                        else
                            contrato.Id = lector[15].ToString();

                        if (lector[16] == null)
                            contrato.Serial = "";
                        else
                            contrato.Serial = lector[16].ToString();
                    }
                }
            }
            catch(Exception ex)
            {

            }
            finally
            {
                if (cx != null)
                {
                    cx.Close();
                    cx.Dispose();
                }
            }
            return contrato;
        }

        public bool GuardarPrecio(int ContratoProducto, Objeto.Contrato contrato)
        {
            bool salida = false;

            string sql = @"UPDATE SCH_CONTRATO
                            SET VALOR_ANO1 = :P0,
                            VALOR_ANO2 = :P1,
                            VALOR_ANO3 = :P2,
                            VALOR_ANO4 = :P3,
                            VALOR_ANO5 = :P4,
                            OTROSI = :P5,
                            VIGENCIA = :P6,
                            ESTATAL = :P7,
                            ASIGNACION_PRESUPUESTAL = :P8,
                            NO_RUBRO_PRESUPUESTAL = :P9,
                            CERTIFICADO_DISPONIBLE = :P10,
                            ACTA_FECHA_INICIO = :P11,
                            NUMERO_FACTURA_INICIAL = :P12,
                            TOTAL_ADICIONES = :P13,
                            TOTAL_PRORROGA_MES = :P14,
                            ID = :P15,
                            SERIAL = :P16
                           WHERE ID_CONTRATO = :P100";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", contrato.Valor_ano1));
                    cm.Parameters.Add(new OracleParameter("P1", contrato.Valor_ano2));
                    cm.Parameters.Add(new OracleParameter("P2", contrato.Valor_ano3));
                    cm.Parameters.Add(new OracleParameter("P3", contrato.Valor_ano4));
                    cm.Parameters.Add(new OracleParameter("P4", contrato.Valor_ano5));
                    if (contrato.Otrosi)
                        cm.Parameters.Add(new OracleParameter("P5", "0"));
                    else
                        cm.Parameters.Add(new OracleParameter("P5", "1"));

                    if (contrato.Vigencia)
                        cm.Parameters.Add(new OracleParameter("P6", "0"));
                    else
                        cm.Parameters.Add(new OracleParameter("P6", "1"));

                    if (contrato.Estatal)
                        cm.Parameters.Add(new OracleParameter("P7", "0"));
                    else
                        cm.Parameters.Add(new OracleParameter("P7", "1"));

                    cm.Parameters.Add(new OracleParameter("P8", contrato.Asignacion_presupuestal));
                    cm.Parameters.Add(new OracleParameter("P9", contrato.No_rubro_presupuestal));
                    cm.Parameters.Add(new OracleParameter("P10", contrato.Certificado_disponible));
                    cm.Parameters.Add(new OracleParameter("P11", contrato.Acta_fecha_inicio));
                    cm.Parameters.Add(new OracleParameter("P12", contrato.Numero_factura_inicial));
                    cm.Parameters.Add(new OracleParameter("P13", contrato.Total_adiciones));
                    cm.Parameters.Add(new OracleParameter("P14", contrato.Total_prorroga_mes));
                    cm.Parameters.Add(new OracleParameter("P15", contrato.Id));
                    cm.Parameters.Add(new OracleParameter("P16", contrato.Serial));
                    cm.Parameters.Add(new OracleParameter("P100", ContratoProducto));

                    int escritor = cm.ExecuteNonQuery();

                    /*Validar valores de los productos del contrato - LEGION*/
                    if (escritor > 0) {
                        var contratoMaestro = CodigoMaestroPrimero(ContratoProducto, true);
                        salida = AplicarDatosPrecioAlMaestro(contratoMaestro, contrato);

                    }
                    /************************************************/
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

        public bool AplicarDatosPrecioAlMaestro(int idMaestro, Objeto.Contrato contrato) {
            bool salida = false;

            string sql = @"UPDATE SCH_CONTRATO
                            SET VALOR_ANO1 = :P0,
                            VALOR_ANO2 = :P1,
                            VALOR_ANO3 = :P2,
                            VALOR_ANO4 = :P3,
                            VALOR_ANO5 = :P4,
                            OTROSI = :P5,
                            VIGENCIA = :P6,
                            ESTATAL = :P7,
                            ASIGNACION_PRESUPUESTAL = :P8,
                            NO_RUBRO_PRESUPUESTAL = :P9,
                            CERTIFICADO_DISPONIBLE = :P10,
                            ACTA_FECHA_INICIO = :P11,
                            NUMERO_FACTURA_INICIAL = :P12,
                            TOTAL_ADICIONES = :P13,
                            TOTAL_PRORROGA_MES = :P14,
                            ID = :P15,
                            SERIAL = :P16
                           WHERE ID_MAESTRO = :P100";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", contrato.Valor_ano1));
                    cm.Parameters.Add(new OracleParameter("P1", contrato.Valor_ano2));
                    cm.Parameters.Add(new OracleParameter("P2", contrato.Valor_ano3));
                    cm.Parameters.Add(new OracleParameter("P3", contrato.Valor_ano4));
                    cm.Parameters.Add(new OracleParameter("P4", contrato.Valor_ano5));
                    if (contrato.Otrosi)
                        cm.Parameters.Add(new OracleParameter("P5", "0"));
                    else
                        cm.Parameters.Add(new OracleParameter("P5", "1"));

                    if (contrato.Vigencia)
                        cm.Parameters.Add(new OracleParameter("P6", "0"));
                    else
                        cm.Parameters.Add(new OracleParameter("P6", "1"));

                    if (contrato.Estatal)
                        cm.Parameters.Add(new OracleParameter("P7", "0"));
                    else
                        cm.Parameters.Add(new OracleParameter("P7", "1"));

                    cm.Parameters.Add(new OracleParameter("P8", contrato.Asignacion_presupuestal));
                    cm.Parameters.Add(new OracleParameter("P9", contrato.No_rubro_presupuestal));
                    cm.Parameters.Add(new OracleParameter("P10", contrato.Certificado_disponible));
                    cm.Parameters.Add(new OracleParameter("P11", contrato.Acta_fecha_inicio));
                    cm.Parameters.Add(new OracleParameter("P12", contrato.Numero_factura_inicial));
                    cm.Parameters.Add(new OracleParameter("P13", contrato.Total_adiciones));
                    cm.Parameters.Add(new OracleParameter("P14", contrato.Total_prorroga_mes));
                    cm.Parameters.Add(new OracleParameter("P15", contrato.Id));
                    cm.Parameters.Add(new OracleParameter("P16", contrato.Serial));
                    cm.Parameters.Add(new OracleParameter("P100", idMaestro));

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

        public Objeto.Contrato ObtenerDelta(int idContratoProducto)
        {
            Objeto.Contrato contrato = new Objeto.Contrato();
            string sql = @"SELECT  
                            DELTAEE, DELTAIP, DELTADIESEL, DELTAIPP, DELTAGAS,
                            DELTATRM, DELTAOTROS, DIESEL, IPC, USD, ENERGIA, OTROS
                        FROM SCH_CONTRATO                           
                        WHERE ID_CONTRATO = :P100";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P100", idContratoProducto));
                    var lector = cm.ExecuteReader();
                    if (lector.Read())
                    {
                        if (lector[0] == null)
                            contrato.Deltaee = 0;
                        else
                            contrato.Deltaee = lector.GetInt32(0);

                        if (lector[1] == DBNull.Value)
                            contrato.Deltaip = 0;
                        else
                            contrato.Deltaip = lector.GetInt32(1);

                        if (lector[2] == DBNull.Value)
                            contrato.Deltadiesel = 0;
                        else
                            contrato.Deltadiesel = lector.GetInt32(2);

                        if (lector[3] == DBNull.Value)
                            contrato.Deltaipp = 0;
                        else
                            contrato.Deltaipp = lector.GetInt32(3);

                        if (lector[4] == DBNull.Value)
                            contrato.Deltagas = 0;
                        else
                            contrato.Deltagas = lector.GetInt32(4);

                        if (lector[5] == DBNull.Value)
                            contrato.Deltatrm = 0;
                        else
                            contrato.Deltatrm = lector.GetInt32(5);

                        if (lector[6] == DBNull.Value)
                            contrato.Deltaotros = 0;
                        else
                            contrato.Deltaotros = lector.GetInt32(6);

                        if (lector[7] == DBNull.Value)
                            contrato.Diesel = 0;
                        else
                            contrato.Diesel = lector.GetInt32(7);

                        if (lector[8] == DBNull.Value)
                            contrato.Ipc = 0;
                        else
                            contrato.Ipc = lector.GetInt32(8);

                        if (lector[9] == DBNull.Value)
                            contrato.Usd = 0;
                        else
                            contrato.Usd = lector.GetInt32(9);

                        if (lector[10] == DBNull.Value)
                            contrato.Energia = 0;
                        else
                            contrato.Energia = lector.GetInt32(10);

                        if (lector[11] == DBNull.Value)
                            contrato.Otros = "";
                        else
                            contrato.Otros = lector[11].ToString();
                    }
                }
            }
            catch
            {

            }
            finally
            {
                if (cx != null)
                {
                    cx.Close();
                    cx.Dispose();
                }
            }
            return contrato;
        }

        public bool GuardarDelta(int ContratoProducto, Objeto.Contrato contrato)
        {
            bool salida = false;

            string sql = @"UPDATE SCH_CONTRATO
                            SET DELTAEE = :P0,
                            DELTAIP = :P1,
                            DELTADIESEL = :P2,
                            DELTAIPP = :P3,
                            DELTAGAS = :P4,
                            DELTATRM = :P5,
                            DELTAOTROS = :P6,
                            DIESEL = :P7,
                            IPC = :P8,
                            USD = :P9,
                            ENERGIA = :P10,
                            OTROS = :P11
                           WHERE ID_CONTRATO = :P100";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", contrato.Deltaee));
                    cm.Parameters.Add(new OracleParameter("P1", contrato.Deltaip));
                    cm.Parameters.Add(new OracleParameter("P2", contrato.Deltadiesel));
                    cm.Parameters.Add(new OracleParameter("P3", contrato.Deltaipp));
                    cm.Parameters.Add(new OracleParameter("P4", contrato.Deltagas));
                    cm.Parameters.Add(new OracleParameter("P5", contrato.Deltatrm));
                    cm.Parameters.Add(new OracleParameter("P6", contrato.Deltaotros));
                    cm.Parameters.Add(new OracleParameter("P7", contrato.Diesel));
                    cm.Parameters.Add(new OracleParameter("P8", contrato.Ipc));
                    cm.Parameters.Add(new OracleParameter("P9", contrato.Usd));
                    cm.Parameters.Add(new OracleParameter("P10", contrato.Energia));
                    cm.Parameters.Add(new OracleParameter("P11", contrato.Otros));
                    cm.Parameters.Add(new OracleParameter("P100", ContratoProducto));

                    int escritor = cm.ExecuteNonQuery();

                    salida = (escritor > 0);

                }
            }
            catch
            {
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


        public Objeto.Contrato ObtenerDetalle(int idContratoProducto) {
            Objeto.Contrato contrato = new Objeto.Contrato();
            string sql = @"SELECT  
                            PRODUCTO,PRECIO_PRODUCTO, CONSUMO_CONTRATADO, C_TOP,TOP,
                            ESCALADO, CANTIDAD_ESCALADO, PRECIO_ESCALADO, PERIODICIDAD,
                            ALQUILER_TANQUES,ARRIENDO_EQUIPO, CANON_ARRENDAMIENTO,
                            GARANTIA,FIRMADO_POR,FECHA_INICIO,FECHA_FIRMADO,
                            FECHA_FINALIZACION,FECHA_FINALIZA_PRO,INICIO_SUMINISTRO,
                            VIGENCIA_MESES,VIGENCIA_AUTO_MESES,AVISO_TERMINACION_CON,
                            CONDICIONES_ESPECIALES,TIEMPO_ESPERA_TRAC,HORAS_MIN_ESPERA,
                            CONTRATO_COMODATO,PLAZO_PAGO_DIAS,MULTA_VENCIMIENTO_FAC,
                            PORCENTAJE_MULTA,AUMENTO_PRECIO, ID_CONTRATO, CERRADO
                        FROM SCH_CONTRATO                           
                        WHERE ID_CONTRATO = :P100";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx)) {
                    cm.Parameters.Add(new OracleParameter("P100", idContratoProducto));
                    var lector = cm.ExecuteReader();
                    if (lector.Read()) {
                        if (lector[0] == DBNull.Value)
                            contrato.Producto = "SIN PRODUCTO";
                        else
                            contrato.Producto = lector.GetString(0);

                        if (lector[1] == DBNull.Value)
                            contrato.Precio_producto = 0;
                        else
                            contrato.Precio_producto = lector.GetInt32(1);

                        if (lector[2] == DBNull.Value)
                            contrato.Consumo_contratado = 0;
                        else
                            contrato.Consumo_contratado = lector.GetInt32(2);

                        if (lector[3] == DBNull.Value)
                            contrato.C_top = false;
                        else
                            contrato.C_top = RevalidarValidarBool(lector.GetInt32(3));

                        if (lector[4] == DBNull.Value)
                            contrato.Top = 0;
                        else
                            contrato.Top = lector.GetInt32(4);

                        if (lector[5] == DBNull.Value)
                            contrato.Escalado = false;
                        else
                            contrato.Escalado = RevalidarValidarBool(lector.GetInt32(5));

                        if (lector[6] == DBNull.Value)
                            contrato.Cantidad_escalado = "0";
                        else
                            contrato.Cantidad_escalado = lector.GetString(6);

                        if (lector[7] == DBNull.Value)
                            contrato.Precio_escalado = "0";
                        else
                            contrato.Precio_escalado = lector.GetString(7);

                        if (lector[8] == DBNull.Value)
                            contrato.Periodicidad = 0;
                        else
                            contrato.Periodicidad = lector.GetInt32(8);

                        if (lector[9] == DBNull.Value)
                            contrato.Alquiler_tanques = false;
                        else
                            contrato.Alquiler_tanques = RevalidarValidarBool(lector.GetInt32(9));

                        if (lector[10] == DBNull.Value)
                            contrato.Arriendo_equipo = false;
                        else
                            contrato.Arriendo_equipo = RevalidarValidarBool(lector.GetInt32(10));

                        if (lector[11] == DBNull.Value)
                            contrato.Canon_arrendamiento = 0;
                        else
                            contrato.Canon_arrendamiento = lector.GetInt32(11);

                        if (lector[12] == DBNull.Value)
                            contrato.Garantia = "";
                        else
                            contrato.Garantia = lector.GetString(12);

                        if (lector[13] == DBNull.Value)
                            contrato.Firmado_por = -1;
                        else
                            contrato.Firmado_por = lector.GetInt32(13);

                        if (lector[14] == DBNull.Value)
                            contrato.Fecha_inicio = DateTime.MinValue;
                        else
                            contrato.Fecha_inicio = lector.GetDateTime(14);

                        if (lector[15] == DBNull.Value)
                            contrato.Fecha_firmado = DateTime.MinValue;
                        else
                            contrato.Fecha_firmado = lector.GetDateTime(15);

                        if (lector[16] == DBNull.Value)
                            contrato.Fecha_finalizacion = DateTime.MinValue;
                        else
                            contrato.Fecha_finalizacion = lector.GetDateTime(16);

                        if (lector[17] == DBNull.Value)
                            contrato.Fecha_finaliza_pro = DateTime.MinValue;
                        else
                            contrato.Fecha_finaliza_pro = lector.GetDateTime(17);

                        if (lector["INICIO_SUMINISTRO"] == DBNull.Value)
                        {
                            contrato.Inicio_suministro = DateTime.MinValue;

                        }
                        else {
                            contrato.Inicio_suministro = DateTime.Parse(lector["INICIO_SUMINISTRO"].ToString());

                        }
                        contrato.Inicio_suministro_texto = contrato.Inicio_suministro.ToShortDateString();


                        if (lector[19] == DBNull.Value)
                            contrato.Vigencia_meses = 0;
                        else
                            contrato.Vigencia_meses = lector.GetInt32(19);

                        if (lector[20] == DBNull.Value)
                            contrato.Vigencia_auto_meses = 0;
                        else
                            contrato.Vigencia_auto_meses = lector.GetInt32(20);

                        if (lector[21] == DBNull.Value)
                            contrato.Aviso_terminacion_con = 0;
                        else
                            contrato.Aviso_terminacion_con = lector.GetInt32(21);

                        if (lector[22] == DBNull.Value)
                            contrato.Condiciones_especiales = "";
                        else
                            contrato.Condiciones_especiales = lector.GetString(22);

                        if (lector[23] == DBNull.Value)
                            contrato.Tiempo_espera_trac = false;
                        else
                            contrato.Tiempo_espera_trac = RevalidarValidarBool(lector.GetInt32(23));

                        if (lector[24] == DBNull.Value)
                            contrato.Horas_min_espera = 0;
                        else
                            contrato.Horas_min_espera = lector.GetInt32(24);

                        if (lector[25] == DBNull.Value)
                            contrato.Contrato_comodato = false;
                        else
                            contrato.Contrato_comodato = RevalidarValidarBool(lector.GetInt32(25));

                        if (lector[26] == DBNull.Value)
                            contrato.Plazo_pago_dias = 0;
                        else
                            contrato.Plazo_pago_dias = lector.GetInt32(26);

                        if (lector[27] == DBNull.Value)
                            contrato.Multa_vencimiento_fac = false;
                        else
                            contrato.Multa_vencimiento_fac = RevalidarValidarBool(lector.GetInt32(27));

                        if (lector[28] == DBNull.Value)
                            contrato.Porcentaje_multa = "0";
                        else
                            contrato.Porcentaje_multa = lector.GetString(28);

                        if (lector[29] == DBNull.Value)
                            contrato.Aumento_precio = "";
                        else
                            contrato.Aumento_precio = lector.GetString(29);


                        contrato.Id_contrato = int.Parse(lector[30].ToString());

                        contrato.Cerrado = int.Parse(lector["CERRADO"].ToString());
                        //if (lector[31] == DBNull.Value)
                        //    contrato.Mail = lector.GetString(30);
                        //else
                        //    contrato.Mail = lector.GetString(31);

                    }
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
            return contrato;
        }

        public bool GuardarDetalle(int ContratoProducto, Objeto.Contrato contrato) {
            bool salida = false;
            string sql = @"UPDATE SCH_CONTRATO
                            SET 
                            PRODUCTO = :P0,
                            PRECIO_PRODUCTO = :P1,
                            CONSUMO_CONTRATADO = :P2,
                            C_TOP = :P3,
                            TOP = :P4,
                            ESCALADO = :P5,
                            CANTIDAD_ESCALADO = :P6,
                            PRECIO_ESCALADO = :P7,
                            PERIODICIDAD = :P8,
                            ALQUILER_TANQUES = :P9,
                            ARRIENDO_EQUIPO = :P10,
                            CANON_ARRENDAMIENTO = :P11,
                            GARANTIA = :P12,
                            FIRMADO_POR = :P13,
                            FECHA_INICIO = :P14,
                            FECHA_FIRMADO = :P15,
                            FECHA_FINALIZACION = :P16,
                            FECHA_FINALIZA_PRO = :P17,
                            INICIO_SUMINISTRO = :P18,
                            VIGENCIA_MESES = :P19,
                            VIGENCIA_AUTO_MESES = :P20,
                            AVISO_TERMINACION_CON = :P21,
                            CONDICIONES_ESPECIALES = :P22,
                            TIEMPO_ESPERA_TRAC = :P23,
                            HORAS_MIN_ESPERA = :P24,
                            CONTRATO_COMODATO = :P25,
                            PLAZO_PAGO_DIAS = :P26,
                            MULTA_VENCIMIENTO_FAC = :P27,
                            PORCENTAJE_MULTA = :P28,
                            AUMENTO_PRECIO = :P29
                           WHERE ID_CONTRATO = :P100";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", contrato.Producto));
                    cm.Parameters.Add(new OracleParameter("P1", contrato.Precio_producto));
                    cm.Parameters.Add(new OracleParameter("P2", contrato.Consumo_contratado));
                    cm.Parameters.Add(new OracleParameter("P3", ValidarBool(contrato.C_top)));
                    cm.Parameters.Add(new OracleParameter("P4", contrato.Top));
                    cm.Parameters.Add(new OracleParameter("P5", ValidarBool(contrato.Escalado)));
                    cm.Parameters.Add(new OracleParameter("P6", contrato.Cantidad_escalado));
                    cm.Parameters.Add(new OracleParameter("P7", contrato.Precio_escalado));
                    cm.Parameters.Add(new OracleParameter("P8", contrato.Periodicidad));
                    cm.Parameters.Add(new OracleParameter("P9", ValidarBool(contrato.Alquiler_tanques)));
                    cm.Parameters.Add(new OracleParameter("P10", ValidarBool(contrato.Arriendo_equipo)));
                    cm.Parameters.Add(new OracleParameter("P11", contrato.Canon_arrendamiento));
                    cm.Parameters.Add(new OracleParameter("P12", contrato.Garantia));
                    cm.Parameters.Add(new OracleParameter("P13", contrato.Firmado_por));
                    cm.Parameters.Add(new OracleParameter("P14", contrato.Fecha_inicio));
                    cm.Parameters.Add(new OracleParameter("P15", contrato.Fecha_firmado));
                    cm.Parameters.Add(new OracleParameter("P16", contrato.Fecha_finalizacion));
                    cm.Parameters.Add(new OracleParameter("P17", contrato.Fecha_finaliza_pro));
                    cm.Parameters.Add(new OracleParameter("P18", contrato.Inicio_suministro));
                    cm.Parameters.Add(new OracleParameter("P19", contrato.Vigencia_meses));
                    cm.Parameters.Add(new OracleParameter("P20", contrato.Vigencia_auto_meses));
                    cm.Parameters.Add(new OracleParameter("P21", contrato.Aviso_terminacion_con));
                    cm.Parameters.Add(new OracleParameter("P22", contrato.Condiciones_especiales));
                    cm.Parameters.Add(new OracleParameter("P23", ValidarBool(contrato.Tiempo_espera_trac)));
                    cm.Parameters.Add(new OracleParameter("P24", contrato.Horas_min_espera));
                    cm.Parameters.Add(new OracleParameter("P25", ValidarBool(contrato.Contrato_comodato)));
                    cm.Parameters.Add(new OracleParameter("P26", contrato.Plazo_pago_dias));
                    cm.Parameters.Add(new OracleParameter("P27", ValidarBool(contrato.Multa_vencimiento_fac))); ;
                    cm.Parameters.Add(new OracleParameter("P28", contrato.Porcentaje_multa));
                    cm.Parameters.Add(new OracleParameter("P29", contrato.Aumento_precio));
                    //cm.Parameters.Add(new OracleParameter("P30", contrato.Mail));

                    cm.Parameters.Add(new OracleParameter("P100", ContratoProducto));

                    int escritor = cm.ExecuteNonQuery();

                    salida = (escritor > 0);

                    int valorMastro = CodigoMaestroPrimero(ContratoProducto, true);

                    salida = ActualizarCorreoVendedor(valorMastro, contrato.Mail);

                    ReplicarDatos(valorMastro, contrato);
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

        private void ReplicarDatos(int idMaestro, Objeto.Contrato contrato) {
            string sql = @"UPDATE SCH_CONTRATO
                            SET 
                            ESCALADO = :P5,
                            CANTIDAD_ESCALADO = :P6,
                            PRECIO_ESCALADO = :P7,
                            ALQUILER_TANQUES = :P9,
                            ARRIENDO_EQUIPO = :P10,
                            CANON_ARRENDAMIENTO = :P11,
                            GARANTIA = :P12,
                            FIRMADO_POR = :P13,
                            FECHA_INICIO = :P14,
                            FECHA_FIRMADO = :P15,
                            FECHA_FINALIZACION = :P16,
                            FECHA_FINALIZA_PRO = :P17,
                            INICIO_SUMINISTRO = :P18,
                            VIGENCIA_MESES = :P19,
                            VIGENCIA_AUTO_MESES = :P20,
                            AVISO_TERMINACION_CON = :P21,
                            CONDICIONES_ESPECIALES = :P22,
                            TIEMPO_ESPERA_TRAC = :P23,
                            HORAS_MIN_ESPERA = :P24,
                            CONTRATO_COMODATO = :P25,
                            PLAZO_PAGO_DIAS = :P26,
                            MULTA_VENCIMIENTO_FAC = :P27,
                            PORCENTAJE_MULTA = :P28,
                            AUMENTO_PRECIO = :P29
                           WHERE ID_MAESTRO = :P100";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P5", ValidarBool(contrato.Escalado)));
                    cm.Parameters.Add(new OracleParameter("P6", contrato.Cantidad_escalado));
                    cm.Parameters.Add(new OracleParameter("P7", contrato.Precio_escalado));
                    cm.Parameters.Add(new OracleParameter("P9", ValidarBool(contrato.Alquiler_tanques)));
                    cm.Parameters.Add(new OracleParameter("P10", ValidarBool(contrato.Arriendo_equipo)));
                    cm.Parameters.Add(new OracleParameter("P11", contrato.Canon_arrendamiento));
                    cm.Parameters.Add(new OracleParameter("P12", contrato.Garantia));
                    cm.Parameters.Add(new OracleParameter("P13", contrato.Firmado_por));
                    cm.Parameters.Add(new OracleParameter("P14", contrato.Fecha_inicio));
                    cm.Parameters.Add(new OracleParameter("P15", contrato.Fecha_firmado));
                    cm.Parameters.Add(new OracleParameter("P16", contrato.Fecha_finalizacion));
                    cm.Parameters.Add(new OracleParameter("P17", contrato.Fecha_finaliza_pro));
                    cm.Parameters.Add(new OracleParameter("P18", contrato.Inicio_suministro));
                    cm.Parameters.Add(new OracleParameter("P19", contrato.Vigencia_meses));
                    cm.Parameters.Add(new OracleParameter("P20", contrato.Vigencia_auto_meses));
                    cm.Parameters.Add(new OracleParameter("P21", contrato.Aviso_terminacion_con));
                    cm.Parameters.Add(new OracleParameter("P22", contrato.Condiciones_especiales));
                    cm.Parameters.Add(new OracleParameter("P23", ValidarBool(contrato.Tiempo_espera_trac)));
                    cm.Parameters.Add(new OracleParameter("P24", contrato.Horas_min_espera));
                    cm.Parameters.Add(new OracleParameter("P25", ValidarBool(contrato.Contrato_comodato)));
                    cm.Parameters.Add(new OracleParameter("P26", contrato.Plazo_pago_dias));
                    cm.Parameters.Add(new OracleParameter("P27", ValidarBool(contrato.Multa_vencimiento_fac))); ;
                    cm.Parameters.Add(new OracleParameter("P28", contrato.Porcentaje_multa));
                    cm.Parameters.Add(new OracleParameter("P29", contrato.Aumento_precio));
                    //cm.Parameters.Add(new OracleParameter("P30", contrato.Mail));

                    cm.Parameters.Add(new OracleParameter("P100", idMaestro));

                    int escritor = cm.ExecuteNonQuery();

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

        }

        private int ValidarBool(bool entrada) {
            if (entrada)
                return 1;
            else
                return 0;
        }

        private bool RevalidarValidarBool(int ? entrada)
        {
            return (entrada == 1);
        }

        private string RevalidarValidarTexto(string entrada)
        {
            string salida = "";
            if (entrada == null)
            {
                salida = "";
            }
            else {
                salida = entrada;
            }
            return salida;
        }

        private int RevalidarValidarNumero(int ? entrada)
        {
            int salida = 0;
            if (entrada == null)
            {
                salida = 0;
            }
            else
            {
                salida = int.Parse(entrada.ToString());
            }
            return salida;
        }

        public bool ActualizarAlertaContrato(int idMaestro, int idContrato, int tipo)
        {
            bool salida = false;
            string sql = @"UPDATE SCH_CONTRATO SET <CAMPO> = <CAMPO> +1 WHERE ID_MAESTRO = :P0 ";
            if (tipo == 1)
                sql = sql.Replace("<CAMPO>", "ALERTA");
            else if (tipo == 2)
                sql = sql.Replace("<CAMPO>", "ALERTA2");
            else if (tipo == 3)
                sql = sql.Replace("<CAMPO>", "ALERTA3");

            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    //cm.Parameters.Add(new OracleParameter("P0", idContrato));
                    cm.Parameters.Add(new OracleParameter("P1", idMaestro));

                    var lector = cm.ExecuteNonQuery();
                    salida = lector > 0;

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

        public string CorreoVendedor(int idMaestro)
        {
            string salida = "";
            string sql = @"SELECT CORREO FROM SCH_CONTRATO_MAESTRO WHERE ID_CONTRATO_MAESTRO = :P0 ";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", idMaestro));

                    var escritor = cm.ExecuteReader();

                    while (escritor.Read()) {
                        salida = escritor["USUARIO"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.Out.WriteLine(ex.ToString());
                salida = "nestor.rodriguez@linde.com";
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

        public bool ActualizarCorreoVendedor(int idMaestro, string correo) {
            bool salida = false;
            string sql = @"UPDATE SCH_CONTRATO_MAESTRO SET CORREO = :P0 WHERE ID_CONTRATO_MAESTRO = :P1 ";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", correo));
                    cm.Parameters.Add(new OracleParameter("P1", idMaestro));

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

        [Obsolete]
        public bool ModificarAlerta(int idMaestro, int idContrato, char signo) {
            bool salida = false;
            string sql = @"UPDATE SCH_CONTRATO SET ALERTA = ALERTA " + signo + " 1 WHERE ID_MAESTRO = ID_CONTRATO = :P1 ";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", idMaestro));
                    cm.Parameters.Add(new OracleParameter("P1", idContrato));

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


        public List<Objeto.AlertaTipo1> BarrerContratosTipo1()
        {

            var salida = new List <Objeto.AlertaTipo1>();
            var AlertaTipo1 = new Objeto.AlertaTipo1();
            string sql = @"SELECT CON.ID_MAESTRO, CON.FECHA_FINALIZACION, CLIE.DESCRIPCION , MAESTRO.ID_CLIENTE, MAESTRO.IDENTIFICACION, MAESTRO.ID_TIPO_CLIENTE, TIPCLI.deta101 AS TIPO_CLIENTE,  CON.NO_RUBRO_PRESUPUESTAL
FROM SCH_CONTRATO CON 
    inner join SCH_CONTRATO_MAESTRO MAESTRO ON (CON.ID_MAESTRO =MAESTRO.ID_CONTRATO_MAESTRO)
    INNER JOIN (SELECT codde01, deta101 FROM M_tabdes WHERE CODTA01 = 11 order by deta101) TIPCLI ON (TIPCLI.codde01 = MAESTRO.ID_TIPO_CLIENTE )
    INNER JOIN (SELECT CLIENT_CLI AS CODIGO, razon__cli AS DESCRIPCION FROM M_DF01 WHERE cp_exp_cli is not null and COSEGURO_CLI = 'N' AND PRINCIPAL_CLI ='N' ) CLIE ON (MAESTRO.ID_CLIENTE = CLIE.CODIGO)                             
WHERE CON.ESTATAL = 1 AND CON.CERRADO = 0 AND CON.ALERTA <> 1 AND (CON.FECHA_FINALIZACION  >= SYSDATE - 45 AND CON.FECHA_FINALIZACION  <= SYSDATE + 45) 
order by 2";
            //string sql = @"SELECT  
            //                PRODUCTO,PRECIO_PRODUCTO, CONSUMO_CONTRATADO, C_TOP,TOP,
            //                ESCALADO, CANTIDAD_ESCALADO, PRECIO_ESCALADO, PERIODICIDAD,
            //                ALQUILER_TANQUES,ARRIENDO_EQUIPO, CANON_ARRENDAMIENTO,
            //                GARANTIA,FIRMADO_POR,FECHA_INICIO,FECHA_FIRMADO,
            //                FECHA_FINALIZACION,FECHA_FINALIZA_PRO,INICIO_SUMINISTRO,
            //                VIGENCIA_MESES,VIGENCIA_AUTO_MESES,AVISO_TERMINACION_CON,
            //                CONDICIONES_ESPECIALES,TIEMPO_ESPERA_TRAC,HORAS_MIN_ESPERA,
            //                CONTRATO_COMODATO,PLAZO_PAGO_DIAS,MULTA_VENCIMIENTO_FAC,
            //                PORCENTAJE_MULTA,AUMENTO_PRECIO, ID_CONTRATO, ID_MAESTRO
            //            FROM SCH_CONTRATO                           
            //            WHERE ESTATAL = 1 AND CERRADO = 0 AND ALERTA <> 1 AND FECHA_FINALIZA_PRO >= SYSDATE - 45";
            //string sql = @"SELECT  
            //                deta.PRODUCTO,deta.PRECIO_PRODUCTO, deta.CONSUMO_CONTRATADO, deta.C_TOP,deta.TOP,
            //                deta.ESCALADO, deta.CANTIDAD_ESCALADO, deta.PRECIO_ESCALADO, deta.PERIODICIDAD,
            //                deta.ALQUILER_TANQUES,deta.ARRIENDO_EQUIPO, deta.CANON_ARRENDAMIENTO,
            //                deta.GARANTIA,deta.FIRMADO_POR,deta.FECHA_INICIO,deta.FECHA_FIRMADO,
            //                deta.FECHA_FINALIZACION,deta.FECHA_FINALIZA_PRO,deta.INICIO_SUMINISTRO,
            //                deta.VIGENCIA_MESES,deta.VIGENCIA_AUTO_MESES,deta.AVISO_TERMINACION_CON,
            //                deta.CONDICIONES_ESPECIALES,deta.TIEMPO_ESPERA_TRAC,deta.HORAS_MIN_ESPERA,
            //                deta.CONTRATO_COMODATO,deta.PLAZO_PAGO_DIAS,deta.MULTA_VENCIMIENTO_FAC,
            //                deta.PORCENTAJE_MULTA,deta.AUMENTO_PRECIO, deta.ID_CONTRATO, deta.ID_MAESTRO, maes.ID_CLIENTE
            //            FROM SCH_CONTRATO deta inner join sch_contrato_maestro maes on DETA.id_maestro = MAES.ID_CONTRATO_MAESTRO
            //            WHERE ID_CONTRATO = 3266";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    var lector = cm.ExecuteReader();
                    while (lector.Read())
                    {
                        AlertaTipo1 = new Objeto.AlertaTipo1();
                        if (lector[0] == DBNull.Value)
                            AlertaTipo1.IdMaestro = -1;
                        else
                            AlertaTipo1.IdMaestro = int.Parse(lector.GetString(0));

                        if (lector[1] == DBNull.Value)
                            AlertaTipo1.FechaFinaliacion  = DateTime.MinValue;
                        else
                            AlertaTipo1.FechaFinaliacion = lector.GetDateTime(1);

                        if (lector[2] == DBNull.Value)
                            AlertaTipo1.NombreCliente = "Sin cliente";
                        else
                            AlertaTipo1.NombreCliente = lector.GetString(2);

                        if (lector[3] == DBNull.Value)
                            AlertaTipo1.IdCliente = "-1";
                        else
                            AlertaTipo1.IdCliente = lector.GetString(3);

                        if (lector[4] == DBNull.Value)
                            AlertaTipo1.Nit = "Sin Nit";
                        else
                            AlertaTipo1.Nit = lector.GetString(4);

                        if (lector[5] == DBNull.Value)
                            AlertaTipo1.TipoCliente = -1;
                        else
                            AlertaTipo1.TipoCliente = lector.GetInt32(5);

                        if (lector[6] == DBNull.Value)
                            AlertaTipo1.NombreTipoCliente = "Sin tipo de cliente";
                        else
                            AlertaTipo1.NombreTipoCliente = lector.GetString(6);

                        if (lector[7] == DBNull.Value)
                            AlertaTipo1.NumeroRubroPresupuestal = "Sin rubro";
                        else
                            AlertaTipo1.NumeroRubroPresupuestal = lector.GetString(7);


                        salida.Add(AlertaTipo1);

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

        public List<Objeto.AlertaTipo2> BarrerContratosTipo2()
        {

            var salida = new List<Objeto.AlertaTipo2>();
            var AlertaTipo2 = new Objeto.AlertaTipo2();
            string sql = @"SELECT     C.ID_MAESTRO CONTRATO,     F_PRODUCTOS(C.ID_MAESTRO) PRODS,     F_NO_RUBRO_PRESUPUESTAL(C.ID_MAESTRO) NUMERO_CONTRATO,     T.DETA101 AS TIPOCLIENTE,
                M.ID_CLIENTE,     M.IDENTIFICACION NIT,    TO_CHAR(C.FECHA_INICIO, 'MM') AS MES,     TO_CHAR(C.FECHA_INICIO, 'YYYY') AS AÑO,     NVL(C.ASIGNACION_PRESUPUESTAL,0),    NVL(C.ASIGNACION_PRESUPUESTAL,0)*0.7 AS CALCULO,     sum(V.VENTAS)     
            FROM SCH_CONTRATO_MAESTRO M, SCH_CONTRATO C , M_TABDES T, SCV_DWH_VTAS V    WHERE 1=1     AND C.CERRADO = 0    AND M.ID_CONTRATO_MAESTRO = C.ID_MAESTRO(+)    AND NOT C.PRODUCTO IS NULL
                AND C.FECHA_INICIO BETWEEN TO_DATE(:P0, 'DD/MM/YYYY') AND TO_DATE(:P1, 'DD/MM/YYYY')
                AND M.ID_TIPO_CLIENTE IN (1, 3, 5, 16) 
                AND M.ID_TIPO_CLIENTE = TO_NUMBER(T.CODDE01) AND 11 = T.CODTA01
                AND '21-' ||M.ID_CLIENTE = V.CLIID AND C.PRODUCTO = V.PRODID
                AND TO_CHAR(C.FECHA_INICIO, 'MM') = V.MES AND TO_CHAR(C.FECHA_INICIO, 'YYYY') = V.ANO
                AND FECHA_FINALIZACION >= SYSDATE group by   C.ID_MAESTRO ,    F_PRODUCTOS(C.ID_MAESTRO) ,    F_NO_RUBRO_PRESUPUESTAL(C.ID_MAESTRO) , 
                T.DETA101 ,     M.ID_CLIENTE,     M.IDENTIFICACION ,     TO_CHAR(C.FECHA_INICIO, 'MM'), TO_CHAR(C.FECHA_INICIO, 'YYYY'), NVL(C.ASIGNACION_PRESUPUESTAL,0), NVL(C.ASIGNACION_PRESUPUESTAL,0)*0.7";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", new DateTime(2010, 01, 01).ToShortDateString() ));
                    cm.Parameters.Add(new OracleParameter("P1", DateTime.Now.ToShortDateString()));
                    cm.CommandTimeout = 360;
                    var lector = cm.ExecuteReader();
                    while (lector.Read())
                    {
                        AlertaTipo2 = new Objeto.AlertaTipo2();
                        if (lector[0] == DBNull.Value)
                            AlertaTipo2.IdContratoMaestro = -1;
                        else
                            AlertaTipo2.IdContratoMaestro = int.Parse(lector.GetString(0));

                        if (lector[1] == DBNull.Value)
                            AlertaTipo2.VestorProductos = "";
                        else
                            AlertaTipo2.VestorProductos = lector.GetString(1);

                        if (lector[2] == DBNull.Value)
                            AlertaTipo2.NumeroContrato = "Sin contrato";
                        else
                            AlertaTipo2.NumeroContrato = lector.GetString(2);

                        if (lector[3] == DBNull.Value)
                            AlertaTipo2.TipoCliente = "Sin tipo de cliente";
                        else
                            AlertaTipo2.TipoCliente = lector.GetString(3);

                        if (lector[4] == DBNull.Value)
                            AlertaTipo2.IdCliente = "Cliente sin identificación";
                        else
                            AlertaTipo2.IdCliente = lector.GetString(4);

                        if (lector[5] == DBNull.Value)
                            AlertaTipo2.Nit = "Sin Nit";
                        else
                            AlertaTipo2.Nit = lector.GetString(5);

                        if (lector[6] == DBNull.Value)
                            AlertaTipo2.Mes = 0;
                        else
                            AlertaTipo2.Mes = int.Parse(lector[6].ToString());// lector.GetInt32(6);

                        if (lector[7] == DBNull.Value)
                            AlertaTipo2.Año = 0;
                        else
                            AlertaTipo2.Año = int.Parse(lector[7].ToString());//lector.GetInt32(7);

                        if (lector[8] == DBNull.Value)
                            AlertaTipo2.AsignacionPresupuestal = -1d;
                        else
                            AlertaTipo2.AsignacionPresupuestal = lector.GetInt64(8);

                        if (lector[9] == DBNull.Value)
                            AlertaTipo2.Calculo = -1d;
                        else
                            AlertaTipo2.Calculo = lector.GetInt64(9);

                        if (lector[10] == DBNull.Value)
                            AlertaTipo2.Ventas = -1d;
                        else
                            AlertaTipo2.Ventas = lector.GetInt64(10);

                        if (AlertaTipo2.Calculo <= AlertaTipo2.Ventas)
                            salida.Add(AlertaTipo2);
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

        private void DefinirFechas(int periodicidad, ref DateTime fechaI, ref DateTime fechaF) {

            int indiceCrecimiento = 0;
            if (periodicidad == 1)      //Mensual
                indiceCrecimiento = 1;
            else if (periodicidad == 2) //BiMensual
                indiceCrecimiento = 2;
            else if (periodicidad == 3) //TriMensual
                indiceCrecimiento = 3;
            else if (periodicidad == 4) //Semestral
                indiceCrecimiento = 6;
            else if (periodicidad == 5) //Anual
                indiceCrecimiento = 12;

            fechaI = new DateTime(fechaI.Year, fechaI.Month, 1);
            fechaF = new DateTime(fechaF.Year, fechaI.AddMonths(indiceCrecimiento).Month, fechaI.AddMonths(indiceCrecimiento + 1).AddDays(-1).Day);

            return; 

        }

        public List<Objeto.Contrato> BarrerContratosPay(int periodicidad)
        {
            DateTime fI = new DateTime();
            DateTime fF = new DateTime();

            DefinirFechas(periodicidad, ref fI, ref fF);

            var salida = new List<Objeto.Contrato>();
            var contrato = new Objeto.Contrato();
            string sql = @"SELECT PRODUCTO,PRECIO_PRODUCTO, CONSUMO_CONTRATADO, C_TOP,TOP,
        ESCALADO, CANTIDAD_ESCALADO, PRECIO_ESCALADO, PERIODICIDAD,
        ALQUILER_TANQUES,ARRIENDO_EQUIPO, CANON_ARRENDAMIENTO,
        GARANTIA,FIRMADO_POR,FECHA_INICIO,FECHA_FIRMADO,
        FECHA_FINALIZACION,FECHA_FINALIZA_PRO,INICIO_SUMINISTRO,
        VIGENCIA_MESES,VIGENCIA_AUTO_MESES,AVISO_TERMINACION_CON,
        CONDICIONES_ESPECIALES,TIEMPO_ESPERA_TRAC,HORAS_MIN_ESPERA,
        CONTRATO_COMODATO,PLAZO_PAGO_DIAS,MULTA_VENCIMIENTO_FAC,
        PORCENTAJE_MULTA,AUMENTO_PRECIO, ID_CONTRATO, ID_MAESTRO, SUM(VEN.VENTTOTAL) VENTA
FROM SCH_CONTRATO CON 
    INNER JOIN SCH_CONTRATO_MAESTRO MAS ON CON.ID_MAESTRO = MAS.ID_CONTRATO_MAESTRO
    INNER JOIN DWHVENTASMES@DECI VEN ON (CLIID =  MAS.ID_COMPANIA || '-' || MAS.ID_CLIENTE AND PRODID = CON.PRODUCTO)
WHERE 1=1 
    AND CON.TOP = 1
    AND CON.PRODUCTO IS NOT NULL 
    AND CON.FECHA_REGISTRO BETWEEN TO_DATE(:P0, 'DD/MM/YYYY') AND TO_DATE(:P1, 'DD/MM/YYYY')
    AND CON.PERIODICIDAD = :P2 
    ADN CON.FECHA_FINALIZA_PRO >= SYSDATE - 30
    AND NOT EXISTS (SELECT 1 FROM SCH_CONTRATO_RUTA RUTA WHERE RUTA.ID_CONTRATO = CON.ID_MAESTRO  )
GROUP BY PRODUCTO,PRECIO_PRODUCTO, CONSUMO_CONTRATADO, C_TOP,TOP,
        ESCALADO, CANTIDAD_ESCALADO, PRECIO_ESCALADO, PERIODICIDAD,
        ALQUILER_TANQUES,ARRIENDO_EQUIPO, CANON_ARRENDAMIENTO,
        GARANTIA,FIRMADO_POR,FECHA_INICIO,FECHA_FIRMADO,
        FECHA_FINALIZACION,FECHA_FINALIZA_PRO,INICIO_SUMINISTRO,
        VIGENCIA_MESES,VIGENCIA_AUTO_MESES,AVISO_TERMINACION_CON,
        CONDICIONES_ESPECIALES,TIEMPO_ESPERA_TRAC,HORAS_MIN_ESPERA,
        CONTRATO_COMODATO,PLAZO_PAGO_DIAS,MULTA_VENCIMIENTO_FAC,
        PORCENTAJE_MULTA,AUMENTO_PRECIO, ID_CONTRATO, ID_MAESTRO ";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", fI.ToShortDateString()));
                    cm.Parameters.Add(new OracleParameter("P1", fF.ToShortDateString()));
                    cm.Parameters.Add(new OracleParameter("P2", periodicidad));

                    var lector = cm.ExecuteReader();
                    while (lector.Read())
                    {
                        contrato = new Objeto.Contrato();
                        if (lector[0] == DBNull.Value)
                            contrato.Producto = "SIN PRODUCTO";
                        else
                            contrato.Producto = lector.GetString(0);

                        if (lector[1] == DBNull.Value)
                            contrato.Precio_producto = 0;
                        else
                            contrato.Precio_producto = lector.GetInt32(1);

                        if (lector[2] == DBNull.Value)
                            contrato.Consumo_contratado = 0;
                        else
                            contrato.Consumo_contratado = lector.GetInt32(2);

                        if (lector[3] == DBNull.Value)
                            contrato.C_top = false;
                        else
                            contrato.C_top = RevalidarValidarBool(lector.GetInt32(3));

                        if (lector[4] == DBNull.Value)
                            contrato.Top = 0;
                        else
                            contrato.Top = lector.GetInt32(4);

                        if (lector[5] == DBNull.Value)
                            contrato.Escalado = false;
                        else
                            contrato.Escalado = RevalidarValidarBool(lector.GetInt32(5));

                        if (lector[6] == DBNull.Value)
                            contrato.Cantidad_escalado = "0";
                        else
                            contrato.Cantidad_escalado = lector.GetString(6);

                        if (lector[7] == DBNull.Value)
                            contrato.Precio_escalado = "0";
                        else
                            contrato.Precio_escalado = lector.GetString(7);

                        if (lector[8] == DBNull.Value)
                            contrato.Periodicidad = 0;
                        else
                            contrato.Periodicidad = lector.GetInt32(8);

                        if (lector[9] == DBNull.Value)
                            contrato.Alquiler_tanques = false;
                        else
                            contrato.Alquiler_tanques = RevalidarValidarBool(lector.GetInt32(9));

                        if (lector[10] == DBNull.Value)
                            contrato.Arriendo_equipo = false;
                        else
                            contrato.Arriendo_equipo = RevalidarValidarBool(lector.GetInt32(10));

                        if (lector[11] == DBNull.Value)
                            contrato.Canon_arrendamiento = 0;
                        else
                            contrato.Canon_arrendamiento = lector.GetInt32(11);

                        if (lector[12] == DBNull.Value)
                            contrato.Garantia = "";
                        else
                            contrato.Garantia = lector.GetString(12);

                        if (lector[13] == DBNull.Value)
                            contrato.Firmado_por = -1;
                        else
                            contrato.Firmado_por = lector.GetInt32(13);

                        if (lector[14] == DBNull.Value)
                            contrato.Fecha_inicio = DateTime.MinValue;
                        else
                            contrato.Fecha_inicio = lector.GetDateTime(14);

                        if (lector[15] == DBNull.Value)
                            contrato.Fecha_firmado = DateTime.MinValue;
                        else
                            contrato.Fecha_firmado = lector.GetDateTime(15);

                        if (lector[16] == DBNull.Value)
                            contrato.Fecha_finalizacion = DateTime.MinValue;
                        else
                            contrato.Fecha_finalizacion = lector.GetDateTime(16);

                        if (lector[17] == DBNull.Value)
                            contrato.Fecha_finaliza_pro = DateTime.MinValue;
                        else
                            contrato.Fecha_finaliza_pro = lector.GetDateTime(17);

                        if (lector["INICIO_SUMINISTRO"] == DBNull.Value)
                        {
                            contrato.Inicio_suministro = DateTime.MinValue;

                        }
                        else
                        {
                            contrato.Inicio_suministro = DateTime.Parse(lector["INICIO_SUMINISTRO"].ToString());

                        }

                        if (lector["ID_CLIENTE"] != DBNull.Value)
                            contrato.Id_cliente = int.Parse(lector["ID_CLIENTE"].ToString());

                        contrato.Inicio_suministro_texto = contrato.Inicio_suministro.ToShortDateString();


                        if (lector[19] == DBNull.Value)
                            contrato.Vigencia_meses = 0;
                        else
                            contrato.Vigencia_meses = lector.GetInt32(19);

                        if (lector[20] == DBNull.Value)
                            contrato.Vigencia_auto_meses = 0;
                        else
                            contrato.Vigencia_auto_meses = lector.GetInt32(20);

                        if (lector[21] == DBNull.Value)
                            contrato.Aviso_terminacion_con = 0;
                        else
                            contrato.Aviso_terminacion_con = lector.GetInt32(21);

                        if (lector[22] == DBNull.Value)
                            contrato.Condiciones_especiales = "";
                        else
                            contrato.Condiciones_especiales = lector.GetString(22);

                        if (lector[23] == DBNull.Value)
                            contrato.Tiempo_espera_trac = false;
                        else
                            contrato.Tiempo_espera_trac = RevalidarValidarBool(lector.GetInt32(23));

                        if (lector[24] == DBNull.Value)
                            contrato.Horas_min_espera = 0;
                        else
                            contrato.Horas_min_espera = lector.GetInt32(24);

                        if (lector[25] == DBNull.Value)
                            contrato.Contrato_comodato = false;
                        else
                            contrato.Contrato_comodato = RevalidarValidarBool(lector.GetInt32(25));

                        if (lector[26] == DBNull.Value)
                            contrato.Plazo_pago_dias = 0;
                        else
                            contrato.Plazo_pago_dias = lector.GetInt32(26);

                        if (lector[27] == DBNull.Value)
                            contrato.Multa_vencimiento_fac = false;
                        else
                            contrato.Multa_vencimiento_fac = RevalidarValidarBool(lector.GetInt32(27));

                        if (lector[28] == DBNull.Value)
                            contrato.Porcentaje_multa = "0";
                        else
                            contrato.Porcentaje_multa = lector.GetString(28);

                        if (lector[29] == DBNull.Value)
                            contrato.Aumento_precio = "";
                        else
                            contrato.Aumento_precio = lector.GetString(29);


                        contrato.Id_contrato = int.Parse(lector[30].ToString());

                        contrato.IdMaestro = int.Parse(lector[31].ToString());
                        salida.Add(contrato);

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

    }
}
