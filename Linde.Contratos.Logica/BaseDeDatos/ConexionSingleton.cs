using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace Linde.Contratos.LogicaV2.BaseDeDatos
{
    class ConexionSingleton
    {
        //private OracleConnection _conexion;
        //public OracleConnection Conexion { get => _conexion;}

        //private static ConexionSingleton _conexionSingleton;
        //private ConexionSingleton() {
        //    string cadena = Utilidades.Lecturas.CadenaConexion();
        //    _conexion = new OracleConnection(cadena);
        //}

        //public static ConexionSingleton GetConexionSingleton{
        //    get {
        //        if (_conexionSingleton == null)
        //            _conexionSingleton = new ConexionSingleton();
        //        return _conexionSingleton;
        //    }
        //}

        private OracleConnection _conexion;

        public OracleConnection GetConexionSingleton()
        {
            string cadena = Utilidades.Lecturas.CadenaConexion();
            _conexion = new OracleConnection(cadena);
            _conexion.Open();
            return _conexion;
        }
    }
}
