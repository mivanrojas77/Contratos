using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;


namespace ContratosLinde.Controllers
{
    public class HomeController : Controller
    {
        private static List<Linde.Contratos.LogicaV2.Objeto.Cliente> _listaClientes = new List<Linde.Contratos.LogicaV2.Objeto.Cliente>();
        public static List<Linde.Contratos.LogicaV2.Objeto.Cliente> ListaClientes { get => _listaClientes; set => _listaClientes = value; }


        public ActionResult Index()
        {
            return View();
        }

        [System.Web.Http.HttpPost]
        public ActionResult AutocompletarCliente(string termino, int? compania, string tipoCliente)
        {

            List<Linde.Contratos.LogicaV2.Objeto.Cliente> salida = new List<Linde.Contratos.LogicaV2.Objeto.Cliente>();
            if (termino != null)
            {
                if (termino.Length > 3)
                {
                    salida = (
                    from x in App_Start.InitBasicas.ListaGeneralCliente
                    where x.Descripcion.ToUpper().Contains(termino.ToUpper()) && x.Compania == compania && x.TipoCliente.PadLeft(5, '0').Equals(tipoCliente)
                    select x).ToList<Linde.Contratos.LogicaV2.Objeto.Cliente>();

                }
                else
                {
                    salida.Add(new Linde.Contratos.LogicaV2.Objeto.Cliente { Descripcion = "" });
                }
            }
            else
            {
                salida.Add(new Linde.Contratos.LogicaV2.Objeto.Cliente { Descripcion = "" });
            }
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Http.HttpPost]
        public ActionResult AutocompletarProducto(string termino)
        {

            List<Linde.Contratos.LogicaV2.Objeto.Producto> salida = new List<Linde.Contratos.LogicaV2.Objeto.Producto>();
            if (termino != null)
            {
                if (termino.Length > 3)
                {
                    salida = App_Start.InitProducto.ListaGeneralProducto.Where(x => x.Descripcion.ToUpper().Contains(termino)).ToList();

                }
                else
                {
                    salida.Add(new Linde.Contratos.LogicaV2.Objeto.Producto { Descripcion = "" });
                }
            }
            else
            {
                salida.Add(new Linde.Contratos.LogicaV2.Objeto.Producto { Descripcion = "" });
            }
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListarTipoCliente(int compania)
        {
            var salida = Linde.Contratos.LogicaV2.Servicio.ListarTipoCliente(compania);
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListarNitCliente(int compania, string idCliente)
        {
            var cCliente = Linde.Contratos.LogicaV2.Utilidades.Lecturas.ExtraeCodigoCadena(idCliente);
            var salida = Linde.Contratos.LogicaV2.Servicio.NitCliente(compania, cCliente);
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListarVendendor(int compania, string idCliente)
        {
            var cCliente = Linde.Contratos.LogicaV2.Utilidades.Lecturas.ExtraeCodigoCadena(idCliente);
            var salida = Linde.Contratos.LogicaV2.Servicio.Vendendor(compania, cCliente);
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CorreoVendendor(string idCliente, int compania)
        {
            var cCliente = Linde.Contratos.LogicaV2.Utilidades.Lecturas.ExtraeCodigoCadena(idCliente);
            var salida = Linde.Contratos.LogicaV2.Servicio.CorreoVendedor(cCliente, compania);
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }



        public ActionResult GuardarCOntratoMaestro(int compania, int tipoCliente, string idCliente, string identificacion, string correo)
        {
            Linde.Contratos.LogicaV2.Objeto.Maestro m = new Linde.Contratos.LogicaV2.Objeto.Maestro()
            {
                IdCompania = compania,
                TipoCliente = tipoCliente,
                Cliente = idCliente,
                Identificacion = identificacion,
                Correo = correo
            };

            var salida = Linde.Contratos.LogicaV2.Servicio.GuardarContratoMaestro(m);
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarContrato(int IdMaestro)
        {
            var salida = Linde.Contratos.LogicaV2.Servicio.GuardarContrato(IdMaestro);
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }

        private int AnalisisNumero(string valor)
        {
            int salida = 0;
            if (valor.Equals("") || valor == null)
            {
                salida = 0;
            }
            else
            {
                salida = int.Parse(valor);
            }
            return salida;
        }
        private String AnalisisTexto(string valor)
        {
            string salida = "";
            if (valor.Equals("") || valor == null)
            {
                salida = "";
            }
            else
            {
                salida = valor;
            }
            return salida;
        }
        private bool AnalisisBool(string valor)
        {
            bool salida = false;
            if (valor.Equals("") || valor == null)
            {
                salida = false;
            }
            else
            {
                salida = bool.Parse(valor);
            }
            return salida;
        }

        private DateTime AnalisisFecha(string valor)
        {
            DateTime salida = DateTime.MinValue;

            if (valor.Equals("") || valor == null)
            {
                salida = DateTime.MinValue;
            }
            else
            {
                salida = DateTime.Parse(valor);
            }
            return salida;
        }

        private string ObtenerCodigo(string s)
        {
            string salida = "";
            int i = -1;
            i = s.IndexOf('-');
            if (i > 0)
            {
                salida = s.Substring(0, i - 1).Trim();
            }
            else
            {
                salida = s;
            }
            return salida;
        }

        public ActionResult GuardarDetalle(int IdMaestro, string prod, string preProd, string conCont, string cTop, string top, string escala, string canEsca, string preEsca, string period, string alqTan, string arrEq, string canonArr, string garant, string firmadoP, string fechaIni, string fechaFir, string FechaFina, string fechaFinaPro, string inicioSumi, string vigenciaMe, string vigenciaAutoMes, string avisoTermCon, string condEspec, string tiempoEsperaTrac, string horasMinEsp, string contratoComo, string plazoPagoD, string multaVencFac, string porcMulta, string aumentoPrecio, string mail)
        {
            Linde.Contratos.LogicaV2.Objeto.Contrato con = new Linde.Contratos.LogicaV2.Objeto.Contrato();
            var p = ObtenerCodigo(prod);
            bool salida = false;
            try
            {
                con.Producto = AnalisisTexto(p);
                con.Precio_producto = AnalisisNumero(preProd);
                con.Consumo_contratado = AnalisisNumero(conCont);
                con.C_top = AnalisisBool(cTop);
                con.Top = AnalisisNumero(top);
                con.Escalado = AnalisisBool(escala);
                con.Cantidad_escalado = AnalisisTexto(canEsca);
                con.Precio_escalado = AnalisisTexto(preEsca);
                con.Periodicidad = AnalisisNumero(period);
                con.Alquiler_tanques = AnalisisBool(alqTan);
                con.Arriendo_equipo = AnalisisBool(arrEq);
                con.Canon_arrendamiento = AnalisisNumero(canonArr);
                con.Garantia = AnalisisTexto(garant);
                con.Firmado_por = AnalisisNumero(firmadoP);
                con.Fecha_inicio = AnalisisFecha(fechaIni);
                con.Fecha_firmado = AnalisisFecha(fechaFir);
                con.Fecha_finalizacion = AnalisisFecha(FechaFina);
                con.Fecha_finaliza_pro = AnalisisFecha(fechaFinaPro);
                con.Inicio_suministro = AnalisisFecha(inicioSumi);
                con.Vigencia_meses = AnalisisNumero(vigenciaMe);
                con.Vigencia_auto_meses = AnalisisNumero(vigenciaAutoMes);
                con.Aviso_terminacion_con = AnalisisNumero(avisoTermCon);
                con.Condiciones_especiales = AnalisisTexto(condEspec);
                con.Tiempo_espera_trac = AnalisisBool(tiempoEsperaTrac);
                con.Horas_min_espera = AnalisisNumero(horasMinEsp);
                con.Contrato_comodato = AnalisisBool(contratoComo);
                con.Plazo_pago_dias = AnalisisNumero(plazoPagoD);
                con.Multa_vencimiento_fac = AnalisisBool(multaVencFac);
                con.Porcentaje_multa = AnalisisTexto(porcMulta);
                con.Aumento_precio = AnalisisTexto(aumentoPrecio);
                con.Mail = AnalisisTexto(mail);

                salida = Linde.Contratos.LogicaV2.Servicio.GuardarDetalle(IdMaestro, con);
            }
            catch (Exception ex)
            {
                System.Console.Out.WriteLine(ex.ToString());
            }
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarDelta(int IdMaestro, string deltaee, string deltaip, string deltadiesel, string deltaipp, string deltagas, string deltatrm, string deltaotros, string disesel, string ipc, string usd, string energia, string otros)
        {
            Linde.Contratos.LogicaV2.Objeto.Contrato contrato = new Linde.Contratos.LogicaV2.Objeto.Contrato();
            bool salida = false;
            try
            {
                contrato.Deltaee = AnalisisNumero(deltaee);
                contrato.Deltaip = AnalisisNumero(deltaip);
                contrato.Deltadiesel = AnalisisNumero(deltadiesel);
                contrato.Deltaipp = AnalisisNumero(deltaipp);
                contrato.Deltagas = AnalisisNumero(deltagas);
                contrato.Deltatrm = AnalisisNumero(deltatrm);
                contrato.Deltaotros = AnalisisNumero(deltaotros);
                contrato.Diesel = AnalisisNumero(disesel);
                contrato.Ipc = AnalisisNumero(ipc);
                contrato.Usd = AnalisisNumero(usd);
                contrato.Energia = AnalisisNumero(energia);
                contrato.Otros = AnalisisTexto(otros);

                salida = Linde.Contratos.LogicaV2.Servicio.GuardarDelta(IdMaestro, contrato);
            }
            catch (Exception ex)
            {
                System.Console.Out.WriteLine(ex.ToString());
            }
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarPrecio(int IdMaestro, string valorAno1, string valorAno2, string valorAno3, string valorAno4, string valorAno5, string otroSi, string vigencia, string estatal, string asignacionPresupuestal, string noRubroPresupuestal, string certificadoDisponible, string actaFechaInicio, string numeroFacturaInicial, string totalAdiciones, string totalProrrogaMes, string id, string serial)
        {
            bool salida = false;
            Linde.Contratos.LogicaV2.Objeto.Contrato contrato = new Linde.Contratos.LogicaV2.Objeto.Contrato();
            try
            {
                contrato.Valor_ano1 = AnalisisNumero(valorAno1);
                contrato.Valor_ano2 = AnalisisNumero(valorAno2);
                contrato.Valor_ano3 = AnalisisNumero(valorAno3);
                contrato.Valor_ano4 = AnalisisNumero(valorAno4);
                contrato.Valor_ano5 = AnalisisNumero(valorAno5);
                contrato.Otrosi = AnalisisBool(otroSi);
                contrato.Vigencia = AnalisisBool(vigencia);
                contrato.Estatal = AnalisisBool(estatal);
                contrato.Asignacion_presupuestal = AnalisisNumero(asignacionPresupuestal);
                contrato.No_rubro_presupuestal = noRubroPresupuestal;
                contrato.Certificado_disponible = certificadoDisponible;
                contrato.Acta_fecha_inicio = AnalisisFecha(actaFechaInicio);
                contrato.Numero_factura_inicial = numeroFacturaInicial;
                contrato.Total_adiciones = AnalisisNumero(totalAdiciones);
                contrato.Total_prorroga_mes = AnalisisNumero(totalProrrogaMes);
                contrato.Id = id;
                contrato.Serial = serial;

                salida = Linde.Contratos.LogicaV2.Servicio.GuardarPrecio(IdMaestro, contrato);
            }
            catch (Exception ex)
            {
                System.Console.Out.WriteLine(ex.ToString());
                salida = false;
            }
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListarMaestros(int emp, string cliente)
        {
            var cli = ObtenerCodigo(cliente);
            var salida = Linde.Contratos.LogicaV2.Servicio.ListarMaestros(emp, cli);
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CerrarContratos(int contrato, string producto)
        {
            var salida = Linde.Contratos.LogicaV2.Servicio.CerrarContratos(contrato, producto);
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InactivarContratos(int contrato, string producto)
        {
            var salida = Linde.Contratos.LogicaV2.Servicio.InactivarContratos(contrato, producto);
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }



        public ActionResult ObtenerDetalle(int contratoProducto)
        {
            var salida = Linde.Contratos.LogicaV2.Servicio.ObtenerDetalle(contratoProducto);
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerDelta(int contratoProducto)
        {
            var salida = Linde.Contratos.LogicaV2.Servicio.ObtenerDelta(contratoProducto);
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerPrecio(int contratoProducto)
        {
            var salida = Linde.Contratos.LogicaV2.Servicio.ObtenerPrecio(contratoProducto);
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult UploadAnexo()
        {
            JsonResult jsonResult;
            string direc = "";
            try
            {
                foreach (string file in base.Request.Files)
                {
                    HttpPostedFileBase fileContent = base.Request.Files[file];
                    if (fileContent == null || fileContent.ContentLength <= 0)
                    {
                        continue;
                    }
                    Stream stream = fileContent.InputStream;
                    int index = fileContent.FileName.LastIndexOf("\\");
                    string nombre = fileContent.FileName.Substring(index + 1);
                    Path.GetFileName(file);
                    string path = string.Concat(ConfigurationManager.AppSettings["UbicacionArchivo"].ToString(), nombre);
                    direc += path + ";";
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    using (FileStream fileStream = System.IO.File.Create(path))
                    {
                        stream.CopyTo(fileStream);
                    }
                }
                return base.Json(direc.Substring(0, direc.Length - 1));
            }
            catch (Exception ex)
            {
                jsonResult = base.Json("Upload failed");
            }
            return jsonResult;
        }

        public ActionResult AlmacenarAnexo(int contrato, string archivo)
        {
            string arc = archivo;
            arc = System.Configuration.ConfigurationManager.AppSettings["UrlArchivo"] + arc;
            var salida = Linde.Contratos.LogicaV2.Servicio.CrearAnexo(contrato, arc);
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerDescarga(int id)
        {
            var salida = Linde.Contratos.LogicaV2.Servicio.DescargarAnexo(id);
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarDescarga(string archivo)
        {
            var ubicacionLocal = System.Configuration.ConfigurationManager.AppSettings["UbicacionArchivo"];
            var ubicacionWeb = System.Configuration.ConfigurationManager.AppSettings["UrlArchivo"];
            archivo = archivo.Replace(ubicacionWeb, ubicacionLocal);
            System.IO.File.Delete(archivo);
            return base.Json("ok", JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerAnexos(int contrato)
        {
            var salida = Linde.Contratos.LogicaV2.Servicio.ObtenerAnexos(contrato);
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarAnexo(int id)
        {
            var salida = Linde.Contratos.LogicaV2.Servicio.EliminarAnexo(id);
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearOtroSi(int idContrato, DateTime fecha, double monto, int tiempo)
        {
            var salida = Linde.Contratos.LogicaV2.Servicio.CrearOtroSi(idContrato, fecha, monto, tiempo);
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerOtroSis(int idContrato)
        {
            var salida = Linde.Contratos.LogicaV2.Servicio.ObtenerOtroSi(idContrato);
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerOtroSisAcu(int idContrato)
        {
            var salida = Linde.Contratos.LogicaV2.Servicio.ObtenerOtroSiAcumulado(idContrato);
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerAlertasPorContratoMaestro(int idMaestro, int idContrato)
        {
            var salida = Linde.Contratos.LogicaV2.Servicio.ObtenerAlertaMensaje(idMaestro, idContrato);
            foreach (var s in salida)
            {
                s.Mensaje = s.Mensaje.Replace(@"\n", "\n");
                int filas = s.Mensaje.Split('\n').Count();
                int columnas = 256;
                s.Mensaje = "<textarea rows='" + filas + "' cols='" + columnas + "' id='" + s.Id + "'>" + s.Mensaje + "</textarea>";
            }
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerAlertasPorFecha(DateTime fechaI, DateTime fechaF)
        {
            var salida = Linde.Contratos.LogicaV2.Servicio.ObtenerAlertaMensajeFecha(fechaI, fechaF);
            foreach (var s in salida)
            {
                s.Mensaje = s.Mensaje.Replace(@"\n", "\n");
                int filas = s.Mensaje.Split('\n').Count();
                int columnas = 256;
                s.Mensaje = "<textarea rows='" + filas + "' cols='" + columnas + "' id='" + s.Id + "'>" + s.Mensaje + "</textarea>";
            }
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AdicionarAlertasPorContratoMaestro(int idMaestro, int idContrato, int tipo)
        {
            var salida = "";
            //var salida = Linde.Contratos.LogicaV2.Servicio.AdicionarAlertaMensaje(idMaestro, idContrato, tipo); LEGION
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerRol(string dominio, string usuario)
        {
            //var nick = dominio + @"\" + usuario;
            string nick = usuario.Replace("\\", "");
            nick = nick.Replace("\f", "");
            nick = nick.Replace("\n", "");
            nick = nick.Replace("\t", "");
            nick = nick.Replace("\b", "");
            nick = nick.Replace("\r", "");
            var salida = Linde.Contratos.LogicaV2.Servicio.Permiso(nick);
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarOtroSi(int id)
        {
            var salida = Linde.Contratos.LogicaV2.Servicio.EliminarOtroSi(id);
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProcesarInforme(DateTime fechaI, DateTime fechaF)
        {
            var salida = Linde.Contratos.LogicaV2.Servicio.InformeContratos(fechaI, fechaF);
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerAutorizaciones(DateTime fechaI, DateTime fechaF)
        {
            var salida = Linde.Contratos.LogicaV2.Servicio.ListarContratoRutas(fechaI, fechaF);
            var infoRutas = Linde.Contratos.LogicaV2.Servicio.ListarRutas();
            foreach (var s in salida)
            {
                s.FechaRegistroTexto = s.FechaRegistro.ToShortDateString();
                var r = infoRutas.Where(x => x.Id == s.IdRuta).FirstOrDefault().AplicaComentario;
                if (r == 0)
                {
                    s.Accion = @"<select id='CboAutorizacion_" + s.Id + "-" + s.idContrato + "'><option value='S'>S</option><option value='N'>N</option></select>";
                }
                else
                {
                    s.Accion = @"<textarea  id='CboAutorizacion_" + s.Id + "-" + s.idContrato + "' value='" + s.Accion + @"' rows=4 cols=32 maxlegth=128 required></textarea>";
                }
            }
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AvanzarTarea(int id, string mensaje, string contrato)
        {
            bool salida = true;
            salida = Linde.Contratos.LogicaV2.Servicio.AvanzarTarea(id, mensaje, contrato);
            return base.Json(salida, JsonRequestBehavior.AllowGet);
        }
    }
}