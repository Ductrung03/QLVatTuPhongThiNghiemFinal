using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cuba_Staterkit.Migrations
{
    /// <inheritdoc />
    public partial class AddViewModelsToDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DanhGiaCapDoViewModel",
                columns: table => new
                {
                    MaDanhGia = table.Column<int>(type: "int", nullable: false),
                    MaTTB = table.Column<int>(type: "int", nullable: false),
                    CapDo = table.Column<int>(type: "int", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NgayDanhGia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NguoiDanhGia = table.Column<int>(type: "int", nullable: false),
                    TenThietBi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenNguoiDanhGia = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "LichThucHanhViewModel",
                columns: table => new
                {
                    MaLich = table.Column<int>(type: "int", nullable: false),
                    ThoiGianBD = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianKT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: true),
                    TenNguoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Loai",
                columns: table => new
                {
                    MaLoai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLoai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loai", x => x.MaLoai);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDung",
                columns: table => new
                {
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDangNhap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDung", x => x.MaNguoiDung);
                });

            migrationBuilder.CreateTable(
                name: "PhongMay",
                columns: table => new
                {
                    MaPhongMay = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenPhongMay = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhongMay", x => x.MaPhongMay);
                });

            migrationBuilder.CreateTable(
                name: "SuaChuaViewModel",
                columns: table => new
                {
                    MaSuaChua = table.Column<int>(type: "int", nullable: false),
                    MaTTB = table.Column<int>(type: "int", nullable: false),
                    LoaiSuaChua = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ChiPhi = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TinhTrangMoi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NguoiThucHien = table.Column<int>(type: "int", nullable: false),
                    TenThietBi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenNguoiThucHien = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ThongKeSuDungTheoThangViewModel",
                columns: table => new
                {
                    Thang = table.Column<int>(type: "int", nullable: false),
                    SoLichThucHanh = table.Column<int>(type: "int", nullable: false),
                    TongGioSuDung = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ThongKeTheoPhongViewModel",
                columns: table => new
                {
                    MaPhongMay = table.Column<int>(type: "int", nullable: false),
                    TenPhongMay = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TongThietBi = table.Column<int>(type: "int", nullable: false),
                    ThietBiTot = table.Column<int>(type: "int", nullable: false),
                    ThietBiHong = table.Column<int>(type: "int", nullable: false),
                    ThietBiDangSua = table.Column<int>(type: "int", nullable: false),
                    GiaTriTrungBinh = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ThuongHieu",
                columns: table => new
                {
                    MaThuongHieu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenThuongHieu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThuongHieu", x => x.MaThuongHieu);
                });

            migrationBuilder.CreateTable(
                name: "TrangTBViewModel",
                columns: table => new
                {
                    MaTTB = table.Column<int>(type: "int", nullable: false),
                    MaPhongMay = table.Column<int>(type: "int", nullable: false),
                    MaLoai = table.Column<int>(type: "int", nullable: false),
                    GiaTien = table.Column<int>(type: "int", nullable: true),
                    TinhTrang = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NgayNhap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaThuongHieu = table.Column<int>(type: "int", nullable: false),
                    SoLanSua = table.Column<int>(type: "int", nullable: true),
                    TenPhongMay = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenLoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenThuongHieu = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "XuatNhapTonViewModel",
                columns: table => new
                {
                    MaPhieu = table.Column<int>(type: "int", nullable: false),
                    LoaiPhieu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaTTB = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NguoiTao = table.Column<int>(type: "int", nullable: false),
                    TenThietBi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenNguoiTao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "LichThucHanh",
                columns: table => new
                {
                    MaLich = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ThoiGianBD = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianKT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichThucHanh", x => x.MaLich);
                    table.ForeignKey(
                        name: "FK_LichThucHanh_NguoiDung_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrangTB",
                columns: table => new
                {
                    MaTTB = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaPhongMay = table.Column<int>(type: "int", nullable: true),
                    MaLoai = table.Column<int>(type: "int", nullable: true),
                    GiaTien = table.Column<int>(type: "int", nullable: true),
                    TinhTrang = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NgayNhap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoLanSua = table.Column<int>(type: "int", nullable: true),
                    MaThuongHieu = table.Column<int>(type: "int", nullable: true),
                    MaLich = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrangTB", x => x.MaTTB);
                    table.ForeignKey(
                        name: "FK_TrangTB_Loai_MaLoai",
                        column: x => x.MaLoai,
                        principalTable: "Loai",
                        principalColumn: "MaLoai",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrangTB_PhongMay_MaPhongMay",
                        column: x => x.MaPhongMay,
                        principalTable: "PhongMay",
                        principalColumn: "MaPhongMay",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrangTB_ThuongHieu_MaThuongHieu",
                        column: x => x.MaThuongHieu,
                        principalTable: "ThuongHieu",
                        principalColumn: "MaThuongHieu",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DanhGiaCapDo",
                columns: table => new
                {
                    MaDanhGia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaTTB = table.Column<int>(type: "int", nullable: false),
                    CapDo = table.Column<int>(type: "int", nullable: false),
                    NgayDanhGia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NguoiDanhGia = table.Column<int>(type: "int", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhGiaCapDo", x => x.MaDanhGia);
                    table.ForeignKey(
                        name: "FK_DanhGiaCapDo_NguoiDung_NguoiDanhGia",
                        column: x => x.NguoiDanhGia,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DanhGiaCapDo_TrangTB_MaTTB",
                        column: x => x.MaTTB,
                        principalTable: "TrangTB",
                        principalColumn: "MaTTB",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LichSuSuaChua",
                columns: table => new
                {
                    MaSuaChua = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaTTB = table.Column<int>(type: "int", nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LoaiSuaChua = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ChiPhi = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NguoiThucHien = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichSuSuaChua", x => x.MaSuaChua);
                    table.ForeignKey(
                        name: "FK_LichSuSuaChua_NguoiDung_NguoiThucHien",
                        column: x => x.NguoiThucHien,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LichSuSuaChua_TrangTB_MaTTB",
                        column: x => x.MaTTB,
                        principalTable: "TrangTB",
                        principalColumn: "MaTTB",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "XuatNhapTon",
                columns: table => new
                {
                    MaPhieu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoaiPhieu = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaTTB = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NguoiTao = table.Column<int>(type: "int", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XuatNhapTon", x => x.MaPhieu);
                    table.ForeignKey(
                        name: "FK_XuatNhapTon_NguoiDung_NguoiTao",
                        column: x => x.NguoiTao,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_XuatNhapTon_TrangTB_MaTTB",
                        column: x => x.MaTTB,
                        principalTable: "TrangTB",
                        principalColumn: "MaTTB",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DanhGiaCapDo_MaTTB",
                table: "DanhGiaCapDo",
                column: "MaTTB");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGiaCapDo_NguoiDanhGia",
                table: "DanhGiaCapDo",
                column: "NguoiDanhGia");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuSuaChua_MaTTB",
                table: "LichSuSuaChua",
                column: "MaTTB");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuSuaChua_NguoiThucHien",
                table: "LichSuSuaChua",
                column: "NguoiThucHien");

            migrationBuilder.CreateIndex(
                name: "IX_LichThucHanh_MaNguoiDung",
                table: "LichThucHanh",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_TrangTB_MaLoai",
                table: "TrangTB",
                column: "MaLoai");

            migrationBuilder.CreateIndex(
                name: "IX_TrangTB_MaPhongMay",
                table: "TrangTB",
                column: "MaPhongMay");

            migrationBuilder.CreateIndex(
                name: "IX_TrangTB_MaThuongHieu",
                table: "TrangTB",
                column: "MaThuongHieu");

            migrationBuilder.CreateIndex(
                name: "IX_XuatNhapTon_MaTTB",
                table: "XuatNhapTon",
                column: "MaTTB");

            migrationBuilder.CreateIndex(
                name: "IX_XuatNhapTon_NguoiTao",
                table: "XuatNhapTon",
                column: "NguoiTao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DanhGiaCapDo");

            migrationBuilder.DropTable(
                name: "DanhGiaCapDoViewModel");

            migrationBuilder.DropTable(
                name: "LichSuSuaChua");

            migrationBuilder.DropTable(
                name: "LichThucHanh");

            migrationBuilder.DropTable(
                name: "LichThucHanhViewModel");

            migrationBuilder.DropTable(
                name: "SuaChuaViewModel");

            migrationBuilder.DropTable(
                name: "ThongKeSuDungTheoThangViewModel");

            migrationBuilder.DropTable(
                name: "ThongKeTheoPhongViewModel");

            migrationBuilder.DropTable(
                name: "TrangTBViewModel");

            migrationBuilder.DropTable(
                name: "XuatNhapTon");

            migrationBuilder.DropTable(
                name: "XuatNhapTonViewModel");

            migrationBuilder.DropTable(
                name: "NguoiDung");

            migrationBuilder.DropTable(
                name: "TrangTB");

            migrationBuilder.DropTable(
                name: "Loai");

            migrationBuilder.DropTable(
                name: "PhongMay");

            migrationBuilder.DropTable(
                name: "ThuongHieu");
        }
    }
}
