using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Webapplication.Data
{
    public partial class HangHoa
    {
        public int MaHh { get; set; }

        [Required]
        [StringLength(100)]
        public string TenHh { get; set; } = null!;

        public string? TenAlias { get; set; }

        [Required]
        public int MaLoai { get; set; }

        public string? MoTaDonVi { get; set; }

        [Required]
        public double? DonGia { get; set; }

        public string? Hinh { get; set; }

        [Required]
        public DateTime NgaySx { get; set; }

        public double GiamGia { get; set; }

        public int SoLanXem { get; set; }

        public string? MoTa { get; set; }

        [Required]
        public string MaNcc { get; set; } = null!;

        [NotMapped]
        public IFormFile? HinhFile { get; set; }

        public virtual ICollection<BanBe> BanBes { get; set; } = new List<BanBe>();
        public virtual ICollection<ChiTietHd> ChiTietHds { get; set; } = new List<ChiTietHd>();
        public virtual Loai MaLoaiNavigation { get; set; } = null!;
        public virtual NhaCungCap MaNccNavigation { get; set; } = null!;
        [NotMapped]
        public virtual ICollection<YeuThich> YeuThiches { get; set; } = new List<YeuThich>();
    }
}
