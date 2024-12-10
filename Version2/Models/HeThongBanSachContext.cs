using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Version2.Models
{
    public partial class HeThongBanSachContext : DbContext
    {
        public HeThongBanSachContext()
        {
        }

        public HeThongBanSachContext(DbContextOptions<HeThongBanSachContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Chitietphieudathang> Chitietphieudathangs { get; set; } = null!;
        public virtual DbSet<Danhmuc> Danhmucs { get; set; } = null!;
        public virtual DbSet<Donhang> Donhangs { get; set; } = null!;
        public virtual DbSet<Hoadon> Hoadons { get; set; } = null!;
        public virtual DbSet<Khachhang> Khachhangs { get; set; } = null!;
        public virtual DbSet<Nguoigiaohang> Nguoigiaohangs { get; set; } = null!;
        public virtual DbSet<Nhaxuatban> Nhaxuatbans { get; set; } = null!;
        public virtual DbSet<Phieudathang> Phieudathangs { get; set; } = null!;
        public virtual DbSet<Sach> Saches { get; set; } = null!;
        public virtual DbSet<Tacgium> Tacgia { get; set; } = null!;
        public virtual DbSet<Taikhoan> Taikhoans { get; set; } = null!;
        public virtual DbSet<Taikhoannguoigiaohang> Taikhoannguoigiaohangs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-2UENL8B;Database=HeThongBanSach;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chitietphieudathang>(entity =>
            {
                entity.HasKey(e => new { e.IdphieuDatHang, e.Idsach })
                    .HasName("PK__CHITIETP__D2C90633424FF605");

                entity.ToTable("CHITIETPHIEUDATHANG");

                entity.Property(e => e.IdphieuDatHang).HasColumnName("IDPhieuDatHang");

                entity.Property(e => e.Idsach).HasColumnName("IDSach");

                entity.Property(e => e.Gia).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SoLuong).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.IdphieuDatHangNavigation)
                    .WithMany(p => p.Chitietphieudathangs)
                    .HasForeignKey(d => d.IdphieuDatHang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CHITIETPH__IDPhi__123EB7A3");

                entity.HasOne(d => d.IdsachNavigation)
                    .WithMany(p => p.Chitietphieudathangs)
                    .HasForeignKey(d => d.Idsach)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CHITIETPH__IDSac__1332DBDC");
            });

            modelBuilder.Entity<Danhmuc>(entity =>
            {
                entity.HasKey(e => e.IddanhMuc)
                    .HasName("PK__DANHMUC__DF6C0BD28A30CF33");

                entity.ToTable("DANHMUC");

                entity.HasIndex(e => e.TenDanhMuc, "UQ__DANHMUC__650CAE4EAABB2EF1")
                    .IsUnique();

                entity.Property(e => e.IddanhMuc).HasColumnName("IDDanhMuc");

                entity.Property(e => e.TenDanhMuc).HasMaxLength(100);
            });

            modelBuilder.Entity<Donhang>(entity =>
            {
                entity.HasKey(e => e.IddonHang)
                    .HasName("PK__DONHANG__9CA232F753561E8B");

                entity.ToTable("DONHANG");

                entity.Property(e => e.IddonHang).HasColumnName("IDDonHang");

                entity.Property(e => e.IdnguoiGiaoHang).HasColumnName("IDNguoiGiaoHang");

                entity.Property(e => e.IdphieuDatHang).HasColumnName("IDPhieuDatHang");

                entity.Property(e => e.NgayGiaoHang).HasColumnType("datetime");

                entity.Property(e => e.TrangThai)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'Đang Giao')");

                entity.HasOne(d => d.IdnguoiGiaoHangNavigation)
                    .WithMany(p => p.Donhangs)
                    .HasForeignKey(d => d.IdnguoiGiaoHang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DONHANG__IDNguoi__2180FB33");

                entity.HasOne(d => d.IdphieuDatHangNavigation)
                    .WithMany(p => p.Donhangs)
                    .HasForeignKey(d => d.IdphieuDatHang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DONHANG__IDPhieu__208CD6FA");
            });

            modelBuilder.Entity<Hoadon>(entity =>
            {
                entity.HasKey(e => e.IdhoaDon)
                    .HasName("PK__HOADON__5B896F4934F5E2CD");

                entity.ToTable("HOADON");

                entity.Property(e => e.IdhoaDon).HasColumnName("IDHoaDon");

                entity.Property(e => e.IdphieuDatHang).HasColumnName("IDPhieuDatHang");

                entity.Property(e => e.NgayLap).HasColumnType("date");

                entity.Property(e => e.TongTien).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TrangThai).HasMaxLength(50);

                entity.HasOne(d => d.IdphieuDatHangNavigation)
                    .WithMany(p => p.Hoadons)
                    .HasForeignKey(d => d.IdphieuDatHang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HOADON__IDPhieuD__0E6E26BF");
            });

            modelBuilder.Entity<Khachhang>(entity =>
            {
                entity.HasKey(e => e.IdkhachHang)
                    .HasName("PK__KHACHHAN__5A7167B5207C1378");

                entity.ToTable("KHACHHANG");

                entity.Property(e => e.IdkhachHang).HasColumnName("IDKhachHang");

                entity.Property(e => e.DiaChi).HasMaxLength(255);

                entity.Property(e => e.NgayDangKy)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NgaySinh).HasColumnType("date");

                entity.Property(e => e.SoDienThoai).HasMaxLength(15);

                entity.Property(e => e.TenKhachHang)
                    .HasMaxLength(100)
                    .HasColumnName("TenKHachHang");
            });

            modelBuilder.Entity<Nguoigiaohang>(entity =>
            {
                entity.HasKey(e => e.IdnguoiGiaoHang)
                    .HasName("PK__NGUOIGIA__EA256A8D171546EE");

                entity.ToTable("NGUOIGIAOHANG");

                entity.HasIndex(e => e.SoDienThoai, "UQ__NGUOIGIA__0389B7BD15349BDD")
                    .IsUnique();

                entity.Property(e => e.IdnguoiGiaoHang).HasColumnName("IDNguoiGiaoHang");

                entity.Property(e => e.DiaChi).HasMaxLength(255);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.NgayThamGia)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SoDienThoai).HasMaxLength(15);

                entity.Property(e => e.TenNguoiGiaoHang).HasMaxLength(100);
            });

            modelBuilder.Entity<Nhaxuatban>(entity =>
            {
                entity.HasKey(e => e.IdnhaXuatBan)
                    .HasName("PK__NHAXUATB__3ADA0354E2ED0193");

                entity.ToTable("NHAXUATBAN");

                entity.Property(e => e.IdnhaXuatBan).HasColumnName("IDNhaXuatBan");

                entity.Property(e => e.DiaChi).HasMaxLength(255);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.SoDienThoai).HasMaxLength(15);

                entity.Property(e => e.TenNhaXuatBan).HasMaxLength(100);

                entity.Property(e => e.Website).HasMaxLength(255);
            });

            modelBuilder.Entity<Phieudathang>(entity =>
            {
                entity.HasKey(e => e.IdphieuDatHang)
                    .HasName("PK__PHIEUDAT__7EA62945B68F7A38");

                entity.ToTable("PHIEUDATHANG");

                entity.Property(e => e.IdphieuDatHang).HasColumnName("IDPhieuDatHang");

                entity.Property(e => e.IdkhachHang).HasColumnName("IDKhachHang");

                entity.Property(e => e.NgayLapHoaDon)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TrangThaiThanhToan)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('ChuaThanhToan')");

                entity.HasOne(d => d.IdkhachHangNavigation)
                    .WithMany(p => p.Phieudathangs)
                    .HasForeignKey(d => d.IdkhachHang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PHIEUDATH__IDKha__0A9D95DB");
            });

            modelBuilder.Entity<Sach>(entity =>
            {
                entity.HasKey(e => e.Idsach)
                    .HasName("PK__SACH__C6F2F76B776C5C49");

                entity.ToTable("SACH");

                entity.HasIndex(e => e.Isbn, "UQ__SACH__447D36EAABC07D0B")
                    .IsUnique();

                entity.Property(e => e.Idsach).HasColumnName("IDSach");

                entity.Property(e => e.Gia).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.HinhAnh).HasMaxLength(255);

                entity.Property(e => e.IddanhMuc).HasColumnName("IDDanhMuc");

                entity.Property(e => e.IdnhaXuatBan).HasColumnName("IDNhaXuatBan");

                entity.Property(e => e.Isbn)
                    .HasMaxLength(20)
                    .HasColumnName("ISBN");

                entity.Property(e => e.SoLuongTon).HasDefaultValueSql("((0))");

                entity.Property(e => e.TenSach).HasMaxLength(200);

                entity.HasOne(d => d.IddanhMucNavigation)
                    .WithMany(p => p.Saches)
                    .HasForeignKey(d => d.IddanhMuc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SACH__IDDanhMuc__7A672E12");

                entity.HasOne(d => d.IdnhaXuatBanNavigation)
                    .WithMany(p => p.Saches)
                    .HasForeignKey(d => d.IdnhaXuatBan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SACH__IDNhaXuatB__797309D9");

                entity.HasMany(d => d.IdtacGia)
                    .WithMany(p => p.Idsaches)
                    .UsingEntity<Dictionary<string, object>>(
                        "Sachtacgium",
                        l => l.HasOne<Tacgium>().WithMany().HasForeignKey("IdtacGia").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__SACHTACGI__IDTac__7E37BEF6"),
                        r => r.HasOne<Sach>().WithMany().HasForeignKey("Idsach").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__SACHTACGI__IDSac__7D439ABD"),
                        j =>
                        {
                            j.HasKey("Idsach", "IdtacGia").HasName("PK__SACHTACG__A55A419477BB57CE");

                            j.ToTable("SACHTACGIA");

                            j.IndexerProperty<int>("Idsach").HasColumnName("IDSach");

                            j.IndexerProperty<int>("IdtacGia").HasColumnName("IDTacGia");
                        });
            });

            modelBuilder.Entity<Tacgium>(entity =>
            {
                entity.HasKey(e => e.IdtacGia)
                    .HasName("PK__TACGIA__3A8B6FF76A8C9E1C");

                entity.ToTable("TACGIA");

                entity.Property(e => e.IdtacGia).HasColumnName("IDTacGia");

                entity.Property(e => e.NgaySinh).HasColumnType("date");

                entity.Property(e => e.QuocTich).HasMaxLength(50);

                entity.Property(e => e.TenTacGia).HasMaxLength(100);
            });

            modelBuilder.Entity<Taikhoan>(entity =>
            {
                entity.HasKey(e => e.IdtaiKhoan)
                    .HasName("PK__TAIKHOAN__BC5F907C26965219");

                entity.ToTable("TAIKHOAN");

                entity.HasIndex(e => e.TenDangNhap, "UQ__TAIKHOAN__55F68FC0D096E8E8")
                    .IsUnique();

                entity.Property(e => e.IdtaiKhoan).HasColumnName("IDTaiKhoan");

                entity.Property(e => e.IdkhachHang).HasColumnName("IDKhachHang");

                entity.Property(e => e.MatKhau).HasMaxLength(255);

                entity.Property(e => e.NgayTaoTaiKhoan)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TenDangNhap).HasMaxLength(50);

                entity.HasOne(d => d.IdkhachHangNavigation)
                    .WithMany(p => p.Taikhoans)
                    .HasForeignKey(d => d.IdkhachHang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TAIKHOAN__IDKhac__05D8E0BE");
            });

            modelBuilder.Entity<Taikhoannguoigiaohang>(entity =>
            {
                entity.HasKey(e => e.IdtaiKhoan)
                    .HasName("PK__TAIKHOAN__BC5F907CAD9307E0");

                entity.ToTable("TAIKHOANNGUOIGIAOHANG");

                entity.HasIndex(e => e.TenDangNhap, "UQ__TAIKHOAN__55F68FC08F64C138")
                    .IsUnique();

                entity.Property(e => e.IdtaiKhoan).HasColumnName("IDTaiKhoan");

                entity.Property(e => e.IdnguoiGiaoHang).HasColumnName("IDNguoiGiaoHang");

                entity.Property(e => e.MatKhau).HasMaxLength(255);

                entity.Property(e => e.NgayTaoTaiKhoan)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TenDangNhap).HasMaxLength(50);

                entity.HasOne(d => d.IdnguoiGiaoHangNavigation)
                    .WithMany(p => p.Taikhoannguoigiaohangs)
                    .HasForeignKey(d => d.IdnguoiGiaoHang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TAIKHOANN__IDNgu__1BC821DD");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
