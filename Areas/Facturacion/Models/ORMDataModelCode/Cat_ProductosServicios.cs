using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace SistFacturacion.Areas.Facturacion.Models.sistemafactura
{

    public partial class Cat_ProductosServicios
    {
        public Cat_ProductosServicios(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
