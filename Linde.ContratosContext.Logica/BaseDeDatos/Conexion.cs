using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linde.ContratosContext.Logica.BaseDeDatos
{
    using Oracle.ManagedDataAccess.Client;
    class Conexion
    {
        private static Conexion con;
        private OracleConnection _conexion;
        public OracleConnection ConexionOracle { get => _conexion; }

        private Conexion()
        {
            string cadena = Utilidades.Lecturas.CadenaConexion();
            _conexion = new OracleConnection(cadena);
            //_conexion.Open();
        }

        public static Conexion GetConnection
        {
            get
            {
                if (con == null)
                    con = new Conexion();
                return con;
            }
        }


    }
}
