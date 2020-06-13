using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using Microsoft.Extensions.Configuration;
using SistFacturacion.Areas.Facturacion.Models.sistemafactura;
using DevExpress.Data.Filtering;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using System.Data.SqlTypes;

namespace SistFacturacion.Areas.Facturacion.Controllers
{
    // If you need to use Data Annotation attributes, attach them to this view model instead of an XPO data model.
    public class CatProdServViewModel {
        [Display(Name = "Id")]            public int Pk_ProductoServicio { get; set; }
        [Display(Name = "Empresa")]       public int Pk_Empresa { get; set; }
        [Display(Name = "Clave Producto/Servicio (SAT)")]         public int Clave_ProdServ_SAT { get; set; }
        [Display(Name = "Clave Interna")] public string Clave_Ident { get; set; }
        [Display(Name = "Codigo de Barras")] public string CodigoBarras { get; set; }
        [Display(Name = "Clave Unidad")]   public int Clave_Unidad_SAT { get; set; }
        public string Unidad { get; set; }
        [Display(Name = "Producto/Servicio (SAT)" )] public string Descripcion_ProdServ_Base { get; set; }
        [Display(Name = "Precio Base")]    public decimal PrecioBase { get; set; }
        [Display(Name = "IVA Trasladado")] public string IVAT { get; set; }
        [Display(Name = "IEPS Trasladado")] public string IEPST { get; set; }
        [Display(Name = "IVA Retenido")]  public string IVAR { get; set; }
        [Display(Name = "ISR Retenido")]  public string ISRR { get; set; }
        [Display(Name = "Impuesto Estatal Retenido")] public string IEstatal { get; set; }
        [Display(Name = "Otra Retencion")] public string OtraRetencion { get; set; }
    }

    [Route("Facturacion/api/[controller]/[action]")]
    public class CatProdServController : Controller
    {
        private UnitOfWork _uow;

        public CatProdServController(IConfiguration configuration) {
            this._uow = new UnitOfWork(ConnectionHelper.GetDataLayer(configuration, AutoCreateOption.SchemaAlreadyExists));
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            if (loadOptions.Filter is null) { loadOptions.Filter = new Object[] { "Pk_Empresa", "=", 0 }; }

            var CatProdServ = _uow.Query<Cat_ProductosServicios>().Select(i => new CatProdServViewModel
            {
                Pk_ProductoServicio = i.Pk_ProductoServicio,
                Pk_Empresa = i.Pk_Empresa,
                Clave_ProdServ_SAT = i.Clave_ProdServ_SAT,
                Clave_Ident = i.Clave_Ident,
                CodigoBarras = i.CodigoBarras,
                Clave_Unidad_SAT = i.Clave_Unidad_SAT,
                Unidad = i.Unidad,
                Descripcion_ProdServ_Base = i.Descripcion_ProdServ_Base,
                PrecioBase = i.PrecioBase,
                IVAT = i.IVAT,
                IEPST = i.IEPST,
                IVAR = i.IVAR,
                ISRR = i.ISRR,
                IEstatal = i.IEstatal,
                OtraRetencion = i.OtraRetencion
            });
            
            // If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.
            // In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.
            // Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "Pk_Serie" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(CatProdServ, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Cat_ProductosServicios(_uow);
            var viewModel = new CatProdServViewModel();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);

            PopulateViewModel(viewModel, valuesDict);

            if(!TryValidateModel(viewModel))
                return BadRequest(GetFullErrorMessage(ModelState));

            CopyToModel(viewModel, model);

            await _uow.CommitChangesAsync();

            return Json(model.Pk_ProductoServicio);
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = _uow.GetObjectByKey<Cat_ProductosServicios>(key, true);
            if(model == null)
                return StatusCode(409, "Object not found");

            var viewModel = new CatProdServViewModel {
                Pk_ProductoServicio = model.Pk_ProductoServicio,
                Pk_Empresa = model.Pk_Empresa,
                Clave_ProdServ_SAT = model.Clave_ProdServ_SAT,
                Clave_Ident = model.Clave_Ident,
                CodigoBarras = model.CodigoBarras,
                Clave_Unidad_SAT = model.Clave_Unidad_SAT,
                Unidad = model.Unidad,
                Descripcion_ProdServ_Base = model.Descripcion_ProdServ_Base,
                PrecioBase = model.PrecioBase,
                IVAT = model.IVAT,
                IEPST = model.IEPST,
                IVAR = model.IVAR,
                ISRR = model.ISRR,
                IEstatal = model.IEstatal,
                OtraRetencion = model.OtraRetencion
            };

            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateViewModel(viewModel, valuesDict);

            if(!TryValidateModel(viewModel))
                return BadRequest(GetFullErrorMessage(ModelState));

            CopyToModel(viewModel, model);

            await _uow.CommitChangesAsync();

            return Ok();
        }

        [HttpDelete]
        public async Task Delete(int key) {
            var model = _uow.GetObjectByKey<Cat_ProductosServicios>(key, true);

            _uow.Delete(model);
            await _uow.CommitChangesAsync();
        }

        const string PK_PRODUCTOSERVICIO = nameof(Cat_ProductosServicios.Pk_ProductoServicio);
        const string PK_EMPRESA = nameof(Cat_ProductosServicios.Pk_Empresa);
        const string CLAVE_PRODSERV_SAT = nameof(Cat_ProductosServicios.Clave_ProdServ_SAT);
        const string CLAVE_IDENT = nameof(Cat_ProductosServicios.Clave_Ident);
        const string CODIGOBARRAS = nameof(Cat_ProductosServicios.CodigoBarras);
        const string CLAVE_UNIDAD_SAT = nameof(Cat_ProductosServicios.Clave_Unidad_SAT);
        const string UNIDAD = nameof(Cat_ProductosServicios.Unidad);
        const string DESCRIPCION_PRODSERV_BASE = nameof(Cat_ProductosServicios.Descripcion_ProdServ_Base);
        const string PRECIOBASE = nameof(Cat_ProductosServicios.PrecioBase);
        const string IVAT = nameof(Cat_ProductosServicios.IVAT);
        const string IEPST = nameof(Cat_ProductosServicios.IEPST);
        const string IVAR = nameof(Cat_ProductosServicios.IVAR);
        const string ISRR = nameof(Cat_ProductosServicios.ISRR);
        const string IESTATAL = nameof(Cat_ProductosServicios.IEstatal);
        const string OTRARETENCION = nameof(Cat_ProductosServicios.OtraRetencion);

        private void PopulateViewModel(CatProdServViewModel viewModel, IDictionary values) {
            if (values.Contains(PK_PRODUCTOSERVICIO)) {
                viewModel.Pk_ProductoServicio = Convert.ToInt32(values[PK_PRODUCTOSERVICIO]);
            }
            if (values.Contains(PK_EMPRESA)) {
                viewModel.Pk_Empresa = Convert.ToInt32(values[PK_EMPRESA]);
            }
            if (values.Contains(CLAVE_PRODSERV_SAT)) {
                viewModel.Clave_ProdServ_SAT = Convert.ToInt32(values[CLAVE_PRODSERV_SAT]);
            }
            if (values.Contains(CLAVE_IDENT)) {
                viewModel.Clave_Ident = Convert.ToString(values[CLAVE_IDENT]);
            }
            if (values.Contains(CODIGOBARRAS)) {
                viewModel.CodigoBarras = Convert.ToString(values[CODIGOBARRAS]);
            }
            if (values.Contains(CLAVE_UNIDAD_SAT)) {
                viewModel.Clave_Unidad_SAT = Convert.ToInt32(values[CLAVE_UNIDAD_SAT]);
            }
            if (values.Contains(UNIDAD)) {
                viewModel.Unidad = Convert.ToString(values[UNIDAD]);
            }
            if (values.Contains(DESCRIPCION_PRODSERV_BASE)) {
                viewModel.Descripcion_ProdServ_Base = Convert.ToString(values[DESCRIPCION_PRODSERV_BASE]);
            }
            if (values.Contains(PRECIOBASE)) {
                viewModel.PrecioBase = Convert.ToDecimal(values[PRECIOBASE]);
            }
            if (values.Contains(IVAT)) {
                viewModel.IVAT = Convert.ToString(values[IVAT]);
            }
            if (values.Contains(IEPST)) {
                viewModel.IEPST = Convert.ToString(values[IEPST]);
            }
            if (values.Contains(IVAR)) { 
                viewModel.IVAR = Convert.ToString(values[IVAR]); 
            }
            if (values.Contains(ISRR)) { 
                viewModel.ISRR = Convert.ToString(values[ISRR]); 
            }
            if (values.Contains(IESTATAL)) { 
                viewModel.IEstatal = Convert.ToString(values[IESTATAL]); 
            }
            if (values.Contains(OTRARETENCION)) { 
                viewModel.OtraRetencion = Convert.ToString(values[OTRARETENCION]); 
            }
        }

        private void CopyToModel(CatProdServViewModel viewModel, Cat_ProductosServicios model) {
            model.Pk_ProductoServicio = viewModel.Pk_ProductoServicio;
            model.Pk_Empresa = viewModel.Pk_Empresa;
            model.Clave_ProdServ_SAT = viewModel.Clave_ProdServ_SAT;
            model.Clave_Ident = viewModel.Clave_Ident;
            model.CodigoBarras = viewModel.CodigoBarras;
            model.Clave_Unidad_SAT = viewModel.Clave_Unidad_SAT;
            model.Unidad = viewModel.Unidad;
            model.Descripcion_ProdServ_Base = viewModel.Descripcion_ProdServ_Base;
            model.PrecioBase = viewModel.PrecioBase;
            model.IVAT = viewModel.IVAT;
            model.IEPST = viewModel.IEPST;
            model.IVAR = viewModel.IVAR;
            model.ISRR = viewModel.ISRR;
            model.IEstatal = viewModel.IEstatal;
            model.OtraRetencion = viewModel.OtraRetencion;
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState) {
            var messages = new List<string>();

            foreach(var entry in modelState) {
                foreach(var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }
    }
}