using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SistFacturacion.Areas.Facturacion.Controllers
{
    [Area("Facturacion")]
    public class CatProdServPageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}