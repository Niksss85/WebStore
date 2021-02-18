using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities
{
    public class Brand : NamedEntity, IOrderedEntity
    {
       // [Table("Brands")]
       // [Column("BrandOrder")]
        public int Order { get; set; }
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();// - устанавливаем связь между таблицами 

    }
}