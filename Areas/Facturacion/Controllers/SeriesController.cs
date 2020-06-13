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
    public class SeriesViewModel {
        [Display(Name = "Id")]
        public int Pk_Serie { get; set; }

        public int Pk_Empresa { get; set; }
        public string Serie { get; set; }
        public int Folio { get; set; }
    }

    [Route("Facturacion/api/[controller]/[action]")]
    public class SeriesController : Controller
    {
        private UnitOfWork _uow;

        public SeriesController(IConfiguration configuration) {
            this._uow = new UnitOfWork(ConnectionHelper.GetDataLayer(configuration, AutoCreateOption.SchemaAlreadyExists));
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            if (loadOptions.Filter is null) { loadOptions.Filter = new Object[] { "Pk_Empresa", "=", 0 }; }

            var series = _uow.Query<Series>().Select(i => new SeriesViewModel {
                Pk_Serie = i.Pk_Serie,
                Pk_Empresa = i.Pk_Empresa,
                Serie = i.Serie,
                Folio = i.Folio
            });
            
            // If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.
            // In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.
            // Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "Pk_Serie" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(series, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Series(_uow);
            var viewModel = new SeriesViewModel();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);

            PopulateViewModel(viewModel, valuesDict);

            if(!TryValidateModel(viewModel))
                return BadRequest(GetFullErrorMessage(ModelState));

            CopyToModel(viewModel, model);

            await _uow.CommitChangesAsync();

            return Json(model.Pk_Serie);
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = _uow.GetObjectByKey<Series>(key, true);
            if(model == null)
                return StatusCode(409, "Object not found");

            var viewModel = new SeriesViewModel {
                Pk_Serie = model.Pk_Serie,
                Pk_Empresa = model.Pk_Empresa,
                Serie = model.Serie,
                Folio = model.Folio
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
            var model = _uow.GetObjectByKey<Series>(key, true);

            _uow.Delete(model);
            await _uow.CommitChangesAsync();
        }


        const string PK_SERIE = nameof(Series.Pk_Serie);
        const string PK_EMPRESA = nameof(Series.Pk_Empresa);
        const string SERIE = nameof(Series.Serie);
        const string FOLIO = nameof(Series.Folio);

        private void PopulateViewModel(SeriesViewModel viewModel, IDictionary values) {
            if(values.Contains(PK_SERIE)) {
                viewModel.Pk_Serie = Convert.ToInt32(values[PK_SERIE]);
            }
            if(values.Contains(PK_EMPRESA)) {
                viewModel.Pk_Empresa = Convert.ToInt32(values[PK_EMPRESA]);
            }
            if(values.Contains(SERIE)) {
                viewModel.Serie = Convert.ToString(values[SERIE]);
            }
            if(values.Contains(FOLIO)) {
                viewModel.Folio = Convert.ToInt32(values[FOLIO]);
            }
        }

        private void CopyToModel(SeriesViewModel viewModel, Series model) {
            model.Pk_Serie = viewModel.Pk_Serie;
            model.Pk_Empresa = viewModel.Pk_Empresa;
            model.Serie = viewModel.Serie;
            model.Folio = viewModel.Folio;
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