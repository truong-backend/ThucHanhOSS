using Version2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Version2.Models;
using Newtonsoft.Json;
using X.PagedList.Extensions;
using X.PagedList;

namespace Version2.Controllers
{
    public class HomeController : Controller
    {
        private const string AdminUsername = "admin";
        private const string AdminPassword = "Admin@1234";
        private readonly HeThongBanSachContext _context;

        public HomeController(HeThongBanSachContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? page)
        {
            int pageSize = 8; // Số sách hiển thị trên mỗi trang
            int pageNumber = page ?? 1; // Trang hiện tại, mặc định là 1

            // Lấy danh sách sách từ database, bao gồm liên kết với nhà xuất bản
            var sachList = _context.Saches
                .Include(s => s.IdnhaXuatBanNavigation) // Liên kết với nhà xuất bản
                .OrderBy(s => s.Idsach) // Sắp xếp theo Idsach
                .ToPagedList(pageNumber, pageSize); // Áp dụng phân trang

            // Truyền dữ liệu sang View
            ViewBag.Sach = sachList;

            // Trả về view
            return View(sachList);
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Taikhoans
                                  .Include(t => t.IdkhachHangNavigation) // Load thông tin Khachhang
                                  .FirstOrDefaultAsync(t => t.TenDangNhap == model.TenDangNhap && t.MatKhau == model.MatKhau);


                if (user != null)
                {
                    Khachhang k = user.IdkhachHangNavigation;
                    string json = JsonConvert.SerializeObject(k, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore // Bỏ qua vòng lặp
                    });
                    HttpContext.Session.SetString("Khachhang", json);


                    return RedirectToAction("Index", "Home");
                }
                else if ((model.TenDangNhap == AdminUsername && model.MatKhau == AdminPassword))
                {

                    return RedirectToAction("Index", "Danhmucs");
                }
                else
                {

                    ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }
            return View();
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("TenDangNhap,MatKhau,TenKhachHang,NgaySinh,DiaChi,SoDienThoai,NgayDangKy")] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem khách hàng đã tồn tại hay chưa
                var existingCustomer = await _context.Khachhangs
                    .FirstOrDefaultAsync(kh => kh.TenKhachHang == model.TenKhachHang);

                Khachhang customer;

                // Nếu khách hàng chưa tồn tại, tạo mới
                if (existingCustomer == null)
                {
                    customer = new Khachhang
                    {
                        TenKhachHang = model.TenKhachHang,
                        NgaySinh = model.NgaySinh,
                        DiaChi = model.DiaChi,
                        SoDienThoai = model.SoDienThoai,
                        NgayDangKy = DateTime.Now
                    };
                    _context.Khachhangs.Add(customer);
                    await _context.SaveChangesAsync(); // Lưu khách hàng mới
                }
                else
                {
                    customer = existingCustomer; // Sử dụng khách hàng đã tồn tại
                }

                // Tạo tài khoản cho khách hàng
                var account = new Taikhoan
                {
                    IdkhachHang = customer.IdkhachHang,
                    TenDangNhap = model.TenDangNhap,
                    MatKhau = model.MatKhau, // Mã hóa mật khẩu nếu cần
                    NgayTaoTaiKhoan = DateTime.Now
                };
                _context.Taikhoans.Add(account);
                await _context.SaveChangesAsync(); // Lưu tài khoản mới

                return RedirectToAction("Index", "Home"); // Chuyển hướng đến trang chủ hoặc trang khác sau khi đăng ký thành công
            }

            return View(model);
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Khachhang");
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }


    }
}
