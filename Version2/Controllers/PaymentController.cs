using Microsoft.AspNetCore.Mvc;
using Version2.Services;
using Version2.ViewModels;

namespace Version2.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IVnPayService _vnPayService;
        public PaymentController(IVnPayService vnPayService)
        {

            _vnPayService = vnPayService;
        }
        [HttpPost]
        public IActionResult CreatePaymentUrl(VnPaymentRequestModel model)
        {
            var url = _vnPayService.CreatePaymentUrl(HttpContext, model);

            return Redirect(url);
        }
        

    }
}
