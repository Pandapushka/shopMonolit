using System.ComponentModel.DataAnnotations.Schema;

namespace shop.Model.Entitys.Cart
{
    public class ShoppingCart : BaseEntity
    {
        public string UserID { get; set; } = string.Empty;
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
        [NotMapped]
        public double TotalAmount { get; set; }

    }
}
