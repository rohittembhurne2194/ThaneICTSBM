//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SwachBharat.CMS.Dal.DataContexts
{
    using System;
    using System.Collections.Generic;
    
    public partial class SauchalayAddress
    {
        public int Id { get; set; }
        public string SauchalayID { get; set; }
        public string Address { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string QrImageUrl { get; set; }
        public string Mobile { get; set; }
        public string Tot { get; set; }
        public Nullable<int> Tns { get; set; }
        public string SauchalayQRCode { get; set; }
        public string ReferanceId { get; set; }
        public Nullable<System.DateTime> lastModifiedDate { get; set; }
    }
}
