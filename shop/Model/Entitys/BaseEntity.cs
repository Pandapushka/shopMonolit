using System.ComponentModel.DataAnnotations;

namespace shop.Model.Entitys
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
