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
    
    public partial class SanPham
    {
        public SanPham()
        {
            this.NguyenLieu = new HashSet<NguyenLieu>();
        }
    
        public int Ma { get; set; }
        public string Ten { get; set; }
        public int MaLoai { get; set; }
        public int Gia { get; set; }
        public byte[] Anh { get; set; }
    
        public virtual LoaiSanPham LoaiSanPham { get; set; }
        public virtual ICollection<NguyenLieu> NguyenLieu { get; set; }
    }
}
