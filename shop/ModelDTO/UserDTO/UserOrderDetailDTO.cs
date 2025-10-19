namespace shop.ModelDTO.UserDTO
{
    public class UserOrderDetailDTO
    {
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double TotalPrice => Quantity * Price;
        public string ProductImage { get; set; }
    }
}
