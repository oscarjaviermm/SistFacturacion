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

namespace SistFacturacion.Areas.Facturacion.Controllers
{
    // If you need to use Data Annotation attributes, attach them to this view model instead of an XPO data model.
    public class EmpresasViewModel {
        [Display(Name = "Id")]
        public int Pk_Empresa { get; set; }
        public string RFC { get; set; }
        public string Denominacion { get; set; }
        [Display(Name = "Certificado")]
        public string? Archivo_Cert { get; set; }
        [Display(Name ="Key")]
        public string? Archivo_Key { get; set; }
        public string? Llave { get; set; }
        public DateTime? Inicio { get; set; }
        public DateTime? Fin { get; set; }
    }
    public class EmpresasResumenViewModel {
        [Display(Name = "Id")]
        public int Pk_Empresa { get; set; }
        public string Denominacion { get; set; }
    }

    [Area("Facturacion")]
    [Route("Facturacion/api/[controller]/[action]")]
    public class EmpresasController : Controller
    {
        private UnitOfWork _uow;

        public EmpresasController(IConfiguration configuration) {
            this._uow = new UnitOfWork(ConnectionHelper.GetDataLayer(configuration, AutoCreateOption.SchemaAlreadyExists));
        }

        [HttpGet]
        public async Task<IActionResult> GetResume(DataSourceLoadOptions loadOptions) {
            var empresas = _uow.Query<Empresas>().Select(i => new EmpresasResumenViewModel {
                    Pk_Empresa = i.Pk_Empresa,
                    Denominacion = i.Denominacion
                }); 
            return Json(await DataSourceLoader.LoadAsync(empresas, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var empresas = _uow.Query<Empresas>().Select(i => new EmpresasViewModel {
                Pk_Empresa = i.Pk_Empresa,
                RFC = i.RFC,
                Denominacion = i.Denominacion,
                Archivo_Cert = i.Archivo_Cert,
                Archivo_Key = i.Archivo_Key,
                Llave = i.Llave,
                Inicio = i.Inicio,
                Fin = i.Fin
            });

            // If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.
            // In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.
            // Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "Pk_Empresa" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(empresas, loadOptions));
        }


        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Empresas(_uow);
            var viewModel = new EmpresasViewModel();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);

            PopulateViewModel(viewModel, valuesDict);

            if(!TryValidateModel(viewModel))
                return BadRequest(GetFullErrorMessage(ModelState));

            CopyToModel(viewModel, model);

            await _uow.CommitChangesAsync();

            return Json(model.Pk_Empresa);
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = _uow.GetObjectByKey<Empresas>(key, true);
            if(model == null)
                return StatusCode(409, "Object not found");

            var viewModel = new EmpresasViewModel {
                Pk_Empresa = model.Pk_Empresa,
                RFC = model.RFC,
                Denominacion = model.Denominacion,
                Archivo_Cert = model.Archivo_Cert,
                Archivo_Key = model.Archivo_Key,
                Llave = model.Llave,
                Inicio = model.Inicio,
                Fin = model.Fin
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
            var model = _uow.GetObjectByKey<Empresas>(key, true);

            _uow.Delete(model);
            await _uow.CommitChangesAsync();
        }


        const string PK_EMPRESA = nameof(Empresas.Pk_Empresa);
        const string RFC = nameof(Empresas.RFC);
        const string DENOMINACION = nameof(Empresas.Denominacion);
        const string ARCHIVO_CERT = nameof(Empresas.Archivo_Cert);
        const string ARCHIVO_KEY = nameof(Empresas.Archivo_Key);
        const string LLAVE = nameof(Empresas.Llave);
        const string INICIO = nameof(Empresas.Inicio);
        const string FIN = nameof(Empresas.Fin);

        private void PopulateViewModel(EmpresasViewModel viewModel, IDictionary values) {
            if(values.Contains(PK_EMPRESA)) {
                viewModel.Pk_Empresa = Convert.ToInt32(values[PK_EMPRESA]);
            }
            if(values.Contains(RFC)) {
                viewModel.RFC = Convert.ToString(values[RFC]);
            }
            if(values.Contains(DENOMINACION)) {
                viewModel.Denominacion = Convert.ToString(values[DENOMINACION]);
            }
            if(values.Contains(ARCHIVO_CERT)) {
                viewModel.Archivo_Cert = Convert.ToString(values[ARCHIVO_CERT]);
            }
            if(values.Contains(ARCHIVO_KEY)) {
                viewModel.Archivo_Key = Convert.ToString(values[ARCHIVO_KEY]);
            }
            if(values.Contains(LLAVE)) {
                viewModel.Llave = Convert.ToString(values[LLAVE]);
            }
            if(values.Contains(INICIO)) {
                viewModel.Inicio = Convert.ToDateTime(values[INICIO]);
            }
            if(values.Contains(FIN)) {
                viewModel.Fin = Convert.ToDateTime(values[FIN]);
            }
        }

        private void CopyToModel(EmpresasViewModel viewModel, Empresas model) {
            model.Pk_Empresa = viewModel.Pk_Empresa;
            model.RFC = viewModel.RFC;
            model.Denominacion = viewModel.Denominacion;
            model.Archivo_Cert = viewModel.Archivo_Cert;
            model.Archivo_Key = viewModel.Archivo_Key;
            model.Llave = viewModel.Llave;
            model.Inicio = viewModel.Inicio;
            model.Fin = viewModel.Fin;
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