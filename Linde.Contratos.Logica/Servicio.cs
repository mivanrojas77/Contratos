using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linde.Contratos.LogicaV2
{
    public class Servicio
    {
        public static bool InitBase() {
            var cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();
            return (cx.State == System.Data.ConnectionState.Open);
        }

        public static bool Ping()
        {
            bool salida = false;
            var cx = new BaseDeDatos.ConexionSingleton().GetConexionSingleton();

            using (OracleCommand cm = new OracleCommand("SELECT SYSDATE FROM DUAL", cx))
            {
                OracleDataReader lector = cm.ExecuteReader();

                while (lector.Read())
                {
                    salida = (lector[0].ToString().Length > 0);
                }
            }
            cx.Close();
            cx.Dispose();

            return (salida);
        }


        public static void ListarProducto(ref List<Objeto.Producto> lista) {
            new Dalc.Producto().ListarProducto(ref lista);
            return;
        }
        public static void ListarCliente(ref List<Objeto.Cliente> lista)
        {
            if (lista == null)
                lista = new List<Objeto.Cliente>();
            new Dalc.Cliente().ListarClientes(true, ref lista);
            return;
        }

        public static List<Objeto.Basica> ListarTipoCliente(int compania) {
            return new Dalc.Basica().ListarTipoCliente(compania);
        }

        public static string NitCliente(int compania, string idCliente) {
            return new Dalc.Cliente().NitCliente(compania, idCliente);
        }

        public static string Vendendor(int compania, string idCliente)
        {
            return new Dalc.Cliente().Vendedor(compania, idCliente);
        }

        public static string CorreoVendedor(string idCliente, int compania) {
            return new Dalc.Cliente().CorreoVendedor(idCliente, compania, "");
        }

        public static int GuardarContrato(int contrato) {
            return new Dalc.Contrato().GuardarContrato(contrato);
        }

        public static int GuardarContratoMaestro(Objeto.Maestro contrato) {
            return new Dalc.Maestro().CrearContrato(contrato);
        }

        public static bool GuardarDetalle(int contratoProducto, Objeto.Contrato contrato) {
            return new Dalc.Contrato().GuardarDetalle(contratoProducto, contrato);
        }

        public static bool GuardarDelta(int contratoProducto, Objeto.Contrato contrato)
        {
            return new Dalc.Contrato().GuardarDelta(contratoProducto, contrato);
        }

        public static bool GuardarPrecio(int contratoProducto, Objeto.Contrato contrato)
        {
            return new Dalc.Contrato().GuardarPrecio(contratoProducto, contrato);
        }

        public static List<Objeto.Busqueda> ListarMaestros(int emp, string cliente) {
            return new Dalc.Maestro().ListarMaestros(emp, cliente);
        }

        public static Objeto.Contrato ObtenerDetalle(int contratoProducto)
        {
            return new Dalc.Contrato().ObtenerDetalle(contratoProducto);
        }

        public static Objeto.Contrato ObtenerDelta(int contratoProducto)
        {
            return new Dalc.Contrato().ObtenerDelta(contratoProducto);
        }

        public static Objeto.Contrato ObtenerPrecio(int contratoProducto)
        {
            return new Dalc.Contrato().ObtenerPrecio(contratoProducto);
        }

        public static bool CrearAnexo(int idContrato, string infoAnexo) {
            var cadenaWeb = System.Configuration.ConfigurationManager.AppSettings["UrlArchivo"];
            var cadenaLocal = System.Configuration.ConfigurationManager.AppSettings["UbicacionArchivo"];
            var ubicacionBinario = infoAnexo.Replace(cadenaWeb, cadenaLocal);
            byte[] bytes = ContenidoArchivo(ubicacionBinario);
            
            var salida = new Dalc.Anexo().CrearAnexo(idContrato, infoAnexo, bytes);

            System.IO.File.Delete(ubicacionBinario);

            return salida;
        }

        private static byte[] ContenidoArchivo(string archivo)
        {
            byte[] byteArray = null;

            using (FileStream fs = new FileStream(archivo, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                byteArray = new byte[fs.Length];
                int iBytesRead = fs.Read(byteArray, 0, (int)fs.Length);
            }
            return byteArray;
        }

        public static bool DescargarAnexo(int idContrato)
        {
            return new Dalc.Anexo().DescargarAnexo(idContrato);
        }

        public static List<Objeto.Anexo> ObtenerAnexos(int idContrato) {
            return new Dalc.Anexo().ObtenerAnexos(idContrato);
        }

        public static bool EliminarAnexo(int id) {
            return new Dalc.Anexo().EliminarAnexo(id);
        }

        public static bool CrearOtroSi(int idContrato, DateTime fecha, double monto, int tiempo) {
            return new Dalc.OtroSi().CrearOtroSi(idContrato, fecha, monto, tiempo);
        }

        public static List<Objeto.OtroSi> ObtenerOtroSi(int idContrato) {
            return new Dalc.OtroSi().ObtenerOtroSi(idContrato);
        }

        public static Objeto.Acumulado ObtenerOtroSiAcumulado(int idContrato)
        {
            return new Dalc.OtroSi().ObtenerOtroSiAcumulado(idContrato);
        }

        public static List<Objeto.TipoAlerta> ObtenerTipoAlerta() {
            return new Dalc.TipoAlerta().ObtenerTipoAlerta();
        }

        public static List<Objeto.AlertaMensaje> ObtenerAlertaMensaje(int idMaestro, int idMensaje) {
            return new Dalc.AlertaMensaje().ObtenerMensajes(idMaestro, idMensaje);
        }
        public static List<Objeto.AlertaMensaje> ObtenerAlertaMensajeFechas(DateTime fI, DateTime fF)
        {
            return new Dalc.AlertaMensaje().ObtenerMensajesFechas(fI, fF);
        }

        public static bool AdicionarAlertaMensaje(int idMaestro, int idContrato, int tipo, string de, string para){
            return new Dalc.AlertaMensaje().AdicionarMensajes(idMaestro, idContrato, tipo, de, para);
        }

        public static bool AdicionarAlertaMensajeTipo1(int idMaestro, string cliente, int tipo, string de, string para)
        {
            return new Dalc.AlertaMensaje().AdicionarMensajesTipo1(idMaestro, cliente, tipo, de, para);
        }

        public static bool AdicionarAlertaMensajeTipo2(int idMaestro, string cliente, int tipo, string de, string para)
        {
            return new Dalc.AlertaMensaje().AdicionarMensajesTipo1(idMaestro, cliente, tipo, de, para);
        }

        public static List<Objeto.AlertaMensaje> ObtenerAlertaMensajeFecha(DateTime fechaI, DateTime fechaF){
            return new Dalc.AlertaMensaje().ObtenerMensajesFechas(fechaI, fechaF);
        }

        public static bool CerrarContratos(int contrato, string producto) {
            return new Dalc.Contrato().CerrarContrato(contrato, producto);
        }

        public static bool InactivarContratos(int contrato, string producto)
        {
            return new Dalc.Contrato().CerrarContrato(contrato, producto, true);
        }


        public static int Permiso(string usuario) {
            //Utilidades.Lecturas.EscribirLog("Usuario: " + usuario);
            int sa = 0;
            var query = new Dalc.Rol().ObteneRoles();
            usuario = usuario.Replace("LINDE", "");
            usuario = usuario.Replace("\\", "");

            //var salida = query.Where(x => x.Usuario.ToUpper().Replace(@"LINDE\", "").Contains(usuario.ToUpper().Replace(@"\", ""))).ToList();
            var salida = query.Where(x => x.Usuario.ToUpper().Replace(@"LINDE\\", "").Contains(usuario.ToUpper().Replace(@"\", ""))).ToList();

            if (salida == null)
                return 0;
            else {
                if (salida.Count == 0)
                    return sa;
                else
                    return salida.First().Permiso;
            }
        }

        public static bool EliminarOtroSi(int id)
        {
            var salida = new Dalc.OtroSi().EliminarOtroSi(id);
            return salida;
        }

        public static string InformeContratos(DateTime fI, DateTime fF) {
            var info = new Dalc.Informe().ObteneInforme(fI, fF);
            var salida = new Utilidades.ArchivoExcel().ConstruirInforme(info);
            return salida;

        }

        public static void FixesFecha() {
            new Dalc.Fixes().AjustarFechaFinalizacion();
            return;
        }

        public static bool ServicioAlerta(DateTime fechaAlerta) {
            bool salida = false;

            return salida;
        }

        public static List<Objeto.Ruta> ListarRutas() {
            return new Dalc.Ruta().ListarRuta();
        }

        public static bool InsertarContratoRuta(Objeto.ContratoRuta objeto) {
            return new Dalc.ContratoRuta().InsertarContratoRuta(objeto);
        }

        public static List<Objeto.ContratoRuta> ListarContratoRutas() {
            return new Dalc.ContratoRuta().ListarContratoRuta();
        }

        public static List<Objeto.ContratoRuta> ListarContratoRutas(string contrato)
        {
            return new Dalc.ContratoRuta().ListarContratoRuta(contrato);
        }

        public static List<Objeto.ContratoRuta> ListarContratoRutas(string contrato, int ruta)
        {
            return new Dalc.ContratoRuta().ListarContratoRuta(contrato, ruta);
        }

        public static List<Objeto.ContratoRuta> ListarContratoRutas(DateTime fI, DateTime fF)
        {
            return new Dalc.ContratoRuta().ListarContratoRuta(fI, fF);
        }

        public static bool AvanzarTarea(int id, string mensaje, string contrato) {
            var salida = new Dalc.ContratoRuta().AvanzarTarea(contrato, id, mensaje);
            return salida;
        }



        #region Metodos de Servicios
        public static bool EjecutarAlertasTipo1(int compania)
        {
            var con = new Dalc.Contrato();
            List<Objeto.AlertaTipo1 > list = new List<Objeto.AlertaTipo1>();
            list = con.BarrerContratosTipo1();
            //Buscar vendedor contrato

            foreach (var l in list)
            {
                var para = CorreoVendedor(l.IdCliente.ToString(), compania);
                var de = System.Configuration.ConfigurationManager.AppSettings["De"].ToString();
                var s = AdicionarAlertaMensajeTipo1(l.IdMaestro, l.NombreCliente , 1, de, para);
            }
            return true;
        }

        public static bool EjecutarAlertasTipo2(int compania)
        {
            var con = new Dalc.Contrato();
            List<Objeto.AlertaTipo2> list = new List<Objeto.AlertaTipo2>();
            list = con.BarrerContratosTipo2();
            //Buscar vendedor contrato

            foreach (var l in list)
            {
                var para = CorreoVendedor(l.IdCliente.ToString(), compania);
                var de = System.Configuration.ConfigurationManager.AppSettings["De"].ToString();
                var s = AdicionarAlertaMensajeTipo1(l.IdContratoMaestro, "Cliente: " + l.IdCliente + "\nContrato:" + l.NumeroContrato + ".\nTipo cliente:" + l.TipoCliente + "\nMonto:" + double.Parse(l.Calculo.ToString()).ToString("N2") + ".\nVentas:" + double.Parse(l.Ventas.ToString()).ToString("N2") , 2, de, para);
            }
            return true;
        }


        #region Alertas

        public static bool EjecutarPay(int compania) {
            //ejectar por periodicidad
            return false;
        }
        #endregion


        #endregion
    }
}
