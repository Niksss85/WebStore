﻿namespace WebStore.Domain
{
    public class ProductFilter
    {
        public int? SectionId { get; init; }

        public int? BrandId { get; init; }

        public int[] ids { get; set; }
    }
}
