using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Contratos.WinServices
{
    public partial class Servicio : ServiceBase
    {
        private Timer _timer;
        private bool _ejecucionTipo1;
        private const int COOX = 21;
        private const int COLC = 25;

        public Servicio()
        {
            InitializeComponent();
            InitVariables();
        }

        private void InitVariables() {
            _timer = new Timer();
            _timer.Interval = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Intervalo"]);
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
            _ejecucionTipo1 = false;
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            while (_ejecucionTipo1 == false) {
                _ejecucionTipo1 = true;
                var ejec1 = Linde.Contratos.LogicaV2.Servicio.EjecutarAlertasTipo1(COOX);
                var ejec2 = Linde.Contratos.LogicaV2.Servicio.EjecutarAlertasTipo1(COLC);
                _ejecucionTipo1 = false;
            }
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
            _timer.Stop();
            _timer.Dispose();
            _timer = null;
        }
    }
}
