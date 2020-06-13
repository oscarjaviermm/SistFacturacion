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

namespace SistFacturacion.Areas.Facturacion.Controllers
{
    // If you need to use Data Annotation attributes, attach them to this view model instead of an XPO data model.
    public class ClientesViewModel {
        [Display(Name = "Id")]
        public int Pk_Cliente { get; set; }
        public int Pk_Empresa { get; set; }     //duda, es necesario?
        public string RazonSocial { get; set; }
        public string Denominacion { get; set; }
        public string RFC { get; set; }
        public string Calle { get; set; }
        public string Interior { get; set; }
        public string Exterior { get; set; }
        public string Colonia { get; set; }
        public string CP { get; set; }
    }

    [Route("Facturacion/api/[controller]/[action]")]
    public class ClientesController : Controller
    {
        private UnitOfWork _uow;

        public ClientesController(IConfiguration configuration) {
            this._uow = new UnitOfWork(ConnectionHelper.GetDataLayer(configuration, AutoCreateOption.SchemaAlreadyExists));
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            if (loadOptions.Filter is null) { loadOptions.Filter = new Object[] { "Pk_Empresa", "=", 0 }; }

            var clientes = _uow.Query<Clientes>().Select(i => new ClientesViewModel
            {
                Pk_Cliente = i.Pk_Cliente,
                Pk_Empresa = i.Pk_Empresa, // probablemente se elimine
                RazonSocial = i.RazonSocial,
                Denominacion = i.Denominacion,
                RFC = i.RFC,
                Calle = i.Calle,
                Interior = i.Interior,
                Exterior = i.Exterior,
                Colonia = i.Colonia,
                CP = i.CP
            }) ;
            
            // If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.
            // In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.
            // Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "Pk_Cliente" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(clientes, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Clientes(_uow);
            var viewModel = new ClientesViewModel();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);

            PopulateViewModel(viewModel, valuesDict);

            if(!TryValidateModel(viewModel))
                return BadRequest(GetFullErrorMessage(ModelState));

            CopyToModel(viewModel, model);

            await _uow.CommitChangesAsync();

            return Json(model.Pk_Cliente);
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = _uow.GetObjectByKey<Clientes>(key, true);
            if(model == null)
                return StatusCode(409, "Object not found");

            var viewModel = new ClientesViewModel {
                Pk_Cliente = model.Pk_Cliente,
                Pk_Empresa = model.Pk_Empresa,
                RazonSocial = model.RazonSocial,
                Denominacion = model.Denominacion,
                RFC = model.RFC,
                Calle = model.Calle,
                Interior = model.Interior,
                Exterior = model.Exterior,
                Colonia = model.Colonia,
                CP = model.CP
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
            var model = _uow.GetObjectByKey<Clientes>(key, true);

            _uow.Delete(model);
            await _uow.CommitChangesAsync();
        }

        const string PK_CLIENTE = nameof(Clientes.Pk_Cliente);
        const string PK_EMPRESA = nameof(Clientes.Pk_Empresa);
        const string RAZONSOCIAL = nameof(Clientes.RazonSocial);
        const string DENOMINACION = nameof(Clientes.Denominacion);
        const string RFC = nameof(Clientes.RFC);
        const string CALLE = nameof(Clientes.Calle);
        const string INTERIOR = nameof(Clientes.Interior);
        const string EXTERIOR = nameof(Clientes.Exterior);
        const string COLONIA = nameof(Clientes.Colonia);
        const string CP = nameof(Clientes.CP);

        private void PopulateViewModel(ClientesViewModel viewModel, IDictionary values) {
            if(values.Contains(PK_CLIENTE)) {
                viewModel.Pk_Cliente = Convert.ToInt32(values[PK_CLIENTE]);
            }
            if(values.Contains(PK_EMPRESA)) {
                viewModel.Pk_Empresa = Convert.ToInt32(values[PK_EMPRESA]);
            }
            if(values.Contains(RAZONSOCIAL)) {
                viewModel.RazonSocial = Convert.ToString(values[RAZONSOCIAL]);
            }
            if (values.Contains(DENOMINACION)) {
                viewModel.Denominacion = Convert.ToString(values[DENOMINACION]);
            }
            if (values.Contains(RFC)) {
                viewModel.RFC = Convert.ToString(values[RFC]);
            }
            if (values.Contains(CALLE)) {
                viewModel.Calle = Convert.ToString(values[CALLE]);
            }
            if (values.Contains(INTERIOR)) {
                viewModel.Interior = Convert.ToString(values[INTERIOR]);
            }
            if (values.Contains(EXTERIOR)) { 
                viewModel.Exterior = Convert.ToString(values[EXTERIOR]); 
            }
            if (values.Contains(COLONIA)) {
                viewModel.Colonia = Convert.ToString(values[COLONIA]);
            }
            if (values.Contains(CP)) {
                viewModel.CP = Convert.ToString(values[CP]);
            }
        }

        private void CopyToModel(ClientesViewModel viewModel, Clientes model) {
            model.Pk_Cliente = viewModel.Pk_Cliente;
            model.Pk_Empresa = viewModel.Pk_Empresa;
            model.RazonSocial = viewModel.RazonSocial;
            model.Denominacion = viewModel.Denominacion;
            model.RFC = viewModel.RFC;
            model.Calle = viewModel.Calle;
            model.Interior = viewModel.Interior;
            model.Exterior = viewModel.Exterior;
            model.Colonia = viewModel.Colonia;
            model.CP = viewModel.CP;
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