using System;
using System.Collections.Generic;
using Linde.Contratos.LogicaV2.Objeto;
using Oracle.ManagedDataAccess.Client;

namespace Linde.Contratos.LogicaV2.Dalc
{
    class Cliente
    {

        private void AdicionarPrimerRegistro(ref List<Objeto.Cliente> lista)
        {
            lista.Add(new Objeto.Cliente() { Descripcion = "0 - Seleccione..." });
        }

        public string Vendedor(int compania, string codigoCliente)
        {
            var salida = "";
            string sql = "";
            if (compania == 21)
                sql = "SELECT sprax_busco_tabdes(16, vended_cli) VENDEDOR FROM M_DF01 WHERE estado_cli = '01' AND cp_exp_cli is not null AND CLIENT_CLI = :P0";
            else if (compania == 22)
                sql = "SELECT sprax_busco_tabdes@COPG(16, vended_cli) VENDEDOR FROM M_DF01@COPG WHERE estado_cli = '01' AND cp_exp_cli is not null AND CLIENT_CLI = :P0";
            else
                sql = "SELECT sprax_busco_tabdes@COLC(16, vended_cli) VENDEDOR FROM M_DF01@COLC WHERE estado_cli = '01' AND cp_exp_cli is not null AND CLIENT_CLI = :P0";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", codigoCliente));
                    OracleDataReader lector = cm.ExecuteReader();

                    while (lector.Read())
                    {
                        salida = lector["VENDEDOR"].ToString();
                    }
                }
            }
            catch
            {
                return "Error ID Vendedor";
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

        public string CorreoVendedor(string codigoCliente, int compania, string iAgencia)
        {
            var salida = System.Configuration.ConfigurationManager.AppSettings["De"];
            var link = "";
            if (compania != 21)
                link = "@COLC";

            string sql = "";

            sql = "SELECT CORRE.CORREO_VENDEDOR as VENDEDOR FROM M_DF01" + link + " CLI INNER JOIN SCB_CORREO_VENDEROR CORRE on (TO_NUMBER(CORRE.ID_VENDEDOR) = TO_NUMBER(CLI.vended_cli)) WHERE CORRE.TIPO = 1 AND CLI.CLIENT_CLI = :P0 AND CORRE.ID_EMP = :P1";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", codigoCliente.PadLeft(7, '0')));
                    cm.Parameters.Add(new OracleParameter("P1", compania));
                    OracleDataReader lector = cm.ExecuteReader();

                    while (lector.Read())
                    {
                        salida = lector["VENDEDOR"].ToString();
                    }
                }
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
            return salida;
        }

        public string NitCliente(int compania, string codigoCliente) {
            var salida = "";
            string sql = "";
            if (compania == 21)
                sql = "SELECT NRODOC_CLI FROM M_DF01 WHERE cp_exp_cli is not null AND CLIENT_CLI = :P0";
            else if (compania == 22)
                sql = "SELECT NRODOC_CLI FROM M_DF01@COPG WHERE cp_exp_cli is not null AND CLIENT_CLI = :P0";
            else
                sql = "SELECT NRODOC_CLI FROM M_DF01@COLC WHERE cp_exp_cli is not null AND CLIENT_CLI = :P0";
            OracleConnection cx = new OracleConnection();
            cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            try
            {
                using (OracleCommand cm = new OracleCommand(sql, cx))
                {
                    cm.Parameters.Add(new OracleParameter("P0", codigoCliente));
                    OracleDataReader lector = cm.ExecuteReader();

                    while (lector.Read())
                    {
                        salida = lector["NRODOC_CLI"].ToString();
                    }
                }
            }
            catch
            {
                return "Error ID cliente";
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
        public void ListarClientes(bool paraCombo, ref List<Objeto.Cliente> lista)
        {
            if (paraCombo)
                AdicionarPrimerRegistro(ref lista);
            //string sql = "SELECT 21 as EMPID, CLIENT_CLI AS CODIGO, razon__cli AS DESCRIPCION, TIPCLI_CLI as TCLI FROM M_DF01 WHERE estado_cli = '01' AND cp_exp_cli is not null " +
            //    "UNION " +
            //    "SELECT 22 as EMPID, CLIENT_CLI AS CODIGO, razon__cli AS DESCRIPCION, TIPCLI_CLI as TCLI FROM M_DF01@COPG WHERE estado_cli = '01' AND cp_exp_cli is not null and COSEGURO_CLI != 'N' AND PRINCIPAL_CLI !='N'";
            string sql = "SELECT 21 as EMPID, CLIENT_CLI AS CODIGO, razon__cli AS DESCRIPCION, TIPCLI_CLI as TCLI FROM M_DF01 WHERE cp_exp_cli is not null and COSEGURO_CLI = 'N' AND PRINCIPAL_CLI ='N'" + 
                "UNION " +
                "SELECT 21 as EMPID, CLIENT_CLI AS CODIGO, razon__cli AS DESCRIPCION, TIPCLI_CLI as TCLI FROM M_DF01 WHERE cp_exp_cli is not null and COSEGURO_CLI = 'S' AND PRINCIPAL_CLI ='S'" +
                "UNION " +
                "SELECT 22 as EMPID, CLIENT_CLI AS CODIGO, razon__cli AS DESCRIPCION, TIPCLI_CLI as TCLI FROM M_DF01@COPG WHERE cp_exp_cli is not null and COSEGURO_CLI = 'N' AND PRINCIPAL_CLI ='N'" +
                "UNION " +
                "SELECT 25 as EMPID,CLIENT_CLI AS CODIGO, razon__cli AS DESCRIPCION, TIPCLI_CLI as TCLI FROM M_DF01@COLC WHERE cp_exp_cli is not null and COSEGURO_CLI = 'N' AND PRINCIPAL_CLI ='N'";

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
                        lista.Add(new Objeto.Cliente() { Descripcion = lector["CODIGO"].ToString() + " - " + lector["DESCRIPCION"].ToString(), Compania = int.Parse(lector["EMPID"].ToString()), TipoCliente = lector["TCLI"].ToString() });
                    }
                }
            }
            catch(Exception ex)
            {
                System.Console.Out.WriteLine(ex.ToString());
                return ;
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
