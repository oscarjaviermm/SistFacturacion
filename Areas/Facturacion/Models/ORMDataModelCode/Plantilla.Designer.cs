﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace SistFacturacion.Areas.Facturacion.Models.sistemafactura
{

    [Persistent(@"Tbl_Plantilla")]
    public partial class Plantilla : XPLiteObject
    {
        int fPk_Plantilla;
        [Key(true)]
        public int Pk_Plantilla
        {
            get { return fPk_Plantilla; }
            set { SetPropertyValue<int>(nameof(Pk_Plantilla), ref fPk_Plantilla, value); }
        }
        int fPk_Empresa;
        public int Pk_Empresa
        {
            get { return fPk_Empresa; }
            set { SetPropertyValue<int>(nameof(Pk_Empresa), ref fPk_Empresa, value); }
        }
        string fPlantilla1;
        [Size(SizeAttribute.Unlimited)]
        [Persistent(@"Plantilla")]
        public string Plantilla1
        {
            get { return fPlantilla1; }
            set { SetPropertyValue<string>(nameof(Plantilla1), ref fPlantilla1, value); }
        }
    }

}
