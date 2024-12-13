namespace Version2.ViewModels
{
    public class CartItem
    {
        public int IdSach { get; set; }
        public string TenSach { get; set; }
        public decimal Gia { get; set; }
        public int SoLuong { get; set; }
        public string? HinhAnh { get; set; }
        public decimal ThanhTien => Gia * SoLuong;
    }
}
   