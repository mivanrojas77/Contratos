using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linde.ContratosContext.Logica.Utilidades
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

        public static string ExtraeCodigoCadena(string cadena)
        {
            int indice = cadena.IndexOf('-');
            string salida = "";
            if (indice >= 0)
            {
                salida = cadena.Substring(0, indice - 1).Trim();
            }
            else
            {
                salida = "-1";
            }
            return salida;
        }
    }
}
