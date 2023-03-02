using Linde.Contratos.LogicaV2.Utilidades;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Linde.Contratos.LogicaV2.Dalc
{
    class AlertaMensaje
    {
        public List<Objeto.AlertaMensaje> ObtenerMensajes(int idMaestro, int idContrato) {
            List<Objeto.AlertaMensaje> salida = new List<Objeto.AlertaMensaje>();
//            string sql = @"select ALERT.ID, ALERT.FECHA_REGISTRO, TIP.ID, TIP.DESCRIPCION_ALERTA, ALERT.MENSAJE  
//from SCH_ALERTAS_CONTRATO ALERT INNER JOIN SCB_TIPO_ALERTA TIP ON (ALERT.TIPO_ALERTA_ID = TIP.ID )
//WHERE ALERT.ID_MAESTRO = " + idContrato +  " AND ALERT.ID_CONTATO  = " + idMaestro + " AND ALERT.LEIDO = 0";
            string sql = @"select ALERT.ID, ALERT.FECHA_REGISTRO, TIP.ID, TIP.DESCRIPCION_ALERTA, ALERT.MENSAJE  
from SCH_ALERTAS_CONTRATO ALERT INNER JOIN SCB_TIPO_ALERTA TIP ON (ALERT.TIPO_ALERTA_ID = TIP.ID )
WHERE ALERT.ID_MAESTRO = " + idMaestro + " AND ALERT.LEIDO = 0";
            OracleConnection cx = new OracleConnection();
            //cx = new BaseDeDatos.ConexionSingleton().ObtenerConexion() ;
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();


            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", idMaestro));
                    cm.Parameters.Add(new OracleParameter("P1", idContrato));

                    var lector = cm.ExecuteReader();

                    while (lector.Read())
                    {
                        Objeto.AlertaMensaje ale = new Objeto.AlertaMensaje();
                        ale.Id = lector.GetInt32(0);
                        ale.FechaRegistro = lector.GetDateTime(1);
                        ale.IdTipoAlerta = lector.GetInt32(2);
                        ale.DesTipoAlerta = lector.GetString(3);
                        ale.Mensaje = lector.GetString(4);
                        ale.IdMaestro = idMaestro;
                        ale.IdContrato = idContrato;
                        salida.Add(ale);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.Out.WriteLine(ex.ToString());
                salida = new List<Objeto.AlertaMensaje>();
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


        public List<Objeto.AlertaMensaje> ObtenerMensajesFechas(DateTime fI, DateTime fF)
        {
            string fechaI = fI.Day.ToString().PadLeft(2, '0') + "-" + fI.Month.ToString().PadLeft(2, '0') + "-" + fI.Year.ToString().PadLeft(4, '0');
            string fechaF = fF.Day.ToString().PadLeft(2, '0') + "-" + fF.Month.ToString().PadLeft(2, '0') + "-" + fF.Year.ToString().PadLeft(4, '0');


            List<Objeto.AlertaMensaje> salida = new List<Objeto.AlertaMensaje>();
            string sql = @"select ALERT.ID, ALERT.FECHA_REGISTRO, TIP.ID, TIP.DESCRIPCION_ALERTA, ALERT.MENSAJE, ALERT.ID_MAESTRO, ALERT.iD_CONTATO  
from SCH_ALERTAS_CONTRATO ALERT INNER JOIN SCB_TIPO_ALERTA TIP ON (ALERT.TIPO_ALERTA_ID = TIP.ID )
WHERE TO_DATE(ALERT.fecha_registro,'DD/MM/YYYY' ) >= '" + fechaI + "' AND TO_DATE(ALERT.fecha_registro,'DD/MM/YYYY' ) <= '" + fechaF + "' AND ALERT.LEIDO = 0";
            OracleConnection cx = new OracleConnection();
            //cx = new BaseDeDatos.ConexionSingleton().ObtenerConexion() ;
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();


            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    //cm.Parameters.Add(new OracleParameter("P0", fI));
                    //cm.Parameters.Add(new OracleParameter("P1", fF));

                    var lector = cm.ExecuteReader();

                    while (lector.Read())
                    {
                        Objeto.AlertaMensaje ale = new Objeto.AlertaMensaje();
                        ale.Id = lector.GetInt32(0);
                        ale.FechaRegistro = lector.GetDateTime(1);
                        ale.IdTipoAlerta = lector.GetInt32(2);
                        ale.DesTipoAlerta = lector.GetString(3);
                        ale.Mensaje = lector.GetString(4);
                        ale.IdMaestro = lector.GetInt32(5);
                        if (lector[6] == DBNull.Value)
                            ale.IdContrato = -1;
                        else
                            ale.IdContrato = lector.GetInt32(6);
                        salida.Add(ale);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.Out.WriteLine(ex.ToString());
                salida = new List<Objeto.AlertaMensaje>();
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

        //LEGION VALIDAR
        public bool ObtenerMensajesTipo1(int IdContratoProducto)
        {
            List<Objeto.AlertaMensaje> salida = new List<Objeto.AlertaMensaje>();
            string sql = @"select ALERT.ID, ALERT.FECHA_REGISTRO, TIP.ID, TIP.DESCRIPCION_ALERTA, ALERT.MENSAJE  
from SCH_ALERTAS_CONTRATO ALERT     INNER JOIN SCB_TIPO_ALERTA TIP ON (ALERT.TIPO_ALERTA_ID = TIP.ID )
WHERE ALERT.FECHA_REGISTRO >= :P0 AND ALERT.FECHA_REGISTRO <= :P1 AND ALERT.LEIDO = 0";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    //cm.Parameters.Add(new OracleParameter("P0", fechaI));
                    //cm.Parameters.Add(new OracleParameter("P1", fechaF));

                    var lector = cm.ExecuteReader();

                    while (lector.Read())
                    {
                        Objeto.AlertaMensaje ale = new Objeto.AlertaMensaje();
                        ale.Id = lector.GetInt32(0);
                        ale.FechaRegistro = lector.GetDateTime(1);
                        ale.IdTipoAlerta = lector.GetInt32(2);
                        ale.DesTipoAlerta = lector.GetString(3);
                        ale.Mensaje = lector.GetString(4);
                        salida.Add(ale);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.Out.WriteLine(ex.ToString());
                salida = new List<Objeto.AlertaMensaje>();
            }
            finally
            {
                if (cx != null)
                {
                    cx.Close();
                    cx.Dispose();
                }
            }
            return true;
        }

        public bool AdicionarMensajes(int idMaestro, int idContrato, int tipo, string de, string para )
        {
            bool salida = false;
            Objeto.TipoAlerta objetoMensaje = new Dalc.TipoAlerta().ObtenerTipoAlerta().Where(x=>x.Id == tipo).ToList().FirstOrDefault();
            string alerta = objetoMensaje.Id + " " + objetoMensaje.Mensaje;
            string mensaje = objetoMensaje.Mensaje;
            mensaje = mensaje.Replace("{0}", para);
            mensaje = mensaje.Replace("{1}", tipo.ToString());
            mensaje = mensaje.Replace("{2}", idMaestro.ToString() + "-" + idContrato);


            string sql = @"insert into SCH_ALERTAS_CONTRATO(ID,FECHA_REGISTRO,MENSAJE,LEIDO,ID_MAESTRO,ID_CONTATO, ENVIADO, DE, PARA, TIPO_ALERTA_ID) 
VALUES(SEQ_SCH_ALERTAS_CONTRATO.NEXTVAL, SYSDATE, :P0, 0, :P1, :P2, 0, :P3, :P4, :P5)";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", mensaje));
                    cm.Parameters.Add(new OracleParameter("P1", idContrato));
                    cm.Parameters.Add(new OracleParameter("P2", idMaestro));
                    cm.Parameters.Add(new OracleParameter("P3", de));
                    cm.Parameters.Add(new OracleParameter("P4", para));
                    cm.Parameters.Add(new OracleParameter("P5", tipo));

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
                    var con = new Contrato();
                    salida = con.ActualizarAlertaContrato(idMaestro, idContrato, tipo);
                    //var correo = con.CorreoVendedor(idMaestro);
                    //var contendo = new Dalc.TipoAlerta().ObtenerTipoAlerta().Where( x=> x.Id == tipo).FirstOrDefault().Mensaje;
                    //salida = new Envio().AdicionarRegistro("PAGOELECTRONICO", 99, correo, contendo, "Alerta contratos");
                }
            }
            return salida;
        }

        public bool AdicionarMensajesTipo1(int idMaestro, string cliente, int tipo, string de, string para )
        {
            bool salida = false;
            Objeto.TipoAlerta objetoMensaje = new Dalc.TipoAlerta().ObtenerTipoAlerta().Where(x=>x.Id == tipo).ToList().FirstOrDefault();
            string alerta = objetoMensaje.Id + " " + objetoMensaje.Mensaje;
            string mensaje = objetoMensaje.Mensaje;
            mensaje = mensaje.Replace("{0}", cliente);
            mensaje = mensaje.Replace("{1}", tipo.ToString());
            mensaje = mensaje.Replace("{2}", idMaestro.ToString());


            string sql = @"insert into SCH_ALERTAS_CONTRATO(ID,FECHA_REGISTRO,MENSAJE,LEIDO,ID_MAESTRO,ID_CONTATO, ENVIADO, DE, PARA, TIPO_ALERTA_ID) 
VALUES(SEQ_SCH_ALERTAS_CONTRATO.NEXTVAL, SYSDATE, :P0, 0, :P1, NULL, 0, :P3, :P4, :P5)";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", mensaje));
                    cm.Parameters.Add(new OracleParameter("P1", idMaestro));
                    //cm.Parameters.Add(new OracleParameter("P2", ));
                    cm.Parameters.Add(new OracleParameter("P3", de));
                    cm.Parameters.Add(new OracleParameter("P4", para));
                    cm.Parameters.Add(new OracleParameter("P5", tipo));

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
                    var con = new Contrato();
                    salida = con.ActualizarAlertaContrato(idMaestro, -1, tipo);
                    //var correo = con.CorreoVendedor(idMaestro);
                    //var contendo = new Dalc.TipoAlerta().ObtenerTipoAlerta().Where( x=> x.Id == tipo).FirstOrDefault().Mensaje;
                    //salida = new Envio().AdicionarRegistro("PAGOELECTRONICO", 99, correo, contendo, "Alerta contratos");
                }
            }
            return salida;
        }
    
        
    }
}
