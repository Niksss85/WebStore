﻿using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities
{
    public class Section : NamedEntity, IOrderedEntity
    {
        public int Order { get; set; }

        public int? ParentId { get; set; }
        [ForeignKey(nameof(ParentId))]
        public virtual Section Parent { get; set; }//свойство зависимости
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();// - устанавливаем связь между таблицами 
    }
}
