using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linde.Contratos.LogicaV2.Objeto
{
    public class Contrato
    {
        public int Id_contrato { get; set; }
        public DateTime Fecha_registro { get; set; }
        public string Fecha_registro_Texto { get; set; }
        public int Id_cliente { get; set; }
        public string Producto { get; set; }
        public int Consumo_contratado { get; set; }
        public int Precio_producto { get; set; }
        public bool C_top	{get;set;}
        public int Top { get; set; }
        public bool Escalado	{get;set;}
        public string Cantidad_escalado { get; set; }
        public string Precio_escalado { get; set; }
        public int Periodicidad { get; set; }
        public bool Alquiler_tanques { get; set; }
        public bool Arriendo_equipo { get; set; }
        public int Canon_arrendamiento { get; set; }
        public DateTime Fecha_inicio { get; set; }
        public DateTime Fecha_firmado { get; set; }
        public DateTime Fecha_finalizacion { get; set; }
        public string Garantia { get; set; }
        public int Firmado_por { get; set; }
        public DateTime Fecha_finaliza_pro { get; set; }
        public int Aviso_terminacion_con { get; set; }
        public DateTime Inicio_suministro { get; set; }

        public String Inicio_suministro_texto { get; set;}
        public string Condiciones_especiales { get; set; }
        public int Vigencia_meses { get; set; }
        public int Vigencia_auto_meses { get; set; }
        public bool Tiempo_espera_trac { get; set; }
        public int Horas_min_espera { get; set; }
        public bool Contrato_comodato { get; set; }
        public int Plazo_pago_dias { get; set; }
        public bool Multa_vencimiento_fac { get; set; }
        public string Porcentaje_multa { get; set; }
        public string Aumento_precio { get; set; }
        public int Deltaee { get; set; }
        public int Deltaip { get; set; }
        public int Deltadiesel { get; set; }
        public int Deltaipp { get; set; }
        public int Deltagas { get; set; }
        public int Deltatrm { get; set; }
        public int Deltaotros { get; set; }
        public int Diesel { get; set; }
        public int Ipc { get; set; }
        public int Usd { get; set; }
        public int Energia { get; set; }
        public string Otros { get; set; }
        public int Valor_ano1 { get; set; }
        public int Valor_ano2 { get; set; }
        public int Valor_ano3 { get; set; }
        public int Valor_ano4 { get; set; }
        public int Valor_ano5 { get; set; }
        public bool Otrosi { get; set; }
        public bool Vigencia { get; set; }
        public bool Estatal { get; set; }
        public int Asignacion_presupuestal { get; set; }
        public string No_rubro_presupuestal { get; set; }
        public string Certificado_disponible { get; set; }
        public DateTime Acta_fecha_inicio { get; set; }
        public string Numero_factura_inicial { get; set; }
        public string Numero_factura_final { get; set; }
        public int Total_adiciones { get; set; }
        public int Total_prorroga_mes { get; set; }
        public string Id { get; set; }
        public string Serial { get; set; }
        public string Mail { get; set; }
        public int IdMaestro { get; set; }
        public int Cerrado { get; set; }


       //   Operacion de calculo de pay LEGION
       public double TotalVenta { get; set; }
    }
}
