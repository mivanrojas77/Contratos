using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linde.Contratos.LogicaV2.Dalc
{
    public class Informe
    {
        public List<Objeto.Informe> ObteneInforme(DateTime fI, DateTime fF)
        {
            List<Objeto.Informe> salida = new List<Objeto.Informe>();

            string sql = "";

            sql = @"SELECT DISTINCT C, ID_CLIENTE    ,IDENTIFICACION    ,FECHA_REGISTRO    ,RAZON__CLI    ,ID_CONTRATO    ,
FECHA_REC_CONRATO_PRODUCTO    ,PRODUCTO    ,CONSUMO_CONTRATADO    ,PRECIO_PRODUCTO    ,
C_TOP    ,TOP    ,ESCALADO    ,CANTIDAD_ESCALADO    ,
PRECIO_ESCALADO    ,PERIODICIDAD    ,ALQUILER_TANQUES    ,ARRIENDO_EQUIPO    ,CANON_ARRENDAMIENTO    ,
FECHA_INICIO    ,FECHA_FIRMADO    ,FECHA_FINALIZACION    ,GARANTIA    ,
FIRMADO_POR    ,FECHA_FINALIZA_PRO    ,AVISO_TERMINACION_CON    ,INICIO_SUMINISTRO    ,CONDICIONES_ESPECIALES    ,VIGENCIA_MESES    ,
VIGENCIA_AUTO_MESES    ,TIEMPO_ESPERA_TRAC    ,HORAS_MIN_ESPERA    ,CONTRATO_COMODATO    ,
PLAZO_PAGO_DIAS    ,MULTA_VENCIMIENTO_FAC    ,PORCENTAJE_MULTA    ,AUMENTO_PRECIO    ,
DELTAEE    ,DELTAIP    ,DELTADIESEL    ,DELTAIPP    ,DELTAGAS    ,DELTATRM    ,
DELTAOTROS    ,DIESEL    ,IPC    ,USD    ,ENERGIA    ,OTROS    ,
VALOR_ANO1    ,VALOR_ANO2    ,VALOR_ANO3    ,VALOR_ANO4    ,
VALOR_ANO5    ,OTROSI    ,VIGENCIA    ,
ESTATAL    ,ASIGNACION_PRESUPUESTAL    ,NO_RUBRO_PRESUPUESTAL    ,CERTIFICADO_DISPONIBLE    ,ACTA_FECHA_INICIO    ,
NUMERO_FACTURA_INICIAL    ,NUMERO_FACTURA_FINAL    ,ID_CONTRATO_    ,SERIAL    ,
ID_MAESTRO    ,ALERTA    ,ALERTA2    ,ALERTA3    ,
CERRADO    ,USUARIO    ,ID_OTROSI    ,ID_CONTRATO_PRODUCTO    ,
FECHA_ACTA    ,TOTAL_ADICIONES    ,TOTAL_PRORROGA_MES    ,ID_ANEXO    ,ANEXO    
FROM SCV_INFORME
WHERE FECHA_REGISTRO >= :P0 AND FECHA_REGISTRO <= :P1 AND CERRADO != 1 ";

            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", fI));
                    cm.Parameters.Add(new OracleParameter("P1", fF));
                    
                    OracleDataReader lector = cm.ExecuteReader();

                    while (lector.Read())
                    {
                        salida.Add(new Objeto.Informe()
                        {
                            C = lector["C"].ToString(),
                            ID_CLIENTE = lector["ID_CLIENTE"].ToString(),
                            IDENTIFICACION = lector["IDENTIFICACION"].ToString(),
                            FECHA_REGISTRO = DateTime.Parse(lector["FECHA_REGISTRO"].ToString()),
                            RAZON__CLI = lector["RAZON__CLI"].ToString(),
                            ID_CONTRATO = lector["ID_CONTRATO"].ToString(),
                            FECHA_REC_CONRATO_PRODUCTO = lector["FECHA_REC_CONRATO_PRODUCTO"].ToString(),
                            PRODUCTO = lector["PRODUCTO"].ToString(),
                            CONSUMO_CONTRATADO = lector["CONSUMO_CONTRATADO"].ToString(),
                            PRECIO_PRODUCTO = lector["PRECIO_PRODUCTO"].ToString(),
                            C_TOP = lector["C_TOP"].ToString(),
                            TOP = lector["TOP"].ToString(),
                            ESCALADO = lector["ESCALADO"].ToString(),
                            CANTIDAD_ESCALADO = lector["CANTIDAD_ESCALADO"].ToString(),
                            PRECIO_ESCALADO = lector["PRECIO_ESCALADO"].ToString(),
                            PERIODICIDAD = lector["PERIODICIDAD"].ToString(),
                            ALQUILER_TANQUES = lector["ALQUILER_TANQUES"].ToString(),
                            ARRIENDO_EQUIPO = lector["ARRIENDO_EQUIPO"].ToString(),
                            CANON_ARRENDAMIENTO = lector["CANON_ARRENDAMIENTO"].ToString(),
                            FECHA_INICIO = lector["FECHA_INICIO"].ToString(),
                            FECHA_FIRMADO = lector["FECHA_FIRMADO"].ToString(),
                            FECHA_FINALIZACION = lector["FECHA_FINALIZACION"].ToString(),
                            GARANTIA = lector["GARANTIA"].ToString(),
                            FIRMADO_POR = lector["FIRMADO_POR"].ToString(),
                            FECHA_FINALIZA_PRO = lector["FECHA_FINALIZA_PRO"].ToString(),
                            AVISO_TERMINACION_CON = lector["AVISO_TERMINACION_CON"].ToString(),
                            INICIO_SUMINISTRO = lector["INICIO_SUMINISTRO"].ToString(),
                            CONDICIONES_ESPECIALES = lector["CONDICIONES_ESPECIALES"].ToString(),
                            VIGENCIA_MESES = lector["VIGENCIA_MESES"].ToString(),
                            VIGENCIA_AUTO_MESES = lector["VIGENCIA_AUTO_MESES"].ToString(),
                            TIEMPO_ESPERA_TRAC = lector["TIEMPO_ESPERA_TRAC"].ToString(),
                            HORAS_MIN_ESPERA = lector["HORAS_MIN_ESPERA"].ToString(),
                            CONTRATO_COMODATO = lector["CONTRATO_COMODATO"].ToString(),
                            PLAZO_PAGO_DIAS = lector["PLAZO_PAGO_DIAS"].ToString(),
                            MULTA_VENCIMIENTO_FAC = lector["MULTA_VENCIMIENTO_FAC"].ToString(),
                            PORCENTAJE_MULTA = lector["PORCENTAJE_MULTA"].ToString(),
                            AUMENTO_PRECIO = lector["AUMENTO_PRECIO"].ToString(),
                            DELTAEE = ValidarNulo(lector["DELTAEE"].ToString()),
                            DELTAIP = ValidarNulo(lector["DELTAIP"].ToString()),
                            DELTADIESEL = ValidarNulo(lector["DELTADIESEL"].ToString()),
                            DELTAIPP = ValidarNulo(lector["DELTAIPP"].ToString()),
                            DELTAGAS = ValidarNulo(lector["DELTAGAS"].ToString()),
                            DELTATRM = ValidarNulo(lector["DELTATRM"].ToString()),
                            DELTAOTROS = ValidarNulo(lector["DELTAOTROS"].ToString()),
                            DIESEL = ValidarNulo(lector["DIESEL"].ToString()),
                            IPC = ValidarNulo(lector["IPC"].ToString()),
                            USD = ValidarNulo(lector["USD"].ToString()),
                            ENERGIA = ValidarNulo(lector["ENERGIA"].ToString()),
                            OTROS = ValidarNulo(lector["OTROS"].ToString()),
                            VALOR_ANO1 = ValidarNulo(lector["VALOR_ANO1"].ToString()),
                            VALOR_ANO2 = ValidarNulo(lector["VALOR_ANO2"].ToString()),
                            VALOR_ANO3 = ValidarNulo(lector["VALOR_ANO3"].ToString()),
                            VALOR_ANO4 = ValidarNulo(lector["VALOR_ANO4"].ToString()),
                            VALOR_ANO5 = ValidarNulo(lector["VALOR_ANO5"].ToString()),
                            OTROSI = ValidarNulo(lector["OTROSI"].ToString()),
                            VIGENCIA = ValidarNulo(lector["VIGENCIA"].ToString()),
                            ESTATAL = ValidarNulo(lector["ESTATAL"].ToString()),
                            ASIGNACION_PRESUPUESTAL = ValidarNulo(lector["ASIGNACION_PRESUPUESTAL"].ToString()),
                            NO_RUBRO_PRESUPUESTAL = ValidarNulo(lector["NO_RUBRO_PRESUPUESTAL"].ToString()),
                            CERTIFICADO_DISPONIBLE = ValidarNulo(lector["CERTIFICADO_DISPONIBLE"].ToString()),
                            ACTA_FECHA_INICIO = ValidarNulo(lector["ACTA_FECHA_INICIO"].ToString()),
                            NUMERO_FACTURA_INICIAL = ValidarNulo(lector["NUMERO_FACTURA_INICIAL"].ToString()),
                            NUMERO_FACTURA_FINAL = ValidarNulo(lector["NUMERO_FACTURA_FINAL"].ToString()),
                            ID_CONTRATO_ = ValidarNulo(lector["ID_CONTRATO_"].ToString()),
                            SERIAL = ValidarNulo(lector["SERIAL"].ToString()),
                            ID_MAESTRO = ValidarNulo(lector["ID_MAESTRO"].ToString()),
                            ALERTA = ValidarNulo(lector["ALERTA"].ToString()),
                            ALERTA2 = ValidarNulo(lector["ALERTA2"].ToString()),
                            ALERTA3 = ValidarNulo(lector["ALERTA3"].ToString()),
                            CERRADO = ValidarNulo(lector["CERRADO"].ToString()),
                            USUARIO = ValidarNulo(lector["USUARIO"].ToString()),
                            ID_OTROSI = ValidarNulo(lector["ID_OTROSI"].ToString()),
                            ID_CONTRATO_PRODUCTO = ValidarNulo(lector["ID_CONTRATO_PRODUCTO"].ToString()),
                            FECHA_ACTA = ValidarNulo(lector["FECHA_ACTA"].ToString()),
                            TOTAL_ADICIONES = ValidarNulo(lector["TOTAL_ADICIONES"].ToString()),
                            TOTAL_PRORROGA_MES = ValidarNulo(lector["TOTAL_PRORROGA_MES"].ToString()),
                            ID_ANEXO = ValidarNulo(lector["ID_ANEXO"].ToString()),
                            ANEXO = ValidarNulo(lector["ANEXO"].ToString())
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

        private static string ValidarNulo(object valor) {
            if (valor == null)
                return "";
            return  valor.ToString();
        }
    }
}