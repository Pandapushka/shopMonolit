namespace shop.ModelDTO.UserDTO
{
    public class UserOrderDTO
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
        public double OrderTotalAmount { get; set; }
        public DateTime OrderDateTime { get; set; }
        public string Status { get; set; }
        public int TotalCount { get; set; }
        public List<UserOrderDetailDTO> OrderDetails { get; set; }
    }
}
