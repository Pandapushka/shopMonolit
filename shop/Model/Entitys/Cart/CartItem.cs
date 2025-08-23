using System.ComponentModel.DataAnnotations.Schema;

namespace shop.Model.Entitys.Cart
{
    public class CartItem : BaseEntity
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int ShoppingCartId { get; set; }


        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [ForeignKey("ShoppingCartId")]
        public virtual ShoppingCart ShoppingCart { get; set; }

        
    }
}
