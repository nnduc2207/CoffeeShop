//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CoffeeShop.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class NguyenLieu
    {
        public int MaSP { get; set; }
        public int MaNL { get; set; }
        public int SoLuong { get; set; }
    
        public virtual KhoNguyenLieu KhoNguyenLieu { get; set; }
        public virtual SanPham SanPham { get; set; }
    }
}
