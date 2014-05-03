using QLVatTuPhongThiNghiem.Models.ViewModels;
using QLVatTuPhongThiNghiem.Repositories.Interfaces;
using QLVatTuPhongThiNghiem.Services.Interfaces;

namespace QLVatTuPhongThiNghiem.Services.Implements
{
    public class ThongBaoService : IThongBaoService
    {
        private readonly IThongBaoRepository _thongBaoRepository;
        private readonly ILichSuHoatDongService _lichSuHoatDongService;

        public ThongBaoService(IThongBaoRepository thongBaoRepository, ILichSuHoatDongService lichSuHoatDongService)
        {
            _thongBaoRepository = thongBaoRepository;
            _lichSuHoatDongService = lichSuHoatDongService;
        }

        public async Task<(bool Success, string Message)> GuiThongBaoAsync(ThongBaoViewModel model)
        {
            try
            {
                // Business validation
                if (string.IsNullOrWhiteSpace(model.TieuDe))
                {
                    return (false, "Tiêu đề thông báo không được để trống");
                }

                if (string.IsNullOrWhiteSpace(model.NoiDung))
                {
                    return (false, "Nội dung thông báo không được để trống");
                }

                if (model.TieuDe.Length > 200)
                {
                    return (false, "Tiêu đề không được quá 200 ký tự");
                }

                var validTypes = new[] { "Thong_Tin", "Canh_Bao", "Loi", "Thanh_Cong" };
                if (!validTypes.Contains(model.LoaiThongBao))
                {
                    return (false, "Loại thông báo không hợp lệ");
                }

                // Set default values
                model.NgayTao = DateTime.Now;
                model.DaDoc = false;

                var (ketQua, thongBao) = await _thongBaoRepository.GuiThongBaoAsync(model);
                return (ketQua == 1, thongBao);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        public async Task<IEnumerable<ThongBaoViewModel>> GetThongBaoByNguoiDungAsync(int maNguoiDung)
        {
            return await _thongBaoRepository.GetThongBaoByNguoiDungAsync(maNguoiDung);
        }

        public async Task<IEnumerable<ThongBaoViewModel>> GetAllThongBaoAsync()
        {
            return await _thongBaoRepository.GetAllThongBaoAsync();
        }

        public async Task<(bool Success, string Message)> DanhDauDaDocAsync(int maThongBao, int maNguoiDung)
        {
            try
            {
                var (ketQua, thongBao) = await _thongBaoRepository.DanhDauDaDocAsync(maThongBao, maNguoiDung);
                return (ketQua == 1, thongBao);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        public async Task<int> GetSoThongBaoChuaDocAsync(int maNguoiDung)
        {
            return await _thongBaoRepository.GetSoThongBaoChuaDocAsync(maNguoiDung);
        }

        public async Task<(bool Success, string Message)> GuiThongBaoHeThongAsync(string tieuDe, string noiDung, string loaiThongBao = "Thong_Tin")
        {
            // Gửi thông báo hệ thống cho tất cả người dùng
            var thongBao = new ThongBaoViewModel
            {
                TieuDe = tieuDe,
                NoiDung = noiDung,
                LoaiThongBao = loaiThongBao,
                MaNguoiNhan = null, // null = gửi cho tất cả
                NgayTao = DateTime.Now
            };

            return await GuiThongBaoAsync(thongBao);
        }

        public async Task<(bool Success, string Message)> GuiThongBaoNhomAsync(string tieuDe, string noiDung, List<int> danhSachNguoiNhan, string loaiThongBao = "Thong_Tin")
        {
            try
            {
                int successCount = 0;
                foreach (var maNguoiNhan in danhSachNguoiNhan)
                {
                    var thongBao = new ThongBaoViewModel
                    {
                        TieuDe = tieuDe,
                        NoiDung = noiDung,
                        LoaiThongBao = loaiThongBao,
                        MaNguoiNhan = maNguoiNhan,
                        NgayTao = DateTime.Now
                    };

                    var (success, _) = await GuiThongBaoAsync(thongBao);
                    if (success) successCount++;
                }

                return (true, $"Đã gửi thông báo cho {successCount}/{danhSachNguoiNhan.Count} người dùng");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<Dictionary<string, int>> GetThongKeThongBaoAsync(int? maNguoiDung = null)
        {
            var allThongBao = maNguoiDung.HasValue
                ? await GetThongBaoByNguoiDungAsync(maNguoiDung.Value)
                : await GetAllThongBaoAsync();

            return new Dictionary<string, int>
            {
                ["TongSo"] = allThongBao.Count(),
                ["ChuaDoc"] = allThongBao.Count(t => !t.DaDoc),
                ["DaDoc"] = allThongBao.Count(t => t.DaDoc),
                ["ThongTin"] = allThongBao.Count(t => t.LoaiThongBao == "Thong_Tin"),
                ["CanhBao"] = allThongBao.Count(t => t.LoaiThongBao == "Canh_Bao"),
                ["Loi"] = allThongBao.Count(t => t.LoaiThongBao == "Loi"),
                ["ThanhCong"] = allThongBao.Count(t => t.LoaiThongBao == "Thanh_Cong"),
                ["HomNay"] = allThongBao.Count(t => t.NgayTao.Date == DateTime.Today),
                ["TuanNay"] = allThongBao.Count(t => t.NgayTao >= DateTime.Today.AddDays(-7))
            };
        }
    }
}