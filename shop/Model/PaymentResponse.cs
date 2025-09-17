namespace shop.Model
{
    public class PaymentResponse
    {
        public bool Success { get; set; }
        public string InterntId { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
