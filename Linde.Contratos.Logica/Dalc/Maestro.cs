using Linde.Contratos.LogicaV2.Objeto;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linde.Contratos.LogicaV2.Dalc
{
    class Maestro
    {
        public int CrearContrato(Objeto.Maestro contrato) {

            var cli = Utilidades.Lecturas.ExtraeCodigoCadena(contrato.Cliente);
            contrato.Cliente = cli;
            int salida = -1;
            string sql = "INSERT INTO SCH_CONTRATO_MAESTRO(ID_CONTRATO_MAESTRO,ID_COMPANIA,ID_TIPO_CLIENTE,ID_CLIENTE,IDENTIFICACION,CORREO) VALUES(SEQ_SCH_CONTRATO_MAESTRO.NEXTVAL,:P0,:P1,:P2,:P3,:P4)";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", contrato.IdCompania));
                    cm.Parameters.Add(new OracleParameter("P1", contrato.TipoCliente ));
                    cm.Parameters.Add(new OracleParameter("P2", contrato.Cliente));
                    cm.Parameters.Add(new OracleParameter("P3", contrato.Identificacion));
                    cm.Parameters.Add(new OracleParameter("P4", contrato.Correo));

                    int escritor = cm.ExecuteNonQuery();

                    if (escritor > 0)
                    {
                        salida = UltimoMaestro(contrato);
                    }
                }
            }
            catch(Exception ex)
            {
                salida = -1;
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

        private int UltimoMaestro(Objeto.Maestro contrato) {
            int salida = 0;
            string sql = "SELECT MAX(ID_CONTRATO_MAESTRO) AS CONTRATO FROM SCH_CONTRATO_MAESTRO WHERE ID_CLIENTE = :P0 AND IDENTIFICACION = :P1 ";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", contrato.Cliente));
                    cm.Parameters.Add(new OracleParameter("P1", contrato.Identificacion));

                    var escritor = cm.ExecuteReader();

                    if (escritor.Read())
                    {
                        salida = int.Parse(escritor["CONTRATO"].ToString());
                    }
                }
            }
            catch(Exception ex)
            {
                salida = -1;
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


        public List<Objeto.Busqueda> ListarMaestros(int emp, string cliente) {
            List<Objeto.Busqueda> salida = new List<Objeto.Busqueda>();
            string sql = @"SELECT MAES.id_contrato_maestro, MAES.FECHA_REGISTRO, CONT.ID_CONTRATO, ART.ARTDSCCORTA, (CONT.ALERTA + CONT.ALERTA2 +  CONT.ALERTA2  )
FROM sch_contrato_maestro MAES INNER JOIN sch_contrato CONT ON (MAES.id_contrato_maestro = CONT.ID_MAESTRO)
     INNER JOIN CS2000.stkarticulo ART ON (ART.ARTCODE = CONT.PRODUCTO)
WHERE ART.EMPID = 21 AND MAES.ID_COMPANIA = :P0 AND MAES.ID_CLIENTE = :P1 and  CONT.CERRADO IN (0, -1)
ORDER BY MAES.id_contrato_maestro, CONT.ID_CONTRATO";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", emp));
                    cm.Parameters.Add(new OracleParameter("P1", cliente));

                    var lector = cm.ExecuteReader();

                    while (lector.Read())
                    {
                        Busqueda b = new Busqueda();
                        b.IdMaestro = lector.GetInt32(0);
                        b.Registro = lector.GetString(1);
                        b.IdContratoProducto = lector.GetInt32(2);
                        b.Producto = lector.GetString(3);
                        if (lector.GetInt32(4) > 0)
                            b.Alerta = "<button id = 'BtnBuscarAlertas' onClick = 'BuscarAlertas(this);'><i class='fa fa-bell'> " + lector.GetInt32(4) + "</i></button>"; // lector.GetInt32(4); 
                        else
                            b.Alerta = "-"; 
                        salida.Add(b);
                    }
                }
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
            return salida;
        }
    } 
}
