using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linde.Contratos.LogicaV2.Utilidades
{
    public class Lecturas
    {
        public static String CadenaConexion()
        {
            return ConfigurationManager.ConnectionStrings["Prax"].ToString();
        }

        protected static String Llave(string Id)
        {
            try
            {
                return ConfigurationManager.AppSettings[Id].ToString();
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        public static string ExtraeCodigoCadena(string cadena) {
            int indice = cadena.IndexOf('-');
            string salida = "";
            if (indice >= 0){
                salida = cadena.Substring(0, indice - 1).Trim();
            }
            else { 
                salida = "-1";
            }
            return salida;
        }

        public static void EscribirLog(string mensaje)
        {
            DateTime fecha = new DateTime();
            fecha = DateTime.Now;
            try
            {
                using (System.IO.StreamWriter esc = new System.IO.StreamWriter(@"c:\temp\LogDigv2.txt", true)) {
                    esc.WriteLine(fecha.ToShortDateString() + " " + fecha.ToShortTimeString() + ": " + mensaje);
                    esc.Close();
                }
            }
            catch (Exception ex){
                System.Console.WriteLine("Error log:" + ex.ToString());
            }

        }
    }
}
