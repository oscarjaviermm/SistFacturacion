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

    [Persistent(@"Tbl_Status")]
    public partial class Status : XPLiteObject
    {
        int fPk_Status;
        [Key]
        public int Pk_Status
        {
            get { return fPk_Status; }
            set { SetPropertyValue<int>(nameof(Pk_Status), ref fPk_Status, value); }
        }
        string fStatus1;
        [Size(SizeAttribute.Unlimited)]
        [Persistent(@"Status")]
        public string Status1
        {
            get { return fStatus1; }
            set { SetPropertyValue<string>(nameof(Status1), ref fStatus1, value); }
        }
    }

}
