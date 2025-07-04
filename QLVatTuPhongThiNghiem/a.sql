USE [QuanLyVatTuPhongThiNghiem]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DanhGiaCapDo]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DanhGiaCapDo](
	[MaDanhGia] [int] IDENTITY(1,1) NOT NULL,
	[MaTTB] [int] NOT NULL,
	[CapDo] [int] NOT NULL,
	[NgayDanhGia] [datetime] NOT NULL,
	[NguoiDanhGia] [int] NOT NULL,
	[GhiChu] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaDanhGia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LichSuDangNhap]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LichSuDangNhap](
	[MaLichSu] [int] NOT NULL,
	[MaNguoiDung] [int] NULL,
	[ThoiDiemDN] [datetime] NULL,
	[ThoiDiemDX] [datetime] NULL,
 CONSTRAINT [PK_LichSuDangNhap] PRIMARY KEY CLUSTERED 
(
	[MaLichSu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LichSuHoatDong]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LichSuHoatDong](
	[MaLichSu] [int] IDENTITY(1,1) NOT NULL,
	[MaNguoiDung] [int] NOT NULL,
	[HanhDong] [nvarchar](50) NOT NULL,
	[Module] [nvarchar](50) NOT NULL,
	[ChiTiet] [nvarchar](max) NULL,
	[ThoiGian] [datetime] NULL,
	[DiaChi_IP] [nvarchar](50) NULL,
	[UserAgent] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaLichSu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LichSuSuaChua]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LichSuSuaChua](
	[MaSuaChua] [int] IDENTITY(1,1) NOT NULL,
	[MaTTB] [int] NOT NULL,
	[NgayBatDau] [datetime] NOT NULL,
	[NgayKetThuc] [datetime] NULL,
	[LoaiSuaChua] [nvarchar](50) NOT NULL,
	[MoTa] [nvarchar](500) NULL,
	[ChiPhi] [decimal](18, 2) NULL,
	[TrangThai] [nvarchar](50) NOT NULL,
	[NguoiThucHien] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MaSuaChua] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LichThucHanh]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LichThucHanh](
	[MaLich] [int] IDENTITY(1,1) NOT NULL,
	[TrangThai] [nvarchar](50) NULL,
	[ThoiGianBD] [datetime] NOT NULL,
	[ThoiGianKT] [datetime] NOT NULL,
	[MaNguoiDung] [int] NULL,
 CONSTRAINT [PK__LichThuc__728A9AE93F273F3E] PRIMARY KEY CLUSTERED 
(
	[MaLich] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LichTruc]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LichTruc](
	[MaLich] [int] NOT NULL,
	[Ngay] [date] NULL,
	[MaNV] [int] NOT NULL,
	[MaPhongMay] [int] NOT NULL,
	[CaLam] [nvarchar](50) NULL,
	[GhiChu] [nvarchar](500) NULL,
	[TrangThai] [nvarchar](50) NULL,
 CONSTRAINT [PK_LichTruc] PRIMARY KEY CLUSTERED 
(
	[MaLich] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Loai]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Loai](
	[MaLoai] [int] NOT NULL,
	[TenLoai] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Loai] PRIMARY KEY CLUSTERED 
(
	[MaLoai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lop]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lop](
	[MaLop] [int] IDENTITY(1,1) NOT NULL,
	[TenLop] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaLop] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NguoiDung]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NguoiDung](
	[MaNguoiDung] [int] NOT NULL,
	[TenDangNhap] [varchar](50) NOT NULL,
	[MatKhau] [varchar](50) NOT NULL,
	[MatKhauHash] [nvarchar](255) NULL,
	[Salt] [nvarchar](255) NULL,
	[Email] [nvarchar](100) NULL,
	[HoTen] [nvarchar](100) NULL,
	[NgayTao] [datetime] NULL,
	[NgayCapNhat] [datetime] NULL,
	[TrangThaiTaiKhoan] [bit] NULL,
	[LanDangNhapCuoi] [datetime] NULL,
	[SoLanDangNhapSai] [int] NULL,
	[NgayKhoaTaiKhoan] [datetime] NULL,
	[TokenDoiMatKhau] [nvarchar](255) NULL,
	[NgayHetHanToken] [datetime] NULL,
 CONSTRAINT [PK__TaiKhoan__55F68FC11E977CAE] PRIMARY KEY CLUSTERED 
(
	[MaNguoiDung] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NguoiDungVaiTro]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NguoiDungVaiTro](
	[MaNguoiDung] [int] NOT NULL,
	[MaVaiTro] [int] NOT NULL,
	[NgayCapQuyen] [datetime] NULL,
	[NgayHetHan] [datetime] NULL,
	[TrangThai] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaNguoiDung] ASC,
	[MaVaiTro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NhanVien]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NhanVien](
	[MaNV] [int] NOT NULL,
	[HoTen] [nvarchar](100) NULL,
 CONSTRAINT [PK_NhanVien] PRIMARY KEY CLUSTERED 
(
	[MaNV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NhatKyThayDoi]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NhatKyThayDoi](
	[MaNhatKy] [int] NOT NULL,
	[MaLichSu] [int] NULL,
	[NoiDung] [nvarchar](max) NULL,
	[ThongTinCu] [nvarchar](max) NULL,
	[ThongTinMoi] [nvarchar](max) NULL,
 CONSTRAINT [PK_NhatKyThayDoi] PRIMARY KEY CLUSTERED 
(
	[MaNhatKy] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhongMay]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhongMay](
	[MaPhongMay] [int] IDENTITY(1,1) NOT NULL,
	[TenPhongMay] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaPhongMay] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QuyenHan]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuyenHan](
	[MaQuyen] [int] IDENTITY(1,1) NOT NULL,
	[TenQuyen] [nvarchar](50) NOT NULL,
	[MoTa] [nvarchar](200) NULL,
	[Module] [nvarchar](50) NOT NULL,
	[HanhDong] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MaQuyen] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ThongBao]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ThongBao](
	[MaThongBao] [int] IDENTITY(1,1) NOT NULL,
	[TieuDe] [nvarchar](200) NOT NULL,
	[NoiDung] [nvarchar](max) NOT NULL,
	[LoaiThongBao] [nvarchar](50) NOT NULL,
	[MaNguoiGui] [int] NULL,
	[MaNguoiNhan] [int] NULL,
	[NgayTao] [datetime] NULL,
	[DaDoc] [bit] NULL,
	[TrangThai] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaThongBao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ThuongHieu]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ThuongHieu](
	[MaThuongHieu] [int] IDENTITY(1,1) NOT NULL,
	[TenThuongHieu] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK__LoaiTTB__730A57596BCC665F] PRIMARY KEY CLUSTERED 
(
	[MaThuongHieu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TrangTB]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrangTB](
	[MaTTB] [int] IDENTITY(1,1) NOT NULL,
	[MaPhongMay] [int] NULL,
	[MaLoai] [int] NULL,
	[GiaTien] [int] NULL,
	[TinhTrang] [nvarchar](100) NULL,
	[NgayNhap] [date] NOT NULL,
	[SoLanSua] [int] NULL,
	[MaThuongHieu] [int] NULL,
	[MaLich] [int] NULL,
	[MaQR] [nvarchar](100) NULL,
	[SoSeri] [nvarchar](100) NULL,
	[XuatXu] [nvarchar](100) NULL,
	[NamSanXuat] [int] NULL,
	[BaoHanhDen] [date] NULL,
	[ViTri] [nvarchar](200) NULL,
	[GhiChu] [nvarchar](500) NULL,
 CONSTRAINT [PK__TrangTB__31480F197B30AE6C] PRIMARY KEY CLUSTERED 
(
	[MaTTB] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VaiTro]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VaiTro](
	[MaVaiTro] [int] IDENTITY(1,1) NOT NULL,
	[TenVaiTro] [nvarchar](50) NOT NULL,
	[MoTa] [nvarchar](200) NULL,
	[NgayTao] [datetime] NULL,
	[TrangThai] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaVaiTro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VaiTroQuyen]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VaiTroQuyen](
	[MaVaiTro] [int] NOT NULL,
	[MaQuyen] [int] NOT NULL,
	[NgayCapQuyen] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaVaiTro] ASC,
	[MaQuyen] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[XuatNhapTon]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[XuatNhapTon](
	[MaPhieu] [int] IDENTITY(1,1) NOT NULL,
	[LoaiPhieu] [varchar](10) NOT NULL,
	[MaTTB] [int] NOT NULL,
	[SoLuong] [int] NOT NULL,
	[NgayTao] [datetime] NOT NULL,
	[NguoiTao] [int] NOT NULL,
	[GhiChu] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaPhieu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DanhGiaCapDo] ADD  DEFAULT (getdate()) FOR [NgayDanhGia]
GO
ALTER TABLE [dbo].[LichSuHoatDong] ADD  DEFAULT (getdate()) FOR [ThoiGian]
GO
ALTER TABLE [dbo].[LichSuSuaChua] ADD  DEFAULT (N'Đang sửa chữa') FOR [TrangThai]
GO
ALTER TABLE [dbo].[LichTruc] ADD  DEFAULT (N'Đã lên lịch') FOR [TrangThai]
GO
ALTER TABLE [dbo].[NguoiDung] ADD  DEFAULT (getdate()) FOR [NgayTao]
GO
ALTER TABLE [dbo].[NguoiDung] ADD  DEFAULT (getdate()) FOR [NgayCapNhat]
GO
ALTER TABLE [dbo].[NguoiDung] ADD  DEFAULT ((1)) FOR [TrangThaiTaiKhoan]
GO
ALTER TABLE [dbo].[NguoiDung] ADD  DEFAULT ((0)) FOR [SoLanDangNhapSai]
GO
ALTER TABLE [dbo].[NguoiDungVaiTro] ADD  DEFAULT (getdate()) FOR [NgayCapQuyen]
GO
ALTER TABLE [dbo].[NguoiDungVaiTro] ADD  DEFAULT ((1)) FOR [TrangThai]
GO
ALTER TABLE [dbo].[ThongBao] ADD  DEFAULT (getdate()) FOR [NgayTao]
GO
ALTER TABLE [dbo].[ThongBao] ADD  DEFAULT ((0)) FOR [DaDoc]
GO
ALTER TABLE [dbo].[ThongBao] ADD  DEFAULT ((1)) FOR [TrangThai]
GO
ALTER TABLE [dbo].[VaiTro] ADD  DEFAULT (getdate()) FOR [NgayTao]
GO
ALTER TABLE [dbo].[VaiTro] ADD  DEFAULT ((1)) FOR [TrangThai]
GO
ALTER TABLE [dbo].[VaiTroQuyen] ADD  DEFAULT (getdate()) FOR [NgayCapQuyen]
GO
ALTER TABLE [dbo].[XuatNhapTon] ADD  DEFAULT (getdate()) FOR [NgayTao]
GO
ALTER TABLE [dbo].[DanhGiaCapDo]  WITH CHECK ADD FOREIGN KEY([MaTTB])
REFERENCES [dbo].[TrangTB] ([MaTTB])
GO
ALTER TABLE [dbo].[DanhGiaCapDo]  WITH CHECK ADD FOREIGN KEY([NguoiDanhGia])
REFERENCES [dbo].[NguoiDung] ([MaNguoiDung])
GO
ALTER TABLE [dbo].[LichSuDangNhap]  WITH CHECK ADD  CONSTRAINT [FK_LichSuDangNhap_NguoiDung] FOREIGN KEY([MaNguoiDung])
REFERENCES [dbo].[NguoiDung] ([MaNguoiDung])
GO
ALTER TABLE [dbo].[LichSuDangNhap] CHECK CONSTRAINT [FK_LichSuDangNhap_NguoiDung]
GO
ALTER TABLE [dbo].[LichSuHoatDong]  WITH CHECK ADD FOREIGN KEY([MaNguoiDung])
REFERENCES [dbo].[NguoiDung] ([MaNguoiDung])
GO
ALTER TABLE [dbo].[LichSuSuaChua]  WITH CHECK ADD FOREIGN KEY([MaTTB])
REFERENCES [dbo].[TrangTB] ([MaTTB])
GO
ALTER TABLE [dbo].[LichSuSuaChua]  WITH CHECK ADD FOREIGN KEY([NguoiThucHien])
REFERENCES [dbo].[NguoiDung] ([MaNguoiDung])
GO
ALTER TABLE [dbo].[LichTruc]  WITH CHECK ADD  CONSTRAINT [FK_LichTruc_NhanVien] FOREIGN KEY([MaNV])
REFERENCES [dbo].[NhanVien] ([MaNV])
GO
ALTER TABLE [dbo].[LichTruc] CHECK CONSTRAINT [FK_LichTruc_NhanVien]
GO
ALTER TABLE [dbo].[LichTruc]  WITH CHECK ADD  CONSTRAINT [FK_LichTruc_PhongMay] FOREIGN KEY([MaPhongMay])
REFERENCES [dbo].[PhongMay] ([MaPhongMay])
GO
ALTER TABLE [dbo].[LichTruc] CHECK CONSTRAINT [FK_LichTruc_PhongMay]
GO
ALTER TABLE [dbo].[NguoiDungVaiTro]  WITH CHECK ADD FOREIGN KEY([MaNguoiDung])
REFERENCES [dbo].[NguoiDung] ([MaNguoiDung])
GO
ALTER TABLE [dbo].[NguoiDungVaiTro]  WITH CHECK ADD FOREIGN KEY([MaVaiTro])
REFERENCES [dbo].[VaiTro] ([MaVaiTro])
GO
ALTER TABLE [dbo].[NhatKyThayDoi]  WITH CHECK ADD  CONSTRAINT [FK_NhatKyThayDoi_LichSuDangNhap] FOREIGN KEY([MaLichSu])
REFERENCES [dbo].[LichSuDangNhap] ([MaLichSu])
GO
ALTER TABLE [dbo].[NhatKyThayDoi] CHECK CONSTRAINT [FK_NhatKyThayDoi_LichSuDangNhap]
GO
ALTER TABLE [dbo].[ThongBao]  WITH CHECK ADD FOREIGN KEY([MaNguoiGui])
REFERENCES [dbo].[NguoiDung] ([MaNguoiDung])
GO
ALTER TABLE [dbo].[ThongBao]  WITH CHECK ADD FOREIGN KEY([MaNguoiNhan])
REFERENCES [dbo].[NguoiDung] ([MaNguoiDung])
GO
ALTER TABLE [dbo].[TrangTB]  WITH CHECK ADD  CONSTRAINT [FK__TrangTB__MaPhong__2FCF1A8A] FOREIGN KEY([MaPhongMay])
REFERENCES [dbo].[PhongMay] ([MaPhongMay])
GO
ALTER TABLE [dbo].[TrangTB] CHECK CONSTRAINT [FK__TrangTB__MaPhong__2FCF1A8A]
GO
ALTER TABLE [dbo].[TrangTB]  WITH CHECK ADD  CONSTRAINT [FK__TrangTB__MaPhong__6E01572D] FOREIGN KEY([MaPhongMay])
REFERENCES [dbo].[PhongMay] ([MaPhongMay])
GO
ALTER TABLE [dbo].[TrangTB] CHECK CONSTRAINT [FK__TrangTB__MaPhong__6E01572D]
GO
ALTER TABLE [dbo].[TrangTB]  WITH CHECK ADD  CONSTRAINT [FK_TrangTB_LichThucHanh] FOREIGN KEY([MaLich])
REFERENCES [dbo].[LichThucHanh] ([MaLich])
GO
ALTER TABLE [dbo].[TrangTB] CHECK CONSTRAINT [FK_TrangTB_LichThucHanh]
GO
ALTER TABLE [dbo].[TrangTB]  WITH CHECK ADD  CONSTRAINT [FK_TrangTB_Loai] FOREIGN KEY([MaLoai])
REFERENCES [dbo].[Loai] ([MaLoai])
GO
ALTER TABLE [dbo].[TrangTB] CHECK CONSTRAINT [FK_TrangTB_Loai]
GO
ALTER TABLE [dbo].[TrangTB]  WITH CHECK ADD  CONSTRAINT [FK_TrangTB_ThuongHieu] FOREIGN KEY([MaThuongHieu])
REFERENCES [dbo].[ThuongHieu] ([MaThuongHieu])
GO
ALTER TABLE [dbo].[TrangTB] CHECK CONSTRAINT [FK_TrangTB_ThuongHieu]
GO
ALTER TABLE [dbo].[VaiTroQuyen]  WITH CHECK ADD FOREIGN KEY([MaQuyen])
REFERENCES [dbo].[QuyenHan] ([MaQuyen])
GO
ALTER TABLE [dbo].[VaiTroQuyen]  WITH CHECK ADD FOREIGN KEY([MaVaiTro])
REFERENCES [dbo].[VaiTro] ([MaVaiTro])
GO
ALTER TABLE [dbo].[XuatNhapTon]  WITH CHECK ADD FOREIGN KEY([MaTTB])
REFERENCES [dbo].[TrangTB] ([MaTTB])
GO
ALTER TABLE [dbo].[XuatNhapTon]  WITH CHECK ADD FOREIGN KEY([NguoiTao])
REFERENCES [dbo].[NguoiDung] ([MaNguoiDung])
GO
ALTER TABLE [dbo].[DanhGiaCapDo]  WITH CHECK ADD CHECK  (([CapDo]>=(1) AND [CapDo]<=(5)))
GO
ALTER TABLE [dbo].[XuatNhapTon]  WITH CHECK ADD CHECK  (([LoaiPhieu]='XUAT' OR [LoaiPhieu]='NHAP'))
GO
/****** Object:  StoredProcedure [dbo].[SP_BaoCaoChiPhiSuaChua]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Báo cáo chi phí sửa chữa
CREATE   PROCEDURE [dbo].[SP_BaoCaoChiPhiSuaChua]
    @TuNgay DATE,
    @DenNgay DATE
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT l.TenLoai, th.TenThuongHieu, 
           COUNT(s.MaSuaChua) as SoLanSuaChua,
           SUM(ISNULL(s.ChiPhi, 0)) as TongChiPhi,
           AVG(ISNULL(s.ChiPhi, 0)) as ChiPhiTrungBinh
    FROM LichSuSuaChua s WITH (NOLOCK)
    INNER JOIN TrangTB t WITH (NOLOCK) ON s.MaTTB = t.MaTTB
    INNER JOIN Loai l WITH (NOLOCK) ON t.MaLoai = l.MaLoai
    INNER JOIN ThuongHieu th WITH (NOLOCK) ON t.MaThuongHieu = th.MaThuongHieu
    WHERE s.NgayBatDau BETWEEN @TuNgay AND @DenNgay
    GROUP BY l.TenLoai, th.TenThuongHieu
    ORDER BY TongChiPhi DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_BaoCaoTonKho]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Báo cáo tồn kho
CREATE   PROCEDURE [dbo].[SP_BaoCaoTonKho]
    @MaPhongMay INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT t.MaTTB, t.MaPhongMay, p.TenPhongMay, 
           l.TenLoai, th.TenThuongHieu,
           ISNULL(SUM(CASE WHEN x.LoaiPhieu = 'NHAP' THEN x.SoLuong ELSE 0 END), 0) as TongNhap,
           ISNULL(SUM(CASE WHEN x.LoaiPhieu = 'XUAT' THEN x.SoLuong ELSE 0 END), 0) as TongXuat,
           ISNULL(SUM(CASE WHEN x.LoaiPhieu = 'NHAP' THEN x.SoLuong ELSE -x.SoLuong END), 0) as TonKho,
           t.TinhTrang
    FROM TrangTB t WITH (NOLOCK)
    INNER JOIN PhongMay p WITH (NOLOCK) ON t.MaPhongMay = p.MaPhongMay
    INNER JOIN Loai l WITH (NOLOCK) ON t.MaLoai = l.MaLoai
    INNER JOIN ThuongHieu th WITH (NOLOCK) ON t.MaThuongHieu = th.MaThuongHieu
    LEFT JOIN XuatNhapTon x WITH (NOLOCK) ON t.MaTTB = x.MaTTB
    WHERE (@MaPhongMay IS NULL OR t.MaPhongMay = @MaPhongMay)
    GROUP BY t.MaTTB, t.MaPhongMay, p.TenPhongMay, l.TenLoai, th.TenThuongHieu, t.TinhTrang
    ORDER BY p.TenPhongMay, l.TenLoai;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_CapNhatTrangThaiLich]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Cập nhật trạng thái lịch thực hành
CREATE   PROCEDURE [dbo].[SP_CapNhatTrangThaiLich]
    @MaLich INT,
    @TrangThai NVARCHAR(50),
    @KetQua INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION
    
    BEGIN TRY
        UPDATE LichThucHanh WITH (XLOCK)
        SET TrangThai = @TrangThai
        WHERE MaLich = @MaLich;
        
        IF @@ROWCOUNT > 0
            SET @KetQua = 1; -- Thành công
        ELSE
            SET @KetQua = 0; -- Không tìm thấy
            
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        SET @KetQua = -1; -- Lỗi
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DangKyLichThucHanh]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- MODULE 7: QUẢN LÝ LỊCH
-- =============================================

-- Đăng ký lịch thực hành
CREATE   PROCEDURE [dbo].[SP_DangKyLichThucHanh]
    @ThoiGianBD DATETIME,
    @ThoiGianKT DATETIME,
    @MaNguoiDung INT,
    @MaLich INT OUTPUT,
    @KetQua INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION
    
    BEGIN TRY
        -- Validate thời gian
        IF @ThoiGianBD >= @ThoiGianKT OR @ThoiGianBD <= GETDATE()
        BEGIN
            SET @KetQua = 0; -- Thời gian không hợp lệ
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra trùng lịch
        IF EXISTS (SELECT 1 FROM LichThucHanh WITH (UPDLOCK)
                   WHERE MaNguoiDung = @MaNguoiDung
                   AND TrangThai != N'Đã hủy'
                   AND ((@ThoiGianBD BETWEEN ThoiGianBD AND ThoiGianKT)
                   OR (@ThoiGianKT BETWEEN ThoiGianBD AND ThoiGianKT)
                   OR (ThoiGianBD BETWEEN @ThoiGianBD AND @ThoiGianKT)))
        BEGIN
            SET @KetQua = 0; -- Trùng lịch
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        INSERT INTO LichThucHanh (TrangThai, ThoiGianBD, ThoiGianKT, MaNguoiDung)
        VALUES (N'Đã đăng ký', @ThoiGianBD, @ThoiGianKT, @MaNguoiDung);
        
        SET @MaLich = SCOPE_IDENTITY();
        SET @KetQua = 1; -- Thành công
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        SET @KetQua = -1; -- Lỗi
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DangNhap]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_DangNhap]
    @TenDangNhap VARCHAR(50),
    @MatKhau VARCHAR(50),
    @KetQua INT OUTPUT,
    @MaNguoiDung INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION
    
    BEGIN TRY
        -- Validate input
        IF @TenDangNhap IS NULL OR @MatKhau IS NULL OR LEN(@TenDangNhap) = 0 OR LEN(@MatKhau) = 0
        BEGIN
            SET @KetQua = 0; -- Thông tin không hợp lệ
            SET @MaNguoiDung = 0;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Check user với lock
        SELECT @MaNguoiDung = MaNguoiDung 
        FROM NguoiDung WITH (UPDLOCK, HOLDLOCK)
        WHERE TenDangNhap = @TenDangNhap AND MatKhau = @MatKhau;
        
        IF @MaNguoiDung IS NOT NULL
        BEGIN
            -- Tạo lịch sử đăng nhập với ID tự động
            DECLARE @MaLichSu INT;
            
            -- Lấy ID tiếp theo cho LichSuDangNhap
            SELECT @MaLichSu = ISNULL(MAX(MaLichSu), 0) + 1 FROM LichSuDangNhap;
            
            INSERT INTO LichSuDangNhap (MaLichSu, MaNguoiDung, ThoiDiemDN)
            VALUES (@MaLichSu, @MaNguoiDung, GETDATE());
            
            SET @KetQua = 1; -- Đăng nhập thành công
        END
        ELSE
        BEGIN
            SET @KetQua = 0; -- Sai thông tin đăng nhập
            SET @MaNguoiDung = 0;
        END
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @KetQua = -1; -- Lỗi hệ thống
        SET @MaNguoiDung = 0;
        
        -- Log error details
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DangNhap_BaoMat]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- 10. TẠO STORED PROCEDURES BẢO MẬT

-- Procedure đăng nhập với bảo mật
CREATE   PROCEDURE [dbo].[SP_DangNhap_BaoMat]
    @TenDangNhap NVARCHAR(50),
    @MatKhau NVARCHAR(50),
    @DiaChi_IP NVARCHAR(50) = NULL,
    @UserAgent NVARCHAR(500) = NULL,
    @KetQua INT OUTPUT,
    @MaNguoiDung INT OUTPUT,
    @ThongBao NVARCHAR(255) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION
    
    BEGIN TRY
        DECLARE @MatKhauHash NVARCHAR(255), @Salt NVARCHAR(255), @TrangThaiTaiKhoan BIT, @SoLanSai INT, @NgayKhoa DATETIME;
        
        -- Kiểm tra tài khoản
        SELECT @MaNguoiDung = MaNguoiDung, 
               @MatKhauHash = MatKhauHash, 
               @Salt = Salt,
               @TrangThaiTaiKhoan = TrangThaiTaiKhoan,
               @SoLanSai = SoLanDangNhapSai,
               @NgayKhoa = NgayKhoaTaiKhoan
        FROM NguoiDung 
        WHERE TenDangNhap = @TenDangNhap;
        
        -- Kiểm tra tài khoản tồn tại
        IF @MaNguoiDung IS NULL
        BEGIN
            SET @KetQua = 0;
            SET @ThongBao = N'Tài khoản không tồn tại';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra tài khoản có bị khóa
        IF @TrangThaiTaiKhoan = 0 OR (@NgayKhoa IS NOT NULL AND @NgayKhoa > GETDATE())
        BEGIN
            SET @KetQua = -2;
            SET @ThongBao = N'Tài khoản đã bị khóa';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra số lần đăng nhập sai (khóa tạm thời nếu > 5 lần)
        IF @SoLanSai >= 5
        BEGIN
            UPDATE NguoiDung 
            SET NgayKhoaTaiKhoan = DATEADD(MINUTE, 30, GETDATE())
            WHERE MaNguoiDung = @MaNguoiDung;
            
            SET @KetQua = -3;
            SET @ThongBao = N'Tài khoản bị khóa 30 phút do đăng nhập sai quá nhiều lần';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra mật khẩu (nếu đã hash)
        IF @MatKhauHash IS NOT NULL
        BEGIN
            DECLARE @MatKhauCheck NVARCHAR(255) = HASHBYTES('SHA2_256', @MatKhau + @Salt);
            IF @MatKhauCheck != @MatKhauHash
            BEGIN
                -- Tăng số lần đăng nhập sai
                UPDATE NguoiDung 
                SET SoLanDangNhapSai = SoLanDangNhapSai + 1
                WHERE MaNguoiDung = @MaNguoiDung;
                
                SET @KetQua = 0;
                SET @ThongBao = N'Mật khẩu không chính xác';
                ROLLBACK TRANSACTION;
                RETURN;
            END
        END
        ELSE
        BEGIN
            -- Kiểm tra mật khẩu cũ (chưa hash)
            IF @MatKhau != (SELECT MatKhau FROM NguoiDung WHERE MaNguoiDung = @MaNguoiDung)
            BEGIN
                UPDATE NguoiDung 
                SET SoLanDangNhapSai = SoLanDangNhapSai + 1
                WHERE MaNguoiDung = @MaNguoiDung;
                
                SET @KetQua = 0;
                SET @ThongBao = N'Mật khẩu không chính xác';
                ROLLBACK TRANSACTION;
                RETURN;
            END
        END
        
        -- Đăng nhập thành công
        UPDATE NguoiDung 
        SET LanDangNhapCuoi = GETDATE(),
            SoLanDangNhapSai = 0,
            NgayKhoaTaiKhoan = NULL
        WHERE MaNguoiDung = @MaNguoiDung;
        
        -- Ghi lịch sử đăng nhập
        DECLARE @MaLichSu INT = ISNULL((SELECT MAX(MaLichSu) FROM LichSuDangNhap), 0) + 1;
        INSERT INTO LichSuDangNhap (MaLichSu, MaNguoiDung, ThoiDiemDN)
        VALUES (@MaLichSu, @MaNguoiDung, GETDATE());
        
        -- Ghi lịch sử hoạt động
        INSERT INTO LichSuHoatDong (MaNguoiDung, HanhDong, Module, ChiTiet, DiaChi_IP, UserAgent)
        VALUES (@MaNguoiDung, N'Đăng nhập', N'Hệ thống', N'Đăng nhập thành công', @DiaChi_IP, @UserAgent);
        
        SET @KetQua = 1;
        SET @ThongBao = N'Đăng nhập thành công';
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @KetQua = -1;
        SET @ThongBao = N'Lỗi hệ thống: ' + ERROR_MESSAGE();
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DangXuat]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Procedure đăng xuất
CREATE   PROCEDURE [dbo].[SP_DangXuat]
    @MaNguoiDung INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION
    
    BEGIN TRY
        -- Update thời điểm đăng xuất
        UPDATE LichSuDangNhap 
        SET ThoiDiemDX = GETDATE()
        WHERE MaNguoiDung = @MaNguoiDung 
        AND ThoiDiemDX IS NULL;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DanhGiaCapDo]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- MODULE 6: ĐÁNH GIÁ CẤP ĐỘ THIẾT BỊ
-- =============================================

-- Đánh giá cấp độ thiết bị
CREATE   PROCEDURE [dbo].[SP_DanhGiaCapDo]
    @MaTTB INT,
    @CapDo INT,
    @NguoiDanhGia INT,
    @GhiChu NVARCHAR(500) = NULL,
    @KetQua INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION
    
    BEGIN TRY
        -- Validate cấp độ
        IF @CapDo NOT BETWEEN 1 AND 5
        BEGIN
            SET @KetQua = 0; -- Cấp độ không hợp lệ (1-5)
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        INSERT INTO DanhGiaCapDo (MaTTB, CapDo, NguoiDanhGia, GhiChu)
        VALUES (@MaTTB, @CapDo, @NguoiDanhGia, @GhiChu);
        
        SET @KetQua = 1; -- Thành công
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        SET @KetQua = -1; -- Lỗi
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DoiMatKhau]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Procedure đổi mật khẩu
CREATE   PROCEDURE [dbo].[SP_DoiMatKhau]
    @MaNguoiDung INT,
    @MatKhauCu NVARCHAR(50),
    @MatKhauMoi NVARCHAR(50),
    @KetQua INT OUTPUT,
    @ThongBao NVARCHAR(255) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION
    
    BEGIN TRY
        DECLARE @MatKhauHash NVARCHAR(255), @Salt NVARCHAR(255);
        
        -- Lấy thông tin hiện tại
        SELECT @MatKhauHash = MatKhauHash, @Salt = Salt
        FROM NguoiDung WHERE MaNguoiDung = @MaNguoiDung;
        
        -- Kiểm tra mật khẩu cũ
        IF @MatKhauHash IS NOT NULL
        BEGIN
            DECLARE @MatKhauCuCheck NVARCHAR(255) = HASHBYTES('SHA2_256', @MatKhauCu + @Salt);
            IF @MatKhauCuCheck != @MatKhauHash
            BEGIN
                SET @KetQua = 0;
                SET @ThongBao = N'Mật khẩu cũ không chính xác';
                ROLLBACK TRANSACTION;
                RETURN;
            END
        END
        ELSE
        BEGIN
            -- Kiểm tra mật khẩu cũ chưa hash
            IF @MatKhauCu != (SELECT MatKhau FROM NguoiDung WHERE MaNguoiDung = @MaNguoiDung)
            BEGIN
                SET @KetQua = 0;
                SET @ThongBao = N'Mật khẩu cũ không chính xác';
                ROLLBACK TRANSACTION;
                RETURN;
            END
        END
        
        -- Tạo salt mới và hash mật khẩu mới
        SET @Salt = CAST(NEWID() AS NVARCHAR(255));
        SET @MatKhauHash = HASHBYTES('SHA2_256', @MatKhauMoi + @Salt);
        
        -- Cập nhật mật khẩu
        UPDATE NguoiDung 
        SET MatKhauHash = @MatKhauHash,
            Salt = @Salt,
            MatKhau = NULL, -- Xóa mật khẩu cũ không hash
            NgayCapNhat = GETDATE()
        WHERE MaNguoiDung = @MaNguoiDung;
        
        -- Ghi lịch sử
        INSERT INTO LichSuHoatDong (MaNguoiDung, HanhDong, Module, ChiTiet)
        VALUES (@MaNguoiDung, N'Đổi mật khẩu', N'Bảo mật', N'Đổi mật khẩu thành công');
        
        SET @KetQua = 1;
        SET @ThongBao = N'Đổi mật khẩu thành công';
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @KetQua = -1;
        SET @ThongBao = N'Lỗi hệ thống: ' + ERROR_MESSAGE();
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetCapDoThietBi]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Lấy cấp độ hiện tại của thiết bị
CREATE   PROCEDURE [dbo].[SP_GetCapDoThietBi]
    @MaTTB INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT TOP 1 d.CapDo, d.NgayDanhGia, n.TenDangNhap as NguoiDanhGia, d.GhiChu
    FROM DanhGiaCapDo d WITH (NOLOCK)
    INNER JOIN NguoiDung n WITH (NOLOCK) ON d.NguoiDanhGia = n.MaNguoiDung
    WHERE d.MaTTB = @MaTTB
    ORDER BY d.NgayDanhGia DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_HoanThanhSuaChua]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Hoàn thành sửa chữa
CREATE   PROCEDURE [dbo].[SP_HoanThanhSuaChua]
    @MaSuaChua INT,
    @ChiPhi DECIMAL(18,2),
    @TinhTrangMoi NVARCHAR(100),
    @KetQua INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION
    
    BEGIN TRY
        DECLARE @MaTTB INT;
        
        -- Lấy thông tin sửa chữa
        SELECT @MaTTB = MaTTB
        FROM LichSuSuaChua WITH (UPDLOCK, HOLDLOCK)
        WHERE MaSuaChua = @MaSuaChua AND NgayKetThuc IS NULL;
        
        IF @MaTTB IS NULL
        BEGIN
            SET @KetQua = 0; -- Không tìm thấy hoặc đã hoàn thành
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Cập nhật lịch sử sửa chữa
        UPDATE LichSuSuaChua WITH (XLOCK)
        SET NgayKetThuc = GETDATE(),
            ChiPhi = @ChiPhi,
            TrangThai = N'Hoàn thành'
        WHERE MaSuaChua = @MaSuaChua;
        
        -- Cập nhật tình trạng thiết bị
        UPDATE TrangTB WITH (XLOCK)
        SET TinhTrang = @TinhTrangMoi
        WHERE MaTTB = @MaTTB;
        
        SET @KetQua = 1; -- Thành công
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        SET @KetQua = -1; -- Lỗi
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_LichThucHanh_CheckConflict]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SP_LichThucHanh_CheckConflict (Kiểm tra trùng lịch)
CREATE PROCEDURE [dbo].[SP_LichThucHanh_CheckConflict]
    @ThoiGianBD DATETIME,
    @ThoiGianKT DATETIME,
    @MaLichLoaiTru INT = NULL,
    @HasConflict BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    IF EXISTS (SELECT 1 FROM LichThucHanh
               WHERE TrangThai NOT IN (N'Đã hủy', N'Hoàn thành')
               AND (@MaLichLoaiTru IS NULL OR MaLich != @MaLichLoaiTru)
               AND ((@ThoiGianBD < ThoiGianKT AND @ThoiGianKT > ThoiGianBD)))
    BEGIN
        SET @HasConflict = 1;
    END
    ELSE
    BEGIN
        SET @HasConflict = 0;
    END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_LichThucHanh_Delete]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- SP_LichThucHanh_Delete  
CREATE PROCEDURE [dbo].[SP_LichThucHanh_Delete]
    @MaLich INT,
    @MaNguoiDung INT,
    @KetQua INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION
    
    BEGIN TRY
        -- Kiểm tra quyền xóa (chỉ chủ sở hữu hoặc admin)
        DECLARE @OwnerUserId INT, @CurrentStatus NVARCHAR(50);
        SELECT @OwnerUserId = MaNguoiDung, @CurrentStatus = TrangThai
        FROM LichThucHanh 
        WHERE MaLich = @MaLich;
        
        IF @OwnerUserId IS NULL
        BEGIN
            SET @KetQua = 0; -- Không tìm thấy
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra quyền (chủ sở hữu hoặc có quyền admin)
        IF @OwnerUserId != @MaNguoiDung
        BEGIN
            -- Kiểm tra quyền admin
            IF NOT EXISTS (SELECT 1 FROM NguoiDungVaiTro nv 
                          INNER JOIN VaiTro v ON nv.MaVaiTro = v.MaVaiTro
                          WHERE nv.MaNguoiDung = @MaNguoiDung 
                          AND v.TenVaiTro IN ('Admin', 'QuanLy')
                          AND nv.TrangThai = 1)
            BEGIN
                SET @KetQua = 0; -- Không có quyền
                ROLLBACK TRANSACTION;
                RETURN;
            END
        END
        
        -- Không cho phép xóa lịch đang thực hiện
        IF @CurrentStatus = N'Đang thực hiện'
        BEGIN
            SET @KetQua = 0; -- Không thể xóa lịch đang thực hiện
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Soft delete - chỉ thay đổi trạng thái
        UPDATE LichThucHanh WITH (XLOCK)
        SET TrangThai = N'Đã hủy'
        WHERE MaLich = @MaLich;
        
        SET @KetQua = 1; -- Thành công
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        SET @KetQua = -1; -- Lỗi
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_LichThucHanh_GetByDate]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SP_LichThucHanh_GetByDate (Lấy lịch theo ngày)
CREATE PROCEDURE [dbo].[SP_LichThucHanh_GetByDate]
    @Ngay DATE
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT l.MaLich, l.TrangThai, l.ThoiGianBD, l.ThoiGianKT, l.MaNguoiDung, 
           ISNULL(n.HoTen, n.TenDangNhap) as TenNguoiDung
    FROM LichThucHanh l 
    LEFT JOIN NguoiDung n ON l.MaNguoiDung = n.MaNguoiDung
    WHERE CAST(l.ThoiGianBD as DATE) = @Ngay
    ORDER BY l.ThoiGianBD;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_LichThucHanh_GetById]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- SP_LichThucHanh_GetById
CREATE PROCEDURE [dbo].[SP_LichThucHanh_GetById]
    @MaLich INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT l.MaLich, l.TrangThai, l.ThoiGianBD, l.ThoiGianKT, l.MaNguoiDung, 
           n.TenDangNhap as TenNguoiDung, n.HoTen,
           n.Email, n.TrangThaiTaiKhoan
    FROM LichThucHanh l 
    LEFT JOIN NguoiDung n ON l.MaNguoiDung = n.MaNguoiDung
    WHERE l.MaLich = @MaLich;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_LichThucHanh_GetByStatus]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SP_LichThucHanh_GetByStatus (Lấy lịch theo trạng thái)
CREATE PROCEDURE [dbo].[SP_LichThucHanh_GetByStatus]
    @TrangThai NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT l.MaLich, l.TrangThai, l.ThoiGianBD, l.ThoiGianKT, l.MaNguoiDung, 
           ISNULL(n.HoTen, n.TenDangNhap) as TenNguoiDung
    FROM LichThucHanh l 
    LEFT JOIN NguoiDung n ON l.MaNguoiDung = n.MaNguoiDung
    WHERE l.TrangThai = @TrangThai
    ORDER BY l.ThoiGianBD DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_LichThucHanh_Update]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- SP_LichThucHanh_Update
CREATE PROCEDURE [dbo].[SP_LichThucHanh_Update]
    @MaLich INT,
    @ThoiGianBD DATETIME,
    @ThoiGianKT DATETIME,
    @TrangThai NVARCHAR(50),
    @GhiChu NVARCHAR(500) = NULL,
    @KetQua INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION
    
    BEGIN TRY
        -- Validate thời gian
        IF @ThoiGianBD >= @ThoiGianKT
        BEGIN
            SET @KetQua = 0; -- Thời gian không hợp lệ
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra lịch tồn tại
        DECLARE @CurrentUserId INT, @CurrentStatus NVARCHAR(50);
        SELECT @CurrentUserId = MaNguoiDung, @CurrentStatus = TrangThai
        FROM LichThucHanh 
        WHERE MaLich = @MaLich;
        
        IF @CurrentUserId IS NULL
        BEGIN
            SET @KetQua = 0; -- Không tìm thấy
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra trùng lịch (nếu thay đổi thời gian)
        IF EXISTS (SELECT 1 FROM LichThucHanh WITH (UPDLOCK)
                   WHERE MaNguoiDung = @CurrentUserId
                   AND MaLich != @MaLich
                   AND TrangThai NOT IN (N'Đã hủy', N'Hoàn thành')
                   AND ((@ThoiGianBD BETWEEN ThoiGianBD AND ThoiGianKT)
                   OR (@ThoiGianKT BETWEEN ThoiGianBD AND ThoiGianKT)
                   OR (ThoiGianBD BETWEEN @ThoiGianBD AND @ThoiGianKT)))
        BEGIN
            SET @KetQua = 0; -- Trùng lịch
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Cập nhật
        UPDATE LichThucHanh WITH (XLOCK)
        SET ThoiGianBD = @ThoiGianBD,
            ThoiGianKT = @ThoiGianKT,
            TrangThai = @TrangThai
        WHERE MaLich = @MaLich;
        
        SET @KetQua = 1; -- Thành công
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        SET @KetQua = -1; -- Lỗi
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Loai_Delete]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[SP_Loai_Delete]
    @MaLoai INT,
    @KetQua INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION
    
    BEGIN TRY
        -- Kiểm tra ràng buộc
        IF EXISTS (SELECT 1 FROM TrangTB WHERE MaLoai = @MaLoai)
        BEGIN
            SET @KetQua = 0; -- Không thể xóa do ràng buộc
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        DELETE FROM Loai WITH (XLOCK)
        WHERE MaLoai = @MaLoai;
        
        IF @@ROWCOUNT > 0
            SET @KetQua = 1; -- Thành công
        ELSE
            SET @KetQua = 0; -- Không tìm thấy
            
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        SET @KetQua = -1; -- Lỗi
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Loai_Insert]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- MODULE 2: MASTER DATA MANAGEMENT
-- =============================================

-- Quản lý Loại thiết bị
CREATE   PROCEDURE [dbo].[SP_Loai_Insert]
    @MaLoai INT,
    @TenLoai NVARCHAR(100),
    @KetQua INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION
    
    BEGIN TRY
        -- Kiểm tra trùng lặp
        IF EXISTS (SELECT 1 FROM Loai WITH (UPDLOCK) WHERE MaLoai = @MaLoai)
        BEGIN
            SET @KetQua = 0; -- Đã tồn tại
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        INSERT INTO Loai (MaLoai, TenLoai)
        VALUES (@MaLoai, @TenLoai);
        
        SET @KetQua = 1; -- Thành công
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        SET @KetQua = -1; -- Lỗi
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Loai_Update]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[SP_Loai_Update]
    @MaLoai INT,
    @TenLoai NVARCHAR(100),
    @KetQua INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION
    
    BEGIN TRY
        UPDATE Loai WITH (XLOCK)
        SET TenLoai = @TenLoai
        WHERE MaLoai = @MaLoai;
        
        IF @@ROWCOUNT > 0
            SET @KetQua = 1; -- Thành công
        ELSE
            SET @KetQua = 0; -- Không tìm thấy
            
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        SET @KetQua = -1; -- Lỗi
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_NhapThietBi]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Nhập thiết bị
CREATE   PROCEDURE [dbo].[SP_NhapThietBi]
    @MaTTB INT,
    @SoLuong INT,
    @NguoiTao INT,
    @GhiChu NVARCHAR(500) = NULL,
    @KetQua INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION
    
    BEGIN TRY
        -- Validate
        IF @SoLuong <= 0
        BEGIN
            SET @KetQua = 0; -- Số lượng không hợp lệ
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        INSERT INTO XuatNhapTon (LoaiPhieu, MaTTB, SoLuong, NguoiTao, GhiChu)
        VALUES ('NHAP', @MaTTB, @SoLuong, @NguoiTao, @GhiChu);
        
        SET @KetQua = 1; -- Thành công
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        SET @KetQua = -1; -- Lỗi
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_TaoPhieuSuaChua]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- MODULE 5: SỬA CHỮA BẢO HÀNH
-- =============================================

-- Tạo phiếu sửa chữa
CREATE   PROCEDURE [dbo].[SP_TaoPhieuSuaChua]
    @MaTTB INT,
    @LoaiSuaChua NVARCHAR(50),
    @MoTa NVARCHAR(500),
    @NguoiThucHien INT,
    @MaSuaChua INT OUTPUT,
    @KetQua INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION
    
    BEGIN TRY
        -- Validate
        IF @LoaiSuaChua NOT IN (N'Sửa chữa', N'Bảo hành')
        BEGIN
            SET @KetQua = 0; -- Loại sửa chữa không hợp lệ
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra thiết bị đang được sửa chữa
        IF EXISTS (SELECT 1 FROM LichSuSuaChua 
                   WHERE MaTTB = @MaTTB AND NgayKetThuc IS NULL)
        BEGIN
            SET @KetQua = 0; -- Thiết bị đang được sửa chữa
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        INSERT INTO LichSuSuaChua (MaTTB, NgayBatDau, LoaiSuaChua, MoTa, NguoiThucHien)
        VALUES (@MaTTB, GETDATE(), @LoaiSuaChua, @MoTa, @NguoiThucHien);
        
        SET @MaSuaChua = SCOPE_IDENTITY();
        
        -- Cập nhật số lần sửa
        UPDATE TrangTB WITH (XLOCK)
        SET SoLanSua = ISNULL(SoLanSua, 0) + 1,
            TinhTrang = N'Đang sửa chữa'
        WHERE MaTTB = @MaTTB;
        
        SET @KetQua = 1; -- Thành công
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        SET @KetQua = -1; -- Lỗi
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_TaoTaiKhoan]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Procedure tạo tài khoản với hash mật khẩu
CREATE   PROCEDURE [dbo].[SP_TaoTaiKhoan]
    @TenDangNhap NVARCHAR(50),
    @MatKhau NVARCHAR(50),
    @Email NVARCHAR(100),
    @HoTen NVARCHAR(100),
    @MaVaiTro INT,
    @MaNguoiTao INT,
    @KetQua INT OUTPUT,
    @MaNguoiDung INT OUTPUT,
    @ThongBao NVARCHAR(255) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION
    
    BEGIN TRY
        -- Kiểm tra tên đăng nhập đã tồn tại
        IF EXISTS (SELECT 1 FROM NguoiDung WHERE TenDangNhap = @TenDangNhap)
        BEGIN
            SET @KetQua = 0;
            SET @ThongBao = N'Tên đăng nhập đã tồn tại';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra email đã tồn tại
        IF EXISTS (SELECT 1 FROM NguoiDung WHERE Email = @Email)
        BEGIN
            SET @KetQua = 0;
            SET @ThongBao = N'Email đã được sử dụng';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Tạo salt và hash mật khẩu
        DECLARE @Salt NVARCHAR(255) = CAST(NEWID() AS NVARCHAR(255));
        DECLARE @MatKhauHash NVARCHAR(255) = HASHBYTES('SHA2_256', @MatKhau + @Salt);
        
        -- Lấy MaNguoiDung tiếp theo
        SET @MaNguoiDung = ISNULL((SELECT MAX(MaNguoiDung) FROM NguoiDung), 0) + 1;
        
        -- Tạo tài khoản
        INSERT INTO NguoiDung (MaNguoiDung, TenDangNhap, MatKhauHash, Salt, Email, HoTen, TrangThaiTaiKhoan)
        VALUES (@MaNguoiDung, @TenDangNhap, @MatKhauHash, @Salt, @Email, @HoTen, 1);
        
        -- Phân quyền
        INSERT INTO NguoiDungVaiTro (MaNguoiDung, MaVaiTro)
        VALUES (@MaNguoiDung, @MaVaiTro);
        
        -- Ghi lịch sử
        INSERT INTO LichSuHoatDong (MaNguoiDung, HanhDong, Module, ChiTiet)
        VALUES (@MaNguoiTao, N'Tạo tài khoản', N'Quản lý người dùng', 
                N'Tạo tài khoản: ' + @TenDangNhap + N' (' + @HoTen + N')');
        
        SET @KetQua = 1;
        SET @ThongBao = N'Tạo tài khoản thành công';
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @KetQua = -1;
        SET @ThongBao = N'Lỗi hệ thống: ' + ERROR_MESSAGE();
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_ThongKeDanhGiaCapDo]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Thống kê đánh giá cấp độ
CREATE   PROCEDURE [dbo].[SP_ThongKeDanhGiaCapDo]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT CapDo,
           COUNT(*) as SoLuongDanhGia,
           CAST(COUNT(*) * 100.0 / SUM(COUNT(*)) OVER() as DECIMAL(5,2)) as TyLe
    FROM (
        SELECT DISTINCT d1.MaTTB, d1.CapDo
        FROM DanhGiaCapDo d1 WITH (NOLOCK)
        WHERE d1.NgayDanhGia = (
            SELECT MAX(d2.NgayDanhGia)
            FROM DanhGiaCapDo d2
            WHERE d2.MaTTB = d1.MaTTB
        )
    ) as LatestRatings
    GROUP BY CapDo
    ORDER BY CapDo;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_ThongKeSuDungTheoThang]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Thống kê sử dụng theo tháng
CREATE   PROCEDURE [dbo].[SP_ThongKeSuDungTheoThang]
    @Nam INT
AS
BEGIN
    SET NOCOUNT ON;
    
    WITH MonthSeries AS (
        SELECT 1 as Thang UNION SELECT 2 UNION SELECT 3 UNION SELECT 4 
        UNION SELECT 5 UNION SELECT 6 UNION SELECT 7 UNION SELECT 8 
        UNION SELECT 9 UNION SELECT 10 UNION SELECT 11 UNION SELECT 12
    )
    SELECT m.Thang,
           ISNULL(COUNT(l.MaLich), 0) as SoLichThucHanh,
           ISNULL(SUM(DATEDIFF(HOUR, l.ThoiGianBD, l.ThoiGianKT)), 0) as TongGioSuDung
    FROM MonthSeries m
    LEFT JOIN LichThucHanh l WITH (NOLOCK) ON MONTH(l.ThoiGianBD) = m.Thang 
                                            AND YEAR(l.ThoiGianBD) = @Nam
                                            AND l.TrangThai = N'Hoàn thành'
    GROUP BY m.Thang
    ORDER BY m.Thang;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_ThongKeThietBiTheoPhong]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- MODULE 8: BÁO CÁO THỐNG KÊ
-- =============================================

-- Thống kê thiết bị theo phòng
CREATE   PROCEDURE [dbo].[SP_ThongKeThietBiTheoPhong]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT p.MaPhongMay, p.TenPhongMay,
           COUNT(t.MaTTB) as TongThietBi,
           SUM(CASE WHEN t.TinhTrang = N'Tốt' THEN 1 ELSE 0 END) as ThietBiTot,
           SUM(CASE WHEN t.TinhTrang = N'Hỏng' THEN 1 ELSE 0 END) as ThietBiHong,
           SUM(CASE WHEN t.TinhTrang = N'Đang sửa chữa' THEN 1 ELSE 0 END) as ThietBiDangSua,
           AVG(CAST(t.GiaTien as FLOAT)) as GiaTriTrungBinh
    FROM PhongMay p WITH (NOLOCK)
    LEFT JOIN TrangTB t WITH (NOLOCK) ON p.MaPhongMay = t.MaPhongMay
    GROUP BY p.MaPhongMay, p.TenPhongMay
    ORDER BY p.TenPhongMay;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_ThuongHieu_GetAll]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Tương tự cho ThuongHieu
CREATE   PROCEDURE [dbo].[SP_ThuongHieu_GetAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT MaThuongHieu, TenThuongHieu 
    FROM ThuongHieu WITH (NOLOCK)
    ORDER BY TenThuongHieu;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_TrangTB_Delete]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Xóa trang thiết bị
CREATE   PROCEDURE [dbo].[SP_TrangTB_Delete]
    @MaTTB INT,
    @KetQua INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION
    
    BEGIN TRY
        -- Kiểm tra ràng buộc
        IF EXISTS (SELECT 1 FROM DanhGiaCapDo WHERE MaTTB = @MaTTB)
        OR EXISTS (SELECT 1 FROM LichSuSuaChua WHERE MaTTB = @MaTTB)
        OR EXISTS (SELECT 1 FROM XuatNhapTon WHERE MaTTB = @MaTTB)
        BEGIN
            SET @KetQua = 0; -- Không thể xóa do ràng buộc
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        DELETE FROM TrangTB WITH (XLOCK)
        WHERE MaTTB = @MaTTB;
        
        IF @@ROWCOUNT > 0
            SET @KetQua = 1; -- Thành công
        ELSE
            SET @KetQua = 0; -- Không tìm thấy
            
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        SET @KetQua = -1; -- Lỗi
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_TrangTB_Insert]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- MODULE 3: EQUIPMENT MANAGEMENT
-- =============================================

-- Thêm trang thiết bị
CREATE   PROCEDURE [dbo].[SP_TrangTB_Insert]
    @MaPhongMay INT,
    @MaLoai INT,
    @GiaTien INT,
    @TinhTrang NVARCHAR(100),
    @NgayNhap DATE,
    @MaThuongHieu INT,
    @MaTTB INT OUTPUT,
    @KetQua INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION
    
    BEGIN TRY
        -- Validate input
        IF @MaPhongMay IS NULL OR @MaLoai IS NULL OR @NgayNhap IS NULL OR @MaThuongHieu IS NULL
        BEGIN
            SET @KetQua = 0; -- Dữ liệu không hợp lệ
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra foreign key
        IF NOT EXISTS (SELECT 1 FROM PhongMay WHERE MaPhongMay = @MaPhongMay)
        OR NOT EXISTS (SELECT 1 FROM Loai WHERE MaLoai = @MaLoai)
        OR NOT EXISTS (SELECT 1 FROM ThuongHieu WHERE MaThuongHieu = @MaThuongHieu)
        BEGIN
            SET @KetQua = 0; -- Dữ liệu tham chiếu không tồn tại
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        INSERT INTO TrangTB (MaPhongMay, MaLoai, GiaTien, TinhTrang, NgayNhap, SoLanSua, MaThuongHieu)
        VALUES (@MaPhongMay, @MaLoai, @GiaTien, @TinhTrang, @NgayNhap, 0, @MaThuongHieu);
        
        SET @MaTTB = SCOPE_IDENTITY();
        SET @KetQua = 1; -- Thành công
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        SET @KetQua = -1; -- Lỗi
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_TrangTB_Search]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Tìm kiếm trang thiết bị
CREATE   PROCEDURE [dbo].[SP_TrangTB_Search]
    @MaPhongMay INT = NULL,
    @MaLoai INT = NULL,
    @TinhTrang NVARCHAR(100) = NULL,
    @MaThuongHieu INT = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 20
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    
    SELECT t.MaTTB, t.MaPhongMay, p.TenPhongMay, t.MaLoai, l.TenLoai,
           t.GiaTien, t.TinhTrang, t.NgayNhap, t.SoLanSua,
           t.MaThuongHieu, th.TenThuongHieu
    FROM TrangTB t WITH (NOLOCK)
    INNER JOIN PhongMay p WITH (NOLOCK) ON t.MaPhongMay = p.MaPhongMay
    INNER JOIN Loai l WITH (NOLOCK) ON t.MaLoai = l.MaLoai
    INNER JOIN ThuongHieu th WITH (NOLOCK) ON t.MaThuongHieu = th.MaThuongHieu
    WHERE (@MaPhongMay IS NULL OR t.MaPhongMay = @MaPhongMay)
    AND (@MaLoai IS NULL OR t.MaLoai = @MaLoai)
    AND (@TinhTrang IS NULL OR t.TinhTrang LIKE '%' + @TinhTrang + '%')
    AND (@MaThuongHieu IS NULL OR t.MaThuongHieu = @MaThuongHieu)
    ORDER BY t.NgayNhap DESC
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
    
    -- Tổng số bản ghi
    SELECT COUNT(*) as TotalRecords
    FROM TrangTB t WITH (NOLOCK)
    WHERE (@MaPhongMay IS NULL OR t.MaPhongMay = @MaPhongMay)
    AND (@MaLoai IS NULL OR t.MaLoai = @MaLoai)
    AND (@TinhTrang IS NULL OR t.TinhTrang LIKE '%' + @TinhTrang + '%')
    AND (@MaThuongHieu IS NULL OR t.MaThuongHieu = @MaThuongHieu);
END
GO
/****** Object:  StoredProcedure [dbo].[SP_TrangTB_Update]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_TrangTB_Update]
    @MaTTB INT,
    @MaPhongMay INT,
    @MaLoai INT,
    @GiaTien INT,
    @TinhTrang NVARCHAR(100),
    @MaThuongHieu INT,
    @NguoiCapNhat INT,
    @KetQua INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION
    
    BEGIN TRY
        DECLARE @ThongTinCu NVARCHAR(MAX), @ThongTinMoi NVARCHAR(MAX);
        
        -- Lấy thông tin cũ
        SELECT @ThongTinCu = CONCAT('PhongMay:', MaPhongMay, 
                                  ';Loai:', MaLoai,
                                  ';GiaTien:', GiaTien,
                                  ';TinhTrang:', TinhTrang,
                                  ';ThuongHieu:', MaThuongHieu)
        FROM TrangTB WITH (UPDLOCK, HOLDLOCK)
        WHERE MaTTB = @MaTTB;
        
        IF @ThongTinCu IS NULL
        BEGIN
            SET @KetQua = 0; -- Không tìm thấy
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Cập nhật
        UPDATE TrangTB WITH (XLOCK)
        SET MaPhongMay = @MaPhongMay,
            MaLoai = @MaLoai,
            GiaTien = @GiaTien,
            TinhTrang = @TinhTrang,
            MaThuongHieu = @MaThuongHieu
        WHERE MaTTB = @MaTTB;
        
        -- Tạo thông tin mới
        SET @ThongTinMoi = CONCAT('PhongMay:', @MaPhongMay, 
                                 ';Loai:', @MaLoai,
                                 ';GiaTien:', @GiaTien,
                                 ';TinhTrang:', @TinhTrang,
                                 ';ThuongHieu:', @MaThuongHieu);
        
        -- Log thay đổi với ID tự động
        DECLARE @MaNhatKy INT;
        SELECT @MaNhatKy = ISNULL(MAX(MaNhatKy), 0) + 1 FROM NhatKyThayDoi;
        
        INSERT INTO NhatKyThayDoi (MaNhatKy, NoiDung, ThongTinCu, ThongTinMoi)
        VALUES (@MaNhatKy, N'Cập nhật thông tin trang thiết bị', @ThongTinCu, @ThongTinMoi);
        
        SET @KetQua = 1; -- Thành công
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        SET @KetQua = -1; -- Lỗi
        ROLLBACK TRANSACTION;
        
        -- Log error
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_XuatThietBi]    Script Date: 04/05/2014 7:11:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- MODULE 4: XUẤT NHẬP TỒN
-- =============================================

-- Xuất thiết bị
CREATE   PROCEDURE [dbo].[SP_XuatThietBi]
    @MaTTB INT,
    @SoLuong INT,
    @NguoiTao INT,
    @GhiChu NVARCHAR(500) = NULL,
    @KetQua INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION
    
    BEGIN TRY
        -- Validate
        IF @SoLuong <= 0
        BEGIN
            SET @KetQua = 0; -- Số lượng không hợp lệ
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra tồn tại thiết bị
        IF NOT EXISTS (SELECT 1 FROM TrangTB WHERE MaTTB = @MaTTB)
        BEGIN
            SET @KetQua = 0; -- Thiết bị không tồn tại
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        INSERT INTO XuatNhapTon (LoaiPhieu, MaTTB, SoLuong, NguoiTao, GhiChu)
        VALUES ('XUAT', @MaTTB, @SoLuong, @NguoiTao, @GhiChu);
        
        SET @KetQua = 1; -- Thành công
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        SET @KetQua = -1; -- Lỗi
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
