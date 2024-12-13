using Microsoft.AspNetCore.Mvc;
using Version2.Models;
using Version2.Helpers; // Tích hợp helper cho session
using Version2.ViewModels;
using Newtonsoft.Json;
using System.Linq;
using Version2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Version2.Controllers
{
    public class GiohangController : Controller
    {
        private readonly HeThongBanSachContext _context; // Database context
        private readonly IVnPayService _vnPayService;


        public GiohangController(HeThongBanSachContext context, IVnPayService vnPayservice)
        {
            _context = context;
            _vnPayService = vnPayservice;
        }

        // Lấy giỏ hàng từ session
        public List<CartItem> GioHang
        {
            get
            {
                var gioHang = HttpContext.Session.Get<List<CartItem>>("GIO_HANG") ?? new List<CartItem>();
                return gioHang;
            }
        }

        // Trang hiển thị giỏ hàng
        public IActionResult Index()
        {
            return View(GioHang);
        }

        // Thêm sản phẩm vào giỏ hàng
        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var gioHang = GioHang;
            var item = gioHang.FirstOrDefault(p => p.IdSach == id);

            if (item == null)
            {
                var sach = _context.Saches.SingleOrDefault(s => s.Idsach == id);
                if (sach == null)
                {
                    TempData["Message"] = "Sản phẩm không tồn tại.";
                    return RedirectToAction("Index");
                }

                // Thêm sản phẩm vào giỏ hàng
                item = new CartItem
                {
                    IdSach = sach.Idsach,
                    TenSach = sach.TenSach,
                    Gia = sach.Gia,
                    SoLuong = quantity,
                    HinhAnh = sach.HinhAnh
                };
                gioHang.Add(item);
            }
            else
            {
                // Cập nhật số lượng
                item.SoLuong += quantity;
            }

            HttpContext.Session.Set("GIO_HANG", gioHang);
            return RedirectToAction("Index");
        }

        // Xóa sản phẩm khỏi giỏ hàng
        public IActionResult RemoveFromCart(int id)
        {
            var gioHang = GioHang;
            var item = gioHang.SingleOrDefault(p => p.IdSach == id);

            if (item != null)
            {
                gioHang.Remove(item);
                HttpContext.Session.Set("GIO_HANG", gioHang);
            }

            return RedirectToAction("Index");
        }

        // Cập nhật số lượng sản phẩm
        [HttpPost]
        public IActionResult UpdateCart(int id, int quantity)
        {
            var gioHang = GioHang;
            var item = gioHang.SingleOrDefault(p => p.IdSach == id);

            if (item != null)
            {
                if (quantity > 0)
                {
                    item.SoLuong = quantity;
                }
                else
                {
                    gioHang.Remove(item);
                }
                HttpContext.Session.Set("GIO_HANG", gioHang);
            }

            return RedirectToAction("Index");
        }
        //LẤY RANDOM IDKHACHHANG
        private int GetRandomDingoGiaoHang()
        {
            int randomId = _context.Set<Nguoigiaohang>()
                .OrderBy(ngh => Guid.NewGuid()) // Sắp xếp ngẫu nhiên
                .Select(ngh => ngh.IdnguoiGiaoHang) // Chỉ lấy IdnguoiGiaoHang
                .FirstOrDefault(); // Lấy bản ghi đầu tiên hoặc giá trị mặc định nếu không có

            return randomId;
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            if (GioHang.Count == 0)
            {
                return Redirect("/");
            }
            return View(GioHang);
        }

        // Xử lý thanh toán (đơn giản, lưu thông tin vào cơ sở dữ liệu)
        [HttpPost]
        public IActionResult Checkout(CheckoutVM model, string payment)
        {


            // Lấy thông tin khách hàng từ phiên đăng nhập
            //var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            var json = HttpContext.Session.GetString("Khachhang");
            if (string.IsNullOrEmpty(json))
            {
                TempData["Message"] = "Vui lòng đăng nhập.";
                return RedirectToAction("Login", "Home");
            }
            var userId = JsonConvert.DeserializeObject<Khachhang>(json);


            //string json = HttpContext.Session.GetString("Khachhang");
            if (userId == null)
            {
                TempData["Message"] = "Vui lòng đăng nhập để thanh toán.";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                // Tạo phiếu đặt hàng
                var phieuDatHang = new Phieudathang
                {
                    IdkhachHang = (userId.IdkhachHang), // ID khách hàng từ phiên đăng nhập
                    NgayLapHoaDon = DateTime.Now,
                    TrangThaiThanhToan = "Đã thanh toán" // Trạng thái mặc định
                };

                _context.Phieudathangs.Add(phieuDatHang);
                _context.SaveChanges();

                // Thêm chi tiết phiếu đặt hàng
                foreach (var item in GioHang)
                {
                    var chiTietPhieu = new Chitietphieudathang
                    {
                        IdphieuDatHang = phieuDatHang.IdphieuDatHang,
                        Idsach = item.IdSach,
                        SoLuong = item.SoLuong,
                        Gia = item.Gia
                    };

                    _context.Chitietphieudathangs.Add(chiTietPhieu);
                }

                _context.SaveChanges();

                // Lập hóa đơn nếu cần thiết
                var hoaDon = new Hoadon
                {
                    IdphieuDatHang = phieuDatHang.IdphieuDatHang,
                    NgayLap = DateTime.Now,
                    TongTien = GioHang.Sum(item => item.ThanhTien),
                    TrangThai = "Thanh Toán"
                };

                //Lập đơn hàng
                _context.Hoadons.Add(hoaDon);
                _context.SaveChanges();

                var donHang = new Donhang
                {
                    IdphieuDatHang = phieuDatHang.IdphieuDatHang,
                    IdnguoiGiaoHang = GetRandomDingoGiaoHang(),
                    NgayGiaoHang = DateTime.Now,
                    GhiChu = "Uy Tín và chât lượng",
                };

                _context.Donhangs.Add(donHang);
                _context.SaveChanges();

                if (payment == "Thanh")
                {
                    var vnPayModel = new VnPaymentRequestModel
                    {
                        Amount = (int)(GioHang.Sum(p => p.ThanhTien)),
                        CreatedDate = DateTime.Now,
                        Description = $"{userId.TenKhachHang} {userId.SoDienThoai}",
                        FullName = userId.TenKhachHang,
                        OrderId = hoaDon.IdhoaDon
                    };
                    string url = _vnPayService.CreatePaymentUrl(HttpContext, vnPayModel);
                    return Redirect(url);
                }

                // Xóa giỏ hàng sau khi thanh toán thành công
                HttpContext.Session.Remove("GIO_HANG");

                TempData["Message"] = "Đặt hàng thành công!";
            }
            return RedirectToAction("Index");
        }



        //[Authorize]
        public IActionResult PaymentSuccess()
        {
            return View("Success");
        }
        //[Authorize]
        public IActionResult PaymentFail()
        {
            return View();
        }

        //[Authorize]
        public IActionResult PaymentCallBack()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["Message"] = $"Lỗi thanh toán VN Pay: {response.VnPayResponseCode}";
                return RedirectToAction("PaymentFail");
            }


            HttpContext.Session.Remove("GIO_HANG");
            // Lưu đơn hàng vô database

            TempData["Message"] = $"Thanh toán VNPay thành công";
            return RedirectToAction("PaymentSuccess");

        }
    }
}
