using System.Web;
using System.Web.Mvc;

namespace ContratosLinde
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            App_Start.InitBasicas.Init();
            App_Start.InitProducto.Init();
        }
    }
}
